using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Nova.SmartLCT.Interface;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Specialized;
using System.ComponentModel;
using GuiLabs.Undo;
using GalaSoft.MvvmLight;
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using Nova.LCT.GigabitSystem.Common;
using System.Windows.Resources;
using CommonAdorner;

namespace Nova.SmartLCT.SmartLCTControl
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:SmartLCTControl"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:SmartLCTControl;assembly=SmartLCTControl"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    /// 

    [StyleTypedProperty(Property = "MyChildStyle", StyleTargetType = typeof(MyRectangleControl))]
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class SmartLCTControl : Control
    {
        #region 定义xmal属性
        /// <summary>
        /// 控件的MyRectLayerProperty属性值
        /// </summary>
        public static readonly DependencyProperty MyRectLayerProperty =
            DependencyProperty.Register("MyRectLayer", typeof(RectLayer), typeof(SmartLCTControl), new FrameworkPropertyMetadata(new RectLayer(), MyRectLayerPropertyChangedCallBack));
        public static readonly DependencyProperty MyChildStyleProperty =
    DependencyProperty.Register("MyChildStyle", typeof(Style), typeof(SmartLCTControl), new FrameworkPropertyMetadata(null, MyRectLayerPropertyChangedCallBack));
        /// <summary>
        /// 控件的SmartLCTActionManagerProperty属性值
        /// </summary>
        public static readonly DependencyProperty SmartLCTActionManagerProperty =
            DependencyProperty.Register("SmartLCTActionManager", typeof(ActionManager), typeof(SmartLCTControl), new FrameworkPropertyMetadata(new ActionManager()));
        /// <summary>
        /// 控件的CurrentRectElementProperty属性值
        /// </summary>
        public static readonly DependencyProperty CurrentRectElementProperty =
            DependencyProperty.Register("CurrentRectElement", typeof(RectElement), typeof(SmartLCTControl), new FrameworkPropertyMetadata(new RectElement()));

        public static readonly DependencyProperty SelectedLayerAndElementValueProperty =
    DependencyProperty.Register("SelectedLayerAndElementValue", typeof(SelectedLayerAndElement), typeof(SmartLCTControl), new FrameworkPropertyMetadata(new SelectedLayerAndElement()));

        public static RoutedCommand IncreaseCommand
        {
            get
            {
                return _increaseCommand;
            }
        }
        private static RoutedCommand _increaseCommand;
        public static RoutedCommand DecreaseCommand
        {
            get
            {
                return _decreaseCommand;
            }
        }
        private static RoutedCommand _decreaseCommand;
        public static RoutedCommand OrginalSizeCommand
        {
            get { return _orginalSizeCommand; }
        }
        private static RoutedCommand _orginalSizeCommand;

        public static RoutedCommand ReDoCommand
        {
            get
            {
                return _reDoCommand;
            }
        }
        private static RoutedCommand _reDoCommand;
        public static RoutedCommand UnDoCommand
        {
            get
            {
                return _unDoCommand;
            }
        }
        private static RoutedCommand _unDoCommand;

        public static RoutedCommand CutCommand
        {
            get { return _cutCommand; }
        }
        private static RoutedCommand _cutCommand;
        public static RoutedCommand CopyCommand
        {
            get { return _copyCommand; }
        }
        private static RoutedCommand _copyCommand;
        public static RoutedCommand PasteCommand
        {
            get
            {
                return _pasteCommand;
            }
        }
        private static RoutedCommand _pasteCommand;
        public static RoutedCommand ClearCommand
        {
            get
            {
                return _clearCommand;
            }
        }
        private static RoutedCommand _clearCommand;
        public static RoutedCommand DeleteCommand
        {
            get { return _deleteCommand; }
        }
        private static RoutedCommand _deleteCommand;
        public static RoutedCommand SelecteAllCommand
        {
            get { return _selectAllCommand; }
        }
        private static RoutedCommand _selectAllCommand;
        public static RoutedCommand ClearLineCommand
        {
            get { return _clearLineCommand; }
        }
        private static RoutedCommand _clearLineCommand; 
        public static RoutedCommand CancelSelectedAllCommand
        {
            get { return _cancelSelectedAllCommand; }
        }
        private static RoutedCommand _cancelSelectedAllCommand;

        public static RoutedCommand MouseLeftButtonDownWithReceiveCmd
        {
            get { return _mouseLeftButtonDownWithReceiveCmd; }
        }
        private static RoutedCommand _mouseLeftButtonDownWithReceiveCmd;
        public static RoutedCommand MouseRightButtonDownWithReceiveCmd
        {
            get { return _mouseRightButtonDownWithReceiveCmd; }
        }
        private static RoutedCommand _mouseRightButtonDownWithReceiveCmd;
        public static RoutedCommand MouseUpWithReceiveCmd
        {
            get { return _mouseUpWithReceiveCmd; }
        }
        private static RoutedCommand _mouseUpWithReceiveCmd;
        public static RoutedCommand MouseMoveWithReceiveCmd
        {
            get { return _mouseMoveWithReceiveCmd; }
        }
        private static RoutedCommand _mouseMoveWithReceiveCmd;
        public static RoutedCommand MouseDoubleClickWithReceiveCmd
        {
            get { return _mouseDoubleClickWithReceiveCmd; }
        }
        private static RoutedCommand _mouseDoubleClickWithReceiveCmd;
        public static RoutedCommand MouseWheelWithReceiveCmd
        {
            get { return _mouseWheelWithReceiveCmd; }
        }
        private static RoutedCommand _mouseWheelWithReceiveCmd;
        public static RoutedCommand KeyDownWithReceiveCmd
        {
            get { return _keyDownWithReceiveCmd; }
        }
        private static RoutedCommand _keyDownWithReceiveCmd;

        public static RoutedCommand MouseLeftButtonDownWithLayerCmd
        {
            get { return _mouseLeftButtonDownWithLayerCmd; }
        }
        private static RoutedCommand _mouseLeftButtonDownWithLayerCmd;
        public static RoutedCommand MouseLeftButtonUpWithLayerCmd
        {
            get { return _mouseLeftButtonUpWithLayerCmd; }
        }
        private static RoutedCommand _mouseLeftButtonUpWithLayerCmd;
        public static RoutedCommand MouseMoveWithLayerCmd
        {
            get { return _mouseMoveWithLayerCmd; }
        }
        private static RoutedCommand _mouseMoveWithLayerCmd;
        #endregion

        #region 属性
        public ActionManager SmartLCTActionManager
        {
            get { return (ActionManager)GetValue(SmartLCTActionManagerProperty); }
            set 
            {
                SetValue(SmartLCTActionManagerProperty, value);
            }
        }
        public RectLayer MyRectLayer
        {
            get { return (RectLayer)GetValue(MyRectLayerProperty); }
            set 
            {
                if (MyRectLayer == null)
                {
                    return;
                }
                else
                {
                    MyRectLayer.PropertyChanged -= new PropertyChangedEventHandler(OnMyRectLayerChanged);
                }
                SetValue(MyRectLayerProperty, value);
                if (MyRectLayer != null)
                {
                    MyRectLayer.PropertyChanged += new PropertyChangedEventHandler(OnMyRectLayerChanged);
                }
            }
        }
        public Style MyChildStyle
        {
            get
            {
                return (Style)GetValue(MyChildStyleProperty);
            }
            set
            {
                SetValue(MyChildStyleProperty, value);
            }
        }
        public RectElement CurrentRectElement
        {
            get { return (RectElement)GetValue(CurrentRectElementProperty); }
            set { SetValue(CurrentRectElementProperty, value); }
        }
        public SelectedLayerAndElement SelectedLayerAndElementValue
        {
            get { return (SelectedLayerAndElement)GetValue(SelectedLayerAndElementValueProperty); }
            set { SetValue(SelectedLayerAndElementValueProperty, value); }
        }
        #endregion

        #region 命令
        public RelayCommand CmdCut
        {
            get;
            private set;
        }
        public RelayCommand CmdCopy
        {
            get;
            private set;
        }
        public RelayCommand CmdPaste
        {
            get;
            private set;
        }
        public RelayCommand CmdDelete
        {
            get;
            private set;
        }
        public RelayCommand CmdSelectedAll
        {
            get;
            private set;
        }
        public RelayCommand CmdClear
        {
            get;
            private set;
        }
        public RelayCommand CmdClearLine
        {
            get;
            private set;
        }
        public RelayCommand CmdCancelSelectedAll
        {
            get;
            private set;
        }
        public RelayCommand CmdCancelGroup
        {
            get;
            private set;
        }
        public RelayCommand CmdCreateGroup
        {
            get;
            private set;
        }
        public RelayCommand CmdChangeReceiveSize
        {
            get;
            private set;
        }
        #endregion

        #region 字段    
        private Cursor _moveCursor = null;
        private ItemsControl _itemsControl = null;
        public ScrollViewer _scrollViewer = null;
        private  Viewbox _viewbox = null;

        private Point _mouseLeftDownPointInThis = new Point();
        private Point _copyPoint = new Point();
        private Point _mousePoint = new Point();
        private List<Key> _keyInfo = new List<Key>();

        private bool _actionIsExcuting = false;
        private ContextMenu _contextMenu = new ContextMenu();
        private ContextMenu _workAreaContextMenu = new ContextMenu();

        private bool _isMouseLeftButtonDown = false;
        private bool _isLayerMouseLeftButtonDown = false;
        private RectLayer _myScreenLayer = new RectLayer();
        private AddReceiveInfo _addReceiveInfo = null;
        private bool _isFrameSelected = false;

        private bool _isStartMoveX = false;
        private bool _isStartMoveY = false;
        private Dictionary<int, IRectElement> _groupframeList = new Dictionary<int, IRectElement>();
        private NotificationMessageAction<AddReceiveInfo> _addReceiveCallback = null;
        private ObservableCollection<ElementSizeInfo> _elementSizeInfo = new ObservableCollection<ElementSizeInfo>();
        private Rect _selectedElementRect = new Rect();
        private Dictionary<int, SelectedInfo> _currentSelectedElement = new Dictionary<int, SelectedInfo>();
        private ObservableCollection<IRectElement> _selectedElementCollection = new ObservableCollection<IRectElement>();
        private ObservableCollection<IRectElement> _copyElementCollection = new ObservableCollection<IRectElement>();
        private ObservableCollection<ElementMoveInfo> _elementMoveInfo = new ObservableCollection<ElementMoveInfo>();
        private bool _isMoving = false;
        private RectLayer _selectedLayer = new RectLayer();
        private bool _isLayerMoving = false;

        /// <summary>
        /// 拖动的区域
        /// </summary>
        private FrameworkElement _dragScope;
        /// <summary>
        /// 用于显示鼠标跟随效果的装饰器
        /// </summary>
        private DragAdorner _adorner;
        //显示位置的装饰器
        private DragAdorner _locationAdorner;
        /// <summary>
        /// 用于呈现DragAdorner的图画
        /// </summary>
        private AdornerLayer _layer;

        private MouseEventHandler draghandler;
        private Point _virtualMoveDifValue = new Point();
        private Rect _selectedReceiveArea = new Rect();
        private bool _isScrollVertial = false;
        private bool _isScrollHorizon = false;
        #endregion

        #region 构造函数
        static SmartLCTControl()
        {
            InitializeCommands();

           // EventManager.RegisterClassHandler(typeof(Rectangle),
           //Mouse, new MouseButtonEventHandler(Rectangle.OnMouseLeftButtonDown), true);


            DefaultStyleKeyProperty.OverrideMetadata(typeof(SmartLCTControl), new FrameworkPropertyMetadata(typeof(SmartLCTControl)));
        }

        public SmartLCTControl()
        {
            this.Loaded+=new RoutedEventHandler(SmartLCTControl_Loaded);
           
        }
        #endregion

        #region 内部方法
        private void InitContextMenu()
        {
            if (_myScreenLayer == null)
            {
                return;
            }
            if (_myScreenLayer.OperateEnviron != OperatEnvironment.DesignScreen)
            {
                return;
            }
            string msg = "";
            #region 右键菜单（工作区）      
            _workAreaContextMenu.Foreground = Brushes.White;
            _workAreaContextMenu.BorderThickness = new Thickness(1, 1, 1, 1);
            _workAreaContextMenu.BorderBrush = Brushes.LightGray;

            MenuItem wSelectedAllItem = new MenuItem();
            MenuItem wClearItem = new MenuItem();
            MenuItem wPasteItem = new MenuItem();
            CommonStaticMethod.GetLanguageString("粘贴", "Lang_SmartLCT_Paste", out msg);
            wPasteItem.Header = msg; 
            CommonStaticMethod.GetLanguageString("全选", "Lang_SmartLCT_SelectedAll", out msg);
            wSelectedAllItem.Header = msg;
            CommonStaticMethod.GetLanguageString("清除", "Lang_SmartLCT_Clear", out msg);
            wClearItem.Header = msg;
            _workAreaContextMenu.Items.Clear();
            if (!_workAreaContextMenu.Items.Contains(wPasteItem))
                _workAreaContextMenu.Items.Add(wPasteItem);
            if (!_workAreaContextMenu.Items.Contains(wSelectedAllItem))
                _workAreaContextMenu.Items.Add(wSelectedAllItem);
            if (!_workAreaContextMenu.Items.Contains(wClearItem))
                _workAreaContextMenu.Items.Add(wClearItem);

            #endregion
            #region 右键菜单(箱体)
            _contextMenu.Foreground = Brushes.White;
            _contextMenu.BorderThickness = new Thickness(1,1,1,1);
            _contextMenu.BorderBrush = Brushes.LightGray;
            
            MenuItem copyItem = new MenuItem();
            MenuItem pasteItem = new MenuItem();
            MenuItem deleteItem = new MenuItem();
            MenuItem selectedAllRectItem = new MenuItem();
            MenuItem clearItem = new MenuItem();
            MenuItem cutItem = new MenuItem();
            MenuItem clearLineItem = new MenuItem();
            MenuItem cancelSelectedAllItem = new MenuItem();
            MenuItem cancelGroupItem = new MenuItem();
            MenuItem createGroupItem = new MenuItem();
            MenuItem changeReceiveSizeItem = new MenuItem();


            CommonStaticMethod.GetLanguageString("编辑大小", "Lang_SmartLCT_MainWin_EditSize", out msg);
            changeReceiveSizeItem.Header = msg;
            CommonStaticMethod.GetLanguageString("取消分组", "Lang_SmartLCT_MainWin_CancelCombine", out msg);
            cancelGroupItem.Header = msg;
            CommonStaticMethod.GetLanguageString("组合", "Lang_SmartLCT_MainWin_Combine", out msg);
            createGroupItem.Header = msg;
            CommonStaticMethod.GetLanguageString("清除连线", "Lang_SmartLCT_ClearLine", out msg);
            clearLineItem.Header = msg;
            CommonStaticMethod.GetLanguageString("剪切", "Lang_SmartLCT_Cut", out msg);
            cutItem.Header = msg;
            CommonStaticMethod.GetLanguageString("复制", "Lang_SmartLCT_Copy", out msg);
            copyItem.Header = msg;
            CommonStaticMethod.GetLanguageString("粘贴", "Lang_SmartLCT_Paste", out msg);
            pasteItem.Header = msg;
            CommonStaticMethod.GetLanguageString("删除", "Lang_SmartLCT_Delete", out msg);
            deleteItem.Header = msg;
            CommonStaticMethod.GetLanguageString("全选", "Lang_SmartLCT_SelectedAll", out msg);
            selectedAllRectItem.Header = msg;
            CommonStaticMethod.GetLanguageString("清除", "Lang_SmartLCT_Clear", out msg);
            clearItem.Header = msg;
            CommonStaticMethod.GetLanguageString("取消选择", "Lang_SmartLCT_CancelSelectedAll", out msg);
            cancelSelectedAllItem.Header = msg;
            _contextMenu.Items.Clear();
            if(!_contextMenu.Items.Contains(cutItem))
            _contextMenu.Items.Add(cutItem);
            if (!_contextMenu.Items.Contains(copyItem))
            _contextMenu.Items.Add(copyItem);
            if (!_contextMenu.Items.Contains(pasteItem))
                _contextMenu.Items.Add(pasteItem);
            if (!_contextMenu.Items.Contains(deleteItem))
                _contextMenu.Items.Add(deleteItem);
            if (!_contextMenu.Items.Contains(selectedAllRectItem))
                _contextMenu.Items.Add(selectedAllRectItem);
            if (!_contextMenu.Items.Contains(cancelSelectedAllItem))
                _contextMenu.Items.Add(cancelSelectedAllItem);
            if (!_contextMenu.Items.Contains(clearItem))
                _contextMenu.Items.Add(clearItem);
            if (!_contextMenu.Items.Contains(clearLineItem))
                _contextMenu.Items.Add(clearLineItem);
            if (!_contextMenu.Items.Contains(cancelGroupItem))
                _contextMenu.Items.Add(cancelGroupItem);
            if (!_contextMenu.Items.Contains(createGroupItem))
                _contextMenu.Items.Add(createGroupItem);
            if (!_contextMenu.Items.Contains(changeReceiveSizeItem))
                _contextMenu.Items.Add(changeReceiveSizeItem);

            CmdChangeReceiveSize = new RelayCommand(OnCmdChangeReceiveSize, CanChangeReceiveSize);
            CmdCreateGroup = new RelayCommand(OnCmdCreateGroup,CanCreateGroup);
            CmdCancelGroup = new RelayCommand(OnCmdCancelGroup,CanCancelGroup);
            CmdClearLine = new RelayCommand(OnCmdClearLine,CanClearLineExecute);
            CmdCopy = new RelayCommand(OnCmdCopy, CanCopyExecute);
            CmdPaste = new RelayCommand(OnCmdPaste, CanPasteExecute);
            CmdDelete = new RelayCommand(OnCmdDelete,CanDeleteExecute);
            CmdSelectedAll = new RelayCommand(OnCmdSelectedAll, CanSelectedAllExecute);
            CmdClear = new RelayCommand(OnCmdClear,CanClearExecute);
            CmdCut = new RelayCommand(OnCmdCut, CanCutExecute);
            CmdCancelSelectedAll = new RelayCommand(OnCancelSelectedAll, CanSelectedAllExecute);
            cutItem.Command = CmdCut;
            copyItem.Command = CmdCopy;
            pasteItem.Command = CmdPaste;
            deleteItem.Command = CmdDelete;
            selectedAllRectItem.Command = CmdSelectedAll;
            clearItem.Command = CmdClear;
            clearLineItem.Command = CmdClearLine;
            cancelSelectedAllItem.Command = CmdCancelSelectedAll;
            cancelGroupItem.Command = CmdCancelGroup;
            createGroupItem.Command = CmdCreateGroup;
            changeReceiveSizeItem.Command = CmdChangeReceiveSize;
            wSelectedAllItem.Command = CmdSelectedAll;
            wClearItem.Command = CmdClear;
            wPasteItem.Command = CmdPaste;
            #endregion
        }
        private static void InitializeCommands()
        {
            _increaseCommand = new RoutedCommand("IncreaseCommand", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_increaseCommand, OnIncreaseCommand, OnCanExecIncreaseCommand));

            _decreaseCommand = new RoutedCommand("DecreaseCommand", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_decreaseCommand, OnDecreaseCommand, OnCanExecDecreaseCommand));

            _orginalSizeCommand = new RoutedCommand("OrginalSizeCommand", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_orginalSizeCommand, OnOrginalSizeCommand,OnCanExecOrignalSizeCommand));

            _cancelSelectedAllCommand = new RoutedCommand("CancelSelectedAllCommand", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_cancelSelectedAllCommand, OnCancelSelectedAllCommand, OnCanExecSelectedAllCommand));

            _pasteCommand = new RoutedCommand("PasteCommand", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_pasteCommand, OnPasteCommand, OnCanExecPasteCommand));
           
            _clearCommand = new RoutedCommand("ClearCommand", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_clearCommand, OnClearCommand,OnCanExecClearCommand));
         
            _unDoCommand = new RoutedCommand("UnDoCommand", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_unDoCommand, OnUnDoCommand,OnCanExecUnDoCommand));

            _reDoCommand = new RoutedCommand("ReDoCommand", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_reDoCommand, OnReDoCommand, OnCanExecReDoCommand));

            _cutCommand = new RoutedCommand("CutCommand", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_cutCommand, OnCutCommand, OnCanExecCutCommand));

            _copyCommand = new RoutedCommand("CopyCommand", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_copyCommand, OnCopyCommand, OnCanExecCopyCommand));

            _deleteCommand = new RoutedCommand("DeleteCommand", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_deleteCommand, OnDeleteCommand, OnCanExecDeleteCommand));
           
            _selectAllCommand = new RoutedCommand("SelectAllCommand", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_selectAllCommand, OnSelectAllCommand, OnCanExecSelectedAllCommand));
           
            _clearLineCommand = new RoutedCommand("ClearLineCommand", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_clearLineCommand, OnClearLineCommand, OnCanExecClearLineCommand));

            _mouseMoveWithReceiveCmd = new RoutedCommand("MouseMoveWithReceiveCmd", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(MouseMoveWithReceiveCmd, OnMouseMoveWithReceiveCommand));
           
            _mouseUpWithReceiveCmd = new RoutedCommand("MouseUpWithReceiveCmd", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_mouseUpWithReceiveCmd, OnMouseUpWithReceiveCommand));

            _mouseRightButtonDownWithReceiveCmd = new RoutedCommand("MouseRightButtonDownWithReceiveCmd", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_mouseRightButtonDownWithReceiveCmd, OnMouseRightButtonDownWithReceiveCommand));

            _mouseLeftButtonDownWithReceiveCmd = new RoutedCommand("MouseLeftButtonDownWithReceiveCmd", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_mouseLeftButtonDownWithReceiveCmd, OnMouseLeftButtonDownWithReceiveCommand));

            _mouseDoubleClickWithReceiveCmd = new RoutedCommand("MouseDoubleClickWithReceiveCmd", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_mouseDoubleClickWithReceiveCmd, OnMouseDoubleClickWithReceiveCommand));

            _mouseWheelWithReceiveCmd = new RoutedCommand("MouseWheelWithReceiveCmd", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_mouseWheelWithReceiveCmd, OnMouseWheelWithReceiveCommand));

            _keyDownWithReceiveCmd = new RoutedCommand("KeyDownWithReceiveCmd", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_keyDownWithReceiveCmd, OnKeyDownWithReceiveCommand));

            _mouseLeftButtonDownWithLayerCmd = new RoutedCommand("MouseLeftButtonDownWithLayerCmd", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_mouseLeftButtonDownWithLayerCmd, OnMouseLeftButtonDownWithLayerCommand));

            _mouseLeftButtonUpWithLayerCmd = new RoutedCommand("MouseLeftButtonUpWithLayerCmd", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_mouseLeftButtonUpWithLayerCmd, OnMouseLeftButtonUpWithLayerCommand));

            _mouseMoveWithLayerCmd = new RoutedCommand("MouseMoveWithLayerCmd", typeof(SmartLCTControl));
            CommandManager.RegisterClassCommandBinding(typeof(SmartLCTControl), new CommandBinding(_mouseMoveWithLayerCmd, OnMouseMoveWithLayerCommand));

        }

        private bool CanChangeReceiveSize()
        {
            if (_selectedElementCollection != null && _selectedElementCollection.Count == 1)
            {
                return true;
            }
            return false;
        }
        private void OnCmdChangeReceiveSize()
        {
            CustomReceiveResult info = new CustomReceiveResult();
            info.Height = (int)_selectedElementCollection[0].Height;
            info.Width = (int)_selectedElementCollection[0].Width;
            NotificationMessageAction<CustomReceiveResult> nsa =
            new NotificationMessageAction<CustomReceiveResult>(this, info, MsgToken.MSG_SHOWSETCUSTOMRECEIVESIZE, SetChangeReceiveCallBack);
            Messenger.Default.Send(nsa, MsgToken.MSG_SHOWSETCUSTOMRECEIVESIZE);
        }
        private void SetChangeReceiveCallBack(CustomReceiveResult info)
        {
            if (info.IsOK==true)
            {
                #region 记录相关
                ObservableCollection<ElementSizeInfo> elementSizeCollection = new ObservableCollection<ElementSizeInfo>();
                ObservableCollection<ElementMoveInfo> elementMoveCollection = new ObservableCollection<ElementMoveInfo>();
                ElementSizeInfo elementSizeinfo = new ElementSizeInfo();
                elementSizeinfo.Element = _selectedElementCollection[0];
                Size oldSize = new Size(_selectedElementCollection[0].Width, _selectedElementCollection[0].Height);
                elementSizeinfo.OldSize = oldSize;
                #endregion

                for (int i = 0; i < _selectedElementCollection.Count; i++)
                {
                    _selectedElementCollection[i].Height = info.Height;
                    _selectedElementCollection[i].Width = info.Width;
                }
                IRectElement rectElement = _selectedElementCollection[0];
                //组框大小等于选中的和没有选中的
                if (rectElement.GroupName != -1)
                {
                    Rect noSelectedElementRect = _currentSelectedElement[rectElement.GroupName].NoSelectedElementRect;
                    Rect SelectedElementRect = new Rect(rectElement.X, rectElement.Y, rectElement.Width, rectElement.Height);
                    Rect groupSize = new Rect();
                    if (noSelectedElementRect.Height == 0 && noSelectedElementRect.Width == 0)
                    {
                        groupSize = SelectedElementRect;
                    }
                    else
                    {
                        groupSize = Rect.Union(SelectedElementRect, noSelectedElementRect);
                    }
                    #region 记录相关
                    ElementMoveInfo groupMoveInfo = new ElementMoveInfo();
                    groupMoveInfo.Element = _groupframeList[rectElement.GroupName];
                    groupMoveInfo.OldPoint = new Point(_groupframeList[rectElement.GroupName].X, _groupframeList[rectElement.GroupName].Y);
                    ElementSizeInfo groupSizeinfo = new ElementSizeInfo();
                    groupSizeinfo.Element = _groupframeList[rectElement.GroupName];
                    groupSizeinfo.OldSize = new Size(_groupframeList[rectElement.GroupName].Width, _groupframeList[rectElement.GroupName].Height);
                    #endregion
                    _groupframeList[rectElement.GroupName].X = groupSize.X;
                    _groupframeList[rectElement.GroupName].Y = groupSize.Y;
                    _groupframeList[rectElement.GroupName].Width = groupSize.Width;
                    _groupframeList[rectElement.GroupName].Height = groupSize.Height;

                    #region 记录相关
                    groupSizeinfo.NewSize = new Size(_groupframeList[rectElement.GroupName].Width, _groupframeList[rectElement.GroupName].Height);
                    groupMoveInfo.NewPoint = new Point(_groupframeList[rectElement.GroupName].X, _groupframeList[rectElement.GroupName].Y);
                    elementMoveCollection.Add(groupMoveInfo);
                    elementSizeCollection.Add(groupSizeinfo);
                    #endregion
                    GetCurrentElementInfo();
                }
                elementSizeinfo.NewSize = new Size(elementSizeinfo.Element.Width, elementSizeinfo.Element.Height);
                elementSizeCollection.Add(elementSizeinfo);
                ElementSizeAction elementSizeAction = new ElementSizeAction(elementSizeCollection);
                ElementMoveAction elementMoveAction = new ElementMoveAction(elementMoveCollection);
                using (Transaction.Create(SmartLCTActionManager))
                {
                    SmartLCTActionManager.RecordAction(elementSizeAction);
                    SmartLCTActionManager.RecordAction(elementMoveAction);
                }

            }     
        }

        private bool CanCreateGroup()
        {
            int num = 0;
            for (int i = 0; i < _selectedElementCollection.Count; i++)
            {
                if (_selectedElementCollection[i].GroupName != -1)
                {
                    return false;
                }
                else
                {
                    num += 1;
                }
            }
            if (num < 2)
            {
                return false;
            }
            return true;
        }
        private void OnCmdCreateGroup()
        {
            ObservableCollection<IElement> addCollection = new ObservableCollection<IElement>();
            ObservableCollection<GroupInfo> groupCollection = new ObservableCollection<GroupInfo>();
            RectElement groupframe = new RectElement();
            groupframe.ElementSelectedState = SelectedState.Selected;
            groupframe.EleType = ElementType.groupframe;
            groupframe.ZIndex = 2;
            groupframe.GroupName = _myScreenLayer.MaxGroupName + 1;
            _myScreenLayer.MaxGroupName += 1;
            groupframe.ZOrder = _myScreenLayer.MaxZorder + 1;
            _myScreenLayer.MaxZorder += 1;
            groupframe.X = _currentSelectedElement[-1].SelectedElementRect.X;
            groupframe.Y = _currentSelectedElement[-1].SelectedElementRect.Y;
            groupframe.Width=_currentSelectedElement[-1].SelectedElementRect.Width;
            groupframe.Height = _currentSelectedElement[-1].SelectedElementRect.Height;
            groupframe.ParentElement = _myScreenLayer;
            for (int i = 0; i < _currentSelectedElement[-1].SelectedGroupElementList.Count; i++)
            {
                RectElement rect = (RectElement)(RectElement)_currentSelectedElement[-1].SelectedGroupElementList[i];
                rect.GroupName = groupframe.GroupName;
                GroupInfo groupInfo = new GroupInfo(rect, new OldAndNewType(-1, rect.GroupName));
                groupCollection.Add(groupInfo);
            }
            _groupframeList.Add(groupframe.GroupName, groupframe);
            _myScreenLayer.ElementCollection.Add(groupframe);
            addCollection.Add(groupframe);
            foreach (int key in _currentSelectedElement.Keys)
            {
                if (key == -1 && _currentSelectedElement[key].SelectedGroupElementList.Count==0 && _currentSelectedElement[key].NoSelectedGroupElementList.Count==0)
                {
                    _groupframeList.Remove(key);
                    break;
                }
            }
            GetCurrentElementInfo();
            AddAction addAction = new AddAction(_myScreenLayer,addCollection);
            CreatOrCancelGroupAction groupAction = new CreatOrCancelGroupAction(groupCollection);
            using (Transaction.Create(SmartLCTActionManager))
            {
                SmartLCTActionManager.RecordAction(addAction);
                SmartLCTActionManager.RecordAction(groupAction);
            }
        }
        private bool CanCancelGroup()
        {
            if (_currentSelectedElement.Count == 0)
            {
                return false;
            }
            foreach (int key in _currentSelectedElement.Keys)
            {
                if (key == -1)
                {
                    if (_currentSelectedElement[key].SelectedGroupElementList.Count != 0)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_currentSelectedElement[key].NoSelectedGroupElementList.Count != 0 && 
                        _currentSelectedElement[key].SelectedGroupElementList.Count!=0)
                    {
                        return false;
                    }
                    if (_currentSelectedElement[key].NoSelectedGroupElementList.Count==0 
                        && _currentSelectedElement[key].SelectedGroupElementList.Count < 2)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private void OnCmdCancelGroup()
        {
            ObservableCollection<IElement> deleteCollection = new ObservableCollection<IElement>();
            ObservableCollection<GroupInfo> groupInfoCollection = new ObservableCollection<GroupInfo>();
            foreach (int key in _currentSelectedElement.Keys)
            {
                if (_currentSelectedElement[key].SelectedGroupElementList.Count == 0)
                {
                    continue;
                }
                for (int i = 0; i < _currentSelectedElement[key].SelectedGroupElementList.Count; i++)
                {
                    IRectElement rect = _currentSelectedElement[key].SelectedGroupElementList[i];
                    GroupInfo groupInfo = new GroupInfo(rect,new OldAndNewType(rect.GroupName,-1));
                    groupInfoCollection.Add(groupInfo);
                    rect.GroupName = -1;
                }
                deleteCollection.Add(_groupframeList[key]);
                _myScreenLayer.ElementCollection.Remove(_groupframeList[key]);
                _groupframeList.Remove(key);
            }
            if(!_groupframeList.Keys.Contains(-1))
            _groupframeList.Add(-1, null);
            GetCurrentElementInfo();
            DeleteAction deleteAction = new DeleteAction(_myScreenLayer,deleteCollection);
            CreatOrCancelGroupAction groupAction = new CreatOrCancelGroupAction(groupInfoCollection);
            using (Transaction.Create(SmartLCTActionManager))
            {
                SmartLCTActionManager.RecordAction(deleteAction);
                SmartLCTActionManager.RecordAction(groupAction);
            }
        }

        private static void OnMouseMoveWithLayerCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.OnMouseMoveWithLayer(e.Parameter);
            }
        }
        private void OnMouseMoveWithLayer(object para)
        {
            ControlTriggerData data = (ControlTriggerData)para;
            object sender = data.Sender;
            MouseEventArgs e = (MouseEventArgs)data.EventArges;
            IElement element = (IElement)data.Parameter;
            Layer_MouseMove(sender, e, element);
        }

        private static void OnMouseLeftButtonUpWithLayerCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.OnMouseLeftButtonUpWithLayer(e.Parameter);
            }
        }
        private void OnMouseLeftButtonUpWithLayer(object para)
        {
            ControlTriggerData data = (ControlTriggerData)para;
            object sender = data.Sender;
            MouseButtonEventArgs e = (MouseButtonEventArgs)data.EventArges;
            IElement element = (IElement)data.Parameter;
            Layer_MouseLeftButtonUp(sender, e, element);
        }

        private static void OnMouseLeftButtonDownWithLayerCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.OnMouseLeftButtonDownWithLayer(e.Parameter);
            }
        }
        private void OnMouseLeftButtonDownWithLayer(object para)
        {
            ControlTriggerData data = (ControlTriggerData)para;
            object sender = data.Sender;
            MouseButtonEventArgs e = (MouseButtonEventArgs)data.EventArges;
            IElement element = (IElement)data.Parameter;
            Layer_MouseLeftButtonDown(sender, e, element);
        }

        private static void OnMouseDoubleClickWithReceiveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.OnMouseDoubleClickWithReceive(e.Parameter);
            }
        }
        private void OnMouseDoubleClickWithReceive(object para)
        {
            ControlTriggerData data = (ControlTriggerData)para;
            object sender = data.Sender;
            MouseButtonEventArgs e = (MouseButtonEventArgs)data.EventArges;
            IElement element = (IElement)data.Parameter;
            Receive_MouseDoubleClick(sender, e, element);
        }

        private static void OnMouseMoveWithReceiveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.OnMouseMoveWithReceive(e.Parameter);
            }
        }
        private void OnMouseMoveWithReceive(object para)
        {
            ControlTriggerData data=(ControlTriggerData)para;
            object sender = data.Sender;
            MouseEventArgs e = (MouseEventArgs)data.EventArges;
            IElement element = (IElement)data.Parameter;
            Receive_MouseMove(sender, e,element);
        }

        private static void OnMouseUpWithReceiveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.OnMouseUpWithReceive(e.Parameter);
            }
        }
        private void OnMouseUpWithReceive(object para)
        {
            ControlTriggerData data = (ControlTriggerData)para;
            object sender = data.Sender;         
            MouseButtonEventArgs e = (MouseButtonEventArgs)data.EventArges;
            IElement element = (IElement)data.Parameter;
            Receive_MouseUp(sender, e, element);
        }

        private static void OnMouseRightButtonDownWithReceiveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.OnMouseRightButtonDownWithReceive(e.Parameter);
            }
        }
        private void OnMouseRightButtonDownWithReceive(object para)
        {
            ControlTriggerData data = (ControlTriggerData)para;
            object sender = data.Sender;
            MouseButtonEventArgs e = (MouseButtonEventArgs)data.EventArges;
            IElement element = (IElement)data.Parameter;
            Receive_MouseRightButtonDown(sender, e, element);
        }

        private static void OnMouseLeftButtonDownWithReceiveCommand(object sender,ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.OnMouseLeftButtonDownWithReceive(e.Parameter);
            }
        }
        private void OnMouseLeftButtonDownWithReceive(object para)
        {
            ControlTriggerData data = (ControlTriggerData)para;
            object sender = data.Sender;
            MouseButtonEventArgs e = (MouseButtonEventArgs)data.EventArges;
            IElement element = (IElement)data.Parameter;
            Receive_MouseLeftButtonDown(sender, e, element);
        }

        private static void OnMouseWheelWithReceiveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.OnMouseWheelWithReceive(e.Parameter);
            }
        }
        private void OnMouseWheelWithReceive(object para)
        {
            ControlTriggerData data = (ControlTriggerData)para;
            object sender = data.Sender;
            MouseWheelEventArgs e = (MouseWheelEventArgs)data.EventArges;
            IElement element = (IElement)data.Parameter;
            Receive_MouseWheel(sender, e, element);
        }

        private static void OnKeyDownWithReceiveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.OnKeyDownWithReceive(e.Parameter);
            }
        }
        private void OnKeyDownWithReceive(object para)
        {
            ControlTriggerData data = (ControlTriggerData)para;
            object sender = data.Sender;
            KeyEventArgs e = (KeyEventArgs)data.EventArges;
            IElement element = (IElement)data.Parameter;
            Receive_KeyDown(sender, e, element);
        }

        private static void OnCancelSelectedAllCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.OnCancelSelectedAll();
            }
        }
        private void OnCancelSelectedAll()
        {
            if (_myScreenLayer == null)
            {
                return;
            }
            //RectLayer old = new RectLayer();
            //old = (RectLayer)MyRectLayer.Clone();

            for (int i = 0; i < _myScreenLayer.ElementCollection.Count; i++)
            {
                if (_myScreenLayer.ElementCollection[i] is RectElement)
                {
                    _myScreenLayer.ElementCollection[i].ElementSelectedState = SelectedState.None;
                }
                else if (_myScreenLayer.ElementCollection[i] is RectLayer)
                {
                    _myScreenLayer.ElementCollection[i].ElementSelectedState = SelectedState.None;
                    Function.SetElementSelectedState((RectLayer)_myScreenLayer.ElementCollection[i], SelectedState.None);
                }
            }
            GetCurrentElementInfo();
            //RectLayer newValue = (RectLayer)MyRectLayer.Clone();
            //PrePropertyChangedEventArgs actionValue = new PrePropertyChangedEventArgs() { PropertyName = "ElementCollection", OldValue = old, NewValue = newValue };
            //OnRecordActionValueChanged(actionValue);
            SelectedLayerAndElementChanged();  
        }

        private static void OnCanExecClearLineCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            SmartLCTControl panel = (SmartLCTControl)sender;
            e.CanExecute = panel.CanClearLineExecute();
        }
        private bool CanClearLineExecute()
        {
            if (_myScreenLayer == null)
            {
                return false;
            }
            if (_myScreenLayer.OperateEnviron != OperatEnvironment.DesignScreen)
            {
                return false ;
            }

            bool isSelected = true;
            for (int i = 0; i < _selectedElementCollection.Count; i++)
            {
                if (_selectedElementCollection[i].SenderIndex < 0 && _selectedElementCollection[i].PortIndex < 0)
                {
                    isSelected = false;
                }
            }
            if (!isSelected)
            {
                return false;
            }


            for (int i = 0; i < _myScreenLayer.SenderConnectInfoList.Count; i++)
            {
                for (int j = 0; j < _myScreenLayer.SenderConnectInfoList[i].PortConnectInfoList.Count; j++)
                {
                    if(_myScreenLayer.SenderConnectInfoList[i].PortConnectInfoList[j].ConnectLineElementList!=null
                        && _myScreenLayer.SenderConnectInfoList[i].PortConnectInfoList[j].ConnectLineElementList.Count > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private static void OnClearLineCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.OnCmdClearLine();
            }
        }
        private void OnCmdClearLine()
        {
            OnClearLine();
        }
        private void OnClearLine()
        {
            ObservableCollection<PortInfo> selectedPortCollection = new ObservableCollection<PortInfo>();
            for (int i = 0; i < _selectedElementCollection.Count; i++)
            {
                if (_selectedElementCollection[i].SenderIndex < 0 && _selectedElementCollection[i].PortIndex < 0)
                {
                    continue;
                }
                PortInfo portInfo = new PortInfo(_selectedElementCollection[i].SenderIndex, _selectedElementCollection[i].PortIndex);
                if (!selectedPortCollection.Contains(portInfo))
                {
                    selectedPortCollection.Add(portInfo);
                }
            }
           
            ObservableCollection<IElement> deleteCollection = new ObservableCollection<IElement>();
            ObservableCollection<AddLineInfo> addLineCollection = new ObservableCollection<AddLineInfo>();
            ObservableCollection<ConnectIconVisibilityInfo> connectIconVisibleCollection = new ObservableCollection<ConnectIconVisibilityInfo>();
            for (int itemIndex = 0; itemIndex < _myScreenLayer.ElementCollection.Count; itemIndex++)
            {
                if (_myScreenLayer.ElementCollection[itemIndex].EleType != ElementType.receive)
                {
                    continue;
                }
                IRectElement element = (IRectElement)_myScreenLayer.ElementCollection[itemIndex];
                if (element.ConnectedIndex >= 0)
                {
                    bool isExist = false;
                    for (int i = 0; i < selectedPortCollection.Count; i++)
                    {
                        if (selectedPortCollection[i].SenderIndex == element.SenderIndex && selectedPortCollection[i].PortIndex == element.PortIndex)
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        continue;
                    }
                    ConnectIconVisibilityInfo info = new ConnectIconVisibilityInfo();
                    info.Element = element;
                    OldAndNewVisibility oldAndNewMaxmaxVisible = new OldAndNewVisibility();
                    oldAndNewMaxmaxVisible.OldValue = element.MaxConnectIndexVisibile;
                    oldAndNewMaxmaxVisible.NewValue = Visibility.Hidden;
                    OldAndNewVisibility oldAndNewMaxminVisible = new OldAndNewVisibility();
                    oldAndNewMaxminVisible.OldValue = element.MinConnectIndexVisibile;
                    oldAndNewMaxminVisible.NewValue = Visibility.Hidden;
                    info.OldAndNewMaxConnectIndexVisibile = oldAndNewMaxmaxVisible;
                    info.OldAndNewMinConnectIndexVisibile = oldAndNewMaxminVisible;
                    connectIconVisibleCollection.Add(info);

                }
            }
            for (int i = 0; i < selectedPortCollection.Count; i++)
            {
                int senderIndex = selectedPortCollection[i].SenderIndex;
                int portIndex = selectedPortCollection[i].PortIndex;
                
                PortConnectInfo portConnectInfo = _myScreenLayer.SenderConnectInfoList[senderIndex].PortConnectInfoList[portIndex];
                if (portConnectInfo.ConnectLineElementList != null && portConnectInfo.ConnectLineElementList.Count > 0)
                {
                    for (int m = 0; m < portConnectInfo.ConnectLineElementList.Count; m++)
                    {
                        AddLineInfo addLineInfo = new AddLineInfo();
                        addLineInfo.Element = portConnectInfo.ConnectLineElementList[m];
                        OldAndNewType oldAndNewConnect=new OldAndNewType();
                        oldAndNewConnect.OldValue=portConnectInfo.ConnectLineElementList[m].ConnectedIndex;
                        oldAndNewConnect.NewValue=-1;
                        addLineInfo.OldAndNewConnectIndex = oldAndNewConnect;
                        OldAndNewType oldAndNewSender = new OldAndNewType();
                        oldAndNewSender.OldValue = portConnectInfo.ConnectLineElementList[m].SenderIndex;
                        oldAndNewSender.NewValue = -1;
                        addLineInfo.OldAndNewSenderIndex = oldAndNewSender;
                        OldAndNewType oldAndNewPort = new OldAndNewType();
                        oldAndNewPort.OldValue = portConnectInfo.ConnectLineElementList[m].PortIndex;
                        oldAndNewPort.NewValue = -1;
                        addLineInfo.OldAndNewPortIndex = oldAndNewPort;
                        addLineCollection.Add(addLineInfo);
                        if (portConnectInfo.ConnectLineElementList[m].FrontLine != null)
                        {
                            deleteCollection.Add(portConnectInfo.ConnectLineElementList[m].FrontLine);
                            _myScreenLayer.ElementCollection.Remove(portConnectInfo.ConnectLineElementList[m].FrontLine);
                        }
                        if (portConnectInfo.ConnectLineElementList[m].EndLine != null)
                        {
                            deleteCollection.Add(portConnectInfo.ConnectLineElementList[m].EndLine);
                            _myScreenLayer.ElementCollection.Remove(portConnectInfo.ConnectLineElementList[m].EndLine);
                        }
                        portConnectInfo.ConnectLineElementList[m].ConnectedIndex = -1;
                        portConnectInfo.ConnectLineElementList[m].SenderIndex = -1;
                        portConnectInfo.ConnectLineElementList[m].PortIndex = -1;
                    }
                    portConnectInfo.ConnectLineElementList.Clear();
                }
                portConnectInfo.MaxConnectElement = null;
                portConnectInfo.MaxConnectIndex = -1;
            }
            #region 更新带载区域信息
            UpdateLoadAreaInfo();
            #endregion
            DeleteAction deleteAction = new DeleteAction(_myScreenLayer,deleteCollection);
            AddLineAction addLineAction = new AddLineAction(addLineCollection);
            ConnectIconVisibilityAction connectVisibleAction = new ConnectIconVisibilityAction(connectIconVisibleCollection);
            using (Transaction.Create(SmartLCTActionManager))
            {
                SmartLCTActionManager.RecordAction(addLineAction);
                SmartLCTActionManager.RecordAction(deleteAction);
                SmartLCTActionManager.RecordAction(connectVisibleAction);
            }
            SelectedLayerAndElementChanged();
        }

        private static void OnCanExecSelectedAllCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            SmartLCTControl panel = (SmartLCTControl)sender;
            e.CanExecute = panel.CanSelectedAllExecute();
        }
        private bool CanSelectedAllExecute()
        {
            if (_myScreenLayer == null)
            {
                return false;
            }
            if (_myScreenLayer.ElementCollection != null && _myScreenLayer.ElementCollection.Count == 0)
            {
                return false;
            }
            return true ;
        }
        private static void OnSelectAllCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.OnCmdSelectedAll();
            }
        }
        private void OnCmdSelectedAll()
        {
          //  CurrentRectLayer = Function.FindCurrentSelectedLayer(MyRectLayer);
            if (_myScreenLayer == null)
            {
                return;
            }
            //RectLayer old = new RectLayer();
            //old = (RectLayer)MyRectLayer.Clone();
            for (int i = 0; i < _myScreenLayer.ElementCollection.Count; i++)
            {
                if (_myScreenLayer.ElementCollection[i] is RectElement)
                {
                    _myScreenLayer.ElementCollection[i].ElementSelectedState = SelectedState.Selected;
                }
                else if (_myScreenLayer.ElementCollection[i] is RectLayer)
                {
                    _myScreenLayer.ElementCollection[i].ElementSelectedState = SelectedState.Selected;
                    Function.SetElementSelectedState((RectLayer)_myScreenLayer.ElementCollection[i], SelectedState.Selected);
                }
            }
            //RectLayer newValue = (RectLayer)MyRectLayer.Clone();
            //PrePropertyChangedEventArgs actionValue = new PrePropertyChangedEventArgs() { PropertyName = "ElementCollection", OldValue = old, NewValue = newValue };
            //OnRecordActionValueChanged(actionValue);
            GetCurrentElementInfo();
            SelectedLayerAndElementChanged();
        }

        private static void OnCanExecDeleteCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            SmartLCTControl panel = (SmartLCTControl)sender;
            e.CanExecute = panel.CanDeleteExecute();
        }
        private bool CanDeleteExecute()
        {
            if (_myScreenLayer == null)
            {
                return false;
            }
            if (_myScreenLayer.OperateEnviron != OperatEnvironment.DesignScreen)
            {
                return false;
            }
            if (_selectedElementCollection.Count == 0)
            {
                return false;
            }
            return true;
        }
        private static void OnDeleteCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.OnCmdDelete();
            }
        }
        private void OnCmdDelete()
        {
            ObservableCollection<IElement> deleteCollection = new ObservableCollection<IElement>();
            ObservableCollection<IElement> addCollection = new ObservableCollection<IElement>();
            ObservableCollection<ConnectIconVisibilityInfo> connectIconVisibleCollection = new ObservableCollection<ConnectIconVisibilityInfo>();
            _elementMoveInfo.Clear();
            _elementSizeInfo.Clear();
            for (int i = 0; i < _selectedElementCollection.Count; i++)
            {
                Point oldPoint = new Point();
                oldPoint.X = _selectedElementCollection[i].X;
                oldPoint.Y = _selectedElementCollection[i].Y;
                _elementMoveInfo.Add(new ElementMoveInfo(_selectedElementCollection[i], new Point(), oldPoint));
            }
            for (int itemIndex = 0; itemIndex < _myScreenLayer.ElementCollection.Count; itemIndex++)
            {
                if (_myScreenLayer.ElementCollection[itemIndex].EleType != ElementType.receive)
                {
                    continue;
                }
                IRectElement element = (IRectElement)_myScreenLayer.ElementCollection[itemIndex];
                if (element.ConnectedIndex >= 0)
                {
                    ConnectIconVisibilityInfo info = new ConnectIconVisibilityInfo();
                    info.Element = element;
                    OldAndNewVisibility oldAndNewMaxmaxVisible = new OldAndNewVisibility();
                    oldAndNewMaxmaxVisible.OldValue = element.MaxConnectIndexVisibile;
                    OldAndNewVisibility oldAndNewMaxminVisible = new OldAndNewVisibility();
                    oldAndNewMaxminVisible.OldValue =element.MinConnectIndexVisibile;
                    info.OldAndNewMaxConnectIndexVisibile = oldAndNewMaxmaxVisible;
                    info.OldAndNewMinConnectIndexVisibile = oldAndNewMaxminVisible;
                    connectIconVisibleCollection.Add(info);

                }
            }
                foreach (int key in _currentSelectedElement.Keys)
                {
                    if (_currentSelectedElement[key].SelectedGroupElementList.Count != 0 && key != -1)
                    {
                        Point oldPoint = new Point();
                        oldPoint.X = _groupframeList[key].X;
                        oldPoint.Y = _groupframeList[key].Y;
                        _elementMoveInfo.Add(new ElementMoveInfo(_groupframeList[key], new Point(), oldPoint));
                        Size oldSize = new Size();
                        oldSize.Width = _groupframeList[key].Width;
                        oldSize.Height = _groupframeList[key].Height;
                        _elementSizeInfo.Add(new ElementSizeInfo(_groupframeList[key], new Size(), oldSize));
                    }
                }
            foreach(int elementIndex in _currentSelectedElement.Keys)
            {
                ObservableCollection<IRectElement> selectedGroupList = _currentSelectedElement[elementIndex].SelectedGroupElementList;
                Rect noSelectedElementRect = _currentSelectedElement[elementIndex].NoSelectedElementRect;
                for (int i = 0; i < selectedGroupList.Count; i++)
                {
                    #region 删除
                    if (selectedGroupList[i].EleType == ElementType.receive)
                    {
                        #region 处理连线
                        RectElement rect = (RectElement)selectedGroupList[i];
                        PortConnectInfo portConnectInfo = new PortConnectInfo();
                        if (rect.SenderIndex != -1 && rect.PortIndex != -1)
                        {
                            portConnectInfo = _myScreenLayer.SenderConnectInfoList[rect.SenderIndex].PortConnectInfoList[rect.PortIndex];
                        }
                        if (rect.FrontLine == null && rect.EndLine == null)
                        {
                            //更新Map值（发送卡带载的x,y的改变，会影响给发送卡下网口的映射位置）
                            //记录删除前发送卡的带载的x,y
                            Point oldSenderPoint = new Point();
                            if (rect.SenderIndex >= 0 && rect.PortIndex >= 0)
                            {
                                oldSenderPoint = new Point(_myScreenLayer.SenderConnectInfoList[rect.SenderIndex].LoadSize.X, _myScreenLayer.SenderConnectInfoList[rect.SenderIndex].LoadSize.Y);
                            }

                            portConnectInfo.ConnectLineElementList.Remove(rect);
                            portConnectInfo.MaxConnectElement = null;
                            portConnectInfo.MaxConnectIndex = -1;
                            deleteCollection.Add(rect);
                            _myScreenLayer.ElementCollection.Remove(rect);

                            if (_myScreenLayer.IsStartSetMapLocation && rect.SenderIndex>=0 && rect.PortIndex>=0)
                            {
                                //更新该发送卡的带载
                                portConnectInfo.LoadSize = Function.UnionRectCollection(portConnectInfo.ConnectLineElementList);
                                if(portConnectInfo.LoadSize.Height==0 && portConnectInfo.LoadSize.Width==0)
                                {
                                    portConnectInfo.MapLocation=new Point();
                                }
                                _myScreenLayer.SenderConnectInfoList[rect.SenderIndex].LoadSize = Function.UnionRectCollection(_myScreenLayer.SenderConnectInfoList[rect.SenderIndex].PortConnectInfoList);
                                Rect newSenderLoadSize = _myScreenLayer.SenderConnectInfoList[rect.SenderIndex].LoadSize;
                                Point newSenderPoint=new Point();
                                Point difSenderPint=new Point();
                                if(newSenderLoadSize.Width==0 && newSenderLoadSize.Height==0)
                                {
                                    //发送卡带载的xy有变化的话更新该发送卡下所有网口的Map
                                    //更新网口的map
                                    for (int j = 0; j < _myScreenLayer.SenderConnectInfoList[rect.SenderIndex].PortConnectInfoList.Count; j++)
                                    {
                                        PortConnectInfo portConnect = _myScreenLayer.SenderConnectInfoList[rect.SenderIndex].PortConnectInfoList[j];
                                        portConnect.MapLocation = new Point();
                                    }                                 
                                }
                                else
                                {
                                    newSenderPoint = new Point(newSenderLoadSize.X, newSenderLoadSize.Y);
                                    difSenderPint = new Point(newSenderLoadSize.X - oldSenderPoint.X, newSenderLoadSize.Y - oldSenderPoint.Y);
                                    //发送卡带载的xy有变化的话更新该发送卡下所有网口的Map
                                    if (difSenderPint.Y != 0 || difSenderPint.X != 0)
                                    {
                                        //更新网口的map
                                        for (int j = 0; j < _myScreenLayer.SenderConnectInfoList[rect.SenderIndex].PortConnectInfoList.Count; j++)
                                        {
                                            PortConnectInfo portConnect = _myScreenLayer.SenderConnectInfoList[rect.SenderIndex].PortConnectInfoList[j];
                                            if (portConnect.LoadSize.Height != 0 && portConnect.LoadSize.Width != 0)
                                            {
                                                Point newmapLocation = new Point(portConnect.MapLocation.X + difSenderPint.X, portConnect.MapLocation.Y + difSenderPint.Y);
                                                portConnect.MapLocation = newmapLocation;
                                            }
                                        }
                                    }
                                }
                               
                            }

                            continue;
                        }
                        if (rect.EndLine != null)
                        {
                            rect.EndLine.EndElement.FrontLine = null;
                            deleteCollection.Add(rect.EndLine);
                            _myScreenLayer.ElementCollection.Remove(rect.EndLine);
                            rect.EndLine = null;
                        }
                        if (rect.FrontLine != null)
                        {
                            rect.FrontLine.FrontElement.EndLine = null;
                            deleteCollection.Add(rect.FrontLine);
                            _myScreenLayer.ElementCollection.Remove(rect.FrontLine);
                            rect.FrontLine = null;
                        }
                        deleteCollection.Add(rect);
                       
                        if (portConnectInfo.MaxConnectIndex != rect.ConnectedIndex)
                        {
                            List<IRectElement> connectList = portConnectInfo.ConnectLineElementList.ToList<IRectElement>();
                            connectList.Sort(delegate(IRectElement first, IRectElement second)
                            {
                                return first.ConnectedIndex.CompareTo(second.ConnectedIndex);
                            });
                            int currentIndex = connectList.IndexOf(rect);                            
                            connectList[currentIndex + 1].ConnectedIndex = connectList[currentIndex + 1].ConnectedIndex;
                            if (currentIndex == 0)
                            {
                                ((RectElement)connectList[currentIndex + 1]).MinConnectIndexVisibile = Visibility.Visible;
                                ((RectElement)connectList[currentIndex + 1]).MaxConnectIndexVisibile = Visibility.Hidden;
                            }
                            if(connectList[currentIndex+1].FrontLine!=null)
                            addCollection.Add(connectList[currentIndex + 1].FrontLine);                          
                        }
                        else
                        {
                            List<IRectElement> connectList = portConnectInfo.ConnectLineElementList.ToList<IRectElement>();
                            connectList.Sort(delegate(IRectElement first, IRectElement second)
                            {
                                return first.ConnectedIndex.CompareTo(second.ConnectedIndex);
                            });
                            if (connectList.Count == 1)
                            {
                                portConnectInfo.MaxConnectIndex = -1;
                                portConnectInfo.MaxConnectElement = null;
                                rect.MinConnectIndexVisibile = Visibility.Hidden;
                                rect.MaxConnectIndexVisibile = Visibility.Hidden;
                            }
                            else
                            {
                                portConnectInfo.MaxConnectIndex = connectList[connectList.Count - 2].ConnectedIndex;
                                portConnectInfo.MaxConnectElement = (RectElement)connectList[connectList.Count - 2];
                                if (connectList.Count > 2)
                                {
                                    ((RectElement)connectList[connectList.Count - 2]).MaxConnectIndexVisibile = Visibility.Visible;
                                }
                            }
                        }
                        //更新Map值（发送卡带载的x,y的改变，会影响给发送卡下网口的映射位置）
                        //记录删除前发送卡的带载的x,y
                        Point oldSenderPoints = new Point(_myScreenLayer.SenderConnectInfoList[rect.SenderIndex].LoadSize.X, _myScreenLayer.SenderConnectInfoList[rect.SenderIndex].LoadSize.Y);
                        portConnectInfo.ConnectLineElementList.Remove(rect);
                        _myScreenLayer.ElementCollection.Remove(rect);
                        if (_myScreenLayer.IsStartSetMapLocation)
                        {
                            //更新该发送卡的带载
                            portConnectInfo.LoadSize = Function.UnionRectCollection(portConnectInfo.ConnectLineElementList);
                            if (portConnectInfo.LoadSize.Height == 0 && portConnectInfo.LoadSize.Width == 0)
                            {
                                portConnectInfo.MapLocation = new Point();
                            }
                            _myScreenLayer.SenderConnectInfoList[rect.SenderIndex].LoadSize = Function.UnionRectCollection(_myScreenLayer.SenderConnectInfoList[rect.SenderIndex].PortConnectInfoList);
                            Rect newSenderLoadSizes = _myScreenLayer.SenderConnectInfoList[rect.SenderIndex].LoadSize;
                            Point newSenderPoints = new Point();
                            Point difSenderPints = new Point();
                            if (newSenderLoadSizes.Height == 0 && newSenderLoadSizes.Width == 0)
                            {
                                //发送卡带载的xy有变化的话更新该发送卡下所有网口的Map
                                //更新网口的map
                                for (int j = 0; j < _myScreenLayer.SenderConnectInfoList[rect.SenderIndex].PortConnectInfoList.Count; j++)
                                {
                                    PortConnectInfo portConnect = _myScreenLayer.SenderConnectInfoList[rect.SenderIndex].PortConnectInfoList[j];
                                    portConnect.MapLocation = new Point();
                                }
                            }
                            else
                            {
                                newSenderPoints = new Point(newSenderLoadSizes.X, newSenderLoadSizes.Y);
                                difSenderPints = new Point(newSenderPoints.X - oldSenderPoints.X, newSenderPoints.Y - oldSenderPoints.Y);
                                //发送卡带载的xy有变化的话更新该发送卡下所有网口的Map
                                if (difSenderPints.Y != 0 || difSenderPints.X != 0)
                                {
                                    //更新网口的map
                                    for (int j = 0; j < _myScreenLayer.SenderConnectInfoList[rect.SenderIndex].PortConnectInfoList.Count; j++)
                                    {
                                        PortConnectInfo portConnect = _myScreenLayer.SenderConnectInfoList[rect.SenderIndex].PortConnectInfoList[j];
                                        if (portConnect.LoadSize.Height != 0 && portConnect.LoadSize.Width != 0)
                                        {
                                            Point newmapLocation = new Point(portConnect.MapLocation.X + difSenderPints.X, portConnect.MapLocation.Y + difSenderPints.Y);
                                            portConnect.MapLocation = newmapLocation;
                                        }
                                    }
                                }
                            }
                            
                        }
                        #endregion
                    }
                    #endregion
                }

                for (int itemIndex = 0; itemIndex < connectIconVisibleCollection.Count; itemIndex++)
                {
                    ConnectIconVisibilityInfo info=connectIconVisibleCollection[itemIndex];
                    if (_myScreenLayer.ElementCollection.Contains(info.Element))
                    {
                        info.OldAndNewMaxConnectIndexVisibile.NewValue = info.Element.MaxConnectIndexVisibile;
                        info.OldAndNewMinConnectIndexVisibile.NewValue = info.Element.MinConnectIndexVisibile;
                    }
                    else
                    {
                        info.OldAndNewMaxConnectIndexVisibile.NewValue = Visibility.Hidden;
                        info.OldAndNewMinConnectIndexVisibile.NewValue = Visibility.Hidden;
                    }
                }               
                    //组框大小和位置=该组内没有选中的接收卡大矩形
                    if (elementIndex != -1)
                    {
                        if (noSelectedElementRect.Width == 0 && noSelectedElementRect.Height == 0)
                        {
                            deleteCollection.Add(_groupframeList[elementIndex]);
                            _myScreenLayer.ElementCollection.Remove(_groupframeList[elementIndex]);
                            _groupframeList.Remove(elementIndex);
                        }
                        else
                        {
                            _groupframeList[elementIndex].X = noSelectedElementRect.X;
                            _groupframeList[elementIndex].Y = noSelectedElementRect.Y;
                            _groupframeList[elementIndex].Width = noSelectedElementRect.Width;
                            _groupframeList[elementIndex].Height = noSelectedElementRect.Height;
                        }
                    }
                    else
                    {
                        if (noSelectedElementRect.Width == 0 && noSelectedElementRect.Height == 0)
                        {
                            _groupframeList.Remove(elementIndex);
                        }
                    }
            }
            AddAction addAction = new AddAction(_myScreenLayer, addCollection);
            DeleteAction deleteAction = new DeleteAction(_myScreenLayer, deleteCollection);
            for(int m=0;m<addCollection.Count;m++)
            {
                if (m < 0)
                {
                    m = 0;
                }
                if(addCollection[m] is LineElement)
                {
                    LineElement addLine=(LineElement)addCollection[m];
                    for (int n = 0; n < deleteCollection.Count; n++)
                    {
                        if(deleteCollection[n] is LineElement)
                        {
                            LineElement deleteLine=(LineElement)deleteCollection[n];
                             if(addLine.FrontElement.SenderIndex==deleteLine.FrontElement.SenderIndex  
                                 && addLine.FrontElement.PortIndex==deleteLine.FrontElement.PortIndex 
                                 && addLine.FrontElement.ConnectedIndex==deleteLine.FrontElement.ConnectedIndex
                                 && addLine.EndElement.ConnectedIndex == deleteLine.EndElement.ConnectedIndex)
                             {
                                 addCollection.Remove(addLine);
                                 m = m - 1;                        
                                 deleteCollection.Remove(deleteLine);
                                 n = n - 1;
                              
                             }
                        }
                       
                    }
                }
            }
            ObservableCollection<ElementMoveInfo> elementMoveInfo = new ObservableCollection<ElementMoveInfo>();
            for (int i = 0; i < _elementMoveInfo.Count; i++)
            {
                Point newPoint = new Point();
                newPoint.X = _elementMoveInfo[i].Element.X;
                newPoint.Y = _elementMoveInfo[i].Element.Y;
                elementMoveInfo.Add(new ElementMoveInfo(_elementMoveInfo[i].Element, newPoint, _elementMoveInfo[i].OldPoint));
            }
            ElementMoveAction moveAction = new ElementMoveAction(elementMoveInfo);
            ObservableCollection<ElementSizeInfo> elementSizeInfo = new ObservableCollection<ElementSizeInfo>();
            for (int j = 0; j < _elementSizeInfo.Count; j++)
            {
                Size newSize = new Size();
                newSize.Width = _elementSizeInfo[j].Element.Width;
                newSize.Height = _elementSizeInfo[j].Element.Height;
                elementSizeInfo.Add(new ElementSizeInfo(_elementSizeInfo[j].Element, newSize, _elementSizeInfo[j].OldSize));
            }
            ElementSizeAction sizeAction = new ElementSizeAction(elementSizeInfo);
            ConnectIconVisibilityAction connectIconAction = new ConnectIconVisibilityAction(connectIconVisibleCollection);
            using (Transaction.Create(SmartLCTActionManager))
            {
                SmartLCTActionManager.RecordAction(moveAction);
                SmartLCTActionManager.RecordAction(sizeAction);
                SmartLCTActionManager.RecordAction(deleteAction);
                SmartLCTActionManager.RecordAction(addAction);
                SmartLCTActionManager.RecordAction(connectIconAction);
            }
            GetCurrentElementInfo();
            SelectedLayerAndElementChanged();
        }

        private static void OnCanExecCopyCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            SmartLCTControl panel = (SmartLCTControl)sender;
            e.CanExecute = panel.CanCopyExecute();
        }
        private bool CanCopyExecute()
        {
            return CanDeleteExecute();
        }
        private static void OnCopyCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.OnCmdCopy();
            }
        }
        private void OnCmdCopy()
        {
            _copyElementCollection.Clear();
            for (int elementIndex = 0; elementIndex < _selectedElementCollection.Count; elementIndex++)
            {
                IRectElement element = (IRectElement)((RectElement)_selectedElementCollection[elementIndex]).Clone();
                _copyElementCollection.Add(element);
            }
        }

        private static void OnCanExecCutCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            SmartLCTControl panel = (SmartLCTControl)sender;
            e.CanExecute = panel.CanCutExecute();
        }
        private bool CanCutExecute()
        {
            return CanDeleteExecute();
        }
        private static void OnCutCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.OnCmdCut();
            }
        }
        private void OnCmdCut()
        {
            OnCmdCopy();
            OnCmdDelete();
        }

        private static void OnCanExecPasteCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            SmartLCTControl panel = (SmartLCTControl)sender;
            e.CanExecute = panel.CanPasteExecute();
        }
        public bool CanPasteExecute()
        {
            if (_copyElementCollection != null && _copyElementCollection.Count != 0)
            {
                return true;
            }
            return false;
        }
        private static void OnPasteCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.OnCmdPaste();
            }
        }
        private void OnCmdPaste()
        {
            ObservableCollection<IElement> addCollection = new ObservableCollection<IElement>();
            Function.SetElementCollectionState(_selectedElementCollection, SelectedState.None);
            Rect unionRect = Function.UnionRectCollection(_selectedElementCollection);
            _selectedElementCollection.Clear();
            Point startPoint = new Point();
            if(_keyInfo.Count==2 
                && (_keyInfo.Contains(Key.LeftCtrl)|| _keyInfo.Contains(Key.RightCtrl)) 
                && _keyInfo.Contains(Key.V)
               )
            {
                if (_copyPoint.X == -1 && _copyPoint.Y == -1)   //键盘操作
                {
                    startPoint.X = unionRect.Left + 10;
                    startPoint.Y = unionRect.Top + 10;
                }
                else
                {
                    startPoint = this.TranslatePoint(_copyPoint, _itemsControl);
                }
            }
            else
            {
                startPoint = this.TranslatePoint(_copyPoint, _itemsControl);
            }
            Rect copyUnionRect = Function.UnionRectCollection(_copyElementCollection);
            for (int elementIndex = 0; elementIndex < _copyElementCollection.Count; elementIndex++)
            {
                IRectElement element = _copyElementCollection[elementIndex];
                RectElement rectElement = new RectElement();
                rectElement = new RectElement(startPoint.X + (element.X - copyUnionRect.Left), startPoint.Y + (element.Y - copyUnionRect.Top), element.Width, element.Height, _myScreenLayer, _myScreenLayer.MaxZorder + 1);
                rectElement.EleType = ElementType.receive;
                _myScreenLayer.MaxZorder += 1;
                rectElement.IsLocked = true;
                rectElement.ZIndex = 7;
                rectElement.GroupName = -1;
                rectElement.ElementSelectedState = SelectedState.Selected;
                _selectedElementCollection.Add(rectElement);
                _myScreenLayer.ElementCollection.Add(rectElement);
                addCollection.Add(rectElement);
            }
            AddAction addAction = new AddAction(_myScreenLayer,addCollection);
            SmartLCTActionManager.RecordAction(addAction);
            GetCurrentElementInfo();
        }

        private static void OnCanExecClearCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            SmartLCTControl panel = (SmartLCTControl)sender;
            e.CanExecute = panel.CanClearExecute();
        }
        private bool CanClearExecute()
        {
            if (_myScreenLayer.ElementCollection.Count == 0)
            {
                return false;
            }
            if (_myScreenLayer.OperateEnviron != OperatEnvironment.DesignScreen)
            {
                return false;
            }
            return true;
        }
        private static void OnClearCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.OnCmdClear();
            }
        }
        private void OnCmdClear()
        {
            ObservableCollection<IElement> deleteCollection = new ObservableCollection<IElement>();
            ObservableCollection<ConnectIconVisibilityInfo> connectIconVisibleCollection = new ObservableCollection<ConnectIconVisibilityInfo>();
            for (int i = 0; i < _myScreenLayer.ElementCollection.Count; i++)
            {
                IElement element = _myScreenLayer.ElementCollection[i];
                deleteCollection.Add(element);
                if(element.EleType==ElementType.receive && element.ConnectedIndex>=0)
                {
                    ConnectIconVisibilityInfo info = new ConnectIconVisibilityInfo();
                    info.Element = (IRectElement)element;
                    OldAndNewVisibility maxVisible = new OldAndNewVisibility();
                    maxVisible.OldValue = info.Element.MaxConnectIndexVisibile;
                    maxVisible.NewValue = Visibility.Hidden;
                    OldAndNewVisibility minVisible = new OldAndNewVisibility();
                    minVisible.OldValue = info.Element.MinConnectIndexVisibile;
                    minVisible.NewValue = Visibility.Hidden;
                    info.OldAndNewMaxConnectIndexVisibile = maxVisible;
                    info.OldAndNewMinConnectIndexVisibile = minVisible;
                    connectIconVisibleCollection.Add(info);
                }
            }
            _myScreenLayer.ElementCollection.Clear();
            UpdateSenderAndPortConnectInfo();
            UpdateGroupframeList();
            UpdateLoadAreaInfo();
            //GetCurrentElementInfo();

            DeleteAction deleteAction = new DeleteAction(_myScreenLayer, deleteCollection);
            ConnectIconVisibilityAction connectVisibleAction = new ConnectIconVisibilityAction(connectIconVisibleCollection);
            using (Transaction.Create(SmartLCTActionManager))
            {
                SmartLCTActionManager.RecordAction(deleteAction);
                SmartLCTActionManager.RecordAction(connectVisibleAction);
            }

            SelectedLayerAndElementValue.SelectedInfoList = null;
            SelectedLayerAndElementValue.SelectedLayer = _myScreenLayer;
            SelectedLayerAndElementValue.CurrentRectElement = null;
            SelectedLayerAndElementValue.GroupframeList = _groupframeList;
            SelectedLayerAndElementValue.SelectedInfoList = _currentSelectedElement;
            SelectedLayerAndElementValue.SelectedElement.Clear();
            SelectedLayerAndElementValue.MainControlSize = new Size(this.ActualWidth, this.ActualHeight);
            Messenger.Default.Send<SelectedLayerAndElement>(SelectedLayerAndElementValue, MsgToken.MSG_SELECTEDLAYERANDELEMENT_CHANGED);
        }

        private static void OnCanExecUnDoCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            SmartLCTControl panel = (SmartLCTControl)sender;
            e.CanExecute = panel.CanUnDo();
        }
        public bool CanUnDo()
        {
            if (_myScreenLayer.OperateEnviron != OperatEnvironment.DesignScreen)
            {
                return false;
            }
            return SmartLCTActionManager.CanUndo;
        }
        private static void OnUnDoCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.UnDo();
            }
        }
        private void UnDo()
        {
            _actionIsExcuting = true;
            SmartLCTActionManager.Undo();
            _actionIsExcuting = false;
            this.Focus();
        
            GetCurrentElementInfo();
            //更新各个网口的连线元素、最大连接数
            if (_myScreenLayer.OperateEnviron == OperatEnvironment.DesignScreen)
            {
                UpdateSenderAndPortConnectInfo();
                UpdateGroupframeList();
            }
            #region 更新带载区域信息
            UpdateLoadAreaInfo();
            #endregion

            SelectedLayerAndElementChanged();
        }

        private static void OnCanExecReDoCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            SmartLCTControl panel = (SmartLCTControl)sender;
            e.CanExecute = panel.CanReDo();
        }
        public bool CanReDo()
        {
            if (_myScreenLayer.OperateEnviron != OperatEnvironment.DesignScreen)
            {
                return false;
            }
            return SmartLCTActionManager.CanRedo;
        }
        private static void OnReDoCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.ReDo();
            }
        }
        private void ReDo()
        {
            _actionIsExcuting = true;
            SmartLCTActionManager.Redo();
            _actionIsExcuting = false;
            this.Focus();
            GetCurrentElementInfo();

            //更新各个网口的连线元素、最大连接数
            if (_myScreenLayer.OperateEnviron == OperatEnvironment.DesignScreen)
            {
                UpdateSenderAndPortConnectInfo();
                UpdateGroupframeList();
            }
            #region 更新带载区域信息
            UpdateLoadAreaInfo();
            #endregion

            SelectedLayerAndElementChanged();

        }

        private static void OnCanExecIncreaseCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            SmartLCTControl panel = (SmartLCTControl)sender;
            e.CanExecute = panel.OnCanExecIncreace();
        }
        private bool OnCanExecIncreace()
        {
            if (_myScreenLayer.IncreaseOrDecreaseIndex >= SmartLCTViewModeBase.MaxIncreaseIndex)
            {
                return false;
            }
            return true;
        }
        private static void OnIncreaseCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.SetIncrease();
            }
        }
        private void SetIncrease()
        {
            if (_myScreenLayer.IncreaseOrDecreaseIndex > SmartLCTViewModeBase.MaxIncreaseIndex)
            {
                return;
            }
            _viewbox.Height = _viewbox.ActualHeight * SmartLCTViewModeBase.IncreaseOrDecreaseValue;
            _viewbox.Width = _viewbox.ActualWidth * SmartLCTViewModeBase.IncreaseOrDecreaseValue;
            _myScreenLayer.IncreaseOrDecreaseIndex = _myScreenLayer.IncreaseOrDecreaseIndex + 1;
            Messenger.Default.Send<int>(_myScreenLayer.IncreaseOrDecreaseIndex, MsgToken.MSG_INCREASEORDECREASEINDEX);

        }

        private static void OnCanExecDecreaseCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            SmartLCTControl panel = (SmartLCTControl)sender;
            e.CanExecute = panel.OnCanExecDecreace();
        }
        private bool OnCanExecDecreace()
        {
            if (_myScreenLayer.OperateEnviron == OperatEnvironment.DesignScreen && _myScreenLayer.IncreaseOrDecreaseIndex < SmartLCTViewModeBase.MinIncreaseIndex)
            {
                return false;
            }
            if (_myScreenLayer.OperateEnviron != OperatEnvironment.DesignScreen && _myScreenLayer.IncreaseOrDecreaseIndex < SmartLCTViewModeBase.MinIncreaseIndex * 2)
            {
                return false;
            }
            return true;
        }
        private static void OnDecreaseCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.SetDecrease();
            }
        }
        private void SetDecrease()
        {
            if(_myScreenLayer.OperateEnviron==OperatEnvironment.DesignScreen && _myScreenLayer.IncreaseOrDecreaseIndex < SmartLCTViewModeBase.MinIncreaseIndex)
            {
                return;
            }
            if (_myScreenLayer.OperateEnviron != OperatEnvironment.DesignScreen && _myScreenLayer.IncreaseOrDecreaseIndex < SmartLCTViewModeBase.MinIncreaseIndex*2)
            {
                return;
            }
            _viewbox.Height = _viewbox.ActualHeight / SmartLCTViewModeBase.IncreaseOrDecreaseValue;
            _viewbox.Width = _viewbox.ActualWidth / SmartLCTViewModeBase.IncreaseOrDecreaseValue;
            _myScreenLayer.IncreaseOrDecreaseIndex = _myScreenLayer.IncreaseOrDecreaseIndex - 1;
            Messenger.Default.Send<int>(_myScreenLayer.IncreaseOrDecreaseIndex, MsgToken.MSG_INCREASEORDECREASEINDEX);
        }

        private static void OnCanExecOrignalSizeCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            SmartLCTControl panel = (SmartLCTControl)sender;
            e.CanExecute = panel.CanOrignalSizeExecute();
        }
        private bool CanOrignalSizeExecute()
        {
            if (_myScreenLayer.OperateEnviron==OperatEnvironment.DesignScreen && _myScreenLayer.IncreaseOrDecreaseIndex == 0)
            {
                return false;
            }
            return true;
        }
        private static void OnOrginalSizeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SmartLCTControl control = sender as SmartLCTControl;
            if (control != null)
            {
                control.OnOrginalSize();
            }
        }
        private void OnOrginalSize()
        {
            if (_myScreenLayer.OperateEnviron == OperatEnvironment.DesignScreen)
            {
                _viewbox.Height = _viewbox.ActualHeight / Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex);
                _viewbox.Width = _viewbox.ActualWidth / Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex);
                _myScreenLayer.IncreaseOrDecreaseIndex = 0;
                Messenger.Default.Send<int>(_myScreenLayer.IncreaseOrDecreaseIndex, MsgToken.MSG_INCREASEORDECREASEINDEX);
            }
            else
            {
                int originalIndex = GetOriginalIncreaseOrDecreaseIndex(_myScreenLayer);
                _viewbox.Height = _viewbox.ActualHeight / Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex-originalIndex);
                _viewbox.Width = _viewbox.ActualWidth / Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex-originalIndex);
                _myScreenLayer.IncreaseOrDecreaseIndex = originalIndex;
                Messenger.Default.Send<int>(_myScreenLayer.IncreaseOrDecreaseIndex, MsgToken.MSG_INCREASEORDECREASEINDEX);
            }
            //if (_myScreenLayer.OperateEnviron == OperatEnvironment.AdjustSenderLocation ||
            //    _myScreenLayer.OperateEnviron == OperatEnvironment.AdjustScreenLocation)
            //{
            //    for (int i = 1; i <= Math.Abs(oldIndex-_myScreenLayer.OriginalIncreaseOrDecreaseIndex); i++)
            //    {

            //        _viewbox.Height = _viewbox.ActualHeight * SmartLCTViewModeBase.IncreaseOrDecreaseValue;
            //        _viewbox.Width = _viewbox.ActualWidth * SmartLCTViewModeBase.IncreaseOrDecreaseValue;

            //    }
            //    _myScreenLayer.IncreaseOrDecreaseIndex = _myScreenLayer.OriginalIncreaseOrDecreaseIndex;

            //    Messenger.Default.Send<int>(_myScreenLayer.IncreaseOrDecreaseIndex, MsgToken.MSG_INCREASEORDECREASEINDEX);

            
        }
        private int GetOriginalIncreaseOrDecreaseIndex(IRectLayer layer)
        {
            int res = 0;
            double maxDviWidth = this.ActualWidth - SmartLCTViewModeBase.ScrollWidth;
            double maxDviHeight = this.ActualHeight - SmartLCTViewModeBase.ScrollWidth;

            int widthDecreaseIndex = -1;
            int heightDecreaseIndex = -1;
            double layerHeight = layer.Height / SmartLCTViewModeBase.IncreaseOrDecreaseValue;
            double layWidth = layer.Width / SmartLCTViewModeBase.IncreaseOrDecreaseValue;
            while (layWidth > maxDviWidth)
            {
                layWidth = layWidth / SmartLCTViewModeBase.IncreaseOrDecreaseValue;
                widthDecreaseIndex -= 1;
            }
            while (layerHeight > maxDviHeight)
            {
                layerHeight = layerHeight / SmartLCTViewModeBase.IncreaseOrDecreaseValue;
                heightDecreaseIndex -= 1;
            }
            if (Math.Abs(heightDecreaseIndex) > Math.Abs(widthDecreaseIndex))
            {
                res = heightDecreaseIndex;
            }
            else
            {
                res = widthDecreaseIndex;
            }
            return res;
        }


        private void OnMyRectLayerChanged(object sender, PropertyChangedEventArgs e)
        {

        }
        private static void MyRectLayerPropertyChangedCallBack(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            SmartLCTControl ctrl = (SmartLCTControl)obj;
            if (e.Property == MyRectLayerProperty)
            {
                ctrl.OnRectLayerPropertyChanged();
            }
        }
        private void OnRectLayerPropertyChanged()
        {
            if (_itemsControl != null)
            {
                Binding myBinding = new Binding("ElementCollection");
                myBinding.Source = MyRectLayer;
                myBinding.Mode = BindingMode.TwoWay;               
                _itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, myBinding);
                
            }
            if (MyRectLayer != null && MyRectLayer.ElementCollection.Count != 0)
            {
                _myScreenLayer = (RectLayer)MyRectLayer.ElementCollection[0];
            }
            else
            {
                _myScreenLayer = new RectLayer();
                _myScreenLayer.EleType = ElementType.newLayer;
            }
            GetCurrentElementInfo();

            //缩放(先还原)
            if (_viewbox != null)
            {
                if (_myScreenLayer.OperateEnviron == OperatEnvironment.DesignScreen)
                {
                    _viewbox.Width = SmartLCTViewModeBase.ViewBoxWidth;
                    _viewbox.Height = SmartLCTViewModeBase.ViewBoxHeight;
                }
                else
                {
                    _viewbox.Width = SmartLCTViewModeBase.DviViewBoxWidth;
                    _viewbox.Height = SmartLCTViewModeBase.DviViewBoxHeight;
                }
                if (_myScreenLayer != new RectLayer() && _viewbox != null && _myScreenLayer.IncreaseOrDecreaseIndex > 0)
                {
                    _viewbox.Height = _viewbox.Height * Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex);
                    _viewbox.Width = _viewbox.Width * Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex);
                }
                else if (_myScreenLayer != new RectLayer() && _viewbox != null && _myScreenLayer.IncreaseOrDecreaseIndex < 0)
                {
                    _viewbox.Height = _viewbox.Height / Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, -_myScreenLayer.IncreaseOrDecreaseIndex);
                    _viewbox.Width = _viewbox.Width / Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, -_myScreenLayer.IncreaseOrDecreaseIndex);
                }
            }
            if (_myScreenLayer != new RectLayer() && _myScreenLayer.OperateEnviron == OperatEnvironment.DesignScreen)
            {
                SelectedLayerAndElementValue.SelectedLayer = _myScreenLayer;
                SelectedLayerAndElementValue.CurrentRectElement = null;
                SelectedLayerAndElementValue.SelectedElement = _selectedElementCollection;
                SelectedLayerAndElementValue.GroupframeList = _groupframeList;
                SelectedLayerAndElementValue.SelectedInfoList = _currentSelectedElement;
                SelectedLayerAndElementValue.MainControlSize = new Size(this.ActualWidth, this.ActualHeight);
                Messenger.Default.Send<SelectedLayerAndElement>(SelectedLayerAndElementValue, MsgToken.MSG_SELECTEDLAYERANDELEMENT_CHANGED);
            }
       
            this.Focus();
        }      
        #endregion

        #region 控件事件
        private void SmartLCTControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_myScreenLayer == null)
            {
                return;
            }
            #region 注册消息
            Messenger.Default.Register<object>(this, MsgToken.MSG_EXIT, OnExit);
            Messenger.Default.Register<NotificationMessageAction<AddReceiveInfo>>(this, MsgToken.MSG_ADDRECEIVE, OnAddReceive);
            #endregion
            if (_itemsControl == null)
            {
                return;
            }
            _itemsControl.AllowDrop = true;
            _itemsControl.MouseLeftButtonDown += new MouseButtonEventHandler(_itemsControl_MouseLeftButtonDown);
            _itemsControl.MouseMove += new MouseEventHandler(_itemsControl_MouseMove);
            _itemsControl.MouseUp += new MouseButtonEventHandler(_itemsControl_MouseUp);
            _itemsControl.MouseRightButtonDown += new MouseButtonEventHandler(_itemsControl_MouseRightButtonDown);
            _itemsControl.MouseWheel+=new MouseWheelEventHandler(_itemsControl_MouseWheel);
            if (_itemsControl != null)
            {
                Binding myBinding = new Binding("ElementCollection");
                myBinding.Source = MyRectLayer;
                myBinding.Mode = BindingMode.TwoWay;
                _itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, myBinding);
                if (MyRectLayer != null)
                {
                    _itemsControl.Height = MyRectLayer.Height;
                    _itemsControl.Width = MyRectLayer.Width;
                }

            }

            InitContextMenu();

            if (MyRectLayer != null && MyRectLayer.ElementCollection.Count != 0)
            {
                _myScreenLayer = (RectLayer)MyRectLayer.ElementCollection[0];
                if (_myScreenLayer.OperateEnviron == OperatEnvironment.DesignScreen)
                {
                    _myScreenLayer.Width = 4000;
                    _myScreenLayer.Height = 3000;
                }

            }
            if (MyRectLayer != null && MyRectLayer.ElementCollection.Count != 0 && _myScreenLayer.OperateEnviron == OperatEnvironment.DesignScreen)
            {
                SelectedLayerAndElementValue.SelectedLayer = _myScreenLayer;
                SelectedLayerAndElementValue.CurrentRectElement = null;
                SelectedLayerAndElementValue.SelectedElement.Clear();
                SelectedLayerAndElementValue.GroupframeList = _groupframeList;
                SelectedLayerAndElementValue.SelectedInfoList = _currentSelectedElement;
                SelectedLayerAndElementValue.MainControlSize = new Size(this.ActualWidth, this.ActualHeight);
                Messenger.Default.Send<SelectedLayerAndElement>(SelectedLayerAndElementValue, MsgToken.MSG_SELECTEDLAYERANDELEMENT_CHANGED);
            }
            this.Focus();
        }
        private void _itemsControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            IElement element = sender as IElement;
            if (_addReceiveInfo != null && _myScreenLayer.OperateEnviron == OperatEnvironment.DesignScreen)
            {
                _addReceiveInfo = null;
                _addReceiveCallback.Execute(null);
                this.Focus();
                SelectedElementChangedHandle(MouseState.None);
                e.Handled = true;
                return;
            }
            ObservableCollection<IElement> selectedElements = new ObservableCollection<IElement>();
            if (sender is RectLayer)
            {
                #region 点击图层，取消该图层上所有元素的选中状态
                for (int elementIndex = 0; elementIndex < ((RectLayer)sender).ElementCollection.Count; elementIndex++)
                {
                    ((RectLayer)sender).ElementCollection[elementIndex].ElementSelectedState = SelectedState.None;
                }
                #endregion
                for (int elementIndex = 0; elementIndex < ((RectLayer)sender).ElementCollection.Count; elementIndex++)
                {
                    if (!(((RectLayer)sender).ElementCollection[elementIndex] is RectElement))
                    {
                        continue;
                    }
                    if (((RectElement)((RectLayer)sender).ElementCollection[elementIndex]).ZOrder == -2)
                    {
                        #region 选中元素
                        RectElement rect1 = ((RectElement)((RectLayer)sender).ElementCollection[elementIndex]);
                        int index = 0;
                        for (int i = 0; i < ((RectLayer)sender).ElementCollection.Count; i++)
                        {
                            RectElement rect2 = ((RectElement)((RectLayer)sender).ElementCollection[i]);

                            if (Function.IsRectIntersect(rect1, rect2))
                            {
                                index = index + 1;
                                ((RectElement)((RectLayer)sender).ElementCollection[i]).ElementSelectedState = SelectedState.Selected;
                                if (index == 1)
                                {
                                    ((RectElement)((RectLayer)sender).ElementCollection[i]).ElementSelectedState = SelectedState.SpecialSelected;
                                }
                            }
                        }
                        ((RectLayer)sender).ElementCollection.RemoveAt(elementIndex);
                        #endregion
                    }
                }
            }
            else
            {
                #region 该图层上已经选中的元素
                //for (int elementIndex = 0; elementIndex < ((RectLayer)element.ParentElement).ElementCollection.Count; elementIndex++)
                //{
                //    if (((RectLayer)element.ParentElement).ElementCollection[elementIndex].ElementSelectedState != SelectedState.None)
                //    {
                //        selectedElements.Add(((RectLayer)element.ParentElement).ElementCollection[elementIndex]);
                //    }
                //}
                #endregion

                #region 点击矩形的处理
                //if ((Keyboard.GetKeyStates(Key.LeftCtrl) == KeyStates.Down || (Keyboard.GetKeyStates(Key.LeftCtrl) == (KeyStates.Down | KeyStates.Toggled))) ||
                //    (Keyboard.GetKeyStates(Key.RightCtrl) == KeyStates.Down || (Keyboard.GetKeyStates(Key.RightCtrl) == (KeyStates.Down | KeyStates.Toggled))))
                //{
                //    #region 按ctrl键
                //    if (ElementSelectedState != SelectedState.None)
                //    {
                //        #region 选中已经被选中的
                //        if (ElementSelectedState == SelectedState.SpecialSelected)
                //        {
                //            for (int selectedElementIndex = 0; selectedElementIndex < selectedElements.Count; selectedElementIndex++)
                //            {
                //                int zorder = ((RectElement)(selectedElements[selectedElementIndex])).ZOrder;
                //                if (zorder != ZOrder)
                //                {
                //                    int index = ((RectLayer)ParentElement).ElementCollection.IndexOf(selectedElements[selectedElementIndex]);
                //                    ((RectLayer)ParentElement).ElementCollection[index].ElementSelectedState = SelectedState.SpecialSelected;
                //                    break;
                //                }
                //            }
                //        }
                //        ElementSelectedState = SelectedState.None;
                //        #endregion
                //    }
                //    else
                //    {
                //        #region 选中还没有被选中
                //        if (selectedElements.Count != 0)
                //        {
                //            ElementSelectedState = SelectedState.Selected;
                //        }
                //        else
                //        {
                //            ElementSelectedState = SelectedState.SpecialSelected;
                //        }
                //        #endregion
                //    }
                //    #endregion
                //}
                //else
                //{
                #region 不按ctrl键
                //for (int selectedElementIndex = 0; selectedElementIndex < selectedElements.Count; selectedElementIndex++)
                //{
                //    int index = ((RectLayer)element.ParentElement).ElementCollection.IndexOf(selectedElements[selectedElementIndex]);
                //    if (element.ElementSelectedState == SelectedState.None)
                //    {
                //        ((RectLayer)element.ParentElement).ElementCollection[index].ElementSelectedState = SelectedState.None;
                //    }
                //    else
                //    {
                //        ((RectLayer)element.ParentElement).ElementCollection[index].ElementSelectedState = SelectedState.Selected;
                //    }
                //}
                //element.ElementSelectedState = SelectedState.Selected;
                #endregion
                //}
                #endregion
            }
            if (_myScreenLayer != null)
                Function.SetElementCollectionState(_myScreenLayer.ElementCollection, SelectedState.None);


            GetCurrentElementInfo();
            SelectedLayerAndElementValue.SelectedLayer = _myScreenLayer;
            SelectedLayerAndElementValue.GroupframeList = _groupframeList;
            SelectedLayerAndElementValue.SelectedInfoList = _currentSelectedElement;
            SelectedLayerAndElementValue.MainControlSize = new Size(this.ActualWidth, this.ActualHeight);
            Messenger.Default.Send<SelectedLayerAndElement>(SelectedLayerAndElementValue, MsgToken.MSG_SELECTEDLAYERANDELEMENT_CHANGED);
            e.Handled = true;
        }
        private void _itemsControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MyRectLayer.ElementCollection.Count == 0)
            {
                e.Handled = true;
                return;
            }
            #region 添加接收卡
            ObservableCollection<IElement> addCollection = new ObservableCollection<IElement>();
            ObservableCollection<ElementSizeInfo> elementSizeCollection = new ObservableCollection<ElementSizeInfo>();
            if (_addReceiveInfo != null && MyRectLayer.ElementCollection.Count != 0 && _myScreenLayer.OperateEnviron == OperatEnvironment.DesignScreen)
            {
                _selectedElementCollection.Clear();
                //取消所有元素的选中态
                for (int i = 0; i < _myScreenLayer.ElementCollection.Count; i++)
                {
                    if (_myScreenLayer.ElementCollection[i].EleType == ElementType.receive && _myScreenLayer.ElementCollection[i].ElementSelectedState != SelectedState.None)
                    {
                        _myScreenLayer.ElementCollection[i].ElementSelectedState = SelectedState.None;
                    }
                }
                Point pointInCanvas = e.GetPosition(_itemsControl);
                pointInCanvas.X -= _myScreenLayer.X;
                pointInCanvas.Y -= _myScreenLayer.Y;
                int cols = _addReceiveInfo.Cols;
                int rows = _addReceiveInfo.Rows;
                ScannerCofigInfo scanConfig = _addReceiveInfo.ScanConfig;
                ScanBoardProperty scanBdProp = scanConfig.ScanBdProp;
                double width = scanBdProp.Width;
                double height = scanBdProp.Height;
                RectElement groupframe;
                if (cols == 1 && rows == 1)
                {
                    groupframe = new RectElement(Math.Round(pointInCanvas.X - width / 2), Math.Round(pointInCanvas.Y - height / 2), width * cols, height * rows, null, -1);
                    RectElement rectElement = new RectElement(Math.Round(pointInCanvas.X - width / 2), Math.Round(pointInCanvas.Y - height / 2), width * cols, height * rows, _myScreenLayer, _myScreenLayer.MaxZorder + 1);
                    rectElement.EleType = ElementType.receive;
                    _myScreenLayer.MaxZorder += 1;
                    rectElement.Tag = scanConfig.Clone();
                    rectElement.IsLocked = true;
                    rectElement.ZIndex = 4;
                    rectElement.GroupName = -1;
                    rectElement.ElementSelectedState = SelectedState.Selected;
                    _selectedElementCollection.Add(rectElement);
                    _myScreenLayer.ElementCollection.Add(rectElement);
                    addCollection.Add(rectElement);

                }
                else
                {
                    groupframe = new RectElement(Math.Round(pointInCanvas.X - width / 2), Math.Round(pointInCanvas.Y - height / 2), width * cols, height * rows, _myScreenLayer, _myScreenLayer.MaxZorder + 1);
                    groupframe.EleType = ElementType.groupframe;
                    groupframe.ZIndex = 5;
                    groupframe.GroupName = _myScreenLayer.MaxGroupName + 1;
                    _myScreenLayer.MaxGroupName += 1;
                    _myScreenLayer.MaxZorder += 1;
                    groupframe.ElementSelectedState = SelectedState.SelectedAll;
                    _groupframeList.Add(groupframe.GroupName, groupframe);
                    for (int i = 0; i < cols; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {
                            RectElement rectElement = new RectElement(width * i + groupframe.X, height * j + groupframe.Y, width, height, _myScreenLayer, _myScreenLayer.MaxZorder + 1);
                            rectElement.EleType = ElementType.receive;
                            _myScreenLayer.MaxZorder += 1;
                            rectElement.Tag = scanConfig.Clone();
                            rectElement.IsLocked = true;
                            rectElement.ZIndex = 4;
                            rectElement.GroupName = groupframe.GroupName;
                            rectElement.ElementSelectedState = SelectedState.Selected;
                            _selectedElementCollection.Add(rectElement);
                            _myScreenLayer.ElementCollection.Add(rectElement);
                            addCollection.Add(rectElement);
                        }
                    }
                    _myScreenLayer.ElementCollection.Add(groupframe);
                    addCollection.Add(groupframe);

                }

                ElementSizeInfo info = new ElementSizeInfo();
                info.Element = _myScreenLayer;
                info.OldSize = new Size(_myScreenLayer.Width, _myScreenLayer.Height);
                bool isChangedSize = false;
                if (groupframe.X + groupframe.Width > _myScreenLayer.Width)
                {
                    _myScreenLayer.Width = groupframe.X + groupframe.Width;
                    isChangedSize = true;
                }
                if (groupframe.Y + groupframe.Height > _myScreenLayer.Height)
                {
                    _myScreenLayer.Height = groupframe.Y + groupframe.Height;
                    isChangedSize = true;
                }
                SelectedElementChangedHandle(MouseState.None);
                //记录
                if (isChangedSize)
                {
                    info.NewSize = new Size(_myScreenLayer.Width, _myScreenLayer.Height);
                    elementSizeCollection.Add(info);
                    ElementSizeAction elementSizeAction = new ElementSizeAction(elementSizeCollection);
                    AddAction addAction = new AddAction(_myScreenLayer, addCollection);
                    using (Transaction.Create(SmartLCTActionManager))
                    {
                        SmartLCTActionManager.RecordAction(elementSizeAction);
                        SmartLCTActionManager.RecordAction(addAction);
                    }
                }
                else
                {
                    AddAction addAction = new AddAction(_myScreenLayer, addCollection);
                    SmartLCTActionManager.RecordAction(addAction);
                } 
                e.Handled = true;
                return;
            }
            #endregion

            if (_myScreenLayer != null)
            {
                Function.SetElementCollectionState(_myScreenLayer.ElementCollection, SelectedState.None);
            }
            if (MyRectLayer == null || MyRectLayer.ElementCollection.Count == 0)
            {
                return;
            }
            _myScreenLayer = (RectLayer)MyRectLayer.ElementCollection[0];
            _mousePoint = this.TranslatePoint(e.GetPosition(this), _itemsControl);
            CurrentRectElement = null;
            _isMouseLeftButtonDown = true;
            GetCurrentElementInfo();

            SelectedLayerAndElementValue.SelectedLayer = _myScreenLayer;
            SelectedLayerAndElementValue.GroupframeList = _groupframeList;
            SelectedLayerAndElementValue.SelectedInfoList = _currentSelectedElement;
            SelectedLayerAndElementValue.MainControlSize = new Size(this.ActualWidth, this.ActualHeight);
            Messenger.Default.Send<SelectedLayerAndElement>(SelectedLayerAndElementValue, MsgToken.MSG_SELECTEDLAYERANDELEMENT_CHANGED);
            //e.Handled = true;
        }
        private void _itemsControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_addReceiveInfo != null)
            {
                e.Handled = false;
                return;
            }
            if (_myScreenLayer != null && _myScreenLayer.CLineType == ConnectLineType.Manual)
            {
                this.Cursor = Cursors.Hand;
                return;
            }
            ObservableCollection<IElement> selectedElements = new ObservableCollection<IElement>();
            List<IElement> selectedElementList = new List<IElement>();

            Point mousePoint = this.TranslatePoint(e.GetPosition(this), _itemsControl);
            if (!_isMouseLeftButtonDown)
            {
                return;
            }
        }
        private void _itemsControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            for (int elementIndex = 0; elementIndex < MyRectLayer.ElementCollection.Count; elementIndex++)
            {
                if (MyRectLayer.ElementCollection[elementIndex] is RectLayer)
                {
                    continue;
                }
                if (((RectElement)MyRectLayer.ElementCollection[elementIndex]).ZOrder == -1)
                {
                    #region 选中框选的元素
                    RectElement rect1 = ((RectElement)MyRectLayer.ElementCollection[elementIndex]);
                    int index = 0;
                    for (int i = 0; i < MyRectLayer.ElementCollection.Count; i++)
                    {
                        if (MyRectLayer.ElementCollection[i] is RectLayer)
                        {
                            continue;
                        }
                        RectElement rect2 = ((RectElement)MyRectLayer.ElementCollection[i]);

                        if (Function.IsRectIntersect(rect1, rect2))
                        {
                            index = index + 1;
                            ((RectElement)MyRectLayer.ElementCollection[i]).ElementSelectedState = SelectedState.Selected;
                            //if (index == 1)
                            //{
                            //    ((RectElement)MyRectLayer.ElementCollection[i]).ElementSelectedState = SelectedState.SpecialSelected;
                            //}
                        }
                    }
                    #endregion
                    MyRectLayer.ElementCollection.RemoveAt(elementIndex);
                }
            }
            _isMouseLeftButtonDown = false;
            _isLayerMouseLeftButtonDown = false;

            Messenger.Default.Send<bool>(false, MsgToken.MSG_MOUSEUP);
        }
        private void _itemsControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if ((Keyboard.GetKeyStates(Key.LeftCtrl) == KeyStates.Down || (Keyboard.GetKeyStates(Key.LeftCtrl) == (KeyStates.Down | KeyStates.Toggled))) ||
            (Keyboard.GetKeyStates(Key.RightCtrl) == KeyStates.Down || (Keyboard.GetKeyStates(Key.RightCtrl) == (KeyStates.Down | KeyStates.Toggled))))
            {
                if (e.Delta > 0)
                {
                    SetDecrease();
                }
                else if (e.Delta < 0)
                {
                    SetIncrease();
                }
                e.Handled = true;
            }
        }

        private void Receive_MouseLeftButtonDown(object sender, MouseButtonEventArgs e, IElement element)
        {
            if (element is LineElement)
            {
                ILineElement lineElement = (LineElement)element;
                IRectElement frontElment = lineElement.FrontElement;
                IRectElement endElement = lineElement.EndElement;
                Point mousePointInLine = e.GetPosition(_itemsControl);
                int clickNum = 0;
                if(mousePointInLine.X<=frontElment.X+frontElment.Width && mousePointInLine.X>=frontElment.X &&
                    mousePointInLine.Y <= frontElment.Y + frontElment.Height && mousePointInLine.Y >= frontElment.Y)
                {
                    element = frontElment;
                    clickNum += 1;
                }
                else if (mousePointInLine.X <= endElement.X + endElement.Width && mousePointInLine.X >= endElement.X &&
                    mousePointInLine.Y <= endElement.Y + endElement.Height && mousePointInLine.Y >= endElement.Y)
                {
                    element = endElement;
                    clickNum += 1;
                }
                if (clickNum == 2)
                {
                    if (frontElment.ZIndex > endElement.ZIndex)
                    {
                        element = frontElment;
                    }
                    else if(frontElment.ZIndex<endElement.ZIndex)
                    {
                        element = endElement;
                    }
                }
                if (clickNum == 0)
                {
                    e.Handled = true;
                    return;
                }
      
            }
            //if (element is RectLayer)
            //{
            //    Console.WriteLine("layer");
            //}
            //if (element is RectElement)
            //{
            //    Console.WriteLine("rect");
            //}
            if (element != null && element.EleType == ElementType.receive)
            {
                CurrentRectElement = (RectElement)element;
            }
            else
            {
                CurrentRectElement = null;
            }
            if (((IRectElement)element).OperateEnviron == OperatEnvironment.AdjustSenderLocation && element.EleType == ElementType.screen)
            {
                e.Handled = true;
                return;
            }
            if (MyRectLayer.ElementCollection.Count == 0)
            {
                e.Handled = true;
                return;
            }
            _myScreenLayer = (RectLayer)MyRectLayer.ElementCollection[0];
            UpdateGroupframeList();
            #region 添加接收卡
            ObservableCollection<IElement> addCollection = new ObservableCollection<IElement>();
            ObservableCollection<ElementSizeInfo> elementSizeCollection = new ObservableCollection<ElementSizeInfo>();
            if (_addReceiveInfo != null && _myScreenLayer.OperateEnviron == OperatEnvironment.DesignScreen)
            {
                this.Cursor = Cursors.Wait;
                _selectedElementCollection.Clear();
                //取消所有元素的选中态
                for (int i = 0; i < _myScreenLayer.ElementCollection.Count; i++)
                {
                    if (_myScreenLayer.ElementCollection[i].EleType == ElementType.receive && _myScreenLayer.ElementCollection[i].ElementSelectedState != SelectedState.None)
                    {
                        _myScreenLayer.ElementCollection[i].ElementSelectedState = SelectedState.None;
                    }
                }
                Point pointInCanvas = e.GetPosition(_itemsControl);
                pointInCanvas.X -= _myScreenLayer.X;
                pointInCanvas.Y -= _myScreenLayer.Y;
                int cols = _addReceiveInfo.Cols;
                int rows = _addReceiveInfo.Rows;
                ScannerCofigInfo scanConfig = _addReceiveInfo.ScanConfig;
                ScanBoardProperty scanBdProp = scanConfig.ScanBdProp;
                double width = scanBdProp.Width;
                double height = scanBdProp.Height;
                RectElement groupframe;
                if (cols == 1 && rows == 1)
                {
                    groupframe = new RectElement(Math.Round(pointInCanvas.X - width / 2), Math.Round(pointInCanvas.Y - height / 2), width * cols, height * rows, null, -1);
                    RectElement rectElement = new RectElement(Math.Round(pointInCanvas.X - width / 2), Math.Round(pointInCanvas.Y - height / 2), width * cols, height * rows, _myScreenLayer, _myScreenLayer.MaxZorder + 1);
                    rectElement.EleType = ElementType.receive;
                    _myScreenLayer.MaxZorder += 1;
                    rectElement.Tag = scanConfig.Clone();
                    rectElement.IsLocked = true;
                    rectElement.ZIndex = 4;
                    rectElement.GroupName = -1;
                    rectElement.ElementSelectedState = SelectedState.Selected;
                    _selectedElementCollection.Add(rectElement);
                    _myScreenLayer.ElementCollection.Add(rectElement);
                    addCollection.Add(rectElement);

                }
                else
                {
                    groupframe = new RectElement(Math.Round(pointInCanvas.X - width / 2), Math.Round(pointInCanvas.Y - height / 2), width * cols, height * rows, _myScreenLayer, _myScreenLayer.MaxZorder + 1);
                    groupframe.EleType = ElementType.groupframe;
                    groupframe.ZIndex = 5;
                    groupframe.GroupName = _myScreenLayer.MaxGroupName + 1;
                    _myScreenLayer.MaxGroupName += 1;
                    _myScreenLayer.MaxZorder += 1;
                    groupframe.ElementSelectedState = SelectedState.SelectedAll;
                    _groupframeList.Add(groupframe.GroupName, groupframe);
                    for (int i = 0; i < cols; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {
                            RectElement rectElement = new RectElement(width * i + groupframe.X, height * j + groupframe.Y, width, height, _myScreenLayer, _myScreenLayer.MaxZorder + 1);
                            rectElement.EleType = ElementType.receive;
                            _myScreenLayer.MaxZorder += 1;
                            rectElement.Tag = scanConfig.Clone();
                            rectElement.IsLocked = true;
                            rectElement.ZIndex = 4;
                            rectElement.GroupName = groupframe.GroupName;
                            rectElement.ElementSelectedState = SelectedState.Selected;
                            _selectedElementCollection.Add(rectElement);
                            _myScreenLayer.ElementCollection.Add(rectElement);
                            addCollection.Add(rectElement);
                        }
                    }
                    _myScreenLayer.ElementCollection.Add(groupframe);
                    addCollection.Add(groupframe);

                }

                ElementSizeInfo info = new ElementSizeInfo();
                info.Element = _myScreenLayer;
                info.OldSize = new Size(_myScreenLayer.Width, _myScreenLayer.Height);
                bool isChangedSize = false;
                if (groupframe.X + groupframe.Width > _myScreenLayer.Width)
                {
                    _myScreenLayer.Width = groupframe.X + groupframe.Width;
                    isChangedSize = true;
                }
                if (groupframe.Y + groupframe.Height > _myScreenLayer.Height)
                {
                    _myScreenLayer.Height = groupframe.Y + groupframe.Height;
                    isChangedSize = true;
                }
                SelectedElementChangedHandle(MouseState.None);
                //记录
                if (isChangedSize)
                {
                    info.NewSize = new Size(_myScreenLayer.Width, _myScreenLayer.Height);
                    elementSizeCollection.Add(info);
                    ElementSizeAction elementSizeAction = new ElementSizeAction(elementSizeCollection);
                    AddAction addAction = new AddAction(_myScreenLayer, addCollection);
                    using (Transaction.Create(SmartLCTActionManager))
                    {
                        SmartLCTActionManager.RecordAction(elementSizeAction);
                        SmartLCTActionManager.RecordAction(addAction);
                    }
                }
                else
                {
                    AddAction addAction = new AddAction(_myScreenLayer, addCollection);
                    SmartLCTActionManager.RecordAction(addAction);
                }
                e.Handled = true;
                return;
            }
            #endregion

            #region 空格+鼠标移动屏体
            if ((Keyboard.GetKeyStates(Key.Space) == KeyStates.Down || (Keyboard.GetKeyStates(Key.Space) == (KeyStates.Down | KeyStates.Toggled))))
            {
                _itemsControl.Cursor = _moveCursor;
                Layer_MouseLeftButtonDown(sender, e, _myScreenLayer);
                e.Handled = true;
                return;
            }
            #endregion
            #region 手动鼠标点击连线
            if (element.EleType == ElementType.receive)
            {
                if (_myScreenLayer.CLineType == ConnectLineType.Manual)
                {
                    if (element.ConnectedIndex >= 0)
                    {
                        e.Handled = true;
                        return;
                    }
                    ObservableCollection<IElement> addElement = new ObservableCollection<IElement>();
                    ObservableCollection<AddLineInfo> addElementInfo = new ObservableCollection<AddLineInfo>();
                    ObservableCollection<ConnectIconVisibilityInfo> connectIconVisibleCollection = new ObservableCollection<ConnectIconVisibilityInfo>();

                    //更新map
                    #region 连线前发送卡的带载
                    int senderIndex = _myScreenLayer.CurrentSenderIndex;
                    int portIndex = _myScreenLayer.CurrentPortIndex;
                    bool isSenderLoadSizeNull = false;//第一次添加该发送卡的数据
                    if (_myScreenLayer.SenderConnectInfoList[senderIndex].LoadSize.Height == 0 &&
                        _myScreenLayer.SenderConnectInfoList[senderIndex].LoadSize.Width == 0)
                    {
                        isSenderLoadSizeNull = true;
                    }
                    Rect oldSenderLoadSize = new Rect();
                    Point oldSenderPoint = new Point();
                    Point oldMap = new Point();
                    if (!isSenderLoadSizeNull)
                    {
                        oldSenderLoadSize = _myScreenLayer.SenderConnectInfoList[senderIndex].LoadSize;
                        oldSenderPoint = new Point(oldSenderLoadSize.X, oldSenderLoadSize.Y);
                        //原来的map
                        oldMap = new Point();
                        for (int j = 0; j < _myScreenLayer.SenderConnectInfoList[_myScreenLayer.CurrentSenderIndex].PortConnectInfoList.Count; j++)
                        {
                            if (_myScreenLayer.SenderConnectInfoList[_myScreenLayer.CurrentSenderIndex].PortConnectInfoList[j].LoadSize.Width != 0 &&
                                _myScreenLayer.SenderConnectInfoList[_myScreenLayer.CurrentSenderIndex].PortConnectInfoList[j].LoadSize.Height != 0)
                            {
                                oldMap = _myScreenLayer.SenderConnectInfoList[_myScreenLayer.CurrentSenderIndex].PortConnectInfoList[j].MapLocation;
                                break;
                            }
                        }
                    }
                    #endregion

                    #region 记录相关
                    //记下该元素的前一个元素的最大和最小连线图标是否显示
                    IRectElement maxConnectElement = _myScreenLayer.SenderConnectInfoList[_myScreenLayer.CurrentSenderIndex].PortConnectInfoList[_myScreenLayer.CurrentPortIndex].MaxConnectElement;
                    if (maxConnectElement != null)
                    {
                        ConnectIconVisibilityInfo info = new ConnectIconVisibilityInfo();
                        info.Element = maxConnectElement;
                        OldAndNewVisibility oldAndNewMaxConnectIndexVisibile = new OldAndNewVisibility();
                        oldAndNewMaxConnectIndexVisibile.OldValue = maxConnectElement.MaxConnectIndexVisibile;
                        OldAndNewVisibility oldAndNewMinConnectIndexVisibile = new OldAndNewVisibility();
                        oldAndNewMinConnectIndexVisibile.OldValue = maxConnectElement.MinConnectIndexVisibile;
                        info.OldAndNewMaxConnectIndexVisibile = oldAndNewMaxConnectIndexVisibile;
                        info.OldAndNewMinConnectIndexVisibile = oldAndNewMinConnectIndexVisibile;
                        connectIconVisibleCollection.Add(info);
                    }
                    #endregion
                    
                    int connectIndex = _myScreenLayer.SenderConnectInfoList[_myScreenLayer.CurrentSenderIndex].PortConnectInfoList[_myScreenLayer.CurrentPortIndex].MaxConnectIndex + 1;
                    
                    ((RectElement)element).SenderIndex = _myScreenLayer.CurrentSenderIndex;
                    ((RectElement)element).PortIndex = _myScreenLayer.CurrentPortIndex;
                    element.ConnectedIndex = connectIndex;

                    #region 记录相关
                    //记下连线后最大和最小连线图标是否显示
                    for (int itemIndex = 0; itemIndex < connectIconVisibleCollection.Count; itemIndex++)
                    {
                        connectIconVisibleCollection[itemIndex].OldAndNewMaxConnectIndexVisibile.NewValue = connectIconVisibleCollection[itemIndex].Element.MaxConnectIndexVisibile;
                        connectIconVisibleCollection[itemIndex].OldAndNewMinConnectIndexVisibile.NewValue = connectIconVisibleCollection[itemIndex].Element.MinConnectIndexVisibile;
                    }
                    //记下该元素最大和最小连线图标是否显示
                    ConnectIconVisibilityInfo connectIconinfo = new ConnectIconVisibilityInfo();
                    connectIconinfo.Element = (IRectElement)element;
                    OldAndNewVisibility oldAndNewMaxConnectIconVisibile = new OldAndNewVisibility();
                    oldAndNewMaxConnectIconVisibile.OldValue = Visibility.Hidden;
                    oldAndNewMaxConnectIconVisibile.NewValue = ((IRectElement)element).MaxConnectIndexVisibile;
                    OldAndNewVisibility oldAndNewMinConnectIconVisibile = new OldAndNewVisibility();
                    oldAndNewMinConnectIconVisibile.OldValue = Visibility.Hidden;
                    oldAndNewMinConnectIconVisibile.NewValue = ((IRectElement)element).MinConnectIndexVisibile;
                    connectIconinfo.OldAndNewMaxConnectIndexVisibile = oldAndNewMaxConnectIconVisibile;
                    connectIconinfo.OldAndNewMinConnectIndexVisibile = oldAndNewMinConnectIconVisibile;
                    connectIconVisibleCollection.Add(connectIconinfo);
                    #endregion

                    #region  连线后发送卡的带载
                    if (_myScreenLayer.IsStartSetMapLocation && (!isSenderLoadSizeNull))
                        {
                            //更新该发送卡的带载
                            Rect newSenderLoadSize = _myScreenLayer.SenderConnectInfoList[_myScreenLayer.CurrentSenderIndex].LoadSize;
                            Point newSenderPoint = new Point(newSenderLoadSize.X, newSenderLoadSize.Y);
                            Point difSenderPint = new Point(newSenderLoadSize.X - oldSenderPoint.X, newSenderLoadSize.Y - oldSenderPoint.Y);
                            //发送卡带载的xy有变化的话更新该发送卡下所有网口的Map
                            if (difSenderPint.Y != 0 || difSenderPint.X != 0)
                            {
                                Point newmapLocation = new Point(oldMap.X + difSenderPint.X, oldMap.Y + difSenderPint.Y);
                                if (newmapLocation.X < 0)
                                {
                                    newmapLocation.X = 0;
                                }
                                if (newmapLocation.Y < 0)
                                {
                                    newmapLocation.Y = 0;
                                }
                                //更新网口的map
                                for (int j = 0; j < _myScreenLayer.SenderConnectInfoList[senderIndex].PortConnectInfoList.Count; j++)
                                {
                                    PortConnectInfo portConnect = _myScreenLayer.SenderConnectInfoList[senderIndex].PortConnectInfoList[j];
                                    if (portConnect.LoadSize.Height != 0 && portConnect.LoadSize.Width != 0)
                                    {
                                        portConnect.MapLocation = newmapLocation;
                                    }
                                }
                            }
                            if (difSenderPint.X == 0 && difSenderPint.Y == 0)
                            {
                                _myScreenLayer.SenderConnectInfoList[senderIndex].PortConnectInfoList[portIndex].MapLocation = oldMap;
                            }
                        }
                    #endregion

                    #region 记录相关
                    if (((IRectElement)element).FrontLine != null)
                    {
                        addElement.Add(((IRectElement)element).FrontLine);
                    }

                    AddLineInfo addInfo = new AddLineInfo();
                    addInfo.Element = (IRectElement)element;
                    OldAndNewType oldAndNewConnect = new OldAndNewType();
                    oldAndNewConnect.NewValue = element.ConnectedIndex;
                    oldAndNewConnect.OldValue = -1;
                    OldAndNewType oldAndNewSender = new OldAndNewType();
                    oldAndNewSender.NewValue = ((IRectElement)element).SenderIndex;
                    oldAndNewSender.OldValue = -1;
                    OldAndNewType oldAndNewPort = new OldAndNewType();
                    oldAndNewPort.NewValue = ((IRectElement)element).PortIndex;
                    oldAndNewPort.OldValue = -1;
                    addInfo.OldAndNewConnectIndex = oldAndNewConnect;
                    addInfo.OldAndNewSenderIndex = oldAndNewSender;
                    addInfo.OldAndNewPortIndex = oldAndNewPort;
                    addElementInfo.Add(addInfo);

                    AddAction addAction = new AddAction(_myScreenLayer, addElement);
                    AddLineAction addLineAction = new AddLineAction(addElementInfo);
                    ConnectIconVisibilityAction connectIconVisibleAction = new ConnectIconVisibilityAction(connectIconVisibleCollection);
                    using (Transaction.Create(SmartLCTActionManager))
                    {
                        SmartLCTActionManager.RecordAction(addAction);
                        SmartLCTActionManager.RecordAction(addLineAction);
                        SmartLCTActionManager.RecordAction(connectIconVisibleAction);
                    }
                    #endregion
                    e.Handled = true;
                    return;
                }

            }
            #endregion
            #region 条件
            FrameworkElement currentFramework = (FrameworkElement)sender;
            _mousePoint = e.GetPosition(currentFramework);
            Point mouseLeftDownPointInThis=this.TranslatePoint(e.GetPosition(this), _itemsControl);
            _mouseLeftDownPointInThis = new Point(mouseLeftDownPointInThis.X,mouseLeftDownPointInThis.Y);
            
            _copyPoint = _mouseLeftDownPointInThis;

            _isMouseLeftButtonDown = true;
            _isStartMoveX = true;
            _isStartMoveY = true;
            #endregion
            GetCurrentElementInfo();

            #region 点击图层和矩形的处理
            ObservableCollection<SelectedStateInfo> oldSelectedStateInfoCollection = new ObservableCollection<SelectedStateInfo>();
            oldSelectedStateInfoCollection = GetOldSelectedStateInfoCollection();
            if (element.EleType == ElementType.screen)
            {
                #region 点击图层的处理
                ((RectLayer)element).CLineType = ConnectLineType.Auto;
                this.Cursor = Cursors.Arrow;
                foreach (int elementIndex in _currentSelectedElement.Keys)
                {
                    ObservableCollection<IRectElement> selectedGroupList = _currentSelectedElement[elementIndex].SelectedGroupElementList;
                    for (int i = 0; i < selectedGroupList.Count; i++)
                    {
                        selectedGroupList[i].ElementSelectedState = SelectedState.None;
                        if (selectedGroupList[i].EleType == ElementType.receive)
                        {
                            selectedGroupList[i].IsLocked = true;
                        }
                    }
                }
                #endregion
            }
            else if (element.EleType == ElementType.receive || element.EleType == ElementType.groupframe)
            {
                #region 展开
                RectLayer myScreen = MyRectLayer;
                while (myScreen != null && myScreen.EleType != ElementType.baseScreen)
                {
                    myScreen = (RectLayer)myScreen.ParentElement;
                }
                if (myScreen != null && myScreen.SenderConnectInfoList != null &&
               myScreen.SenderConnectInfoList.Count > 0)
                {
                    for (int m = 0; m < myScreen.SenderConnectInfoList.Count; m++)
                    {
                        bool isexpand = false;
                        ObservableCollection<PortConnectInfo> portConnectList = myScreen.SenderConnectInfoList[m].PortConnectInfoList;
                        if (portConnectList != null)
                        {
                            for (int i = 0; i < portConnectList.Count; i++)
                            {
                                if (portConnectList[i].IsOverLoad)
                                {
                                    isexpand = true;
                                    break;
                                }
                            }
                        }
                        if (isexpand)
                        {
                            myScreen.SenderConnectInfoList[m].IsExpand = true;
                        }
                        else
                        {
                            myScreen.SenderConnectInfoList[m].IsExpand = false;
                        }
                    }
                }

                if (myScreen != null && ((IRectElement)element).SenderIndex >= 0 && myScreen.SenderConnectInfoList != null &&
                    myScreen.SenderConnectInfoList.Count > 0 &&
                    myScreen.SenderConnectInfoList.Count >= ((IRectElement)element).SenderIndex)
                {
                    myScreen.SenderConnectInfoList[((IRectElement)element).SenderIndex].IsExpand = true;
                    myScreen.SenderConnectInfoList[((IRectElement)element).SenderIndex].PortConnectInfoList[((IRectElement)element).PortIndex].IsSelected = true;
                }
                #endregion
                if ((Keyboard.GetKeyStates(Key.LeftCtrl) == KeyStates.Down || (Keyboard.GetKeyStates(Key.LeftCtrl) == (KeyStates.Down | KeyStates.Toggled))) ||
                    (Keyboard.GetKeyStates(Key.RightCtrl) == KeyStates.Down || (Keyboard.GetKeyStates(Key.RightCtrl) == (KeyStates.Down | KeyStates.Toggled))))
                {
                    #region 按ctrl键
                    if (element.ElementSelectedState == SelectedState.None)
                    {
                        element.ElementSelectedState = SelectedState.Selected;
                    }
                    else if (element.ElementSelectedState != SelectedState.None)
                    {
                        element.ElementSelectedState = SelectedState.None;
                    }
                    #endregion
                }
                else
                {
                    #region 不按ctrl键
                    if (element.ElementSelectedState == SelectedState.None)
                    {
                        Function.SetElementCollectionState(_selectedElementCollection, SelectedState.None);
                        if (element.GroupName != -1)
                        {
                            ObservableCollection<IRectElement> noSelectedGroupElement = _currentSelectedElement[element.GroupName].NoSelectedGroupElementList;
                            ObservableCollection<IRectElement> selectedGroupElement = _currentSelectedElement[element.GroupName].SelectedGroupElementList;
                            Function.SetElementCollectionState(noSelectedGroupElement, SelectedState.Selected);
                            Function.SetElementCollectionState(selectedGroupElement, SelectedState.Selected);
                        }
                        element.ElementSelectedState = SelectedState.Selected;
                    }
                    #endregion
                }
            }
            #endregion
            GetCurrentElementInfo();

            RecordBeforeMoveData();
            SelectedElementChangedHandle(MouseState.None);
            e.Handled = true;
        }
        private void Receive_MouseMove(object sender, MouseEventArgs e, IElement element)
        {
            if (element is LineElement)
            {
                ILineElement lineElement = (LineElement)element;
                IRectElement frontElment = lineElement.FrontElement;
                IRectElement endElement = lineElement.EndElement;
                Point mousePointInLine = e.GetPosition(_itemsControl);
                int clickNum = 0;
                if (mousePointInLine.X <= frontElment.X + frontElment.Width && mousePointInLine.X >= frontElment.X &&
                    mousePointInLine.Y <= frontElment.Y + frontElment.Height && mousePointInLine.Y >= frontElment.Y)
                {
                    element = frontElment;
                    clickNum += 1;
                }
                else if (mousePointInLine.X <= endElement.X + endElement.Width && mousePointInLine.X >= endElement.X &&
                    mousePointInLine.Y <= endElement.Y + endElement.Height && mousePointInLine.Y >= endElement.Y)
                {
                    element = endElement;
                    clickNum += 1;
                }
                if (clickNum == 2)
                {
                    if (frontElment.ZIndex > endElement.ZIndex)
                    {
                        element = frontElment;
                    }
                    else if (frontElment.ZIndex < endElement.ZIndex)
                    {
                        element = endElement;
                    }
                }
                if (clickNum == 0)
                {
                    e.Handled = true;
                    return;
                }

            }
            FrameworkElement currentFramework = (FrameworkElement)sender;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                currentFramework.CaptureMouse();
            }
            else
            {
                currentFramework.ReleaseMouseCapture();
                if (this._adorner != null || (_addReceiveInfo != null && _myScreenLayer.OperateEnviron == OperatEnvironment.DesignScreen))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
                return;
            }

            if (this._adorner != null)
            {
                e.Handled = false;
                return;
            }
            else
            {
                if ((Keyboard.GetKeyStates(Key.LeftCtrl) == KeyStates.Down || (Keyboard.GetKeyStates(Key.LeftCtrl) == (KeyStates.Down | KeyStates.Toggled))) ||
       (Keyboard.GetKeyStates(Key.RightCtrl) == KeyStates.Down || (Keyboard.GetKeyStates(Key.RightCtrl) == (KeyStates.Down | KeyStates.Toggled))))
                {
                }
                else if (_isMouseLeftButtonDown == true && element.EleType == ElementType.receive && element.ElementSelectedState != SelectedState.None)
                {
                    //虚构选中箱体的区域
                    _selectedReceiveArea = new Rect();
                    if (_selectedElementCollection != null && _selectedElementCollection.Count != 0)
                    {
                        double changedValue = Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex);
                        #region 1.计算选中箱体的区域
                        foreach (int groupName in _currentSelectedElement.Keys)
                        {
                            Rect currentSelectedRect = _currentSelectedElement[groupName].SelectedElementRect;
                            if (currentSelectedRect.Height != 0 && currentSelectedRect.Width != 0)
                            {
                                if (_selectedReceiveArea == new Rect())
                                {
                                    _selectedReceiveArea = currentSelectedRect;
                                }
                                else
                                {
                                    _selectedReceiveArea = Rect.Union(_selectedReceiveArea, currentSelectedRect);
                                }
                            }
                        }
                        #endregion
                        #region 2.生成虚构的箱体移动区域
                        //拖动的区域
                        this._dragScope = _itemsControl as FrameworkElement;

                        //是否可以拖放
                        this._dragScope.AllowDrop = true;
                        draghandler = new MouseEventHandler(DragScope_PreviewDragOver);
                        //加载处理拖放的路由事件
                        this._dragScope.MouseMove += draghandler;
                        this._dragScope.PreviewMouseMove += draghandler;
                        //鼠标跟随效果的装饰器
                        Point adornerMousePoint = new Point();
                        adornerMousePoint = e.GetPosition(this);
                        adornerMousePoint = this.TranslatePoint(adornerMousePoint, _itemsControl);
                        //已经是缩放后的真实值
                        _virtualMoveDifValue = new Point(adornerMousePoint.X - _selectedReceiveArea.X, adornerMousePoint.Y - _selectedReceiveArea.Y);
                        if(_myScreenLayer.OperateEnviron==OperatEnvironment.AdjustScreenLocation ||
                            _myScreenLayer.OperateEnviron == OperatEnvironment.AdjustSenderLocation)
                        {
                            _virtualMoveDifValue.X -= _myScreenLayer.X;
                            _virtualMoveDifValue.Y -= _myScreenLayer.Y;
                        }
                        //_virtualMoveDifValue.X = Math.Round(_virtualMoveDifValue.X);
                        //_virtualMoveDifValue.Y = Math.Round(_virtualMoveDifValue.Y);
                        this._adorner = new DragAdorner(this._dragScope, _selectedReceiveArea.Width, _selectedReceiveArea.Height, adornerMousePoint, 0.5);
                        //装饰器的位置
                        this._adorner.LeftOffset = _selectedReceiveArea.X * changedValue;
                        this._adorner.TopOffset = _selectedReceiveArea.Y * changedValue;
                        if(_myScreenLayer.OperateEnviron==OperatEnvironment.AdjustSenderLocation ||
                            _myScreenLayer.OperateEnviron == OperatEnvironment.AdjustScreenLocation)
                        {
                            this._adorner.LeftOffset += _myScreenLayer.X*changedValue;
                            this._adorner.TopOffset += _myScreenLayer.Y*changedValue;
                        }
                        this._adorner.Height = _selectedReceiveArea.Height;
                        this._adorner.Width = _selectedReceiveArea.Width;

                        this._layer = AdornerLayer.GetAdornerLayer(this._dragScope as Visual);
                        this._layer.Add(this._adorner);


                        //显示位置的装饰器
                        this._locationAdorner = new DragAdorner(this._dragScope, this._adorner);
                        //this._locationAdorner.TopOffset = adornerMousePoint.Y-20;
                        //this._locationAdorner.LeftOffset = adornerMousePoint.X;
                        this._locationAdorner.LeftOffset = _selectedReceiveArea.X * changedValue;
                        this._locationAdorner.TopOffset = _selectedReceiveArea.Y * changedValue;
                        if (_myScreenLayer.OperateEnviron == OperatEnvironment.AdjustSenderLocation ||
                            _myScreenLayer.OperateEnviron == OperatEnvironment.AdjustScreenLocation)
                        {
                            this._locationAdorner.LeftOffset += _myScreenLayer.X*changedValue;
                            this._locationAdorner.TopOffset += _myScreenLayer.Y*changedValue;
                        }
                        this._layer.Add(this._locationAdorner);

                        return;
                        #endregion
                    }
                }
            }
            if (_addReceiveInfo != null && _myScreenLayer.OperateEnviron == OperatEnvironment.DesignScreen)
            {
                e.Handled = false;
                return;
            }
            if (_myScreenLayer != null && _myScreenLayer.CLineType == ConnectLineType.Manual)
            {
                this.Cursor = Cursors.Hand;
                e.Handled = true;
                return;
            }
            #region 空格+鼠标移动屏体
            if ((Keyboard.GetKeyStates(Key.Space) == KeyStates.Down || (Keyboard.GetKeyStates(Key.Space) == (KeyStates.Down | KeyStates.Toggled))))
            {
                Layer_MouseMove(sender, e, _myScreenLayer);
                e.Handled = true;
                return;
            }
            #endregion

            #region 条件

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                currentFramework.CaptureMouse();
            }
            else
            {
                currentFramework.ReleaseMouseCapture();
                e.Handled = true;
                return;
            }
            List<IElement> selectedElementList = new List<IElement>();
            bool isMoveX = true;
            bool isMoveY = true;
            Point mousePoint = e.GetPosition(currentFramework);
            if (element.EleType == ElementType.receive && element.ElementSelectedState == SelectedState.None)
            {
                if (Keyboard.GetKeyStates(Key.LeftCtrl) == KeyStates.Down || Keyboard.GetKeyStates(Key.LeftCtrl) == (KeyStates.Down | KeyStates.Toggled))
                {
                    //有可能是框选
                }
                else
                {
                    e.Handled = true;
                    return;
                }

            }

            if ((!_isMouseLeftButtonDown) || element.ParentElement.IsLocked)
            {
                e.Handled = true;
                return;
            }
            #endregion

            #region 鼠标框选处理
            
            if (((Keyboard.GetKeyStates(Key.LeftCtrl) == KeyStates.Down || (Keyboard.GetKeyStates(Key.LeftCtrl) == (KeyStates.Down | KeyStates.Toggled))) && element.EleType == ElementType.receive) ||
                element.EleType == ElementType.screen ||
                  element.EleType == ElementType.groupframe)
            {
               
                Rect oldRect = new Rect();
                if (((IRectElement)element.ParentElement).OperateEnviron == OperatEnvironment.DesignScreen)
                {
                    #region 移除前一个(框选的矩形zorder==-1)
                    for (int elementIndex = 0; elementIndex < _myScreenLayer.ElementCollection.Count; elementIndex++)
                    {
                        if (!(_myScreenLayer.ElementCollection[elementIndex] is RectElement))
                        {
                            continue;
                        }
                        if (((RectElement)_myScreenLayer.ElementCollection[elementIndex]).ZOrder == -1)
                        {
                            IRectElement re=(RectElement)_myScreenLayer.ElementCollection[elementIndex];
                            _myScreenLayer.ElementCollection.RemoveAt(elementIndex);
                            oldRect = new Rect(re.X, re.Y, re.Width, re.Height);
                        }
                    }
                    #endregion
                    Point mouseMovePointInThis = e.GetPosition(this);
                    mouseMovePointInThis.X = mouseMovePointInThis.X;
                    mouseMovePointInThis.Y = mouseMovePointInThis.Y;
                    Point mouseMovePointInItemsControl = this.TranslatePoint(mouseMovePointInThis, _itemsControl);
                    if (mouseMovePointInItemsControl.X > SmartLCTViewModeBase.MaxScreenWidth)
                    {
                        mouseMovePointInItemsControl.X = SmartLCTViewModeBase.MaxScreenWidth;
                    }
                    if (mouseMovePointInItemsControl.Y > SmartLCTViewModeBase.MaxScreenHeight)
                    {
                        mouseMovePointInItemsControl.Y = SmartLCTViewModeBase.MaxScreenHeight;
                    }
                    if (mouseMovePointInItemsControl.X < 0)
                    {
                        mouseMovePointInItemsControl.X = 0;
                    }
                    if (mouseMovePointInItemsControl.Y < 0)
                    {
                        mouseMovePointInItemsControl.Y = 0;
                    }
                    Point mouseLeftDownPointInItemControl = _mouseLeftDownPointInThis;
                    
                    double height = Math.Abs(mouseMovePointInItemsControl.Y - mouseLeftDownPointInItemControl.Y);
                    double width = Math.Abs(mouseMovePointInItemsControl.X - mouseLeftDownPointInItemControl.X);

                    RectElement rect = new RectElement(mouseLeftDownPointInItemControl.X, mouseLeftDownPointInItemControl.Y, width, height, _myScreenLayer, -1);
                    
                    if (mouseMovePointInItemsControl.X > mouseLeftDownPointInItemControl.X && mouseMovePointInItemsControl.Y > mouseLeftDownPointInItemControl.Y)
                    {
                        rect.X = mouseLeftDownPointInItemControl.X;
                        rect.Y = mouseLeftDownPointInItemControl.Y;

                    }
                    else if (mouseMovePointInItemsControl.X > mouseLeftDownPointInItemControl.X && mouseMovePointInItemsControl.Y < mouseLeftDownPointInItemControl.Y)
                    {
                        rect.X = mouseLeftDownPointInItemControl.X;
                        rect.Y = mouseMovePointInItemsControl.Y;
                    }
                    else if (mouseMovePointInItemsControl.X < mouseLeftDownPointInItemControl.X && mouseMovePointInItemsControl.Y > mouseLeftDownPointInItemControl.Y)
                    {
                        rect.X = mouseMovePointInItemsControl.X;
                        rect.Y = mouseLeftDownPointInItemControl.Y;
                    }
                    else if (mouseMovePointInItemsControl.X < mouseLeftDownPointInItemControl.X && mouseMovePointInItemsControl.Y < mouseLeftDownPointInItemControl.Y)
                    {
                        rect.X = mouseMovePointInItemsControl.X;
                        rect.Y = mouseMovePointInItemsControl.Y;
                    } 

                    

                    Thickness margin = new Thickness(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
                    rect.Margin = margin;
                    rect.ZOrder = -1;
                    rect.BackgroundBrush = Brushes.Gray;
                    rect.Opacity = 0.5;
                    rect.ZIndex = 8;
                    rect.MyLockAndVisibleButtonVisible = Visibility.Hidden;
                    rect.ElementSelectedState = SelectedState.FrameSelected;
                    rect.EleType = ElementType.frameSelected;
                    _myScreenLayer.ElementCollection.Add(rect);
                    _isFrameSelected = true;
                     
                    Point minPoint=new Point(0,0);
                    Point maxPoint = new Point(this.ActualWidth, this.ActualHeight);
                    minPoint=this.TranslatePoint(minPoint, _itemsControl);
                    maxPoint = this.TranslatePoint(maxPoint, _itemsControl);
                    //判断移动的方向
                    if (oldRect != new Rect())
                    {
                        //向右框选
                        if (oldRect.X == rect.X && oldRect.Width<rect.Width && rect.X + rect.Width > maxPoint.X)
                        {
                            _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + rect.X + rect.Width - maxPoint.X);
                        }
                        else if (oldRect.X+oldRect.Width==rect.X+rect.Width && oldRect.Width > rect.Width && rect.X > maxPoint.X)
                        {
                            _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + rect.X - maxPoint.X);
                        }
                        //向左框选
                        if (oldRect.X + oldRect.Width == rect.X + rect.Width && oldRect.X > rect.X && rect.X < minPoint.X)
                        {
                            _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset - (minPoint.X-rect.X));
                        }
                        else if (oldRect.X == rect.X && oldRect.Width > rect.Width && rect.X + rect.Width < minPoint.X)
                        {
                            _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset - (minPoint.X - rect.X-rect.Width));
                        }
                        //向下框选
                        if (oldRect.Y == rect.Y && oldRect.Height < rect.Height && rect.Y + rect.Height > maxPoint.Y)
                        {
                            _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + rect.Y + rect.Height - maxPoint.Y);
                        }
                        else if (oldRect.Y + oldRect.Height == rect.Y + rect.Height && oldRect.Height > rect.Height && rect.Y > maxPoint.Y)
                        {
                            _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + rect.Y - maxPoint.Y);
                        }
                        //向上框选
                        if (oldRect.Y + oldRect.Height == rect.Y + rect.Height && oldRect.Y > rect.Y && rect.Y < minPoint.Y)
                        {
                            _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset - (minPoint.Y - rect.Y));
                        }
                        else if (oldRect.Y == rect.Y && oldRect.Height > rect.Height && rect.Y + rect.Height < minPoint.Y)
                        {
                            _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset - (minPoint.Y - rect.Y - rect.Height));
                        }
                    }
                    
                    e.Handled = true;
                    return;
                }
            }
            #endregion

            #region 旧的移动
            //if (element.EleType==ElementType.receive && ((IRectElement)element).OperateEnviron==OperatEnvironment.DesignScreen)
            //{
            //    element.AddressVisible = Visibility.Visible;
            //    if (_selectedElementCollection.Count == 0)
            //    {
            //        e.Handled = true;
            //        return;
            //    }
            //}

            //#region 移动距离处理(超出最大最小边界时)
            //double controlMovingValueX = mousePoint.X - _mousePoint.X;
            //double controlMovingValueY = mousePoint.Y - _mousePoint.Y;
            ////controlMovingValueX = Math.Round(controlMovingValueX);
            ////controlMovingValueY = Math.Round(controlMovingValueY);
            //double maxScreenHeight = 0;
            //double maxScreenWidth = 0;
            //if (_myScreenLayer.OperateEnviron == OperatEnvironment.DesignScreen)
            //{
            //    maxScreenHeight = SmartLCTViewModeBase.MaxScreenHeight;
            //    maxScreenWidth = SmartLCTViewModeBase.MaxScreenWidth;
            //}
            //else
            //{
            //    maxScreenWidth = _myScreenLayer.Width;
            //    maxScreenHeight = _myScreenLayer.Height;
            //}
            //if (_selectedElementRect.Width > maxScreenWidth)
            //{
            //    controlMovingValueX = -_selectedElementRect.X;
            //}
            //else if (_selectedElementRect.X + controlMovingValueX + _selectedElementRect.Width > maxScreenWidth)
            // {
            //     controlMovingValueX = maxScreenWidth - _selectedElementRect.X - _selectedElementRect.Width;
            //}
            //else if (_selectedElementRect.X + controlMovingValueX < 0)
            // {
            //     controlMovingValueX = -_selectedElementRect.X;
            //     _isStartMoveX = false;
            // }
            //if (_selectedElementRect.Height > maxScreenHeight)
            //{
            //    controlMovingValueY = -_selectedElementRect.Y;
            //}
            //else if (_selectedElementRect.Y + controlMovingValueY + _selectedElementRect.Height > maxScreenHeight)
            // {
            //     controlMovingValueY = maxScreenHeight - _selectedElementRect.Y - _selectedElementRect.Height;            
            // }
            //else if (_selectedElementRect.Y + controlMovingValueY < 0)
            // {
            //     controlMovingValueY = -_selectedElementRect.Y;
            //     _isStartMoveY = false;
            // }        
            // #endregion

            // #region 吸附

            //if (_isStartMoveX) //刚开始移动（移动距离大于SmartLCTViewModeBase.AdsorbValue才能移动）
            //{
            //    isMoveX = true;
            //    if (Math.Abs(controlMovingValueX) < SmartLCTViewModeBase.AdsorbValue)
            //    {
            //        isMoveX = false;
            //        controlMovingValueX = 0;
            //    }
            //    else
            //    {
            //        isMoveX = true;
            //        _isStartMoveX = false;
            //    }
            //}
            //if (_isStartMoveY) //刚开始移动（移动距离大于5才能移动）
            //{
            //    isMoveY = true;
            //    if (Math.Abs(controlMovingValueY) < SmartLCTViewModeBase.AdsorbValue)
            //    {
            //        isMoveY = false;
            //        controlMovingValueY = 0;
            //    }
            //    else
            //    {
            //        isMoveY = true;
            //        _isStartMoveY = false;
            //    }
            //}
            //if (isMoveY == false && isMoveX == false)
            //{
            //    e.Handled = true;
            //    return;
            //}
            //double valueX = SmartLCTViewModeBase.AdsorbValue + 1;
            //double valueY = SmartLCTViewModeBase.AdsorbValue + 1;


            //double currentElementMaxX = 0;
            //double currentElementMaxY = 0;
            //IRectElement currentElement = (IRectElement)element;
            //currentElementMaxX = currentElement.X;
            //currentElementMaxY = currentElement.Y;
            //if (controlMovingValueX > 0)
            //{
            //    if (_currentSelectedElement[element.GroupName].NoSelectedGroupElementList.Count == 0)
            //    {
            //        currentElementMaxX += _currentSelectedElement[element.GroupName].SelectedElementRect.Width;
            //    }
            //    else
            //    {
            //        currentElementMaxX += currentElement.Width;
            //    }
            //}
            //if (controlMovingValueY > 0)
            //{
            //    if (_currentSelectedElement[element.GroupName].NoSelectedGroupElementList.Count == 0)
            //    {
            //        currentElementMaxY += _currentSelectedElement[element.GroupName].SelectedElementRect.Height;
            //    }
            //    else
            //    {
            //        currentElementMaxY += currentElement.Height;
            //    }
            //}
            //for (int i = 0; i < _myScreenLayer.ElementCollection.Count; i++)
            //{
            //    if (_myScreenLayer.ElementCollection[i] is LineElement || 
            //        _myScreenLayer.ElementCollection[i].ElementSelectedState!=SelectedState.None)
            //    {
            //        continue;
            //    }
            //    IRectElement rect = (IRectElement)_myScreenLayer.ElementCollection[i];

            //    if (controlMovingValueX < 0 && isMoveX)
            //    {
            //        #region 向左移动
            //        if (rect.X > currentElementMaxX
            //            || rect.X + rect.Width < currentElementMaxX - SmartLCTViewModeBase.AdsorbValue
            //            || rect.Y > currentElementMaxY + currentElement.Height
            //            || rect.Height + rect.Y < currentElementMaxY)
            //        {
            //            continue;
            //        }
            //        if (rect.X < currentElementMaxX
            //            && rect.X + rect.Width > currentElement.X + currentElement.Width
            //            && currentElementMaxX - rect.X < valueX && currentElementMaxX - rect.X > 0)
            //        {
            //            valueX = currentElementMaxX - rect.X;
            //        }
            //        else
            //        {
            //            if (currentElementMaxX - rect.X < valueX && currentElementMaxX - rect.X > 0)
            //            {
            //                valueX = currentElementMaxX - rect.X;
            //            }
            //            else if (currentElementMaxX - rect.X - rect.Width < valueX
            //                && currentElementMaxX - rect.X - rect.Width > 0)
            //            {
            //                valueX = currentElementMaxX - rect.X - rect.Width;
            //            }
            //        }
            //        if (valueX < SmartLCTViewModeBase.AdsorbValue + 1 && valueX > 0)
            //        {
            //            controlMovingValueX = -valueX;
            //            _isStartMoveX = true;
            //        }
            //        #endregion
            //    }
            //    else if (controlMovingValueX > 0 && isMoveX)
            //    {
            //        #region 向右移动
            //        if (rect.X + rect.Width < currentElementMaxX
            //            || rect.X > currentElementMaxX + SmartLCTViewModeBase.AdsorbValue
            //            || rect.Y + rect.Height < currentElementMaxY
            //            || rect.Y > currentElementMaxY + currentElement.Height)
            //        {
            //            continue;
            //        }
            //        if (rect.X < currentElementMaxX && rect.X + rect.Width > currentElementMaxX
            //            && rect.X + rect.Width - currentElementMaxX < valueX
            //            && rect.X + rect.Width - currentElementMaxX > 0)
            //        {
            //            valueX = rect.X + rect.Width - currentElementMaxX;
            //        }
            //        else
            //        {
            //            if (rect.X - currentElementMaxX < valueX && rect.X - currentElementMaxX > 0)
            //            {
            //                valueX = rect.X - currentElementMaxX;
            //            }
            //            else if (rect.X + rect.Width - currentElementMaxX < valueX
            //                && rect.X + rect.Width - currentElementMaxX > 0)
            //            {
            //                valueX = rect.X + rect.Width - currentElementMaxX;
            //            }
            //        }
            //        if (valueX < SmartLCTViewModeBase.AdsorbValue + 1 && valueX > 0)
            //        {
            //            controlMovingValueX = valueX;
            //            _isStartMoveX = true;
            //        }
            //        #endregion
            //    }
            //    if (controlMovingValueY < 0 && isMoveY)
            //    {
            //        #region 向上移动
            //        if (rect.Y > currentElementMaxY
            //            || rect.Y + rect.Height < currentElementMaxY - SmartLCTViewModeBase.AdsorbValue
            //            || rect.X > currentElementMaxX + currentElement.Width
            //            || rect.Width + rect.X < currentElementMaxX)
            //        {
            //            continue;
            //        }
            //        if (rect.Y < currentElementMaxY
            //            && rect.Y + rect.Height > currentElement.Y + currentElement.Height
            //            && currentElementMaxY - rect.Y < valueY && currentElementMaxY - rect.Y > 0)
            //        {
            //            valueY = currentElementMaxY - rect.Y;
            //        }
            //        else
            //        {
            //            if (currentElementMaxY - rect.Y < valueY && currentElementMaxY - rect.Y > 0)
            //            {
            //                valueY = currentElementMaxY - rect.Y;
            //            }
            //            else if (currentElementMaxY - rect.Y - rect.Height < valueY
            //                && currentElementMaxY - rect.Y - rect.Height > 0)
            //            {
            //                valueY = currentElementMaxY - rect.Y - rect.Height;
            //            }
            //        }
            //        if (valueY < SmartLCTViewModeBase.AdsorbValue + 1 && valueY > 0)
            //        {
            //            controlMovingValueY = -valueY;
            //            _isStartMoveY = true;
            //        }
            //        #endregion
            //    }
            //    else if (controlMovingValueY > 0 && isMoveY)
            //    {
            //        #region 向下移动
            //        if (rect.Y + rect.Height < currentElementMaxY
            //            || rect.Y > currentElementMaxY + SmartLCTViewModeBase.AdsorbValue
            //            || rect.X + rect.Width < currentElementMaxX
            //            || rect.X > currentElementMaxX + currentElement.Width)
            //        {
            //            continue;
            //        }
            //        if (rect.Y < currentElementMaxY && rect.Y + rect.Height > currentElementMaxY
            //            && rect.Y + rect.Height - currentElementMaxY < valueY
            //            && rect.Y + rect.Height - currentElementMaxY > 0)
            //        {
            //            valueY = rect.Y + rect.Height - currentElementMaxY;
            //        }
            //        else
            //        {
            //            if (rect.Y - currentElementMaxY < valueY && rect.Y - currentElementMaxY > 0)
            //            {
            //                valueY = rect.Y - currentElementMaxY;
            //            }
            //            else if (rect.Y + rect.Height - currentElementMaxY < valueY
            //                && rect.Y + rect.Height - currentElementMaxY > 0)
            //            {
            //                valueY = rect.Y + rect.Height - currentElementMaxY;
            //            }

            //            if (valueY < SmartLCTViewModeBase.AdsorbValue + 1 && valueY > 0)
            //            {
            //                controlMovingValueY = valueY;
            //                _isStartMoveY = true;
            //            }
            //        }
            //        #endregion
            //    }
            //}
            // #endregion    

            // #region 滚动条处理
            // Point maxXInThis = new Point();
            // Point minXInThis = new Point();
            // minXInThis.X = _selectedElementRect.X + controlMovingValueX + _myScreenLayer.X;
            // minXInThis.Y = _selectedElementRect.Y + controlMovingValueY + _myScreenLayer.Y;
            // maxXInThis.X = _selectedElementRect.X + controlMovingValueX + _selectedElementRect.Width + _myScreenLayer.X;
            // maxXInThis.Y = _selectedElementRect.Y + controlMovingValueY + _selectedElementRect.Height + _myScreenLayer.Y;
            // maxXInThis = _itemsControl.TranslatePoint(maxXInThis, this);
            // minXInThis = _itemsControl.TranslatePoint(minXInThis, this);
            //if (isMoveX)
            // {
            //     if (minXInThis.X < 0 && controlMovingValueX < 0)
            //     {
            //         if (minXInThis.X - controlMovingValueX < 0)
            //         {
            //             _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + controlMovingValueX * Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex));
            //         }
            //         else
            //         {
            //             _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + minXInThis.X * Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex));
            //         }
            //     }
            //     else if (maxXInThis.X > this.ActualWidth - (_scrollViewer.ActualWidth - _scrollViewer.ViewportWidth) && controlMovingValueX > 0)
            //     {
            //         if (maxXInThis.X - controlMovingValueX > this.ActualWidth - (_scrollViewer.ActualWidth - _scrollViewer.ViewportWidth))
            //         {
            //             _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + controlMovingValueX * Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex));
            //         }
            //         else
            //         {
            //             _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + (maxXInThis.X - this.ActualWidth + (_scrollViewer.ActualWidth - _scrollViewer.ViewportWidth)) * Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex));
            //         }
            //     }
            // }
            // if (isMoveY)
            // {
            //     if (minXInThis.Y < 0 && controlMovingValueY < 0)
            //     {
            //         if (minXInThis.Y - controlMovingValueY < 0)
            //         {
            //             _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + controlMovingValueY * Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex));
            //         }
            //         else
            //         {
            //             _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + minXInThis.Y * Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex));
            //         }
            //     }
            //     else if (maxXInThis.Y > this.ActualHeight - (_scrollViewer.ActualHeight - _scrollViewer.ViewportHeight) && controlMovingValueY > 0)
            //     {
            //         if (maxXInThis.Y - controlMovingValueY > this.ActualHeight - (_scrollViewer.ActualHeight - _scrollViewer.ViewportHeight))
            //         {
            //             _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + controlMovingValueY * Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex));
            //         }
            //         else
            //         {
            //             _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + (maxXInThis.Y - this.ActualHeight + (_scrollViewer.ActualHeight - _scrollViewer.ViewportHeight)) * Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex));
            //         }
            //     }
            // }
            // #endregion

            // #region 移动与边框改变
            // if (controlMovingValueX != 0 || controlMovingValueY != 0)
            // {
            //     _isMoving = true;
            // }
            // foreach (int i in _currentSelectedElement.Keys)
            // {
            //     if (_currentSelectedElement[i].SelectedGroupElementList.Count == 0)
            //     {
            //         continue;
            //     }
            //     ObservableCollection<IRectElement> selectedList = _currentSelectedElement[i].SelectedGroupElementList;
            //     ObservableCollection<IRectElement> noSelectedList = _currentSelectedElement[i].NoSelectedGroupElementList;
            //     Rect selectedElementRect = _currentSelectedElement[i].SelectedElementRect;
            //     Rect noSelectedElementRect = _currentSelectedElement[i].NoSelectedElementRect;
            //    if (controlMovingValueX != 0)
            //    {
            //        Console.WriteLine("旧的移动x:" + controlMovingValueX.ToString());
            //        Function.SetElementCollectionX(selectedList, controlMovingValueX);
            //        selectedElementRect.X += controlMovingValueX;
            //        selectedElementRect.X = Math.Round(selectedElementRect.X);
            //    }
            //    if (controlMovingValueY != 0)
            //    {
            //        Console.WriteLine("旧的移动y:" + controlMovingValueY.ToString());

            //        Function.SetElementCollectionY(selectedList, controlMovingValueY);
            //        selectedElementRect.Y += controlMovingValueY;
            //        selectedElementRect.Y = Math.Round(selectedElementRect.Y);
            //    }
            //    if (i != -1 && (controlMovingValueX != 0 || controlMovingValueY != 0))
            //    {
            //        if (noSelectedList.Count == 0)
            //        {
            //            _groupframeList[i].X += controlMovingValueX;
            //            _groupframeList[i].Y += controlMovingValueY;
            //        }
            //        else
            //        {
            //            Rect unionrect = Rect.Union(selectedElementRect, noSelectedElementRect);
            //            _groupframeList[i].Width = unionrect.Width;
            //            _groupframeList[i].Height = unionrect.Height; 
            //            _groupframeList[i].X = unionrect.X;
            //            _groupframeList[i].Y = unionrect.Y;

            //        }
            //        _currentSelectedElement[i].SelectedElementRect = selectedElementRect;
            //        _currentSelectedElement[i].NoSelectedElementRect = noSelectedElementRect;
            //    }
            //    else if (i == -1 && (controlMovingValueX != 0 || controlMovingValueY != 0))
            //    {
            //        _currentSelectedElement[i].SelectedElementRect = selectedElementRect;
            //    }
            // }
            // _selectedElementRect = new Rect();
            // foreach (int key in _currentSelectedElement.Keys)
            // {
            //     if (_currentSelectedElement[key].SelectedGroupElementList.Count != 0)
            //     {
            //         if (_selectedElementRect == new Rect())
            //         {
            //             _selectedElementRect = _currentSelectedElement[key].SelectedElementRect;
            //         }
            //         else
            //         {
            //             _selectedElementRect = Rect.Union(_selectedElementRect, _currentSelectedElement[key].SelectedElementRect);
            //         }

            //     }
            // }
            // #endregion
            #endregion
            e.Handled = true;
        }
        void DragScope_PreviewDragOver(object sender, MouseEventArgs args)
        {
            //图片跟随鼠标的移动
            if (this._adorner != null)
            {
                #region 处理移动中的情况
                #region 移动距离处理(超出最大最小边界时)
                double controlMovingValueX = 0;
                double controlMovingValueY = 0;
                double changedvalue = Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex);
                if (!_isScrollHorizon)
                {
                    controlMovingValueX = (args.GetPosition(this._dragScope).X - _virtualMoveDifValue.X - this._adorner.LeftOffset / changedvalue) * changedvalue;
                }
                if (!_isScrollVertial)
                {
                    controlMovingValueY = (args.GetPosition(this._dragScope).Y - _virtualMoveDifValue.Y - this._adorner.TopOffset / changedvalue) * changedvalue;
                }
        
                _isScrollVertial = false;
                _isScrollHorizon = false;
                if (controlMovingValueX == 0 && controlMovingValueY == 0)
                {

                    return;
                }
                double maxScreenHeight = 0;
                double maxScreenWidth = 0;
                double dviX = 0;
                double dviY = 0;
                if (_myScreenLayer.OperateEnviron == OperatEnvironment.DesignScreen)
                {
                    maxScreenHeight = SmartLCTViewModeBase.MaxScreenHeight;
                    maxScreenWidth = SmartLCTViewModeBase.MaxScreenWidth;
                }
                else
                {
                    maxScreenWidth = _myScreenLayer.Width;
                    maxScreenHeight = _myScreenLayer.Height;
                    if(_myScreenLayer.OperateEnviron==OperatEnvironment.AdjustScreenLocation ||
                        _myScreenLayer.OperateEnviron == OperatEnvironment.AdjustSenderLocation)
                    {
                        dviX = _myScreenLayer.X;
                        dviY = _myScreenLayer.Y;
                    }
                }
                if (_selectedReceiveArea.Width > maxScreenWidth)
                {
                    controlMovingValueX = -_adorner.LeftOffset + dviX * changedvalue;
                }
                else if (_adorner.LeftOffset / changedvalue - dviX + controlMovingValueX + _selectedReceiveArea.Width > maxScreenWidth )
                {
                    controlMovingValueX = (maxScreenWidth  - _adorner.LeftOffset / changedvalue + dviX - _selectedReceiveArea.Width) * changedvalue;
                }
                else if (_adorner.LeftOffset - dviX * changedvalue + controlMovingValueX < 0)
                {
                    controlMovingValueX = dviX * changedvalue - _adorner.LeftOffset;
                    _isStartMoveX = false;
                }
                if (_selectedReceiveArea.Height > maxScreenHeight)
                {
                    controlMovingValueY = -_adorner.TopOffset + dviY * changedvalue; ;
                }
                else if (_adorner.TopOffset / changedvalue - dviY + controlMovingValueY + _selectedReceiveArea.Height > maxScreenHeight)
                {
                    controlMovingValueY = (maxScreenHeight - _adorner.TopOffset / changedvalue + dviY - _selectedReceiveArea.Height) * changedvalue;
                }
                else if (_adorner.TopOffset - dviY * changedvalue  + controlMovingValueY < 0)
                {
                    controlMovingValueY =  -_adorner.TopOffset + dviY * changedvalue;
                    _isStartMoveY = false;
                }
                #endregion
                controlMovingValueX = Math.Round(controlMovingValueX);
                controlMovingValueY = Math.Round(controlMovingValueY);
                #region 吸附
                bool isMoveX = true;
                bool isMoveY = true;
                int adsorbValue = (int)Math.Round(SmartLCTViewModeBase.AdsorbValue / changedvalue);
                if (_isStartMoveX) //刚开始移动（移动距离大于SmartLCTViewModeBase.AdsorbValue才能移动）
                {
                    isMoveX = true;
                    if (Math.Abs(controlMovingValueX) < SmartLCTViewModeBase.AdsorbValue)
                    {
                        isMoveX = false;
                        controlMovingValueX = 0;
                    }
                    else
                    {
                        isMoveX = true;
                        _isStartMoveX = false;
                    }
                }
                if (_isStartMoveY) //刚开始移动（移动距离大于5才能移动）
                {
                    isMoveY = true;
                    if (Math.Abs(controlMovingValueY) < SmartLCTViewModeBase.AdsorbValue)
                    {
                        isMoveY = false;
                        controlMovingValueY = 0;
                    }
                    else
                    {
                        isMoveY = true;
                        _isStartMoveY = false;
                    }
                }
                if (isMoveY == false && isMoveX == false)
                {
                    return;
                }
                double valueX = (adsorbValue + 1);
                double valueY = (adsorbValue + 1);

                double currentElementMaxX = Math.Round(_adorner.LeftOffset / changedvalue - dviX);//移动的元素最大的x
                double currentElementMaxY = Math.Round(_adorner.TopOffset / changedvalue - dviY);//移动的元素最大的Y
                if (controlMovingValueX > 0)
                {
                    currentElementMaxX += _selectedReceiveArea.Width;
                }
                if (controlMovingValueY > 0)
                {
                    currentElementMaxY += _selectedReceiveArea.Height;
                }

                for (int i = 0; i < _myScreenLayer.ElementCollection.Count; i++)
                {
                    if (_myScreenLayer.ElementCollection[i] is LineElement ||
                        _myScreenLayer.ElementCollection[i].ElementSelectedState != SelectedState.None)
                    {
                        continue;
                    }
                    IRectElement rect = (IRectElement)_myScreenLayer.ElementCollection[i];

                    if (controlMovingValueX < 0 && isMoveX)
                    {
                        #region 向左移动
                        if (rect.X > currentElementMaxX
                            || rect.X + rect.Width < currentElementMaxX - adsorbValue
                            || rect.Y > currentElementMaxY + _selectedReceiveArea.Height
                            || rect.Height + rect.Y < currentElementMaxY)
                        {
                            continue;
                        }
                        if (rect.X < currentElementMaxX
                            && rect.X + rect.Width > Math.Round(_adorner.LeftOffset / changedvalue) - dviX + _selectedReceiveArea.Width
                            && currentElementMaxX - rect.X < valueX && currentElementMaxX - rect.X > 0)
                        {
                            valueX = currentElementMaxX - rect.X;
                        }
                        else
                        {
                            if (currentElementMaxX - rect.X < valueX && currentElementMaxX - rect.X > 0)
                            {
                                valueX = currentElementMaxX - rect.X;
                            }
                            else if (currentElementMaxX - rect.X - rect.Width < valueX
                                && currentElementMaxX - rect.X - rect.Width > 0)
                            {
                                valueX = currentElementMaxX - rect.X - rect.Width;
                            }
                        }
                        valueX = Math.Round(valueX);
                        if (valueX < adsorbValue + 1 && valueX > 0)
                        {
                            controlMovingValueX = -valueX * changedvalue;
                            _isStartMoveX = true;
                        }
                        #endregion
                    }
                    else if (controlMovingValueX > 0 && isMoveX)
                    {
                        #region 向右移动
                        if (rect.X + rect.Width < currentElementMaxX
                            || rect.X > currentElementMaxX + adsorbValue
                            || rect.Y + rect.Height < currentElementMaxY
                            || rect.Y > currentElementMaxY + _selectedReceiveArea.Height)
                        {
                            continue;
                        }
                        if (rect.X < currentElementMaxX && rect.X + rect.Width > currentElementMaxX
                            && rect.X + rect.Width - currentElementMaxX < valueX
                            && rect.X + rect.Width - currentElementMaxX > 0)
                        {
                            valueX = rect.X + rect.Width - currentElementMaxX;
                        }
                        else
                        {
                            if (rect.X - currentElementMaxX < valueX && rect.X - currentElementMaxX > 0)
                            {
                                valueX = rect.X - currentElementMaxX;
                            }
                            else if (rect.X + rect.Width - currentElementMaxX < valueX
                                && rect.X + rect.Width - currentElementMaxX > 0)
                            {
                                valueX = rect.X + rect.Width - currentElementMaxX;
                            }
                        }
                        if (valueX < adsorbValue + 1 && valueX > 0)
                        {
                            controlMovingValueX = valueX * changedvalue;
                            _isStartMoveX = true;
                        }
                        #endregion
                    }
                    if (controlMovingValueY < 0 && isMoveY)
                    {
                        #region 向上移动
                        if (rect.Y > currentElementMaxY
                            || rect.Y + rect.Height < currentElementMaxY - adsorbValue
                            || rect.X > currentElementMaxX + _selectedReceiveArea.Width
                            || rect.Width + rect.X < currentElementMaxX)
                        {
                            continue;
                        }
                        if (rect.Y < currentElementMaxY
                            && rect.Y + rect.Height > Math.Round(_adorner.TopOffset / changedvalue) - dviY + _selectedReceiveArea.Height
                            && currentElementMaxY - rect.Y < valueY && currentElementMaxY - rect.Y > 0)
                        {

                            valueY = currentElementMaxY - rect.Y;
                        }
                        else
                        {
                            if (currentElementMaxY - rect.Y < valueY && currentElementMaxY - rect.Y > 0)
                            {
                                valueY = currentElementMaxY - rect.Y;
                            }
                            else if (currentElementMaxY - rect.Y - rect.Height < valueY
                                && currentElementMaxY - rect.Y - rect.Height > 0)
                            {
                                valueY = currentElementMaxY - rect.Y - rect.Height;
                            }
                        }
                        if (valueY < adsorbValue + 1 && valueY > 0)
                        {
                            controlMovingValueY = -valueY * changedvalue;
                            _isStartMoveY = true;
                        }

                        #endregion
                    }
                    else if (controlMovingValueY > 0 && isMoveY)
                    {
                        #region 向下移动
                        if (rect.Y + rect.Height < currentElementMaxY
                            || rect.Y > currentElementMaxY + adsorbValue
                            || rect.X + rect.Width < currentElementMaxX
                            || rect.X > currentElementMaxX + _selectedReceiveArea.Width)
                        {
                            continue;
                        }
                        if (rect.Y < currentElementMaxY && rect.Y + rect.Height > currentElementMaxY
                            && rect.Y + rect.Height - currentElementMaxY < valueY
                            && rect.Y + rect.Height - currentElementMaxY > 0)
                        {
                            valueY = rect.Y + rect.Height - currentElementMaxY;
                        }
                        else
                        {
                            if (rect.Y - currentElementMaxY < valueY && rect.Y - currentElementMaxY > 0)
                            {
                                valueY = rect.Y - currentElementMaxY;
                            }
                            else if (rect.Y + rect.Height - currentElementMaxY < valueY
                                && rect.Y + rect.Height - currentElementMaxY > 0)
                            {
                                valueY = rect.Y + rect.Height - currentElementMaxY;
                            }

                            if (valueY < adsorbValue + 1 && valueY > 0)
                            {
                                controlMovingValueY = valueY * changedvalue;
                                _isStartMoveY = true;
                            }
                        }
                        #endregion
                    }
                }
                if (Math.Abs(controlMovingValueX) > 0 && Math.Abs(controlMovingValueX) < 1)
                {
                    controlMovingValueX = controlMovingValueX / Math.Abs(controlMovingValueX);
                }
                if (Math.Abs(controlMovingValueY) > 0 && Math.Abs(controlMovingValueY) < 1)
                {
                    controlMovingValueY = controlMovingValueY / Math.Abs(controlMovingValueY);
                }
                controlMovingValueX = Math.Round(controlMovingValueX);
                controlMovingValueY = Math.Round(controlMovingValueY);

                #endregion

                #region 滚动条处理
                Point maxXInThis = new Point();
                Point minXInThis = new Point();
                minXInThis.X = _adorner.LeftOffset / changedvalue - dviX + controlMovingValueX / changedvalue + _myScreenLayer.X;
                minXInThis.Y = _adorner.TopOffset / changedvalue - dviY  + controlMovingValueY / changedvalue + _myScreenLayer.Y;
                maxXInThis.X = _adorner.LeftOffset / changedvalue - dviX + controlMovingValueX / changedvalue + _selectedReceiveArea.Width + _myScreenLayer.X;
                maxXInThis.Y = _adorner.TopOffset / changedvalue - dviY + controlMovingValueY / changedvalue + _selectedReceiveArea.Height + _myScreenLayer.Y;
                maxXInThis = _itemsControl.TranslatePoint(maxXInThis, this);
                minXInThis = _itemsControl.TranslatePoint(minXInThis, this);
                if (isMoveX)
                {
                    if (minXInThis.X < 0 && controlMovingValueX / changedvalue < 0)
                    {
                        if (minXInThis.X - controlMovingValueX / changedvalue < 0)
                        {
                            _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + controlMovingValueX);
                            _isScrollHorizon = true;
                        }
                        else
                        {
                            _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + minXInThis.X);
                            _isScrollHorizon = true;
                        }
                    }
                    else if (maxXInThis.X > this.ActualWidth - (_scrollViewer.ActualWidth - _scrollViewer.ViewportWidth) && controlMovingValueX > 0)
                    {
                        if (maxXInThis.X - controlMovingValueX / changedvalue > this.ActualWidth - (_scrollViewer.ActualWidth - _scrollViewer.ViewportWidth))
                        {
                            _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + controlMovingValueX);
                            _isScrollHorizon = true;
                        }
                        else
                        {
                            _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + (maxXInThis.X - this.ActualWidth + (_scrollViewer.ActualWidth - _scrollViewer.ViewportWidth)));
                            _isScrollHorizon = true;
                        }
                    }
                }
                if (isMoveY)
                {
                    if (minXInThis.Y < 0 && controlMovingValueY < 0)
                    {
                        if (minXInThis.Y - controlMovingValueY / changedvalue < 0)
                        {
                            _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + controlMovingValueY);
                            _isScrollVertial = true;
                        }
                        else
                        {
                            _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + minXInThis.Y);
                            _isScrollVertial = true;
                        }
                    }
                    else if (maxXInThis.Y > this.ActualHeight - (_scrollViewer.ActualHeight - _scrollViewer.ViewportHeight) && controlMovingValueY > 0)
                    {
                        if (maxXInThis.Y - controlMovingValueY / changedvalue > this.ActualHeight - (_scrollViewer.ActualHeight - _scrollViewer.ViewportHeight))
                        {
                            _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + controlMovingValueY);
                            _isScrollVertial = true;
                        }
                        else
                        {
                            _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + (maxXInThis.Y - this.ActualHeight + (_scrollViewer.ActualHeight - _scrollViewer.ViewportHeight)));
                            _isScrollVertial = true;
                        }
                    }
                }
                #endregion
                #endregion

                controlMovingValueX = Math.Round(controlMovingValueX);
                controlMovingValueY = Math.Round(controlMovingValueY);

                this._adorner.LeftOffset = this._adorner.LeftOffset + controlMovingValueX;
                this._adorner.TopOffset = this._adorner.TopOffset + controlMovingValueY;

                this._locationAdorner.LeftOffset = this._adorner.LeftOffset;
                this._locationAdorner.TopOffset = this._adorner.TopOffset;
                Point currentAdornerPoint = new Point(_adorner.LeftOffset / changedvalue, _adorner.TopOffset / changedvalue);
                currentAdornerPoint.X = Math.Round(currentAdornerPoint.X);
                currentAdornerPoint.Y = Math.Round(currentAdornerPoint.Y);
                if(_myScreenLayer.OperateEnviron==OperatEnvironment.AdjustSenderLocation ||
                    _myScreenLayer.OperateEnviron == OperatEnvironment.AdjustScreenLocation)
                {
                    currentAdornerPoint.X -= _myScreenLayer.X;
                    currentAdornerPoint.Y -= _myScreenLayer.Y;
                }
                this._locationAdorner.textBlock.Text = "X=" + currentAdornerPoint.X.ToString() + "  Y=" + currentAdornerPoint.Y.ToString();
            }

        }
        public void Receive_MouseUp(object sender, MouseButtonEventArgs e, IElement element)
        {
            //if (element is LineElement)
            //{
            //    return;
            //}
            //清理工作
            _isScrollHorizon = false;
            _isScrollVertial = false;
            if (_addReceiveInfo != null)
            {
                this.Cursor = Cursors.Arrow;

            }
            if (this._adorner != null)
            {
                #region 移动与边框改变
                double changedValue = Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex);
                Point currentAdorner = new Point(_adorner.LeftOffset / changedValue, _adorner.TopOffset / changedValue);
                if (_myScreenLayer.OperateEnviron == OperatEnvironment.AdjustSenderLocation ||
                   _myScreenLayer.OperateEnviron == OperatEnvironment.AdjustScreenLocation)
                {
                    currentAdorner.X -= _myScreenLayer.X;
                    currentAdorner.Y -= _myScreenLayer.Y;
                }
                //currentAdorner = this.TranslatePoint(currentAdorner,_itemsControl);
                currentAdorner.X = Math.Round(currentAdorner.X);
                currentAdorner.Y = Math.Round(currentAdorner.Y);
                double controlMovingValueX = currentAdorner.X - _selectedReceiveArea.X;
                double controlMovingValueY = currentAdorner.Y - _selectedReceiveArea.Y;
                controlMovingValueX = Math.Round(controlMovingValueX);
                controlMovingValueY = Math.Round(controlMovingValueY);
                if (controlMovingValueX != 0 || controlMovingValueY != 0)
                {
                    _isMoving = true;
                }
                foreach (int i in _currentSelectedElement.Keys)
                {
                    if (_currentSelectedElement[i].SelectedGroupElementList.Count == 0)
                    {
                        continue;
                    }
                    ObservableCollection<IRectElement> selectedList = _currentSelectedElement[i].SelectedGroupElementList;
                    ObservableCollection<IRectElement> noSelectedList = _currentSelectedElement[i].NoSelectedGroupElementList;
                    Rect selectedElementRect = _currentSelectedElement[i].SelectedElementRect;
                    Rect noSelectedElementRect = _currentSelectedElement[i].NoSelectedElementRect;
                    if (controlMovingValueX != 0)
                    {
                        Function.SetElementCollectionX(selectedList, controlMovingValueX);
                        selectedElementRect.X += controlMovingValueX;
                        selectedElementRect.X = Math.Round(selectedElementRect.X);
                    }
                    if (controlMovingValueY != 0)
                    {
                        Function.SetElementCollectionY(selectedList, controlMovingValueY);
                        selectedElementRect.Y += controlMovingValueY;
                        selectedElementRect.Y = Math.Round(selectedElementRect.Y);
                    }
                    if (i != -1 && (controlMovingValueX != 0 || controlMovingValueY != 0))
                    {
                        if (noSelectedList.Count == 0)
                        {
                            _groupframeList[i].X += controlMovingValueX;
                            _groupframeList[i].Y += controlMovingValueY;
                        }
                        else
                        {
                            Rect unionrect = Rect.Union(selectedElementRect, noSelectedElementRect);
                            _groupframeList[i].Width = unionrect.Width;
                            _groupframeList[i].Height = unionrect.Height;
                            _groupframeList[i].X = unionrect.X;
                            _groupframeList[i].Y = unionrect.Y;

                        }
                        _currentSelectedElement[i].SelectedElementRect = selectedElementRect;
                        _currentSelectedElement[i].NoSelectedElementRect = noSelectedElementRect;
                    }
                    else if (i == -1 && (controlMovingValueX != 0 || controlMovingValueY != 0))
                    {
                        _currentSelectedElement[i].SelectedElementRect = selectedElementRect;
                    }
                }
                _selectedElementRect = new Rect();
                foreach (int key in _currentSelectedElement.Keys)
                {
                    if (_currentSelectedElement[key].SelectedGroupElementList.Count != 0)
                    {
                        if (_selectedElementRect == new Rect())
                        {
                            _selectedElementRect = _currentSelectedElement[key].SelectedElementRect;
                        }
                        else
                        {
                            _selectedElementRect = Rect.Union(_selectedElementRect, _currentSelectedElement[key].SelectedElementRect);
                        }

                    }
                }
                #endregion

                AdornerLayer.GetAdornerLayer(this._dragScope).Remove(this._adorner);
                AdornerLayer.GetAdornerLayer(this._dragScope).Remove(this._locationAdorner);
                this._adorner = null;
                this._locationAdorner = null;
                this._dragScope.MouseMove -= draghandler;
                this._dragScope.PreviewMouseMove -= draghandler;
            }

            #region 空格+鼠标移动屏体
            if ((Keyboard.GetKeyStates(Key.Space) == KeyStates.Down || (Keyboard.GetKeyStates(Key.Space) == (KeyStates.Down | KeyStates.Toggled))))
            {
                Layer_MouseLeftButtonUp(sender, e, _myScreenLayer);
                e.Handled = true;
                return;
            }
            #endregion
            _myScreenLayer = (RectLayer)MyRectLayer.ElementCollection[0];
            element.AddressVisible = Visibility.Hidden;
            FrameworkElement currentFramework = (FrameworkElement)sender;
            #region 记录移动动作
            if (_isMoving)
            {
                RecordEndMoveData();
            }
            _isMoving = false;
            #endregion

            #region 框选
            if (_isFrameSelected)
            {
                ObservableCollection<SelectedStateInfo> oldSelectedStateInfoCollection = new ObservableCollection<SelectedStateInfo>();
                oldSelectedStateInfoCollection = GetOldSelectedStateInfoCollection();
                Function.SetElementCollectionState(_selectedElementCollection, SelectedState.None);
                for (int elementIndex = 0; elementIndex < _myScreenLayer.ElementCollection.Count; elementIndex++)
                {
                    if (!(_myScreenLayer.ElementCollection[elementIndex] is RectElement))
                    {
                        continue;
                    }
                    if ((_myScreenLayer.ElementCollection[elementIndex]).ZOrder == -1)
                    {
                        #region 选中框选的元素
                        RectElement rect1 = (RectElement)_myScreenLayer.ElementCollection[elementIndex];
                        for (int i = 0; i < _myScreenLayer.ElementCollection.Count; i++)
                        {
                            if (_myScreenLayer.ElementCollection[i].EleType != ElementType.receive)
                            {
                                continue;
                            }
                            IRectElement rect2 = ((IRectElement)_myScreenLayer.ElementCollection[i]);
                            if (Function.IsRectIntersect(rect1, rect2))
                            {
                                if (rect2.EleType == ElementType.receive)
                                {
                                    ((IRectElement)_myScreenLayer.ElementCollection[i]).ElementSelectedState = SelectedState.Selected;
                                }
                            }
                        }
                        #endregion
                        ((RectLayer)_myScreenLayer).ElementCollection.RemoveAt(elementIndex);
                        break;
                    }
                }
            }
            #endregion

            GetCurrentElementInfo();

            SelectedElementChangedHandle(MouseState.MouseUp);

            _isLayerMouseLeftButtonDown = false;
            _isFrameSelected = false;
            _isMouseLeftButtonDown = false;
            currentFramework.ReleaseMouseCapture();
            e.Handled = true;
        }
        private void Receive_MouseRightButtonDown(object sender, MouseButtonEventArgs e, IElement element)
        {
            //if (element is LineElement)
            //{
            //    return;
            //}
            FrameworkElement currentFramework = (FrameworkElement)sender;
            _copyPoint = e.GetPosition(this);
            if (_addReceiveInfo != null && _myScreenLayer.OperateEnviron == OperatEnvironment.DesignScreen)
            {
                _addReceiveInfo = null;
                _addReceiveCallback.Execute(null);
                SelectedElementChangedHandle(MouseState.None);
                e.Handled = true;
                return;
            }
            if (sender is MyRectLayerControl)
            {
                #region 点击图层的处理
                ((RectLayer)element).CLineType = ConnectLineType.Auto;
                foreach (int elementIndex in _currentSelectedElement.Keys)
                {
                    ObservableCollection<IRectElement> selectedGroupList = _currentSelectedElement[elementIndex].SelectedGroupElementList;
                    for (int i = 0; i < selectedGroupList.Count; i++)
                    {
                        selectedGroupList[i].ElementSelectedState = SelectedState.None;
                        if (selectedGroupList[i].EleType == ElementType.receive)
                        {
                            selectedGroupList[i].IsLocked = true;
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region 点击矩形的处理
                if (element.ElementSelectedState == SelectedState.None)
                {
                    element.IsLocked = false;
                }
                else
                {
                    element.IsLocked = true;
                }
                #region 不按ctrl键
                if (element.ElementSelectedState == SelectedState.None)
                {
                    Function.SetElementCollectionState(_selectedElementCollection, SelectedState.None);
                    if (element.GroupName != -1)
                    {
                        ObservableCollection<IRectElement> noSelectedGroupElement = _currentSelectedElement[element.GroupName].NoSelectedGroupElementList;
                        ObservableCollection<IRectElement> selectedGroupElement = _currentSelectedElement[element.GroupName].SelectedGroupElementList;
                        Function.SetElementCollectionState(noSelectedGroupElement, SelectedState.Selected);
                        Function.SetElementCollectionState(selectedGroupElement, SelectedState.Selected);
                    }
                    element.ElementSelectedState = SelectedState.Selected;
                }
                #endregion

                #endregion
            }
            GetCurrentElementInfo();
            RightButtonDownChangedHandle(element);
            e.Handled = true;
        }
        private void Receive_MouseDoubleClick(object sender, MouseButtonEventArgs e, IElement element)
        {
            //if (element is LineElement)
            //{
            //    return;
            //}
            FrameworkElement currentFramework = (FrameworkElement)sender;
            IElement parent = element.ParentElement;
            ObservableCollection<SelectedStateInfo> oldSelectedStateInfoCollection = new ObservableCollection<SelectedStateInfo>();
            oldSelectedStateInfoCollection = GetOldSelectedStateInfoCollection();
            _copyPoint = e.GetPosition(this);
            #region 展开
            if (element.EleType == ElementType.receive || element.EleType == ElementType.groupframe)
            {
                RectLayer myScreen = MyRectLayer;
                while (myScreen != null && myScreen.EleType != ElementType.baseScreen)
                {
                    myScreen = (RectLayer)myScreen.ParentElement;
                }
                if (myScreen != null && myScreen.SenderConnectInfoList != null &&
              myScreen.SenderConnectInfoList.Count > 0)
                {
                    for (int m = 0; m < myScreen.SenderConnectInfoList.Count; m++)
                    {
                        bool isexpand = false;
                        ObservableCollection<PortConnectInfo> portConnectList = myScreen.SenderConnectInfoList[m].PortConnectInfoList;
                        if (portConnectList != null)
                        {
                            for (int i = 0; i < portConnectList.Count; i++)
                            {
                                if (portConnectList[i].IsOverLoad)
                                {
                                    isexpand = true;
                                    break;
                                }
                            }
                        }

                        if (isexpand)
                        {
                            myScreen.SenderConnectInfoList[m].IsExpand = true;
                        }
                        else
                        {
                            myScreen.SenderConnectInfoList[m].IsExpand = false;
                        }
                    }
                }

                if (myScreen != null && ((IRectElement)element).SenderIndex >= 0 && myScreen.SenderConnectInfoList != null &&
                    myScreen.SenderConnectInfoList.Count > 0 &&
                    myScreen.SenderConnectInfoList.Count >= ((IRectElement)element).SenderIndex)
                {
                    myScreen.SenderConnectInfoList[((IRectElement)element).SenderIndex].IsExpand = true;
                    myScreen.SenderConnectInfoList[((IRectElement)element).SenderIndex].PortConnectInfoList[((IRectElement)element).PortIndex].IsSelected = true;

                }

            }
            #endregion
            #region 取消图层中所有元素的选中状态
            if (element.ParentElement != null)
            {
                for (int elementIndex = 0; elementIndex < ((RectLayer)MyRectLayer.ElementCollection[0]).ElementCollection.Count; elementIndex++)
                {
                    if (((RectLayer)MyRectLayer.ElementCollection[0]).ElementCollection[elementIndex].ElementSelectedState != SelectedState.None)
                    {
                        ((RectLayer)MyRectLayer.ElementCollection[0]).ElementCollection[elementIndex].ElementSelectedState = SelectedState.None;
                    }
                }
            }
            #endregion
            element.ElementSelectedState = SelectedState.Selected;
            // element.IsLocked = false;
            _elementMoveInfo.Clear();
            GetCurrentElementInfo();
            //记录移动的旧值
            for (int i = 0; i < _selectedElementCollection.Count; i++)
            {
                Point oldPoint = new Point();
                oldPoint.X = _selectedElementCollection[i].X;
                oldPoint.Y = _selectedElementCollection[i].Y;
                _elementMoveInfo.Add(new ElementMoveInfo(_selectedElementCollection[i], new Point(), oldPoint));
            }
            _mousePoint = e.GetPosition(currentFramework);
            _isMouseLeftButtonDown = false;
            SelectedElementChangedHandle(MouseState.MouseLeftDoubleDown);

            e.Handled = true;
        }
        private void Receive_MouseWheel(object sender, MouseWheelEventArgs e, IElement element)
        {
            //if (element is LineElement)
            //{
            //    return;
            //}
            if ((Keyboard.GetKeyStates(Key.LeftCtrl) == KeyStates.Down || (Keyboard.GetKeyStates(Key.LeftCtrl) == (KeyStates.Down | KeyStates.Toggled))) ||
           (Keyboard.GetKeyStates(Key.RightCtrl) == KeyStates.Down || (Keyboard.GetKeyStates(Key.RightCtrl) == (KeyStates.Down | KeyStates.Toggled))))
            {
                if (e.Delta > 0)
                {
                    SetDecrease();
                }
                else if (e.Delta < 0)
                {
                    SetIncrease();
                }
                e.Handled = true;
            }

        }
        private void Receive_KeyDown(object sender, KeyEventArgs e, IElement element)
        {
            //if (element is LineElement)
            //{
            //    return;
            //} 
            #region 条件判断
            if (_selectedElementCollection.Count == 0)
            {
                return;
            }
            if (!(e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Down || e.Key == Key.Up))
            {
                return;
            }
            #endregion
            #region 移动处理
            #region 记录移动前的数据
            RecordBeforeMoveData();
            #endregion

            #region 距离
            double controlMovingValueX = 0;
            double controlMovingValueY = 0;
            if (e.Key == Key.Left)
            {
                controlMovingValueX = -1;
            }
            else if (e.Key == Key.Right)
            {
                controlMovingValueX = 1;
            }
            else if (e.Key == Key.Up) 
            { 
                controlMovingValueY = -1; 
            }
            else if (e.Key == Key.Down) 
            { 
                controlMovingValueY = 1;
            }
            #endregion
            #region 移动距离处理(超出最大最小边界时)
            Rect selectedReceiveArea = Function.UnionRectCollection(_selectedElementCollection);//移动的区域
            double changedvalue = Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex);
            double maxScreenHeight = 0;
            double maxScreenWidth = 0;
            if (_myScreenLayer.OperateEnviron == OperatEnvironment.DesignScreen)
            {
                maxScreenHeight = SmartLCTViewModeBase.MaxScreenHeight;
                maxScreenWidth = SmartLCTViewModeBase.MaxScreenWidth;
            }
            else
            {
                maxScreenWidth = _myScreenLayer.Width;
                maxScreenHeight = _myScreenLayer.Height;
            }
            if (selectedReceiveArea.Width > maxScreenWidth)
            {
                controlMovingValueX = -selectedReceiveArea.Left;
            }
            else if (selectedReceiveArea.Left + controlMovingValueX + selectedReceiveArea.Width > maxScreenWidth)
            {
                controlMovingValueX = (maxScreenWidth - selectedReceiveArea.Left - selectedReceiveArea.Width) * changedvalue;
            }
            else if (selectedReceiveArea.Left + controlMovingValueX < 0)
            {
                controlMovingValueX = -selectedReceiveArea.Left;
                _isStartMoveX = false;
            }
            if (selectedReceiveArea.Height > maxScreenHeight)
            {
                controlMovingValueY = -selectedReceiveArea.Top;
            }
            else if (selectedReceiveArea.Top + controlMovingValueY + selectedReceiveArea.Height > maxScreenHeight)
            {
                controlMovingValueY = (maxScreenHeight - selectedReceiveArea.Top - selectedReceiveArea.Height) * changedvalue;
            }
            else if (selectedReceiveArea.Top + controlMovingValueY < 0)
            {
                controlMovingValueY = -selectedReceiveArea.Top;
                _isStartMoveY = false;
            }
            controlMovingValueX = Math.Round(controlMovingValueX);
            controlMovingValueY = Math.Round(controlMovingValueY);
            #endregion
            #region 滚动条处理
            Point maxXInThis = new Point();
            Point minXInThis = new Point();
            minXInThis.X = selectedReceiveArea.Left + controlMovingValueX / changedvalue + _myScreenLayer.X;
            minXInThis.Y = selectedReceiveArea.Top + controlMovingValueY / changedvalue + _myScreenLayer.Y;
            maxXInThis.X = selectedReceiveArea.Left + controlMovingValueX / changedvalue + selectedReceiveArea.Width + _myScreenLayer.X;
            maxXInThis.Y = selectedReceiveArea.Top + controlMovingValueY / changedvalue + selectedReceiveArea.Height + _myScreenLayer.Y;
            maxXInThis = _itemsControl.TranslatePoint(maxXInThis, this);
            minXInThis = _itemsControl.TranslatePoint(minXInThis, this);
            if (controlMovingValueX!=0)
            {
                if (minXInThis.X < 0 && controlMovingValueX / changedvalue < 0)
                {
                    if (minXInThis.X - controlMovingValueX / changedvalue < 0)
                    {
                        _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + controlMovingValueX);
                        _isScrollHorizon = true;
                    }
                    else
                    {
                        _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + minXInThis.X);
                        _isScrollHorizon = true;
                    }
                }
                else if (maxXInThis.X > this.ActualWidth - (_scrollViewer.ActualWidth - _scrollViewer.ViewportWidth) && controlMovingValueX > 0)
                {
                    if (maxXInThis.X - controlMovingValueX / changedvalue > this.ActualWidth - (_scrollViewer.ActualWidth - _scrollViewer.ViewportWidth))
                    {
                        _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + controlMovingValueX);
                        _isScrollHorizon = true;
                    }
                    else
                    {
                        _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + (maxXInThis.X - this.ActualWidth + (_scrollViewer.ActualWidth - _scrollViewer.ViewportWidth)));
                        _isScrollHorizon = true;
                    }
                }
            }
            if (controlMovingValueY!=0)
            {
                if (minXInThis.Y < 0 && controlMovingValueY < 0)
                {
                    if (minXInThis.Y - controlMovingValueY / changedvalue < 0)
                    {
                        _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + controlMovingValueY);
                        _isScrollVertial = true;
                    }
                    else
                    {
                        _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + minXInThis.Y);
                        _isScrollVertial = true;
                    }
                }
                else if (maxXInThis.Y > this.ActualHeight - (_scrollViewer.ActualHeight - _scrollViewer.ViewportHeight) && controlMovingValueY > 0)
                {
                    if (maxXInThis.Y - controlMovingValueY / changedvalue > this.ActualHeight - (_scrollViewer.ActualHeight - _scrollViewer.ViewportHeight))
                    {
                        _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + controlMovingValueY);
                        _isScrollVertial = true;
                    }
                    else
                    {
                        _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + (maxXInThis.Y - this.ActualHeight + (_scrollViewer.ActualHeight - _scrollViewer.ViewportHeight)));
                        _isScrollVertial = true;
                    }
                }
            }
            #endregion
            #region 移动
            foreach (int i in _currentSelectedElement.Keys)
            {
                if (_currentSelectedElement[i].SelectedGroupElementList.Count == 0)
                {
                    continue;
                }
                ObservableCollection<IRectElement> selectedList = _currentSelectedElement[i].SelectedGroupElementList;
                ObservableCollection<IRectElement> noSelectedList = _currentSelectedElement[i].NoSelectedGroupElementList;
                Rect selectedElementRect = _currentSelectedElement[i].SelectedElementRect;
                Rect noSelectedElementRect = _currentSelectedElement[i].NoSelectedElementRect;
                if (controlMovingValueX != 0)
                {
                    Function.SetElementCollectionX(selectedList, controlMovingValueX);
                    selectedElementRect.X += controlMovingValueX;
                    selectedElementRect.X = Math.Round(selectedElementRect.X);
                }
                if (controlMovingValueY != 0)
                {
                    Function.SetElementCollectionY(selectedList, controlMovingValueY);
                    selectedElementRect.Y += controlMovingValueY;
                    selectedElementRect.Y = Math.Round(selectedElementRect.Y);
                }
                if (i != -1 && (controlMovingValueX != 0 || controlMovingValueY != 0))
                {
                    if (noSelectedList.Count == 0)
                    {
                        _groupframeList[i].X += controlMovingValueX;
                        _groupframeList[i].Y += controlMovingValueY;
                    }
                    else
                    {
                        Rect unionrect = Rect.Union(selectedElementRect, noSelectedElementRect);
                        _groupframeList[i].Width = unionrect.Width;
                        _groupframeList[i].Height = unionrect.Height;
                        _groupframeList[i].X = unionrect.X;
                        _groupframeList[i].Y = unionrect.Y;

                    }
                    _currentSelectedElement[i].SelectedElementRect = selectedElementRect;
                    _currentSelectedElement[i].NoSelectedElementRect = noSelectedElementRect;
                }
                else if (i == -1 && (controlMovingValueX != 0 || controlMovingValueY != 0))
                {
                    _currentSelectedElement[i].SelectedElementRect = selectedElementRect;
                }
            }
            _selectedElementRect = new Rect();
            foreach (int key in _currentSelectedElement.Keys)
            {
                if (_currentSelectedElement[key].SelectedGroupElementList.Count != 0)
                {
                    if (_selectedElementRect == new Rect())
                    {
                        _selectedElementRect = _currentSelectedElement[key].SelectedElementRect;
                    }
                    else
                    {
                        _selectedElementRect = Rect.Union(_selectedElementRect, _currentSelectedElement[key].SelectedElementRect);
                    }

                }
            }
            #endregion
            #region 记录移动动作
            RecordEndMoveData();
            #endregion

            #endregion
            #region 更新数据
            GetCurrentElementInfo();
            SelectedElementChangedHandle(MouseState.MouseUp);
            #endregion
            e.Handled = true;
        }

        private void Layer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e, IElement element)
        {
            FrameworkElement currentFramework = (FrameworkElement)sender;
            if (element.ParentElement.IsLocked)
            {
                return;
            }
            IElement parent = element.ParentElement;

            _isLayerMouseLeftButtonDown = true;
            if (element.ParentElement is RectLayer)
            {
                for (int i = 0; i < ((RectLayer)element.ParentElement).ElementCollection.Count; i++)
                {
                    if (((RectLayer)element.ParentElement).ElementCollection[i].ZOrder == element.ZOrder)
                    {
                        if (((RectLayer)element.ParentElement).ElementCollection[i] is RectLayer)
                        {
                            _selectedLayer = (RectLayer)((RectLayer)element.ParentElement).ElementCollection[i];
                            break;
                        }
                    }
                }
            }
            _mousePoint = e.GetPosition(currentFramework);
            SelectedElementChangedHandle(MouseState.None);
            e.Handled = true;
        }
        private void Layer_MouseMove(object sender, MouseEventArgs e, IElement element)
        {
            FrameworkElement currentFramework = (FrameworkElement)sender;
            if (_myScreenLayer != null && _myScreenLayer.CLineType == ConnectLineType.Manual)
            {
                this.Cursor = Cursors.Hand;
            }
            if (!_isLayerMouseLeftButtonDown)
            {
                return;
            }
            if (_selectedLayer.IsLocked)
            {
                return;
            }
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                currentFramework.CaptureMouse();
            }
            else
            {
                currentFramework.ReleaseMouseCapture();
            }

            element.AddressVisible = Visibility.Visible;

            #region 计算移动的距离
            Point mousePoint = e.GetPosition(_itemsControl);
            double controlMovingValueX = mousePoint.X - _mousePoint.X;
            double controlMovingValueY = mousePoint.Y - _mousePoint.Y;
            #endregion
            #region 移动
            _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + (-controlMovingValueX) * Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex));
            _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + (-controlMovingValueY) * Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex));
            _isLayerMoving = true;
            #endregion

            e.Handled = true;
        }
        private void Layer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e, IElement element)
        {
            _itemsControl.Cursor = Cursors.Arrow;

            FrameworkElement currentFramework = (FrameworkElement)sender;
            element.AddressVisible = Visibility.Hidden;
            if (_isLayerMoving)
            {
                //   RectLayer newValue = new RectLayer();
                //   IElement parent = element.ParentElement;
                //   while (parent.ParentElement != null)
                //   {
                //       parent = parent.ParentElement;
                //   }
                //   newValue = (RectLayer)((RectLayer)parent).Clone();
                //   PrePropertyChangedEventArgs actionValue = new PrePropertyChangedEventArgs() { PropertyName = "ElementCollection", OldValue = old, NewValue = element.ParentElement, ZOrder = element.ParentElement.ZOrder, ParentElement = element.ParentElement.ParentElement };
                ////   Messenger.Default.Send<PrePropertyChangedEventArgs>(actionValue, MsgToken.MSG_RECORD_ACTIONVALUE);
                //   OnRecordActionValueChanged(actionValue);
            }
            _isLayerMoving = false;
            _isLayerMouseLeftButtonDown = false;
            currentFramework.ReleaseMouseCapture();
            e.Handled = false;
        }

        #endregion

        #region 私有函数
        private void SelectedLayerAndElementChanged()
        {          
            SelectedLayerAndElementValue.SelectedLayer = _myScreenLayer;
            SelectedLayerAndElementValue.CurrentRectElement = CurrentRectElement;
            SelectedLayerAndElementValue.GroupframeList = _groupframeList;
            SelectedLayerAndElementValue.SelectedInfoList = _currentSelectedElement;
            SelectedLayerAndElementValue.MainControlSize = new Size(this.ActualWidth, this.ActualHeight);
            Messenger.Default.Send<SelectedLayerAndElement>(SelectedLayerAndElementValue, MsgToken.MSG_SELECTEDLAYERANDELEMENT_CHANGED);
        }
        private void GetCurrentElementInfo()
        {
            #region 当前图层上已经选中的元素
            UpdateGroupframeList();
            _currentSelectedElement.Clear();
            _selectedElementCollection.Clear();
            int foreachIndex = -1;
            bool isfu = false;
            if (_groupframeList.Keys.Contains(-1))
            {
                isfu = true;
            }
            else
            {
                _groupframeList.Add(-1, null);
            }
            foreach (int groupName in _groupframeList.Keys)
            {
                ObservableCollection<IRectElement> selectedElement = new ObservableCollection<IRectElement>();
                ObservableCollection<IRectElement> noSelectedElement = new ObservableCollection<IRectElement>();
                Rect selectedElementRect = new Rect();
                Rect noSelectedElementRect = new Rect();
                for (int i = 0; i < _myScreenLayer.ElementCollection.Count; i++)
                {
                    if (_myScreenLayer.ElementCollection[i] is LineElement)
                    {
                        _myScreenLayer.ElementCollection[i].ZIndex = 7;
                        continue;
                    }
                    if (_myScreenLayer.ElementCollection[i].EleType == ElementType.frameSelected)
                    {
                        _myScreenLayer.ElementCollection[i].ZIndex = 8;
                        continue;
                    }
                    if (_myScreenLayer.ElementCollection[i].EleType != ElementType.receive
                        || _myScreenLayer.ElementCollection[i].GroupName != groupName)
                    {
                        continue;
                    }
                    IRectElement rectelement = (IRectElement)_myScreenLayer.ElementCollection[i];
                    Rect rect = new Rect(rectelement.X, rectelement.Y, rectelement.Width, rectelement.Height);
                    if (rectelement.ElementSelectedState != SelectedState.None)
                    {
                        if (rectelement.GroupName == -1)
                        {
                            _myScreenLayer.ElementCollection[i].ZIndex = 5;
                        }
                        else
                        {
                            _myScreenLayer.ElementCollection[i].ZIndex = 4;
                        }
                        selectedElement.Add((IRectElement)_myScreenLayer.ElementCollection[i]);
                        _selectedElementCollection.Add((IRectElement)_myScreenLayer.ElementCollection[i]);
                        if (selectedElement.Count == 1)
                        {
                            selectedElementRect = rect;
                        }
                        else
                        {
                            selectedElementRect = Rect.Union(selectedElementRect, rect);
                        }
                    }
                    else
                    {
                        _myScreenLayer.ElementCollection[i].ZIndex = 3;
                        noSelectedElement.Add((IRectElement)_myScreenLayer.ElementCollection[i]);
                        if (noSelectedElement.Count == 1)
                        {
                            noSelectedElementRect = rect;
                        }
                        else
                        {
                            noSelectedElementRect = Rect.Union(noSelectedElementRect, rect);
                        }
                    }
                }
                if (selectedElement.Count != 0)
                {
                    if (groupName>=0 && _groupframeList.Count!=0 && _groupframeList[groupName] != null)
                    {
                        _groupframeList[groupName].ZIndex = 5;
                        _groupframeList[groupName].ElementSelectedState = SelectedState.Selected;
                    }
                    for (int n = 0; n < noSelectedElement.Count; n++)
                    {
                        noSelectedElement[n].ZIndex = 3;
                    }
                 
                }
                else
                {
                    if (groupName>=0 && _groupframeList.Count!=0 && _groupframeList[groupName] != null)
                    {
                        _groupframeList[groupName].ZIndex = 1;
                        _groupframeList[groupName].ElementSelectedState = SelectedState.None;
                    }
                    for (int n = 0; n < noSelectedElement.Count; n++)
                    {
                        noSelectedElement[n].ZIndex = 1;
                    }

                }
                SelectedInfo selectedInfo = new SelectedInfo(selectedElement, noSelectedElement, selectedElementRect, noSelectedElementRect);
                if (!(groupName == -1 && selectedElement.Count == 0 && noSelectedElement.Count == 0))
                {
                    _currentSelectedElement.Add(groupName, selectedInfo);
                }
                if (selectedElement.Count != 0)
                {
                    if (foreachIndex == -1)
                    {
                        foreachIndex += 1;
                        _selectedElementRect = selectedElementRect;
                    }
                    else
                    {
                        _selectedElementRect = Rect.Union(_selectedElementRect, selectedElementRect);
                    }
                }
                

            }
            if (CurrentRectElement != null)
            {
                CurrentRectElement.ZIndex = 6;
                if (CurrentRectElement.GroupName != -1)
                {
                    if(_groupframeList!=null && _groupframeList.Count!=0 && _groupframeList.Keys.Contains(CurrentRectElement.GroupName))
                    _groupframeList[CurrentRectElement.GroupName].ZIndex = 6;
                }
            }
            if (!isfu)
            {
                _groupframeList.Remove(-1);
            }
            

            #endregion
            UpdateExpandAndSelectedInfo();
        }
        private void UpdateExpandAndSelectedInfo()
        {
            for (int j = 0; j < _myScreenLayer.SenderConnectInfoList.Count; j++)
            {
                _myScreenLayer.SenderConnectInfoList[j].IsExpand = false;
                _myScreenLayer.SenderConnectInfoList[j].IsSelected = false;
                bool isoverload = _myScreenLayer.SenderConnectInfoList[j].IsOverLoad;
                if (isoverload)
                {
                    //_myScreenLayer.SenderConnectInfoList[j].IsOverLoad = false;
                    //_myScreenLayer.SenderConnectInfoList[j].IsOverLoad = true;
                }

                for (int m = 0; m < _myScreenLayer.SenderConnectInfoList[j].PortConnectInfoList.Count; m++)
                {
                    _myScreenLayer.SenderConnectInfoList[j].PortConnectInfoList[m].IsSelected = false;
                    bool isoverloadport = _myScreenLayer.SenderConnectInfoList[j].PortConnectInfoList[m].IsOverLoad;
                    if (isoverloadport)
                    {
                        _myScreenLayer.SenderConnectInfoList[j].IsExpand = true;
                        //_myScreenLayer.SenderConnectInfoList[j].PortConnectInfoList[m].IsOverLoad = false;
                        //_myScreenLayer.SenderConnectInfoList[j].PortConnectInfoList[m].IsOverLoad = true;
                    }
                    
                }
            }

            //更新选中的网口和发送卡是否展开的信息
            if (_selectedElementCollection.Count > 0)
            {
                IRectElement rect = _selectedElementCollection[0];
                if (rect.OperateEnviron == OperatEnvironment.AdjustSenderLocation)
                {
                    return;
                }
                if (rect.SenderIndex == -1 || rect.PortIndex == -1)
                {
                    return;
                }
                for (int m = 1; m < _selectedElementCollection.Count; m++)
                {
                    if (_selectedElementCollection[m].SenderIndex == -1
                        || _selectedElementCollection[m].PortIndex == -1
                        || rect.SenderIndex != _selectedElementCollection[m].SenderIndex
                        || rect.PortIndex != _selectedElementCollection[m].PortIndex)
                    {
                        return;
                    }

                    _myScreenLayer.SenderConnectInfoList[rect.SenderIndex].IsExpand = true;
                    _myScreenLayer.SenderConnectInfoList[rect.SenderIndex].PortConnectInfoList[rect.PortIndex].IsSelected = true;
                }
            }

        }
        private void UpdateSenderAndPortConnectInfo()
        {
            for (int j = 0; j < _myScreenLayer.SenderConnectInfoList.Count; j++)
            {
                _myScreenLayer.SenderConnectInfoList[j].IsExpand = false;
                _myScreenLayer.SenderConnectInfoList[j].IsSelected = false;
                _myScreenLayer.SenderConnectInfoList[j].MapLocation = new Point();
                for (int m = 0; m < _myScreenLayer.SenderConnectInfoList[j].PortConnectInfoList.Count; m++)
                {
                    if (_myScreenLayer.SenderConnectInfoList[j].PortConnectInfoList[m].ConnectLineElementList != null)
                        _myScreenLayer.SenderConnectInfoList[j].PortConnectInfoList[m].ConnectLineElementList.Clear();

                    _myScreenLayer.SenderConnectInfoList[j].PortConnectInfoList[m].MaxConnectIndex = -1;
                    _myScreenLayer.SenderConnectInfoList[j].PortConnectInfoList[m].MaxConnectElement = null;

                    _myScreenLayer.SenderConnectInfoList[j].PortConnectInfoList[m].IsSelected = false;
                }
            }
            //更新各个网口的连线元素
            if (_myScreenLayer.ElementCollection.Count == 0)
            {
                return;
            }
            for (int i = 0; i < _myScreenLayer.ElementCollection.Count; i++)
            {
                if (_myScreenLayer.ElementCollection[i].ConnectedIndex != -1
                    && _myScreenLayer.ElementCollection[i].EleType == ElementType.receive
                    )
                {
                    IRectElement rect = (IRectElement)_myScreenLayer.ElementCollection[i];
                    _myScreenLayer.SenderConnectInfoList[rect.SenderIndex].PortConnectInfoList[rect.PortIndex].ConnectLineElementList.Add(rect);
                }
            }
            for (int i = 0; i < _myScreenLayer.SenderConnectInfoList.Count; i++)
            {
                for (int j = 0; j < _myScreenLayer.SenderConnectInfoList[i].PortConnectInfoList.Count; j++)
                {
                    if (_myScreenLayer.SenderConnectInfoList[i].PortConnectInfoList[j].ConnectLineElementList == null
                        || _myScreenLayer.SenderConnectInfoList[i].PortConnectInfoList[j].ConnectLineElementList.Count == 0)
                    {
                        continue;
                    }
                    List<IRectElement> connectList = _myScreenLayer.SenderConnectInfoList[i].PortConnectInfoList[j].ConnectLineElementList.ToList<IRectElement>();
                    connectList.Sort(delegate(IRectElement first, IRectElement second)
                    {
                        return first.ConnectedIndex.CompareTo(second.ConnectedIndex);
                    });
                    _myScreenLayer.SenderConnectInfoList[i].PortConnectInfoList[j].MaxConnectElement = ((RectElement)connectList[connectList.Count - 1]);
                    _myScreenLayer.SenderConnectInfoList[i].PortConnectInfoList[j].MaxConnectIndex = ((RectElement)connectList[connectList.Count - 1]).ConnectedIndex;
                }
            }

            //更新选中的网口和发送卡是否展开的信息
            if (_selectedElementCollection.Count > 0)
            {
                IRectElement rect = _selectedElementCollection[0];
                if (rect.SenderIndex == -1 || rect.PortIndex == -1)
                {
                    return;
                }
                for (int m = 1; m < _selectedElementCollection.Count; m++)
                {
                    if (_selectedElementCollection[m].SenderIndex == -1 
                        || _selectedElementCollection[m].PortIndex == -1 
                        || rect.SenderIndex !=_selectedElementCollection[m].SenderIndex
                        || rect.PortIndex != _selectedElementCollection[m].PortIndex)
                    {
                        return;
                    }
                }
                _myScreenLayer.SenderConnectInfoList[rect.SenderIndex].IsExpand = true;
                _myScreenLayer.SenderConnectInfoList[rect.SenderIndex].PortConnectInfoList[rect.PortIndex].IsSelected = true;
            }

        }
        private void UpdateGroupframeList()
        {
            _groupframeList.Clear();
            for (int i = 0; i < _myScreenLayer.ElementCollection.Count; i++)
            {
                if (_myScreenLayer.ElementCollection[i].EleType == ElementType.groupframe)
                {
                    _groupframeList.Add(_myScreenLayer.ElementCollection[i].GroupName, (IRectElement)_myScreenLayer.ElementCollection[i]);
                }
            }
            if (_groupframeList.Count != 0)
            {
                List<int> groupnameList = _groupframeList.Keys.ToList<int>();
                groupnameList.Sort(delegate(int first, int second)
                {
                    return first.CompareTo(second);
                });
                _myScreenLayer.MaxGroupName = groupnameList[groupnameList.Count - 1];
            }
        }
        private void UpdateLoadAreaInfo()
        {
            if (_myScreenLayer != null && _myScreenLayer.EleType == ElementType.screen)
            {
                Function.UpdateSenderConnectInfo(_myScreenLayer.SenderConnectInfoList,_myScreenLayer);
            }
        }
        private ObservableCollection<SelectedStateInfo> GetOldSelectedStateInfoCollection()
        {
            ObservableCollection<SelectedStateInfo> selectedStateInfoCollection = new ObservableCollection<SelectedStateInfo>();
            for (int i = 0; i < _myScreenLayer.ElementCollection.Count; i++)
            {
                if (_myScreenLayer.ElementCollection[i].EleType != ElementType.receive)
                {
                    continue;
                }
                IRectElement rect = (IRectElement)_myScreenLayer.ElementCollection[i];
                SelectedStateInfo selectedStateInfo = new SelectedStateInfo();
                selectedStateInfo.Element = rect;
                selectedStateInfo.OldSelectedState = rect.ElementSelectedState;
                selectedStateInfoCollection.Add(selectedStateInfo);

            }
            return selectedStateInfoCollection;
        }
        private ObservableCollection<SelectedStateInfo> GetNewSelectedStateInfoCollection(ObservableCollection<SelectedStateInfo> oldSelectedStateCollection)
        {
            ObservableCollection<SelectedStateInfo> selectedStateInfoCollection = new ObservableCollection<SelectedStateInfo>();
            for (int i = 0; i < oldSelectedStateCollection.Count; i++)
            {
                IRectElement rect = oldSelectedStateCollection[i].Element;
                SelectedStateInfo selectedStateInfo = new SelectedStateInfo();
                selectedStateInfo.Element = rect;
                selectedStateInfo.OldSelectedState = oldSelectedStateCollection[i].OldSelectedState;
                selectedStateInfo.NewSelectedState = oldSelectedStateCollection[i].Element.ElementSelectedState;
                selectedStateInfoCollection.Add(selectedStateInfo);
            }
            return selectedStateInfoCollection;
        }
        private void RecordBeforeMoveData()
        {
            #region 记录移动前的数据
            _elementSizeInfo.Clear();
            _elementMoveInfo.Clear();
            for (int i = 0; i < _selectedElementCollection.Count; i++)
            {
                Point oldPoint = new Point();
                oldPoint.X = _selectedElementCollection[i].X;
                oldPoint.Y = _selectedElementCollection[i].Y;
                _elementMoveInfo.Add(new ElementMoveInfo(_selectedElementCollection[i], new Point(), oldPoint));
            }
            foreach (int key in _currentSelectedElement.Keys)
            {
                if (_currentSelectedElement[key].SelectedGroupElementList.Count != 0 && key != -1)
                {
                    Point oldPoint = new Point();
                    oldPoint.X = _groupframeList[key].X;
                    oldPoint.Y = _groupframeList[key].Y;
                    _elementMoveInfo.Add(new ElementMoveInfo(_groupframeList[key], new Point(), oldPoint));
                    Size oldSize = new Size();
                    oldSize.Width = _groupframeList[key].Width;
                    oldSize.Height = _groupframeList[key].Height;
                    _elementSizeInfo.Add(new ElementSizeInfo(_groupframeList[key], new Size(), oldSize));
                }
            }
            #endregion

        }
        private void RecordEndMoveData()
        {
            ObservableCollection<ElementMoveInfo> elementMoveInfo = new ObservableCollection<ElementMoveInfo>();
            for (int i = 0; i < _elementMoveInfo.Count; i++)
            {
                Point newPoint = new Point();
                newPoint.X = _elementMoveInfo[i].Element.X;
                newPoint.Y = _elementMoveInfo[i].Element.Y;
                elementMoveInfo.Add(new ElementMoveInfo(_elementMoveInfo[i].Element, newPoint, _elementMoveInfo[i].OldPoint));
            }
            ElementMoveAction moveAction = new ElementMoveAction(elementMoveInfo);
            ObservableCollection<ElementSizeInfo> elementSizeInfo = new ObservableCollection<ElementSizeInfo>();
            for (int j = 0; j < _elementSizeInfo.Count; j++)
            {
                Size newSize = new Size();
                newSize.Width = _elementSizeInfo[j].Element.Width;
                newSize.Height = _elementSizeInfo[j].Element.Height;
                elementSizeInfo.Add(new ElementSizeInfo(_elementSizeInfo[j].Element, newSize, _elementSizeInfo[j].OldSize));
            }
            ElementSizeAction sizeAction = new ElementSizeAction(elementSizeInfo);
            using (Transaction.Create(SmartLCTActionManager))
            {
                SmartLCTActionManager.RecordAction(moveAction);
                SmartLCTActionManager.RecordAction(sizeAction);
            }
        }
        #endregion
        #region 消息处理函数
        private void OnAddReceive(NotificationMessageAction<AddReceiveInfo> info)
        {
            if (info.Notification == MsgToken.MSG_ADDRECEIVE)
            {
                _addReceiveCallback = info;
                _addReceiveInfo = (AddReceiveInfo)info.Target;
                SelectedElementChangedHandle(MouseState.None);
            }
        }
        private void OnCompleteAddReceive(bool isComplete)
        {
            
        }

        private void OnExit(object s)
        {
            Messenger.Default.Unregister<object>(this, MsgToken.MSG_EXIT, OnExit);
            Messenger.Default.Unregister<NotificationMessageAction<AddReceiveInfo>>(this, MsgToken.MSG_ADDRECEIVE, OnAddReceive);
        }
        private void RightButtonDownChangedHandle(IElement element)
        {
            if (_myScreenLayer.OperateEnviron != OperatEnvironment.DesignScreen)
            {
                return;
            }
            if (element.EleType == ElementType.screen)
            {
                #region 当前选择的是图层
                    element.ElementSelectedState = SelectedState.Selected;
                    if (element.EleType == ElementType.screen)
                    {
                        _workAreaContextMenu.PlacementTarget = this;
                        _workAreaContextMenu.IsOpen = true;
                    }
               
                #endregion
            }
            else if(element.EleType==ElementType.receive)
            {
                #region 当前选择的是矩形
                _contextMenu.PlacementTarget = this;
                _contextMenu.IsOpen = true;
                #endregion
            }
            SelectedLayerAndElementValue.SelectedLayer = _myScreenLayer;
            SelectedLayerAndElementValue.CurrentRectElement = CurrentRectElement;
            SelectedLayerAndElementValue.SelectedElement = _selectedElementCollection;
            SelectedLayerAndElementValue.GroupframeList = _groupframeList;
            SelectedLayerAndElementValue.SelectedInfoList = _currentSelectedElement;
            SelectedLayerAndElementValue.MainControlSize = new Size(this.ActualWidth, this.ActualHeight);
            Messenger.Default.Send<SelectedLayerAndElement>(SelectedLayerAndElementValue, MsgToken.MSG_SELECTEDLAYERANDELEMENT_CHANGED);
        }
        private void SelectedElementChangedHandle(MouseState mouseAtion)
        {
            #region 将当前选中图层和选中的元素传出去
            if (_addReceiveInfo != null && _myScreenLayer.OperateEnviron==OperatEnvironment.DesignScreen)
            {
                SelectedLayerAndElementValue.IsAddingReceive = true;
            }
            else
            {
                SelectedLayerAndElementValue.IsAddingReceive = false;
            }
            SelectedLayerAndElementValue.SelectedLayer = _myScreenLayer;
            SelectedLayerAndElementValue.SelectedElement = _selectedElementCollection;
            SelectedLayerAndElementValue.GroupframeList = _groupframeList;
            SelectedLayerAndElementValue.SelectedInfoList = _currentSelectedElement;
            SelectedLayerAndElementValue.MouseState = mouseAtion;
            SelectedLayerAndElementValue.MainControlSize = new Size(this.ActualWidth, this.ActualHeight);
            Messenger.Default.Send<SelectedLayerAndElement>(SelectedLayerAndElementValue, MsgToken.MSG_SELECTEDLAYERANDELEMENT_CHANGED);
            #endregion         
        }
        #endregion

        #region 重载        
        public override void OnApplyTemplate()
        {
            var viewbox = this.GetTemplateChild("MyViewbox") as Viewbox;
            var scroll = this.GetTemplateChild("MyScrollViewer") as ScrollViewer;
            var itemsControl = this.GetTemplateChild("MyItemsControl") as ItemsControl;
            _itemsControl = itemsControl;          
            _scrollViewer = scroll;
            _viewbox = viewbox;
            if (_viewbox != null)
            {
                if (_myScreenLayer.OperateEnviron == OperatEnvironment.DesignScreen)
                {
                    _viewbox.Width = SmartLCTViewModeBase.ViewBoxWidth;
                    _viewbox.Height = SmartLCTViewModeBase.ViewBoxHeight;
                }
                else
                {
                    _viewbox.Width = SmartLCTViewModeBase.DviViewBoxWidth;
                    _viewbox.Height = SmartLCTViewModeBase.DviViewBoxHeight;
                }
          
            if (_myScreenLayer != new RectLayer() && _viewbox != null && _myScreenLayer.IncreaseOrDecreaseIndex > 0)
            {
                _viewbox.Height = _viewbox.Height * Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex);
                _viewbox.Width = _viewbox.Width * Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, _myScreenLayer.IncreaseOrDecreaseIndex);
            }
            else if (_myScreenLayer != new RectLayer() && _viewbox != null && _myScreenLayer.IncreaseOrDecreaseIndex < 0)
            {
                _viewbox.Height = _viewbox.Height / Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, -_myScreenLayer.IncreaseOrDecreaseIndex);
                _viewbox.Width = _viewbox.Width / Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, -_myScreenLayer.IncreaseOrDecreaseIndex);
            }
            if (_myScreenLayer != new RectLayer() && _myScreenLayer.OperateEnviron == OperatEnvironment.DesignScreen)
            {
                SelectedLayerAndElementValue.SelectedLayer = _myScreenLayer;
                SelectedLayerAndElementValue.CurrentRectElement = null;
                SelectedLayerAndElementValue.SelectedElement = _selectedElementCollection;
                SelectedLayerAndElementValue.GroupframeList = _groupframeList;
                SelectedLayerAndElementValue.SelectedInfoList = _currentSelectedElement;
                SelectedLayerAndElementValue.MainControlSize = new Size(this.ActualWidth, this.ActualHeight);
                Messenger.Default.Send<SelectedLayerAndElement>(SelectedLayerAndElementValue, MsgToken.MSG_SELECTEDLAYERANDELEMENT_CHANGED);
            }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            //若电脑开启翻译软件，那么拖动鼠标时可能会触发keydown事件，获取到的key值分别是ctrl和c
            base.OnKeyDown(e);
            if (_myScreenLayer.OperateEnviron != OperatEnvironment.DesignScreen)
            {
                return;
            }
            #region delete删除选中元素
            ObservableCollection<RectElement> selectedRectList = new ObservableCollection<RectElement>();
            if (e.Key == Key.Delete)
            {                
                OnCmdDelete();
                return;
            }
            #endregion
            #region 全选CTRL+A、剪切、复制、粘贴、撤销、重做、删除
            if (e.Key == Key.LeftCtrl || e.Key==Key.RightCtrl)
            {
                if (_keyInfo.Count == 0)
                {
                    _keyInfo.Add(e.Key);
                }
                if (_keyInfo.Contains(Key.A) && _keyInfo.Count == 1)
                {
                    _keyInfo.Add(e.Key);
                }
            }
            else if (e.Key == Key.A || e.Key==Key.C || e.Key==Key.V || e.Key==Key.Z || e.Key==Key.X || e.Key==Key.Y)
            {
                if (!_keyInfo.Contains(e.Key))
                {
                    for (int i = 0; i < _keyInfo.Count; i++)
                    {
                        if (!(_keyInfo[i] == Key.LeftCtrl || _keyInfo[i] == Key.RightCtrl))
                        {
                            _keyInfo.RemoveAt(i);
                        }
                    }
                    _keyInfo.Add(e.Key);
                }
            }
            if (_keyInfo.Count == 2)
            {
                #region 全选
                if (_keyInfo.Contains(Key.A))
                {
                    OnCmdSelectedAll();
                    _keyInfo.Remove(Key.A);
                }
                #endregion
                #region 复制
                if (_keyInfo.Contains(Key.C))
                {
                    _copyPoint = new Point(-1, -1);
                    OnCmdCopy();
                    _keyInfo.Remove(Key.C);
                }
                #endregion
                #region 粘贴
                if (_keyInfo.Contains(Key.V))
                {
                    OnCmdPaste();
                    _keyInfo.Remove(Key.V);
                    _copyPoint = new Point(-1, -1);
                }
                #endregion
                #region 撤销
                if (_keyInfo.Contains(Key.Z))
                {
                    UnDo();
                    _keyInfo.Remove(Key.Z);
                }
                #endregion
                #region 重做
                if (_keyInfo.Contains(Key.Y))
                {
                    ReDo();
                    _keyInfo.Remove(Key.Y);
                }
                #endregion
                #region 剪切
                if (_keyInfo.Contains(Key.X))
                {
                    Rect unionRect = Function.UnionRectCollection(_selectedElementCollection);
                    _copyPoint = new Point(unionRect.Left, unionRect.Top);
                    _copyPoint = _itemsControl.TranslatePoint(_copyPoint, this);
                    OnCmdCut();
                    _keyInfo.Remove(Key.X);
                }
                #endregion
            }
            #endregion
            #region 空格
            if (e.Key == Key.Space)
            {
                
                 StreamResourceInfo sri = Application.GetResourceStream(
                new Uri("/Nova.SmartLCT.Skin;component/Image/BlueMode/Normal/move.cur", UriKind.Relative));
                _moveCursor=new Cursor(sri.Stream);
                _itemsControl.Cursor = _moveCursor;
                return;
            }
            #endregion
            #region 方向键
            if (e.Key==Key.Left)
            {

            }
            #endregion

        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (_myScreenLayer.OperateEnviron != OperatEnvironment.DesignScreen)
            {
                return;
            }
            if (e.Key == Key.Space)
            {
                _itemsControl.Cursor = Cursors.Arrow;
                return;
            }
            bool isKey = false;
            if (e.Key == Key.LeftCtrl || e.Key==Key.RightCtrl)
            {
                isKey = true;
            }
            else if (e.Key == Key.A || e.Key==Key.C || e.Key==Key.V || e.Key==Key.Z || e.Key==Key.X)
            {
                isKey = true;
            }
            if (isKey && _keyInfo.Contains(e.Key))
            {
                _keyInfo.Remove(e.Key);
            }
        }

        #endregion
    }
}
