using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Nova.SmartLCT.Interface;
using System.Collections.ObjectModel;
using GuiLabs.Undo;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using Nova.Wpf.Control;
using GalaSoft.MvvmLight.Command;
using System.Windows.Data;

namespace Nova.SmartLCT.UI
{
    public class ScreenMapLocationPropertyPanel:Control
    {
        #region 定义xmal属性
        /// <summary>
        /// 控件的IsLockProperty属性值
        /// </summary>
        public static readonly DependencyProperty ScreenMapRealParamsProperty =
            DependencyProperty.Register("ScreenMapRealParams", typeof(ScreenMapRealParameters), typeof(ScreenMapLocationPropertyPanel),
            new FrameworkPropertyMetadata(new ScreenMapRealParameters(), OnScreenMapRealParamsChanged));

        public static readonly DependencyProperty CurrentScreenProperty =
    DependencyProperty.Register("CurrentScreen", typeof(IRectElement), typeof(ScreenMapLocationPropertyPanel),
    new FrameworkPropertyMetadata(null, OnCurrentScreenChanged));

        public static readonly DependencyProperty SmartLCTActionManagerProperty =
            DependencyProperty.Register("SmartLCTActionManager", typeof(ActionManager),
            typeof(ScreenMapLocationPropertyPanel), new FrameworkPropertyMetadata(new ActionManager()));

        public static readonly DependencyProperty CheckSenderIndexProperty =
         DependencyProperty.Register("CheckSenderIndex", typeof(int),
         typeof(ScreenMapLocationPropertyPanel), new FrameworkPropertyMetadata(-1, OnCheckSenderIndexChanged));

        public static readonly DependencyProperty MaxDviHeightProperty =
         DependencyProperty.Register("MaxDviHeight", typeof(double), typeof(ScreenMapLocationPropertyPanel));

        public static readonly DependencyProperty MaxDviWidthProperty =
         DependencyProperty.Register("MaxDviWidth", typeof(double), typeof(ScreenMapLocationPropertyPanel));


        public static RoutedCommand HeightChangedBeforeCommand
        {
            get { return _heightChangedBeforeCommand; }
        }
        private static RoutedCommand _heightChangedBeforeCommand;
        public static RoutedCommand HeightChangedCommand
        {
            get { return _heightChangedCommand; }
        }
        private static RoutedCommand _heightChangedCommand;
        public static RoutedCommand WidthChangedBeforeCommand
        {
            get { return _widthChangedBeforeCommand; }
        }
        private static RoutedCommand _widthChangedBeforeCommand;
        public static RoutedCommand WidthChangedCommand
        {
            get { return _widthChangedCommand; }
        }
        private static RoutedCommand _widthChangedCommand; 
        #endregion

        #region 属性
        public double MaxDviHeight
        {
            get { return (double)GetValue(MaxDviHeightProperty); }
            set
            {
                SetValue(MaxDviHeightProperty, value);
            }
        }
        public double MaxDviWidth
        {
            get { return (double)GetValue(MaxDviWidthProperty); }
            set 
            {
                SetValue(MaxDviWidthProperty, value);
            }
        }
        public ScreenMapRealParameters ScreenMapRealParams
        {
            get { return (ScreenMapRealParameters)GetValue(ScreenMapRealParamsProperty); }
            set
            {
                SetValue(ScreenMapRealParamsProperty, value);
            }
        }
        public ActionManager SmartLCTActionManager
        {
            get { return (ActionManager)GetValue(SmartLCTActionManagerProperty); }
            set
            {
                SetValue(SmartLCTActionManagerProperty, value);
            }
        }
        public IRectElement CurrentScreen
        {
            get { return (IRectElement)GetValue(CurrentScreenProperty); }
            set
            {
                SetValue(CurrentScreenProperty,value);
            }
        }
        public int CheckSenderIndex
        {
            get { return (int)GetValue(CheckSenderIndexProperty); }
            set { SetValue(CheckSenderIndexProperty, value); }
        }
        #endregion

        #region 字段
        private double _lastHeight = 0;
        private double _lastWidth = 0;
        private Grid _myRadioButGrid = null;
        #endregion

