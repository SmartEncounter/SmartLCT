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
using System.Collections.Specialized;
using Nova.LCT.GigabitSystem.Common;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Controls.Primitives;
using GalaSoft.MvvmLight.Command;
using CommonAdorner;
using GuiLabs.Undo;
using System.Windows.Resources;

namespace Nova.SmartLCT.UI
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Nova.SmartLCT.UI"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Nova.SmartLCT.UI;assembly=Nova.SmartLCT.UI"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:ScreenPropertyPanel/>
    ///
    /// </summary>
    public class ScreenPropertyPanel : Control
    {
        #region Xaml属性
        public static readonly DependencyProperty ScannerCofigCollectionProperty =
           DependencyProperty.Register(
               "ScannerCofigCollection", typeof(ObservableCollection<ScannerCofigInfo>), typeof(ScreenPropertyPanel),
               new FrameworkPropertyMetadata(null,
                   new PropertyChangedCallback(OnScannerCofigCollectionChangedCallback)
               )
           );

        public static readonly DependencyProperty ScreenRealParamsProperty =
           DependencyProperty.Register(
               "ScreenRealParams", typeof(ScreenRealParameters), typeof(ScreenPropertyPanel),
               new FrameworkPropertyMetadata(null,
                   new PropertyChangedCallback(OnScreenRealParamsChanged)
               )
           );
        

        private static readonly DependencyProperty SelectedScannerConfigProperty =
          DependencyProperty.Register(
              "SelectedScannerConfig", typeof(ScannerCofigInfo), typeof(ScreenPropertyPanel),
              new FrameworkPropertyMetadata(null, OnSelectedScannerConfigChanged)
          );
        private static readonly DependencyProperty SenderConfigCollectionProperty =
          DependencyProperty.Register(
              "SenderConfigCollection", typeof(ObservableCollection<SenderConfigInfo>), typeof(ScreenPropertyPanel),
              new FrameworkPropertyMetadata(null, OnSenderConfigCollectionChanged)
          );

        public static readonly DependencyProperty SmartLCTActionManagerProperty =
            DependencyProperty.Register("SmartLCTActionManager", typeof(ActionManager),
            typeof(ScreenPropertyPanel), new FrameworkPropertyMetadata(new ActionManager()));

        public static readonly DependencyProperty AddRowsCountProperty =
          DependencyProperty.Register(
              "AddRowsCount", typeof(int), typeof(ScreenPropertyPanel),
                 new FrameworkPropertyMetadata(1, OnAddRowsCountChanged)
        );
        public static readonly DependencyProperty AddColsCountProperty =
          DependencyProperty.Register(
              "AddColsCount", typeof(int), typeof(ScreenPropertyPanel),
                new FrameworkPropertyMetadata(2, OnAddColsCountChanged)
        );
        public static readonly DependencyProperty IsCheckedProperty =
    DependencyProperty.Register("IsChecked", typeof(bool), typeof(ScreenPropertyPanel));

        public static readonly DependencyProperty CurrentSenderConfigInfoProperty =
    DependencyProperty.Register("CurrentSenderConfigInfo", typeof(SenderConfigInfo), typeof(ScreenPropertyPanel), new FrameworkPropertyMetadata(null, OnCurrentSenderConfigInfoChanged));
        public static readonly DependencyProperty SelectedSenderConfigInfoProperty =
    DependencyProperty.Register("SelectedSenderConfigInfo", typeof(SenderConfigInfo), typeof(ScreenPropertyPanel));

        public static readonly DependencyProperty PopupHeightProperty =
    DependencyProperty.Register(" PopupHeight", typeof(double), typeof(ScreenPropertyPanel));

        public static readonly DependencyProperty IncreaseOrDecreaseIndexProperty =
    DependencyProperty.Register("IncreaseOrDecreaseIndex", typeof(int), typeof(ScreenPropertyPanel));

        public static readonly DependencyProperty IsConnectLineProperty =
    DependencyProperty.Register("IsConnectLine", typeof(bool), typeof(ScreenPropertyPanel), new FrameworkPropertyMetadata(false, OnIsConnectLineChanged));
        
        public static readonly DependencyProperty SelectedArrangeTypeProperty =
    DependencyProperty.Register("SelectedArrangeType", typeof(ArrangeType), typeof(ScreenPropertyPanel));

        public static readonly DependencyProperty SenderAndPortPicCollectionProperty =
    DependencyProperty.Register("SenderAndPortPicCollection", typeof(Dictionary<int,SenderAndPortPicInfo>), typeof(ScreenPropertyPanel));

        
        #endregion

        #region 属性
        public Dictionary<int,SenderAndPortPicInfo> SenderAndPortPicCollection
        {
            get { return (Dictionary<int,SenderAndPortPicInfo>)GetValue(SenderAndPortPicCollectionProperty); }
            set
            {
                SetValue(SenderAndPortPicCollectionProperty ,value);
            }
        }
        public ArrangeType SelectedArrangeType
        {
            get { return (ArrangeType)GetValue(SelectedArrangeTypeProperty); }
            set { SetValue(SelectedArrangeTypeProperty, value); }
        }
        public bool IsConnectLine
        {
            get { return (bool)GetValue(IsConnectLineProperty); }
            set { SetValue(IsConnectLineProperty, value); }
        }
        public int IncreaseOrDecreaseIndex
        {
            get { return (int)GetValue(IncreaseOrDecreaseIndexProperty); }
            set { SetValue(IncreaseOrDecreaseIndexProperty,value); }
        }
        public double PopupHeight
        {
            get { return (double)GetValue(PopupHeightProperty); }
            set { SetValue(PopupHeightProperty, value); }
        }
        public SenderConfigInfo CurrentSenderConfigInfo
        {
            get { return (SenderConfigInfo)GetValue(CurrentSenderConfigInfoProperty); }
            set { SetValue(CurrentSenderConfigInfoProperty, value); }
        }
        public SenderConfigInfo SelectedSenderConfigInfo
        {
            get { return (SenderConfigInfo)GetValue(SelectedSenderConfigInfoProperty); }
            set { SetValue(SelectedSenderConfigInfoProperty, value); }
        }

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        public ScreenRealParameters ScreenRealParams
        {
            get { return (ScreenRealParameters)GetValue(ScreenRealParamsProperty); }
            set
            {
                SetValue(ScreenRealParamsProperty, value);
            }
        }
        /// <summary>
        /// 配置文件列表
        /// </summary>
        public ObservableCollection<ScannerCofigInfo> ScannerCofigCollection
        {
            get { return (ObservableCollection<ScannerCofigInfo>)GetValue(ScannerCofigCollectionProperty); }
            set
            {
                if (ScannerCofigCollection != null)
                {                  
                    ScannerCofigCollection.CollectionChanged -= new NotifyCollectionChangedEventHandler(OnScannerCofigCollectionChanged);
                }
                SetValue(ScannerCofigCollectionProperty, value);
                if (ScannerCofigCollection != null)
                {
                    ScannerCofigCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(OnScannerCofigCollectionChanged);
                }
            }
        }
        public ScannerCofigInfo SelectedScannerConfig
        {
            get
            {
                return (ScannerCofigInfo)GetValue(SelectedScannerConfigProperty);
            }
            set
            {
                SetValue(SelectedScannerConfigProperty, value);

            }
        }

        public ObservableCollection<SenderConfigInfo> SenderConfigCollection
        {
            get { return (ObservableCollection<SenderConfigInfo>)GetValue(SenderConfigCollectionProperty); }

            set
            {
                SetValue(SenderConfigCollectionProperty, value);

                PopupHeight = SenderConfigCollection.Count * 30;
            }
        }
        private ObservableCollection<SenderConfigInfo> _senderConfigCollection = new ObservableCollection<SenderConfigInfo>();


        public ActionManager SmartLCTActionManager
        {
            get { return (ActionManager)GetValue(SmartLCTActionManagerProperty); }
            set
            {
                SetValue(SmartLCTActionManagerProperty, value);
            }
        }

        public int AddRowsCount
        {
            get { return (int)GetValue(AddRowsCountProperty); }
            set
            {
                SetValue(AddRowsCountProperty, value);
            }
        }

        public int AddColsCount
        {
            get { return (int)GetValue(AddColsCountProperty); }
            set
            {
                SetValue(AddColsCountProperty, value);
            }
        }

        #endregion

        #region 命令
        public static RoutedCommand CmdAddScanner
        {
            get { return _cmdAddScanner; }
        }
        private static RoutedCommand _cmdAddScanner;
        public static RoutedCommand CmdShowScanBoardConfigManager
        {
            get { return _cmdShowScanBoardConfigManager; }
        }
        private static RoutedCommand _cmdShowScanBoardConfigManager;

        public static RoutedCommand CmdAddSenderIndex
        {
            get { return _cmdAddSenderIndex; }
        }
        private static RoutedCommand _cmdAddSenderIndex;
        public static RoutedCommand CmdRemoveSenderIndex
        {
            get { return _cmdRemoveSenderIndex; }
        }
        private static RoutedCommand _cmdRemoveSenderIndex;
        public static RoutedCommand CmdAddPortIndex
        {
            get { return _cmdAddPortIndex; }
        }
        private static RoutedCommand _cmdAddPortIndex;
        public static RoutedCommand CmdArrangeScanner
        {
            get { return _cmdArrangeScanner; }
        }
        private static RoutedCommand _cmdArrangeScanner;
        public static RoutedCommand CmdManualLine
        {
            get { return _cmdManualLine; }
        }
        private static RoutedCommand _cmdManualLine;

        public static RoutedCommand CmdCustomReceiveSize
        {
            get { return _cmdCustomReceiveSize; }
        }
        private static RoutedCommand _cmdCustomReceiveSize;

        public static RoutedCommand ChangedSenderTypeCmd
        {
            get { return _changedSenderTypeCmd; }
        }
        private static RoutedCommand _changedSenderTypeCmd;

        public static RoutedCommand MouseLeftButtonDownWithSenderType
        {
            get { return _mouseLeftButtonDownWithSenderType; }
        }
        private static RoutedCommand _mouseLeftButtonDownWithSenderType;
        public static RoutedCommand MouseUpWithSenderType
        {
            get { return _mouseUpWithSenderType; }
        }
        private static RoutedCommand _mouseUpWithSenderType;
        public static RoutedCommand MouseDoubleClickWithSenderType
        {
            get { return _mouseDoubleClickWithSenderType; }
        }
        private static RoutedCommand _mouseDoubleClickWithSenderType;
        public static RoutedCommand MouseMoveWithAddScannerCommand
        {
            get { return _mouseMoveWithAddScannerCommand; }
        }
        private static RoutedCommand _mouseMoveWithAddScannerCommand;
        public static RoutedCommand CustomSizeDropDownClosed
        {
            get { return _customSizeDropDownClosed; }
        }
        private static RoutedCommand _customSizeDropDownClosed;
        #endregion

        #region 字段
        private Canvas _senderCanvas = null;
        private Canvas _portCanvas = null;
        private Button _addSenderBut = null;
        private TextBlock _senderTypeText = null;
        private Popup _myPopup = null;

        private SmartLCTViewModeBase _smartLCTViewModeBase = new SmartLCTViewModeBase();

        /// <summary>
        /// 拖动的区域
        /// </summary>
        private FrameworkElement _dragScope;
        /// <summary>
        /// 用于显示鼠标跟随效果的装饰器
        /// </summary>
        private DragAdorner _adorner;
        /// <summary>
        /// 用于呈现DragAdorner的图画
        /// </summary>
        private AdornerLayer _layer;

        private double _lastX = 0;
        private double _lastY = 0;
        private double _lastHeight = 0;
        private double _lastWidth = 0;
        #endregion

        #region 构造
        static ScreenPropertyPanel()
        {
            InitializeCommands();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScreenPropertyPanel), new FrameworkPropertyMetadata(typeof(ScreenPropertyPanel)));
        }

        #endregion

        #region 私有函数
        private static void InitializeCommands()
        {
            _cmdAddScanner = new RoutedCommand("CmdAddScanner", typeof(ScreenPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScreenPropertyPanel), new CommandBinding(_cmdAddScanner, OnAddScannerCommand, OnCanExecAddScannerCommand));
            _cmdShowScanBoardConfigManager = new RoutedCommand("CmdShowScanBoardConfigManager", typeof(ScreenPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScreenPropertyPanel), new CommandBinding(_cmdShowScanBoardConfigManager, OnShowScanBoardConfigManagerCommand));
            _cmdAddSenderIndex = new RoutedCommand("_cmdAddSenderIndex", typeof(ScreenPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScreenPropertyPanel), new CommandBinding(_cmdAddSenderIndex, OnAddSenderIndexCommand, OnCanExecAddSenderIndexCommand));
            _cmdAddPortIndex = new RoutedCommand("_cmdAddPortIndex", typeof(ScreenPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScreenPropertyPanel), new CommandBinding(_cmdAddPortIndex, OnAddPortIndexCommand, OnCanExecAddPortIndexCommand));
            _cmdArrangeScanner = new RoutedCommand("CmdArrangeScanner", typeof(ScreenPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScreenPropertyPanel), new CommandBinding(_cmdArrangeScanner, OnCmdArrangeScanner, OnCanExecArrangeCommand));
            _cmdManualLine = new RoutedCommand("_cmdManualLine", typeof(ScreenPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScreenPropertyPanel), new CommandBinding(_cmdManualLine, OnManualLineCommand, OnCanExecManualLineCommand));

            _cmdCustomReceiveSize = new RoutedCommand("_cmdCustomReceiveSize", typeof(ScreenPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScreenPropertyPanel), new CommandBinding(_cmdCustomReceiveSize, OnCustomReceiveSizeCommand));
            
            _changedSenderTypeCmd = new RoutedCommand("ChangedSenderTypeCmd", typeof(ScreenPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScreenPropertyPanel), new CommandBinding(_changedSenderTypeCmd, OnChangedSenderTypeCommand));
            _cmdRemoveSenderIndex = new RoutedCommand("CmdRemoveSMoenderIndex", typeof(ScreenPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScreenPropertyPanel), new CommandBinding(_cmdRemoveSenderIndex, OnRemoveSenderIndexCommand,OnCanExecRemoveSenderIndexCommand));
            
            _mouseUpWithSenderType = new RoutedCommand("MouseUpWithSenderType", typeof(ScreenPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScreenPropertyPanel), new CommandBinding(_mouseUpWithSenderType, OnMouseUpWithSenderType));
            _mouseLeftButtonDownWithSenderType = new RoutedCommand("MouseLeftButtonDownWithSenderType", typeof(ScreenPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScreenPropertyPanel), new CommandBinding(_mouseLeftButtonDownWithSenderType, OnMouseLeftButtonDownWithSenderType));
            _mouseDoubleClickWithSenderType = new RoutedCommand("MouseDoubleClickWithSenderType", typeof(ScreenPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScreenPropertyPanel), new CommandBinding(_mouseDoubleClickWithSenderType, OnMouseDoubleClickWithSenderType));

            _mouseMoveWithAddScannerCommand = new RoutedCommand("MouseMoveWithAddScannerCommand", typeof(ScreenPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScreenPropertyPanel), new CommandBinding(_mouseMoveWithAddScannerCommand, OnMouseMoveWithAddScannerCommand));

            _customSizeDropDownClosed = new RoutedCommand("CustomSizeDropDownClosed", typeof(ScreenPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScreenPropertyPanel), new CommandBinding(_customSizeDropDownClosed, OnCustomReceiveSizeCommand));

        }
        private static void OnMouseMoveWithAddScannerCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ScreenPropertyPanel control = sender as ScreenPropertyPanel;
            if (control != null)
            {
                
                control.OnMouseMoveWithAddScanner();
            }
        }
        private void OnMouseMoveWithAddScanner()
        {
            if (_isMouseLeftButtonDown)
            {
                //Console.WriteLine("mousemove");
               // this.StartDrag();
            }
        }
        #region 添加接收卡
        private static void OnAddScannerCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ScreenPropertyPanel panel = (ScreenPropertyPanel)sender;
            panel.OnAddScanner(panel);

        }
        //处理拖放的路由事件
        MouseEventHandler draghandler;
        private bool _isMouseLeftButtonDown = false;
        private void OnAddScanner(ScreenPropertyPanel panel)
        {
             ScreenRealParams.ScreenLayer.CLineType = ConnectLineType.Auto;
            int cols = AddColsCount;
            int rows = AddRowsCount;
            ScanBoardProperty scanBdProp = SelectedScannerConfig.ScanBdProp;
            int width = scanBdProp.Width;
            int height = scanBdProp.Height;


            _isMouseLeftButtonDown = true;

            //拖动的区域
            this._dragScope = Application.Current.Windows[Application.Current.Windows.Count-1].Content as FrameworkElement;

            //是否可以拖放
            this._dragScope.AllowDrop = true;
            draghandler = new MouseEventHandler(DragScope_PreviewDragOver);
            //加载处理拖放的路由事件
            this._dragScope.MouseMove += draghandler;
            //鼠标跟随效果的装饰器
            Point mousePoint = new Point();
            mousePoint = Mouse.GetPosition(this._dragScope);
            this._adorner = new DragAdorner(this._dragScope, width * Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, IncreaseOrDecreaseIndex), height * Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, IncreaseOrDecreaseIndex), rows, cols, mousePoint, 0.5);
            this._adorner.LeftOffset = mousePoint.X;
            this._adorner.TopOffset = mousePoint.Y;
            this._layer = AdornerLayer.GetAdornerLayer(this._dragScope as Visual);
            this._layer.Add(this._adorner);




            NotificationMessageAction<AddReceiveInfo> nsa =
            new NotificationMessageAction<AddReceiveInfo>(this, new AddReceiveInfo(rows, cols, SelectedScannerConfig), MsgToken.MSG_ADDRECEIVE, AddReceiveNotifycationCallBack);
            Messenger.Default.Send(nsa, MsgToken.MSG_ADDRECEIVE);
        }
        void DragScope_PreviewDragOver(object sender, MouseEventArgs args)
        {
            //Console.WriteLine("DragScope_PreviewDragOver");
            //图片跟随鼠标的移动
            if (this._adorner != null)
            {

                this._adorner.LeftOffset = args.GetPosition(this._dragScope).X;
                this._adorner.TopOffset = args.GetPosition(this._dragScope).Y;
                //Console.WriteLine(this._adorner.LeftOffset.ToString() + ", " + this._adorner.TopOffset.ToString());
            }
           
        }
        private static void OnCanExecAddScannerCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            ScreenPropertyPanel panel = (ScreenPropertyPanel)sender;
            e.CanExecute = panel.OnCanExecAddScanner();
        }
        private bool OnCanExecAddScanner()
        {
            //没有选择要添加卡的配置
            if (SelectedScannerConfig == null ||
                SelectedScannerConfig.ScanBdProp == null)
            {
                return false;
            }
            //填写的行数或列数小于0
            if (AddColsCount <= 0 ||
                AddRowsCount <= 0)
            {
                return false;
            }

            return true;
        }

        private void AddReceiveNotifycationCallBack(AddReceiveInfo info)
        {
            //清理工作
            AdornerLayer.GetAdornerLayer(this._dragScope).Remove(this._adorner);
            this._adorner = null;

            this._dragScope.MouseMove -= draghandler;
            _isMouseLeftButtonDown = false;
        }
        #endregion


        private static void OnCurrentSenderConfigInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScreenPropertyPanel panel = (ScreenPropertyPanel)d;
            if (panel != null)
            {
                panel.OnCurrentSenderConfigInfo();
            }
        }
        private void OnCurrentSenderConfigInfo()
        {
            if (ScreenRealParams.ScreenLayer != null && ScreenRealParams.ScreenLayer.SenderConnectInfoList!=null)
            {
                RectLayer removeLayer = (RectLayer)ScreenRealParams.ScreenLayer;
                while (removeLayer != null && removeLayer.EleType != ElementType.baseScreen)
                {
                    removeLayer = (RectLayer)removeLayer.ParentElement;
                }
                if (removeLayer == null)
                {
                    return;
                }
                
                for (int j = 0; j < removeLayer.ElementCollection.Count; j++)
                {
                    if (removeLayer.ElementCollection[j].EleType == ElementType.newLayer)
                    {
                        continue; 
                    }
                    RectLayer reLayer = (RectLayer)((RectLayer)removeLayer.ElementCollection[j]).ElementCollection[0];
                    for (int m = 0; m < removeLayer.SenderConnectInfoList.Count; m++)
                        UpdateSenderConnectInfo(reLayer, reLayer.SenderConnectInfoList[m]);

                }
                for (int i = 0; i < removeLayer.SenderConnectInfoList.Count; i++)
                {
                    for (int j = 0; j < removeLayer.SenderConnectInfoList[i].PortConnectInfoList.Count; j++)
                    {
                        ObservableCollection<PortConnectInfo> portConnectInfoList = removeLayer.SenderConnectInfoList[i].PortConnectInfoList;
                        if (portConnectInfoList[j].PortIndex >= CurrentSenderConfigInfo.PortCount)
                        {
                            portConnectInfoList.RemoveAt(j);
                            j = j - 1;
                        }
                    }
                    if (removeLayer.SenderConnectInfoList[i].PortConnectInfoList.Count < CurrentSenderConfigInfo.PortCount)
                    {
                        int addPortCount = CurrentSenderConfigInfo.PortCount - removeLayer.SenderConnectInfoList[i].PortConnectInfoList.Count;
                        for (int m = 0; m < addPortCount; m++)
                        {
                            PortConnectInfo portConnectInfo = new PortConnectInfo(removeLayer.SenderConnectInfoList[i].PortConnectInfoList.Count, removeLayer.SenderConnectInfoList[i].SenderIndex, - 1, null, null, new Rect());
                            removeLayer.SenderConnectInfoList[i].PortConnectInfoList.Add(portConnectInfo);
                        }
                    }
                }
                //更新网口和发送卡带载
                for (int j = 0; j < removeLayer.ElementCollection.Count; j++)
                {
                    if (removeLayer.ElementCollection[j].EleType == ElementType.newLayer)
                    {
                        continue;
                    }
                    RectLayer reLayer = (RectLayer)((RectLayer)removeLayer.ElementCollection[j]).ElementCollection[0];
                    Function.UpdateSenderConnectInfo(reLayer.SenderConnectInfoList,removeLayer);
                }
                   // Function.UpdateSenderConnectInfo(removeLayer.SenderConnectInfoList, removeLayer);
                if (_portCanvas != null)
                {
                    _portCanvas.Children.Clear();
                    for (int i = 0; i < CurrentSenderConfigInfo.PortCount; i++)
                    {
                        AddPortIndex(i);
                    }
                    ScreenRealParams.ScreenLayer.CurrentPortIndex = -1;

                }
            }

        }
        private static void OnMouseDoubleClickWithSenderType(object sender, ExecutedRoutedEventArgs e)
        {
            ScreenPropertyPanel control = sender as ScreenPropertyPanel;
            if (control != null)
            {
                control.OnMouseDoubleClickWithSenderType();
            }
        }
        private void OnMouseDoubleClickWithSenderType()
        {
            if (SelectedSenderConfigInfo == null)
            {
                return;
            }
            if (CurrentSenderConfigInfo == SelectedSenderConfigInfo)
            {
                return;
            }
            NotificationMessageAction<SenderConfigInfo> nsa =
     new NotificationMessageAction<SenderConfigInfo>(SelectedSenderConfigInfo, "", MsgToken.MSG_ISMODIFYSENDERTYPE, SetChangedSenderTypeCallBack);
            Messenger.Default.Send(nsa, MsgToken.MSG_ISMODIFYSENDERTYPE);

        }
        private static void OnMouseUpWithSenderType(object sender, ExecutedRoutedEventArgs e)
        {
            ScreenPropertyPanel control = sender as ScreenPropertyPanel;
            if (control != null)
            {
                control.OnMouseUpWithSenderType();
            }
        }
        private void OnMouseUpWithSenderType()
        {
            _myPopup.StaysOpen = false;
            _myPopup.IsOpen = false;
        }
        private static void OnMouseLeftButtonDownWithSenderType(object sender, ExecutedRoutedEventArgs e)
        {
            ScreenPropertyPanel control = sender as ScreenPropertyPanel;
            if (control != null)
            {
                control.OnMouseLeftButtonDownWithSenderType();
            }
        }
        private void OnMouseLeftButtonDownWithSenderType()
        {
            _myPopup.IsOpen = true;
            _myPopup.StaysOpen = true;

        }

        #region 选择发送卡型号
        private static void OnChangedSenderTypeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ScreenPropertyPanel control = sender as ScreenPropertyPanel;
            if (control != null)
            {
                control.OnChangedSenderType();
            }
        }
        private void OnChangedSenderType()
        {
            NotificationMessageAction<SenderConfigInfo> nsa =
                 new NotificationMessageAction<SenderConfigInfo>(this, "", MsgToken.MSG_SHOWCHANGEDSENDERTYPE, SetChangedSenderTypeCallBack);
            Messenger.Default.Send(nsa, MsgToken.MSG_SHOWCHANGEDSENDERTYPE);

        }
        private void SetChangedSenderTypeCallBack(SenderConfigInfo info)
        {
            if (info.SenderTypeName != CurrentSenderConfigInfo.SenderTypeName)
            {
                for (int j = 0; j < ScreenRealParams.ScreenLayer.SenderConnectInfoList.Count; j++)
                {
                    UpdateSenderConnectInfo((RectLayer)ScreenRealParams.ScreenLayer,ScreenRealParams.ScreenLayer.SenderConnectInfoList[j]);
                }
            }
           
            CurrentSenderConfigInfo=info;
        }
        private void UpdateSenderConnectInfo(RectLayer layer,SenderConnectInfo senderConnectInfo)
        {
            senderConnectInfo.IsExpand = false;
            senderConnectInfo.IsSelected = false;
            senderConnectInfo.LoadSize = new Rect();
            senderConnectInfo.MapLocation = new Point();
            senderConnectInfo.MaxLoadArea = Function.CalculateSenderLoadSize(senderConnectInfo.PortConnectInfoList.Count, 60, 24);
            for (int m = 0; m < senderConnectInfo.PortConnectInfoList.Count; m++)
            {
               //更新后箱体网口号不存在，则删除连线信息
                if (senderConnectInfo.PortConnectInfoList[m].PortIndex >= CurrentSenderConfigInfo.PortCount)
                {
                    if (senderConnectInfo.PortConnectInfoList[m].ConnectLineElementList != null)
                    {
                        ObservableCollection<IRectElement> connectLineElementList = senderConnectInfo.PortConnectInfoList[m].ConnectLineElementList;
                        for (int i = 0; i < connectLineElementList.Count; i++)
                        {
                            if (connectLineElementList[i].FrontLine != null)
                            {
                                layer.ElementCollection.Remove(connectLineElementList[i].FrontLine);
                            }
                            if (connectLineElementList[i].EndLine != null)
                            {
                                layer.ElementCollection.Remove(connectLineElementList[i].EndLine);
                            }
                            connectLineElementList[i].ConnectedIndex = -1;
                            connectLineElementList[i].SenderIndex = -1;
                            connectLineElementList[i].PortIndex = -1;
                        }
                        senderConnectInfo.PortConnectInfoList[m].ConnectLineElementList.Clear();
                    }
                    senderConnectInfo.PortConnectInfoList[m].MaxConnectIndex = -1;
                    senderConnectInfo.PortConnectInfoList[m].MaxConnectElement = null;
                    senderConnectInfo.PortConnectInfoList[m].LoadSize = new Rect();
                    senderConnectInfo.PortConnectInfoList[m].IsSelected = false;
                    senderConnectInfo.PortConnectInfoList[m].MaxLoadArea = Function.CalculatePortLoadSize(60, 24);
                    senderConnectInfo.PortConnectInfoList.RemoveAt(m);
                    m = m - 1;
                }
            }
            if (senderConnectInfo.PortConnectInfoList.Count < CurrentSenderConfigInfo.PortCount)
            {
                int addPortCount = CurrentSenderConfigInfo.PortCount - senderConnectInfo.PortConnectInfoList.Count;
                for (int i = 0; i < addPortCount; i++)
                {
                    PortConnectInfo portConnectInfo = new PortConnectInfo(senderConnectInfo.PortConnectInfoList.Count, senderConnectInfo .SenderIndex,- 1, null, null, new Rect());
                    senderConnectInfo.PortConnectInfoList.Add(portConnectInfo);
                }
            }
        }

        private void OnSelecteSender()
        {
            //_senderTypeList.Visibility = Visibility.Visible;
            //_closeSenderTypeListBut.Visibility = Visibility.Visible;

            _senderTypeText.ContextMenu = new System.Windows.Controls.ContextMenu();
            for (int i = 0; i < SenderConfigCollection.Count; i++)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Header = SenderConfigCollection[i].SenderTypeName;

                //System.Drawing.Bitmap m_Bitmap = new System.Drawing.Bitmap(SenderConfigCollection[i].SenderPicturePath, false);
                //IntPtr ip = m_Bitmap.GetHbitmap();
                //BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                //    ip, IntPtr.Zero, Int32Rect.Empty,
                //    System.Windows.Media.Imaging.BitmapSizeOptions.FromWidthAndHeight(30, 30));
                //Image senderImage = new Image(); senderImage.Source = bitmapSource;
                //menuItem.Icon = senderImage;
                _senderTypeText.ContextMenu.Items.Add(menuItem);
            }
            //b.Style = (Style)this.FindResource("MenuItemStyle1");
            _senderTypeText.ContextMenu.PlacementTarget = _senderTypeText;
            //位置
            _senderTypeText.ContextMenu.Placement = PlacementMode.Right;
            //显示菜单
            _senderTypeText.ContextMenu.IsOpen = true;
            //    _senderTypeText.ContextMenu.Visibility = Visibility.Visible;
            
        }
        #endregion

        #region 排布接收卡
        private static void OnIsConnectLineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScreenPropertyPanel panel = (ScreenPropertyPanel)d;
            ArrangeType arrangeType = panel.SelectedArrangeType;
            switch (arrangeType)
            {
                case ArrangeType.LeftTop_Hor: panel.OnArrangeLeftTopHor(); break;
                case ArrangeType.RightTop_Hor: panel.OnArrangeRightTopHor(); break;
                case ArrangeType.LeftBottom_Hor: panel.OnArrangeLeftBottomHor(); break;
                case ArrangeType.RightBottom_Hor: panel.OnArrangeRightBottomHor(); break;
                case ArrangeType.LeftTop_Ver: panel.OnArrangeLeftTopVer(); break;
                case ArrangeType.RightTop_Ver: panel.OnArrangeRightTopVer(); break;
                case ArrangeType.LeftBottom_Ver: panel.OnArrangeLeftBottomVer(); break;
                case ArrangeType.RightBottom_Ver: panel.OnArrangeRightBottomVer(); break;
            }
        }


        private static void OnCmdArrangeScanner(object sender, ExecutedRoutedEventArgs e)
        {
            ScreenPropertyPanel panel = (ScreenPropertyPanel)sender;
            panel.OnCmdArrangeScan(sender, e);
        }
       
        private  ObservableCollection<IElement> addCollection = new ObservableCollection<IElement>();

        private void OnCmdArrangeScan(object sender, ExecutedRoutedEventArgs e)
        {
            //判断是否使用其他屏用过的网口（多发送卡的情况下，多个显示屏不允许共用一个网口）
            IRectLayer screenLayer = ScreenRealParams.ScreenLayer;
            while (screenLayer != null && screenLayer.EleType != ElementType.baseScreen)
            {
                screenLayer = (IRectLayer)screenLayer.ParentElement;
            }
            if (screenLayer != null)
            {
                int senderIndex=-1;
                int sendercount = Function.FindSenderCount(screenLayer.SenderConnectInfoList, out senderIndex);
                if (sendercount > 1)
                {
                    for (int i = 0; i < screenLayer.ElementCollection.Count; i++)
                    {
                        if(screenLayer.ElementCollection[i].EleType==ElementType.newLayer)
                        {
                            continue;
                        }
                        RectLayer screen=(RectLayer)(((RectLayer)screenLayer.ElementCollection[i]).ElementCollection[0]);
                        if(screen.ParentElement.ConnectedIndex==ScreenRealParams.ScreenLayer.ParentElement.ConnectedIndex)
                        {
                            continue;
                        }
                        if(screen.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].PortConnectInfoList[ScreenRealParams.ScreenLayer.CurrentPortIndex].LoadSize.Width!=0 &&
                            screen.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].PortConnectInfoList[ScreenRealParams.ScreenLayer.CurrentPortIndex].LoadSize.Height != 0)
                        {
                            string msg = "";
                            CommonStaticMethod.GetLanguageString("多发送卡的情况下，不允许多个屏使用同一个网口！", "Lang_PortPropPanel_CannotUserOnePort", out msg);
                            string strScr = "";
                            CommonStaticMethod.GetLanguageString("显示屏", "Lang_Global_Screen", out strScr);
                            string strWarning = "";
                            CommonStaticMethod.GetLanguageString("已经使用了该网口。", "Lang_PortPropPanel_HaveUseThisPort", out strWarning);

                            msg = msg + "\n" + strScr + " " + (screen.ParentElement.ConnectedIndex + 1).ToString() + " " + strWarning;

                            _smartLCTViewModeBase.ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);
                            return;
                        }
                    }
                }
            }
            //记录连线之前的数据
            addCollection.Clear();

            ObservableCollection<AddLineInfo> beforeAddLineCollection = new ObservableCollection<AddLineInfo>();
            for (int i = 0; i < ScreenRealParams.SelectedElement.Count; i++)
            {
                AddLineInfo addlineInfo = new AddLineInfo();
                OldAndNewType oldAndNewConnect = new OldAndNewType();
                OldAndNewType oldAndNewSender = new OldAndNewType();
                OldAndNewType oldAndNewPort = new OldAndNewType();
                oldAndNewSender.OldValue = ScreenRealParams.SelectedElement[i].SenderIndex;
                oldAndNewPort.OldValue = ScreenRealParams.SelectedElement[i].PortIndex;
                oldAndNewConnect.OldValue = ScreenRealParams.SelectedElement[i].ConnectedIndex;
                addlineInfo.Element = ScreenRealParams.SelectedElement[i];
                addlineInfo.OldAndNewConnectIndex=oldAndNewConnect;
                addlineInfo.OldAndNewSenderIndex=oldAndNewSender;
                addlineInfo.OldAndNewPortIndex=oldAndNewPort;
                beforeAddLineCollection.Add(addlineInfo);
            }

            #region 记录相关
            //记下该元素的前一个元素的最大和最小连线图标是否显示
            ObservableCollection<ConnectIconVisibilityInfo> connectIconVisibleCollection = new ObservableCollection<ConnectIconVisibilityInfo>();
            IRectElement maxConnectElement = ScreenRealParams.ScreenLayer.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].PortConnectInfoList[ScreenRealParams.ScreenLayer.CurrentPortIndex].MaxConnectElement;
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
            ScreenPropertyPanel panel = (ScreenPropertyPanel)sender;
            ArrangeType arrangeType = (ArrangeType)e.Parameter;
            bool res = false;
            switch (arrangeType)
            {
                case ArrangeType.LeftTop_Hor: res = panel.OnArrangeLeftTopHor(); break;
                case ArrangeType.RightTop_Hor: res = panel.OnArrangeRightTopHor(); break;
                case ArrangeType.LeftBottom_Hor: res = panel.OnArrangeLeftBottomHor(); break;
                case ArrangeType.RightBottom_Hor: res = panel.OnArrangeRightBottomHor(); break;
                case ArrangeType.LeftTop_Ver: res = panel.OnArrangeLeftTopVer(); break;
                case ArrangeType.RightTop_Ver: res = panel.OnArrangeRightTopVer(); break;
                case ArrangeType.LeftBottom_Ver: res = panel.OnArrangeLeftBottomVer(); break;
                case ArrangeType.RightBottom_Ver: res = panel.OnArrangeRightBottomVer(); break;
            }

            if (!res)
            {
                return;
            }

            ObservableCollection<AddLineInfo> EndAddLineCollection = new ObservableCollection<AddLineInfo>();
            for (int j = 0; j < beforeAddLineCollection.Count; j++)
            {
                AddLineInfo addLineInfo = new AddLineInfo();
                OldAndNewType oldAndNewConnect = new OldAndNewType();
                OldAndNewType oldAndNewSender = new OldAndNewType();
                OldAndNewType oldAndNewPort = new OldAndNewType();
                oldAndNewSender.OldValue = beforeAddLineCollection[j].OldAndNewSenderIndex.OldValue;
                oldAndNewPort.OldValue = beforeAddLineCollection[j].OldAndNewPortIndex.OldValue;
                oldAndNewConnect.OldValue = beforeAddLineCollection[j].OldAndNewConnectIndex.OldValue;
                oldAndNewSender.NewValue = beforeAddLineCollection[j].Element.SenderIndex;
                oldAndNewPort.NewValue = beforeAddLineCollection[j].Element.PortIndex;
                oldAndNewConnect.NewValue = beforeAddLineCollection[j].Element.ConnectedIndex;
                addLineInfo.OldAndNewConnectIndex = oldAndNewConnect;
                addLineInfo.OldAndNewSenderIndex = oldAndNewSender;
                addLineInfo.OldAndNewPortIndex = oldAndNewPort;
                addLineInfo.Element = beforeAddLineCollection[j].Element;
                
                EndAddLineCollection.Add(addLineInfo);
            }
            ObservableCollection<IElement> addInfoCollection = new ObservableCollection<IElement>();
            for (int m = 0; m < addCollection.Count; m++)
            {
                IElement element=addCollection[m];
                addInfoCollection.Add(element);
            }

            #region 记录相关
            //记下连线后最大和最小连线图标是否显示
            for (int itemIndex = 0; itemIndex < connectIconVisibleCollection.Count; itemIndex++)
            {
                connectIconVisibleCollection[itemIndex].OldAndNewMaxConnectIndexVisibile.NewValue = connectIconVisibleCollection[itemIndex].Element.MaxConnectIndexVisibile;
                connectIconVisibleCollection[itemIndex].OldAndNewMinConnectIndexVisibile.NewValue = connectIconVisibleCollection[itemIndex].Element.MinConnectIndexVisibile;
            }
            //记下该元素最大和最小连线图标是否显示
            ConnectIconVisibilityInfo connectIconinfo = new ConnectIconVisibilityInfo();
            connectIconinfo.Element = ScreenRealParams.ScreenLayer.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].PortConnectInfoList[ScreenRealParams.ScreenLayer.CurrentPortIndex].MaxConnectElement;
            OldAndNewVisibility oldAndNewMaxConnectIconVisibile = new OldAndNewVisibility();
            oldAndNewMaxConnectIconVisibile.OldValue = Visibility.Hidden;
            oldAndNewMaxConnectIconVisibile.NewValue = connectIconinfo.Element.MaxConnectIndexVisibile;
            OldAndNewVisibility oldAndNewMinConnectIconVisibile = new OldAndNewVisibility();
            oldAndNewMinConnectIconVisibile.OldValue = Visibility.Hidden;
            oldAndNewMinConnectIconVisibile.NewValue = connectIconinfo.Element.MinConnectIndexVisibile;
            connectIconinfo.OldAndNewMaxConnectIndexVisibile = oldAndNewMaxConnectIconVisibile;
            connectIconinfo.OldAndNewMinConnectIndexVisibile = oldAndNewMinConnectIconVisibile;
            connectIconVisibleCollection.Add(connectIconinfo);
            #endregion

            AddAction addAction = new AddAction(ScreenRealParams.ScreenLayer,addInfoCollection);
            AddLineAction addLineAction = new AddLineAction(EndAddLineCollection);
            ConnectIconVisibilityAction connectIconVisibleAction = new ConnectIconVisibilityAction(connectIconVisibleCollection);
            using (Transaction.Create(SmartLCTActionManager, true))
            {
                SmartLCTActionManager.RecordAction(addAction);
                SmartLCTActionManager.RecordAction(addLineAction);
                SmartLCTActionManager.RecordAction(connectIconVisibleAction);
            }
        }
        #region 记录活动
        private void OnRecordActionValueChanged(PrePropertyChangedEventArgs e)
        {
            RectLayerChangedAction action;
            action = new RectLayerChangedAction(ScreenRealParams.ScreenLayer.ParentElement, e.PropertyName, ((RectLayer)e.NewValue).ElementCollection, ((RectLayer)e.OldValue).ElementCollection);
            SmartLCTActionManager.RecordAction(action);
        }
        #endregion


        #region 水平方向
        private bool OnArrangeLeftTopHor()
        {
            #region 将要添加连线的卡按从左到右从上到下的顺序排列
            List<IRectElement> selectedElementCollection = new List<IRectElement>();
            for (int i = 0; i < ScreenRealParams.SelectedElement.Count; i++)
            {
                if (ScreenRealParams.SelectedElement[i].ConnectedIndex < 0)
                {
                    selectedElementCollection.Add(ScreenRealParams.SelectedElement[i]);
                }
            }
            selectedElementCollection.Sort(delegate(IRectElement first, IRectElement second)
            {
                if (first.Y < second.Y)
                {
                    return -1;
                }
                else if (first.Y > second.Y)
                {
                    return 1;
                }
                else
                {
                    if (first.X < second.X)
                    {
                        return -1;
                    }
                    else if (first.X > second.X)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            });
            #endregion

            #region 将连线排序
            int count = selectedElementCollection.Count;
            if (count == 0)
            {
                string msg = "";
                CommonStaticMethod.GetLanguageString("请清除当前网口连线后再进行连线！", "Lang_PortPropPanel_ClearLine", out msg);
                _smartLCTViewModeBase.ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);
                return false;
            }

            return ReverseCollection(selectedElementCollection, count, true);
            #endregion
        }

        private bool OnArrangeRightTopHor()
        {
            #region 将要添加连线的卡按从右到左从上到下的顺序排列
            List<IRectElement> selectedElementCollection = new List<IRectElement>();
            for (int i = 0; i < ScreenRealParams.SelectedElement.Count; i++)
            {
                if (ScreenRealParams.SelectedElement[i].ConnectedIndex < 0)
                {
                    selectedElementCollection.Add(ScreenRealParams.SelectedElement[i]);
                }
            }
            selectedElementCollection.Sort(delegate(IRectElement first, IRectElement second)
            {
                if (first.Y < second.Y)
                {
                    return -1;
                }
                else if (first.Y > second.Y)
                {
                    return 1;
                }
                else
                {
                    if (first.X < second.X)
                    {
                        return 1;
                    }
                    else if (first.X > second.X)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            });
            #endregion

            #region 将连线排序
            int count = selectedElementCollection.Count;
            if (count == 0)
            {
                string msg = "";
                CommonStaticMethod.GetLanguageString("请清除当前网口连线后再进行连线！", "Lang_PortPropPanel_ClearLine", out msg);
                _smartLCTViewModeBase.ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);

                return false;
            }

            return ReverseCollection(selectedElementCollection, count, true);
            #endregion
        }

        private bool OnArrangeLeftBottomHor()
        {
            #region 将要添加连线的卡按从左到右从下到上的顺序排列
            List<IRectElement> selectedElementCollection = new List<IRectElement>();
            for (int i = 0; i < ScreenRealParams.SelectedElement.Count; i++)
            {
                if (ScreenRealParams.SelectedElement[i].ConnectedIndex < 0)
                {
                    selectedElementCollection.Add(ScreenRealParams.SelectedElement[i]);
                }
            }
            selectedElementCollection.Sort(delegate(IRectElement first, IRectElement second)
            {
                if (first.Y < second.Y)
                {
                    return 1;
                }
                else if (first.Y > second.Y)
                {
                    return -1;
                }
                else
                {
                    if (first.X < second.X)
                    {
                        return -1;
                    }
                    else if (first.X > second.X)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            });
            #endregion

            #region 将连线排序
            int count = selectedElementCollection.Count;
            if (count == 0)
            {
                string msg = "";
                CommonStaticMethod.GetLanguageString("请清除当前网口连线后再进行连线！", "Lang_PortPropPanel_ClearLine", out msg);
                _smartLCTViewModeBase.ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);

                return false;
            }

            return ReverseCollection(selectedElementCollection, count, true);
            #endregion
        }

        private bool OnArrangeRightBottomHor()
        {
            #region 将要添加连线的卡按从右到左从下到上的顺序排列
            List<IRectElement> selectedElementCollection = new List<IRectElement>();
            for (int i = 0; i < ScreenRealParams.SelectedElement.Count; i++)
            {
                if (ScreenRealParams.SelectedElement[i].ConnectedIndex < 0)
                {
                    selectedElementCollection.Add(ScreenRealParams.SelectedElement[i]);
                }
            }
            selectedElementCollection.Sort(delegate(IRectElement first, IRectElement second)
            {
                if (first.Y < second.Y)
                {
                    return 1;
                }
                else if (first.Y > second.Y)
                {
                    return -1;
                }
                else
                {
                    if (first.X < second.X)
                    {
                        return 1;
                    }
                    else if (first.X > second.X)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            });
            #endregion

            #region 将连线排序
            int count = selectedElementCollection.Count;
            if (count == 0)
            {
                string msg = "";
                CommonStaticMethod.GetLanguageString("请清除当前网口连线后再进行连线！", "Lang_PortPropPanel_ClearLine", out msg);
                _smartLCTViewModeBase.ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);

                return false;
            }

            return ReverseCollection(selectedElementCollection, count, true);
            #endregion
        }
        #endregion

        #region 垂直方向
        private bool OnArrangeLeftTopVer()
        {
            #region 将要添加连线的卡按从左到右从上到下的顺序排列
            List<IRectElement> selectedElementCollection = new List<IRectElement>();
            for (int i = 0; i < ScreenRealParams.SelectedElement.Count; i++)
            {
                if (ScreenRealParams.SelectedElement[i].ConnectedIndex < 0)
                {
                    selectedElementCollection.Add(ScreenRealParams.SelectedElement[i]);
                }
            }
            selectedElementCollection.Sort(delegate(IRectElement first, IRectElement second)
            {
                if (first.X < second.X)
                {
                    return -1;
                }
                else if (first.X > second.X)
                {
                    return 1;
                }
                else
                {
                    if (first.Y < second.Y)
                    {
                        return -1;
                    }
                    else if (first.Y > second.Y)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            });
            #endregion

            #region 将连线排序
            int count = selectedElementCollection.Count;
            if (count == 0)
            {
                string msg = "";
                CommonStaticMethod.GetLanguageString("请清除当前网口连线后再进行连线！", "Lang_PortPropPanel_ClearLine", out msg);
                _smartLCTViewModeBase.ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);

                return false;
            }

            return ReverseCollection(selectedElementCollection, count, false);
            #endregion

        }

        private bool OnArrangeRightTopVer()
        {
            #region 将要添加连线的卡按从左到右从上到下的顺序排列
            List<IRectElement> selectedElementCollection = new List<IRectElement>();
            for (int i = 0; i < ScreenRealParams.SelectedElement.Count; i++)
            {
                if (ScreenRealParams.SelectedElement[i].ConnectedIndex < 0)
                {
                    selectedElementCollection.Add(ScreenRealParams.SelectedElement[i]);
                }
            }
            selectedElementCollection.Sort(delegate(IRectElement first, IRectElement second)
            {
                if (first.X < second.X)
                {
                    return 1;
                }
                else if (first.X > second.X)
                {
                    return -1;
                }
                else
                {
                    if (first.Y < second.Y)
                    {
                        return -1;
                    }
                    else if (first.Y > second.Y)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            });
            #endregion

            #region 将连线排序
            int count = selectedElementCollection.Count;
            if (count == 0)
            {
                string msg = "";
                CommonStaticMethod.GetLanguageString("请清除当前网口连线后再进行连线！", "Lang_PortPropPanel_ClearLine", out msg);
                _smartLCTViewModeBase.ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);

                return false;
            }

            return ReverseCollection(selectedElementCollection, count, false);
            #endregion
        }

        private bool OnArrangeLeftBottomVer()
        {
            #region 将要添加连线的卡按从左到右从上到下的顺序排列
            List<IRectElement> selectedElementCollection = new List<IRectElement>();
            for (int i = 0; i < ScreenRealParams.SelectedElement.Count; i++)
            {
                if (ScreenRealParams.SelectedElement[i].ConnectedIndex < 0)
                {
                    selectedElementCollection.Add(ScreenRealParams.SelectedElement[i]);
                }
            }
            selectedElementCollection.Sort(delegate(IRectElement first, IRectElement second)
            {
                if (first.X < second.X)
                {
                    return -1;
                }
                else if (first.X > second.X)
                {
                    return 1;
                }
                else
                {
                    if (first.Y < second.Y)
                    {
                        return 1;
                    }
                    else if (first.Y > second.Y)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            });
            #endregion

            #region 将连线排序
            int count = selectedElementCollection.Count;
            if (count == 0)
            {
                string msg = "";
                CommonStaticMethod.GetLanguageString("请清除当前网口连线后再进行连线！", "Lang_PortPropPanel_ClearLine", out msg);
                _smartLCTViewModeBase.ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);

                return false;
            }

            return ReverseCollection(selectedElementCollection, count, false);
            #endregion
        }

        private bool OnArrangeRightBottomVer()
        {
            #region 将要添加连线的卡按从左到右从上到下的顺序排列
            List<IRectElement> selectedElementCollection = new List<IRectElement>();
            for (int i = 0; i < ScreenRealParams.SelectedElement.Count; i++)
            {
                if (ScreenRealParams.SelectedElement[i].ConnectedIndex < 0)
                {
                    selectedElementCollection.Add(ScreenRealParams.SelectedElement[i]);
                }
            }
            selectedElementCollection.Sort(delegate(IRectElement first, IRectElement second)
            {
                if (first.X < second.X)
                {
                    return 1;
                }
                else if (first.X > second.X)
                {
                    return -1;
                }
                else
                {
                    if (first.Y < second.Y)
                    {
                        return 1;
                    }
                    else if (first.Y > second.Y)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            });
            #endregion

            #region 将连线排序
            int count = selectedElementCollection.Count;
            if (count == 0)
            {
                string msg = "";
                CommonStaticMethod.GetLanguageString("请清除当前网口连线后再进行连线！", "Lang_PortPropPanel_ClearLine", out msg);
                _smartLCTViewModeBase.ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);

                return false;
            }

            return ReverseCollection(selectedElementCollection, count, false);
            #endregion
        }
        #endregion

        private bool ReverseCollection(List<IRectElement> selectedElementCollection, int count, bool isHor)
        {
            int lastX = 0;
            int lastY = 0;
            List<List<IRectElement>> rowList = new List<List<IRectElement>>();
            List<IRectElement> oneRow = null;

            for (int i = 0; i < count; i++)
            {
                int curX = (int)Math.Round(selectedElementCollection[i].X, 0);
                int curY = (int)Math.Round(selectedElementCollection[i].Y, 0);
                if (isHor)
                {
                    if (i == 0 ||
                        curY != lastY)
                    {
                        lastY = curY;
                        oneRow = new List<IRectElement>();
                        rowList.Add(oneRow);
                    }
                }
                else
                {
                    if (i == 0 ||
                        curX != lastX)
                    {
                        lastX = curX;
                        oneRow = new List<IRectElement>();
                        rowList.Add(oneRow);
                    }
                }
                IRectElement element = selectedElementCollection[i];
                oneRow.Add(element);
            }
            int senderIndex=ScreenRealParams.ScreenLayer.CurrentSenderIndex;
            int portIndex=ScreenRealParams.ScreenLayer.CurrentPortIndex;

            //判断由单卡转为多卡时，单卡是否存在共用一个网口的情况
            RectLayer screenlayer =(RectLayer) ScreenRealParams.ScreenLayer;
            while (screenlayer != null && screenlayer.EleType != ElementType.baseScreen)
            {
                screenlayer = (RectLayer)screenlayer.ParentElement;
            }
            ObservableCollection<int> senderList=new ObservableCollection<int>();
            if (screenlayer != null)
            {
                for (int i = 0; i < screenlayer.SenderConnectInfoList.Count; i++)
                {
                    if (screenlayer.SenderConnectInfoList[i].LoadSize.Width != 0 && screenlayer.SenderConnectInfoList[i].LoadSize.Height != 0)
                    {
                        senderList.Add(screenlayer.SenderConnectInfoList[i].SenderIndex);
                    }
                }
            }
            if (senderList.Count == 1 && senderList[0] != senderIndex)
            {
                //单卡变为多卡的情况下，检测单卡是否有网口共用的问题
                ObservableCollection<int> portShareConnect = new ObservableCollection<int>();
                int portIndexNum=0;
                for(int j=0;j<screenlayer.SenderConnectInfoList[senderList[0]].PortConnectInfoList.Count;j++)
                {
                    //网口portIndexNum在哪些屏中有用过
                    portIndexNum=screenlayer.SenderConnectInfoList[senderList[0]].PortConnectInfoList[j].PortIndex;
                    for (int i = 0; i < screenlayer.ElementCollection.Count; i++)
                    {
                        if (screenlayer.ElementCollection[i].EleType == ElementType.newLayer)
                        {
                            continue;
                        }
                        RectLayer screen = (RectLayer)((RectLayer)screenlayer.ElementCollection[i]).ElementCollection[0];
                        if(screen.SenderConnectInfoList[senderList[0]].PortConnectInfoList[portIndexNum].LoadSize.Height!=0 &&
                            screen.SenderConnectInfoList[senderList[0]].PortConnectInfoList[portIndexNum].LoadSize.Width != 0)
                        {
                            if(!portShareConnect.Contains(screen.ParentElement.ConnectedIndex))
                                portShareConnect.Add(screen.ParentElement.ConnectedIndex);
                        }
                    }
                    if (portShareConnect.Count > 1)
                    {
                        string msg = "";
                        CommonStaticMethod.GetLanguageString("多发送卡的情况下，不允许多个屏使用同一个网口！", "Lang_PortPropPanel_CannotUserOnePort", out msg);
                        string strScr = "";
                        CommonStaticMethod.GetLanguageString("显示屏", "Lang_Global_Screen", out strScr);
                        string strWarning = "";
                        CommonStaticMethod.GetLanguageString("使用了相同的网口。", "Lang_PortPropPanel_UseSamePort", out strWarning);
                        string strSender = "";
                        CommonStaticMethod.GetLanguageString("发送卡", "Lang_Global_SendingBoard", out strSender);
                        string strPort = "";
                        CommonStaticMethod.GetLanguageString("网口", "Lang_Global_NetPort", out strPort);


                        msg = msg + "\n";
                        
                        for (int m = 0; m < portShareConnect.Count; m++)
                        {
                            msg += strScr + (portShareConnect[m] + 1).ToString();
                        }
                        msg = msg + strWarning + "(" + strSender + (senderList[0] + 1).ToString() + strPort + (portIndexNum + 1).ToString() + ")";
                        _smartLCTViewModeBase.ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);
                        return false;
                    }
                    else
                    {
                        portShareConnect.Clear();
                    }
                }
            }



            //更新map
            //连线前发送卡的带载
    
            bool isSenderLoadSizeNull = false;//第一次添加该发送卡的数据
            if(ScreenRealParams.ScreenLayer.SenderConnectInfoList[senderIndex].LoadSize.Height==0 &&
                ScreenRealParams.ScreenLayer.SenderConnectInfoList[senderIndex].LoadSize.Width == 0)
            {
                isSenderLoadSizeNull = true;
            }
            Rect oldSenderLoadSize = new Rect();
            Point oldSenderPoint = new Point();
            Point oldMap = new Point();
            if (!isSenderLoadSizeNull)
            {
                oldSenderLoadSize = ScreenRealParams.ScreenLayer.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].LoadSize;
                oldSenderPoint = new Point(oldSenderLoadSize.X, oldSenderLoadSize.Y);
                //原来的map
                oldMap = new Point();
                for (int j = 0; j < ScreenRealParams.ScreenLayer.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].PortConnectInfoList.Count; j++)
                {
                    if (ScreenRealParams.ScreenLayer.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].PortConnectInfoList[j].LoadSize.Width != 0 &&
                        ScreenRealParams.ScreenLayer.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].PortConnectInfoList[j].LoadSize.Height != 0)
                    {
                        oldMap = ScreenRealParams.ScreenLayer.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].PortConnectInfoList[j].MapLocation;
                        break;
                    }
                }
            }

                for (int i = 0; i < rowList.Count; i++)
                {
                    List<IRectElement> tempRow = rowList[i];
                    if (i % 2 != 0)
                    {
                        tempRow.Reverse();
                    }
                    for (int j = 0; j < tempRow.Count; j++)
                    {
                        IRectElement element = tempRow[j];
                        int nextIndex = GetNextIndexInPort(ScreenRealParams.ScreenLayer);
                        element.SenderIndex = ScreenRealParams.ScreenLayer.CurrentSenderIndex;
                        element.PortIndex = ScreenRealParams.ScreenLayer.CurrentPortIndex;
                        element.ConnectedIndex = nextIndex;
                        if (nextIndex > 0)
                        {
                            addCollection.Add(element.FrontLine);
                        }

                    }
                }

            //连线后发送卡的带载
            if (((RectLayer)ScreenRealParams.ScreenLayer).IsStartSetMapLocation && (!isSenderLoadSizeNull))
            {
                //更新该发送卡的带载
                Rect newSenderLoadSize = ScreenRealParams.ScreenLayer.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].LoadSize;
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
                    for (int j = 0; j < ScreenRealParams.ScreenLayer.SenderConnectInfoList[senderIndex].PortConnectInfoList.Count; j++)
                    {
                        PortConnectInfo portConnect = ScreenRealParams.ScreenLayer.SenderConnectInfoList[senderIndex].PortConnectInfoList[j];
                        if (portConnect.LoadSize.Height != 0 && portConnect.LoadSize.Width != 0)
                        {
                            portConnect.MapLocation = newmapLocation;
                        }
                    }
                }
                if (difSenderPint.X == 0 && difSenderPint.Y == 0)
                {
                    ScreenRealParams.ScreenLayer.SenderConnectInfoList[senderIndex].PortConnectInfoList[portIndex].MapLocation = oldMap;
                }
            }

            return true;

        }

        private int GetNextIndexInPort(IRectLayer screenLayer)
        {
            int nextIndex = 0;

            for (int i = 0; i < screenLayer.SenderConnectInfoList.Count; i++)
            {
                if (screenLayer.SenderConnectInfoList[i].SenderIndex == ScreenRealParams.ScreenLayer.CurrentSenderIndex)
                {
                    for (int j = 0; j < screenLayer.SenderConnectInfoList[i].PortConnectInfoList.Count; j++)
                    {
                        if (screenLayer.SenderConnectInfoList[i].PortConnectInfoList[j].PortIndex == ScreenRealParams.ScreenLayer.CurrentPortIndex)
                        {
                            nextIndex = screenLayer.SenderConnectInfoList[i].PortConnectInfoList[j].MaxConnectIndex+1;
                        }
                    }
                }
            }
            return nextIndex;
            //int nextIndex = 0;
            //int curMaxIndex = -1;
            //ObservableCollection<IElement> portElementCollection = portLayer.ElementCollection;
            //for (int i = 0; i < portElementCollection.Count; i++)
            //{
            //    IElement element = portElementCollection[i];
            //    if (element is RectElement)
            //    {
            //        RectElement re = element as RectElement;
            //        int connectedIndex = re.ConnectedIndex;
            //        if (connectedIndex >= 0)
            //        {
            //            curMaxIndex = Math.Max(connectedIndex, curMaxIndex);
            //        }
            //    }
            //}
            //nextIndex = curMaxIndex + 1;
            //return nextIndex;
        }

        private static void OnCanExecArrangeCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            ScreenPropertyPanel panel = (ScreenPropertyPanel)sender;
            e.CanExecute = panel.OnCanExecArrange(sender);
        }
        private bool OnCanExecArrange(object sender)
        {
            ScreenPropertyPanel panel = (ScreenPropertyPanel)sender;
            if (panel.ScreenRealParams == null ||
                panel.ScreenRealParams.SelectedElement == null || 
                panel.ScreenRealParams.SelectedElement.Count==0 || 
                panel.ScreenRealParams.ScreenLayer.ElementCollection.Count == 0 ||
                panel.ScreenRealParams.ScreenLayer.CLineType != ConnectLineType.Auto ||
                ScreenRealParams.ScreenLayer.CurrentSenderIndex < 0 ||
                ScreenRealParams.ScreenLayer.CurrentPortIndex < 0)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < panel.ScreenRealParams.SelectedElement.Count; i++)
                {
                    if (panel.ScreenRealParams.SelectedElement[i].ConnectedIndex > 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        #endregion

        private static void OnCustomReceiveSizeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ScreenPropertyPanel control = sender as ScreenPropertyPanel;
            if (control != null)
            {
                control.OnCustomReceiveSize();
            }
        }
        private void OnCustomReceiveSize()
        {
            if (SelectedScannerConfig == null)
            {
                return;
            }
            if (SelectedScannerConfig.ScanBdSizeType == ScannerSizeType.Custom)
            {               
                CustomReceiveResult info=new CustomReceiveResult();
                info.Width=64;
                info.Height=64;
                NotificationMessageAction<CustomReceiveResult> nsa =
                new NotificationMessageAction<CustomReceiveResult>(this, info, MsgToken.MSG_SHOWSETCUSTOMRECEIVESIZE, SetCustomReceiveNotifycationCallBack);
                Messenger.Default.Send(nsa, MsgToken.MSG_SHOWSETCUSTOMRECEIVESIZE);

            }
        }

        private void SetCustomReceiveNotifycationCallBack(CustomReceiveResult info)
        {
            if (info.IsOK != true)
            {
                if (ScannerCofigCollection != null && ScannerCofigCollection.Count == 1)
                {
                    this.SelectedScannerConfig = null;
                }
                else
                {
                    this.SelectedScannerConfig = ScannerCofigCollection[0];
                }
            }
            else
            {
                ScannerCofigInfo scanConfig = new ScannerCofigInfo();
                ScanBoardProperty scanBdProp = new ScanBoardProperty();
                scanBdProp.StandardLedModuleProp.DriverChipType = ChipType.Unknown;
                scanBdProp.ModCascadeType = ModuleCascadeDiretion.Unknown;
                scanBdProp.Width = info.Width;
                scanBdProp.Height = info.Height;
                scanConfig.ScanBdProp = scanBdProp;
                scanConfig.DisplayName = info.Width.ToString() + "_" + info.Height.ToString();
                scanConfig.ScanBdSizeType = ScannerSizeType.NoCustom;
                int index = -1;
                for (int i = 0; i < ScannerCofigCollection.Count; i++)
                {
                    if (ScannerCofigCollection[i].DisplayName == scanConfig.DisplayName)
                    {
                        index = i;
                        break;

                    }
                }
                if (index >= 0)
                {
                    SelectedScannerConfig = ScannerCofigCollection[index];
                }
                else
                {
                    ScannerCofigCollection.Insert(ScannerCofigCollection.Count - 1, scanConfig);
                }
            }
        }

        private static void OnCanExecManualLineCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            ScreenPropertyPanel panel = (ScreenPropertyPanel)sender;
            e.CanExecute = panel.OnCanExecManualLine(sender);
        }
        private bool OnCanExecManualLine(object sender)
        {
            
            ScreenPropertyPanel panel = (ScreenPropertyPanel)sender;
            if (panel.ScreenRealParams != null &&
               panel.ScreenRealParams.ScreenLayer != null)
            {
                if (panel.ScreenRealParams.ScreenLayer.CLineType == ConnectLineType.Auto)
                    this.Cursor = Cursors.Arrow;
                else if (panel.ScreenRealParams.ScreenLayer.CLineType == ConnectLineType.Manual)
                {
                    this.Cursor = Cursors.Hand;
                }
            }
            if (panel.ScreenRealParams == null ||
                panel.ScreenRealParams.SelectedElement == null ||
                panel.ScreenRealParams.ScreenLayer.ElementCollection.Count == 0 ||
                ScreenRealParams.ScreenLayer.CurrentPortIndex < 0 ||
                ScreenRealParams.ScreenLayer.CurrentSenderIndex < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private static void OnManualLineCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ScreenPropertyPanel control = sender as ScreenPropertyPanel;
            if (control != null)
            {
                control.OnManualLine();
            }
        }
        private void OnManualLine()
        {
            if (ScreenRealParams.ScreenLayer.CLineType == ConnectLineType.Manual)
            {
                ScreenRealParams.ScreenLayer.CLineType = ConnectLineType.Auto;
            }
            else
            {
                //判断是否使用其他屏用过的网口（多发送卡的情况下，多个显示屏不允许共用一个网口）
                IRectLayer screenLayer = ScreenRealParams.ScreenLayer;
                while (screenLayer != null && screenLayer.EleType != ElementType.baseScreen)
                {
                    screenLayer = (IRectLayer)screenLayer.ParentElement;
                }
                if (screenLayer != null)
                {
                    int senderIndexs = -1;
                    int sendercount = Function.FindSenderCount(screenLayer.SenderConnectInfoList, out senderIndexs);
                    if (sendercount > 1)
                    {
                        for (int i = 0; i < screenLayer.ElementCollection.Count; i++)
                        {
                            if (screenLayer.ElementCollection[i].EleType == ElementType.newLayer)
                            {
                                continue;
                            }
                            RectLayer screen = (RectLayer)(((RectLayer)screenLayer.ElementCollection[i]).ElementCollection[0]);
                            if (screen.ParentElement.ConnectedIndex == ScreenRealParams.ScreenLayer.ParentElement.ConnectedIndex)
                            {
                                continue;
                            }
                            if (screen.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].PortConnectInfoList[ScreenRealParams.ScreenLayer.CurrentPortIndex].LoadSize.Width != 0 &&
                                screen.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].PortConnectInfoList[ScreenRealParams.ScreenLayer.CurrentPortIndex].LoadSize.Height != 0)
                            {
                                string msg = "";
                                CommonStaticMethod.GetLanguageString("多发送卡的情况下，不允许多个屏使用同一个网口！", "Lang_PortPropPanel_CannotUserOnePort", out msg);
                                string strScr = "";
                                CommonStaticMethod.GetLanguageString("显示屏", "Lang_Global_Screen", out strScr);
                                string strWarning = "";
                                CommonStaticMethod.GetLanguageString("已经使用了该网口。", "Lang_PortPropPanel_HaveUseThisPort", out strWarning);

                                msg = msg + "\n" + strScr + " " + (screen.ParentElement.ConnectedIndex + 1).ToString() + " " + strWarning;

                                _smartLCTViewModeBase.ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);
                                IsChecked = false; 
                                return;
                            }
                        }
                    }
                }
                #region 多卡的情况下，判断不同的显示屏是否共用同一个网口
                int senderIndex = ScreenRealParams.ScreenLayer.CurrentSenderIndex;
                int portIndex = ScreenRealParams.ScreenLayer.CurrentPortIndex;

                //判断由单卡转为多卡时，单卡是否存在共用一个网口的情况
                RectLayer screenlayer = (RectLayer)ScreenRealParams.ScreenLayer;
                while (screenlayer != null && screenlayer.EleType != ElementType.baseScreen)
                {
                    screenlayer = (RectLayer)screenlayer.ParentElement;
                }
                ObservableCollection<int> senderList = new ObservableCollection<int>();
                if (screenlayer != null)
                {
                    for (int i = 0; i < screenlayer.SenderConnectInfoList.Count; i++)
                    {
                        if (screenlayer.SenderConnectInfoList[i].LoadSize.Width != 0 && screenlayer.SenderConnectInfoList[i].LoadSize.Height != 0)
                        {
                            senderList.Add(screenlayer.SenderConnectInfoList[i].SenderIndex);
                        }
                    }
                }
                if (senderList.Count == 1 && senderList[0] != senderIndex)
                {
                    //单卡变为多卡的情况下，检测单卡是否有网口共用的问题
                    ObservableCollection<int> portShareConnect = new ObservableCollection<int>();
                    int portIndexNum = 0;
                    for (int j = 0; j < screenlayer.SenderConnectInfoList[senderList[0]].PortConnectInfoList.Count; j++)
                    {
                        //网口portIndexNum在哪些屏中有用过
                        portIndexNum = screenlayer.SenderConnectInfoList[senderList[0]].PortConnectInfoList[j].PortIndex;
                        for (int i = 0; i < screenlayer.ElementCollection.Count; i++)
                        {
                            if (screenlayer.ElementCollection[i].EleType == ElementType.newLayer)
                            {
                                continue;
                            }
                            RectLayer screen = (RectLayer)((RectLayer)screenlayer.ElementCollection[i]).ElementCollection[0];
                            if (screen.SenderConnectInfoList[senderList[0]].PortConnectInfoList[portIndexNum].LoadSize.Height != 0 &&
                                screen.SenderConnectInfoList[senderList[0]].PortConnectInfoList[portIndexNum].LoadSize.Width != 0)
                            {
                                if (!portShareConnect.Contains(screen.ParentElement.ConnectedIndex))
                                    portShareConnect.Add(screen.ParentElement.ConnectedIndex);
                            }
                        }
                        if (portShareConnect.Count > 1)
                        {
                            string msg = "使用多张发送卡时，不允许多屏使用同一个网口！\n发送卡" + (senderList[0] + 1).ToString() + "的网口" + (portIndexNum + 1).ToString() + "同时被";
                            for (int m = 0; m < portShareConnect.Count; m++)
                            {
                                msg += "屏" + (portShareConnect[m] + 1).ToString();
                            }
                            msg += "使用。";
                            _smartLCTViewModeBase.ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);
                            IsChecked = false;
                            return;
                        }
                        else
                        {
                            portShareConnect.Clear();
                        }
                    }
                }        
                #endregion

                ScreenRealParams.ScreenLayer.CLineType = ConnectLineType.Manual;
                this.Cursor = Cursors.Hand;
            }
        }

        private static void OnAddPortIndexCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ScreenPropertyPanel control = sender as ScreenPropertyPanel;
            if (control != null)
            {
                control.OnAddPortIndex();
            }
        }
        private void OnAddPortIndex()
        {
            List<int> portIndexList = new List<int>();
            int addPortIndex = -1;
            for (int portIndex = 0; portIndex < ScreenRealParams.ScreenLayer.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].PortConnectInfoList.Count; portIndex++)
            {
                portIndexList.Add(ScreenRealParams.ScreenLayer.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].PortConnectInfoList[portIndex].PortIndex);
            }
            if (portIndexList.Count != 0)
            {
                portIndexList.Sort(delegate(int first, int second)
                {
                    return first.CompareTo(second);
                });
                addPortIndex = portIndexList[portIndexList.Count - 1] + 1;
            }
            else
            {
                addPortIndex = 0;
            }
            int senderIndex = ScreenRealParams.ScreenLayer.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].SenderIndex;
            ScreenRealParams.ScreenLayer.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].PortConnectInfoList.Add(new PortConnectInfo(addPortIndex,senderIndex, -1, null, null, new Rect()));
            AddPortIndex(addPortIndex);
            SetPortTogButSelectedState(_portCanvas, addPortIndex);
            ScreenRealParams.ScreenLayer.CurrentPortIndex = addPortIndex;
            ScreenRealParams.ScreenLayer.CurrentPortIndex = ScreenRealParams.ScreenLayer.CurrentPortIndex;
        }
        private static void OnCanExecAddPortIndexCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            ScreenPropertyPanel panel = (ScreenPropertyPanel)sender;
            e.CanExecute = panel.OnCanExecAddPortIndex();
        }
        private bool OnCanExecAddPortIndex()
        {
            if (ScreenRealParams == null
                || ScreenRealParams.ScreenLayer == null)
            {
                return false;
            }
            if (ScreenRealParams.ScreenLayer.CurrentSenderIndex < 0)
            {
                return false;
            }
            if (ScreenRealParams.ScreenLayer.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].PortConnectInfoList.Count > 3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //添加发送卡
        private static void OnCanExecAddSenderIndexCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            ScreenPropertyPanel panel = (ScreenPropertyPanel)sender;
            e.CanExecute = panel.OnCanExecAddSenderIndex();
        }
        private bool OnCanExecAddSenderIndex()
        {
            if (ScreenRealParams == null
                || ScreenRealParams.ScreenLayer == null)
            {
                return false;
            }
            if (ScreenRealParams.ScreenLayer.SenderConnectInfoList.Count > 9)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private static void OnAddSenderIndexCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ScreenPropertyPanel control = sender as ScreenPropertyPanel;
            if (control != null)
            {
                control.OnAddSenderIndex();
            }
        }
        private void OnAddSenderIndex()
        {
            List<int> senderIndexList = new List<int>();
            int addSenderIndex = -1;
            for (int senderIndex = 0; senderIndex < ScreenRealParams.ScreenLayer.SenderConnectInfoList.Count; senderIndex++)
            {
                senderIndexList.Add(ScreenRealParams.ScreenLayer.SenderConnectInfoList[senderIndex].SenderIndex);
            }
            if (senderIndexList.Count != 0)
            {
                senderIndexList.Sort(delegate(int first, int second)
                {
                    return first.CompareTo(second);
                });
                addSenderIndex = senderIndexList[senderIndexList.Count - 1] + 1;
            }
            else
            {
                addSenderIndex = 0;
            }
            SenderConnectInfo senderConnectInfo = new SenderConnectInfo();
            ObservableCollection<PortConnectInfo> portConnectList = new ObservableCollection<PortConnectInfo>();
            for (int i = 0; i < CurrentSenderConfigInfo.PortCount; i++)
            {
                portConnectList.Add(new PortConnectInfo(i, addSenderIndex, - 1, null, null, new Rect()));
            }
            senderConnectInfo.SenderIndex = addSenderIndex;
            senderConnectInfo.PortConnectInfoList = portConnectList;
            ScreenRealParams.ScreenLayer.SenderConnectInfoList.Insert(addSenderIndex,senderConnectInfo);
            AddSenderIndex(addSenderIndex);
            SetSenderTogButSelectedState(_senderCanvas, addSenderIndex);
            ScreenRealParams.ScreenLayer.CurrentSenderIndex = addSenderIndex;
            ScreenRealParams.ScreenLayer.CurrentPortIndex = 0;
            SetPortTogButSelectedState(_portCanvas, 0);
            ScreenRealParams.ScreenLayer.CurrentPortIndex = ScreenRealParams.ScreenLayer.CurrentPortIndex;
            ScreenRealParams.ScreenLayer.CurrentSenderIndex = ScreenRealParams.ScreenLayer.CurrentSenderIndex;
        }

        //删除发送卡
        private static void OnCanExecRemoveSenderIndexCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            ScreenPropertyPanel panel = (ScreenPropertyPanel)sender;
            e.CanExecute = panel.OnCanExecRemoveSenderIndex();
        }
        private bool OnCanExecRemoveSenderIndex()
        {
 
            if(ScreenRealParams!=null && ScreenRealParams.ScreenLayer!=null && ScreenRealParams.ScreenLayer.SenderConnectInfoList!=null
                && ScreenRealParams.ScreenLayer.SenderConnectInfoList.Count == 1)
            {
                return false;
            }
            if (ScreenRealParams != null && ScreenRealParams.ScreenLayer != null && ScreenRealParams.ScreenLayer.CurrentSenderIndex < 0)
            {
                return false;
            }
            return true;

        }
        private static void OnRemoveSenderIndexCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ScreenPropertyPanel control = sender as ScreenPropertyPanel;
            if (control != null)
            {
                control.OnRemoveSenderIndex();
            }
        }
        private void OnRemoveSenderIndex()
        {           
                RectLayer removeLayer = (RectLayer)ScreenRealParams.ScreenLayer;
                while (removeLayer != null && removeLayer.EleType != ElementType.baseScreen)
                {
                    removeLayer = (RectLayer)removeLayer.ParentElement;
                }
                if (removeLayer != null)
                {
                    for (int m = 0; m < removeLayer.SenderConnectInfoList.Count; m++)
                    {
                        if (removeLayer.SenderConnectInfoList[m].SenderIndex == ScreenRealParams.ScreenLayer.CurrentSenderIndex)
                        {
                            removeLayer.CurrentSenderIndex = ScreenRealParams.ScreenLayer.CurrentSenderIndex;
                            removeLayer.SenderConnectInfoList.RemoveAt(m);
                            break;
                       }
                    }
                    for (int index = 0; index < removeLayer.SenderConnectInfoList.Count; index++)
                    {
                        if (removeLayer.SenderConnectInfoList[index].SenderIndex > ScreenRealParams.ScreenLayer.CurrentSenderIndex)
                        {
                            removeLayer.SenderConnectInfoList[index].SenderIndex -= 1;
                        }
                    }
                }
                //if (ScreenRealParams.ScreenLayer.SenderConnectInfoList[senderIndex].SenderIndex == _currentSenderIndex)
                //{
                //    #region 处理与要删除的发送卡关联的信息（该发送卡下的箱体及连线）
                //  //  UpdateSenderConnectInfo(ScreenRealParams.ScreenLayer.SenderConnectInfoList[senderIndex]);
                //    #endregion
                //    ScreenRealParams.ScreenLayer.SenderConnectInfoList.RemoveAt(senderIndex);
                //    break;
                //}
            
            //for (int senderIndex = 0; senderIndex < ScreenRealParams.ScreenLayer.SenderConnectInfoList.Count; senderIndex++)
            //{
            //    if (ScreenRealParams.ScreenLayer.SenderConnectInfoList[senderIndex].SenderIndex > _currentSenderIndex)
            //    {
            //        ScreenRealParams.ScreenLayer.SenderConnectInfoList[senderIndex].SenderIndex -= 1;
            //    }
            //}
            int oldPortIndex = -1;
            int oldSenderIndex = -1;
    //        ClearPortIndex(out oldPortIndex);
            ClearSenderIndex(out oldSenderIndex);
            for (int senderIndex = 0; senderIndex < ScreenRealParams.ScreenLayer.SenderConnectInfoList.Count; senderIndex++)
            {
                AddSenderIndex(ScreenRealParams.ScreenLayer.SenderConnectInfoList[senderIndex].SenderIndex);
                //for (int portIndex = 0; portIndex < PortRealParams.PortLayer.SenderAndPortList[senderIndex].PortIndexList.Count; portIndex++)
                //{
                //    AddPortIndex(PortRealParams.PortLayer.SenderAndPortList[senderIndex].PortIndexList[portIndex]);
                //}
            }
            //ClearPortIndex(out oldPortIndex);
            //for (int portIndex = 0; portIndex < CurrentSenderConfigInfo.PortCount; portIndex++)
            //{
            //    AddPortIndex(portIndex);
            //}
            SetPortTogButSelectedState(_portCanvas, -1);
            ScreenRealParams.ScreenLayer.CurrentPortIndex = -1;
            ScreenRealParams.ScreenLayer.CurrentSenderIndex = -1;
}
        

        #region 添加发送卡和网口
        private void AddSenderIndex(int senderIndex)
        {
            if (_senderCanvas == null)
            {
                return;
            }
            ToggleButton togBut = new ToggleButton();
            togBut.Width = 30;
            togBut.Height = 30;
            togBut.Content = (senderIndex + 1).ToString();
            togBut.Command = new RelayCommand<ToggleButton>(OnToggleButtonWithSenderCmd);
            togBut.SetResourceReference(ToggleButton.StyleProperty, "MyToggleButtonStyle");
            togBut.CommandParameter = togBut;
            Thickness margin = new Thickness();
            if (senderIndex < 5)
            {
                margin.Left = 10 + (togBut.Width + 15) * senderIndex;
                margin.Top = 15;
            }
            else
            {
                margin.Left = 10 + (togBut.Width + 15) * (senderIndex - 5);
                margin.Top = 50;
            }
            togBut.Margin = margin;

            if (SenderAndPortPicCollection != null && SenderAndPortPicCollection.Count != 0)
            {
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri(SenderAndPortPicCollection[0].NoSelectedPicPath, UriKind.Absolute));
                imageBrush.Stretch = Stretch.Fill;//设置图像的显示格式
                togBut.Background = imageBrush;
            }   


            _senderCanvas.Children.Add(togBut);
            //int oldPortIndex = -1;
            //ClearPortIndex(out oldPortIndex);
            //for (int index = 0; index < CurrentSenderConfigInfo.PortCount; index++)
            //{
            //    AddPortIndex(index);
            //}
            //_currentPortIndex = oldPortIndex;
            //ScreenRealParams.ScreenLayer.CurrentPortIndex = _currentPortIndex;
        }
        private void AddPortIndex(int portIndex)
        {
            if (_portCanvas == null)
            {
                return; 
            }
            ToggleButton togBut = new ToggleButton();
            togBut.Width = 30;
            togBut.Height = 30;
            togBut.Content = (portIndex + 1).ToString();
            togBut.Command = new RelayCommand<ToggleButton>(OnToggleButtonWithPortCmd);
            togBut.SetResourceReference(ToggleButton.StyleProperty, "MyToggleButtonStyle");
            
            togBut.CommandParameter = togBut;
            Thickness margin = new Thickness();
            margin.Left = 10 + (togBut.Width + 15) * portIndex;
            margin.Top = 15;
            togBut.Margin = margin;

            //给网口加载图片
            int senderIndex =ScreenRealParams.ScreenLayer.CurrentSenderIndex;
            if (senderIndex == -1)
            {
                senderIndex = 0;
            }
            int colorIndex = 4 * senderIndex + portIndex+1;
            if (SenderAndPortPicCollection != null && SenderAndPortPicCollection.Count != 0)
            {
                if (!SenderAndPortPicCollection.Keys.Contains(colorIndex))
                {
                    colorIndex = 0;
                }
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri(SenderAndPortPicCollection[colorIndex].NoSelectedPicPath, UriKind.Absolute));
                imageBrush.Stretch = Stretch.Fill;//设置图像的显示格式
                togBut.Background = imageBrush;
            }

            _portCanvas.Children.Add(togBut);
        }


        private void ClearPortIndex(out int checkIndex)
        {
            checkIndex = -1;
            if (_portCanvas == null)
            {
                return;
            }
            for (int portIndex = 0; portIndex < _portCanvas.Children.Count; portIndex++)
            {
                if (_portCanvas.Children[portIndex] is ToggleButton)
                {
                    if ((bool)((ToggleButton)_portCanvas.Children[portIndex]).IsChecked)
                    {
                        checkIndex = int.Parse(((ToggleButton)_portCanvas.Children[portIndex]).Content.ToString()) - 1;
                    }
                    _portCanvas.Children.RemoveAt(portIndex);
                    portIndex = portIndex - 1;
                }
            }
            //_currentPortIndex = -1;
        }
        private void RemoveSenderIndex(int index)
        {
            if (_senderCanvas == null)
            {
                return;
            }
            for (int senderIndex = 0; senderIndex < _senderCanvas.Children.Count; senderIndex++)
            {
                if (_senderCanvas.Children[senderIndex] is ToggleButton)
                {
                    int togButContent=int.Parse(((ToggleButton)_senderCanvas.Children[senderIndex]).Content.ToString());
                    if (togButContent==index)
                    {
                        _senderCanvas.Children.RemoveAt(senderIndex);
                        return;
                    }                          
                }
            }
        }
        private void UpdateSenderIndex(int index)
        {
            if (_senderCanvas == null)
            {
                return;
            }
            for (int senderIndex = 0; senderIndex < _senderCanvas.Children.Count; senderIndex++)
            {
                if (_senderCanvas.Children[senderIndex] is ToggleButton)
                {
                    int togButContent = int.Parse(((ToggleButton)_senderCanvas.Children[senderIndex]).Content.ToString());
                    if (togButContent > index)
                    {
                        ((ToggleButton)_senderCanvas.Children[senderIndex]).Content = togButContent - 1;
                    }
                }
            }
        }
        private void ClearSenderIndex(out int checkIndex)
        {
            checkIndex = -1;
            if (_senderCanvas == null)
            {
                return;
            }
            for (int senderIndex = 0; senderIndex < _senderCanvas.Children.Count; senderIndex++)
            {
                if (_senderCanvas.Children[senderIndex] is ToggleButton)
                {
                    if ((bool)((ToggleButton)_senderCanvas.Children[senderIndex]).IsChecked)
                    {
                        checkIndex = int.Parse(((ToggleButton)_senderCanvas.Children[senderIndex]).Content.ToString()) - 1;
                    }
                    _senderCanvas.Children.RemoveAt(senderIndex);
                    senderIndex = senderIndex - 1;
                }
            }
            //    _currentSenderIndex = -1;
        }
        private void CancelPortIndexIsChecked()
        {
            for (int index = 0; index < _portCanvas.Children.Count; index++)
            {
                if (_portCanvas.Children[index] is ToggleButton)
                {
                    ((ToggleButton)_portCanvas.Children[index]).IsChecked = false;
                }
            }
            ScreenRealParams.ScreenLayer.CurrentPortIndex = -1;

        }
        private void CancelSenderIndexIsChecked()
        {
            for (int index = 0; index < _senderCanvas.Children.Count; index++)
            {
                if (_senderCanvas.Children[index] is ToggleButton)
                {
                    ((ToggleButton)_senderCanvas.Children[index]).IsChecked = false;
                }
            }
            ScreenRealParams.ScreenLayer.CurrentSenderIndex = -1;
        }
        //发送卡按钮
        private void SetSenderTogButSelectedState(Canvas canvas, int togIndex)
        {
            if (canvas == null)
            {
                return;
            }
            for (int index = 0; index < canvas.Children.Count; index++)
            {
                if (canvas.Children[index] is ToggleButton)
                {
                    ToggleButton togBut = ((ToggleButton)canvas.Children[index]);
                    if (((ToggleButton)canvas.Children[index]).Content.ToString() == (togIndex + 1).ToString())
                    {

                        togBut.IsChecked = true;                  
                        //给网口加载图片
                        if (SenderAndPortPicCollection != null && SenderAndPortPicCollection.Count != 0)
                        {
                            ImageBrush imageBrush = new ImageBrush();
                            imageBrush.ImageSource = new BitmapImage(new Uri(SenderAndPortPicCollection[0].SelectedPicPath, UriKind.Absolute));
                            imageBrush.Stretch = Stretch.Fill;//设置图像的显示格式
                            togBut.Background = imageBrush;
                        }                 
                    }
                    else
                    {
                        togBut.IsChecked = false;
                        //给网口加载图片
                        if (SenderAndPortPicCollection != null && SenderAndPortPicCollection.Count != 0)
                        {
                            ImageBrush imageBrush = new ImageBrush();
                            imageBrush.ImageSource = new BitmapImage(new Uri(SenderAndPortPicCollection[0].NoSelectedPicPath, UriKind.Absolute));
                            imageBrush.Stretch = Stretch.Fill;//设置图像的显示格式
                            togBut.Background = imageBrush;
                        }      
                    }
                }
            }
        }
        //网口按钮
        private void SetPortTogButSelectedState(Canvas canvas, int togIndex)
        {
            if (canvas == null)
            {
                return;
            }
            for (int index = 0; index < canvas.Children.Count; index++)
            {
                if (canvas.Children[index] is ToggleButton)
                {
                    ToggleButton togBut = ((ToggleButton)canvas.Children[index]);
                    int portIndex = int.Parse(((ToggleButton)canvas.Children[index]).Content.ToString())-1;
                    int senderIndex = ScreenRealParams.ScreenLayer.CurrentSenderIndex;
                    if (senderIndex == -1)
                    {
                        senderIndex = 0;
                    }                
                    int colorIndex = 4 * senderIndex + portIndex+1;
                    if (portIndex == togIndex)
                    {

                        togBut.IsChecked = true;
                        //给网口加载图片

                        if (SenderAndPortPicCollection != null && SenderAndPortPicCollection.Count != 0)
                        {
                            if (!SenderAndPortPicCollection.Keys.Contains(colorIndex))
                            {
                                colorIndex = 0;
                            }
                            ImageBrush imageBrush = new ImageBrush();
                            imageBrush.ImageSource = new BitmapImage(new Uri(SenderAndPortPicCollection[colorIndex].SelectedPicPath, UriKind.Absolute));
                            imageBrush.Stretch = Stretch.Fill;//设置图像的显示格式
                            togBut.Background = imageBrush;
                        }
                    }
                    else
                    {
                        togBut.IsChecked = false;
                        //给网口加载图片
                        if (SenderAndPortPicCollection != null && SenderAndPortPicCollection.Count != 0)
                        {
                            if (!SenderAndPortPicCollection.Keys.Contains(colorIndex))
                            {
                                colorIndex = 0;
                            }
                            ImageBrush imageBrush = new ImageBrush();
                            imageBrush.ImageSource = new BitmapImage(new Uri(SenderAndPortPicCollection[colorIndex].NoSelectedPicPath, UriKind.Absolute));
                            imageBrush.Stretch = Stretch.Fill;//设置图像的显示格式
                            togBut.Background = imageBrush;
                        }
                    }
                }
            }
        }

        #endregion

        #region 点击发送卡和网口序号
        private void OnToggleButtonWithSenderCmd(ToggleButton togBut)
        {
            ScreenRealParams.ScreenLayer.CurrentSenderIndex = int.Parse(togBut.Content.ToString()) - 1;
            SetSenderTogButSelectedState(_senderCanvas, ScreenRealParams.ScreenLayer.CurrentSenderIndex);
            int oldIndex = -1;
            ClearPortIndex(out oldIndex);
            for (int portIndex = 0; portIndex < CurrentSenderConfigInfo.PortCount; portIndex++)
            {
                AddPortIndex(portIndex);
            }
            //for (int portIndex = 0; portIndex < ScreenRealParams.ScreenLayer.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].PortConnectInfoList.Count; portIndex++)
            //{
            //    AddPortIndex(ScreenRealParams.ScreenLayer.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].PortConnectInfoList[portIndex].PortIndex);
            //}
            ScreenRealParams.ScreenLayer.CurrentPortIndex = ScreenRealParams.ScreenLayer.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].SelectedPortIndex;
            SetPortTogButSelectedState(_portCanvas, ScreenRealParams.ScreenLayer.CurrentPortIndex);
        }
        private void OnToggleButtonWithPortCmd(ToggleButton togBut)
        {
            if (ScreenRealParams.ScreenLayer.CurrentSenderIndex == -1)
            {
                return;
            }

            ScreenRealParams.ScreenLayer.CurrentPortIndex = int.Parse(togBut.Content.ToString()) - 1;
            ScreenRealParams.ScreenLayer.SenderConnectInfoList[ScreenRealParams.ScreenLayer.CurrentSenderIndex].SelectedPortIndex = ScreenRealParams.ScreenLayer.CurrentPortIndex;
            SetPortTogButSelectedState(_portCanvas, ScreenRealParams.ScreenLayer.CurrentPortIndex);
        }
        #endregion

        #region PortRealParams集合变化处理
        private static void OnScreenRealParamsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScreenPropertyPanel ctrl = (ScreenPropertyPanel)d;
            ctrl.OnScreenRealParamsSet(e,ctrl);
        }
        private void OnScreenRealParamsSet(DependencyPropertyChangedEventArgs e,ScreenPropertyPanel ctrl)
        {
            if (e.Property == ScreenRealParamsProperty)
            {
                ScreenRealParameters oldParams = e.OldValue as ScreenRealParameters;
                ScreenRealParameters newParams = e.NewValue as ScreenRealParameters;

                if (oldParams != null &&
                    oldParams.ScreenLayer != null &&
                    oldParams.ScreenLayer.ElementCollection != null)
                {
                    oldParams.ScreenLayer.ElementCollection.CollectionChanged -= new NotifyCollectionChangedEventHandler(OnScreenElementCollectionChanged);
                }

                if (newParams != null &&
                    newParams.ScreenLayer != null &&
                    newParams.ScreenLayer.ElementCollection != null)
                {
                    newParams.ScreenLayer.ElementCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(OnScreenElementCollectionChanged);
                    OnScreenElementCollectionChanged(null, null);
                }
                //if (newParams != null &&
                //    newParams.PortLayer != null &&
                //    newParams.PortLayer.ParentElement != null)
                //{
                //    MaxPortHeight = ((RectLayer)newParams.PortLayer.ParentElement).Height;
                //    MaxPortWidth = ((RectLayer)newParams.PortLayer.ParentElement).Width;
                //}
               // ctrl.Cursor = Cursors.Arrow;
                if(newParams!=null &&
                    newParams.ScreenLayer != null)
                {
                    IRectLayer parentLayer = (IRectLayer)newParams.ScreenLayer.ParentElement;
                    while (parentLayer != null && parentLayer.EleType != ElementType.baselayer)
                    {
                        parentLayer = (IRectLayer)parentLayer.ParentElement;
                    }
                    if (parentLayer != null && parentLayer.EleType == ElementType.baselayer)
                    {
                        SmartLCTActionManager = parentLayer.MyActionManager;
                    }
                }
            }

            //if (ScannerCofigCollection == null ||
            //    ScannerCofigCollection.Count == 0)
            //{
            //    ScannerCofigInfo customScannerConfigInfo = new ScannerCofigInfo();
            //    string strCustom = "";
            //    CommonStaticMethod.GetLanguageString("自定义", "Lang_Global_Custom", out strCustom);
            //    customScannerConfigInfo.DisplayName = strCustom;
            //    customScannerConfigInfo.ScanBdSizeType = ScannerSizeType.Custom;       
            //    ScannerCofigCollection = new ObservableCollection<ScannerCofigInfo>();
            //    ScannerCofigCollection.Add(customScannerConfigInfo);
            //}
            //else
            //{
            //    bool isHaveCustom = false;
            //    for (int i = 0; i < ScannerCofigCollection.Count; i++)
            //    {
            //        if (ScannerCofigCollection[i].ScanBdSizeType == ScannerSizeType.Custom)
            //        {
            //            isHaveCustom = true;
            //            break;
            //        }
            //    }
            //    if (!isHaveCustom)
            //    {
            //        ScannerCofigInfo customScannerConfigInfo = new ScannerCofigInfo();
            //        string strCustom = "";
            //        CommonStaticMethod.GetLanguageString("自定义", "Lang_Global_Custom", out strCustom);
            //        customScannerConfigInfo.DisplayName = strCustom;
            //        customScannerConfigInfo.ScanBdSizeType = ScannerSizeType.Custom;
            //        ScannerCofigCollection.Insert(ScannerCofigCollection.Count,customScannerConfigInfo);
            //    }
            //}
            //if (ScannerCofigCollection != null && ScannerCofigCollection.Count == 1)
            //{
            //    SelectedScannerConfig = null;
            //}
            //else if (ScannerCofigCollection != null && ScannerCofigCollection.Count>1 && 
            //    SelectedScannerConfig.ScanBdSizeType == ScannerSizeType.Custom)
            //{
            //    SelectedScannerConfig = ScannerCofigCollection[0];
            //}
           // ctrl.DragOver += new DragEventHandler(ctrl_DragOver);
           // ctrl.Drop += new DragEventHandler(ctrl_Drop);
        }
        //void addSenderBut_PreviewMouseMove(object sender, MouseEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //    //    this.StartDrag(e);
        //    }
        //}

        //private void StartDrag(MouseEventArgs e)
        //{
        //    this._dragScope = Application.Current.MainWindow.Content as FrameworkElement;

        //    this._dragScope.AllowDrop = true;
        //    Rectangle a = new Rectangle();
        //    a.Width = 100;
        //    a.Height = 100;
        //    a.Fill = Brushes.Red;
        //    a.StrokeThickness = 5;

        //    a.Margin = new Thickness(0, 0, 100, 100);
        //    DragEventHandler draghandler = new DragEventHandler(DragScope_PreviewDragOver);
        //    this._dragScope.PreviewDragOver += draghandler;
        //    this._adorner = new DragAdorner(this._dragScope, 0.5);
        //    this._layer = AdornerLayer.GetAdornerLayer(this._dragScope as Visual);
        //    this._layer.Add(this._adorner);

        //    DataObject data = new DataObject(typeof(Rectangle), a);
        //    DragDrop.DoDragDrop(a, data, DragDropEffects.Move);

        //    AdornerLayer.GetAdornerLayer(this._dragScope).Remove(this._adorner);
        //    this._adorner = null;

        //    this._dragScope.PreviewDragOver -= draghandler;
        //}

        //void DragScope_PreviewDragOver(object sender, DragEventArgs args)
        //{
        //    if (this._adorner != null)
        //    {
        //        this._adorner.LeftOffset = args.GetPosition(this._dragScope).X;
        //        this._adorner.TopOffset = args.GetPosition(this._dragScope).Y;
        //    }
        //}
        private void OnScreenElementCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {          
            int oldPortIndex = -1;
            int oldSenderIndex = -1;
            ClearPortIndex(out oldPortIndex);
            ClearSenderIndex(out oldSenderIndex);
            for (int senderIndex = 0; senderIndex < ScreenRealParams.ScreenLayer.SenderConnectInfoList.Count; senderIndex++)
            {
                AddSenderIndex(ScreenRealParams.ScreenLayer.SenderConnectInfoList[senderIndex].SenderIndex);
            }
            //CurrentSenderConfigInfo = ((RectLayer)ScreenRealParams.ScreenLayer).SelectedSenderConfigInfo;
            for (int portIndex = 0; portIndex < CurrentSenderConfigInfo.PortCount; portIndex++)
            {
                AddPortIndex(portIndex);
            }
            //显示所属发送卡和网口
            bool isSenderEqual = true;
            bool isPortEqual = true;
            int senderNum = -1;
            int portNum = -1;
            if (ScreenRealParams.SelectedElement.Count > 0)
            {
                senderNum = ScreenRealParams.SelectedElement[0].SenderIndex;
                portNum = ScreenRealParams.SelectedElement[0].PortIndex;
                if (senderNum >= 0 && portNum >= 0)
                {
                    for (int index = 1; index < ScreenRealParams.SelectedElement.Count; index++)
                    {
                        if (ScreenRealParams.SelectedElement[index].SenderIndex != senderNum)
                        {
                            isSenderEqual = false;
                            isPortEqual = false;
                            break;
                        }
                        if (ScreenRealParams.SelectedElement[index].PortIndex != portNum)
                        {
                            isPortEqual = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                isPortEqual = false;
                isSenderEqual = false;
            }

            if (isSenderEqual && isPortEqual && senderNum >= 0 && portNum >= 0)
            {

                SetSenderTogButSelectedState(_senderCanvas, senderNum);
                ScreenRealParams.ScreenLayer.CurrentSenderIndex = senderNum;
                int oldIndex = -1;
                //ClearPortIndex(out oldIndex);
                //for (int portIndex = 0; portIndex < ScreenRealParams.ScreenLayer.SenderConnectInfoList[_currentSenderIndex].PortConnectInfoList.Count; portIndex++)
                //{
                //    AddPortIndex(ScreenRealParams.ScreenLayer.SenderConnectInfoList[_currentSenderIndex].PortConnectInfoList[portIndex].PortIndex);
                //}
                SetPortTogButSelectedState(_portCanvas, portNum);
                ScreenRealParams.ScreenLayer.CurrentPortIndex = portNum;
                ScreenRealParams.ScreenLayer.CurrentPortIndex = ScreenRealParams.ScreenLayer.CurrentPortIndex;
                ScreenRealParams.ScreenLayer.CurrentSenderIndex = ScreenRealParams.ScreenLayer.CurrentSenderIndex;
            }
            else
            {
                ScreenRealParams.ScreenLayer.CurrentPortIndex = oldPortIndex;
                ScreenRealParams.ScreenLayer.CurrentSenderIndex = oldSenderIndex;
                int oldIndex = -1;
                if (ScreenRealParams.ScreenLayer.SenderConnectInfoList.Count < ScreenRealParams.ScreenLayer.CurrentSenderIndex)
                {
                    ScreenRealParams.ScreenLayer.CurrentSenderIndex = -1;
                }
                if (ScreenRealParams.ScreenLayer.CurrentSenderIndex >= 0)
                {
                    ClearPortIndex(out oldIndex);
                    for (int portIndex = 0; portIndex < CurrentSenderConfigInfo.PortCount; portIndex++)
                    {
                        AddPortIndex(portIndex);
                    }
                }
                if (ScreenRealParams.ScreenLayer.CurrentSenderIndex == -1)
                {
                    ScreenRealParams.ScreenLayer.CurrentSenderIndex = 0;
                }
                if (ScreenRealParams.ScreenLayer.CurrentPortIndex == -1)
                {
                    ScreenRealParams.ScreenLayer.CurrentPortIndex = 0;
                }
                ScreenRealParams.ScreenLayer.CurrentSenderIndex = ScreenRealParams.ScreenLayer.CurrentSenderIndex;
                ScreenRealParams.ScreenLayer.CurrentPortIndex = ScreenRealParams.ScreenLayer.CurrentPortIndex;
                SetSenderTogButSelectedState(_senderCanvas, ScreenRealParams.ScreenLayer.CurrentSenderIndex);
                SetPortTogButSelectedState(_portCanvas, ScreenRealParams.ScreenLayer.CurrentPortIndex);
            }

            if (ScreenRealParams.ScreenLayer.CLineType == ConnectLineType.Auto)
            {
                IsChecked = false;
            }
            else if (ScreenRealParams.ScreenLayer.CLineType == ConnectLineType.Manual)
            {
                IsChecked = true;
            }
            //  TotalLeftToAddedCount = TotalCanAddedCount - PortRealParams.PortLayer.ElementCollection.Count;

            //TotalLeftToAddedCount = TotalCanAddedCount - PortRealParams.PortLayer.ElementCollection.Count;

        }

        #endregion
        private void OnScannerCofigCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (ScannerCofigCollection.Count == 1)
                {
                    SelectedScannerConfig = null;
                }
                else
                {
                    if (((ScannerCofigInfo)e.NewItems[0]).ScanBdSizeType != ScannerSizeType.Custom)
                    {
                        SelectedScannerConfig = (ScannerCofigInfo)e.NewItems[0];
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (ScannerCofigCollection != null)
                {
                    if (ScannerCofigCollection.Count > 1)
                    {
                        SelectedScannerConfig = ScannerCofigCollection[0];
                    }
                    else if (ScannerCofigCollection.Count == 1)
                    {
                        SelectedScannerConfig = null;
                    }
                }
            }
        }

        private void ResetScannnerSetCustom()
        {
            //if (ScannerCofigCollection == null ||
            //    ScannerCofigCollection.Count == 0)
            //{
            //    ScannerCofigInfo customScannerConfigInfo = new ScannerCofigInfo();
            //    string strCustom = "";
            //    CommonStaticMethod.GetLanguageString("自定义", "Lang_Global_Custom", out strCustom);
            //    customScannerConfigInfo.DisplayName = strCustom;
            //    customScannerConfigInfo.ScanBdSizeType = ScannerSizeType.Custom;
            //    ScannerCofigCollection = new ObservableCollection<ScannerCofigInfo>();
            //    ScannerCofigCollection.Add(customScannerConfigInfo);
            //}
            //else
            {
                bool isHaveCustom = false;
                for (int i = 0; i < ScannerCofigCollection.Count; i++)
                {
                    if (ScannerCofigCollection[i].ScanBdSizeType == ScannerSizeType.Custom)
                    {
                        isHaveCustom = true;
                        break;
                    }
                }
                if (!isHaveCustom)
                {
                    ScannerCofigInfo customScannerConfigInfo = new ScannerCofigInfo();
                    string strCustom = "";
                    CommonStaticMethod.GetLanguageString("自定义", "Lang_Global_Custom", out strCustom);
                    customScannerConfigInfo.DisplayName = strCustom;
                    customScannerConfigInfo.ScanBdSizeType = ScannerSizeType.Custom;
                    ScannerCofigCollection.Insert(ScannerCofigCollection.Count, customScannerConfigInfo);
                }
            }
            if (ScannerCofigCollection != null && ScannerCofigCollection.Count == 1)
            {
                SelectedScannerConfig = null;
            }
            else if (ScannerCofigCollection != null && ScannerCofigCollection.Count > 1)
            {
                SelectedScannerConfig = ScannerCofigCollection[0];
            }
        }
        private static void OnScannerCofigCollectionChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ObservableCollection<ScannerCofigInfo> oldVal = e.OldValue as ObservableCollection<ScannerCofigInfo>;
            ObservableCollection<ScannerCofigInfo> newVal = e.NewValue as ObservableCollection<ScannerCofigInfo>;
            ScreenPropertyPanel panel = (ScreenPropertyPanel)d;
            panel.ScannerCofigCollection = panel.ScannerCofigCollection;
            panel.ResetScannnerSetCustom();
            if (newVal != null
                && newVal.Count > 0)
            {
                panel.SelectedScannerConfig = (ScannerCofigInfo)newVal[0];
            }
            else
            {
                panel.SelectedScannerConfig = null;
                
            }
        }
        private static void OnSenderConfigCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScreenPropertyPanel panel = (ScreenPropertyPanel)d;
            panel.OnSenderConfigCollection();
        }
        private void OnSenderConfigCollection()
        {
            PopupHeight = SenderConfigCollection.Count * 30;
        }
        private static void OnSelectedScannerConfigChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {        
            OnAddRowsCountChanged(d, e);
            OnAddColsCountChanged(d, e);
   
        }
        private static void OnAddRowsCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScreenPropertyPanel panel = (ScreenPropertyPanel)d;
            panel.OnAddRowsCount();
        }
        private void OnAddRowsCount()
        {
            if (SelectedScannerConfig == null || SelectedScannerConfig.ScanBdProp == null)
            {
                return;
            }
            ScanBoardProperty scanBdProp = SelectedScannerConfig.ScanBdProp;
            int height = scanBdProp.Height;
            double portHeight = SmartLCTViewModeBase.MaxScreenHeight;
            int maxRowsCount = (int)portHeight / height;
            if (AddRowsCount > maxRowsCount)
            {
                AddRowsCount = maxRowsCount;
            }
        }
        private static void OnAddColsCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScreenPropertyPanel panel = (ScreenPropertyPanel)d;
            panel.OnAddColsCount();
        }
        private void OnAddColsCount()
        {
            if (SelectedScannerConfig == null || SelectedScannerConfig.ScanBdProp == null)
            {
                return;
            }
            ScanBoardProperty scanBdProp = SelectedScannerConfig.ScanBdProp;
            int width = scanBdProp.Width;
            double portWidth = SmartLCTViewModeBase.MaxScreenWidth;
            int maxColsCount = (int)portWidth / width;
            if (AddColsCount > maxColsCount)
            {
                AddColsCount = maxColsCount;
            }
        }


        private static void OnShowScanBoardConfigManagerCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ScreenPropertyPanel panel = (ScreenPropertyPanel)sender;
            panel.OnShowScanBoardConfigManager();
        }
        private void OnShowScanBoardConfigManager()
        {
            Messenger.Default.Send<string>("", MsgToken.MSG_SHOWSCANBOARDCONFIGMANAGER);
        }
        #endregion

        #region  重载
        public override void OnApplyTemplate()
        {
            var senderCanvas = this.GetTemplateChild("MySenderCanvas") as Canvas;
            var portCanvas = this.GetTemplateChild("MyPortCanvas") as Canvas;
            var senderTypeText = this.GetTemplateChild("SenderTypeText") as TextBlock;
            var mypopup = this.GetTemplateChild("Mypopup") as Popup;
            _myPopup = mypopup;
            _senderTypeText = senderTypeText;
            _senderCanvas = senderCanvas;
            _portCanvas = portCanvas;
            var addSenderBut=this.GetTemplateChild("AddSenderBut") as Button;
            _addSenderBut = addSenderBut;
        }
        #endregion

    }
}
