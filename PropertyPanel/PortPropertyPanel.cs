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
using System.Diagnostics;
using Nova.Wpf.Control;
using Nova.LCT.GigabitSystem.Common;
using GalaSoft.MvvmLight.Messaging;
using GuiLabs.Undo;
using System.ComponentModel;
using System.Windows.Interop;
using System.Windows.Controls.Primitives;
using GalaSoft.MvvmLight.Command;

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
    ///     <MyNamespace:PortPropertyPanel/>
    ///
    /// </summary>
    public class PortPropertyPanel : Control
    {
        //#region Xaml属性
        //public static readonly DependencyProperty PortRealParamsProperty =
        //   DependencyProperty.Register(
        //       "PortRealParams", typeof(PortRealParameters), typeof(PortPropertyPanel),
        //       new FrameworkPropertyMetadata(null,
        //           new PropertyChangedCallback(OnPortRealParamsChanged)
        //       )
        //   );

        //public static readonly DependencyProperty ScannerCofigCollectionProperty =
        //   DependencyProperty.Register(
        //       "ScannerCofigCollection", typeof(ObservableCollection<ScannerCofigInfo>), typeof(PortPropertyPanel),
        //       new FrameworkPropertyMetadata(null,
        //           new PropertyChangedCallback(OnScannerCofigCollectionChanged)
        //       )
        //   );

        //private static readonly DependencyProperty SelectedScannerConfigProperty =
        //  DependencyProperty.Register(
        //      "SelectedScannerConfig", typeof(ScannerCofigInfo), typeof(PortPropertyPanel),
        //      new FrameworkPropertyMetadata(null, OnSelectedScannerConfigChanged)
        //  );

        //public static readonly DependencyProperty AddRowsCountProperty =
        //  DependencyProperty.Register(
        //      "AddRowsCount", typeof(int), typeof(PortPropertyPanel),
        //      new FrameworkPropertyMetadata(1, OnAddRowsCountChanged)
        //  );
        //public static readonly DependencyProperty AddColsCountProperty =
        //  DependencyProperty.Register(
        //      "AddColsCount", typeof(int), typeof(PortPropertyPanel),
        //      new FrameworkPropertyMetadata(2, OnAddColsCountChanged)
        //  );
        //public static readonly DependencyProperty TotalCanAddedCountProperty =
        //  DependencyProperty.Register(
        //      "TotalCanAddedCount", typeof(int), typeof(PortPropertyPanel),
        //      new FrameworkPropertyMetadata(512, OnTotalCanAddedCountChanged));
        //public static readonly DependencyProperty TotalLeftToAddedCountProperty =
        //  DependencyProperty.Register(
        //      "TotalLeftToAddedCount", typeof(int), typeof(PortPropertyPanel),
        //      new FrameworkPropertyMetadata(512, OnTotalLeftToAddedCountChanged));
        //public static readonly DependencyProperty SmartLCTActionManagerProperty =
        //    DependencyProperty.Register("SmartLCTActionManager", typeof(ActionManager),
        //    typeof(PortPropertyPanel), new FrameworkPropertyMetadata(new ActionManager()));
        //public static readonly DependencyProperty MyRectLayerProperty =
        //    DependencyProperty.Register("MyRectLayer", typeof(RectLayer), typeof(PortPropertyPanel), new FrameworkPropertyMetadata(new RectLayer()));
        //public static readonly DependencyProperty MaxPortHeightProperty =
        //    DependencyProperty.Register("MaxPortHeight", typeof(double), typeof(PortPropertyPanel));
        //public static readonly DependencyProperty MaxPortWidthProperty =
        //    DependencyProperty.Register("MaxPortWidth", typeof(double), typeof(PortPropertyPanel));

        //public static readonly DependencyProperty IsCheckedProperty =
        //    DependencyProperty.Register("IsChecked", typeof(bool), typeof(PortPropertyPanel));

        //#endregion

      //  #region 属性
       // public bool IsChecked
       // {
       //     get { return (bool)GetValue(IsCheckedProperty); }
       //     set { SetValue(IsCheckedProperty,value); }
       // }

       // public double MaxPortHeight
       // {
       //     get { return (double)GetValue(MaxPortHeightProperty); }
       //     set { SetValue(MaxPortHeightProperty, value); }
       // }
       // public double MaxPortWidth
       // {
       //     get { return (double)GetValue(MaxPortWidthProperty); }
       //     set { SetValue(MaxPortWidthProperty, value); }
       // }
       // public RectLayer MyRectLayer
       // {
       //     get { return (RectLayer)GetValue(MyRectLayerProperty); }
       //     set
       //     {
       //         SetValue(MyRectLayerProperty, value);
       //     }
       // }

       // public PortRealParameters PortRealParams
       // {
       //     get { return (PortRealParameters)GetValue(PortRealParamsProperty); }
       //     set
       //     {
       //         SetValue(PortRealParamsProperty, value);
       //     }
       // }
       // /// <summary>
       // /// 配置文件列表
       // /// </summary>
       // public ObservableCollection<ScannerCofigInfo> ScannerCofigCollection
       // {
       //     get { return (ObservableCollection<ScannerCofigInfo>)GetValue(ScannerCofigCollectionProperty); }
       //     set
       //     {
       //         SetValue(ScannerCofigCollectionProperty, value);
       //     }
       // }

       // private ScannerCofigInfo SelectedScannerConfig
       // {
       //     get
       //     {
       //         return (ScannerCofigInfo)GetValue(SelectedScannerConfigProperty);
       //     }
       //     set
       //     {
       //         SetValue(SelectedScannerConfigProperty, value);
       //     }
       // }

       // public int AddRowsCount
       // {
       //     get { return (int)GetValue(AddRowsCountProperty); }
       //     set
       //     {
       //         SetValue(AddRowsCountProperty, value);
       //     }
       // }

       // public int AddColsCount
       // {
       //     get { return (int)GetValue(AddColsCountProperty); }
       //     set
       //     {
       //         SetValue(AddColsCountProperty, value);
       //     }
       // }

       // public int TotalCanAddedCount
       // {
       //     get { return (int)GetValue(TotalCanAddedCountProperty); }
       //     set
       //     {
       //         SetValue(TotalCanAddedCountProperty, value);
       //     }
       // }

       // public int TotalLeftToAddedCount
       // {
       //     get 
       //     {
       //         return (int)GetValue(TotalLeftToAddedCountProperty);
       //     }
       //     set
       //     {
       //         SetValue(TotalLeftToAddedCountProperty, value);
       //     }
       // }

       // public ActionManager SmartLCTActionManager
       // {
       //     get { return (ActionManager)GetValue(SmartLCTActionManagerProperty); }
       //     set
       //     {
       //         SetValue(SmartLCTActionManagerProperty, value);
       //     }
       // }
       // #endregion
            
       // #region 命令
       // public static RoutedCommand XChangedCommand
       // {
       //     get { return _xChangedCommand; }
       // }
       // private static RoutedCommand _xChangedCommand;

       // public static RoutedCommand XChangedBeforeCommand
       // {
       //     get { return _xChangedBeforeCommand; }
       // }
       // private static RoutedCommand _xChangedBeforeCommand;
       // public static RoutedCommand YChangedCommand
       // {
       //     get { return _yChangedCommand; }
       // }
       // private static RoutedCommand _yChangedCommand;

       // public static RoutedCommand YChangedBeforeCommand
       // {
       //     get { return _yChangedBeforeCommand; }
       // }
       // private static RoutedCommand _yChangedBeforeCommand;
       // public static RoutedCommand HeightChangedBeforeCommand
       // {
       //     get { return _heightChangedBeforeCommand; }
       // }
       // private static RoutedCommand _heightChangedBeforeCommand;
       // public static RoutedCommand HeightChangedCommand
       // {
       //     get { return _heightChangedCommand; }
       // }
       // private static RoutedCommand _heightChangedCommand;
       // public static RoutedCommand WidthChangedBeforeCommand
       // {
       //     get { return _widthChangedBeforeCommand; }
       // }
       // private static RoutedCommand _widthChangedBeforeCommand;
       // public static RoutedCommand WidthChangedCommand
       // {
       //     get { return _widthChangedCommand; }
       // }
       // private static RoutedCommand _widthChangedCommand; 

       // public static RoutedCommand CmdAddScanner
       // {
       //     get { return _cmdAddScanner; }
       // }
       // private static RoutedCommand _cmdAddScanner;

       // public static RoutedCommand CmdArrangeScanner
       // {
       //     get { return _cmdArrangeScanner; }
       // }
       // private static RoutedCommand _cmdArrangeScanner;

       // public static RoutedCommand CmdAddSenderIndex
       // {
       //     get { return _cmdAddSenderIndex; }
       // }
       // private static RoutedCommand _cmdAddSenderIndex;

       // public static RoutedCommand CmdAddPortIndex
       // {
       //     get { return _cmdAddPortIndex; }
       // }
       // private static RoutedCommand _cmdAddPortIndex;

       // public static RoutedCommand CmdManualLine
       // {
       //     get { return _cmdManualLine; }
       // }
       // private static RoutedCommand _cmdManualLine;
       // #endregion

       // #region 字段
       // private double _lastX = 0;
       // private double _lastY = 0;
       // private double _lastHeight = 0;
       // private double _lastWidth = 0;
       // private SmartLCTViewModeBase _smartLCTViewModeBase = new SmartLCTViewModeBase();

       // private Canvas _senderCanvas = null;
       // private Canvas _portCanvas = null;

       // private int _currentSenderIndex = -1;
       // private int _currentPortIndex = -1;
       // #endregion

       // #region 构造函数
       // static PortPropertyPanel()
       // {
       //     InitializeCommands();

       //     //EventManager.RegisterClassHandler(typeof(NumericUpDown),
       //     //    NumericUpDown.ValueChangedRoutedEvent, new RoutedValueChangedHandler(PortPropertyPanel.OnMouseLeftButtonDown), true);


       //     DefaultStyleKeyProperty.OverrideMetadata(typeof(PortPropertyPanel), new FrameworkPropertyMetadata(typeof(PortPropertyPanel)));
            
       // }
       // #endregion

       // #region 私有函数

       // #region 命令处理
       // private static void InitializeCommands()
       // {

       //     _cmdManualLine = new RoutedCommand("_cmdManualLine", typeof(PortPropertyPanel));
       //     CommandManager.RegisterClassCommandBinding(typeof(PortPropertyPanel), new CommandBinding(_cmdManualLine, OnManualLineCommand, OnCanExecManualLineCommand));

       //     _cmdAddSenderIndex = new RoutedCommand("_cmdAddSenderIndex", typeof(PortPropertyPanel));
       //     CommandManager.RegisterClassCommandBinding(typeof(PortPropertyPanel), new CommandBinding(_cmdAddSenderIndex, OnAddSenderIndexCommand, OnCanExecAddSenderIndexCommand));

       //     _cmdAddPortIndex = new RoutedCommand("_cmdAddPortIndex", typeof(PortPropertyPanel));
       //     CommandManager.RegisterClassCommandBinding(typeof(PortPropertyPanel), new CommandBinding(_cmdAddPortIndex, OnAddPortIndexCommand, OnCanExecAddPortIndexCommand));

       //     _cmdAddScanner = new RoutedCommand("CmdAddScanner", typeof(PortPropertyPanel));
       //     CommandManager.RegisterClassCommandBinding(typeof(PortPropertyPanel), new CommandBinding(_cmdAddScanner, OnAddScannerCommand, OnCanExecAddScannerCommand));

       //     _cmdArrangeScanner = new RoutedCommand("CmdArrangeScanner", typeof(PortPropertyPanel));
       //     CommandManager.RegisterClassCommandBinding(typeof(PortPropertyPanel), new CommandBinding(_cmdArrangeScanner, OnCmdArrangeScanner, OnCanExecArrangeCommand));

       //     _xChangedCommand = new RoutedCommand("XChangedCommand", typeof(PortPropertyPanel));
       //     CommandManager.RegisterClassCommandBinding(typeof(PortPropertyPanel), new CommandBinding(_xChangedCommand, OnXChangedCommand));
       //     _xChangedBeforeCommand = new RoutedCommand("XChangedBeforeCommand", typeof(PortPropertyPanel));
       //     CommandManager.RegisterClassCommandBinding(typeof(PortPropertyPanel), new CommandBinding(_xChangedBeforeCommand, OnXChangedBeforeCommand));

       //     _yChangedCommand = new RoutedCommand("YChangedCommand", typeof(PortPropertyPanel));
       //     CommandManager.RegisterClassCommandBinding(typeof(PortPropertyPanel), new CommandBinding(_yChangedCommand, OnYChangedCommand));
       //     _yChangedBeforeCommand = new RoutedCommand("YChangedBeforeCommand", typeof(PortPropertyPanel));
       //     CommandManager.RegisterClassCommandBinding(typeof(PortPropertyPanel), new CommandBinding(_yChangedBeforeCommand, OnYChangedBeforeCommand));

       //     _heightChangedCommand = new RoutedCommand("HeightChangedCommand", typeof(PortPropertyPanel));
       //     CommandManager.RegisterClassCommandBinding(typeof(PortPropertyPanel), new CommandBinding(_heightChangedCommand, OnHeightChangedCommand));
       //     _heightChangedBeforeCommand = new RoutedCommand("HeightChangedBeforeCommand", typeof(PortPropertyPanel));
       //     CommandManager.RegisterClassCommandBinding(typeof(PortPropertyPanel), new CommandBinding(_heightChangedBeforeCommand, OnHeightChangedBeforeCommand));

       //     _widthChangedCommand = new RoutedCommand("WidthChangedCommand", typeof(PortPropertyPanel));
       //     CommandManager.RegisterClassCommandBinding(typeof(PortPropertyPanel), new CommandBinding(_widthChangedCommand, OnWidthChangedCommand));
       //     _widthChangedBeforeCommand = new RoutedCommand("WidthChangedBeforeCommand", typeof(PortPropertyPanel));
       //     CommandManager.RegisterClassCommandBinding(typeof(PortPropertyPanel), new CommandBinding(_widthChangedBeforeCommand, OnWidthChangedBeforeCommand));

       // }
       // private static void OnCanExecManualLineCommand(object sender, CanExecuteRoutedEventArgs e)
       // {
       //     PortPropertyPanel panel = (PortPropertyPanel)sender;
       //     e.CanExecute = panel.OnCanExecManualLine(sender);
       // }
       // private bool OnCanExecManualLine(object sender)
       // {
       //     PortPropertyPanel panel = (PortPropertyPanel)sender;
       //     if (panel.PortRealParams == null ||
       //         panel.PortRealParams.SelectedElementCollection == null ||
       //         panel.PortRealParams.PortLayer.ElementCollection.Count == 0 || 
       //         _currentPortIndex<0 ||
       //         _currentSenderIndex<0 )
       //     {
       //         return false;
       //     }
       //     else
       //     {
       //         return true;
       //     }
       // }
       // private static void OnManualLineCommand(object sender,ExecutedRoutedEventArgs e)
       // {
       //     PortPropertyPanel control = sender as PortPropertyPanel;
       //     if (control != null)
       //     {
       //         control.OnManualLine();
       //     }
       // }
       // private void OnManualLine()
       // {
       //     if (PortRealParams.PortLayer.CLineType == ConnectLineType.Manual)
       //     {
       //         PortRealParams.PortLayer.CLineType = ConnectLineType.Auto;
       //     }
       //     else
       //     {
       //         PortRealParams.PortLayer.CLineType = ConnectLineType.Manual;
       //     }
       // }

       // private static void OnCanExecAddSenderIndexCommand(object sender, CanExecuteRoutedEventArgs e)
       // {
       //     PortPropertyPanel panel = (PortPropertyPanel)sender;
       //     e.CanExecute = panel.OnCanExecAddSenderIndex();
       // }
       // private bool OnCanExecAddSenderIndex()
       // {
       //     //if (PortRealParams == null
       //     //    || PortRealParams.PortLayer == null)
       //     //{
       //     //    return false;
       //     //}
       //     //if (PortRealParams.PortLayer.SenderAndPortList.Count > 9)
       //     //{
       //     //    return false;
       //     //}
       //     //else
       //     //{
       //     //    return true;
       //     //}
       //     return false;
       // }
       // private static void OnAddSenderIndexCommand(object sender, ExecutedRoutedEventArgs e)
       // {
       //     PortPropertyPanel control = sender as PortPropertyPanel;
       //     if (control != null)
       //     {
       //         control.OnAddSenderIndex();
       //     }
       // }
       // private void OnAddSenderIndex()
       // //{
       // //    List<int> senderIndexList = new List<int>();
       // //    int addSenderIndex = -1;
       // //    for (int senderIndex = 0; senderIndex < PortRealParams.PortLayer.SenderAndPortList.Count; senderIndex++)
       // //    {
       // //        senderIndexList.Add(PortRealParams.PortLayer.SenderAndPortList[senderIndex].SenderIndex);
       // //    }
       // //    if (senderIndexList.Count != 0)
       // //    {
       // //        senderIndexList.Sort(delegate(int first, int second)
       // //       {
       // //           return first.CompareTo(second);
       // //       });
       // //        addSenderIndex = senderIndexList[senderIndexList.Count - 1] + 1;
       // //    }
       // //    else
       // //    {
       // //        addSenderIndex = 0;
       // //    }
       // //    SenderAndPort senderAndPort = new SenderAndPort();
       // //    ObservableCollection<int> portIndexList = new ObservableCollection<int>();
       // //    portIndexList.Add(0);
       // //    portIndexList.Add(1);
       // //    senderAndPort.SenderIndex = addSenderIndex;
       // //    senderAndPort.PortIndexList = portIndexList;
       // //    PortRealParams.PortLayer.SenderAndPortList.Add(senderAndPort);
       // //    AddSenderIndex(addSenderIndex);
       // //    SetTogButSelectedState(_senderCanvas, addSenderIndex);
       // //    _currentSenderIndex = addSenderIndex;
       // //    PortRealParams.PortLayer.CurrentSenderIndex = _currentSenderIndex;
       // }

       // private static void OnAddPortIndexCommand(object sender, ExecutedRoutedEventArgs e)
       // {
       //     PortPropertyPanel control = sender as PortPropertyPanel;
       //     if (control != null)
       //     {
       //         control.OnAddPortIndex();
       //     }
       // }
       // private void OnAddPortIndex()
       // {
       //     List<int> portIndexList = new List<int>();
       //     int addPortIndex = -1;
       //     for (int portIndex = 0; portIndex < PortRealParams.PortLayer.SenderAndPortList[_currentSenderIndex].PortIndexList.Count; portIndex++)
       //     {
       //         portIndexList.Add(PortRealParams.PortLayer.SenderAndPortList[_currentSenderIndex].PortIndexList[portIndex]);
       //     }
       //     if (portIndexList.Count != 0)
       //     {
       //         portIndexList.Sort(delegate(int first, int second)
       //         {
       //             return first.CompareTo(second);
       //         });
       //         addPortIndex = portIndexList[portIndexList.Count - 1] + 1;
       //     }
       //     else
       //     {
       //         addPortIndex = 0;
       //     }            
       //     PortRealParams.PortLayer.SenderAndPortList[_currentSenderIndex].PortIndexList.Add(addPortIndex);
       //     AddPortIndex(addPortIndex);
       //     SetTogButSelectedState(_portCanvas, addPortIndex);
       //     _currentPortIndex = addPortIndex;
       //     PortRealParams.PortLayer.CurrentPortIndex = _currentPortIndex;
       // }
       // private static void OnCanExecAddPortIndexCommand(object sender, CanExecuteRoutedEventArgs e)
       // {
       //     PortPropertyPanel panel = (PortPropertyPanel)sender;
       //     e.CanExecute = panel.OnCanExecAddPortIndex();
       // }
       // private bool OnCanExecAddPortIndex()
       // {
       //     if(PortRealParams==null
       //         || PortRealParams.PortLayer == null)
       //     {
       //         return false;
       //     }
       //     if (_currentSenderIndex <0)
       //     {
       //         return false;
       //     }
       //     if (PortRealParams.PortLayer.SenderAndPortList[_currentSenderIndex].PortIndexList.Count > 3)
       //     {
       //         return false;
       //     }
       //     else
       //     {
       //         return true;
       //     }
       // }

       // private static void OnHeightChangedBeforeCommand(object sender, ExecutedRoutedEventArgs e)
       // {
       //     PortPropertyPanel control = sender as PortPropertyPanel;
       //     if (control != null)
       //     {
       //         control.OnHeightChangedBefore((double)e.Parameter);
       //     }
       // }
       // private void OnHeightChangedBefore(double value)
       // {
       //     _lastHeight = value;
       // }
       // private static void OnHeightChangedCommand(object sender, ExecutedRoutedEventArgs e)
       // {
       //     PortPropertyPanel control = sender as PortPropertyPanel;
       //     if (control != null)
       //     {
       //         control.OnHeightChanged((double)e.Parameter);
       //     }
       // }
       // private void OnHeightChanged(double value)
       // {
       //     RectLayerChangedAction action;
       //     action = new RectLayerChangedAction(PortRealParams.PortLayer, "Height", value, _lastHeight);
       //     SmartLCTActionManager.RecordAction(action);
       //     _lastHeight = value;

       //     OnAddRowsCount(AddRowsCount);
       // }

       // private static void OnWidthChangedBeforeCommand(object sender, ExecutedRoutedEventArgs e)
       // {
       //     PortPropertyPanel control = sender as PortPropertyPanel;
       //     if (control != null)
       //     {
       //         control.OnWidthChangedBefore((double)e.Parameter);
       //     }
          
            
       // }
       // private void OnWidthChangedBefore(double value)
       // {
       //     _lastWidth = value;
       // }
       // private static void OnWidthChangedCommand(object sender, ExecutedRoutedEventArgs e)
       // {
       //     PortPropertyPanel control = sender as PortPropertyPanel;
       //     if (control != null)
       //     {
       //         control.OnWidthChanged((double)e.Parameter);
       //     }
       // }
       // private void OnWidthChanged(double value)
       // {
       //     RectLayerChangedAction action;
       //     action = new RectLayerChangedAction(PortRealParams.PortLayer, "Width", value, _lastWidth);
       //     SmartLCTActionManager.RecordAction(action);
       //     _lastWidth = value;
       //     OnAddColsCount(AddColsCount);
       // }

       // private static void OnYChangedBeforeCommand(object sender, ExecutedRoutedEventArgs e)
       // {
       //     PortPropertyPanel control = sender as PortPropertyPanel;
       //     if (control != null)
       //     {
       //         control.OnYChangedBefore((double)e.Parameter);
       //     }
       // }
       // private void OnYChangedBefore(double value)
       // {
       //     _lastY = value;
       // }
       // private static void OnYChangedCommand(object sender, ExecutedRoutedEventArgs e)
       // {
       //     PortPropertyPanel control = sender as PortPropertyPanel;
       //     if (control != null)
       //     {
       //         control.OnYChanged((double)e.Parameter);
       //     }
       // }
       // private void OnYChanged(double value)
       // {
       //     RectLayerChangedAction action;
       //     action = new RectLayerChangedAction(PortRealParams.PortLayer, "Y", value, _lastY);
       //     SmartLCTActionManager.RecordAction(action);
       //     _lastY = value;
       // }

       // private static void OnXChangedBeforeCommand(object sender, ExecutedRoutedEventArgs e)
       // {
       //     PortPropertyPanel control = sender as PortPropertyPanel;
       //     if (control != null)
       //     {
       //         control.OnXChangedBefore((double)e.Parameter);
       //     }
       // }
       // private void OnXChangedBefore(double value)
       // {
       //     _lastX = value;
       // }
       // private static void OnXChangedCommand(object sender, ExecutedRoutedEventArgs e)
       // {
       //     PortPropertyPanel control = sender as PortPropertyPanel;
       //     if (control != null)
       //     {
       //         control.OnXChanged((double)e.Parameter);
       //     }
       // }
       // private void OnXChanged(double value)
       // {
       //     RectLayerChangedAction action;
       //     action = new RectLayerChangedAction(PortRealParams.PortLayer, "X", value, _lastX);
       //     SmartLCTActionManager.RecordAction(action);
       //     _lastX = value;
       // }

       // #region 点击发送卡和网口序号
       // private void OnToggleButtonWithSenderCmd(ToggleButton togBut)
       // {
       //     _currentSenderIndex = int.Parse(togBut.Content.ToString())-1;
       //     PortRealParams.PortLayer.CurrentSenderIndex = _currentSenderIndex;
       //     SetTogButSelectedState(_senderCanvas, _currentSenderIndex);
       //     int oldIndex = -1;
       //     ClearPortIndex(out oldIndex);
       //     for (int portIndex = 0; portIndex < PortRealParams.PortLayer.SenderAndPortList[_currentSenderIndex].PortIndexList.Count; portIndex++)
       //     {
       //         AddPortIndex(PortRealParams.PortLayer.SenderAndPortList[_currentSenderIndex].PortIndexList[portIndex]);
       //     }
       // }
       // private void OnToggleButtonWithPortCmd(ToggleButton togBut)
       // {
       //     _currentPortIndex = int.Parse(togBut.Content.ToString())-1;
       //     PortRealParams.PortLayer.CurrentPortIndex = _currentPortIndex;
       //     SetTogButSelectedState(_portCanvas, _currentPortIndex);
       // }
       // #endregion

       // #region 添加
       // private static void OnAddScannerCommand(object sender, ExecutedRoutedEventArgs e)
       // {
       //     PortPropertyPanel panel = (PortPropertyPanel)sender;
       //     panel.OnAddScanner();
       // }
       // private void OnAddScanner()
       // {
       //     if (PortRealParams.PortLayer == null)
       //     {
       //         return;
       //     }
       //     RectLayer old = new RectLayer();
       //     IElement parent = PortRealParams.PortLayer.ParentElement;
       //     if (parent == null)
       //     {
       //         old = (RectLayer)((RectLayer)PortRealParams.PortLayer).Clone();
       //     }
       //     else
       //     {
       //         while (parent.ParentElement != null)
       //         {
       //             parent = parent.ParentElement;
       //         }
       //         old = (RectLayer)((RectLayer)parent).Clone();
       //     }

       //     ObservableCollection<IRectElement> selectedElement = new ObservableCollection<IRectElement>();
       //     int cols = AddColsCount;
       //     int rows = AddRowsCount;
       //     for (int i = 0; i < cols; i++)
       //     {
       //         for (int j = 0; j < rows; j++)
       //         {
       //             ScanBoardProperty scanBdProp = SelectedScannerConfig.ScanBdProp;
       //             int width = scanBdProp.Width;
       //             int height = scanBdProp.Height;
       //             RectElement element = new RectElement(width * i, height * j, width, height, PortRealParams.PortLayer, 0);
       //             element.Tag = SelectedScannerConfig.Clone();
       //             element.IsLocked = true;
       //             PortRealParams.PortLayer.ElementSelectedState = SelectedState.Selected;
       //             PortRealParams.PortLayer.ElementCollection.Add(element);
       //             selectedElement.Add(element);
       //         }
       //     }
       //     PortRealParameters portRealPara = new PortRealParameters();
       //     portRealPara.PortLayer = PortRealParams.PortLayer;
       //     portRealPara.SelectedElementCollection = selectedElement;
       //     portRealPara.PortLayer.CLineType = ConnectLineType.Auto;
       //     IsChecked = false; 
       //     PortRealParams = portRealPara;

            
       //     RectLayer newValue = new RectLayer();
       //     IElement newParent = PortRealParams.PortLayer.ParentElement;
       //     while (newParent.ParentElement != null)
       //     {
       //         newParent = newParent.ParentElement;
       //     }
       //     newValue = (RectLayer)((RectLayer)newParent).Clone();
       //     PrePropertyChangedEventArgs actionValue = new PrePropertyChangedEventArgs() { PropertyName = "ElementCollection", NewValue = newValue, OldValue = old };
       //     OnRecordActionValueChanged(actionValue);

       //     #region 处理添加接收卡后，另一个发送卡处于选中状态
       //     Function.SetSenderSelectedState(MyRectLayer);
       //     #endregion
       // }

       // private static void OnCanExecAddScannerCommand(object sender, CanExecuteRoutedEventArgs e)
       // {
       //     PortPropertyPanel panel = (PortPropertyPanel)sender;
       //     e.CanExecute = panel.OnCanExecAddScanner();
       // }      
       // private bool OnCanExecAddScanner()
       // {
       //     //没有选择要添加卡的配置
       //     if (SelectedScannerConfig == null ||
       //         SelectedScannerConfig.ScanBdProp == null)
       //     {
       //         return false;
       //     }
       //     //填写的行数或列数小于0
       //     if (AddColsCount <= 0 ||
       //         AddRowsCount <= 0)
       //     {
       //         return false;
       //     }
       //     //如果剩余可添加的数目不足
       //     int left = TotalLeftToAddedCount - AddColsCount * AddRowsCount;
       //     if (left < 0)
       //     {
       //         return false;
       //     }

       //     return true;
       // }
       // #endregion
        
       // #region 排布接收卡

       // private static void OnCmdArrangeScanner(object sender, ExecutedRoutedEventArgs e)
       // {
       //     PortPropertyPanel panel = (PortPropertyPanel)sender;
       //     panel.OnCmdArrangeScan(sender, e);
       //     //ArrangeType arrangeType = (ArrangeType)e.Parameter;
       //     //switch (arrangeType)
       //     //{
       //     //    case ArrangeType.LeftTop_Hor: panel.OnArrangeLeftTopHor(e); break;
       //     //    case ArrangeType.RightTop_Hor: panel.OnArrangeRightTopHor(e); break;
       //     //    case ArrangeType.LeftBottom_Hor: panel.OnArrangeLeftBottomHor(e); break;
       //     //    case ArrangeType.RightBottom_Hor: panel.OnArrangeRightBottomHor(e); break;
       //     //    case ArrangeType.LeftTop_Ver: panel.OnArrangeLeftTopVer(e); break;
       //     //    case ArrangeType.RightTop_Ver: panel.OnArrangeRightTopVer(e); break;
       //     //    case ArrangeType.LeftBottom_Ver: panel.OnArrangeLeftBottomVer(e); break;
       //     //    case ArrangeType.RightBottom_Ver: panel.OnArrangeRightBottomVer(e); break;
       //     //}
       // }

       // private void OnCmdArrangeScan(object sender, ExecutedRoutedEventArgs e)
       // {
       //     RectLayer old = new RectLayer();
       //     IElement parent = PortRealParams.PortLayer.ParentElement;
       //     if (parent == null)
       //     {
       //         old = (RectLayer)((RectLayer)PortRealParams.PortLayer).Clone();
       //     }
       //     else
       //     {
       //         while (parent.ParentElement != null)
       //         {
       //             parent = parent.ParentElement;
       //         }
       //         old = (RectLayer)((RectLayer)parent).Clone();
       //     }


       //     PortPropertyPanel panel = (PortPropertyPanel)sender;
       //     ArrangeType arrangeType = (ArrangeType)e.Parameter;
       //     switch (arrangeType)
       //     {
       //         case ArrangeType.LeftTop_Hor: panel.OnArrangeLeftTopHor(e); break;
       //         case ArrangeType.RightTop_Hor: panel.OnArrangeRightTopHor(e); break;
       //         case ArrangeType.LeftBottom_Hor: panel.OnArrangeLeftBottomHor(e); break;
       //         case ArrangeType.RightBottom_Hor: panel.OnArrangeRightBottomHor(e); break;
       //         case ArrangeType.LeftTop_Ver: panel.OnArrangeLeftTopVer(e); break;
       //         case ArrangeType.RightTop_Ver: panel.OnArrangeRightTopVer(e); break;
       //         case ArrangeType.LeftBottom_Ver: panel.OnArrangeLeftBottomVer(e); break;
       //         case ArrangeType.RightBottom_Ver: panel.OnArrangeRightBottomVer(e); break;
       //     }


       //     RectLayer newValue = new RectLayer();
       //     IElement newParent = PortRealParams.PortLayer.ParentElement;
       //     while (newParent.ParentElement != null)
       //     {
       //         newParent = newParent.ParentElement;
       //     }
       //     newValue = (RectLayer)((RectLayer)newParent).Clone();
       //     PrePropertyChangedEventArgs actionValue = new PrePropertyChangedEventArgs() { PropertyName = "ElementCollection", NewValue = newValue, OldValue = old };
       //     OnRecordActionValueChanged(actionValue);
       //     #region 处理添加接收卡后，另一个发送卡处于选中状态
       ////     Function.SetSenderSelectedState(MyRectLayer); 
       //     #endregion
       // }


       // #region 水平方向
       // private void OnArrangeLeftTopHor(ExecutedRoutedEventArgs e)
       // {
       //     #region 将要添加连线的卡按从左到右从上到下的顺序排列
       //     List<IRectElement> selectedElementCollection = new List<IRectElement>();
       //     for (int i = 0; i < PortRealParams.SelectedElementCollection.Count; i++ )
       //     {
       //         if (PortRealParams.SelectedElementCollection[i].ConnectedIndex < 0)
       //         {
       //             selectedElementCollection.Add(PortRealParams.SelectedElementCollection[i]);
       //         }
       //     }
       //     selectedElementCollection.Sort(delegate(IRectElement first, IRectElement second)
       //     {
       //         if (first.Y < second.Y)
       //         {
       //             return -1;
       //         }
       //         else if (first.Y > second.Y)
       //         {
       //             return 1;
       //         }
       //         else 
       //         {
       //             if (first.X < second.X)
       //             {
       //                 return -1;
       //             }
       //             else if (first.X > second.X)
       //             {
       //                 return 1;
       //             }
       //             else
       //             {
       //                 return 0;
       //             }
       //         }
       //     });
       //     #endregion

       //     #region 将连线排序
       //     int count = selectedElementCollection.Count;
       //     if (count == 0)
       //     {
       //         string msg = "";
       //         CommonStaticMethod.GetLanguageString("请清除当前网口连线后再进行连线！", "Lang_PortPropPanel_ClearLine", out msg);
       //         _smartLCTViewModeBase.ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);
       //         return;
       //     }

       //     ReverseCollection(selectedElementCollection, count, true);
       //     #endregion
       // }

       // private void OnArrangeRightTopHor(ExecutedRoutedEventArgs e)
       // {
       //     #region 将要添加连线的卡按从右到左从上到下的顺序排列
       //     List<IRectElement> selectedElementCollection = new List<IRectElement>();
       //     for (int i = 0; i < PortRealParams.SelectedElementCollection.Count; i++)
       //     {
       //         if (PortRealParams.SelectedElementCollection[i].ConnectedIndex < 0)
       //         {
       //             selectedElementCollection.Add(PortRealParams.SelectedElementCollection[i]);
       //         }
       //     }
       //     selectedElementCollection.Sort(delegate(IRectElement first, IRectElement second)
       //     {
       //         if (first.Y < second.Y)
       //         {
       //             return -1;
       //         }
       //         else if (first.Y > second.Y)
       //         {
       //             return 1;
       //         }
       //         else
       //         {
       //             if (first.X < second.X)
       //             {
       //                 return 1;
       //             }
       //             else if (first.X > second.X)
       //             {
       //                 return -1;
       //             }
       //             else
       //             {
       //                 return 0;
       //             }
       //         }
       //     });
       //     #endregion

       //     #region 将连线排序
       //     int count = selectedElementCollection.Count;
       //     if (count == 0)
       //     {
       //         string msg = "";
       //         CommonStaticMethod.GetLanguageString("请清除当前网口连线后再进行连线！", "Lang_PortPropPanel_ClearLine", out msg);
       //         _smartLCTViewModeBase.ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);

       //         return;
       //     }

       //     ReverseCollection(selectedElementCollection, count, true);
       //     #endregion
       // }

       // private void OnArrangeLeftBottomHor(ExecutedRoutedEventArgs e)
       // {
       //     #region 将要添加连线的卡按从左到右从下到上的顺序排列
       //     List<IRectElement> selectedElementCollection = new List<IRectElement>();
       //     for (int i = 0; i < PortRealParams.SelectedElementCollection.Count; i++)
       //     {
       //         if (PortRealParams.SelectedElementCollection[i].ConnectedIndex < 0)
       //         {
       //             selectedElementCollection.Add(PortRealParams.SelectedElementCollection[i]);
       //         }
       //     }
       //     selectedElementCollection.Sort(delegate(IRectElement first, IRectElement second)
       //     {
       //         if (first.Y < second.Y)
       //         {
       //             return 1;
       //         }
       //         else if (first.Y > second.Y)
       //         {
       //             return -1;
       //         }
       //         else
       //         {
       //             if (first.X < second.X)
       //             {
       //                 return -1;
       //             }
       //             else if (first.X > second.X)
       //             {
       //                 return 1;
       //             }
       //             else
       //             {
       //                 return 0;
       //             }
       //         }
       //     });
       //     #endregion

       //     #region 将连线排序
       //     int count = selectedElementCollection.Count;
       //     if (count == 0)
       //     {
       //         string msg = "";
       //         CommonStaticMethod.GetLanguageString("请清除当前网口连线后再进行连线！", "Lang_PortPropPanel_ClearLine", out msg);
       //         _smartLCTViewModeBase.ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);

       //         return;
       //     }

       //     ReverseCollection(selectedElementCollection, count, true);
       //     #endregion
       // }

       // private void OnArrangeRightBottomHor(ExecutedRoutedEventArgs e)
       // {
       //     #region 将要添加连线的卡按从右到左从下到上的顺序排列
       //     List<IRectElement> selectedElementCollection = new List<IRectElement>();
       //     for (int i = 0; i < PortRealParams.SelectedElementCollection.Count; i++)
       //     {
       //         if (PortRealParams.SelectedElementCollection[i].ConnectedIndex < 0)
       //         {
       //             selectedElementCollection.Add(PortRealParams.SelectedElementCollection[i]);
       //         }
       //     }
       //     selectedElementCollection.Sort(delegate(IRectElement first, IRectElement second)
       //     {
       //         if (first.Y < second.Y)
       //         {
       //             return 1;
       //         }
       //         else if (first.Y > second.Y)
       //         {
       //             return -1;
       //         }
       //         else
       //         {
       //             if (first.X < second.X)
       //             {
       //                 return 1;
       //             }
       //             else if (first.X > second.X)
       //             {
       //                 return -1;
       //             }
       //             else
       //             {
       //                 return 0;
       //             }
       //         }
       //     });
       //     #endregion

       //     #region 将连线排序
       //     int count = selectedElementCollection.Count;
       //     if (count == 0)
       //     {
       //         string msg = "";
       //         CommonStaticMethod.GetLanguageString("请清除当前网口连线后再进行连线！", "Lang_PortPropPanel_ClearLine", out msg);
       //         _smartLCTViewModeBase.ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);

       //         return;
       //     }

       //     ReverseCollection(selectedElementCollection, count, true);
       //     #endregion
       // }
       // #endregion

       // #region 垂直方向
       // private void OnArrangeLeftTopVer(ExecutedRoutedEventArgs e)
       // {
       //     #region 将要添加连线的卡按从左到右从上到下的顺序排列
       //     List<IRectElement> selectedElementCollection = new List<IRectElement>();
       //     for (int i = 0; i < PortRealParams.SelectedElementCollection.Count; i++)
       //     {
       //         if (PortRealParams.SelectedElementCollection[i].ConnectedIndex < 0)
       //         {
       //             selectedElementCollection.Add(PortRealParams.SelectedElementCollection[i]);
       //         }
       //     }
       //     selectedElementCollection.Sort(delegate(IRectElement first, IRectElement second)
       //     {
       //         if (first.X < second.X)
       //         {
       //             return -1;
       //         }
       //         else if (first.X > second.X)
       //         {
       //             return 1;
       //         }
       //         else
       //         {
       //             if (first.Y < second.Y)
       //             {
       //                 return -1;
       //             }
       //             else if (first.Y > second.Y)
       //             {
       //                 return 1;
       //             }
       //             else
       //             {
       //                 return 0;
       //             }
       //         }
       //     });
       //     #endregion

       //     #region 将连线排序
       //     int count = selectedElementCollection.Count;
       //     if (count == 0)
       //     {
       //         string msg = "";
       //         CommonStaticMethod.GetLanguageString("请清除当前网口连线后再进行连线！", "Lang_PortPropPanel_ClearLine", out msg);
       //         _smartLCTViewModeBase.ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);

       //         return;
       //     }

       //     ReverseCollection(selectedElementCollection, count, false);
       //     #endregion

       // }

       // private void OnArrangeRightTopVer(ExecutedRoutedEventArgs e)
       // {
       //     #region 将要添加连线的卡按从左到右从上到下的顺序排列
       //     List<IRectElement> selectedElementCollection = new List<IRectElement>();
       //     for (int i = 0; i < PortRealParams.SelectedElementCollection.Count; i++)
       //     {
       //         if (PortRealParams.SelectedElementCollection[i].ConnectedIndex < 0)
       //         {
       //             selectedElementCollection.Add(PortRealParams.SelectedElementCollection[i]);
       //         }
       //     }
       //     selectedElementCollection.Sort(delegate(IRectElement first, IRectElement second)
       //     {
       //         if (first.X < second.X)
       //         {
       //             return 1;
       //         }
       //         else if (first.X > second.X)
       //         {
       //             return -1;
       //         }
       //         else
       //         {
       //             if (first.Y < second.Y)
       //             {
       //                 return -1;
       //             }
       //             else if (first.Y > second.Y)
       //             {
       //                 return 1;
       //             }
       //             else
       //             {
       //                 return 0;
       //             }
       //         }
       //     });
       //     #endregion

       //     #region 将连线排序
       //     int count = selectedElementCollection.Count;
       //     if (count == 0)
       //     {
       //         string msg = "";
       //         CommonStaticMethod.GetLanguageString("请清除当前网口连线后再进行连线！", "Lang_PortPropPanel_ClearLine", out msg);
       //         _smartLCTViewModeBase.ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);

       //         return;
       //     }

       //     ReverseCollection(selectedElementCollection, count, false);
       //     #endregion
       // }

       // private void OnArrangeLeftBottomVer(ExecutedRoutedEventArgs e)
       // {
       //     #region 将要添加连线的卡按从左到右从上到下的顺序排列
       //     List<IRectElement> selectedElementCollection = new List<IRectElement>();
       //     for (int i = 0; i < PortRealParams.SelectedElementCollection.Count; i++)
       //     {
       //         if (PortRealParams.SelectedElementCollection[i].ConnectedIndex < 0)
       //         {
       //             selectedElementCollection.Add(PortRealParams.SelectedElementCollection[i]);
       //         }
       //     }
       //     selectedElementCollection.Sort(delegate(IRectElement first, IRectElement second)
       //     {
       //         if (first.X < second.X)
       //         {
       //             return -1;
       //         }
       //         else if (first.X > second.X)
       //         {
       //             return 1;
       //         }
       //         else
       //         {
       //             if (first.Y < second.Y)
       //             {
       //                 return 1;
       //             }
       //             else if (first.Y > second.Y)
       //             {
       //                 return -1;
       //             }
       //             else
       //             {
       //                 return 0;
       //             }
       //         }
       //     });
       //     #endregion

       //     #region 将连线排序
       //     int count = selectedElementCollection.Count;
       //     if (count == 0)
       //     {
       //         string msg = "";
       //         CommonStaticMethod.GetLanguageString("请清除当前网口连线后再进行连线！", "Lang_PortPropPanel_ClearLine", out msg);
       //         _smartLCTViewModeBase.ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);

       //         return;
       //     }

       //     ReverseCollection(selectedElementCollection, count, false);
       //     #endregion
       // }

       // private void OnArrangeRightBottomVer(ExecutedRoutedEventArgs e)
       // {
       //     #region 将要添加连线的卡按从左到右从上到下的顺序排列
       //     List<IRectElement> selectedElementCollection = new List<IRectElement>();
       //     for (int i = 0; i < PortRealParams.SelectedElementCollection.Count; i++)
       //     {
       //         if (PortRealParams.SelectedElementCollection[i].ConnectedIndex < 0)
       //         {
       //             selectedElementCollection.Add(PortRealParams.SelectedElementCollection[i]);
       //         }
       //     }
       //     selectedElementCollection.Sort(delegate(IRectElement first, IRectElement second)
       //     {
       //         if (first.X < second.X)
       //         {
       //             return 1;
       //         }
       //         else if (first.X > second.X)
       //         {
       //             return -1;
       //         }
       //         else
       //         {
       //             if (first.Y < second.Y)
       //             {
       //                 return 1;
       //             }
       //             else if (first.Y > second.Y)
       //             {
       //                 return -1;
       //             }
       //             else
       //             {
       //                 return 0;
       //             }
       //         }
       //     });
       //     #endregion

       //     #region 将连线排序
       //     int count = selectedElementCollection.Count;
       //     if (count == 0)
       //     {
       //         string msg = "";
       //         CommonStaticMethod.GetLanguageString("请清除当前网口连线后再进行连线！", "Lang_PortPropPanel_ClearLine", out msg);
       //         _smartLCTViewModeBase.ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);

       //         return;
       //     }

       //     ReverseCollection(selectedElementCollection, count, false);
       //     #endregion
       // }
       // #endregion

       // private void ReverseCollection(List<IRectElement> selectedElementCollection, int count, bool isHor)
       // {
       //     int lastX = 0;
       //     int lastY = 0;
       //     List<List<IRectElement>> rowList = new List<List<IRectElement>>();
       //     List<IRectElement> oneRow = null;

       //     for (int i = 0; i < count; i++)
       //     {
       //         int curX = (int)Math.Round(selectedElementCollection[i].X, 0);
       //         int curY = (int)Math.Round(selectedElementCollection[i].Y, 0);
       //         if (isHor)
       //         {
       //             if (i == 0 ||
       //                 curY != lastY)
       //             {
       //                 lastY = curY;
       //                 oneRow = new List<IRectElement>();
       //                 rowList.Add(oneRow);
       //             }
       //         }
       //         else
       //         {
       //             if (i == 0 ||
       //                 curX != lastX)
       //             {
       //                 lastX = curX;
       //                 oneRow = new List<IRectElement>();
       //                 rowList.Add(oneRow);
       //             }
       //         }
       //         IRectElement element = selectedElementCollection[i];
       //         oneRow.Add(element);
       //     }

       //     for (int i = 0; i < rowList.Count; i++)
       //     {
       //         List<IRectElement> tempRow = rowList[i];
       //         if (i % 2 != 0)
       //         {
       //             tempRow.Reverse();
       //         }
       //         for (int j = 0; j < tempRow.Count; j++)
       //         {
       //             IRectElement element = tempRow[j];
       //             int nextIndex = GetNextIndexInPort(PortRealParams.PortLayer);
       //             element.SenderIndex = _currentSenderIndex;
       //             element.PortIndex = _currentPortIndex;
       //             element.ConnectedIndex = nextIndex;

       //         }
       //     }

       //     //for (int i = 0; i < selectedElementCollection.Count; i++)
       //     //{
       //     //    IRectElement rectElement = selectedElementCollection[i];
       //     //    string msg = "坐标：(" + rectElement.X + "," + rectElement.Y + "), 连接序号：" + rectElement.ConnectedIndex;
       //     //    Debug.WriteLine(msg);
       //     //    selectedElementCollection[i].ConnectedIndex = -1;
       //     //}
       // }

       // private int GetNextIndexInPort(IRectLayer portLayer)
       // {
       //     int nextIndex = 0;
       //     int curMaxIndex = -1;
       //     ObservableCollection<IElement> portElementCollection = portLayer.ElementCollection;
       //     for (int i = 0; i < portElementCollection.Count; i++)
       //     {
       //         IElement element = portElementCollection[i];
       //         if (element is RectElement)
       //         {
       //             RectElement re = element as RectElement;
       //             int connectedIndex = re.ConnectedIndex;
       //             if (connectedIndex >= 0)
       //             {
       //                 curMaxIndex = Math.Max(connectedIndex, curMaxIndex);
       //             }
       //         }
       //     }
       //     nextIndex = curMaxIndex + 1;
       //     return nextIndex;
       // }

       // private static void OnCanExecArrangeCommand(object sender, CanExecuteRoutedEventArgs e)
       // {
       //     PortPropertyPanel panel = (PortPropertyPanel)sender;
       //     e.CanExecute = panel.OnCanExecArrange(sender);
       // }
       // private bool OnCanExecArrange(object sender)
       // {
       //     PortPropertyPanel panel = (PortPropertyPanel)sender;
       //     if (panel.PortRealParams == null ||
       //         panel.PortRealParams.SelectedElementCollection == null ||
       //         panel.PortRealParams.PortLayer.ElementCollection.Count == 0 ||
       //         panel.PortRealParams.PortLayer.CLineType != ConnectLineType.Auto ||
       //         _currentSenderIndex < 0 ||
       //         _currentPortIndex < 0)
       //     {
       //         return false;
       //     }
       //     else
       //     {
       //         return true;
       //     }
       // }
       // #endregion
       // #endregion

       // #region PortRealParams集合变化处理
       // private static void OnPortRealParamsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
       // {
       //     PortPropertyPanel ctrl = (PortPropertyPanel)d;
       //     ctrl.OnPortRealParamsSet(e);
       // }
       // private void OnPortRealParamsSet(DependencyPropertyChangedEventArgs e)
       // {
       //     if (e.Property == PortRealParamsProperty)
       //     {
       //         PortRealParameters oldParams = e.OldValue as PortRealParameters;
       //         PortRealParameters newParams = e.NewValue as PortRealParameters;

       //         if (oldParams != null &&
       //             oldParams.PortLayer != null &&
       //             oldParams.PortLayer.ElementCollection != null)
       //         {
       //             oldParams.PortLayer.ElementCollection.CollectionChanged -= new NotifyCollectionChangedEventHandler(OnPortElementCollectionChanged);
       //         }

       //         if (newParams != null &&
       //             newParams.PortLayer != null &&
       //             newParams.PortLayer.ElementCollection != null)
       //         {
       //             newParams.PortLayer.ElementCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(OnPortElementCollectionChanged);
       //             OnPortElementCollectionChanged(null, null);
       //         }
       //         if (newParams != null &&
       //             newParams.PortLayer != null &&
       //             newParams.PortLayer.ParentElement != null)
       //         {
       //             MaxPortHeight = ((RectLayer)newParams.PortLayer.ParentElement).Height;
       //             MaxPortWidth = ((RectLayer)newParams.PortLayer.ParentElement).Width;
       //         }
       //     }
       // }
       // private void OnPortElementCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
       // {
       //     PortRealParams.SelectedElementCollection.Clear();
       //     for (int i = 0; i < PortRealParams.PortLayer.ElementCollection.Count; i++)
       //     {
       //         if (PortRealParams.PortLayer.ElementCollection[i] is RectElement &&
       //              PortRealParams.PortLayer.ElementCollection[i].ElementSelectedState != SelectedState.None)
       //         {
       //             PortRealParams.SelectedElementCollection.Add((IRectElement)PortRealParams.PortLayer.ElementCollection[i]);
       //         }
       //     }
       //     int oldPortIndex = -1;
       //     int oldSenderIndex = -1;
       //     ClearPortIndex(out oldPortIndex);
       //     ClearSenderIndex(out oldSenderIndex);
       //     for (int senderIndex = 0; senderIndex < PortRealParams.PortLayer.SenderAndPortList.Count; senderIndex++)
       //     {
       //         AddSenderIndex(PortRealParams.PortLayer.SenderAndPortList[senderIndex].SenderIndex);
       //         //for (int portIndex = 0; portIndex < PortRealParams.PortLayer.SenderAndPortList[senderIndex].PortIndexList.Count; portIndex++)
       //         //{
       //         //    AddPortIndex(PortRealParams.PortLayer.SenderAndPortList[senderIndex].PortIndexList[portIndex]);
       //         //}
       //     }

       //     bool isSenderEqual = true;
       //     bool isPortEqual = true;
       //     int senderNum = -1;
       //     int portNum = -1;
       //     if (PortRealParams.SelectedElementCollection.Count > 0)
       //     {
       //         senderNum = PortRealParams.SelectedElementCollection[0].SenderIndex;
       //         portNum = PortRealParams.SelectedElementCollection[0].PortIndex;
       //         if (senderNum >=0 && portNum >= 0)
       //         {
       //             for (int index = 1; index < PortRealParams.SelectedElementCollection.Count; index++)
       //             {
       //                 if (PortRealParams.SelectedElementCollection[index].SenderIndex != senderNum)
       //                 {
       //                     isSenderEqual = false;
       //                     isPortEqual = false;
       //                     break;
       //                 }
       //                 if (PortRealParams.SelectedElementCollection[index].PortIndex != portNum)
       //                 {
       //                     isPortEqual = false;
       //                     break;
       //                 }
       //             }
       //         }
       //     }
       //     else
       //     {
       //         isPortEqual = false;
       //         isSenderEqual = false;
       //     }

       //     if (isSenderEqual && isPortEqual && senderNum>=0 && portNum>=0)
       //     {

       //         SetTogButSelectedState(_senderCanvas, senderNum);
       //         _currentSenderIndex = senderNum;
       //         int oldIndex = -1;
       //         ClearPortIndex(out oldIndex);
       //         for (int portIndex = 0; portIndex < PortRealParams.PortLayer.SenderAndPortList[_currentSenderIndex].PortIndexList.Count; portIndex++)
       //         {
       //             AddPortIndex(PortRealParams.PortLayer.SenderAndPortList[_currentSenderIndex].PortIndexList[portIndex]);
       //         }
       //         SetTogButSelectedState(_portCanvas, portNum);
       //         _currentPortIndex = portNum;
       //         PortRealParams.PortLayer.CurrentPortIndex = _currentPortIndex;
       //         PortRealParams.PortLayer.CurrentSenderIndex = _currentSenderIndex;
       //     }
       //     else
       //     {
       //         _currentPortIndex = oldPortIndex;
       //         _currentSenderIndex = oldSenderIndex;
       //         int oldIndex = -1;
       //         if (_currentSenderIndex >= 0)
       //         {
       //             ClearPortIndex(out oldIndex);
       //             for (int portIndex = 0; portIndex < PortRealParams.PortLayer.SenderAndPortList[_currentSenderIndex].PortIndexList.Count; portIndex++)
       //             {
       //                 AddPortIndex(PortRealParams.PortLayer.SenderAndPortList[_currentSenderIndex].PortIndexList[portIndex]);
       //             }
       //         }
       //         PortRealParams.PortLayer.CurrentSenderIndex = _currentSenderIndex;
       //         PortRealParams.PortLayer.CurrentPortIndex = _currentPortIndex;
       //         SetTogButSelectedState(_senderCanvas, _currentSenderIndex);
       //         SetTogButSelectedState(_portCanvas, _currentPortIndex);
       //     }

       //     if (PortRealParams.PortLayer.CLineType == ConnectLineType.Auto)
       //     {
       //         IsChecked = false;
       //     }
       //     else if (PortRealParams.PortLayer.CLineType == ConnectLineType.Manual)
       //     {
       //         IsChecked = true;
       //     }         
       //     TotalLeftToAddedCount = TotalCanAddedCount - PortRealParams.PortLayer.ElementCollection.Count;
       // }
     
       // #endregion

       // #region 添加发送卡和网口
       // private void AddSenderIndex(int senderIndex)
       // {
       //     ToggleButton togBut = new ToggleButton();
       //     togBut.Width = 25;
       //     togBut.Height = 25;
       //     togBut.Content = (senderIndex + 1).ToString();
       //     togBut.Command = new RelayCommand<ToggleButton>(OnToggleButtonWithSenderCmd);
       //     togBut.SetResourceReference(ToggleButton.StyleProperty, "MyToggleButtonStyle");
       //     togBut.CommandParameter = togBut;
       //     Thickness margin = new Thickness();
       //     if (senderIndex < 5)
       //     {
       //         margin.Left = 10 + (togBut.Width + 15) * senderIndex;
       //         margin.Top = 15;
       //     }
       //     else
       //     {
       //         margin.Left = 10 + (togBut.Width + 15) * (senderIndex - 5);
       //         margin.Top = 50;
       //     }
       //     togBut.Margin = margin;
       //     _senderCanvas.Children.Add(togBut);
       //     int oldPortIndex = -1;
       //     ClearPortIndex(out oldPortIndex);
       //     for (int index = 0; index < PortRealParams.PortLayer.SenderAndPortList[senderIndex].PortIndexList.Count; index++)
       //     {
       //         AddPortIndex(PortRealParams.PortLayer.SenderAndPortList[senderIndex].PortIndexList[index]);
       //     }
       //     _currentPortIndex = oldPortIndex;
       //     PortRealParams.PortLayer.CurrentPortIndex = _currentPortIndex;
       // }
       // private void AddPortIndex(int portIndex)
       // {
       //     ToggleButton togBut = new ToggleButton();
       //     togBut.Width = 25;
       //     togBut.Height = 25;
       //     togBut.Content = (portIndex + 1).ToString();
       //     togBut.Command = new RelayCommand<ToggleButton>(OnToggleButtonWithPortCmd);
       //     togBut.SetResourceReference(ToggleButton.StyleProperty, "MyToggleButtonStyle");
       //     togBut.CommandParameter = togBut;
       //     Thickness margin = new Thickness();
       //     margin.Left = 10 + (togBut.Width + 15) * portIndex;
       //     margin.Top = 15;
       //     togBut.Margin = margin;
       //     _portCanvas.Children.Add(togBut);
       // }

       // private void ClearPortIndex(out int checkIndex)
       // {
       //     checkIndex = -1;
       //     for (int portIndex = 0; portIndex < _portCanvas.Children.Count; portIndex++)
       //     {
       //         if (_portCanvas.Children[portIndex] is ToggleButton)
       //         {
       //             if ((bool)((ToggleButton)_portCanvas.Children[portIndex]).IsChecked)
       //             {
       //                 checkIndex = int.Parse(((ToggleButton)_portCanvas.Children[portIndex]).Content.ToString())-1;
       //             }
       //             _portCanvas.Children.RemoveAt(portIndex);
       //             portIndex = portIndex - 1;
       //         }
       //     }
       //     //_currentPortIndex = -1;
       // }
       // private void ClearSenderIndex(out int checkIndex)
       // {
       //     checkIndex = -1;
       //     for (int senderIndex = 0; senderIndex < _senderCanvas.Children.Count; senderIndex++)
       //     {
       //         if (_senderCanvas.Children[senderIndex] is ToggleButton)
       //         {
       //             if ((bool)((ToggleButton)_senderCanvas.Children[senderIndex]).IsChecked)
       //             {
       //                 checkIndex = int.Parse(((ToggleButton)_senderCanvas.Children[senderIndex]).Content.ToString())-1;
       //             }
       //             _senderCanvas.Children.RemoveAt(senderIndex);
       //             senderIndex = senderIndex - 1;
       //         }
       //     }
       // //    _currentSenderIndex = -1;
       // }
       // private void CancelPortIndexIsChecked()
       // {
       //     for (int index = 0; index < _portCanvas.Children.Count; index++)
       //     {
       //         if (_portCanvas.Children[index] is ToggleButton)
       //         {
       //             ((ToggleButton)_portCanvas.Children[index]).IsChecked = false;
       //         }
       //     }
       //     _currentPortIndex = -1;
       //     PortRealParams.PortLayer.CurrentPortIndex = _currentPortIndex;

       // }
       // private void CancelSenderIndexIsChecked()
       // {
       //     for (int index = 0; index < _senderCanvas.Children.Count; index++)
       //     {
       //         if (_senderCanvas.Children[index] is ToggleButton)
       //         {
       //             ((ToggleButton)_senderCanvas.Children[index]).IsChecked = false;
       //         }
       //     }
       //     _currentSenderIndex = -1;
       // }
       // private void SetTogButSelectedState(Canvas canvas, int togIndex)
       // {
       //     for (int index = 0; index < canvas.Children.Count; index++)
       //     {
       //         if (canvas.Children[index] is ToggleButton)
       //         {
       //             if (((ToggleButton)canvas.Children[index]).Content.ToString() == (togIndex + 1).ToString())
       //             {
       //                 ((ToggleButton)canvas.Children[index]).IsChecked = true;
       //             }
       //             else
       //             {
       //                 ((ToggleButton)canvas.Children[index]).IsChecked = false;
       //             }
       //         }
       //     }
       // }
       // #endregion

       // private static void OnScannerCofigCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
       // {
       //     ObservableCollection<ScannerCofigInfo> oldVal = e.OldValue as ObservableCollection<ScannerCofigInfo>;
       //     ObservableCollection<ScannerCofigInfo> newVal = e.NewValue as ObservableCollection<ScannerCofigInfo>;
       //     PortPropertyPanel panel = (PortPropertyPanel)d;
       //     if (newVal != null 
       //         && newVal.Count > 0)
       //     {
                
       //         panel.SelectedScannerConfig = (ScannerCofigInfo)newVal[0];
       //     }
       //     else
       //     {
       //         panel.SelectedScannerConfig = null;
       //     }
       // }
       // private static void OnSelectedScannerConfigChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
       // {

       // }

       // private static void OnAddRowsCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
       // {
       //     PortPropertyPanel panel = (PortPropertyPanel)d;
       //     panel.OnAddRowsCount((int)e.NewValue);
       // }
       // private void OnAddRowsCount(int value)
       // {
       //     ScanBoardProperty scanBdProp = SelectedScannerConfig.ScanBdProp;
       //     int height = scanBdProp.Height;
       //     double portHeight = PortRealParams.PortLayer.Height;
       //     int maxRowsCount = (int)portHeight / height;
       //     if (AddRowsCount > maxRowsCount)
       //     {
       //         AddRowsCount = maxRowsCount;
       //     }
       // }
       // private static void OnAddColsCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
       // {
       //     PortPropertyPanel panel = (PortPropertyPanel)d;
       //     panel.OnAddColsCount((int)e.NewValue);
       // }
       // private void OnAddColsCount(int value)
       // {
       //     ScanBoardProperty scanBdProp = SelectedScannerConfig.ScanBdProp;
       //     int width = scanBdProp.Width;
       //     double portWidth = PortRealParams.PortLayer.Width;
       //     int maxColsCount = (int)portWidth / width;
       //     if (AddColsCount > maxColsCount)
       //     {
       //         AddColsCount = maxColsCount;
       //     }
       // }
       // private static void OnTotalCanAddedCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
       // {

       // }
       // private static void OnTotalLeftToAddedCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
       // {

       // }
       // #endregion

       // private static void OnMouseLeftButtonDown(object sender, ValueChangedEventArgs e)
       // {

       // }
      
       // #region  重载
       // public override void OnApplyTemplate()
       // {
       //     var senderCanvas = this.GetTemplateChild("MySenderCanvas") as Canvas;
       //     var portCanvas = this.GetTemplateChild("MyPortCanvas") as Canvas;
       //     _senderCanvas = senderCanvas;
       //     _portCanvas = portCanvas;
            
       // }
       // #endregion

       // #region 记录活动
       // private void OnRecordActionValueChanged(PrePropertyChangedEventArgs e)
       // {
       //     RectElement rectElement = new RectElement();
       //     RectLayerChangedAction action;
       //     action = new RectLayerChangedAction(MyRectLayer, e.PropertyName, ((RectLayer)e.NewValue).ElementCollection, ((RectLayer)e.OldValue).ElementCollection);
       //     SmartLCTActionManager.RecordAction(action);
       // }
      // #endregion
    }
}