        #region 构造函数
        static ScreenMapLocationPropertyPanel()
        {
            InitializeCommands();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScreenMapLocationPropertyPanel), new FrameworkPropertyMetadata(typeof(ScreenMapLocationPropertyPanel)));
        }
        #endregion

        #region 私有函数
        private static void OnCurrentScreenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScreenMapLocationPropertyPanel panel = (ScreenMapLocationPropertyPanel)d;
            if (panel != null)
            {
                panel.OnCurrentScreen();
            }
        }
        private void OnCurrentScreen()
        {
            if (_myRadioButGrid == null)
            {
                return;
            }
            if (CurrentScreen == null)
            {
                return;
            }
            if (CurrentScreen.EleType == ElementType.sender)
            {
                foreach (int key in _senderCheckBox.Keys)
                {
                    if (key == CurrentScreen.SenderIndex)
                    {
                        _senderCheckBox[key].IsChecked = true;
                    }
                    else
                    {
                        _senderCheckBox[key].IsChecked = false;
                    }
                }
                for (int i = 0; i < ScreenMapRealParams.RectLayerCollection.Count; i++)
                {
                    if (((IRectElement)ScreenMapRealParams.RectLayerCollection[i]).SenderIndex != CurrentScreen.SenderIndex)
                    {
                        ScreenMapRealParams.RectLayerCollection[i].ElementSelectedState = SelectedState.None;
                    }
                }
            }
            else
            {
                //将和element发送卡和网口不同的选项去掉
                for (int i = 0; i < ScreenMapRealParams.RectLayerCollection.Count; i++)
                {
                        ScreenMapRealParams.RectLayerCollection[i].ElementSelectedState = SelectedState.None;
                }
                //发送卡处理
                for (int i = 0; i < ScreenMapRealParams.RectLayerCollection.Count; i++)
                {
                    if (((IRectElement)ScreenMapRealParams.RectLayerCollection[i]).ConnectedIndex == CurrentScreen.ConnectedIndex && 
                        ((IRectElement)ScreenMapRealParams.RectLayerCollection[i]).SenderIndex == CurrentScreen.SenderIndex &&
                        ((IRectElement)ScreenMapRealParams.RectLayerCollection[i]).PortIndex == CurrentScreen.PortIndex)
                    {
                        ((IRectElement)ScreenMapRealParams.RectLayerCollection[i]).ElementSelectedState = SelectedState.Selected;
                    }
                }

                CurrentScreen.ElementSelectedState = SelectedState.Selected;
                if (CurrentScreen.EleType == ElementType.groupframe)
                {
                    for (int i = 0; i < ScreenMapRealParams.RectLayerCollection.Count; i++)
                    {
                        if (((IRectElement)ScreenMapRealParams.RectLayerCollection[i]).ConnectedIndex == CurrentScreen.ConnectedIndex &&
                            ((IRectElement)ScreenMapRealParams.RectLayerCollection[i]).GroupName==CurrentScreen.GroupName)
                        {
                            ((IRectElement)ScreenMapRealParams.RectLayerCollection[i]).ElementSelectedState = SelectedState.Selected;
                        }
                    }
                }
                for (int j = 0; j < _myRadioButGrid.Children.Count; j++)
                {
                    if (_myRadioButGrid.Children[j] is CheckBox)
                    {
                        CheckBox chbox = _myRadioButGrid.Children[j] as CheckBox;
                        IRectElement tag = (IRectElement)chbox.Tag;
                        if (tag == null)
                        {
                            continue;
                        }
                        if (tag.ElementSelectedState != SelectedState.None)
                        {
                            chbox.IsChecked = true;
                        }
                        else
                        {
                            chbox.IsChecked = false;
                        }
                    }
                }
                foreach (int key in _senderCheckBox.Keys)
                {
                    if (key == CurrentScreen.SenderIndex)
                    {
                        _senderCheckBox[key].IsChecked = true;
                    }
                    else
                    {
                        _senderCheckBox[key].IsChecked = false;
                    }
                }

            }

        }

        private static void OnCheckSenderIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScreenMapLocationPropertyPanel panel = (ScreenMapLocationPropertyPanel)d;
            if (panel != null)
            {
                panel.OnCheckSenderIndex();
            }
        }
        private void OnCheckSenderIndex()
        {
            if (_senderCheckBox.Keys.Contains(CheckSenderIndex))
            {
                _senderCheckBox[CheckSenderIndex].IsChecked = true;
            }
            for (int j = 0; j < _myRadioButGrid.Children.Count; j++)
            {
                if (_myRadioButGrid.Children[j] is CheckBox)
                {
                    CheckBox chbox = _myRadioButGrid.Children[j] as CheckBox;
                    IRectElement tag = (IRectElement)chbox.Tag;
                    if (tag == null)
                    {
                        continue;
                    }
                    if (tag.ElementSelectedState != SelectedState.None)
                    {
                        chbox.IsChecked = true;
                    }
                    else
                    {
                        chbox.IsChecked = false;
                    }
                }
            }
        }

        private static void OnScreenMapRealParamsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScreenMapLocationPropertyPanel panel = (ScreenMapLocationPropertyPanel)d;
            if (panel != null)
            {
                panel.OnScreenMapRealParams();
            }
        }
        private double _height = 0;
        private double _width = 0;
        private Dictionary<int,CheckBox> _senderCheckBox = new Dictionary<int,CheckBox>();
        private void OnScreenMapRealParams()
        {
            //生成相应的显示屏按钮
            _height = ScreenMapRealParams.SenderLoadRectLayer.Height;
            _width = ScreenMapRealParams.SenderLoadRectLayer.Width;
           
            if (_myRadioButGrid == null)
            {
                return;
            }
            _myRadioButGrid.Children.Clear();
            _senderCheckBox.Clear();
            if (ScreenMapRealParams.RectLayerCollection != null && ScreenMapRealParams.RectLayerCollection.Count != 0)
            {

                if (ScreenMapRealParams.RectLayerType == ElementType.sender)
                {
                    ObservableCollection<SenderConnectInfo> senderAndPortInfo = new ObservableCollection<SenderConnectInfo>();
                    #region 有多少发送卡及发送卡下的网口
                    for (int i = 0; i < ScreenMapRealParams.RectLayerCollection.Count; i++)
                    {
                        if (ScreenMapRealParams.RectLayerCollection[i].EleType == ElementType.groupframe)
                        {
                            continue;
                        }
                        //是否存在此发送卡
                        bool isHave = false;
                        for (int j = 0; j < senderAndPortInfo.Count; j++)
                        {
                            if (senderAndPortInfo[j].SenderIndex == ((IRectElement)ScreenMapRealParams.RectLayerCollection[i]).SenderIndex ||
                                ((IRectElement)ScreenMapRealParams.RectLayerCollection[i]).SenderIndex<0)
                            {
                                isHave = true;
                                break;
                            }
                        }
                        if (!isHave)
                        {
                            SenderConnectInfo senderInfo=new SenderConnectInfo();
                            senderInfo.SenderIndex=((IRectElement)ScreenMapRealParams.RectLayerCollection[i]).SenderIndex;
                            senderAndPortInfo.Add(senderInfo);
                        }
                    }
                    for (int i = 0; i < ScreenMapRealParams.RectLayerCollection.Count; i++)
                    {
                        if (ScreenMapRealParams.RectLayerCollection[i].EleType == ElementType.groupframe)
                        {
                            continue;
                        }
                        if (((IRectElement)ScreenMapRealParams.RectLayerCollection[i]).SenderIndex < 0)
                        {
                            continue;
                        }
                        for (int j = 0; j < senderAndPortInfo.Count; j++)
                        {
                            if (senderAndPortInfo[j].SenderIndex == ((IRectElement)ScreenMapRealParams.RectLayerCollection[i]).SenderIndex)
                            {
                                //是否有该网口
                                bool isHavePort = false;
                                for (int m = 0; m < senderAndPortInfo[j].PortConnectInfoList.Count; m++)
                                {
                                    if (senderAndPortInfo[j].PortConnectInfoList == null)
                                    {
                                        senderAndPortInfo[j].PortConnectInfoList = new ObservableCollection<PortConnectInfo>();
                                        isHavePort = true; ;
                                        break;
                                    }
                                    if (senderAndPortInfo[j].PortConnectInfoList[m].PortIndex == ((IRectElement)ScreenMapRealParams.RectLayerCollection[i]).PortIndex)
                                    {
                                        isHavePort = true;
                                        break;
                                    }

                                }
                                if (!isHavePort)
                                {
                                    PortConnectInfo portInfo = new PortConnectInfo();
                                    portInfo.PortIndex = ((IRectElement)ScreenMapRealParams.RectLayerCollection[i]).PortIndex;
                                    portInfo.MaxConnectElement = (RectElement)ScreenMapRealParams.RectLayerCollection[i];
                                    senderAndPortInfo[j].PortConnectInfoList.Add(portInfo);
                                }
                                break;
                            }
                        }
                    }
                    #endregion
                    _myRadioButGrid.Height = 8 * (ScreenMapRealParams.RectLayerCollection.Count + senderAndPortInfo.Count) + 17 * (ScreenMapRealParams.RectLayerCollection.Count+senderAndPortInfo.Count)+(senderAndPortInfo.Count-1)*10;
                    #region  生成按钮
                    int addIndex = 0;
                    for (int m = 0; m < senderAndPortInfo.Count; m++)
                    {
                        CheckBox senderBut = new CheckBox();
                        addIndex += 1;

                        string msg = "";
                        CommonStaticMethod.GetLanguageString("发送卡", "Lang_Global_SendingBoard", out msg);
                        senderBut.Content = msg + " " + (senderAndPortInfo[m].SenderIndex+ 1);
                        senderBut.Foreground = Brushes.White;
                       
                        senderBut.Command = new RelayCommand<IRectElement>(OnCmdSenderBut);
                        Thickness margin = new Thickness();
                        margin.Left = 10;
                        margin.Top = 8 * addIndex + 17 * (addIndex-1)+10*m;
                        senderBut.Margin = margin;
                        senderBut.HorizontalAlignment = HorizontalAlignment.Left;
                        senderBut.VerticalAlignment = VerticalAlignment.Top;
                        _myRadioButGrid.Children.Add(senderBut);
                        _senderCheckBox.Add(senderAndPortInfo[m].SenderIndex, senderBut);
                         for (int j = 0; j < senderAndPortInfo[m].PortConnectInfoList.Count; j++)
                         {
                             if (j == 0)
                             {
                                 senderBut.CommandParameter = senderAndPortInfo[m].PortConnectInfoList[j].MaxConnectElement;
                             }
                             CheckBox portBut = new CheckBox();
                             addIndex += 1;

                             CommonStaticMethod.GetLanguageString("网口", "Lang_Global_NetPort", out msg);

                             portBut.Content = msg + " " + (senderAndPortInfo[m].PortConnectInfoList[j].PortIndex + 1);
                             portBut.Foreground = Brushes.White;
                             portBut.Command = new RelayCommand<IRectElement>(OnCmdPortBut);
                             portBut.Tag = senderAndPortInfo[m].PortConnectInfoList[j].MaxConnectElement;
                             portBut.CommandParameter = senderAndPortInfo[m].PortConnectInfoList[j].MaxConnectElement;
                             portBut.HorizontalAlignment = HorizontalAlignment.Left;
                             portBut.VerticalAlignment = VerticalAlignment.Top;
                             Thickness portmargin = new Thickness();
                             portmargin.Left = 30;
                             portmargin.Top = 8 * addIndex + 17 * (addIndex - 1)+10*m;
                             portBut.Margin = portmargin;
                             Binding myBinding = new Binding("ElementSelectedState");
                             SelectedStateConverterIsSelected convert = new SelectedStateConverterIsSelected();
                             myBinding.Converter = convert;
                             myBinding.Source = senderAndPortInfo[m].PortConnectInfoList[j].MaxConnectElement;
                             myBinding.Mode = BindingMode.TwoWay;
                             if (senderAndPortInfo[m].PortConnectInfoList[j].MaxConnectElement.ElementSelectedState != SelectedState.None)
                             {
                                 portBut.IsChecked = true;
                                 senderBut.IsChecked = true;
                             }
                             else
                             {
                                 portBut.IsChecked = false;
                             }
                             _myRadioButGrid.Children.Add(portBut);
                         }
                       
                    }
                    #endregion
                }
                if (ScreenMapRealParams.RectLayerType == ElementType.screen)
                {
                    _myRadioButGrid.Height = 8 * (ScreenMapRealParams.RectLayerCollection.Count) + 17 * (ScreenMapRealParams.RectLayerCollection.Count)+(ScreenMapRealParams.RectLayerCollection.Count-1)*7;
                    for (int i = 0; i < ScreenMapRealParams.RectLayerCollection.Count; i++)
                    {
                        IRectElement layer = (IRectElement)ScreenMapRealParams.RectLayerCollection[i];
                        CheckBox screenBut = new CheckBox();
                        string butContent = "";

                        string msg = "";
                        CommonStaticMethod.GetLanguageString("显示屏", "Lang_Global_Screen", out msg);

                        butContent = msg + " " + (layer.ConnectedIndex + 1);
                        layer.DisplayName = butContent;
                        if (layer.ElementSelectedState == SelectedState.Selected)
                        {
                            screenBut.IsChecked = true;
                        }
                        else
                        {
                            screenBut.IsChecked = false;
                        }
                        screenBut.Content = butContent;
                        screenBut.Foreground = Brushes.White;
                        screenBut.HorizontalAlignment = HorizontalAlignment.Left;
                        screenBut.VerticalAlignment = VerticalAlignment.Top;
                        screenBut.Command = new RelayCommand<CheckBox>(OnCmdScreenBut);
                        screenBut.Tag = ScreenMapRealParams.RectLayerCollection[i];
                        screenBut.CommandParameter = screenBut;
                        Thickness margin = new Thickness();
                        margin.Left = 10;
                        margin.Top = 8 * (i + 1) + 17 * i+i*7;
                        screenBut.Margin = margin;
                        _myRadioButGrid.Children.Add(screenBut);
                    }
                }
            }
            
            
        }
        private void OnCmdSenderBut(IRectElement senderElement)
        {
            RectElement currentScreen = new RectElement();
            currentScreen.SenderIndex = senderElement.SenderIndex ;
            currentScreen.EleType = ElementType.sender;
            currentScreen.OperateEnviron = OperatEnvironment.AdjustSenderLocation;
            currentScreen.ConnectedIndex = senderElement.ConnectedIndex;
            CurrentScreen = currentScreen;
        }
        private void OnCmdPortBut(IRectElement element)
        {
            CurrentScreen = element;
        }
        private void OnCmdScreenBut(CheckBox radioBut)
        {
            string currentScreenName = radioBut.Content.ToString();
            for (int i = 0; i < ScreenMapRealParams.RectLayerCollection.Count; i++)
            {
                string screenName = "";
                string msg = "";
                if (ScreenMapRealParams.RectLayerType == ElementType.screen)
                {
                    CommonStaticMethod.GetLanguageString("显示屏", "Lang_Global_Screen", out msg);
                    screenName = msg + " " + (ScreenMapRealParams.RectLayerCollection[i].ConnectedIndex + 1);
                }
                else if (ScreenMapRealParams.RectLayerType == ElementType.sender)
                {
                    CommonStaticMethod.GetLanguageString("发送卡", "Lang_Global_SendingBoard", out msg);
                    screenName = msg + " " + (ScreenMapRealParams.RectLayerCollection[i].ConnectedIndex + 1);
                }
                if (currentScreenName == screenName)
                {
                    CurrentScreen = (IRectElement)ScreenMapRealParams.RectLayerCollection[i];
                    CurrentScreen.ElementSelectedState = SelectedState.Selected;
                    CurrentScreen.ZIndex = 3;
                }
                else
                {
                    ScreenMapRealParams.RectLayerCollection[i].ElementSelectedState = SelectedState.None;
                    ScreenMapRealParams.RectLayerCollection[i].ZIndex = 1;
                }
            }
            for (int j = 0; j < _myRadioButGrid.Children.Count; j++)
            {
                if (_myRadioButGrid.Children[j] is CheckBox)
                {
                    CheckBox chbox = _myRadioButGrid.Children[j] as CheckBox;
                    IRectElement tag = (IRectElement)chbox.Tag;
                    if (tag == null)
                    {
                        continue;
                    }
                    if (tag.ElementSelectedState != SelectedState.None)
                    {
                        chbox.IsChecked = true;
                    }
                    else
                    {
                        chbox.IsChecked = false;
                    }
                }
            }
        }

        private static void InitializeCommands()
        {
            _heightChangedCommand = new RoutedCommand("HeightChangedCommand", typeof(ScreenMapLocationPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScreenMapLocationPropertyPanel), new CommandBinding(_heightChangedCommand, OnHeightChangedCommand));
            _heightChangedBeforeCommand = new RoutedCommand("HeightChangedBeforeCommand", typeof(ScreenMapLocationPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScreenMapLocationPropertyPanel), new CommandBinding(_heightChangedBeforeCommand, OnHeightChangedBeforeCommand));

            _widthChangedCommand = new RoutedCommand("WidthChangedCommand", typeof(ScreenMapLocationPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScreenMapLocationPropertyPanel), new CommandBinding(_widthChangedCommand, OnWidthChangedCommand));
            _widthChangedBeforeCommand = new RoutedCommand("WidthChangedBeforeCommand", typeof(ScreenMapLocationPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScreenMapLocationPropertyPanel), new CommandBinding(_widthChangedBeforeCommand, OnWidthChangedBeforeCommand));


        }
        private static void OnHeightChangedBeforeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ScreenMapLocationPropertyPanel control = sender as ScreenMapLocationPropertyPanel;
            if (control != null)
            {
                control.OnHeightChangedBefore((double)e.Parameter);
            }
        }
        private void OnHeightChangedBefore(double value)
        {
            _lastHeight = value;
        }
        private static void OnHeightChangedCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ScreenMapLocationPropertyPanel control = sender as ScreenMapLocationPropertyPanel;
            if (control != null)
            {
                control.OnHeightChanged((double)e.Parameter);
            }
        }
        private void OnHeightChanged(double value)
        {
            //发送卡DVI更新
            if(ScreenMapRealParams.SenderLoadRectLayer!=null)
            {
                RectLayer layer=(RectLayer)ScreenMapRealParams.SenderLoadRectLayer;
               if(layer!=null && layer.SelectedSenderConfigInfo!=null && layer.SelectedSenderConfigInfo.PortCount!=0)
               {
                   Size maxDVI = Function.CalculateSenderLoadSize(layer.SelectedSenderConfigInfo.PortCount, 60, 24);
                   MaxDviWidth = maxDVI.Height * maxDVI.Width / value;
               }
               else
               {
                   Size maxDVI = Function.CalculateSenderLoadSize(4, 60, 24);
                   MaxDviWidth = maxDVI.Width;
               }
            }
        }


        private static void OnWidthChangedBeforeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ScreenMapLocationPropertyPanel control = sender as ScreenMapLocationPropertyPanel;
            if (control != null)
            {
                control.OnWidthChangedBefore((double)e.Parameter);
            }
        }
        private void OnWidthChangedBefore(double value)
        {
            _lastWidth = value;
        }
        private static void OnWidthChangedCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ScreenMapLocationPropertyPanel control = sender as ScreenMapLocationPropertyPanel;
            if (control != null)
            {
                control.OnWidthChanged((double)e.Parameter);
            }
        }
        private void OnWidthChanged(double value)
        {
            //发送卡DVI更新
            if (ScreenMapRealParams.SenderLoadRectLayer != null)
            {
                RectLayer layer = (RectLayer)ScreenMapRealParams.SenderLoadRectLayer;
                if (layer != null && layer.SelectedSenderConfigInfo != null && layer.SelectedSenderConfigInfo.PortCount != 0)
                {
                    Size maxDVI = Function.CalculateSenderLoadSize(layer.SelectedSenderConfigInfo.PortCount, 60, 24);
                    MaxDviHeight = maxDVI.Height * maxDVI.Width / value;
                }
                else
                {
                    Size maxDVI = Function.CalculateSenderLoadSize(4, 60, 24);
                    MaxDviHeight = maxDVI.Height;
                }
            }
            //RectLayerChangedAction action;
            //action = new RectLayerChangedAction(SenderRealParams.Element, "Width", value, _lastWidth);
            //SmartLCTActionManager.RecordAction(action);
            //_lastWidth = value;
        }
        #endregion

        #region 重载
        public override void OnApplyTemplate()
        {
            NumericUpDown myHeight = (NumericUpDown)GetTemplateChild("myHeight");
            NumericUpDown myWidth = (NumericUpDown)GetTemplateChild("myWidth");
            if (myHeight != null && myHeight.Value != _height)
            {
                myHeight.Value = _height;
            }
            if (myWidth != null && myWidth.Value != _width)
            {
                myWidth.Value = _width;
            }
            Grid myRadioButGrid = (Grid)GetTemplateChild("myRadioButGrid");
            _myRadioButGrid = myRadioButGrid;
            if (_myRadioButGrid != null && _myRadioButGrid.Children.Count == 0 && ScreenMapRealParams.RectLayerCollection != null && ScreenMapRealParams.RectLayerCollection.Count != 0)
            {

                OnScreenMapRealParams();
            }
        }
        #endregion
    }
}
