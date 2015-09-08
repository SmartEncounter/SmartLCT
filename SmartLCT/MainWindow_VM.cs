using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nova.SmartLCT.Interface;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Input;
//using Microsoft.Expression.Interactivity.Core;
using System.Windows.Data;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Nova.LCT.GigabitSystem.Common;
using Microsoft.Win32;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Collections.Specialized;
using GuiLabs.Undo;
using Nova.SmartLCT.Database;

namespace SmartLCT
{
    class MainWindow_VM : SmartLCTViewModeBase
    {
        #region 属性
        public RectLayer ScreenLocationRectLayer
        {
            get { return _screenLocationRectLayer; }
            set
            {
                _screenLocationRectLayer = value;
                
                NotifyPropertyChanged(GetPropertyName(o => this.ScreenLocationRectLayer));
            }
        }
        private RectLayer _screenLocationRectLayer = new RectLayer();
        public ConfigurationData MyConfigurationData
        {
            get { return _myConfigurationData; }
            set
            {
                _myConfigurationData = value;
                if (value != null)
                {
                    SelectedScannerConfigInfo = value.SelectedScannerConfigInfo;
                    OnCreateScreenWithConfigurationData(value);
                }
                if (SelectedEnvironMentIndex==1)
                OnSelectedEnvironmentChanged();
                NotifyPropertyChanged(GetPropertyName(o => this.MyConfigurationData));
            }
        }
        private ConfigurationData _myConfigurationData = new ConfigurationData();
        public RectLayer MyScreen
        {
            get { return _myScreen; }
            set
            {
                if (_myScreen.ElementCollection != null)
                {
                    _myScreen.ElementCollection.CollectionChanged -= new NotifyCollectionChangedEventHandler(OnRectLayerCollectionChange);

                }
                _myScreen = value;
                if (_myScreen.ElementCollection != null)
                {
                    _myScreen.ElementCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(OnRectLayerCollectionChange);

                }
                NotifyPropertyChanged(GetPropertyName(o => this.MyScreen));
            }
        }
        private RectLayer _myScreen = new RectLayer();
        private void OnRectLayerCollectionChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            for (int i = 0; i < MyScreen.ElementCollection.Count; i++)
            {
                MyScreen.ElementCollection[i].ConnectedIndex = i;
                if (i == MyScreen.ElementCollection.Count - 1)
                {
                    MyScreen.ElementCollection[i].ConnectedIndex = -1;
                }
                //string msg = "";
                //CommonStaticMethod.GetLanguageString("显示屏", "Lang_SmartLCT_VM_Screen", out msg);
                //RectLayerCollection[i].DisplayName = msg + (i + 1).ToString();
            }
        }
        public RectLayer SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                _selectedValue = value;
                if (value != null)
                {
                    for (int i = 0; i < value.ElementCollection.Count; i++)
                    {
                        value.ElementCollection[i].ElementSelectedState = SelectedState.None;
                    }
                }
                
