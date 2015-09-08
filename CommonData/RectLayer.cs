using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows;
using Nova.SmartLCT.Interface;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Collections.Specialized;
using GalaSoft.MvvmLight.Messaging;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace Nova.SmartLCT.Interface
{
    [Serializable]
    public class RectLayer : IRectLayer
    {
        #region 属性
       
        public bool IsStartSetMapLocation
        {
            get { return _isStartSetMapLocation; }
            set
            {
                _isStartSetMapLocation = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsStartSetMapLocation));
            }
        }
        private bool _isStartSetMapLocation = false;
        public SenderConfigInfo SelectedSenderConfigInfo
        {
            get { return _selectedSenderConfigInfo; }
            set
            {
                _selectedSenderConfigInfo = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SelectedSenderConfigInfo));
            }
        }
        private SenderConfigInfo _selectedSenderConfigInfo = new SenderConfigInfo();

        public override OperatEnvironment OperateEnviron
        {
            get
            {
                return base.OperateEnviron;
            }
            set
            {
                base.OperateEnviron = value;
                NotifyPropertyChanged(GetPropertyName(o => this.OperateEnviron));
            }
        }
        public override ElementType EleType
        {
            get
            {
                return base.EleType;
            }
            set
            {
                base.EleType = value;
                NotifyPropertyChanged(GetPropertyName(o => this.EleType));
            }
        }
        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                _isExpanded = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsExpanded));
            }
        }
        private bool _isExpanded = false;
        public Visibility SizeVisibility
        {
            get
            {
                return _sizeVisibility;
            }
            set
            {
                if (value == _sizeVisibility)
                {
                    return;
                }
                _sizeVisibility = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SizeVisibility));
            }
        }
        private Visibility _sizeVisibility = Visibility.Hidden;
        #region IRectLayer 成员
        public override ObservableCollection<IElement> ElementCollection
        {
            get
            {
                return _elementCollection;
            }
            set
            {
                if (_elementCollection != null)
                {
                    _elementCollection.CollectionChanged -= new NotifyCollectionChangedEventHandler(OnElementCollectionChange);

                }
                _elementCollection = value;
                if (_elementCollection != null)
                {
                    _elementCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(OnElementCollectionChange);
                    
                }

                RectLayer myScreenLayer = new RectLayer();
                if (EleType == ElementType.baselayer && this.ElementCollection!=null && this.ElementCollection.Count!=0)
                {
                    myScreenLayer =(RectLayer) ElementCollection[0];
                }
                if (EleType == ElementType.screen)
                {
                    myScreenLayer = this;
                }
                NotifyPropertyChanged(GetPropertyName(o => this.ElementCollection));
            }
        }
        
        private void OnElementCollectionChange(object sender, NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                #region 添加
                IElement newElement = (IElement)e.NewItems[0];
                if (EleType == ElementType.baseScreen && newElement.EleType == ElementType.baselayer)
                {
                    RectLayer newLayer = (RectLayer)((RectLayer)newElement).ElementCollection[0];
                    ObservableCollection<SenderConnectInfo> senderInfoList = new ObservableCollection<SenderConnectInfo>();
                    for (int i = 0; i < SenderConnectInfoList.Count; i++)
                    {
                        ObservableCollection<PortConnectInfo> portConnectList = SenderConnectInfoList[i].PortConnectInfoList;
                        ObservableCollection<PortConnectInfo> newportConnectList = new ObservableCollection<PortConnectInfo>();
                        for (int j = 0; j < portConnectList.Count; j++)
                        {
                            newportConnectList.Add(new PortConnectInfo(portConnectList[j].PortIndex, SenderConnectInfoList[i].SenderIndex ,- 1, null, null, new Rect()));
                        }
                        SenderConnectInfo newsenderConnectInfo = new SenderConnectInfo(SenderConnectInfoList[i].SenderIndex, newportConnectList, new Rect());
                        senderInfoList.Add(newsenderConnectInfo);
                    }
                    newLayer.SenderConnectInfoList = senderInfoList;
                }
                #endregion
                if (EleType == ElementType.screen)
                {
                    #region 更新带载区域信息
                    Function.UpdateSenderConnectInfo(SenderConnectInfoList, this);
                    #endregion
                }  
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                #region 移除

                #endregion
                NotifyPropertyChanged(GetPropertyName(o => this.ElementCollection));
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                #region 修改
                #endregion
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                #region 重置

                #endregion
            }
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {

            }

        }
      
        public override  ObservableCollection<SenderConnectInfo> SenderConnectInfoList
        {
            get
            {
                return base.SenderConnectInfoList;
            }
            set
            {
                
                if (_senderConnectInfoList != null)
                {
                    _senderConnectInfoList.CollectionChanged -= new NotifyCollectionChangedEventHandler(OnSenderConnectInfoCollectionChange);

                }
                base.SenderConnectInfoList = value;
                if (_senderConnectInfoList != null)
                {
                    _senderConnectInfoList.CollectionChanged += new NotifyCollectionChangedEventHandler(OnSenderConnectInfoCollectionChange);

                }
            }
        }

        private void UpdateSenderConnectInfo(RectLayer layer,SenderConnectInfo senderConnectInfo)
        {
            senderConnectInfo.IsExpand = false;
            senderConnectInfo.IsSelected = false;
            for (int m = 0; m < senderConnectInfo.PortConnectInfoList.Count; m++)
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

                senderConnectInfo.PortConnectInfoList[m].IsSelected = false;
            }
        }

        private void OnSenderConnectInfoCollectionChange(object sender, NotifyCollectionChangedEventArgs e)
        {
           
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                #region 添加
                IElement layer = this;
                while (layer != null && layer.EleType != ElementType.baseScreen)
                {
                    layer = layer.ParentElement;
                }
                if (layer == null)
                {
                    return;
                }
                ObservableCollection<SenderConnectInfo> newdata = new ObservableCollection<SenderConnectInfo>();
                int portCount = 0;

                for (int i = 0; i < e.NewItems.Count; i++)
                {

                    ObservableCollection<PortConnectInfo> portList = new ObservableCollection<PortConnectInfo>();
                    SenderConnectInfo info = e.NewItems[i] as SenderConnectInfo;
                    portCount = info.PortConnectInfoList.Count;
                    for (int j = 0; j < info.PortConnectInfoList.Count; j++)
                    {
                        portList.Add(new PortConnectInfo(info.PortConnectInfoList[j].PortIndex, info.SenderIndex, - 1, null, null, new Rect()));
                    }
                    SenderConnectInfo newInfo = new SenderConnectInfo(info.SenderIndex, portList, new Rect());

                    newdata.Add(newInfo);
                }
                RectLayer myScreen = (RectLayer)layer;
                if (EleType == ElementType.screen)
                {
                    for (int i = 0; i < newdata.Count; i++)
                    {
                        bool ishave = false;
                        for (int n = 0; n < myScreen.SenderConnectInfoList.Count; n++)
                        {
                            if (myScreen.SenderConnectInfoList[n].SenderIndex == newdata[i].SenderIndex)
                            {
                                ishave = true;
                                break;
                            }
                        }
                        if (!ishave && myScreen.SenderConnectInfoList.Count != SenderConnectInfoList.Count)
                        {

                            myScreen.SenderConnectInfoList.Add(newdata[i]);
                        }
                    }
                    for (int i = 0; i < myScreen.ElementCollection.Count; i++)
                    {
                        if (myScreen.ElementCollection[i].EleType == ElementType.newLayer)
                        {
                            continue;
                        }
                        for (int j = 0; j < newdata.Count; j++)
                        {

                            if (((RectLayer)((RectLayer)myScreen.ElementCollection[i]).ElementCollection[0]).SenderConnectInfoList.Count == SenderConnectInfoList.Count)
                            {
                                continue;
                            }
                            bool ishave = false;
                            for (int n = 0; n < ((RectLayer)((RectLayer)myScreen.ElementCollection[i]).ElementCollection[0]).SenderConnectInfoList.Count; n++)
                            {
                                if (((RectLayer)((RectLayer)myScreen.ElementCollection[i]).ElementCollection[0]).SenderConnectInfoList[n].SenderIndex == newdata[j].SenderIndex)
                                {
                                    ishave = true;
                                    break;
                                }
                            }
                            if (!ishave)
                                ((RectLayer)((RectLayer)myScreen.ElementCollection[i]).ElementCollection[0]).SenderConnectInfoList.Add(newdata[j]);
                        }
                    }
                }
                else if (EleType == ElementType.baseScreen)
                {
                    for (int i = 0; i < myScreen.ElementCollection.Count; i++)
                    {
                        if (myScreen.ElementCollection[i].EleType == ElementType.newLayer)
                        {
                            continue;
                        }
                        for (int j = 0; j < newdata.Count; j++)
                        {
                            if (((RectLayer)((RectLayer)myScreen.ElementCollection[i]).ElementCollection[0]).SenderConnectInfoList.Count == SenderConnectInfoList.Count)
                            {
                                continue;
                            }
                            bool ishave = false;
                            for (int n = 0; n < ((RectLayer)((RectLayer)myScreen.ElementCollection[i]).ElementCollection[0]).SenderConnectInfoList.Count; n++)
                            {
                                if (((RectLayer)((RectLayer)myScreen.ElementCollection[i]).ElementCollection[0]).SenderConnectInfoList[n].SenderIndex == newdata[j].SenderIndex)
                                {
                                    ishave = true;
                                    break;
                                }
                            }
                            if (!ishave)
                                ((RectLayer)((RectLayer)myScreen.ElementCollection[i]).ElementCollection[0]).SenderConnectInfoList.Add(newdata[j]);
                        }
                    }
                }
                #endregion
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                #region 移除
                IElement removeElement = this;
                if (EleType != ElementType.baseScreen)
                {
                    return;
                }
               
                RectLayer removeLayer=(RectLayer)removeElement;

                    for (int i = 0; i < removeLayer.ElementCollection.Count; i++)
                    {
                        if (removeLayer.ElementCollection[i].EleType == ElementType.newLayer)
                        {
                            continue;
                        }
                        RectLayer reLayer = (RectLayer)((RectLayer)removeLayer.ElementCollection[i]).ElementCollection[0];
                        for (int m = 0; m < reLayer.SenderConnectInfoList.Count; m++)
                        {
                            if (reLayer.SenderConnectInfoList[m].SenderIndex == CurrentSenderIndex)
                            {
                                UpdateSenderConnectInfo(reLayer, reLayer.SenderConnectInfoList[m]);

                                reLayer.SenderConnectInfoList.RemoveAt(m);

                                break;
                            }
                        }
                        for (int m = 0; m < reLayer.SenderConnectInfoList.Count; m++)
                        {
                            if (reLayer.SenderConnectInfoList[m].SenderIndex > CurrentSenderIndex)
                            {
                                //有连线的接收卡connectIndex-1;
                                for (int n = 0; n < reLayer.SenderConnectInfoList[m].PortConnectInfoList.Count; n++)
                                {
                                    ObservableCollection<IRectElement> connectElementList = reLayer.SenderConnectInfoList[m].PortConnectInfoList[n].ConnectLineElementList;
                                    if (connectElementList == null)
                                    {
                                        continue;
                                    }
                                    for (int u = 0; u < connectElementList.Count; u++)
                                    {
                                        connectElementList[u].SenderIndex -= 1;
                                    }
                                }
                                reLayer.SenderConnectInfoList[m].SenderIndex -= 1;
                            }
                        }
                    }
                #endregion
                NotifyPropertyChanged(GetPropertyName(o => this.ElementCollection));
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                #region 修改
                #endregion
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                #region 重置

                #endregion
            }
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {

            }
        }

        #endregion
        #region IRectElement 成员
        public override double X
        {
            get
            {
                return _x;
            }
            set
            {
                Thickness margin = new Thickness();
                margin = Margin;
                if (value == _x || value > 10000)
                {
                    return;
                }
                if (value < 0)
                {
                    _x = 0;                               
                    margin.Left = _x;
                    Margin = margin;
                    NotifyPropertyChanged(GetPropertyName(o => this.X));
                    return;
                }
                if (ParentElement != null && ParentElement.EleType == ElementType.screen)
                {
                    if (value + Width > ((RectLayer)ParentElement).Width)
                    {
                        if (value + Width > SmartLCTViewModeBase.MaxScreenWidth)
                        {
                            value = SmartLCTViewModeBase.MaxScreenWidth - Width;
                        }
                        ((RectLayer)ParentElement).Width = value + Width;
                    }
                }
                _x = value;
                margin.Left = _x;
                Margin = margin;
                NotifyPropertyChanged(GetPropertyName(o => this.X));
            }
        }
        public override double Y
        {
            get
            {
                return _y;
            }
            set
            {
                Thickness margin = new Thickness();
                margin = Margin;
                if (value == _y || value > 10000)
                {
                    return;
                }
                if (value < 0)
                {
                    _y = 0;
                    margin.Top = _y;
                    Margin = margin;
                    NotifyPropertyChanged(GetPropertyName(o => this.Y));
                    return;
                }
                if (ParentElement != null && ParentElement.EleType == ElementType.screen)
                {
                    if (value + Height > ((RectLayer)ParentElement).Height)
                    {
                        if (value + Height > SmartLCTViewModeBase.MaxScreenHeight)
                        {
                            value = SmartLCTViewModeBase.MaxScreenHeight - Height;
                        }
                        ((RectLayer)ParentElement).Height = value + Height;
                    }
                }
                _y = value;
                margin.Top = _y;
                Margin = margin;
                NotifyPropertyChanged(GetPropertyName(o => this.Y));
            }
        }
        public override double Width
        {
            get
            {
                return _width;
            }
            set
            {
                if (value == _width)
                {
                    return;
                }          
                if (EleType==ElementType.screen && value > SmartLCTViewModeBase.MaxScreenWidth && OperateEnviron==OperatEnvironment.DesignScreen)
                {
                    value = SmartLCTViewModeBase.MaxScreenWidth;
                }
                _width = Math.Round(value);

                StartAndEndIconMargin = new Thickness(Width / 2 - 10, Height / 2 - 10, 20, 20);
                NotifyPropertyChanged(GetPropertyName(o => this.Width));
            }
        }
    
        public override double Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (value == _height)
                {
                    return;
                }
                if (EleType==ElementType.screen && value > SmartLCTViewModeBase.MaxScreenHeight && OperateEnviron==OperatEnvironment.DesignScreen)
                {
                    value = SmartLCTViewModeBase.MaxScreenHeight;
                }
                _height = Math.Round(value);
                
                NotifyPropertyChanged(GetPropertyName(o => this.Height));
            }
        }
       
        public override int ZOrder
        {
            get
            {
                return _zOrder;
            }
            set
            {
                if (value == _zOrder)
                {
                    return;
                }
                List<int> zorderList = new List<int>();
                if (ParentElement != null)
                {
                    for (int i = 0; i < ((RectLayer)ParentElement).ElementCollection.Count; i++)
                    {
                        zorderList.Add(((RectLayer)ParentElement).ElementCollection[i].ZOrder);
                    }
                    zorderList.Remove(value);
                    while (zorderList.Contains(value))
                    {
                        value = value + 1;
                    }
                }

                _zOrder = value;
                
                NotifyPropertyChanged(GetPropertyName(o => this.ZOrder));

            }
        }
        [XmlIgnore]
        public override IElement ParentElement
        {
            get { return _parentElement; }
            set 
            {
                if (value == _parentElement)
                {
                    return;
                }
                _parentElement = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ParentElement));

            }
        }
        public override Thickness Margin
        {
            get { return _margin; }
            set
            {
                if (value == _margin)
                {
                    return;
                }
                Thickness old = _margin;
                X = Math.Round(value.Left);
                Y = Math.Round(value.Top);
                _margin.Left = X;
                _margin.Top = Y;
                NotifyPropertyChanged(GetPropertyName(o => this.Margin));
            }
        }
        public override SelectedState ElementSelectedState
        {
            get
            {
                return _elementSelectedState;
            }
            set
            {
                if (value == _elementSelectedState)
                {
                    return;
                }

                if (value == SelectedState.None && EleType==ElementType.groupframe)
                {
                    for (int i = 0; i < ElementCollection.Count; i++)
                    {                    
                        ElementCollection[i].ElementSelectedState = SelectedState.None;
                        if (ElementCollection[i].EleType == ElementType.receive)
                        {
                            ElementCollection[i].IsLocked = true;
                        }
                    }
                    SizeVisibility = Visibility.Hidden;
                }
                else
                {
                    if (EleType == ElementType.port)
                    {
                        SizeVisibility = Visibility.Visible;
                    }
                    if (ParentElement != null)
                    {
                        ParentElement.ElementSelectedState = SelectedState.None;
                    }
                }
                SelectedState old = _elementSelectedState;
                _elementSelectedState = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ElementSelectedState));
            }
        }
        #endregion

        #region IElement 成员

        public override int ConnectedIndex
        {
            get { return _connectedIndex; }
            set
            {
                _connectedIndex = value;
                string msg = "";
                if (_connectedIndex == -1)
                {
                    CommonStaticMethod.GetLanguageString("新建", "Lang_CommonData_RectLayer_New", out msg);
                    DisplayName = msg;
                }
                else if (EleType == ElementType.port)
                {
                    CommonStaticMethod.GetLanguageString("网口", "Lang_CommonData_RectLayer_Port", out msg);
                    DisplayName = msg + (value + 1).ToString();
                }
                else if (EleType == ElementType.sender)
                {
                    CommonStaticMethod.GetLanguageString("发送卡", "Lang_CommonData_RectLayer_Sender", out msg);
                    DisplayName = msg + (value + 1).ToString();
                }
                else if (EleType == ElementType.baselayer)
                {
                    CommonStaticMethod.GetLanguageString("显示屏", "Lang_CommonData_RectLayer_Screen", out msg);
                    DisplayName = msg + (value + 1).ToString();
                }
                NotifyPropertyChanged(GetPropertyName(o => this.ConnectedIndex));
            }

        }

        public override bool IsLocked
        {
            get
            {
                return _isLocked;
            }
            set
            {
                if (value == _isLocked)
                {
                    return;
                }
                _isLocked = value;
                bool old = _isLocked;
                NotifyPropertyChanged(GetPropertyName(o => this.IsLocked));
            }
        }

        public override Visibility Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                if (value == _visible)
                {
                    return;
                }
                _visible = value;
                if (ElementCollection != null)
                {
                    for (int i = 0; i < ElementCollection.Count; i++)
                    {
                        ElementCollection[i].Visible = value;
                    }
                }
                NotifyPropertyChanged(GetPropertyName(o => this.Visible));
            }
        }

        public override Thickness StartAndEndIconMargin
        {
            get
            {
                return _startAndEndIconMargin;
            }
            set
            {
                _startAndEndIconMargin = value;
                NotifyPropertyChanged(GetPropertyName(o => this.StartAndEndIconMargin));
            }
        }
        #endregion
        #endregion

        #region 构造函数
        public RectLayer() 
        { 
            SenderConnectInfoList = SenderConnectInfoList;
        }
        public RectLayer(double x, double y, double width, double height, IElement parentElement, int zOrder,ElementType eleType,int connectedIndex)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Margin = new Thickness(x, y, (x + width), (y + height));
            ParentElement = parentElement;
            ZOrder = zOrder;
            StartAndEndIconMargin = new Thickness(width / 2 - 10, height / 2 - 10, 20, 20);
            ElementCollection = ElementCollection;
            EleType = eleType;
            ConnectedIndex = connectedIndex;
            Visible = Visibility.Visible ;
            AddressVisible = Visibility.Hidden;
            SenderConnectInfoList = SenderConnectInfoList;
            CLineType = ConnectLineType.Auto;

        }
        #endregion

        #region ICopy 成员
        public bool CopyTow(object obj)
        {
            if (!(obj is RectLayer))
            {
                return false;
            }
            RectLayer temp = (RectLayer)obj;
            #region
            temp.BackgroundBrush = this.BackgroundBrush;
            temp.CenterX = this.CenterX;
            temp.CenterY = this.CenterY;
            temp.ElementSelectedState = this.ElementSelectedState;
            temp.Height = this.Height;
            temp.IsLocked = this.IsLocked;
            temp.IsOutlineDisplay = this.IsOutlineDisplay;
            temp.StartAndEndIconMargin = this.StartAndEndIconMargin;
            temp.Name = this.Name;
            temp.Opacity = this.Opacity;
            temp.OutlineBrush = this.OutlineBrush;
            temp.Visible = this.Visible;
            temp.Width = this.Width;
            temp.Tag = this.Tag;
            temp.EleType = this.EleType;
            temp.BorderThickness = this.BorderThickness;
            temp.SizeVisibility = this.SizeVisibility;
            temp.CLineType = this.CLineType;
            temp.MaxGroupName = this.MaxGroupName;
            temp.MaxZorder = this.MaxZorder;
            temp.CurrentPortIndex = this.CurrentPortIndex;
            temp.CurrentSenderIndex = this.CurrentSenderIndex;
            temp.RectSize = this.RectSize;
            temp.OperateEnviron = this.OperateEnviron;
            #endregion
           
            temp.ElementCollection = new ObservableCollection<IElement>();
            for (int i = 0; i < this.ElementCollection.Count; i++)
            {
                if (this.ElementCollection[i] is RectElement)
                {
                    RectElement rect = new RectElement();
                    rect = (RectElement)((RectElement)this.ElementCollection[i]).Clone();
                    rect.ParentElement = temp;
                    temp.ElementCollection.Add(rect);
                }
                else if (this.ElementCollection[i] is RectLayer)
                {
                    RectLayer rectLayer = new RectLayer();
                    rectLayer = (RectLayer)((RectLayer)this.ElementCollection[i]).Clone();
                    rectLayer.ParentElement = temp;
                    temp.ElementCollection.Add(rectLayer);
                }
                else if (this.ElementCollection[i] is LineElement)
                {
                    LineElement lineElement = new LineElement();
                    ((LineElement)this.ElementCollection[i]).CopyTo(lineElement);
                    lineElement.ParentElement = temp;
                    temp.ElementCollection.Add(lineElement);
                }
            }
            for (int i = 0; i < temp.ElementCollection.Count; i++)
            {
                if (temp.ElementCollection[i] is LineElement)
                {
                    for (int j = 0; j < temp.ElementCollection.Count; j++)
                    {
                        if ((temp.ElementCollection[j] is RectElement) && (((LineElement)temp.ElementCollection[i]).FrontElement.ZOrder == temp.ElementCollection[j].ZOrder))
                        {
                            ((LineElement)temp.ElementCollection[i]).FrontElement=(RectElement)temp.ElementCollection[j];
                        }
                        if ((temp.ElementCollection[j] is RectElement) && (((LineElement)temp.ElementCollection[i]).EndElement.ZOrder == temp.ElementCollection[j].ZOrder))
                        {
                            ((LineElement)temp.ElementCollection[i]).EndElement = (RectElement)temp.ElementCollection[j];
                        }
                    }
                    ((LineElement)temp.ElementCollection[i]).FrontElement.EndLine = (LineElement)temp.ElementCollection[i];
                    ((LineElement)temp.ElementCollection[i]).EndElement.FrontLine = (LineElement)temp.ElementCollection[i];
                }
            }
            return true;
        }

        public bool CopyTo(object obj)
        {
            if (!(obj is RectLayer))
            {
                return false;
            }
            RectLayer temp = (RectLayer)obj;
            temp.BackgroundBrush = this.BackgroundBrush;
            temp.CenterX = this.CenterX;
            temp.CenterY = this.CenterY;
            temp.ElementSelectedState = this.ElementSelectedState;
            temp.Height = this.Height;
            temp.IsLocked = this.IsLocked;
            temp.IsOutlineDisplay = this.IsOutlineDisplay;
            temp.StartAndEndIconMargin = this.StartAndEndIconMargin;
            Thickness margin = new Thickness();
            margin.Left = this.Margin.Left;
            margin.Right = this.Margin.Right;
            margin.Bottom = this.Margin.Bottom;
            margin.Top = this.Margin.Top;
            temp.Margin = margin;
            temp.Name = this.Name;
            temp.Opacity = this.Opacity;
            temp.OutlineBrush = this.OutlineBrush;
            temp.ParentElement = this.ParentElement;
            temp.Visible = this.Visible;
            temp.Width = this.Width;
            temp.X = this.X;
            temp.Y = this.Y;
            temp.ZOrder = this.ZOrder;
            temp.CLineType = this.CLineType;

            temp.DisplayName = this.DisplayName;
            temp.Tag = this.Tag;
            temp.EleType = this.EleType;
            temp.BorderThickness = this.BorderThickness;
            temp.ConnectedIndex = this.ConnectedIndex;
            temp.ElementCollection = new ObservableCollection<IElement>();
            temp.SizeVisibility = this.SizeVisibility;
			temp.MaxZorder=this.MaxZorder;
            temp.MaxGroupName = this.MaxGroupName;
            temp.CurrentPortIndex = this.CurrentPortIndex;
            temp.RectSize = this.RectSize;
            temp.OperateEnviron = this.OperateEnviron;
            temp.CurrentSenderIndex = this.CurrentSenderIndex;
            temp.SelectedSenderConfigInfo = this.SelectedSenderConfigInfo;
            for (int i = 0; i < this.ElementCollection.Count; i++)
            {
                RectElement rect = new RectElement();
                RectLayer rectLayer = new RectLayer();
                LineElement lineElement = new LineElement();
                if (this.ElementCollection[i] is RectElement)
                {
                    rect = (RectElement)((RectElement)this.ElementCollection[i]).Clone();
                    rect.ParentElement = temp;
                    temp.ElementCollection.Add(rect);
                }
                else if (this.ElementCollection[i] is RectLayer)
                {
                    ((RectLayer)this.ElementCollection[i]).CopyTo(rectLayer);
                    rectLayer.ParentElement = temp;
                    temp.ElementCollection.Add(rectLayer);
                }
                else if (this.ElementCollection[i] is LineElement)
                {
                    ((LineElement)this.ElementCollection[i]).CopyTo(lineElement);
                    lineElement.ParentElement = temp;
                    temp.ElementCollection.Add(lineElement);
                }
            }
            for (int i = 0; i < temp.ElementCollection.Count; i++)
            {
                if (temp.ElementCollection[i] is LineElement)
                {
                    for (int j = 0; j < temp.ElementCollection.Count; j++)
                    {
                        if ((temp.ElementCollection[j] is RectElement) && (((LineElement)temp.ElementCollection[i]).FrontElement.ZOrder == temp.ElementCollection[j].ZOrder))
                        {
                            ((LineElement)temp.ElementCollection[i]).FrontElement=(RectElement)temp.ElementCollection[j];
                        }
                        if ((temp.ElementCollection[j] is RectElement) && (((LineElement)temp.ElementCollection[i]).EndElement.ZOrder == temp.ElementCollection[j].ZOrder))
                        {
                            ((LineElement)temp.ElementCollection[i]).EndElement = (RectElement)temp.ElementCollection[j];
                        }
                    }
                    ((LineElement)temp.ElementCollection[i]).FrontElement.EndLine = (LineElement)temp.ElementCollection[i];
                    ((LineElement)temp.ElementCollection[i]).EndElement.FrontLine = (LineElement)temp.ElementCollection[i];
                }
            }
            return true;
        }
        #endregion

        #region ICloneable 成员
        public object Clone()
        {
            RectLayer newObj = new RectLayer();
            bool res = this.CopyTo(newObj);
            if (!res)
            {
                return null;
            }
            else
            {
                return newObj;
            }
        }
        public object Clonew()
        {
            RectLayer newObj = new RectLayer();
            bool res = this.CopyTow(newObj);
            if (!res)
            {
                return null;
            }
            else
            {
                return newObj;
            }
        }
        #endregion


        public event PrePropertyChangedEventHandler PrePropertyChangedEvent;
        private void OnPrePropertyChangedEvent(object sender, PrePropertyChangedEventArgs e)
        {
            if (PrePropertyChangedEvent != null)
            {
                PrePropertyChangedEvent(sender, e);
            }
        }
        #region RectLayer 成员
        public void NotifyChildRectConnectedChanged()
        {
            //TODO:添加线条元素
        }
        #endregion

    }

}
