using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Collections.Specialized;
using GuiLabs.Undo;

namespace Nova.SmartLCT.Interface
{
    public abstract class IRectLayer : IRectElement
    {
        [XmlArrayItem(ElementName = "Item11", IsNullable = true, Type = typeof(RectElement)),
        XmlArrayItem(ElementName = "Item22", IsNullable = true, Type = typeof(RectLayer)),
        XmlArrayItem(ElementName = "Item33", IsNullable = true, Type = typeof(LineElement))]
        public virtual ObservableCollection<IElement> ElementCollection
        {
            get
            {
                return _elementCollection;
            }
            set
            {
                _elementCollection = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ElementCollection));
            }
        }
        protected ObservableCollection<IElement> _elementCollection = new ObservableCollection<IElement>();
        [XmlIgnore]
        public ActionManager MyActionManager
        {
            get { return _myActionManager; }
            set
            {
                _myActionManager = value;
                NotifyPropertyChanged(GetPropertyName(o => this.MyActionManager));
            }
        }
        private ActionManager _myActionManager = new ActionManager();
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
        public int CurrentSenderIndex
        {
            get { return _currentSenderIndex; }
            set
            {
                _currentSenderIndex = value;
                NotifyPropertyChanged(GetPropertyName(o => this.CurrentSenderIndex));
            }
        }
        private int _currentSenderIndex = -1;
        public int CurrentPortIndex
        {
            get { return _currentPortIndex; }
            set
            {
                _currentPortIndex = value;
                NotifyPropertyChanged(GetPropertyName(o => this.CurrentPortIndex));
            }
        }
        private int _currentPortIndex = -1;
        public int MaxZorder
        {
            get { return _maxZorder; }
            set
            {
                _maxZorder = value;
                NotifyPropertyChanged(GetPropertyName(o => this.MaxZorder));
            }
        }
        private int _maxZorder = -1;
        public int MaxGroupName
        {
            get { return _maxGroupName; }
            set
            {
                _maxGroupName = value;
                NotifyPropertyChanged(GetPropertyName(o => this.MaxGroupName));
            }
        }
        private int _maxGroupName = -1;

        [XmlIgnore]
        public virtual ObservableCollection<SenderConnectInfo> SenderConnectInfoList
        {
            get { return _senderConnectInfoList; }
            set
            {
                _senderConnectInfoList = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SenderConnectInfoList));
            }
        }
        protected ObservableCollection<SenderConnectInfo> _senderConnectInfoList = new ObservableCollection<SenderConnectInfo>();
     
        public ConnectLineType CLineType
        {
            get { return _cLineType; }
            set
            {
                _cLineType = value;
                NotifyPropertyChanged(GetPropertyName(o => this.CLineType));
            }
        }
        private ConnectLineType _cLineType = ConnectLineType.Auto;
        public Thickness BorderThickness
        {
            get
            {
                return _borderThickness;
            }
            set
            {
                _borderThickness = value;
                NotifyPropertyChanged(GetPropertyName(o => this.BorderThickness));
            }
        }
        private Thickness _borderThickness = new Thickness();

    }
}
