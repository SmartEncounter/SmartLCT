using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Xml.Serialization;
using Nova.LCT.GigabitSystem.Common;

namespace Nova.SmartLCT.Interface
{
    /// <summary>
    /// 表示控件中的一个元素，所有控件元素继承自这个这个接口
    /// </summary>
    [XmlInclude(typeof(ScannerCofigInfo))]
    public abstract class IElement : INotifyPropertyChanged
    {
        /// <summary>
        /// 该元素是否被锁定
        /// </summary>
        public virtual bool IsLocked
        {
            get
            {
                return _isLocked;
            }
            set
            {
                _isLocked = value;
                //if (IsLocked)
                //{
                //    LockVisible = Visibility.Visible;
                //}
                //else
                //{
                //    LockVisible = Visibility.Hidden;
                //}
                NotifyPropertyChanged(GetPropertyName(o => this.IsLocked));
            }
        }
        protected bool _isLocked = false;
        /// <summary>
        /// 该元素是否可见
        /// </summary>
        public virtual Visibility Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
                NotifyPropertyChanged(GetPropertyName(o => this.Visible));
            }
        }
        protected Visibility _visible = Visibility.Visible;
        /// <summary>
        /// 元素的名称
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged(GetPropertyName(o => this.Name));
            }
        }
        private  string _name = "Design";
        /// <summary>
        /// 是否只显示轮廓
        /// </summary>
        public bool IsOutlineDisplay
        {
            get
            {
                return _isOutLineDisplay;
            }
            set
            {
                _isOutLineDisplay = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsOutlineDisplay));
            }
        }
        private bool _isOutLineDisplay = false;
        /// <summary>
        /// 轮廓线的画刷
        /// </summary>
        public Brush OutlineBrush
        {
            get
            {
                return _outlineBrush;
            }
            set
            {
                _outlineBrush = value;
                NotifyPropertyChanged(GetPropertyName(o => this.OutlineBrush));
            }
        }
        private Brush _outlineBrush = null;
        /// <summary>
        /// 背景画刷
        /// </summary>
        [XmlIgnore]
        public virtual Brush BackgroundBrush
        {
            get
            {
                return _backgroundBrush;
            }
            set
            {
                _backgroundBrush = value;
                NotifyPropertyChanged(GetPropertyName(o => this.BackgroundBrush));
            }
        }
        protected Brush _backgroundBrush = Brushes.Green;
        /// <summary>
        /// 父级元素
        /// </summary>
        [XmlIgnore]
        public virtual IElement ParentElement
        {
            get
            {
                return _parentElement;
            }
            set
            {
                _parentElement = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ParentElement));
            }
        }
        protected IElement _parentElement = null;
        /// <summary>
        /// 选中状态
        /// </summary>
        public virtual SelectedState ElementSelectedState
        {
            get
            {
                return _elementSelectedState;
            }
            set
            {
                _elementSelectedState = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ElementSelectedState));
            }
        }
        protected SelectedState _elementSelectedState = SelectedState.None;
        /// <summary>
        /// 透明度
        /// </summary>
        public double Opacity
        {
            get
            {
                return _opacity;
            }
            set
            {
                _opacity = value;
                NotifyPropertyChanged(GetPropertyName(o => this.Opacity));
            }
        }
        private double _opacity = 1.0;
        //public virtual Visibility LockVisible
        //{
        //    get
        //    {
        //        return _lockVisible;
        //    }
        //    set
        //    {
        //        if (value == _lockVisible)
        //        {
        //            return;
        //        }
        //        _lockVisible = value;
        //        NotifyPropertyChanged(GetPropertyName(o => this.LockVisible));
        //    }
        //}
        //protected Visibility _lockVisible = Visibility.Visible;
        /// <summary>
        /// 层的Z序
        /// </summary>
        public virtual int ZOrder
        {
            get
            {
                return _zOrder;
            }
            set
            {
                _zOrder = value;
                NotifyPropertyChanged(GetPropertyName(o => ZOrder));
            }
        }
        protected int _zOrder = 0;
        public object Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
                NotifyPropertyChanged(GetPropertyName(o => this.Tag));
            }
        }
        private object _tag = null;
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
        private string _displayName = "";

        public int GroupName
        {
            get
            {
                return _groupName;
            }
            set
            {
                _groupName = value;
                NotifyPropertyChanged(GetPropertyName(o => this.GroupName));
            }
        }
        private  int _groupName = -1;
        
        public virtual int ConnectedIndex
        {
            get
            {
                return _connectedIndex;
            }
            set
            {
                _connectedIndex = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ConnectedIndex));
            }
        }
        protected int _connectedIndex = -1;

        public Visibility AddressVisible
        {
            get
            {
                return _addressVisible;
            }
            set
            {
                _addressVisible = value;                
                NotifyPropertyChanged(GetPropertyName(o => this.AddressVisible));
            }
        }
        private Visibility _addressVisible = Visibility.Hidden;
        public Point Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                NotifyPropertyChanged(GetPropertyName(o => this.Position));
            }
        }
        private Point _position = new Point();
        public Size RectSize
        {
            get { return _rectSize; }
            set
            {
                _rectSize = value;
                NotifyPropertyChanged(GetPropertyName(o => this.RectSize));
            }
        }
        private Size _rectSize = new Size();
        public int ZIndex
        {
            get
            {
                return _zIndex;
            }
            set
            {
                _zIndex = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ZIndex));
            }
        }
        private int _zIndex = -1;
        public virtual  ElementType EleType
        {
            get
            {
                return _eleType;
            }
            set
            {
                _eleType = value;
                NotifyPropertyChanged(GetPropertyName(o => this.EleType));
            }
        }
        protected ElementType _eleType = ElementType.None;

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
}