                NotifyPropertyChanged(GetPropertyName(o => this.SelectedValue));
            }
        }
        private RectLayer _selectedValue = new RectLayer();
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SelectedIndex));
            }
        }
        private int _selectedIndex = 0;

        public RectLayer SelectedScreenLayer
        {
            get { return _selectedScreenLayer; }
            set 
            { 
                _selectedScreenLayer = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SelectedScreenLayer));

            }
        }
        private RectLayer _selectedScreenLayer = new RectLayer();
        public ObservableCollection<ScannerCofigInfo> ScannerTypeCollection
        {
            get { return _scannerTypeCollection; }
            set
            {
                _scannerTypeCollection = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ScannerTypeCollection));
            }
        }
        private ObservableCollection<ScannerCofigInfo> _scannerTypeCollection = new ObservableCollection<ScannerCofigInfo>();
        public ScannerCofigInfo SelectedScannerConfigInfo
        {
            get { return _selectedScannerConfigInfo; }
            set
            {
                _selectedScannerConfigInfo = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SelectedScannerConfigInfo));
            }
        }
        private ScannerCofigInfo _selectedScannerConfigInfo = new ScannerCofigInfo();
        public ObservableCollection<SenderConfigInfo> SenderConfigCollection
        {
            get { return _senderConfigCollection; }
            set 
            {
                _senderConfigCollection = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SenderConfigCollection));
            }
        }
        private ObservableCollection<SenderConfigInfo> _senderConfigCollection = new ObservableCollection<SenderConfigInfo>();
        public SenderRealParameters SenderRealParametersValue
        {
            get { return _senderRealParametersValue; }
            set
            {
                if (_senderRealParametersValue.Element != null)
                {
                    _senderRealParametersValue.Element.PropertyChanged -= new PropertyChangedEventHandler(SenderRealParameters_PropertyChanged);
                }
                _senderRealParametersValue = value;
                if (_senderRealParametersValue.Element != null)
                {
                    _senderRealParametersValue.Element.PropertyChanged += new PropertyChangedEventHandler(SenderRealParameters_PropertyChanged);

                }
                NotifyPropertyChanged(GetPropertyName(o => this.SenderRealParametersValue));
            }
        }
        private SenderRealParameters _senderRealParametersValue=new SenderRealParameters();
        private void SenderRealParameters_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "X" || e.PropertyName == "Y")
            {
                if (_senderCount > 1)
                {
                    //if (CurrentScreen.EleType == ElementType.groupframe)
                    //{
                    //    //整个组在移动（记录该组中全部网口的MAP）
                    //    RectLayer screenLayer = ((RectLayer)ScreenLocationRectLayer.ElementCollection[0]);
                        
                    //    for (int i = 0; i < screenLayer.ElementCollection.Count; i++)
                    //    {
                    //        //找到该组的元素，确定是哪个屏哪发送卡哪个网口
                    //        IRectElement rectElement = (IRectElement)screenLayer.ElementCollection[i];
                    //        if (rectElement.SenderIndex < 0 || rectElement.PortIndex < 0)
                    //        {
                    //            continue;
                    //        }
                    //        if (rectElement.ConnectedIndex == CurrentScreen.ConnectedIndex &&
                    //            rectElement.GroupName == CurrentScreen.GroupName)
                    //        {
                    //            Point mapLocation = new Point(CurrentScreen.X,CurrentScreen.Y);
                    //            ((RectLayer)((RectLayer)MyScreen.ElementCollection[rectElement.ConnectedIndex]).ElementCollection[0]).SenderConnectInfoList[rectElement.SenderIndex].PortConnectInfoList[rectElement.PortIndex].MapLocation = mapLocation;
                    //        }
                    //    }
                    //    Function.UpdateSenderConnectInfo(((RectLayer)((RectLayer)MyScreen.ElementCollection[CurrentScreen.ConnectedIndex]).ElementCollection[0]).SenderConnectInfoList, MyScreen);
                    //}
                    //else 
                    if (CurrentScreen.EleType == ElementType.receive && CurrentScreen.GroupName >= 0)
                    {
                        //组中的某一个在移动(根据移动的距离去移动网口对应的箱体，并记录map)
                        //看是移动一个组还是移动组中的一个
                        //计算原来网口的位置
                        SenderConnectInfo oldsenderInfo = ((RectLayer)((RectLayer)MyScreen.ElementCollection[CurrentScreen.ConnectedIndex]).ElementCollection[0]).SenderConnectInfoList[CurrentScreen.SenderIndex];
                        Rect senderLoadSize = oldsenderInfo.LoadSize;
                        Point oldPortPoint = new Point();
                        PortConnectInfo oldPortInfo = oldsenderInfo.PortConnectInfoList[CurrentScreen.PortIndex];
                        oldPortPoint.X = oldPortInfo.LoadSize.X - senderLoadSize.X + oldPortInfo.MapLocation.X;
                        oldPortPoint.Y = oldPortInfo.LoadSize.Y - senderLoadSize.Y + oldPortInfo.MapLocation.Y;
                        //计算现在网口和原来网口对比，移动的距离
                        Point difPoint = new Point(CurrentScreen.X - oldPortPoint.X, CurrentScreen.Y - oldPortPoint.Y);
                        //判断设计区域的箱体按difPoint去移动，是否超出区域，超出的距离让该发送卡的其他网口移动
                        Point overDifPoint = new Point();
                        if (oldPortInfo.LoadSize.X + difPoint.X < 0)
                        {
                            overDifPoint.X = -(oldPortInfo.LoadSize.X + difPoint.X);
                            difPoint.X = -oldPortInfo.LoadSize.X;
                        }
                        if (oldPortInfo.LoadSize.Y + difPoint.Y < 0)
                        {
                            overDifPoint.Y = -(oldPortInfo.LoadSize.Y + difPoint.Y);
                            difPoint.Y = -oldPortInfo.LoadSize.Y;
                        }
                        if (oldPortInfo.LoadSize.X + oldPortInfo.LoadSize.Width + difPoint.X > SmartLCTViewModeBase.MaxScreenWidth)
                        {
                            double oldX = difPoint.X;
                            difPoint.X = SmartLCTViewModeBase.MaxScreenWidth - oldPortInfo.LoadSize.X - oldPortInfo.LoadSize.Width;
                            overDifPoint.X = difPoint.X - oldX;
                        }
                        if (oldPortInfo.LoadSize.Y + oldPortInfo.LoadSize.Height + difPoint.Y > SmartLCTViewModeBase.MaxScreenHeight)
                        {
                            double oldY = difPoint.Y;
                            difPoint.Y = SmartLCTViewModeBase.MaxScreenHeight - oldPortInfo.LoadSize.Y - oldPortInfo.LoadSize.Height;
                            overDifPoint.Y = difPoint.Y - oldY;
                        }
                        //移动箱体
                        Function.SetElementCollectionX(oldPortInfo.ConnectLineElementList, difPoint.X);
                        Function.SetElementCollectionY(oldPortInfo.ConnectLineElementList, difPoint.Y);
                        //看移动的箱体都是那些组框下面的
                        ObservableCollection<int> groupnameList = new ObservableCollection<int>();
                        for (int i = 0; i < oldPortInfo.ConnectLineElementList.Count; i++)
                        {
                            int groupname = oldPortInfo.ConnectLineElementList[i].GroupName;
                            if (!groupnameList.Contains(groupname))
                            {
                                groupnameList.Add(groupname);
                            }
                        }
                        //移动该网口的组框(先计算组框的位置和大小)
                        RectLayer screen = ((RectLayer)((RectLayer)MyScreen.ElementCollection[CurrentScreen.ConnectedIndex]).ElementCollection[0]);
                        for (int i = 0; i < groupnameList.Count; i++)
                        {
                            int groupname = groupnameList[i];
                            IRectElement groupElement = new RectElement();
                            Rect groupframeRect = new Rect();
                            //找到该组下的全部元素
                            for (int j = 0; j < screen.ElementCollection.Count; j++)
                            {
                                if (screen.ElementCollection[j].EleType == ElementType.groupframe && screen.ElementCollection[j].GroupName==groupname)
                                {
                                    groupElement = (RectElement)screen.ElementCollection[j];
                                }
                                else if (screen.ElementCollection[j].EleType == ElementType.receive && screen.ElementCollection[j].GroupName == groupname)
                                {
                                    IRectElement element=(IRectElement)screen.ElementCollection[j];
                                    Rect rect = new Rect(element.X, element.Y, element.Width, element.Height);
                                    if (groupframeRect == new Rect())
                                    {
                                        groupframeRect = rect;
                                    }
                                    else
                                    {
                                        groupframeRect = Rect.Union(groupframeRect, rect);
                                    }
                                }
                            }
                            groupElement.X = groupframeRect.X;
                            groupElement.Y = groupframeRect.Y;
                            groupElement.Width = groupframeRect.Width;
                            groupElement.Height = groupframeRect.Height;
                        }
                       
                        //移动其他网口的箱体（当按照difPont去移动超出区域时）
                        if (overDifPoint.X != 0 || overDifPoint.Y != 0)
                        {
                            ObservableCollection<PortConnectInfo> portConnectInfoList = screen.SenderConnectInfoList[CurrentScreen.SenderIndex].PortConnectInfoList;
                            for (int i = 0; i < portConnectInfoList.Count; i++)
                            {
                                if (portConnectInfoList[i].LoadSize.Width != 0 && portConnectInfoList[i].LoadSize.Height != 0 && portConnectInfoList[i].PortIndex != CurrentScreen.PortIndex)
                                {
                                    if (overDifPoint.X != 0)
                                        Function.SetElementCollectionX(portConnectInfoList[i].ConnectLineElementList, overDifPoint.X);
                                    if (overDifPoint.Y != 0)
                                        Function.SetElementCollectionY(portConnectInfoList[i].ConnectLineElementList, overDifPoint.Y);
                                    //看移动的箱体都是那些组框下面的
                                    ObservableCollection<int> groupnameLists = new ObservableCollection<int>();
                                    for (int j = 0; j < portConnectInfoList[i].ConnectLineElementList.Count; j++)
                                    {
                                        int groupname = portConnectInfoList[i].ConnectLineElementList[j].GroupName;
                                        if (!groupnameLists.Contains(groupname))
                                        {
                                            groupnameLists.Add(groupname);
                                        }
                                    }
                                    //移动组框(先计算组框的位置和大小)
                                    for (int m = 0; m < groupnameLists.Count; m++)
                                    {
                                        int groupname = groupnameLists[m];
                                        IRectElement groupElement = new RectElement();
                                        Rect groupframeRect = new Rect();
                                        //找到该组下的全部元素
                                        for (int j = 0; j < screen.ElementCollection.Count; j++)
                                        {
                                            if (screen.ElementCollection[j].EleType == ElementType.groupframe && screen.ElementCollection[j].GroupName == groupname)
                                            {
                                                groupElement = (RectElement)screen.ElementCollection[j];
                                            }
                                            else if (screen.ElementCollection[j].EleType == ElementType.receive && screen.ElementCollection[j].GroupName == groupname)
                                            {
                                                IRectElement element = (IRectElement)screen.ElementCollection[j];
                                                Rect rect = new Rect(element.X, element.Y, element.Width, element.Height);
                                                if (groupframeRect == new Rect())
                                                {
                                                    groupframeRect = rect;
                                                }
                                                else
                                                {
                                                    groupframeRect = Rect.Union(groupframeRect, rect);
                                                }
                                            }
                                        }
                                        groupElement.X = groupframeRect.X;
                                        groupElement.Y = groupframeRect.Y;
                                        groupElement.Width = groupframeRect.Width;
                                        groupElement.Height = groupframeRect.Height;
                                    }
                                }
                            }
                        }
                        //更新组中全部网口的Map
                        ObservableCollection<IRectElement> selectedCollection = new ObservableCollection<IRectElement>();
                        for (int i = 0; i < ((RectLayer)ScreenLocationRectLayer.ElementCollection[0]).ElementCollection.Count; i++)
                        {
                            IRectElement rectElement = (IRectElement)((RectLayer)ScreenLocationRectLayer.ElementCollection[0]).ElementCollection[i];
                            if (rectElement.ConnectedIndex == CurrentScreen.ConnectedIndex &&
                                rectElement.GroupName == CurrentScreen.GroupName)
                            {
                                if (rectElement.EleType != ElementType.groupframe)
                                {
                                    selectedCollection.Add(rectElement);
                                }
                            }
                        }
                        Rect groupRect = Function.UnionRectCollection(selectedCollection);
                        Point mapLocation = new Point();
                        mapLocation.X = groupRect.X;
                        mapLocation.Y = groupRect.Y;
                        for (int j = 0; j < selectedCollection.Count; j++)
                        {
                            ((RectLayer)((RectLayer)MyScreen.ElementCollection[selectedCollection[j].ConnectedIndex]).ElementCollection[0]).SenderConnectInfoList[selectedCollection[j].SenderIndex].PortConnectInfoList[selectedCollection[j].PortIndex].MapLocation = mapLocation;
                        }
                        Function.UpdateSenderConnectInfo(((RectLayer)((RectLayer)MyScreen.ElementCollection[CurrentScreen.ConnectedIndex]).ElementCollection[0]).SenderConnectInfoList, MyScreen);
                    }
                    else if (CurrentScreen.EleType == ElementType.receive && CurrentScreen.GroupName < 0)
                    {
                        //不属于某个组的单个网口在移动(记录map)
                        Point mapLocation = new Point(CurrentScreen.X, CurrentScreen.Y);
                        ((RectLayer)((RectLayer)MyScreen.ElementCollection[CurrentScreen.ConnectedIndex]).ElementCollection[0]).SenderConnectInfoList[CurrentScreen.SenderIndex].PortConnectInfoList[CurrentScreen.PortIndex].MapLocation = mapLocation;
                        Function.UpdateSenderConnectInfo(((RectLayer)((RectLayer)MyScreen.ElementCollection[CurrentScreen.ConnectedIndex]).ElementCollection[0]).SenderConnectInfoList, MyScreen);

                    }
                }
                else if (_senderCount == 1)
                {
                    #region 单卡
                    //是哪个卡
                    int currentSenderIndex = -1;
                    for (int n = 0; n < MyScreen.SenderConnectInfoList.Count; n++)
                    {
                        if (MyScreen.SenderConnectInfoList[n].LoadSize.Height != 0 && MyScreen.SenderConnectInfoList[n].LoadSize.Width != 0)
                        {
                            currentSenderIndex = n;
                            break;
                        }
                    }
                    if (currentSenderIndex >= 0)
                    {
                        //更新带载
                        //保存显示屏映射位置
                        for (int i = 0; i < ScreenMapRealParametersValue.RectLayerCollection.Count; i++)
                        {
                            Point mapLocation = new Point();
                            mapLocation.X = ((IRectElement)ScreenMapRealParametersValue.RectLayerCollection[i]).X;
                            mapLocation.Y = ((IRectElement)ScreenMapRealParametersValue.RectLayerCollection[i]).Y;
                            //找到相应的显示屏
                            for (int j = 0; j < MyScreen.ElementCollection.Count; j++)
                            {

                                if (MyScreen.ElementCollection[j].ConnectedIndex == ScreenMapRealParametersValue.RectLayerCollection[i].ConnectedIndex)
                                {
                                    RectLayer screenRectLayer = ((RectLayer)(((RectLayer)MyScreen.ElementCollection[j]).ElementCollection[0]));

                                    for (int m = 0; m < screenRectLayer.SenderConnectInfoList[currentSenderIndex].PortConnectInfoList.Count; m++)
                                    {
                                        PortConnectInfo portInfo = screenRectLayer.SenderConnectInfoList[currentSenderIndex].PortConnectInfoList[m];
                                        portInfo.MapLocation = mapLocation;
                                    }
                                    //screenRectLayer.SenderConnectInfoList[0].MapLocation = mapLocation;
                                    Function.UpdateSenderConnectInfo(screenRectLayer.SenderConnectInfoList, screenRectLayer);
                                    break;
                                }
                            }
                        }
                        //保存DVI
                        MyScreen.SenderConnectInfoList[0].DviSize = new Size(ScreenMapRealParametersValue.SenderLoadRectLayer.Width, ScreenMapRealParametersValue.SenderLoadRectLayer.Height);
                    #endregion
                    }
                }
            }
        }

        public ScannerRealParameters ScannerRealParametersValue
        {
            get { return _scannerRealParametersValue; }
            set 
            {
                _scannerRealParametersValue = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ScannerRealParametersValue));
            }
        }
        private ScannerRealParameters _scannerRealParametersValue = new ScannerRealParameters();
        public ScreenRealParameters ScreenRealParametersValue
        {
            get { return _screenRealParametersValue; }
            set
            {
                _screenRealParametersValue = value;
                //SelectedLayerAndElement selectedLayerAndElement=new SelectedLayerAndElement();
                //selectedLayerAndElement.SelectedLayer=(RectLayer)value.ScreenLayer;
                //selectedLayerAndElement.SelectedElement=new ObservableCollection<IRectElement>();
                //if (value.SelectedElement != null)
                //{
                //    selectedLayerAndElement.SelectedElement = value.SelectedElement;
                //}
                //OnSelectedLayerAndElementChanged(selectedLayerAndElement);
                NotifyPropertyChanged(GetPropertyName(o => this.ScreenRealParametersValue));
            }
        }
        private ScreenRealParameters _screenRealParametersValue = new ScreenRealParameters();
        public ScreenMapRealParameters ScreenMapRealParametersValue
        {
            get { return _screenMapRealParametersValue; }
            set
            {
                if (_screenMapRealParametersValue.SenderLoadRectLayer != null)
                {
                    _screenMapRealParametersValue.SenderLoadRectLayer.PropertyChanged -= new PropertyChangedEventHandler(ScreenMapRealParameters_PropertyChanged);
                }
                _screenMapRealParametersValue = value;
                if (_screenMapRealParametersValue.SenderLoadRectLayer != null)
                {
                    _screenMapRealParametersValue.SenderLoadRectLayer.PropertyChanged += new PropertyChangedEventHandler(ScreenMapRealParameters_PropertyChanged);

                }
                NotifyPropertyChanged(GetPropertyName(o => this.ScreenMapRealParametersValue));
            }
        }
        private ScreenMapRealParameters _screenMapRealParametersValue = new ScreenMapRealParameters();
        private void ScreenMapRealParameters_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Height" || e.PropertyName=="Width")
            {
                //保存发送卡的DVI
                if (ScreenLocationRectLayer.SenderIndex >= 0)
                {
                    Size dviSize = new Size();
                    dviSize.Width = ScreenMapRealParametersValue.SenderLoadRectLayer.Width;
                    dviSize.Height = ScreenMapRealParametersValue.SenderLoadRectLayer.Height;
                    MyScreen.SenderConnectInfoList[ScreenLocationRectLayer.SenderIndex].DviSize = dviSize;
                    int increaseOrDecreaseIndex = ((IRectLayer)ScreenLocationRectLayer.ElementCollection[0]).IncreaseOrDecreaseIndex;
                    MyScreen.SenderConnectInfoList[ScreenLocationRectLayer.SenderIndex].IncreaseOrDecreaseIndex = increaseOrDecreaseIndex;
                    Point newLocation = GetDviCenterPoint(increaseOrDecreaseIndex, (IRectLayer)ScreenLocationRectLayer.ElementCollection[0]);
                    ((IRectElement)ScreenLocationRectLayer.ElementCollection[0]).X = newLocation.X;
                    ((IRectElement)ScreenLocationRectLayer.ElementCollection[0]).Y = newLocation.Y;
                }

            }
        }
        public Visibility CustomScanVisible
        {
            get { return _customScanVisible; }
            set 
            { 
                _customScanVisible = value;
                NotifyPropertyChanged(GetPropertyName(o => this.CustomScanVisible));
            }
        }
        private Visibility _customScanVisible = Visibility.Visible;
        public Visibility ScreenVisible
        {
            get { return _screenVisible; }
            set
            {
                _screenVisible = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ScreenVisible));
            }
        }
        private Visibility _screenVisible = Visibility.Visible;
        public Visibility SenderVisible
        {
            get { return _senderVisible; }
            set
            {
                _senderVisible = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SenderVisible));
            }
        }
        private Visibility _senderVisible = Visibility.Visible;
        public Visibility ScanVisible
        {
            get { return _scanVisible; }
            set
            {
                _scanVisible = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ScanVisible));
            }
        }
        private Visibility _scanVisible = Visibility.Visible;
        public Visibility PleaseSelElmentVisible
        {
            get { return _pleaseSelElementVisible; }
            set
            {
                _pleaseSelElementVisible = value;
                NotifyPropertyChanged(GetPropertyName(o => this.PleaseSelElmentVisible));
            }
        }
        private Visibility _pleaseSelElementVisible = Visibility.Hidden;
        public Visibility ScreenMapLocationVisible
        {
            get { return _screenMapLocationVisible; }
            set
            {
                _screenMapLocationVisible = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ScreenMapLocationVisible));
            }
        }
        private Visibility _screenMapLocationVisible = Visibility.Hidden;

        public bool IsControlEnabled
        {
            get { return _isControlEnabled; }
            set
            {
                _isControlEnabled = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsControlEnabled));
            }
        }
        private bool _isControlEnabled = true;

        public ObservableCollection<LangItemData> LangItemCollection
        {
            get { return _langItemCollection; }
            set
            {
                _langItemCollection = value;
                NotifyPropertyChanged(GetPropertyName(o => this.LangItemCollection));
            }
        }
        private ObservableCollection<LangItemData> _langItemCollection = new ObservableCollection<LangItemData>();
		
        public ActionManager SmartLCTActionManager
        {
            get { return _smartLCTActionManager; }
            set
            {
                _smartLCTActionManager = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SmartLCTActionManager));
            }
        }
        private ActionManager _smartLCTActionManager = new ActionManager();
        public SenderConfigInfo CurrentSenderConfigInfo
        {
            get { return _currentSendrConfigInfo; }
            set 
            {
                _currentSendrConfigInfo = value;
                MyScreen.SelectedSenderConfigInfo = value;
                NotifyPropertyChanged(GetPropertyName(o => this.CurrentSenderConfigInfo));
            }
        }
        private SenderConfigInfo _currentSendrConfigInfo = new SenderConfigInfo();
        public int IncreaseOrDecreaseIndex
        {
            get { return _increaseOrDecreaseIndex; }
            set
            {
                _increaseOrDecreaseIndex = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IncreaseOrDecreaseIndex));
            }
        }
        private int _increaseOrDecreaseIndex = 0;
        public ArrangeType SelectedArrangeType
        {
            get { return _selectedArrangeType; }
            set
            {
                _selectedArrangeType = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SelectedArrangeType));
            }
        }
        private ArrangeType _selectedArrangeType = ArrangeType.LeftTop_Hor;
        public bool IsConnectLine
        {
            get { return _isConnectLine; }
            set 
            {
                _isConnectLine = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsConnectLine));
            }
        }
        private bool _isConnectLine = false;
        public int SelectedEnvironMentIndex
        {
            get { return _selectedEnvironmentIndex; }
            set
            {
                if (IsAddingReceive)
                {
                    value = 0;
                    _selectedEnvironmentIndex = value;
                    NotifyPropertyChanged(GetPropertyName(o => this.SelectedEnvironMentIndex));
                }
                else
                {
                    _selectedEnvironmentIndex = value;
                    OnSelectedEnvironmentChanged();
                    NotifyPropertyChanged(GetPropertyName(o => this.SelectedEnvironMentIndex));
                }
            }
        }
        private int _selectedEnvironmentIndex = 0;

        public IRectElement CurrentScreen
        {
            get { return _currentScreen; }
            set
            {
                //保存发送卡的DVI
                if (value != null && _currentScreen!=null && _currentScreen.SenderIndex != value.SenderIndex)
                {
                    if (_currentScreen.SenderIndex >= 0)
                    {
                        Size dviSize = new Size();
                        dviSize.Width = ScreenMapRealParametersValue.SenderLoadRectLayer.Width;
                        dviSize.Height = ScreenMapRealParametersValue.SenderLoadRectLayer.Height;
                        MyScreen.SenderConnectInfoList[_currentScreen.SenderIndex].DviSize = dviSize;
                    }
                }
       
                IRectElement oldCurrentScreen=new RectElement();
                if (_currentScreen != null)
                {
                    oldCurrentScreen = (IRectElement)((RectElement)_currentScreen).Clone();
                }

                _currentScreen = value;
                SenderRealParameters senderRealPara = new SenderRealParameters();
                //根据单卡多卡分情况
                if (_senderCount == 1)
                {
                    senderRealPara.Element = value;
                    senderRealPara.EleType = ElementType.screen;
                    SenderRealParametersValue = senderRealPara;
                    if (value == null)
                    {
                        SenderVisible = Visibility.Collapsed;
                    }
                    else
                    {
                        SenderVisible = Visibility.Visible;
                    }
                }
                else if (_senderCount > 1)
                {                   
                    if (value != null)
                    {
                        if (value.EleType ==ElementType.groupframe)//选择了一组
                        {
                            NotifyPropertyChanged(GetPropertyName(o => this.CurrentScreen));
                            return;
                        }
                        if(oldCurrentScreen.ConnectedIndex==value.ConnectedIndex && 
                            oldCurrentScreen.SenderIndex==value.SenderIndex &&
                            oldCurrentScreen.PortIndex == value.PortIndex)
                        {
                            NotifyPropertyChanged(GetPropertyName(o => this.CurrentScreen));
                            return;
                        }

                        if (oldCurrentScreen.SenderIndex == CurrentScreen.SenderIndex)
                        {
                            RectLayer screenLayer = (RectLayer)ScreenLocationRectLayer.ElementCollection[0];
                            for (int i = 0; i < screenLayer.ElementCollection.Count; i++)
                            {
                                screenLayer.ElementCollection[i].ElementSelectedState = SelectedState.None;
                            }
                            CurrentScreen.ElementSelectedState = SelectedState.Selected;
                            SenderRealParameters senderRealParas = new SenderRealParameters();
                            senderRealParas.Element = CurrentScreen;
                            senderRealParas.EleType = ElementType.port;
                            SenderRealParametersValue = senderRealParas; //网口基本信息
                            NotifyPropertyChanged(GetPropertyName(o => this.CurrentScreen));

                            return;
                        }
                        #region 选中网口
                        Size sender1LoadSize = MyScreen.SenderConnectInfoList[CurrentScreen.SenderIndex].DviSize;
                        ScreenLocationRectLayer.ElementCollection.Clear();
                        RectLayer screenLocationRectLayer = new RectLayer(0, 0, SmartLCTViewModeBase.DviViewBoxWidth, SmartLCTViewModeBase.DviViewBoxHeight, null, 0, ElementType.baselayer, -1);
                        screenLocationRectLayer.OperateEnviron = OperatEnvironment.AdjustSenderLocation;
                        RectLayer screenBaseloaRectLayer = new RectLayer(0, 0, sender1LoadSize.Width, sender1LoadSize.Height, screenLocationRectLayer, 0, ElementType.screen, -1);
                        screenBaseloaRectLayer.OperateEnviron = OperatEnvironment.AdjustSenderLocation;
                        ObservableCollection<IRectElement> portCollection = new ObservableCollection<IRectElement>();
                        for (int j = 0; j < ScreenMapRealParametersValue.RectLayerCollection.Count; j++)
                        {
                            IRectElement currentPortScreen = (IRectElement)ScreenMapRealParametersValue.RectLayerCollection[j];
                            if (currentPortScreen.PortIndex < 0)
                            {
                                continue;
                            }
                            if (currentPortScreen.SenderIndex == CurrentScreen.SenderIndex && currentPortScreen.PortIndex == CurrentScreen.PortIndex)
                            {
                                RectLayer currentScreen = (RectLayer)((RectLayer)MyScreen.ElementCollection[currentPortScreen.ConnectedIndex]).ElementCollection[0];
                                RectElement e = (RectElement)((RectElement)ScreenMapRealParametersValue.RectLayerCollection[j]);
                                e.ParentElement = screenBaseloaRectLayer;
                                ScreenMapRealParametersValue.RectLayerCollection[j].ElementSelectedState = SelectedState.Selected;
                                e.ElementSelectedState = SelectedState.Selected;

                                e.DisplayName = GetMultiSenderDisplayString(e.ConnectedIndex + 1, e.SenderIndex + 1, e.PortIndex + 1);
                                screenBaseloaRectLayer.ElementCollection.Add(e);
                                portCollection.Add(e);
                                SenderRealParameters senderRealParas = new SenderRealParameters();
                                senderRealParas.Element = e;
                                senderRealParas.EleType = ElementType.port;
                                SenderRealParametersValue = senderRealParas; //网口基本信息
                            }
                            else if (currentPortScreen.SenderIndex == CurrentScreen.SenderIndex)
                            {
                                RectLayer currentScreen = (RectLayer)((RectLayer)MyScreen.ElementCollection[currentPortScreen.ConnectedIndex]).ElementCollection[0];
                                RectElement e = (RectElement)((RectElement)ScreenMapRealParametersValue.RectLayerCollection[j]);
                                ScreenMapRealParametersValue.RectLayerCollection[j].ElementSelectedState = SelectedState.None;
                                e.ParentElement = screenBaseloaRectLayer;
                                e.EleType = ElementType.receive;
                                e.DisplayName = GetMultiSenderDisplayString(e.ConnectedIndex + 1, e.SenderIndex + 1, e.PortIndex + 1);
                                screenBaseloaRectLayer.ElementCollection.Add(e);

                            }
                            else
                            {
                                ScreenMapRealParametersValue.RectLayerCollection[j].ElementSelectedState = SelectedState.None;
                            }
                        }
                        for (int m = 0; m < ScreenMapRealParametersValue.RectLayerCollection.Count; m++)
                        {
                            if (ScreenMapRealParametersValue.RectLayerCollection[m].EleType == ElementType.groupframe)
                            {
                                ObservableCollection<IRectElement> portList = new ObservableCollection<IRectElement>();
                                for (int j = 0; j < screenBaseloaRectLayer.ElementCollection.Count; j++)
                                {
                                    //同一个屏下面的同一张发送卡下的网口用组框框起来
                                    if (((IRectElement)screenBaseloaRectLayer.ElementCollection[j]).ConnectedIndex == ScreenMapRealParametersValue.RectLayerCollection[m].ConnectedIndex &&
                                        ((IRectElement)screenBaseloaRectLayer.ElementCollection[j]).SenderIndex == ((IRectElement)ScreenMapRealParametersValue.RectLayerCollection[m]).SenderIndex)
                                    {
                                        portList.Add((IRectElement)screenBaseloaRectLayer.ElementCollection[j]);
                                    }
                                }
                                if (portList.Count < 2)
                                {
                                    continue;
                                }

                                Rect groupRect = Function.UnionRectCollection(portList);
                                RectElement groupElement =(RectElement) ((RectElement)ScreenMapRealParametersValue.RectLayerCollection[m]).Clone();
                                groupElement.X = groupRect.X;
                                groupElement.Y = groupRect.Y;
                                groupElement.Width = groupRect.Width;
                                groupElement.Height = groupRect.Height;
                                groupElement.ParentElement = screenBaseloaRectLayer;
                                screenBaseloaRectLayer.MaxGroupName += 1;
                                screenBaseloaRectLayer.ElementCollection.Add(groupElement);
                            }
                        }
                        screenLocationRectLayer.ElementCollection.Add(screenBaseloaRectLayer);
                        screenLocationRectLayer.SenderIndex = CurrentScreen.SenderIndex;
                        int increaseOrDecreaseIndex = GetIncreaseOrDecreaseIndex(CurrentScreen.SenderIndex, ((RectLayer)screenLocationRectLayer.ElementCollection[0]));
                        ((RectLayer)screenLocationRectLayer.ElementCollection[0]).IncreaseOrDecreaseIndex = increaseOrDecreaseIndex;
                        Point newLocation = GetDviCenterPoint(increaseOrDecreaseIndex, ((RectLayer)screenLocationRectLayer.ElementCollection[0]));
                        ((RectLayer)screenLocationRectLayer.ElementCollection[0]).X = newLocation.X;
                        ((RectLayer)screenLocationRectLayer.ElementCollection[0]).Y = newLocation.Y;
                        ScreenLocationRectLayer = screenLocationRectLayer;//当前调整映射位置的发送卡
                        MyScreen.SenderConnectInfoList[CurrentScreen.SenderIndex].IncreaseOrDecreaseIndex = increaseOrDecreaseIndex;
                        MyScreen.SenderConnectInfoList[CurrentScreen.SenderIndex].IsStatIncreaseOrDecreaseIndex = true;

                        ScreenMapRealParameters p = new ScreenMapRealParameters();
                        p.SenderLoadRectLayer = screenBaseloaRectLayer;
                        p.RectLayerCollection = ScreenMapRealParametersValue.RectLayerCollection;
                        p.RectLayerType = ElementType.sender;

                        ScreenMapRealParametersValue = p;
                        #endregion
                    }
                }
                NotifyPropertyChanged(GetPropertyName(o => this.CurrentScreen));
            }
        }
        private IRectElement _currentScreen = new RectElement();
        public IRectElement CurrentSenderScreen
        {
            get { return _currentSenderScreen; }
            set 
            {
                _currentSenderScreen = value;
        
                NotifyPropertyChanged(GetPropertyName(o => this.CurrentSenderScreen));
            }
        }
        private IRectElement _currentSenderScreen = null;
        public int CheckSenderIndex
        {
            get { return _checkSenderIndex; }
            set
            {
                _checkSenderIndex = value;
                NotifyPropertyChanged(GetPropertyName(o => this.CheckSenderIndex));
            }
        }
        private int _checkSenderIndex = -1;
        public bool IsAddingReceive
        {
            get { return _isAddingReceive; }
            set
            {
                _isAddingReceive = value;
                IsControlEnabled = !value;//添加接收卡时禁用其他关于点击、输入的功能
                NotifyPropertyChanged(GetPropertyName(o => this.IsAddingReceive));
            }
        }
        private bool _isAddingReceive = false;
        public Dictionary<int,SenderAndPortPicInfo> SenderAndPortPicCollection
        {
            get { return _senderAndPortPicCollection; }
            set 
            {
                _senderAndPortPicCollection=value;
                NotifyPropertyChanged(GetPropertyName(o => this.SenderAndPortPicCollection));
            }
        }
        private Dictionary<int,SenderAndPortPicInfo> _senderAndPortPicCollection = new Dictionary<int,SenderAndPortPicInfo>();

        public double WidthOfPropertyArea
        {
            get { return _widthOfPropertyArea; }
            set
            {
                _widthOfPropertyArea = value;
                NotifyPropertyChanged(GetPropertyName(o => this.WidthOfPropertyArea));
            }
        }
        private double _widthOfPropertyArea=300;
        #endregion

        #region 命令
        public RelayCommand CmdNewWizard
        {
            get;
            private set;
        }
        public RelayCommand CmdNewEmptyProject
        {
            get;
            private set;
        }
        public RelayCommand<RectLayer> CmdNewLayer
        {
            get;
            private set;
        }
        public RelayCommand CmdMenuNewLayer
        {
            get;
            private set;
        }
        public RelayCommand<IElement> CmdDeleteLayer
        {
            get;
            private set;
        }

        public RelayCommand CmdOtherSaveSysConfigFile
        {
            get;
            private set;
        } 
        public RelayCommand CmdSaveSysConfigFile
        {
            get;
            private set;
        }
        public RelayCommand CmdOpenConfigFile
        {
            get;
            private set;
        }

        public RelayCommand CmdClose
        {
            get;
            private set;
        }
        public RelayCommand<CancelEventArgs> CmdExit
        {
            get;
            private set;
        }

        public RelayCommand<PortConnectInfo> CmdSelectedTreeViewValueWithPort
        {
            get;
            private set;
        }
        public RelayCommand<SenderConnectInfo> CmdSelectedTreeViewValueWithSender
        {
            get;
            private set;
        }   

        public RelayCommand CmdShowSystemCheckDlg
        {
            get;
            private set;
        }
        public RelayCommand CmdShowSendDisplayInfoDlg
        {
            get;
            private set;
        }
        public RelayCommand CmdShowScanBoardConfigManager
        {
            get;
            private set;
        }
        public RelayCommand CmdShowEDIDManager
        {
            get;
            private set;
        }
        public RelayCommand CmdShowBrightAdjust
        {
            get;
            private set;
        }

        public RelayCommand CmdStartTestTool
        {
            get;
            private set;
        }
        public RelayCommand CmdStartCalculator
        {
            get;
            private set;
        }

        public RelayCommand<SkinType> CmdChangedSkin
        {
            get;
            private set;
        }

        public RelayCommand<string> CmdChangedLang
        {
            get;
            private set;
        }
        #endregion

        #region 字段
        private List<Process> _myStartProcessList = new List<Process>();
        private ObservableCollection<IElement> serialRectLayerCollection = new ObservableCollection<IElement>();
        private SelectedLayerAndElement _selectedLayerAndELment = new SelectedLayerAndElement();
        private int _senderCount = 0;
        private int _oldSenderCount = 0;
        private Size _mainControlSize = new Size();
        #endregion

        #region 构造
        public MainWindow_VM()
        {
            string msg = "显示屏连线配置";
            CommonStaticMethod.GetLanguageString(msg, "Lang_ScrCfg_ScreenName", out msg);
            WindowRealTitle = msg;
            MyScreen.ElementCollection = MyScreen.ElementCollection;
            MyScreen.EleType = ElementType.baseScreen;
            MyScreen.OperateEnviron = OperatEnvironment.DesignScreen;

            if (!this.IsInDesignMode)
            {
                ScannerTypeCollection = _globalParams.ScannerConfigCollection;
                SenderConfigCollection = _globalParams.SenderConfigCollection;
                LangItemCollection = _globalParams.LangItemCollection;
                if (_globalParams.SenderConfigCollection != null && _globalParams.SenderConfigCollection.Count != 0)
                {
                    CurrentSenderConfigInfo = _globalParams.SenderConfigCollection[0];
                }
                else
                {
                    SenderConfigInfo info = new SenderConfigInfo();
                    info.SenderTypeName = "MCTRL500";
                    info.PortCount = 4;
                    CurrentSenderConfigInfo = info;
                }
                if (_globalParams.SenderAndPortPicCollection != null && _globalParams.SenderAndPortPicCollection.Count != 0)
                {
                    SenderAndPortPicCollection = _globalParams.SenderAndPortPicCollection;
                }
            }
            else
            {
                LangItemCollection.Add(new LangItemData() { LangDisplayName = "中文", LangFlag = "zh-CN", IsSelected = false });
            }
      
            if (!this.IsInDesignMode)
            {
                #region 初始化显示屏的发送卡信息
                ObservableCollection<PortConnectInfo> portConnectList = new ObservableCollection<PortConnectInfo>();
                for (int i = 0; i < CurrentSenderConfigInfo.PortCount; i++)
                {
                    portConnectList.Add(new PortConnectInfo(i,0, -1, null, null, new Rect()));
                }
                MyScreen.SenderConnectInfoList.Add(new SenderConnectInfo(0, portConnectList, new Rect()));

                #endregion

                SenderRealParametersValue = new SenderRealParameters();
                SenderRealParametersValue.Element = new RectElement();
                SenderRealParametersValue = SenderRealParametersValue;
                #region 注册消息
                Messenger.Default.Register<SelectedLayerAndElement>(this, MsgToken.MSG_SELECTEDLAYERANDELEMENT_CHANGED, OnSelectedLayerAndElementChanged);
                Messenger.Default.Register<int>(this, MsgToken.MSG_INCREASEORDECREASEINDEX, OnIncreaseOrDecreaseIndexChanged);

                #endregion
                #region 显示屏
                RectLayer myRectLayer3 = new RectLayer(0, 0, SmartLCTViewModeBase.MaxScreenWidth + SmartLCTViewModeBase.ScrollWidth, SmartLCTViewModeBase.MaxScreenHeight + SmartLCTViewModeBase.ScrollWidth, MyScreen, 0, ElementType.baselayer, 0);
                myRectLayer3.OperateEnviron = OperatEnvironment.DesignScreen;
                RectLayer Layer3_sender1 = new RectLayer(0, 0, SmartLCTViewModeBase.MaxScreenWidth, SmartLCTViewModeBase.MaxScreenHeight, myRectLayer3, 3, ElementType.screen, 0);
                Layer3_sender1.OperateEnviron = OperatEnvironment.DesignScreen;
                myRectLayer3.ElementSelectedState = SelectedState.None;
                myRectLayer3.ElementCollection.Add(Layer3_sender1);
                MyScreen.ElementCollection.Add(myRectLayer3);
                SelectedScreenLayer = myRectLayer3;
                #endregion
                #region 显示屏映射
                RectLayer screenLocationRectLayer = new RectLayer(0, 0, 2000 * 4, 1000 * 4, null, 0, ElementType.baselayer, 0);
                screenLocationRectLayer.OperateEnviron = OperatEnvironment.AdjustScreenLocation;
                ScreenLocationRectLayer = screenLocationRectLayer;
                #endregion
                #region 新建
                RectLayer newLayer = new RectLayer(0, 0, SmartLCTViewModeBase.MaxScreenWidth, SmartLCTViewModeBase.MaxScreenHeight, MyScreen, -1, ElementType.newLayer, -1);
                MyScreen.ElementCollection.Add(newLayer);
                #endregion

                #region 定义命令执行函数
                CmdNewWizard = new RelayCommand(OnCmdWizardProject);
                CmdNewEmptyProject = new RelayCommand(OnCmdNewEmptyProject);
                CmdNewLayer = new RelayCommand<RectLayer>(OnCmdNewLayer, CanCmdNewLayer);
                CmdMenuNewLayer = new RelayCommand(OnCmdMenuNewLayer);
                CmdDeleteLayer = new RelayCommand<IElement>(OnCmdDeleteLayer, CanCmdDeleteLayer);

                //CmdSaveSysConfigFile = new RelayCommand(OnCmdSaveSysConfigFile);
                CmdSaveSysConfigFile = new RelayCommand(OnOtherSaveSysConfigFile);
                CmdOtherSaveSysConfigFile = new RelayCommand(OnOtherSaveSysConfigFile);
                CmdOpenConfigFile = new RelayCommand(OnCmdOpenConfigFile);

                CmdClose = new RelayCommand(OnCmdClose);
                CmdExit = new RelayCommand<CancelEventArgs>(OnCmdExit);

                CmdSelectedTreeViewValueWithSender = new RelayCommand<SenderConnectInfo>(OnCmdSelectedTreeViewValueWithSender);
                CmdSelectedTreeViewValueWithPort = new RelayCommand<PortConnectInfo>(OnCmdSelectedTreeViewValueWithPort);

                CmdShowSystemCheckDlg = new RelayCommand(OnCmdShowSystemCheckDlg);
                CmdShowSendDisplayInfoDlg = new RelayCommand(OnCmdShowSendDisplayInfoDlg);
                CmdShowScanBoardConfigManager = new RelayCommand(OnCmdShowScanBoardConfigManager);
                CmdShowEDIDManager = new RelayCommand(OnCmdShowEDIDManager);
                CmdShowBrightAdjust = new RelayCommand(OnCmdShowBrightAdjust);

                CmdStartTestTool = new RelayCommand(OnCmdStartTestTool);
                CmdStartCalculator = new RelayCommand(OnCmdStartCalculator);

                CmdChangedSkin = new RelayCommand<SkinType>(OnCmdChangedSkin);

                CmdChangedLang = new RelayCommand<string>(OnCmdChangedLang);
                #endregion
                ScreenMapLocationVisible = Visibility.Collapsed;
                SenderVisible = Visibility.Collapsed;
            }
        }
        #endregion

        #region  命令处理函数
        private void OnCmdExit(CancelEventArgs e)
        {
            if (e == null)
            {
                Messenger.Default.Send<object>(null, MsgToken.MSG_EXIT);
                return;
            }
            string msg = "";
            CommonStaticMethod.GetLanguageString("是否保存？", "Lang_SmartLCT_VM_IsSave", out msg);
            MessageBoxResult result = ShowQuestionMessage(msg, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                OnOtherSaveSysConfigFile();
            }
            else if (result == MessageBoxResult.Cancel)
            {
                if (e != null)
                {
                    e.Cancel = true;
                }
                return;
            }
            else if (result == MessageBoxResult.No)
            {
                //如果该工程之前保存过，则将最近打开工程路径中此次文件提到最前面
                string projectPath = MyConfigurationData.ProjectLocationPath + "\\" + MyConfigurationData.ProjectName + SmartLCTViewModeBase.ProjectExtension;
                if (File.Exists(projectPath))
                {
                    for (int i = 0; i < _globalParams.RecentProjectPaths.Count; i++)
                    {
                        if (_globalParams.RecentProjectPaths[i].ProjectPath == projectPath)
                        {
                            _globalParams.RecentProjectPaths.Move(i, 0);
                            break;
                        }
                    }
                }
            }
            //保存最近打开的工程文件路径
            SaveRecentProjectFile(_globalParams.RecentProjectPaths);
            Messenger.Default.Unregister<SelectedLayerAndElement>(this, MsgToken.MSG_SELECTEDLAYERANDELEMENT_CHANGED, OnSelectedLayerAndElementChanged);            
        }

        private void OnCmdClose()
        {
            string msg = "";
            CommonStaticMethod.GetLanguageString("是否保存？", "Lang_SmartLCT_VM_IsSave", out msg);
            MessageBoxResult result = ShowQuestionMessage(msg, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                OnOtherSaveSysConfigFile();
            }
            else if (result == MessageBoxResult.Cancel)
            {
                return;
            }
            MyScreen.ElementCollection.Clear();
            #region 新建
            RectLayer newLayer = new RectLayer(0, 0, SmartLCTViewModeBase.MaxScreenWidth, SmartLCTViewModeBase.MaxScreenHeight, MyScreen, -1, ElementType.newLayer, -1);
            MyScreen.ElementCollection.Add(newLayer);
            #endregion
        }
        /// <summary>
        /// 打开项目文件
        /// </summary>
        private void OnCmdOpenConfigFile()
        {
            string filename = Function.GetConfigFileName(MyConfigurationData.ProjectLocationPath);
            if (filename != "")
            {
                //提示是否保存当前工程
                string msg = "";
                CommonStaticMethod.GetLanguageString("是否保存当前工程？", "Lang_SmartLCT_VM_IsSave", out msg);
                MessageBoxResult result = ShowQuestionMessage(msg, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    OnOtherSaveSysConfigFile();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
                OpenConfigFileAndHandleData(filename);
            }
        }
        /// <summary>
        /// 项目文件另存为
        /// </summary>
        private void OnOtherSaveSysConfigFile()
        {
            string fileName = SaveSysConfigFileDialog(MyConfigurationData.ProjectLocationPath, MyConfigurationData.ProjectName);
            if (fileName != "")
            {
                SaveSysConfigFile(fileName);
                //更新保存路径
                string[] splitName = fileName.Split('\\');
                foreach (string name in splitName)
                {
                    MyConfigurationData.ProjectName = name;
                }
                MyConfigurationData.ProjectName = MyConfigurationData.ProjectName.Substring(0, MyConfigurationData.ProjectName.Length - SmartLCTViewModeBase.ProjectExtension.Length);
                MyConfigurationData.ProjectLocationPath = fileName.Substring(0, fileName.Length - SmartLCTViewModeBase.ProjectExtension.Length - MyConfigurationData.ProjectName.Length - 1);

            }

        }
        /// <summary>
        /// 保存项目文件到新建时设置路径下
        /// </summary>
        private void OnCmdSaveSysConfigFile()
        {
            SaveSysConfigFile(MyConfigurationData.ProjectLocationPath + "\\" + MyConfigurationData.ProjectName + ".xml");
        }
        private void OnCmdSelectedTreeViewValueWithSender(SenderConnectInfo senderInfo)
        {
            if (IsAddingReceive)
            {
                return;
            }
            Function.SetElementCollectionState(SelectedScreenLayer.ElementCollection, SelectedState.None);
            if (SelectedScreenLayer != null && SelectedScreenLayer.SenderConnectInfoList != null && SelectedScreenLayer.SenderConnectInfoList.Count != 0)
            {
                SenderConnectInfo senderConnectInfo = SelectedScreenLayer.SenderConnectInfoList[senderInfo.SenderIndex];
                for (int i = 0; i < senderConnectInfo.PortConnectInfoList.Count; i++)
                {
                    senderInfo.IsSelected = true;
   
                    Function.SetElementCollectionState(senderConnectInfo.PortConnectInfoList[i].ConnectLineElementList, SelectedState.Selected);

                }
            }
        }
        private void OnCmdSelectedTreeViewValueWithPort(PortConnectInfo portInfo)
        {
            if (IsAddingReceive)
            {
                return;
            }
            portInfo.IsSelected = true;

            if (SelectedScreenLayer != null && SelectedScreenLayer.SenderConnectInfoList != null && SelectedScreenLayer.SenderConnectInfoList.Count != 0)
            {
                if (SelectedScreenLayer.SenderConnectInfoList[portInfo.SenderIndex].PortConnectInfoList != null &&
                    SelectedScreenLayer.SenderConnectInfoList[portInfo.SenderIndex].PortConnectInfoList.Count != 0)
                {
                    PortConnectInfo portConnectInfo = SelectedScreenLayer.SenderConnectInfoList[portInfo.SenderIndex].PortConnectInfoList[portInfo.PortIndex];
                    Function.SetElementCollectionState(SelectedScreenLayer.ElementCollection, SelectedState.None);
                    Function.SetElementCollectionState(portConnectInfo.ConnectLineElementList, SelectedState.Selected);
                }
            }
        }

        private void OnCmdWizardProject()
        {
            //保存当前工程
            string msg = "";
            CommonStaticMethod.GetLanguageString("是否保存？", "Lang_SmartLCT_VM_IsSave", out msg);
            MessageBoxResult result = ShowQuestionMessage(msg, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                //SaveSysConfigFile(MyConfigurationData.ProjectLocationPath + "\\" + MyConfigurationData.ProjectName + ".xml");
                OnOtherSaveSysConfigFile();
            }
            else if (result == MessageBoxResult.Cancel)
            {
                return;
            }
            //发送新建工程的消息，并返回新建的信息    
            ConfigurationData data = new ConfigurationData();
            data.OperateType = OperateScreenType.UpdateScreen;
            NotificationMessageAction<ConfigurationData> nsa =
            new NotificationMessageAction<ConfigurationData>(this, data, MsgToken.MSG_SHOWGUIDETWO, NewProjectCallBack);
            Messenger.Default.Send(nsa, MsgToken.MSG_SHOWGUIDETWO);

        }
        private void NewProjectCallBack(ConfigurationData data)
        {
            if (data != null)
            {
                //新建工程
                MyScreen.ElementCollection.Clear();
                //更新发送卡信息
                MyScreen.SenderConnectInfoList = new ObservableCollection<SenderConnectInfo>();
                ObservableCollection<PortConnectInfo> portConnectInfoList = new ObservableCollection<PortConnectInfo>();
                for (int i = 0; i < CurrentSenderConfigInfo.PortCount; i++)
                {
                    PortConnectInfo portConnectInfo = new PortConnectInfo(i, 0, -1, null, null, new Rect());
                    portConnectInfoList.Add(portConnectInfo);
                }
                SenderConnectInfo senderConnectIinfo = new SenderConnectInfo(0, portConnectInfoList, new Rect());
                MyScreen.SenderConnectInfoList.Add(senderConnectIinfo);

                RectLayer myRectLayer3 = new RectLayer(0, 0, SmartLCTViewModeBase.MaxScreenWidth + SmartLCTViewModeBase.ScrollWidth, SmartLCTViewModeBase.MaxScreenHeight + SmartLCTViewModeBase.ScrollWidth, MyScreen, 0, ElementType.baselayer, 0);

                RectLayer Layer3_sender1 = new RectLayer(0, 0, SmartLCTViewModeBase.MaxScreenWidth, SmartLCTViewModeBase.MaxScreenHeight, myRectLayer3, 3, ElementType.screen, 0);
                myRectLayer3.ElementSelectedState = SelectedState.None;
                myRectLayer3.ElementCollection.Add(Layer3_sender1);
                MyScreen.ElementCollection.Add(myRectLayer3);

                RectLayer newLayer = new RectLayer(0, 0, SmartLCTViewModeBase.MaxScreenWidth, SmartLCTViewModeBase.MaxScreenHeight, MyScreen, -1, ElementType.newLayer, -1);
                MyScreen.ElementCollection.Add(newLayer);
                MyConfigurationData = data;
                if (SelectedEnvironMentIndex == 1)
                {
                    SelectedEnvironMentIndex = 1;
                }
            }
        }
        private void OnCmdNewEmptyProject()
        {
            //保存当前工程
            string msg = "";
            CommonStaticMethod.GetLanguageString("是否保存？", "Lang_SmartLCT_VM_IsSave", out msg);
            MessageBoxResult result = ShowQuestionMessage(msg, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                //SaveSysConfigFile(MyConfigurationData.ProjectLocationPath + "\\" + MyConfigurationData.ProjectName + ".xml");
                OnOtherSaveSysConfigFile();
            }
            else if (result == MessageBoxResult.Cancel)
            {
                return;
            }
            //新建工程
            MyScreen.ElementCollection.Clear();
            //更新发送卡信息
            MyScreen.SenderConnectInfoList = new ObservableCollection<SenderConnectInfo>();
            ObservableCollection<PortConnectInfo> portConnectInfoList = new ObservableCollection<PortConnectInfo>();
            for (int i = 0; i < CurrentSenderConfigInfo.PortCount; i++)
            {
                PortConnectInfo portConnectInfo = new PortConnectInfo(i, 0, -1, null, null, new Rect());
                portConnectInfoList.Add(portConnectInfo);
            }
            SenderConnectInfo senderConnectIinfo = new SenderConnectInfo(0, portConnectInfoList, new Rect());
            MyScreen.SenderConnectInfoList.Add(senderConnectIinfo);

            RectLayer myRectLayer3 = new RectLayer(0, 0, SmartLCTViewModeBase.MaxScreenWidth + SmartLCTViewModeBase.ScrollWidth, SmartLCTViewModeBase.MaxScreenHeight + SmartLCTViewModeBase.ScrollWidth, MyScreen, 0, ElementType.baselayer, 0);

            RectLayer Layer3_sender1 = new RectLayer(0, 0, SmartLCTViewModeBase.MaxScreenWidth, SmartLCTViewModeBase.MaxScreenHeight, myRectLayer3, 3, ElementType.screen, 0);
            myRectLayer3.ElementSelectedState = SelectedState.None;
            myRectLayer3.ElementCollection.Add(Layer3_sender1);
            MyScreen.ElementCollection.Add(myRectLayer3);

            RectLayer newLayer = new RectLayer(0, 0, SmartLCTViewModeBase.MaxScreenWidth, SmartLCTViewModeBase.MaxScreenHeight, MyScreen, -1, ElementType.newLayer, -1);
            MyScreen.ElementCollection.Add(newLayer);
            if (SelectedEnvironMentIndex == 1)
            {
                SelectedEnvironMentIndex = 1;
            }

            //ConfigurationData ConfigData = new ConfigurationData();
            //ConfigData.OperateType = OperateScreenType.CreateScreen;
            //ConfigData.IsCreateEmptyProject = true;
            //ConfigData.SelectedScannerConfigInfo = null;
            //ConfigData.ProjectLocationPath = Function.GetDefaultCurrentProjectPath(_globalParams.RecentProjectPaths);
            //ConfigData.ProjectName = Function.GetDefaultProjectName(SmartLCTViewModeBase.DefaultProjectMainName, ".xml", ConfigData.ProjectLocationPath);
            //MyConfigurationData = ConfigData;
        }
        private void OnCmdMenuNewLayer()
        {
            RectLayer newLayer = new RectLayer(0, 0, SmartLCTViewModeBase.MaxScreenWidth + SmartLCTViewModeBase.ScrollWidth, SmartLCTViewModeBase.MaxScreenHeight + SmartLCTViewModeBase.ScrollWidth, MyScreen, 0, ElementType.baselayer, MyScreen.ElementCollection.Count - 1);
            RectLayer newLayer_sender1 = new RectLayer(0, 0, SmartLCTViewModeBase.MaxScreenWidth, SmartLCTViewModeBase.MaxScreenHeight, newLayer, 1, ElementType.screen, 0);
            newLayer_sender1.ElementSelectedState = SelectedState.None;
            newLayer.ElementCollection.Add(newLayer_sender1);
            _myScreen.ElementCollection.Insert(MyScreen.ElementCollection.Count - 1, newLayer);
            SelectedValue = newLayer;
        }

        private bool CanCmdNewLayer(RectLayer layer)
        {
            if (IsAddingReceive)
            {
                return false;
            }
            return true;
        }
        private void OnCmdNewLayer(RectLayer element)
        {
            if (element.ConnectedIndex == -1)
            {
                RectLayer newLayer = new RectLayer(0, 0, SmartLCTViewModeBase.MaxScreenWidth + SmartLCTViewModeBase.ScrollWidth, SmartLCTViewModeBase.MaxScreenHeight + SmartLCTViewModeBase.ScrollWidth, MyScreen, 0, ElementType.baselayer, MyScreen.ElementCollection.Count - 1);
                RectLayer newLayer_sender1 = new RectLayer(0, 0, SmartLCTViewModeBase.MaxScreenWidth, SmartLCTViewModeBase.MaxScreenHeight, newLayer, 1, ElementType.screen, 0);
                newLayer_sender1.SelectedSenderConfigInfo = MyScreen.SelectedSenderConfigInfo;
                CurrentSenderConfigInfo = MyScreen.SelectedSenderConfigInfo;
                newLayer_sender1.ElementSelectedState = SelectedState.None;
                newLayer.ElementCollection.Add(newLayer_sender1);
                MyScreen.ElementCollection.Insert(MyScreen.ElementCollection.Count - 1, newLayer);
                SelectedValue = (RectLayer)MyScreen.ElementCollection[MyScreen.ElementCollection.Count - 2];
            }
            else
            {
                SelectedValue = element;
            }

        }

        private bool CanCmdDeleteLayer(IElement element)
        {
            if (IsAddingReceive)
            {
                return false;
            }
            return true;
        }
        private void OnCmdDeleteLayer(IElement element)
        {
            string msg = "";
            CommonStaticMethod.GetLanguageString("确认要删除显示屏", "Lang_SmartLCT_MainWin_SureToDeleScr", out msg);
            MessageBoxResult result = ShowQuestionMessage(msg + " " + (element.ConnectedIndex + 1).ToString() + "?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            MyScreen.ElementCollection.Remove((RectLayer)element);
            for (int i = 0; i < MyScreen.ElementCollection.Count; i++)
            {
                if (MyScreen.ElementCollection[i].ConnectedIndex == -1)
                {
                    MyScreen.ElementCollection.Move(i, MyScreen.ElementCollection.Count - 1);
                }
            }
            for (int j = 0; j < MyScreen.ElementCollection.Count - 1; j++)
            {
                MyScreen.ElementCollection[j].ConnectedIndex = j;
                //string msg = "";
                //CommonStaticMethod.GetLanguageString("显示屏", "Lang_SmartLCT_VM_Screen", out msg);
                //RectLayerCollection[j].DisplayName = msg + (j + 1).ToString();

            }
            if (SelectedValue == null)
            {

                if (MyScreen.ElementCollection.Count - 2 < 0)
                {
                    SelectedValue = (RectLayer)MyScreen.ElementCollection[0];
                    //    SelectedIndex = 0;
                }
                else
                {
                    SelectedValue = (RectLayer)MyScreen.ElementCollection[MyScreen.ElementCollection.Count - 2];
                    //  SelectedIndex = MyScreen.ElementCollection.Count - 2;
                }
            }
            //更新带载
            for (int j = 0; j < MyScreen.ElementCollection.Count; j++)
            {
                if (MyScreen.ElementCollection[j].EleType == ElementType.newLayer)
                {
                    continue;
                }
                RectLayer reLayer = (RectLayer)((RectLayer)MyScreen.ElementCollection[j]).ElementCollection[0];
                Function.UpdateSenderConnectInfo(reLayer.SenderConnectInfoList, MyScreen);
            }
            if (MyScreen.ElementCollection.Count == 1)
            {
                for (int i = 0; i < MyScreen.SenderConnectInfoList.Count; i++)
                {
                    MyScreen.SenderConnectInfoList[i].LoadSize = new Rect();
                    if (MyScreen.SenderConnectInfoList[i].PortConnectInfoList != null)
                    {
                        ObservableCollection<PortConnectInfo> portConnectInfoList = MyScreen.SenderConnectInfoList[i].PortConnectInfoList;
                        for (int j = 0; j < portConnectInfoList.Count; j++)
                        {
                            if (portConnectInfoList[j].ConnectLineElementList != null)
                            {
                                portConnectInfoList[j].ConnectLineElementList.Clear();
                            }
                            portConnectInfoList[j].LoadSize = new Rect();
                            portConnectInfoList[j].MaxConnectElement = null;
                            portConnectInfoList[j].MaxConnectIndex = -1;
                        }
                    }
                }
            }
        }

        private void OnCmdShowSystemCheckDlg()
        {
            //ShowGlobalDialogMessage("Haha", MessageBoxImage.Information);
            if (_globalParams != null)
            {
                _globalParams.IsSendCurrentDisplayConfig = false;

                Messenger.Default.Send<string>("", MsgToken.MSG_SHOWEQUIPMENTMANAGER);
            }
        }

        private void OnCmdShowSendDisplayInfoDlg()
        {
            if (_globalParams != null)
            {
                _globalParams.IsSendCurrentDisplayConfig = true;
                _globalParams.CurrentDisplayInfoList = new List<ILEDDisplayInfo>();

                #region 屏体信息
                List<ILEDDisplayInfo> ledInfoList = new List<ILEDDisplayInfo>();

                for (int i = 0; i < MyScreen.ElementCollection.Count; i++)
                {
                    if (MyScreen.ElementCollection[i].ConnectedIndex == -1)
                    {
                        continue;
                    }
                    ComplexLEDDisplayInfo info = new ComplexLEDDisplayInfo();

                    List<RectElement> receiveList = new List<RectElement>();
                    receiveList = FindRect((RectLayer)MyScreen.ElementCollection[i]);
                    for (int j = 0; j < receiveList.Count; j++)
                    {
                        ScanBoardRegionInfo receiveInfo = new ScanBoardRegionInfo();
                        receiveInfo.Height = (ushort)receiveList[j].Height;
                        receiveInfo.Width = (ushort)receiveList[j].Width;
                        receiveInfo.ConnectIndex = (ushort)receiveList[j].ConnectedIndex;
                        int portIndex = receiveList[j].ParentElement.ConnectedIndex;
                        receiveInfo.PortIndex = (byte)portIndex;
                        int senderIndex = receiveList[j].ParentElement.ParentElement.ConnectedIndex;
                        receiveInfo.SenderIndex = (byte)senderIndex;
                        receiveInfo.X = (ushort)receiveList[j].X;
                        receiveInfo.Y = (ushort)receiveList[j].Y;

                        RectLayer parent = new RectLayer();
                        parent = (RectLayer)receiveList[j].ParentElement;
                        while (parent.ParentElement != null)
                        {
                            receiveInfo.X += (ushort)parent.X;
                            receiveInfo.Y += (ushort)parent.Y;
                            parent = (RectLayer)parent.ParentElement;
                        }
                        receiveInfo.XInPort = (ushort)receiveList[j].X;
                        receiveInfo.YInPort = (ushort)receiveList[j].Y;

                        info.ScanBoardRegionInfoList.Add(receiveInfo);
                    }

                    ledInfoList.Add(info);
                }
                #endregion
                _globalParams.CurrentDisplayInfoList = GetLedDisplayInfoList();
                Messenger.Default.Send<string>("", MsgToken.MSG_SHOWEQUIPMENTMANAGER);
            }
        }
        private void OnCmdShowScanBoardConfigManager()
        {
            Messenger.Default.Send<string>("", MsgToken.MSG_SHOWSCANBOARDCONFIGMANAGER);
        }

        private void OnCmdShowEDIDManager()
        {
            Messenger.Default.Send<string>("", MsgToken.MSG_SHOWEDIDMANAGER);
        }

        private void OnCmdShowBrightAdjust()
        {
            Messenger.Default.Send<string>("", MsgToken.MSG_SHOWBRIGHTADJUST);
        }

        private void OnCmdStartTestTool()
        {
            StartTestTool();
        }
        private void OnCmdStartCalculator()
        {
            StartCalculator();
        }

        #endregion

        #region 私有
        /// <summary>
        /// 切换设计界面和调整映射位置界面
        /// </summary>
        private void OnSelectedEnvironmentChanged()
        {
            List<int> senderIndexList = new List<int>();
            int senderIndex;
            _oldSenderCount = _senderCount;
            int senderCount = Function.FindSenderCount(MyScreen.SenderConnectInfoList, out senderIndex);
            _senderCount = senderCount;
            if (_oldSenderCount != senderCount)
            {
                //发送卡个数更改，则map置为初始值
                for (int i = 0; i < MyScreen.ElementCollection.Count; i++)
                {
                    if (MyScreen.ElementCollection[i].EleType == ElementType.newLayer)
                    {
                        continue;
                    }
                    RectLayer screen = (RectLayer)((RectLayer)MyScreen.ElementCollection[i]).ElementCollection[0];
                    Function.InitMapLocation(screen);
                }
            }
            if (SelectedEnvironMentIndex == 0)
            {
                ScreenMapLocationVisible = Visibility.Collapsed;
                SenderVisible = Visibility.Collapsed;
                ScanVisible = Visibility.Collapsed;
                CustomScanVisible = Visibility.Collapsed;
                if (MyScreen!=null && MyScreen.ElementCollection!=null)
                {
                    if (MyScreen.ElementCollection.Count <= 1)
                    {
                        ScreenVisible = Visibility.Collapsed;
                    }
                    else
                    {
                        ScreenVisible = Visibility.Visible;
                    }
                }
                else
                {
                    ScreenVisible = Visibility.Collapsed;
                }
                PleaseSelElmentVisible = Visibility.Hidden;
            }
            else if (SelectedEnvironMentIndex == 1)
            {
                //查看用了多少个发送卡
                #region 单发送卡
                if (senderCount == 0)
                {
                    ScreenVisible = Visibility.Collapsed;
                    ScanVisible = Visibility.Collapsed;
                    CustomScanVisible = Visibility.Collapsed;
                    PleaseSelElmentVisible = Visibility.Hidden;
                    SenderVisible = Visibility.Collapsed;
                    ScreenMapLocationVisible = Visibility.Collapsed;
                    ScreenLocationRectLayer = new RectLayer(0, 0, SmartLCTViewModeBase.DviViewBoxWidth, SmartLCTViewModeBase.DviViewBoxHeight, null, 0, ElementType.baselayer, 0);
                }
                else if (senderCount == 1)
                {

                    ScreenMapLocationVisible = Visibility.Visible;
                    ScreenVisible = Visibility.Collapsed;
                    ScanVisible = Visibility.Collapsed;
                    CustomScanVisible = Visibility.Collapsed;
                    PleaseSelElmentVisible = Visibility.Hidden;
                    int selectedIndex = -1;
                    for (int i = 0; i < ScreenMapRealParametersValue.RectLayerCollection.Count; i++)
                    {
                        if (ScreenMapRealParametersValue.RectLayerCollection[i].ElementSelectedState != SelectedState.None)
                        {
                            selectedIndex = ScreenMapRealParametersValue.RectLayerCollection[i].ConnectedIndex;
                            break;
                        }
                    }
                    #region 显示屏映射(测试界面数据)
                    ScreenLocationRectLayer.ElementCollection.Clear();

                    RectLayer screenLocationRectLayer = new RectLayer(0, 0, SmartLCTViewModeBase.DviViewBoxWidth, SmartLCTViewModeBase.DviViewBoxHeight, null, 0, ElementType.baselayer, 0);
                    screenLocationRectLayer.OperateEnviron = OperatEnvironment.AdjustScreenLocation;
                    //显示屏移动区域的Layer（没有DVI则由发送卡最大带载决定）
                    Size screenLayerSize = MyScreen.SenderConnectInfoList[senderIndex].DviSize;
                    RectLayer screenBaseRectLayer = new RectLayer(0, 0, screenLayerSize.Width, screenLayerSize.Height, screenLocationRectLayer, 0, ElementType.screen, 0);
                    screenBaseRectLayer.OperateEnviron = OperatEnvironment.AdjustScreenLocation;

                    //由每个屏的发送卡带载生成各个显示屏带载
                    for (int i = 0; i < MyScreen.ElementCollection.Count; i++)
                    {
                        if (MyScreen.ElementCollection[i].ConnectedIndex < 0)
                        {
                            continue;
                        }
                        RectLayer screenRectLayer = ((RectLayer)(((RectLayer)MyScreen.ElementCollection[i]).ElementCollection[0]));
                        Rect screenRect = screenRectLayer.SenderConnectInfoList[senderIndex].LoadSize;
                        Point screenMapLocation = new Point();
                        for (int n = 0; n < screenRectLayer.SenderConnectInfoList[senderIndex].PortConnectInfoList.Count; n++)
                        {
                            if (screenRectLayer.SenderConnectInfoList[senderIndex].PortConnectInfoList[n].LoadSize.Width != 0 &&
                                screenRectLayer.SenderConnectInfoList[senderIndex].PortConnectInfoList[n].LoadSize.Height != 0)
                            {
                                screenMapLocation = screenRectLayer.SenderConnectInfoList[senderIndex].PortConnectInfoList[n].MapLocation;
                                break;
                            }
                        }
                        if (screenRect.Width == 0 || screenRect.Height == 0)
                        {
                            continue;
                        }
                        RectElement screen = new RectElement(screenMapLocation.X, screenMapLocation.Y, screenRect.Width, screenRect.Height, screenBaseRectLayer, i);
                        screen.OperateEnviron = OperatEnvironment.AdjustScreenLocation;
                        screen.ConnectedIndex = MyScreen.ElementCollection[i].ConnectedIndex;
                        screen.Opacity = 0.5;

                        string msg = "";
                        CommonStaticMethod.GetLanguageString("显示屏", "Lang_Global_Screen", out msg);

                        screen.DisplayName = msg + (screen.ConnectedIndex + 1);
                        screenBaseRectLayer.ElementCollection.Add(screen);
                    }

                    screenLocationRectLayer.ElementCollection.Add(screenBaseRectLayer);
                    screenLocationRectLayer.SenderIndex = senderIndex;
                    int increaseOrDecreaseIndex = GetIncreaseOrDecreaseIndex(senderIndex, ((RectLayer)screenLocationRectLayer.ElementCollection[0]));
                    ((RectLayer)screenLocationRectLayer.ElementCollection[0]).IncreaseOrDecreaseIndex = increaseOrDecreaseIndex;
                    Point newLocation=GetDviCenterPoint(increaseOrDecreaseIndex, ((RectLayer)screenLocationRectLayer.ElementCollection[0]));
                    ((RectLayer)screenLocationRectLayer.ElementCollection[0]).X = newLocation.X;
                    ((RectLayer)screenLocationRectLayer.ElementCollection[0]).Y = newLocation.Y;
                    ScreenLocationRectLayer = screenLocationRectLayer;
                    MyScreen.SenderConnectInfoList[senderIndex].IncreaseOrDecreaseIndex = increaseOrDecreaseIndex;
                    MyScreen.SenderConnectInfoList[senderIndex].IsStatIncreaseOrDecreaseIndex = true;

                    ScreenMapLocationVisible = Visibility.Visible;

                    ScreenMapRealParameters p = new ScreenMapRealParameters();
                    p.SenderLoadRectLayer = screenBaseRectLayer;
                    p.RectLayerCollection = screenBaseRectLayer.ElementCollection;
                    p.RectLayerType = ElementType.screen;


                    if (screenBaseRectLayer.ElementCollection.Count != 0)
                    {
                        if (selectedIndex != -1)
                        {
                            int realIndex = -1;
                            for (int i = 0; i < screenBaseRectLayer.ElementCollection.Count; i++)
                            {
                                if (screenBaseRectLayer.ElementCollection[i].ConnectedIndex == selectedIndex)
                                {
                                    realIndex = i;
                                    break;
                                }
                            }
                            if (realIndex != -1)
                            {
                                SenderRealParameters senderRealPara = new SenderRealParameters();
                                senderRealPara.Element = (IRectElement)screenBaseRectLayer.ElementCollection[realIndex];
                                senderRealPara.Element.ElementSelectedState = SelectedState.Selected;
                                senderRealPara.EleType = ElementType.screen;
                                SenderRealParametersValue = senderRealPara;
                                SenderVisible = Visibility.Visible;
                            }
                            else
                            {
                                SenderVisible = Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            SenderVisible = Visibility.Collapsed;
                        }

                    }
                    else
                    {
                        SenderVisible = Visibility.Collapsed;
                    }
                    ScreenMapRealParametersValue = p;
                    PleaseSelElmentVisible = Visibility.Hidden;
                    #endregion

                }
                #endregion
                #region 多发送卡
                else if (senderCount > 1)
                {
                    #region 选中的网口
                    IRectElement selectedPort = new RectElement();
                    if (_currentScreen != null)
                    {
                        selectedPort.SenderIndex = _currentScreen.SenderIndex;
                        selectedPort.PortIndex = _currentScreen.PortIndex;
                        selectedPort.OperateEnviron = OperatEnvironment.AdjustSenderLocation;
                        selectedPort.ConnectedIndex = _currentScreen.ConnectedIndex;
                    }
                    if (selectedPort.SenderIndex >= 0 && selectedPort.PortIndex < 0)
                    {
                        RectLayer screenLayer = (RectLayer)((RectLayer)MyScreen.ElementCollection[selectedPort.ConnectedIndex]).ElementCollection[0];

                        ObservableCollection<PortConnectInfo> portConnectInfoList = screenLayer.SenderConnectInfoList[selectedPort.SenderIndex].PortConnectInfoList;
                        for (int m = 0; m < portConnectInfoList.Count; m++)
                        {
                            if (portConnectInfoList[m].LoadSize.Height == 0 && portConnectInfoList[m].LoadSize.Width == 0)
                            {
                                continue;
                            }
                            selectedPort.PortIndex = m;
                            break;
                        }
                    }
                    else if (selectedPort.SenderIndex < 0 && selectedPort.PortIndex < 0)
                    {
                        for (int i = 0; i < MyScreen.ElementCollection.Count; i++)
                        {
                            if (selectedPort.SenderIndex >= 0 && selectedPort.PortIndex >= 0)
                            {
                                break;
                            }
                            if (MyScreen.ElementCollection[i].EleType == ElementType.newLayer)
                            {
                                continue;
                            }
                            RectLayer screenLayer = (RectLayer)((RectLayer)MyScreen.ElementCollection[i]).ElementCollection[0];
                            for (int j = 0; j < screenLayer.SenderConnectInfoList.Count; j++)
                            {
                                if (selectedPort.SenderIndex >= 0 && selectedPort.PortIndex >= 0)
                                {
                                    break;
                                }
                                if (screenLayer.SenderConnectInfoList[j].LoadSize.Height == 0 && screenLayer.SenderConnectInfoList[j].LoadSize.Width == 0)
                                {
                                    continue;
                                }
                                ObservableCollection<PortConnectInfo> portConnectInfoList = screenLayer.SenderConnectInfoList[j].PortConnectInfoList;
                                for (int m = 0; m < portConnectInfoList.Count; m++)
                                {
                                    if (portConnectInfoList[m].LoadSize.Height == 0 && portConnectInfoList[m].LoadSize.Width == 0)
                                    {
                                        continue;
                                    }
                                    selectedPort = new RectElement();
                                    selectedPort.ConnectedIndex = i;
                                    selectedPort.SenderIndex = j;
                                    selectedPort.PortIndex = m;
                                    break;
                                }
                            }
                        }
                    }
                    #endregion

                    ScreenMapLocationVisible = Visibility.Visible;
                    ScreenVisible = Visibility.Collapsed;
                    ScanVisible = Visibility.Collapsed;
                    CustomScanVisible = Visibility.Collapsed;
                    PleaseSelElmentVisible = Visibility.Hidden;
                    #region 全部发送卡的带载(用于缩略图的大小)
                    Rect senderLoadSizeSum = new Rect();
                    for (int i = 0; i < MyScreen.SenderConnectInfoList.Count; i++)
                    {
                        if (MyScreen.SenderConnectInfoList[i].LoadSize.Height == 0 && MyScreen.SenderConnectInfoList[i].LoadSize.Width == 0)
                        {
                            continue;
                        }
                        if (senderLoadSizeSum == new Rect())
                        {
                            senderLoadSizeSum = MyScreen.SenderConnectInfoList[i].LoadSize;
                        }
                        else
                        {
                            senderLoadSizeSum = Rect.Union(senderLoadSizeSum, MyScreen.SenderConnectInfoList[i].LoadSize);
                        }
                    }
                    #endregion

                    #region 缩略图数据
                    RectLayer senderMapRectLayer = new RectLayer(0, 0, senderLoadSizeSum.Width, senderLoadSizeSum.Height, null, 0, ElementType.baselayer, 0);
                    senderMapRectLayer.OperateEnviron = OperatEnvironment.AdjustSenderLocation;

                    RectLayer screenBaseRectLayer = new RectLayer(0, 0, senderLoadSizeSum.Width, senderLoadSizeSum.Height, senderMapRectLayer, 0, ElementType.screen, 0);
                    screenBaseRectLayer.OperateEnviron = OperatEnvironment.AdjustSenderLocation;
                    //每个发送卡在所有显示屏中的网口带载 生成显示屏（缩略图中显示所有发送卡的网口）
                    for (int m = 0; m < MyScreen.SenderConnectInfoList.Count; m++)
                    {
                        //发送卡m在所有显示屏中的网口带载
                        for (int i = 0; i < MyScreen.ElementCollection.Count; i++)
                        {
                            if (MyScreen.ElementCollection[i].EleType == ElementType.newLayer)
                            {
                                continue;
                            }
                            RectLayer screenLayer = (RectLayer)((RectLayer)MyScreen.ElementCollection[i]).ElementCollection[0];
                            if (screenLayer.SenderConnectInfoList[m].LoadSize.Width == 0 && screenLayer.SenderConnectInfoList[m].LoadSize.Height == 0)
                            {
                                continue;
                            }
                            Rect senderLoadSize = screenLayer.SenderConnectInfoList[m].LoadSize;
                            ObservableCollection<PortConnectInfo> portConnectInfoList = screenLayer.SenderConnectInfoList[m].PortConnectInfoList;
                            ObservableCollection<IRectElement> portScreenCollection = new ObservableCollection<IRectElement>();
                            for (int n = 0; n < portConnectInfoList.Count; n++)
                            {
                                Rect portRect = portConnectInfoList[n].LoadSize;
                                if (portRect.Width == 0 && portRect.Height == 0)
                                {
                                    continue;
                                }
                                Point portMapLocation = portConnectInfoList[n].MapLocation;
                                RectElement portScreen = new RectElement(portRect.X - senderLoadSize.X + portMapLocation.X, portRect.Y - senderLoadSize.Y + portMapLocation.Y, portRect.Width, portRect.Height, screenBaseRectLayer, screenBaseRectLayer.MaxZorder + 1);
                                screenBaseRectLayer.MaxZorder += 1;
                                portScreen.OperateEnviron = OperatEnvironment.AdjustSenderLocation;
                                portScreen.ConnectedIndex = MyScreen.ElementCollection[i].ConnectedIndex;
                                portScreen.SenderIndex = m;
                                portScreen.PortIndex = portConnectInfoList[n].PortIndex;
                                portScreen.DisplayName = GetMultiSenderDisplayString(portScreen.ConnectedIndex + 1, portScreen.SenderIndex + 1, portScreen.PortIndex + 1);
                                screenBaseRectLayer.ElementCollection.Add(portScreen);
                                portScreenCollection.Add(portScreen);
                            }
                            //一个显示屏使用了一个发送卡的多个网口，则生成的带载显示屏成为一个组（因为一个屏里一个发送卡的网口之间的相对位置是有效的）
                            if (portScreenCollection.Count > 1)
                            {
                                Rect groupRect = Function.UnionRectCollection(portScreenCollection);
                                RectElement groupElement = new RectElement(groupRect.X, groupRect.Y, groupRect.Width, groupRect.Height, screenBaseRectLayer, screenBaseRectLayer.MaxZorder + 1);
                                screenBaseRectLayer.MaxZorder += 1;
                                groupElement.EleType = ElementType.groupframe;

                                groupElement.GroupName = screenBaseRectLayer.MaxGroupName + 1;
                                screenBaseRectLayer.MaxGroupName += 1;
                                groupElement.OperateEnviron = OperatEnvironment.DesignScreen;
                                groupElement.ConnectedIndex = MyScreen.ElementCollection[i].ConnectedIndex;
                                groupElement.SenderIndex = m;
                                screenBaseRectLayer.ElementCollection.Add(groupElement);
                                for (int j = 0; j < portScreenCollection.Count; j++)
                                {
                                    portScreenCollection[j].GroupName = groupElement.GroupName;
                                }
                            }

                        }
                    }

                    senderMapRectLayer.ElementCollection.Add(screenBaseRectLayer);
                    SenderVisible = Visibility.Visible;
                    #endregion

                    #region 选中网口
                    RectElement currentSelectedScreen = new RectElement();
                    Size sender1LoadSize = MyScreen.SenderConnectInfoList[selectedPort.SenderIndex].DviSize;
                    ScreenLocationRectLayer.ElementCollection.Clear();
                    RectLayer screenLocationRectLayer = new RectLayer(0, 0, SmartLCTViewModeBase.DviViewBoxWidth, SmartLCTViewModeBase.DviViewBoxHeight, null, 0, ElementType.baselayer, -1);
                    screenLocationRectLayer.OperateEnviron = OperatEnvironment.AdjustSenderLocation;
                    RectLayer screenBaseloaRectLayer = new RectLayer(0, 0, sender1LoadSize.Width, sender1LoadSize.Height, screenLocationRectLayer, 0, ElementType.screen, -1);
                    screenBaseloaRectLayer.OperateEnviron = OperatEnvironment.AdjustSenderLocation;
                    ObservableCollection<IRectElement> portCollection = new ObservableCollection<IRectElement>();
                    for (int j = 0; j < screenBaseRectLayer.ElementCollection.Count; j++)
                    {
                        IRectElement currentPortScreen = (IRectElement)screenBaseRectLayer.ElementCollection[j];
                        if (currentPortScreen.PortIndex < 0)
                        {
                            continue;
                        }
                        if (currentPortScreen.SenderIndex == selectedPort.SenderIndex && currentPortScreen.PortIndex == selectedPort.PortIndex)
                        {
                            RectLayer currentScreen = (RectLayer)((RectLayer)MyScreen.ElementCollection[currentPortScreen.ConnectedIndex]).ElementCollection[0];
                            RectElement e = (RectElement)((RectElement)screenBaseRectLayer.ElementCollection[j]).Clone();
                            e.ParentElement = screenBaseloaRectLayer;
                            screenBaseRectLayer.ElementCollection[j].ElementSelectedState = SelectedState.Selected;
                            e.ElementSelectedState = SelectedState.Selected;
                            e.EleType = ElementType.receive;
                            e.DisplayName = GetMultiSenderDisplayString(e.ConnectedIndex + 1, e.SenderIndex + 1, e.PortIndex + 1);
                            screenBaseloaRectLayer.ElementCollection.Add(e);
                            portCollection.Add(e);
                            SenderRealParameters senderRealPara = new SenderRealParameters();
                            senderRealPara.Element = e;
                            senderRealPara.EleType = ElementType.port;
                            SenderRealParametersValue = senderRealPara; //网口基本信息
                            currentSelectedScreen = e;
                        }
                        else if (currentPortScreen.SenderIndex == selectedPort.SenderIndex)
                        {
                            RectLayer currentScreen = (RectLayer)((RectLayer)MyScreen.ElementCollection[currentPortScreen.ConnectedIndex]).ElementCollection[0];
                            RectElement e = (RectElement)((RectElement)screenBaseRectLayer.ElementCollection[j]).Clone();
                            e.ParentElement = screenBaseloaRectLayer;
                            e.EleType = ElementType.receive;
                            e.DisplayName = GetMultiSenderDisplayString(e.ConnectedIndex + 1, e.SenderIndex + 1, e.PortIndex + 1);
                            screenBaseloaRectLayer.ElementCollection.Add(e);

                        }
                    }
                    for (int m = 0; m < screenBaseRectLayer.ElementCollection.Count; m++)
                    {
                        if (screenBaseRectLayer.ElementCollection[m].EleType == ElementType.groupframe)
                        {
                            ObservableCollection<IRectElement> portList = new ObservableCollection<IRectElement>();
                            for (int j = 0; j < screenBaseloaRectLayer.ElementCollection.Count; j++)
                            {
                                //同一个屏下面的同一张发送卡下的网口用组框框起来
                                if (((IRectElement)screenBaseloaRectLayer.ElementCollection[j]).ConnectedIndex == screenBaseRectLayer.ElementCollection[m].ConnectedIndex &&
                                    ((IRectElement)screenBaseloaRectLayer.ElementCollection[j]).SenderIndex == ((IRectElement)screenBaseRectLayer.ElementCollection[m]).SenderIndex)
                                {
                                    portList.Add((IRectElement)screenBaseloaRectLayer.ElementCollection[j]);
                                }
                            }
                            if (portList.Count < 2)
                            {
                                continue;
                            }

                            Rect groupRect = Function.UnionRectCollection(portList);
                            RectElement groupElement = (RectElement)((RectElement)screenBaseRectLayer.ElementCollection[m]).Clone();
                            groupElement.X = groupRect.X;
                            groupElement.Y = groupRect.Y;
                            groupElement.Width = groupRect.Width;
                            groupElement.Height = groupRect.Height;
                            groupElement.ParentElement = screenBaseloaRectLayer;
                            screenBaseloaRectLayer.ElementCollection.Add(groupElement);
                        }
                    }
                    screenLocationRectLayer.ElementCollection.Add(screenBaseloaRectLayer);
                    screenLocationRectLayer.SenderIndex = selectedPort.SenderIndex;
                    int increaseOrDecreaseIndex = GetIncreaseOrDecreaseIndex(selectedPort.SenderIndex, ((RectLayer)screenLocationRectLayer.ElementCollection[0]));
                    ((RectLayer)screenLocationRectLayer.ElementCollection[0]).IncreaseOrDecreaseIndex = increaseOrDecreaseIndex;
                    Point newLocation = GetDviCenterPoint(increaseOrDecreaseIndex, ((RectLayer)screenLocationRectLayer.ElementCollection[0]));
                    ((RectLayer)screenLocationRectLayer.ElementCollection[0]).X = newLocation.X;
                    ((RectLayer)screenLocationRectLayer.ElementCollection[0]).Y = newLocation.Y; 
                    ScreenLocationRectLayer = screenLocationRectLayer;
                    MyScreen.SenderConnectInfoList[selectedPort.SenderIndex].IncreaseOrDecreaseIndex = increaseOrDecreaseIndex;
                    MyScreen.SenderConnectInfoList[selectedPort.SenderIndex].IsStatIncreaseOrDecreaseIndex = true;

                    ScreenMapRealParameters p = new ScreenMapRealParameters();
                    p.SenderLoadRectLayer = screenBaseloaRectLayer;
                    p.RectLayerCollection = screenBaseRectLayer.ElementCollection;
                    p.RectLayerType = ElementType.sender;
                    ScreenMapRealParametersValue = p;
                    CurrentScreen = currentSelectedScreen;
                    #endregion
                    PleaseSelElmentVisible = Visibility.Hidden;

                }
                //开始设置映射位置
                for (int i = 0; i < MyScreen.ElementCollection.Count; i++)
                {
                    if (MyScreen.ElementCollection[i].EleType == ElementType.newLayer)
                    {
                        continue;
                    }
                    RectLayer screenlayer = (RectLayer)((RectLayer)MyScreen.ElementCollection[i]).ElementCollection[0];
                }
                #endregion
                Function.UpdateSenderConnectInfo(MyScreen.SenderConnectInfoList, MyScreen);

            }
        }
        private void OnCreateScreenWithConfigurationData(ConfigurationData data)
        {

            string projectPath = data.ProjectLocationPath + "\\" + data.ProjectName + SmartLCTViewModeBase.ProjectExtension;

            if (data.OperateType == OperateScreenType.CreateScreenAndOpenConfigFile)
            {
                OpenConfigFileAndHandleData(projectPath);
                return;
            }
            SelectedLayerAndElement selectedLayerAndElement = new SelectedLayerAndElement();
            ObservableCollection<IRectElement> selectedElementCollection = new ObservableCollection<IRectElement>();
            Dictionary<int, IRectElement> groupframeList = new Dictionary<int, IRectElement>();
            RectLayer screen = (RectLayer)SelectedValue.ElementCollection[0];
            if (data.IsCreateEmptyProject)
            {
                    // 创建空工程   
                    return;
            }

            CurrentSenderConfigInfo = data.SelectedSenderConfigInfo;
            //更新发送卡连线数据
            MyScreen.SenderConnectInfoList.Clear();
            ObservableCollection<PortConnectInfo> portConnectList = new ObservableCollection<PortConnectInfo>();
            for (int i = 0; i < CurrentSenderConfigInfo.PortCount; i++)
            {
                portConnectList.Add(new PortConnectInfo(i, 0, -1, null, null, new Rect()));
            }
            MyScreen.SenderConnectInfoList.Add(new SenderConnectInfo(0, portConnectList, new Rect()));

            #region 添加接收卡
            ObservableCollection<IElement> addCollection = new ObservableCollection<IElement>();
            Point pointInCanvas = new Point(0, 0);
            int cols = data.Cols;
            int rows = data.Rows;

            ScannerCofigInfo scanConfig = data.SelectedScannerConfigInfo;
            ScanBoardProperty scanBdProp = scanConfig.ScanBdProp;
            int width = scanBdProp.Width;
            int height = scanBdProp.Height;
            //每个网口行的整数倍和列的整数倍
            double rowIndex = -1;
            double colIndex = -1;
            //需要多少网口可以带载完 
            Size portLoadPoint = Function.CalculatePortLoadSize(60, 24);
            double portLoadSize = portLoadPoint.Height * portLoadPoint.Width;
            //1、需要几个网口
            double portCount = Math.Ceiling(rows * cols / (portLoadSize / (height * width)));

            //2、计算每个网口的行列数（水平串线则是列的整数倍，垂直串线则是行的整数倍）
            while (true)
            {
                if (data.SelectedArrangeType == ArrangeType.LeftBottom_Hor
                    || data.SelectedArrangeType == ArrangeType.LeftTop_Hor
                    || data.SelectedArrangeType == ArrangeType.RightBottom_Hor
                    || data.SelectedArrangeType == ArrangeType.RightTop_Hor)//水平串线
                {
                    rowIndex = Math.Ceiling(rows * cols / portCount / cols);
                    if (rowIndex * cols * height * width > portLoadSize)
                    {
                        portCount += 1;
                        if (portCount > data.SelectedSenderConfigInfo.PortCount)
                        {
                            string msg = "";
                            CommonStaticMethod.GetLanguageString("超出带载，请重新设置!", "Lang_ScanBoardConfigManager_OverLoad", out msg);
                            ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);
                            return;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    colIndex = Math.Ceiling(rows * cols / portCount / rows);
                    if (colIndex * rows * height * width > portLoadSize)
                    {
                        portCount += 1;
                        if (portCount > data.SelectedSenderConfigInfo.PortCount)
                        {
                            string msg = "";
                            CommonStaticMethod.GetLanguageString("超出带载，请重新设置!", "Lang_ScanBoardConfigManager_OverLoad", out msg);
                            ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);
                            return;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            RectElement groupframe;
            if (cols == 1 && rows == 1)
            {
                groupframe = new RectElement(0, 0, width * cols, height * rows, null, -1);
                RectElement rectElement = new RectElement(0, 0, width * cols, height * rows, screen, screen.MaxZorder + 1);
                rectElement.EleType = ElementType.receive;
                screen.MaxZorder += 1;
                rectElement.Tag = scanConfig.Clone();
                rectElement.IsLocked = true;
                rectElement.ZIndex = 4;
                rectElement.GroupName = -1;
                rectElement.ElementSelectedState = SelectedState.None;
                rectElement.SenderIndex = 0;
                rectElement.PortIndex = 0;
                screen.ElementCollection.Add(rectElement);
                selectedElementCollection.Add(rectElement);
                addCollection.Add(rectElement);

            }
            else
            {
                for (int i = 0; i < cols; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        RectElement rectElement = new RectElement(width * i, height * j, width, height, screen, screen.MaxZorder + 1);
                        rectElement.EleType = ElementType.receive;
                        screen.MaxZorder += 1;
                        rectElement.Tag = scanConfig.Clone();
                        rectElement.IsLocked = true;
                        rectElement.ZIndex = 4;
                        rectElement.ElementSelectedState = SelectedState.None;
                        rectElement.SenderIndex = 0;
                        if (rowIndex != -1)
                        {
                            rectElement.PortIndex = (int)Math.Ceiling((j + 1) / rowIndex) - 1;
                        }
                        else if (colIndex != -1)
                        {
                            rectElement.PortIndex = (int)Math.Ceiling((i + 1) / colIndex) - 1;
                        }
                        screen.ElementCollection.Add(rectElement);
                        addCollection.Add(rectElement);
                        selectedElementCollection.Add(rectElement);

                    }
                }

            }


            ////记录添加
            //AddAction addAction = new AddAction(SelectedScreenLayer, addCollection);
            //SmartLCTActionManager.RecordAction(addAction);
            Dictionary<int, ObservableCollection<IRectElement>> eachPortElement = new Dictionary<int, ObservableCollection<IRectElement>>();
            for (int j = 0; j < portCount; j++)
            {
                eachPortElement.Add(j, new ObservableCollection<IRectElement>());
            }
            for (int i = 0; i < selectedElementCollection.Count; i++)
            {
                eachPortElement[selectedElementCollection[i].PortIndex].Add(selectedElementCollection[i]);
            }
            foreach (int key in eachPortElement.Keys)
            {
                if (eachPortElement[key].Count == 0)
                {
                    continue;
                }
                ScreenRealParameters screenRealPara = new ScreenRealParameters();
                screen.CurrentPortIndex = key;
                screen.CurrentSenderIndex = 0;
                screenRealPara.ScreenLayer = screen;
                screenRealPara.SelectedElement = eachPortElement[key];
                #region 每个网口添加组框
                Rect groupRect = new Rect();
                for (int i = 0; i < screenRealPara.SelectedElement.Count; i++)
                {
                    IRectElement rectElement = screenRealPara.SelectedElement[i];
                    rectElement.GroupName = screen.MaxGroupName + 1;
                    Rect rect = new Rect(rectElement.X, rectElement.Y, rectElement.Width, rectElement.Height);
                    if (i == 0)
                    {
                        groupRect = rect;
                    }
                    groupRect = Rect.Union(groupRect, rect);
                }
                groupframe = new RectElement(groupRect.X, groupRect.Y, groupRect.Width, groupRect.Height, screen, screen.MaxZorder + 1);
                groupframe.EleType = ElementType.groupframe;
                groupframe.ZIndex = 5;
                groupframe.GroupName = screen.MaxGroupName + 1;
                screen.MaxGroupName += 1;
                screen.MaxZorder += 1;
                groupframe.ElementSelectedState = SelectedState.SelectedAll;
                groupframeList.Add(groupframe.GroupName, groupframe);
                screen.ElementCollection.Add(groupframe);

                #endregion
                ScreenRealParametersValue = screenRealPara;
                SelectedArrangeType = data.SelectedArrangeType;
                IsConnectLine = !IsConnectLine;

            }
            #endregion

        }

        private void OpenConfigFileAndHandleData(string fileName)
        {
            OpenConfigFile(fileName);
            for (int i = 0; i < MyScreen.ElementCollection.Count; i++)
            {
                Function.FindConnectedIndex((RectLayer)MyScreen.ElementCollection[i]);
            }
            //更新SenderConnectInfoList
            for (int j = 0; j < MyScreen.ElementCollection.Count; j++)
            {
                if (MyScreen.ElementCollection[j].EleType == ElementType.newLayer)
                {
                    continue;
                }
                RectLayer screenRectLayer = ((RectLayer)(((RectLayer)MyScreen.ElementCollection[j]).ElementCollection[0]));
                Function.UpdateSenderConnectInfo(screenRectLayer.SenderConnectInfoList, MyScreen);
            }
            MyScreen.ElementCollection = MyScreen.ElementCollection;
            if (MyScreen.ElementCollection != null && MyScreen.ElementCollection.Count != 0)
            {
                SelectedValue = (RectLayer)MyScreen.ElementCollection[0];
            }
            if (SelectedEnvironMentIndex == 1)
                OnSelectedEnvironmentChanged();
        }
        private void OpenConfigFile(string fileName)
        {
            RectLayer myscreen = null;
            if (!LoadFromFile(fileName, out myscreen))
            {
                return;
            }
            //更新保存路径
            string[] splitName = fileName.Split('\\');
            foreach (string name in splitName)
            {
                MyConfigurationData.ProjectName = name;
            }
            MyConfigurationData.ProjectName = MyConfigurationData.ProjectName.Substring(0, MyConfigurationData.ProjectName.Length - SmartLCTViewModeBase.ProjectExtension.Length);
            MyConfigurationData.ProjectLocationPath = fileName.Substring(0, fileName.Length - SmartLCTViewModeBase.ProjectExtension.Length - MyConfigurationData.ProjectName.Length - 1);

            MyScreen.ElementCollection.Clear();
            MyScreen = myscreen;
            CurrentSenderConfigInfo = MyScreen.SelectedSenderConfigInfo;

            for (int i = 0; i < MyScreen.ElementCollection.Count; i++)
            {
                if (myscreen.ElementCollection[i].EleType != ElementType.newLayer)
                {
                    ((RectLayer)((RectLayer)MyScreen.ElementCollection[i]).ElementCollection[0]).IncreaseOrDecreaseIndex = 0;
                }
                SetRectElementConnectIndex((RectLayer)MyScreen.ElementCollection[i]);
            }
            if (MyScreen.ElementCollection != null && MyScreen.ElementCollection.Count != 0)
            {
                SelectedValue = (RectLayer)MyScreen.ElementCollection[0];
            }
            int senderIndex = -1;
            _senderCount = Function.FindSenderCount(MyScreen.SenderConnectInfoList, out senderIndex);

        }
        private bool LoadFromFile(string fileName, out RectLayer myscreen)
        {
            myscreen = null;
            XmlSerializer xmls = null;
            StreamReader sr = null;
            XmlReader xmlReader = null;
            string msg = string.Empty;
            bool res = false;
            try
            {
                xmls = new XmlSerializer(typeof(RectLayer));
                sr = new StreamReader(fileName);
                xmlReader = XmlReader.Create(sr);
                if (!xmls.CanDeserialize(xmlReader))
                {
                    res = false;
                    return res;
                }
                else
                {
                    RectLayer tempScanBdProp = (RectLayer)xmls.Deserialize(xmlReader);
                    myscreen = tempScanBdProp;

                    //for (int m = 0;m<myscreen.ElementCollection.Count; m++)
                    //{
                    //    if (myscreen.ElementCollection[m] != null && ((RectLayer)myscreen.ElementCollection[m]).ElementCollection.Count != 0 
                    //        && ((RectLayer)myscreen.ElementCollection[m]).ElementCollection[0].EleType == ElementType.screen)
                    //    {
                    //        SetSenderConnectInfoList((RectLayer)((RectLayer)myscreen.ElementCollection[m]).ElementCollection[0]);
                    //        UpdateSenderAndPortConnectInfo((RectLayer)((RectLayer)myscreen.ElementCollection[m]).ElementCollection[0]);
                    //    }
                    //}
                    SetElementParentElement(myscreen);
                    ImproveElementCollectionProp(myscreen.ElementCollection);
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                res = false;
                return res;
            }
            finally
            {

                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
            }
        }

        private int GetIncreaseOrDecreaseIndex(int senderIndex,IRectLayer layer)
        {
            int res = 0;
            if (!MyScreen.SenderConnectInfoList[senderIndex].IsStatIncreaseOrDecreaseIndex)
            {
                double maxDviWidth = _mainControlSize.Width - SmartLCTViewModeBase.ScrollWidth;
                double maxDviHeight = _mainControlSize.Height - SmartLCTViewModeBase.ScrollWidth;

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
            }
            else
            {
                res = MyScreen.SenderConnectInfoList[senderIndex].IncreaseOrDecreaseIndex;
            }
            return res;
        }
        private List<RectElement> FindRect(RectLayer layer)
        {
            List<RectElement> receiveList = new List<RectElement>();
            for (int i = 0; i < layer.ElementCollection.Count; i++)
            {
                if (layer.ElementCollection[i] is RectElement)
                {
                    receiveList.Add((RectElement)layer.ElementCollection[i]);
                }
                else if (layer.ElementCollection[i] is RectLayer)
                {
                    List<RectElement> receive = new List<RectElement>();
                    receive = FindRect((RectLayer)layer.ElementCollection[i]);
                    for (int j = 0; j < receive.Count; j++)
                    {
                        receiveList.Add(receive[j]);
                    }
                }

            }
            return receiveList;
        }
        private void ImproveElementCollectionProp(ObservableCollection<IElement> layerCollection)
        {
            //更新发送卡连线信息
            for (int m = 0; m < layerCollection.Count; m++)
            {
                if (layerCollection[m] != null && ((RectLayer)layerCollection[m]).ElementCollection.Count != 0
                    && ((RectLayer)layerCollection[m]).ElementCollection[0].EleType == ElementType.screen)
                {
                    UpdateSenderAndPortConnectInfo((RectLayer)((RectLayer)layerCollection[m]).ElementCollection[0]);
                }
            }
            for (int i = 0; i < layerCollection.Count; i++)
            {
                SetElementFrontAndEndElement((RectLayer)layerCollection[i]);
            }


        }
        private void SetElementParentElement(RectLayer layer)
        {
            for (int i = 0; i < layer.ElementCollection.Count; i++)
            {
                layer.ElementCollection[i].ParentElement = layer;
                if (layer.ElementCollection[i] is LineElement)
                {
                    layer.ElementCollection.RemoveAt(i);
                    i--;
                }
                if (layer.ElementCollection[i] is RectLayer)
                {
                    SetElementParentElement((RectLayer)layer.ElementCollection[i]);
                }
            }
        }
        private void UpdateSenderAndPortConnectInfo(RectLayer layer)
        {
            for (int j = 0; j < layer.SenderConnectInfoList.Count; j++)
            {
                for (int m = 0; m < layer.SenderConnectInfoList[j].PortConnectInfoList.Count; m++)
                {
                    if (layer.SenderConnectInfoList[j].PortConnectInfoList[m].ConnectLineElementList != null)
                        layer.SenderConnectInfoList[j].PortConnectInfoList[m].ConnectLineElementList.Clear();
                    else
                    {
                        layer.SenderConnectInfoList[j].PortConnectInfoList[m].ConnectLineElementList = new ObservableCollection<IRectElement>();
                    }

                    layer.SenderConnectInfoList[j].PortConnectInfoList[m].MaxConnectIndex = -1;
                    layer.SenderConnectInfoList[j].PortConnectInfoList[m].MaxConnectElement = null;

                }
            }
            //更新各个网口的连线元素
            if (layer.ElementCollection.Count == 0)
            {
                return;
            }
            for (int i = 0; i < layer.ElementCollection.Count; i++)
            {
                if (layer.ElementCollection[i].ConnectedIndex != -1
                    && layer.ElementCollection[i].EleType == ElementType.receive
                    )
                {
                    IRectElement rect = (IRectElement)layer.ElementCollection[i];
                    layer.SenderConnectInfoList[rect.SenderIndex].PortConnectInfoList[rect.PortIndex].ConnectLineElementList.Add(rect);
                }
            }
            for (int i = 0; i < layer.SenderConnectInfoList.Count; i++)
            {
                for (int j = 0; j < layer.SenderConnectInfoList[i].PortConnectInfoList.Count; j++)
                {
                    if (layer.SenderConnectInfoList[i].PortConnectInfoList[j].ConnectLineElementList == null
                        || layer.SenderConnectInfoList[i].PortConnectInfoList[j].ConnectLineElementList.Count == 0)
                    {
                        continue;
                    }
                    List<IRectElement> connectList = layer.SenderConnectInfoList[i].PortConnectInfoList[j].ConnectLineElementList.ToList<IRectElement>();
                    connectList.Sort(delegate(IRectElement first, IRectElement second)
                    {
                        return first.ConnectedIndex.CompareTo(second.ConnectedIndex);
                    });
                    layer.SenderConnectInfoList[i].PortConnectInfoList[j].MaxConnectElement = ((RectElement)connectList[connectList.Count - 1]);
                    layer.SenderConnectInfoList[i].PortConnectInfoList[j].MaxConnectIndex = ((RectElement)connectList[connectList.Count - 1]).ConnectedIndex;
                }
            }
        }
        private void SetElementFrontAndEndElement(RectLayer layer)
        {
            List<IElement> elementCollection = layer.ElementCollection.ToList<IElement>();
            elementCollection.Sort(delegate(IElement firse, IElement second)
            {
                return ((IRectElement)firse).ConnectedIndex.CompareTo(((IRectElement)second).ConnectedIndex);
            });
            layer.ElementCollection = new ObservableCollection<IElement>(elementCollection);
            for (int i = 0; i < layer.ElementCollection.Count; i++)
            {
                if (layer.ElementCollection[i] is RectElement)
                {
                    ((RectElement)layer.ElementCollection[i]).FrontLine = null;
                    ((RectElement)layer.ElementCollection[i]).EndLine = null;
                    int connectIndex = layer.ElementCollection[i].ConnectedIndex;
                    layer.ElementCollection[i].ConnectedIndex = -2;
                    layer.ElementCollection[i].ConnectedIndex = connectIndex;
                    if (((IRectElement)layer.ElementCollection[i]).FrontLine != null)
                    {
                        ((IRectElement)layer.ElementCollection[i]).FrontLine.ZIndex = 7;
                    }
                }
                if (layer.ElementCollection[i] is RectLayer)
                {
                    SetElementFrontAndEndElement((RectLayer)layer.ElementCollection[i]);
                }
            }
        }
        public void SetRectElementConnectIndex(RectLayer layer)
        {
            //根据发送卡类型更新发送卡序号和网口序号
            for (int i = 0; i < layer.ElementCollection.Count; i++)
            {
                if (layer.ElementCollection[i] is RectElement)
                {
                    RectElement rect = (RectElement)((RectElement)layer.ElementCollection[i]).Clone();
                    int connectedIndex = rect.ConnectedIndex;
                    layer.ElementCollection[i].ConnectedIndex = connectedIndex;
                }
                else if (layer.ElementCollection[i] is RectLayer)
                {
                    SetRectElementConnectIndex((RectLayer)layer.ElementCollection[i]);
                }
            }
        }

        private string SaveSysConfigFileDialog(string initDirectoryName, string filename)
        {
            SaveFileDialog sfd = new SaveFileDialog();         
            string msg = "";
            CommonStaticMethod.GetLanguageString("系统配置文件", "Lang_SmartLCT_VM_SystemConfigFile", out msg);

            sfd.Filter = msg + "(*.xml)|*.xml";
            sfd.DefaultExt = ".xml";

            string saveAs = "";
            //CommonStaticMethod.GetLanguageString("另存为", "Lang_Global_SaveAs", out saveAs);
            CommonStaticMethod.GetLanguageString("保存", "Lang_Global_Save", out saveAs);
            sfd.Title = saveAs;
            if (filename != "")
            {
                sfd.FileName = filename;
            }
            if (initDirectoryName != "")
            {
                sfd.InitialDirectory = initDirectoryName;
            }
            if (sfd.ShowDialog() != true)
            {
                return "";
            }
            else
            {
                return sfd.FileName;
            }
        }
        private void SaveSysConfigFile(string fileName)
        {
            string msg = "";
            CommonStaticMethod.GetLanguageString("系统配置文件", "Lang_SmartLCT_VM_SystemConfigFile", out msg);
            bool serialRes = SaveToFile(fileName, MyScreen);

            if (!serialRes)
            {
                CommonStaticMethod.GetLanguageString("保存系统配置文件失败", "Lang_SmartLCT_VM_SaveError", out msg);
                ShowGlobalDialogMessage(msg, MessageBoxImage.Information);
            }
            else
            {
                ProjectInfo projectInfo = new ProjectInfo();
                projectInfo.ProjectPath = fileName;
                int lastIndex = fileName.LastIndexOf('\\');
                projectInfo.ProjectName = fileName.Substring(lastIndex + 1, fileName.Length - lastIndex - SmartLCTViewModeBase.ProjectExtension.Length - 1);
                if (fileName != "")
                {
                    for (int i = 0; i < _globalParams.RecentProjectPaths.Count; i++)
                    {
                        if (fileName == _globalParams.RecentProjectPaths[i].ProjectPath)
                        {
                            _globalParams.RecentProjectPaths.RemoveAt(i);
                            i = i - 1;
                        }
                    }
                    _globalParams.RecentProjectPaths.Insert(0, projectInfo);
                    while (_globalParams.RecentProjectPaths.Count > SmartLCTViewModeBase.MaxRecentOpenFileCount)
                    {
                        _globalParams.RecentProjectPaths.RemoveAt(SmartLCTViewModeBase.MaxRecentOpenFileCount);
                    }
                }
            }
        }

        private bool SaveToFile(string fileName, RectLayer myscreen)
        {
            XmlSerializer xmls = null;
            StreamWriter sw = null;
            string msg = string.Empty;
            try
            {
                xmls = new XmlSerializer(typeof(RectLayer));
                sw = new StreamWriter(fileName);
                //XmlWriterSettings setting = new XmlWriterSettings();
                //setting.CloseOutput = true;

                XmlWriter xmlWriter = XmlWriter.Create(sw);
                xmls.Serialize(sw, myscreen);
                sw.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
        }
        private bool SaveRecentProjectFile(ObservableCollection<ProjectInfo> recentProjectPaths)
        {
            string filename = SmartLCTViewModeBase.ReleasePath + "Resource\\Config\\RecentProjectFile\\recentProjectFile.xml";
            XmlSerializer xmls = null;
            StreamWriter sw = null;
            string msg = string.Empty;
            try
            {
                xmls = new XmlSerializer(typeof(ObservableCollection<ProjectInfo>));
                sw = new StreamWriter(filename);
                //XmlWriterSettings setting = new XmlWriterSettings();
                //setting.CloseOutput = true;

                XmlWriter xmlWriter = XmlWriter.Create(sw);
                xmls.Serialize(sw, recentProjectPaths);
                sw.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

        /// <summary>
        /// 获取每个显示屏的接收卡位置
        /// </summary>
        /// <returns></returns>
        private List<ILEDDisplayInfo> GetLedDisplayInfoList()
        {
            List<ILEDDisplayInfo> ledDisplayInfoList = new List<ILEDDisplayInfo>();
            int currentSenderIndex;
            int senderCount = Function.FindSenderCount(MyScreen.SenderConnectInfoList, out currentSenderIndex);

            //获取每个屏里接收卡相对于发送卡的位置坐标
            for (int screenIndex = 0; screenIndex < MyScreen.ElementCollection.Count; screenIndex++)
            {
                if (MyScreen.ElementCollection[screenIndex].EleType == ElementType.newLayer)
                {
                    continue;
                }
                ComplexLEDDisplayInfo ledDisplayInfo = new ComplexLEDDisplayInfo();

                RectLayer screen = (RectLayer)((RectLayer)MyScreen.ElementCollection[screenIndex]).ElementCollection[0];
                ObservableCollection<SenderConnectInfo> senderConnectInfoList = screen.SenderConnectInfoList;
                for (int senderIndex = 0; senderIndex < senderConnectInfoList.Count; senderIndex++)
                {
                    SenderConnectInfo senderConnectInfo = senderConnectInfoList[senderIndex];
                    Rect senderLoadSize = senderConnectInfo.LoadSize;
                    Point mapLocation = new Point();

                    for (int portIndex = 0; portIndex < senderConnectInfo.PortConnectInfoList.Count; portIndex++)
                    {
                        int connectIndexGlobal = 0;
                        for (int sIndex = 0; sIndex < screenIndex; sIndex++)
                        {
                            ObservableCollection<IRectElement> cList = ((RectLayer)((RectLayer)MyScreen.ElementCollection[sIndex]).ElementCollection[0]).SenderConnectInfoList[senderIndex].PortConnectInfoList[portIndex].ConnectLineElementList;
                            if (cList != null && cList.Count != 0)
                            {
                                connectIndexGlobal += cList.Count;
                            }
                        }

                        ObservableCollection<IRectElement> scanBdCollection = senderConnectInfo.PortConnectInfoList[portIndex].ConnectLineElementList;
                        if (scanBdCollection == null || scanBdCollection.Count == 0)
                        {
                            continue;
                        }
                        //connectIndex的值不是连续的，所以排个序才能知道先后
                        List<IRectElement> scanBdList = new List<IRectElement>();
                        for (int scanIndex = 0; scanIndex < scanBdCollection.Count; scanIndex++)
                        {
                            scanBdList.Add(scanBdCollection[scanIndex]);
                        }
                        scanBdList.Sort(delegate(IRectElement first, IRectElement second)
                        {
                            return first.ConnectedIndex.CompareTo(second.ConnectedIndex);
                        });

                        //if (senderCount > 1)
                        //{
                        mapLocation = senderConnectInfo.PortConnectInfoList[portIndex].MapLocation;
                        //}
                        for (int scanBdIndex = 0; scanBdIndex < scanBdList.Count; scanBdIndex++)
                        {
                            IRectElement scanBdElement = scanBdList[scanBdIndex];
                            ScanBoardRegionInfo scanBdInfo = new ScanBoardRegionInfo();
                            scanBdInfo.SenderIndex = (byte)scanBdElement.SenderIndex;
                            scanBdInfo.PortIndex = (byte)(scanBdElement.PortIndex);
                            scanBdInfo.ConnectIndex = (ushort)(scanBdIndex + connectIndexGlobal);
                            scanBdInfo.Width = (ushort)scanBdElement.Width;
                            scanBdInfo.Height = (ushort)scanBdElement.Height;
                            scanBdInfo.X = (ushort)(scanBdElement.X - senderLoadSize.Left + mapLocation.X);
                            scanBdInfo.Y = (ushort)(scanBdElement.Y - senderLoadSize.Top + mapLocation.Y);
                            ledDisplayInfo.ScanBoardRegionInfoList.Add(scanBdInfo);
                        }
                    }
                }
                if (ledDisplayInfo.ScanBoardRegionInfoList.Count != 0)
                {
                    ledDisplayInfoList.Add(ledDisplayInfo);
                }
            }
            return ledDisplayInfoList;

        }

        private string GetMultiSenderDisplayString(int scrIndex, int senderIndex, int portIndex)
        {
            string strScr = "";
            string strSender = "";
            string strPort = "";
            CommonStaticMethod.GetLanguageString("屏", "Lang_Global_ScreenShort", out strScr);
            CommonStaticMethod.GetLanguageString("发送卡", "Lang_Global_SendingBoard", out strSender);
            CommonStaticMethod.GetLanguageString("网口", "Lang_Global_NetPort", out strPort);

            string msg = strScr + scrIndex + strSender + senderIndex + strPort + portIndex;
            return msg;
        }
        private void SetConnectedIndex(ObservableCollection<IElement> _rectLayerCollection)
        {
            for (int i = 0; i < _rectLayerCollection.Count; i++)
            {
                if (_rectLayerCollection[i] is RectLayer)
                {
                    int connectedIndex = _rectLayerCollection[i].ConnectedIndex;
                    _rectLayerCollection[i].ConnectedIndex = connectedIndex;
                    if (_rectLayerCollection[i].EleType != ElementType.port)
                    {
                        SetConnectedIndex(((RectLayer)_rectLayerCollection[i]).ElementCollection);
                    }
                }
            }
        }

        #endregion

        #region 消息处理函数
        private void OnIncreaseOrDecreaseIndexChanged(int index)
        {
            IncreaseOrDecreaseIndex = index;
            if (SelectedEnvironMentIndex == 1)
            {
                int increaseOrDecreaseIndex=((IRectLayer)ScreenLocationRectLayer.ElementCollection[0]).IncreaseOrDecreaseIndex;
                MyScreen.SenderConnectInfoList[ScreenLocationRectLayer.SenderIndex].IncreaseOrDecreaseIndex = increaseOrDecreaseIndex;
                Point newLocation = GetDviCenterPoint(increaseOrDecreaseIndex, (IRectLayer)ScreenLocationRectLayer.ElementCollection[0]);
                ((IRectElement)ScreenLocationRectLayer.ElementCollection[0]).X = newLocation.X;
                ((IRectElement)ScreenLocationRectLayer.ElementCollection[0]).Y = newLocation.Y;
            }
        }
        private Point GetDviCenterPoint(int increaseOrDecreaseIndex, IRectLayer layer)
        {
            Point res = new Point();
            double changedValue = Math.Pow(SmartLCTViewModeBase.IncreaseOrDecreaseValue, increaseOrDecreaseIndex);
            res.X = (int)(_mainControlSize.Width - layer.Width * changedValue) / 2 / changedValue;
            res.Y = (int)(_mainControlSize.Height - layer.Height * changedValue) / 2 / changedValue;
            if (res.X < 0)
            {
                res.X = 0;
            }
            if (res.Y < 0)
            {
                res.Y = 0;
            }
            return res;
        }

        private void OnSelectedLayerAndElementChanged(SelectedLayerAndElement selectedLayerAndElement)
        {
            IsAddingReceive = selectedLayerAndElement.IsAddingReceive;
            if (IsAddingReceive)
            {
                return;
            }
            SelectedScreenLayer = selectedLayerAndElement.SelectedLayer;
            if (SelectedScreenLayer.OperateEnviron == OperatEnvironment.DesignScreen)
            {
                _mainControlSize = selectedLayerAndElement.MainControlSize;
            }
            if (SelectedScreenLayer.OperateEnviron != OperatEnvironment.DesignScreen)
            {
                int senderIndex;
                int senderCount = Function.FindSenderCount(MyScreen.SenderConnectInfoList, out senderIndex);

                if (selectedLayerAndElement.SelectedElement.Count == 1)
                {
                    SenderVisible = Visibility.Visible;
                    if (senderCount == 1)
                    {
                        CurrentScreen = selectedLayerAndElement.SelectedElement[0];
                    }
                    else if (senderCount > 1)
                    {
                        CurrentScreen = selectedLayerAndElement.SelectedElement[0];
                        CurrentSenderScreen = selectedLayerAndElement.SelectedElement[0];
                        if (selectedLayerAndElement.fj == 2)
                        {
                            CurrentScreen = selectedLayerAndElement.CurrentRectElement;
                        }
                    }
                    PleaseSelElmentVisible = Visibility.Hidden;
                }
                else
                {

                    if (_senderCount == 1)
                    {
                        SenderRealParameters senderRealPara = new SenderRealParameters();
                        senderRealPara.Element = null;
                        SenderRealParametersValue = senderRealPara;
                        CurrentScreen = null;
                        SenderVisible = Visibility.Collapsed;
                    }
                    else if (_senderCount > 1)
                    {
                        //多发送的情况下单击选择了一个组
                        for (int i = 0; i < ScreenMapRealParametersValue.RectLayerCollection.Count; i++)
                        {
                            ScreenMapRealParametersValue.RectLayerCollection[i].ElementSelectedState = SelectedState.None;
                        }
                        for (int j = 0; j < selectedLayerAndElement.SelectedElement.Count; j++)
                        {
                            IRectElement selectedELement = selectedLayerAndElement.SelectedElement[j];
                            for (int i = 0; i < ScreenMapRealParametersValue.RectLayerCollection.Count; i++)
                            {
                                IRectElement rectelement = (IRectElement)ScreenMapRealParametersValue.RectLayerCollection[i];
                                if (rectelement.SenderIndex == selectedELement.SenderIndex && rectelement.PortIndex == selectedELement.PortIndex)
                                {
                                    rectelement.ElementSelectedState = SelectedState.Selected;
                                }
                            }
                        }
                        if (selectedLayerAndElement.SelectedElement.Count != 0)
                        {
                            CheckSenderIndex = -1;
                            CheckSenderIndex = selectedLayerAndElement.SelectedElement[0].SenderIndex;
                        }
                        if (selectedLayerAndElement.SelectedElement.Count > 1)
                        {
                            //找到移动的组
                            RectLayer screenLayer = (RectLayer)ScreenLocationRectLayer.ElementCollection[0];
                            for (int i = 0; i < screenLayer.ElementCollection.Count; i++)
                            {
                                if (screenLayer.ElementCollection[i].EleType == ElementType.groupframe &&
                                    screenLayer.ElementCollection[i].GroupName == selectedLayerAndElement.SelectedElement[0].GroupName)
                                {
                                    CurrentScreen = (IRectElement)screenLayer.ElementCollection[i];
                                    for (int j = 0; j < selectedLayerAndElement.SelectedElement.Count; j++)
                                    {
                                        IRectElement rect = selectedLayerAndElement.SelectedElement[j];
                                        rect.ElementSelectedState = SelectedState.Selected;
                                        ((RectLayer)((RectLayer)MyScreen.ElementCollection[rect.ConnectedIndex]).ElementCollection[0]).SenderConnectInfoList[rect.SenderIndex].PortConnectInfoList[rect.PortIndex].MapLocation = new Point(CurrentScreen.X,CurrentScreen.Y);
                                    }
                                    break;
                                }
                            }
                            SenderVisible = Visibility.Collapsed;
                        }
                        else
                        {
                            SenderVisible = Visibility.Visible;
                        }
                    }
                    PleaseSelElmentVisible = Visibility.Hidden;
                }
                return;
            }
            
            if (SelectedScreenLayer != null)
            {
                IncreaseOrDecreaseIndex = SelectedScreenLayer.IncreaseOrDecreaseIndex;
            }
            if (selectedLayerAndElement.SelectedLayer.EleType == ElementType.newLayer)
            {
                SenderVisible = Visibility.Collapsed;
                ScanVisible = Visibility.Collapsed;
                CustomScanVisible = Visibility.Collapsed;
                PleaseSelElmentVisible = Visibility.Hidden;
                ScreenVisible = Visibility.Collapsed;
                return;
            }
            if (selectedLayerAndElement.SelectedLayer == null)
            {
                SenderVisible = Visibility.Collapsed;
                ScanVisible = Visibility.Collapsed;
                CustomScanVisible = Visibility.Collapsed;
                PleaseSelElmentVisible = Visibility.Visible;
                ScreenVisible = Visibility.Collapsed;
                return;
            }
            _selectedLayerAndELment = selectedLayerAndElement;
            if (selectedLayerAndElement.SelectedLayer.EleType == ElementType.None)
            {
                SenderRealParameters rp = new SenderRealParameters();
                rp.Element = new RectLayer();

                SenderRealParametersValue = rp;
                ScreenVisible = Visibility.Collapsed;
                ScanVisible = Visibility.Collapsed;
                CustomScanVisible = Visibility.Collapsed;
                PleaseSelElmentVisible = Visibility.Visible;
            }
            else if (selectedLayerAndElement.SelectedLayer.EleType == ElementType.screen)
            {
                ScreenVisible = Visibility.Visible;
                SenderVisible = Visibility.Collapsed;
                if (selectedLayerAndElement.SelectedElement.Count == 1)
                {
                    ScannerRealParameters scannerParameters = new ScannerRealParameters();
                    scannerParameters.ScannerElement = selectedLayerAndElement.SelectedElement[0];
                    int groupname = selectedLayerAndElement.SelectedElement[0].GroupName;
                    if (groupname != -1)
                    {
                        scannerParameters.Groupframe = selectedLayerAndElement.GroupframeList[groupname];
                        scannerParameters.NoSelectedElementRect = selectedLayerAndElement.SelectedInfoList[groupname].NoSelectedElementRect;
                    }
                    object tag = selectedLayerAndElement.SelectedElement[0].Tag;
                    if (tag is ScannerCofigInfo)
                    {
                        scannerParameters.ScannerConfig = (ScannerCofigInfo)tag;
                        if (scannerParameters.ScannerConfig.ScanBdProp.StandardLedModuleProp.DriverChipType == ChipType.Unknown)
                        {
                            CustomScanVisible = Visibility.Visible;
                            ScanVisible = Visibility.Collapsed;
                        }
                        else
                        {
                            ScanVisible = Visibility.Visible;
                            CustomScanVisible = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        scannerParameters.ScannerConfig = null;
                        ScanVisible = Visibility.Visible;
                        CustomScanVisible = Visibility.Collapsed;
                    }
                    ScannerRealParametersValue = scannerParameters;
                }
                else
                {
                    ScanVisible = Visibility.Collapsed;
                    CustomScanVisible = Visibility.Collapsed;
                }

                ScreenRealParameters screenParameters = new ScreenRealParameters();
                screenParameters.ScreenLayer = selectedLayerAndElement.SelectedLayer;
                screenParameters.SelectedElement = selectedLayerAndElement.SelectedElement;

                ScreenRealParametersValue = screenParameters;

                PleaseSelElmentVisible = Visibility.Hidden;
            }
            else
            {
                SenderVisible = Visibility.Collapsed;
                ScanVisible = Visibility.Collapsed;
                CustomScanVisible = Visibility.Collapsed;
                PleaseSelElmentVisible = Visibility.Visible;
                ScreenVisible = Visibility.Collapsed;
            }
        }

        #endregion

        #region 打开外部程序
        private void StartCalculator()
        {
            if (!File.Exists(CACULATOR_PATH))
            {
                string msg = "在系统目录下找不到计算器！";
                CommonStaticMethod.GetLanguageString(msg, "Lang_ScrCfg_NotFileCacls", out msg);
                ShowGlobalDialogMessage(msg, MessageBoxImage.Error);
                return;
            }
            Process proc = Process.Start(CACULATOR_PATH);
            _myStartProcessList.Add(proc);
        }

        private void StartTestTool()
        {
            if (!File.Exists(PLUG_IN_PATH))
            {
                string msg = "在程序路径下找不到测试工具！";
                CommonStaticMethod.GetLanguageString(msg, "Lang_ScrCfg_NotFileTestTool", out msg);
                ShowGlobalDialogMessage(msg, MessageBoxImage.Error);
                return;
            }
            Process proc = Process.Start(PLUG_IN_PATH, _curLangFlag);
            _myStartProcessList.Add(proc);
        }

        private void TerminateSoftStartProc()
        {
            for (int i = 0; i < _myStartProcessList.Count; i++)
            {
                Process proc = _myStartProcessList[i];
                TerminateProcess(proc.Handle.ToInt32(), 0);
            }
        }
        #endregion

        #region 更换皮肤
        private void OnCmdChangedSkin(SkinType skinType)
        {
            if (skinType == SkinType.Blue)
            {
                Application.Current.Resources.MergedDictionaries.RemoveAt(2);
                Application.Current.Resources.MergedDictionaries.RemoveAt(1);

                ResourceDictionary res = (ResourceDictionary)Application.LoadComponent(new Uri("/Nova.SmartLCT.Skin;Component/BlueMode/BlueMood.UI.xaml", UriKind.RelativeOrAbsolute));
                Application.Current.Resources.MergedDictionaries.Add(res);

                res = (ResourceDictionary)Application.LoadComponent(new Uri("/Nova.SmartLCT.Skin;Component/BlueMode/BlueMood.Color.xaml", UriKind.RelativeOrAbsolute));
                Application.Current.Resources.MergedDictionaries.Add(res);

            }

            else if (skinType == SkinType.Red)
            {
                Application.Current.Resources.MergedDictionaries.RemoveAt(2);
                Application.Current.Resources.MergedDictionaries.RemoveAt(1);

                ResourceDictionary res = (ResourceDictionary)Application.LoadComponent(new Uri("/Nova.SmartLCT.Skin;Component/RedMode/RedMood.UI.xaml", UriKind.RelativeOrAbsolute));
                Application.Current.Resources.MergedDictionaries.Add(res);

                res = (ResourceDictionary)Application.LoadComponent(new Uri("/Nova.SmartLCT.Skin;Component/RedMode/RedMood.Color.xaml", UriKind.RelativeOrAbsolute));
                Application.Current.Resources.MergedDictionaries.Add(res);

            }
        }
        #endregion

        #region 更换语言
        private void OnCmdChangedLang(string langFlag)
        {
            ChangedLanguage(langFlag);
            foreach (LangItemData item in LangItemCollection)
            {
                if (item.LangFlag == langFlag)
                {
                    item.IsSelected = true;
                }
                else
                {
                    item.IsSelected = false;
                }
            }
            _globalParams.AppConfig.LangFlag = langFlag;
            SQLiteAccessor accessor = SQLiteAccessor.Instance;
            accessor.SaveAppConfig(_globalParams.AppConfig);
        }
        #endregion

        #region  重载
        public override void Cleanup()
        {
            TerminateSoftStartProc();
        }
        protected override void OnLangFlagChanged(string langFlag)
        {
            base.OnLangFlagChanged(langFlag);
            foreach (RectLayer layer in _myScreen.ElementCollection)
            {
                int connectedIndex = layer.ConnectedIndex;
                layer.ConnectedIndex = connectedIndex;
                SetConnectedIndex(layer.ElementCollection);
            }
        }
        #endregion

    }
}
