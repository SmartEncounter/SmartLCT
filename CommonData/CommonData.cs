using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nova.LCT.GigabitSystem.Common;
using System.Windows.Data;
using System.Globalization;
using Nova.LCT.Message.Client;
using GuiLabs.Undo;
using Nova.LCT.GigabitSystem.Files;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Nova.SmartLCT.Interface
{

    public class BrightInfo
    {
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
        private string _displayName;
        public double AreaUpperValue
        {
            get { return _areaUpperValue; }
            set { _areaUpperValue = value; }
        }
        private double _areaUpperValue=-1;
        public double AreaLowerValue
        {
            get { return _areaLowerValue; }
            set { _areaLowerValue = value; }
        }
        private double _areaLowerValue=-1;

        public BrightInfo() { }
        public BrightInfo(string displayName,double areaUpperValue, double areaLowerValue)
        {
            _displayName = displayName;
            _areaLowerValue = areaLowerValue;
            _areaUpperValue = areaUpperValue;
        }
    }

    public class EnvironAndDisplayBrightCollection : ObservableCollection<DisplayAutoBrightMapping>
    {

    }

    public class WeekInfo : NotificationObject
    {
        public DayOfWeek WeekValue
        {
            get { return _weekValue; }
            set 
            { 
                _weekValue = value;
                NotifyPropertyChanged("WeekValue");
            }
        }
        private DayOfWeek _weekValue;
        public bool IsSelected
        {
            get { return _isSelected; }
            set 
            {
                _isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }
        }
        private bool _isSelected = false;
        public WeekInfo() { }
        public WeekInfo(DayOfWeek weekValue, bool isSelected)
        {
            _weekValue = weekValue;
            _isSelected = isSelected;
        }
    }
    

    public class SenderAndPortPicInfo
    {
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
            }
        }
        private int _index;
        public string PortColor
        {
            get { return _portColor; }
            set
            {
                _portColor = value;
            }
        }
        private string _portColor;
        public string SelectedPicPath
        {
            get { return _selectedPicPath; }
            set
            {
                _selectedPicPath = value;
            }
        }
        private string _selectedPicPath;
        public string NoSelectedPicPath
        {
            get { return _noSelectedPicPath; }
            set
            {
                _noSelectedPicPath = value;
            }
        }
        private string _noSelectedPicPath;
        public SenderAndPortPicInfo() { }
        public SenderAndPortPicInfo(int index,string portColor, string selectedPicPath, string noSelectedPicPath)
        {
            _index = index;
            _portColor = portColor;
            _selectedPicPath = selectedPicPath;
            _noSelectedPicPath = noSelectedPicPath;
        }
    }
    public class PortInfo
    {
        public int SenderIndex
        {
            get { return _senderIndex; }
            set
            {
                _senderIndex = value;
            }
        }
        private int _senderIndex = -1;
        public int PortIndex
        {
            get { return _portIndex; }
            set
            {
                _portIndex = value;
            }
        }
        private int _portIndex = -1;
        public PortInfo()
        {

        }
        public PortInfo(int senderIndex, int portIndex)
        {
            _senderIndex = senderIndex;
            _portIndex = portIndex;
        }
    }
    /// <summary>
    /// 显示屏映射位置信息
    /// </summary>
    public class ScreenMapRealParameters:SmartLCTViewModeBase
    {
        public IRectElement SenderLoadRectLayer
        {
            get { return _senderLoadRectLayer; }
            set 
            { 
                _senderLoadRectLayer = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SenderLoadRectLayer));
            }
        }
        private IRectElement _senderLoadRectLayer = new RectElement();

        public ElementType RectLayerType
        {
            get { return _rectLayerType; }
            set
            {
                _rectLayerType = value;
                NotifyPropertyChanged(GetPropertyName(o => this.RectLayerType));
            }
        }
        private ElementType _rectLayerType = ElementType.None;
        public ObservableCollection<IElement> RectLayerCollection
        {
            get 
            { return _rectLayerCollection; }
            set 
            {
                _rectLayerCollection=value;
                NotifyPropertyChanged(GetPropertyName(o => this.RectLayerCollection));
            }
        }
        private ObservableCollection<IElement> _rectLayerCollection = new ObservableCollection<IElement>();
        public ScreenMapRealParameters() { }
        public ScreenMapRealParameters(IRectElement senderLoadRectLayer, ObservableCollection<IElement> rectLayerCollection,ElementType type)
        {
            _senderLoadRectLayer = senderLoadRectLayer;
            _rectLayerCollection = rectLayerCollection;
            _rectLayerType = type;
        }
    }
    public class SenderAndPortIndexInfo
    {
        public ElementType EleType
        {
            get { return _eleType; }
            set { _eleType = value; }
        }
        private ElementType _eleType;
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        private int _index;
        public SenderAndPortIndexInfo() { }
        public SenderAndPortIndexInfo(ElementType eleType, int index)
        {
            _eleType = eleType;
            _index = index;
        }
    }
    public class SelectedStateInfo
    {
        public IRectElement Element
        {
            get { return _element; }
            set { _element = value; }
        }
        private IRectElement _element;
        public SelectedState OldSelectedState
        {
            get { return _oldSelectedState; }
            set { _oldSelectedState = value; }
        }
        private SelectedState _oldSelectedState;
        public SelectedState NewSelectedState
        {
            get { return _newSelectedState; }
            set { _newSelectedState = value; }
        }
        private SelectedState _newSelectedState;
        public SelectedStateInfo() { }
        public SelectedStateInfo(IRectElement element, SelectedState oldSelectedState, SelectedState newSelectedState)
        {
            _element = element;
            _oldSelectedState = oldSelectedState;
            _newSelectedState = newSelectedState;
        }
    }
    public class GroupInfo
    {
        public IRectElement Element
        {
            get { return _element; }
            set { _element = value; }
        }
        private IRectElement _element;
        public OldAndNewType OldAndNewGroupName
        {
            get { return _oldAndNewGroupName; }
            set { _oldAndNewGroupName = value; }
        }
        private OldAndNewType _oldAndNewGroupName;
        public GroupInfo() { }
        public GroupInfo(IRectElement element, OldAndNewType oldAndNewGroupName)
        {
            _element = element;
            _oldAndNewGroupName = oldAndNewGroupName;
        }
    }
    public class AddLineInfo
    {
        public IRectElement Element
        {
            get { return _element; }
            set { _element = value; }
        }
        private IRectElement _element;

        public OldAndNewType OldAndNewConnectIndex
        {
            get { return _oldAndNewConnectIndex; }
            set { _oldAndNewConnectIndex = value; }
        }
        private OldAndNewType _oldAndNewConnectIndex;
        public OldAndNewType OldAndNewSenderIndex
        {
            get { return _oldAndNewSenderIndex; }
            set { _oldAndNewSenderIndex = value; }
        }
        private OldAndNewType _oldAndNewSenderIndex;
        public OldAndNewType OldAndNewPortIndex
        {
            get { return _oldAndNewPortIndex; }
            set { _oldAndNewPortIndex = value; }
        }
        private OldAndNewType _oldAndNewPortIndex;
        public AddLineInfo() { }
        public AddLineInfo(IRectElement element, OldAndNewType oldAndNewConnectIndex, OldAndNewType oldAndNewSenderIndex,
            OldAndNewType oldAndNewPortIndex)
        {
            _element = element;
            _oldAndNewConnectIndex = oldAndNewConnectIndex;
            _oldAndNewPortIndex = oldAndNewPortIndex;
            _oldAndNewSenderIndex = oldAndNewSenderIndex;
        }
    }
    public class ConnectIconVisibilityInfo
    {
        public IRectElement Element
        {
            get { return _element; }
            set { _element = value; }
        }
        private IRectElement _element;
        public int SenderIndex
        {
            get { return _senderIndex; }
            set { _senderIndex = value; }
        }
        private int _senderIndex;
        public int PortIndex
        {
            get { return _portIndex; }
            set { _portIndex = value; }
        }
        private int _portIndex;
        public OldAndNewVisibility OldAndNewMaxConnectIndexVisibile
        {
            get { return _oldAndNewMaxConnectIndexVisible; }
            set { _oldAndNewMaxConnectIndexVisible = value; }
        }
        private OldAndNewVisibility _oldAndNewMaxConnectIndexVisible;
        public OldAndNewVisibility OldAndNewMinConnectIndexVisibile
        {
            get { return _oldAndNewMinConnectIndexVisibile; }
            set { _oldAndNewMinConnectIndexVisibile = value; }
        }
        private OldAndNewVisibility _oldAndNewMinConnectIndexVisibile;
        public ConnectIconVisibilityInfo() { }
        public ConnectIconVisibilityInfo(IRectElement element,OldAndNewVisibility oldAndNewMaxConnectIndexVisibile, OldAndNewVisibility oldAndNewMinConnectIndexVisibile)
        {
            _element = element;
            _oldAndNewMaxConnectIndexVisible = oldAndNewMaxConnectIndexVisibile;
            _oldAndNewMinConnectIndexVisibile = oldAndNewMinConnectIndexVisibile;
        }
    }
    public class OldAndNewType
    {
        public int OldValue
        {
            get { return _oldValue; }
            set { _oldValue = value; }
        }
        private int _oldValue;
        public int NewValue
        {
            get { return _newValue; }
            set { _newValue = value; }
        }
        private int _newValue;
        public OldAndNewType() { }
        public OldAndNewType(int oldValue, int newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }
    }
    public class OldAndNewVisibility
    {
        public Visibility OldValue
        {
            get { return _oldValue; }
            set { _oldValue = value; }
        }
        private Visibility _oldValue;
        public Visibility NewValue
        {
            get { return _newValue; }
            set { _newValue = value; }
        }
        private Visibility _newValue;
        public OldAndNewVisibility() { }
        public OldAndNewVisibility(Visibility oldValue, Visibility newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }
    }
    public class ElementSizeInfo
    {
        public IRectElement Element
        {
            get { return _element; }
            set { _element = value; }
        }
        private IRectElement _element;
        public Size OldSize
        {
            get { return _oldSize; }
            set { _oldSize = value; }
        }
        private Size _oldSize;
        public Size NewSize
        {
            get { return _newSize; }
            set { _newSize = value; }
        }
        private Size _newSize;
        public ElementSizeInfo() { }
        public ElementSizeInfo(IRectElement element, Size newSize, Size oldSize)
        {
            _element = element;
            _oldSize = oldSize;
            _newSize = newSize;
        }
    }

    public class ElementMoveInfo
    {
        public IRectElement Element
        {
            get { return _element; }
            set { _element = value; }
        }
        private IRectElement _element;
        public Point OldPoint
        {
            get { return _oldPoint; }
            set { _oldPoint = value; }
        }
        private Point _oldPoint = new Point();
        public Point NewPoint
        {
            get { return _newPoint; }
            set { _newPoint = value; }
        }
        private Point _newPoint = new Point();
        public ElementMoveInfo() { }
        public ElementMoveInfo(IRectElement element, Point newPoint, Point oldPoint)
        {
            _element = element;
            _newPoint = newPoint;
            _oldPoint = oldPoint;
        }
    }

    public class AddReceiveInfo
    {
        public int Rows
        {
            get { return _rows; }
            set
            {
                _rows = value;
            }
        }
        private int _rows;
        public int Cols
        {
            get { return _cols; }
            set { _cols = value; }
        }
        private int _cols;
        public ScannerCofigInfo ScanConfig
        {
            get { return _scanConfig; }
            set
            {
                _scanConfig = value;
            }
        }
        private ScannerCofigInfo _scanConfig;
        public AddReceiveInfo() { }
        public AddReceiveInfo(int rows, int cols, ScannerCofigInfo scanConfig)
        {
            _scanConfig = scanConfig;
            _rows = rows;
            _cols = cols;
        }
    }

    [Serializable]
    public class PortConnectInfo : INotifyPropertyChanged
    {
        public int SenderIndex
        {
            get { return _senderIndex; }
            set
            {
                _senderIndex = value;
                NotifyPropertyChanged(GetPropertyName(o => this.MapLocation));
            }
        }
        private int _senderIndex;
        public Point MapLocation
        {
            get { return _mapLocation; }
            set
            {
                _mapLocation = value;
                NotifyPropertyChanged(GetPropertyName(o => this.MapLocation));
            }
        }
        private Point _mapLocation;
        public int PortIndex
        {
            get { return _portIndex; }
            set
            {
                _portIndex = value;
            }
        }
        private int _portIndex;
        public Rect LoadSize
        {
            get { return _loadSize; }
            set
            {
                _loadSize = value;
                if (value.Width * value.Height > MaxLoadArea.Width * MaxLoadArea.Height)
                {
                    IsOverLoad = true;
                }
                else
                {
                    IsOverLoad = false;
                }
                NotifyPropertyChanged(GetPropertyName(o => this.LoadSize));

            }
        }
        private Rect _loadSize;
        public bool IsOverLoad
        {
            get { return _isOverLoad; }
            set
            {
                _isOverLoad = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsOverLoad));
            }
        }
        private bool _isOverLoad = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsSelected));
            }
        }
        private bool _isSelected;
        public Size MaxLoadArea
        {
            get { return _maxLoadArea; }
            set { _maxLoadArea = value; }
        }
        private Size _maxLoadArea;
        public int MaxConnectIndex
        {
            get { return _maxConnectIndex; }
            set
            {
                _maxConnectIndex = value;
            }
        }
        private int _maxConnectIndex;
        [XmlIgnore]
        public RectElement MaxConnectElement
        {
            get { return _maxConnectElement; }
            set
            {
                _maxConnectElement = value;
            }
        }
        private RectElement _maxConnectElement;
        [XmlIgnore]
        public ObservableCollection<IRectElement> ConnectLineElementList
        {
            get { return _connectLineElementList; }
            set
            {
                _connectLineElementList = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ConnectLineElementList));
            }
        }
        private ObservableCollection<IRectElement> _connectLineElementList = new ObservableCollection<IRectElement>();

        public PortConnectInfo()
        {
            _maxLoadArea = Function.CalculatePortLoadSize(60, 24);
        }
        public PortConnectInfo(int portIndex, int senderIndex, int maxConnectIndex, RectElement maxConnecElement, ObservableCollection<IRectElement> connectLineElementList, Rect portSize)
        {
            _portIndex = portIndex;
            _senderIndex = senderIndex;
            _maxConnectElement = maxConnecElement;
            _maxConnectIndex = maxConnectIndex;
            _connectLineElementList = connectLineElementList;
            _loadSize = portSize;
            _maxLoadArea = Function.CalculatePortLoadSize(60, 24);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string GetPropertyName<T>(Expression<Func<NotificationST, T>> expr)
        {
            var name = ((MemberExpression)expr.Body).Member.Name;
            return name;
        }
    }

    [Serializable]
    public class SenderConnectInfo : INotifyPropertyChanged
    {
        public bool IsStatIncreaseOrDecreaseIndex
        {
            get { return _isStartIndexOrDecreaseIndex; }
            set
            {
                _isStartIndexOrDecreaseIndex = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsStatIncreaseOrDecreaseIndex));
            }
        }
        private bool _isStartIndexOrDecreaseIndex = false;
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
        public Size DviSize
        {
            get { return _dviSize; }
            set
            {
                _dviSize = value;
                NotifyPropertyChanged(GetPropertyName(o => this.DviSize));
            }
        }
        private Size _dviSize = new Size();
        public Point MapLocation
        {
            get { return _mapLocation; }
            set
            {
                _mapLocation = value;
                //Rect loadSize = LoadSize;
                //loadSize.X += value.X;
                //loadSize.Y += value.Y;
                NotifyPropertyChanged(GetPropertyName(o => this.MapLocation));
            }
        }
        private Point _mapLocation;
        public int SenderIndex
        {
            get { return _senderIndex; }
            set
            {
                _senderIndex = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SenderIndex));
            }
        }
        private int _senderIndex;
        public int SelectedPortIndex
        {
            get { return _selectedPortIndex; }
            set
            {
                _selectedPortIndex = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SelectedPortIndex));
            }
        }
        private int _selectedPortIndex = 0;
        public ElementType EleType
        {
            get { return _eleType; }
            set 
            { 
                _eleType = value;
                NotifyPropertyChanged(GetPropertyName(o => this.EleType));
            }
        }
        private ElementType _eleType;
        public Rect LoadSize
        {
            get { return _loadSize; }
            set
            {
                _loadSize = value;
                if (value.Width * value.Height > MaxLoadArea.Height * MaxLoadArea.Width)
                {
                    IsOverLoad = true;
                }
                else
                {
                    IsOverLoad = false;
                }
                NotifyPropertyChanged(GetPropertyName(o => this.LoadSize));

            }
        }
        private Rect _loadSize;
        public Size MaxLoadArea
        {
            get { return _maxLoadArea; }
            set 
            { 
                _maxLoadArea = value;
                NotifyPropertyChanged(GetPropertyName(o => this.MaxLoadArea));
            }
        }
        private Size _maxLoadArea;
        public bool IsOverLoad
        {
            get { return _isOverLoad; }
            set
            {
                _isOverLoad = value;
                if (IsOverLoad)
                {
                    IsExpand = true;
                }
                NotifyPropertyChanged(GetPropertyName(o => this.IsOverLoad));
            }
        }
        private bool _isOverLoad = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsSelected));
            }
        }
        private bool _isSelected;
        public bool IsExpand
        {
            get { return _isExpand; }
            set
            {
                _isExpand = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsExpand));
            }
        }
        private bool _isExpand;
        public ObservableCollection<PortConnectInfo> PortConnectInfoList
        {
            get { return _portConnectInfoList; }
            set
            {
                if (_portConnectInfoList != null)
                {
                    _portConnectInfoList.CollectionChanged -= new NotifyCollectionChangedEventHandler(OnPortConnectInfoListChange);

                }
                _portConnectInfoList = value;
                if (_portConnectInfoList != null)
                {
                    _portConnectInfoList.CollectionChanged += new NotifyCollectionChangedEventHandler(OnPortConnectInfoListChange);

                }

                NotifyPropertyChanged(GetPropertyName(o => this.PortConnectInfoList));
            }
        }
        private ObservableCollection<PortConnectInfo> _portConnectInfoList = new ObservableCollection<PortConnectInfo>();
        private void OnPortConnectInfoListChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            MaxLoadArea = Function.CalculateSenderLoadSize(PortConnectInfoList.Count, 60, 24);
            if (PortConnectInfoList != null && PortConnectInfoList.Count != 0)
            {
                for (int i = 0; i < PortConnectInfoList.Count; i++)
                {
                    if (PortConnectInfoList[i].IsOverLoad)
                    {
                        IsExpand = true;
                        break;
                    }
                }
            }
            NotifyPropertyChanged(GetPropertyName(o => this.PortConnectInfoList));
        }
        public SenderConnectInfo()
        {
            _maxLoadArea = Function.CalculateSenderLoadSize(PortConnectInfoList.Count, 60, 24);
            PortConnectInfoList = PortConnectInfoList;
            _dviSize = _maxLoadArea;
            IsStatIncreaseOrDecreaseIndex = false;
            IncreaseOrDecreaseIndex = 0;
        }
        public SenderConnectInfo(int senderIndex, ObservableCollection<PortConnectInfo> portConnectInfoList, Rect senderSize)
        {
            _senderIndex = senderIndex;
            PortConnectInfoList = portConnectInfoList;
            _loadSize = senderSize;
            _eleType = ElementType.sender;
            _maxLoadArea = Function.CalculateSenderLoadSize(portConnectInfoList.Count, 60, 24);
            IsStatIncreaseOrDecreaseIndex = false;
            IncreaseOrDecreaseIndex = 0;
            _dviSize = _maxLoadArea;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string GetPropertyName<T>(Expression<Func<NotificationST, T>> expr)
        {
            var name = ((MemberExpression)expr.Body).Member.Name;
            return name;
        }
    }

    public class SelectedInfo
    {
        public ObservableCollection<IRectElement> SelectedGroupElementList
        {
            get { return _selectedGroupElementList; }
            set
            {
                _selectedGroupElementList = value;
            }
        }
        private ObservableCollection<IRectElement> _selectedGroupElementList = new ObservableCollection<IRectElement>();
        public ObservableCollection<IRectElement> NoSelectedGroupElementList
        {
            get { return _noSelectedGroupElementList; }
            set
            {
                _noSelectedGroupElementList = value;
            }
        }
        private ObservableCollection<IRectElement> _noSelectedGroupElementList = new ObservableCollection<IRectElement>();
        public Rect SelectedElementRect
        {
            get { return _selectedElementRect; }
            set
            {
                _selectedElementRect = value;
            }
        }
        private Rect _selectedElementRect;
        public Rect NoSelectedElementRect
        {
            get { return _noSelectedElementRect; }
            set
            {
                _noSelectedElementRect = value;
            }
        }
        private Rect _noSelectedElementRect;
        public SelectedInfo() { }
        public SelectedInfo(ObservableCollection<IRectElement> selectedGroupElement, ObservableCollection<IRectElement> noSelectedGroupElement, Rect selectedElementeRect, Rect noSelectedElementRect)
        {
            _selectedGroupElementList = selectedGroupElement;
            _selectedElementRect = selectedElementeRect;
            _noSelectedElementRect = noSelectedElementRect;
            _noSelectedGroupElementList = noSelectedGroupElement;
        }
    }
    public class SenderAndPort
    {
        public int SenderIndex
        {
            get { return _senderIndex; }
            set { _senderIndex = value; }
        }
        private int _senderIndex;
        public ObservableCollection<int> PortIndexList
        {
            get { return _portIndexList; }
            set
            {
                _portIndexList = value;
            }
        }
        private ObservableCollection<int> _portIndexList;
        public SenderAndPort() { }
        public SenderAndPort(int senderIndex, ObservableCollection<int> portIndexList)
        {
            _senderIndex = senderIndex;
            _portIndexList = portIndexList;
        }
    }
    public class ControlTriggerData
    {
        public object Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }
        private object _sender;
        public object EventArges
        {
            get { return _eventArgs; }
            set { _eventArgs = value; }
        }
        private object _eventArgs;
        public object Parameter
        {
            get { return _parameter; }
            set { _parameter = value; }
        }
        private object _parameter;
        public ControlTriggerData() { }
        public ControlTriggerData(object sender, object eventArges, object parameter)
        {
            _sender = sender;
            _eventArgs = eventArges;
            _parameter = parameter;
        }
    }
    public class ArrageTypeParam
    {
        public static ArrangeType LeftTopHor = ArrangeType.LeftTop_Hor;
        public static ArrangeType RightTopHor = ArrangeType.RightTop_Hor;
        public static ArrangeType LeftBottomHor = ArrangeType.LeftBottom_Hor;
        public static ArrangeType RightBottomHor = ArrangeType.RightBottom_Hor;
        public static ArrangeType LeftTopVer = ArrangeType.LeftTop_Ver;
        public static ArrangeType RightTopVer = ArrangeType.RightTop_Ver;
        public static ArrangeType LeftBottomVer = ArrangeType.LeftBottom_Ver;
        public static ArrangeType RightBottomVer = ArrangeType.RightBottom_Ver;
    }
    public class ConfigurationData
    {
        #region 属性
        public OperateScreenType OperateType
        {
            get { return _operateType; }
            set
            {  _operateType = value;}
        }
        private OperateScreenType _operateType = OperateScreenType.CreateScreen;
        public bool IsCreateEmptyProject
        {
            get { return _isCreateEmptyProject; }
            set { _isCreateEmptyProject = value; }
        }
        private bool _isCreateEmptyProject = false;
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                _projectName = value;
            }
        }
        private string _projectName;
        public string ProjectLocationPath
        {
            get { return _projectLocationPath; }
            set
            {
                _projectLocationPath = value;
            }
        }
        private string _projectLocationPath = "";
        public SenderConfigInfo SelectedSenderConfigInfo
        {
            get { return _selectedSenderConfigInfo; }
            set
            {
                _selectedSenderConfigInfo = value;
            }
        }
        private SenderConfigInfo _selectedSenderConfigInfo = new SenderConfigInfo();
        public ScannerCofigInfo SelectedScannerConfigInfo
        {
            get
            {
                return _selectedScannerConfigInfo;
            }
            set
            {
                _selectedScannerConfigInfo = value;
            }
        }
        private ScannerCofigInfo _selectedScannerConfigInfo = new ScannerCofigInfo();
        public int Rows
        {
            get { return _rows; }
            set
            {
                _rows = value;
            }
        }
        private int _rows = 1;
        public int Cols
        {
            get { return _cols; }
            set
            {
                _cols = value;
            }
        }
        private int _cols = 1;
        public ArrangeType SelectedArrangeType
        {
            get { return _selectedArrangeType; }
            set { _selectedArrangeType = value; }
        }
        private ArrangeType _selectedArrangeType = ArrangeType.LeftBottom_Hor;
        public ConfigurationData()
        {

        }
        public ConfigurationData(string projectName, string projectPath, SenderConfigInfo selectedSenderConfigInfo, ScannerCofigInfo selectedScannerConfigInfo, int rows, int cols,ArrangeType selectedArrangeType,bool isCreateEmptyProject,OperateScreenType operateType)
        {
            _projectName = projectName;
            _projectLocationPath = projectPath;
            _selectedSenderConfigInfo = selectedSenderConfigInfo;
            _selectedScannerConfigInfo = selectedScannerConfigInfo;
            _rows = rows;
            _cols = cols;
            _selectedArrangeType = selectedArrangeType;
            _isCreateEmptyProject = isCreateEmptyProject;
            _operateType = operateType;
        }
        #endregion

    }
    public class ProductInfo : INotifyPropertyChanged
    {
        public string PicPath
        {
            get { return _picPath; }
            set
            {
                _picPath = value;
            }
        }
        private string _picPath = "";
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        private string _description = "";
        public double Width
        {
            get { return _width; }
            set
            { 
                _width = value;
                NotifyPropertyChanged(GetPropertyName(o => this.Width));
            }
        }
        private double _width = 0;
        public double Height
        {
            get { return _height; }
            set 
            {
                _height = value;
                NotifyPropertyChanged(GetPropertyName(o => this.Height));
            }
        }
        private double _height = 0;
        public ProductInfo() { }
        public ProductInfo(string picPath, string description,double height,double width)
        {
            _picPath = picPath;
            _description = description;
            _height = height;
            _width = width;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string GetPropertyName<T>(Expression<Func<NotificationST, T>> expr)
        {
            var name = ((MemberExpression)expr.Body).Member.Name;
            return name;
        }

    }
    public class ProjectInfo
    {
        public string ProjectPath
        {
            get { return _projectPath; }
            set
            {
                _projectPath = value;
            }
        }
        private string _projectPath = "";
        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }
        private string _projectName = "";
        public ProjectInfo() { }
        public ProjectInfo(string path, string projectName)
        {
            _projectPath = path;
            _projectName = projectName;
        }
    }
    public class StaicResizeType
    {
        public ResizeType TL
        {
            get { return _tl; }
            set { _tl = value; }
        }
        private ResizeType _tl = ResizeType.TL;

        public ResizeType TC
        {
            get { return _tc; }
            set { _tc = value; }
        }
        private ResizeType _tc = ResizeType.TC;

        public ResizeType TR
        {
            get { return _tr; }
            set { _tr = value; }
        }
        private ResizeType _tr = ResizeType.TR;

        public ResizeType CL
        {
            get { return _cl; }
            set { _cl = value; }
        }
        private ResizeType _cl = ResizeType.CL;

        public ResizeType CC
        {
            get { return _cc; }
            set { _cc = value; }
        }
        private ResizeType _cc = ResizeType.CC;

        public ResizeType CR
        {
            get { return _cr; }
            set { _cr = value; }
        }
        private ResizeType _cr = ResizeType.CR;

        public ResizeType BL
        {
            get { return _bl; }
            set { _bl = value; }
        }
        private ResizeType _bl = ResizeType.BL;

        public ResizeType BC
        {
            get { return _bc; }
            set { _bc = value; }
        }
        private ResizeType _bc = ResizeType.BC;

        public ResizeType BR
        {
            get { return _br; }
            set { _br = value; }
        }
        private ResizeType _br = ResizeType.BR;
    }

    public class SenderInfo : NotificationST
    {
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
        private string _displayName = "";
        public int ZOrder
        {
            get { return _zOrder; }
            set
            {
                _zOrder = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ZOrder));
            }
        }
        private int _zOrder = -1;
        public ObservableCollection<int> PortCollection
        {
            get { return _portCollection; }
            set
            {
                _portCollection = value;
                NotifyPropertyChanged(GetPropertyName(o => this.PortCollection));
            }
        }
        private ObservableCollection<int> _portCollection = new ObservableCollection<int>();
        public SenderInfo(string displayName, int zOrder, ObservableCollection<int> portCollection)
        {
            _displayName = displayName;
            _zOrder = zOrder;
            _portCollection = portCollection;
        }
        public SenderInfo()
        {

        }
    }
    public class SenderAndPortInfo
    {
        public RectLayer Layer
        {
            get { return _layer; }
            set
            {
                _layer = value;
            }
        }
        private RectLayer _layer = null;
        public bool IsEnabledSelectedSender
        {
            get { return _isEnabledSelectedSender; }
            set { _isEnabledSelectedSender = value; }
        }
        private bool _isEnabledSelectedSender = true;
        public ObservableCollection<SenderInfo> SenderInfoList
        {
            get
            {
                return _senderInfoList;
            }
            set
            {
                _senderInfoList = value;
            }
        }
        private ObservableCollection<SenderInfo> _senderInfoList = new ObservableCollection<SenderInfo>();

        public SenderAndPortInfo(ObservableCollection<SenderInfo> senderInfoList)
        {
            _senderInfoList = senderInfoList;
        }
        public SenderAndPortInfo() { }
    }
    public class SenderCollectionInfo
    {
        public ObservableCollection<int> SenderCollection
        {
            get { return _senderCollection; }
            set
            {
                _senderCollection = value;
            }
        }
        private ObservableCollection<int> _senderCollection = new ObservableCollection<int>();
        public RectLayer Layer
        {
            get { return _layer; }
            set
            {
                _layer = value;
            }
        }
        private RectLayer _layer = null;
        public SenderCollectionInfo() { }
        public SenderCollectionInfo(ObservableCollection<int> senderCollection, RectLayer layer)
        {
            _senderCollection = senderCollection;
            _layer = layer;
        }

    }
    public class AddPortInfo
    {
        public SenderInfo SelectedSenderInfo
        {
            get { return _selectedSenderInfo; }
            set
            {
                _selectedSenderInfo = value;
            }
        }
        private SenderInfo _selectedSenderInfo = new SenderInfo();
        public int PortIndex
        {
            get { return _portIndex; }
            set
            {
                _portIndex = value;
            }
        }
        private int _portIndex = -1;
        public RectLayer Layer
        {
            get { return _layer; }
            set
            {
                _layer = value;
            }
        }
        private RectLayer _layer = null;
        public AddPortInfo(SenderInfo senderInfo, int portIndex, RectLayer layer)
        {
            _selectedSenderInfo = senderInfo;
            _portIndex = portIndex;
            _layer = layer;
        }
    }
    public class AddSenderInfo
    {
        public int SenderIndex
        {
            get { return _senderIndex; }
            set
            {
                _senderIndex = value;
            }
        }
        private int _senderIndex = 0;
        public RectLayer Layer
        {
            get { return _layer; }
            set
            {
                _layer = value;
            }
        }
        private RectLayer _layer = null;
        public AddSenderInfo() { }
        public AddSenderInfo(int senderIndex, RectLayer layer)
        {
            _senderIndex = senderIndex;
            _layer = layer;
        }
    }
    public class SelectedElementInfo
    {
        public RectLayer ParentElement
        {
            get { return _parentElement; }
            set { _parentElement = value; }
        }
        private RectLayer _parentElement = new RectLayer();
        public int ZOrder
        {
            get { return _zOrder; }
            set { _zOrder = value; }
        }
        private int _zOrder = 0;

        public SelectedElementInfo() { }
        public SelectedElementInfo(RectLayer parentElment, int zOrder)
        {
            _parentElement = parentElment;
            _zOrder = zOrder;
        }
    }
    public class SelectedLayerAndElement
    {
        public Size MainControlSize
        {
            get { return _mainControlSize; }
            set
            {
                _mainControlSize = value;
            }
        }
        private Size _mainControlSize = new Size();
        public bool IsAddingReceive
        {
            get { return _isAddingReceive; }
            set
            {
                _isAddingReceive = value;
            }
        }
        private bool _isAddingReceive = false;
        public int fj
        {
            get { return _fj; }
            set { _fj = value; }
        }
        private int _fj = 0;
        public MouseState MouseState
        {
            get { return _mouseState; }
            set { _mouseState = value; }
        }
        private MouseState _mouseState = MouseState.None;
        public RectLayer SelectedLayer
        {
            get
            {
                return _selectedLayer;
            }
            set
            {
                _selectedLayer = value;
            }
        }
        private RectLayer _selectedLayer;
        public ObservableCollection<IRectElement> SelectedElement
        {
            get { return _selectedElment; }
            set { _selectedElment = value; }
        }
        private ObservableCollection<IRectElement> _selectedElment = new ObservableCollection<IRectElement>();

        public Dictionary<int, SelectedInfo> SelectedInfoList
        {
            get { return _selectedInfoList; }
            set
            {
                _selectedInfoList = value;
            }
        }
        private Dictionary<int, SelectedInfo> _selectedInfoList = new Dictionary<int, SelectedInfo>();

        public Dictionary<int, IRectElement> GroupframeList
        {
            get { return _groupframeList; }
            set { _groupframeList = value; }
        }
        private Dictionary<int, IRectElement> _groupframeList = new Dictionary<int, IRectElement>();

        public IRectElement CurrentRectElement
        {
            get { return _currentRectElement; }
            set { _currentRectElement = value; }
        }
        private IRectElement _currentRectElement;

        public SelectedLayerAndElement(RectLayer selectedLayer, ObservableCollection<IRectElement> selectedElement, IRectElement currentRectElement, Dictionary<int, SelectedInfo> selectedInfoList, Dictionary<int, IRectElement> groupframeList, MouseState MouseState)
        {
            _selectedLayer = selectedLayer;
            _selectedElment = selectedElement;
            _currentRectElement = currentRectElement;
            _selectedInfoList = selectedInfoList;
            _groupframeList = groupframeList;
            _mouseState = MouseState;
        }
        public SelectedLayerAndElement()
        {

        }

    }
    [Serializable]
    public class ScannerCofigInfo : NotificationST, ICloneable, ICopy
    {
        public ScannerSizeType ScanBdSizeType
        {
            get { return _scanBdSizeType; }
            set 
            { 
                _scanBdSizeType = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ScanBdSizeType));
            }
        }
        private ScannerSizeType _scanBdSizeType=ScannerSizeType.NoCustom;
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                _displayName = value;
                NotifyPropertyChanged(GetPropertyName(o => this.DisplayName));
            }
        }
        private string _displayName = "Unknown";

        public string StrChipType
        {
            get 
            {
                return _strChipType;
            }
            set 
            {
                _strChipType = value;
                NotifyPropertyChanged(GetPropertyName(o => this.StrChipType));
            }
        }
        private string _strChipType = "Unknown";

        public string StrCascadeType
        {
            get
            {
                return _strCascadeType;
            }
            set
            {
                _strCascadeType = value;
                NotifyPropertyChanged(GetPropertyName(o => this.StrCascadeType));
            }
        }
        private string _strCascadeType = "Unknown";

        public int DataGroup
        {
            get 
            {
                return _dataGroup;
            }
            set
            {
                _dataGroup = value;
                NotifyPropertyChanged(GetPropertyName(o => this.DataGroup));
            }
        }
        private int _dataGroup = 0;

        public string StrScanType
        {
            get 
            {
                if (ScanBdProp != null)
                {
                    return ScanBdProp.StandardLedModuleProp.ScanType.ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                _strScanType = value;
                NotifyPropertyChanged(GetPropertyName(o => StrScanType));
            }
        }
        private string _strScanType = "Unknown";

        public ScanBoardProperty ScanBdProp
        {
            get { return _scanBdProp; }
            set
            {
                _scanBdProp = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ScanBdProp));
            }
        }
        private ScanBoardProperty _scanBdProp = null;

        public bool CopyTo(object obj)
        {
            if (!(obj is ScannerCofigInfo))
            {
                return false;
            }

            ScannerCofigInfo info = (ScannerCofigInfo)obj;

            info.DataGroup = this.DataGroup;
            info.DisplayName = this.DisplayName;
            info.StrCascadeType = this.StrCascadeType;
            info.StrChipType = this.StrChipType;
            info.StrScanType = this.StrScanType;
            info.ScanBdSizeType = this.ScanBdSizeType;
            if (this.ScanBdProp != null)
            {
                if (info.ScanBdProp == null)
                {
                    info.ScanBdProp = new ScanBoardProperty();
                }
                this.ScanBdProp.CopyTo(info.ScanBdProp);
            }
            else
            {
                info.ScanBdProp = null;
            }

            return true;
        }

        public object Clone()
        {
            ScannerCofigInfo info = new ScannerCofigInfo();
            if (this.CopyTo(info))
            {
                return info;
            }
            return null;
        }
    }

    public class GlobalParameters : NotificationST
    {
        public ILCTServerBaseProxy ServerProxy
        {
            get { return _serverProxy; }
            set
            {
                _serverProxy = value;

            }
        }
        private ILCTServerBaseProxy _serverProxy = null;

        public string SelectPort = "";
        public SenderProperty SenderProp
        {
            get { return _senderProp; }
            set
            {
                _senderProp = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SenderProp));
            }
        }
        private SenderProperty _senderProp = new SenderProperty();
        public ScanBoardProperty ScanBdProp = new ScanBoardProperty();
        public Dictionary<string, List<ILEDDisplayInfo>> AllCommPortLedDisplayDic
        {
            get { return _allCommPortLedDisplayDic; }
            set
            {
                _allCommPortLedDisplayDic = value;
                NotifyPropertyChanged(GetPropertyName(o => this.AllCommPortLedDisplayDic));
            }
        }
        private Dictionary<string, List<ILEDDisplayInfo>> _allCommPortLedDisplayDic = null;

        public AllCOMHWBaseInfo AllBaseInfo
        {
            get
            {
                return _allBaseInfo;
            }
            set
            {
                _allBaseInfo = value;
                NotifyPropertyChanged(GetPropertyName(o => this.AllBaseInfo));
            }
        }
        private AllCOMHWBaseInfo _allBaseInfo = null;

        public List<SupperDisplay> SupperDisplayList
        {
            get
            {
                return _supperDisplayList;
            }
            set
            {
                _supperDisplayList = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SupperDisplayList));
            }
        }
        private List<SupperDisplay> _supperDisplayList = new List<SupperDisplay>();

        public Dictionary<string, List<SenderRedundancyInfo>> SenderReduInfoDic = null;
        public Dictionary<string, GraphicsDVIPortInfo> GraphicDviInfoDict = new Dictionary<string, GraphicsDVIPortInfo>();
        public ChipInherentProperty ChipInherentProp = new ChipInherentProperty();
        public bool IsDemoMode
        {
            get { return _isDemoMode; }
            set
            {
                _isDemoMode = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsDemoMode));
            }
        }
        private bool _isDemoMode = false;
        public ActionManager GlobalActionManager = new ActionManager();
        public Dictionary<int, ColorTempRGBMapping> ColorTempRGBMappingDict = null;
        /// <summary>
        /// 语言标志
        /// </summary>
        public string LangFlag
        {
            get { return _langFlag; }
            set
            {
                _langFlag = value;
                NotifyPropertyChanged(GetPropertyName(o => this.LangFlag));
            }
        }
        private string _langFlag = "zh-CN";

        /// <summary>
        /// 最近打开的文件（目前限制最多5个）
        /// </summary>
        public ObservableCollection<ProjectInfo> RecentProjectPaths
        {
            get { return _recentProjectPaths; }
            set
            {
                _recentProjectPaths = value;
                NotifyPropertyChanged(GetPropertyName(o => this.RecentProjectPaths));
            }
        }
        private ObservableCollection<ProjectInfo> _recentProjectPaths = new ObservableCollection<ProjectInfo>();

        /// <summary>
        /// 发送卡和网口的图片和颜色
        /// </summary>
        public Dictionary<int,SenderAndPortPicInfo> SenderAndPortPicCollection
        {
            get { return _senderAndPortPicCollection; }
            set
            {
                _senderAndPortPicCollection = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SenderAndPortPicCollection));
            }
        }
        private Dictionary<int,SenderAndPortPicInfo> _senderAndPortPicCollection = new Dictionary<int,SenderAndPortPicInfo>();

        public object SmartBrightManager = null; //类型SmartBright

        #region 发送卡配置文件数据

        public ObservableCollection<SenderConfigInfo> SenderConfigCollection
        {
            get 
            { 
                return _senderConfigCollection;
            }
            set 
            { 
                _senderConfigCollection = value;
                NotifyPropertyChanged(GetPropertyName(o => SenderConfigCollection));
            }
        }
        private ObservableCollection<SenderConfigInfo> _senderConfigCollection = new ObservableCollection<SenderConfigInfo>();
        #endregion

        #region 接收卡配置文件数据
        public List<string> OriginalScanFiles
        {
            get { return _originalScanFiles; }
            set
            {
                _originalScanFiles = value;
                NotifyPropertyChanged(GetPropertyName(o => OriginalScanFiles));
            }
        }
        private List<string> _originalScanFiles = new List<string>();

        public ObservableCollection<ScannerCofigInfo> ScannerConfigCollection
        {
            get { return _scannerConfigCollection; }
            set
            {
                _scannerConfigCollection = value;
                NotifyPropertyChanged(GetPropertyName(o => ScannerConfigCollection));
            }
        }
        private ObservableCollection<ScannerCofigInfo> _scannerConfigCollection = new ObservableCollection<ScannerCofigInfo>();

        #endregion

        #region 显示屏配置数据
        public List<ILEDDisplayInfo> CurrentDisplayInfoList = null;
        public bool IsSendCurrentDisplayConfig = false;
        #endregion

        #region 语言
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
        #endregion

        public AppConfiguration AppConfig
        {
            get;
            set;
        }
    }

    public class LCTConstData
    {
        public static readonly string GLOBAL_PARAMES_KEY = "GLOBAL_PARAMES_KEY";
        public static readonly int MAX_SENDER_COUNT = 10;
        public static readonly int MAX_PORT_COUNT = 4;
    }

    public class LangItemData : NotificationST
    {
        public string LangDisplayName
        {
            get { return _langDisplayName; }
            set
            {
                _langDisplayName = value;
                NotifyPropertyChanged(GetPropertyName(o => this.LangDisplayName));
            }
        }
        private string _langDisplayName = "";

        public string LangFlag
        {
            get { return _langFlag; }
            set
            {
                _langFlag = value;
                NotifyPropertyChanged(GetPropertyName(o => this.LangFlag));
            }
        }
        private string _langFlag = "";

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsSelected));
            }
        }
        private bool _isSelected = false;
    }

    [Serializable]
    public class SenderConfigInfo
    {
        public string SenderTypeName
        {
            get
            {
                return _senderTypeName;
            }
            set
            {
                _senderTypeName = value;
            }
        }
        private string _senderTypeName;
        public SenderProperty SenderProp
        {
            get
            {
                return _senderProp;
            }
            set
            {
                _senderProp = value;
            }
        }
        private SenderProperty _senderProp = null;
        public string SenderPicturePath
        {
            get
            {
                return _senderPicPath;
            }
            set
            {
                _senderPicPath = value;
            }
        }
        private string _senderPicPath = "";
        public int PortCount
        {
            get { return _portCount; }
            set { _portCount = value; }
        }
        private int _portCount = 0;
        public SenderConfigInfo() { }
        public SenderConfigInfo(string senderType, SenderProperty senderProp, string senderPicPath,int portCount)
        {
            _senderTypeName = senderType;
            _senderProp = senderProp;
            _senderPicPath = senderPicPath;
            _portCount = portCount;
        }
    }

    public class CustomReceiveResult
    {
        public bool? IsOK
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }
    }

    public class AppConfiguration
    {
        public string LangFlag
        {   get;
            set;
        }
        public AppConfiguration()
        {
            LangFlag = "en";
        }
    }
}
