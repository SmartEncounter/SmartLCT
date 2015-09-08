using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;

namespace Nova.SmartLCT.Interface
{
    public abstract class IRectElement : IElement
    {
        public Visibility MaxConnectIndexVisibile
        {
            get { return _isMaxConnectIndexVisibile; }
            set
            {
                _isMaxConnectIndexVisibile = value;
                NotifyPropertyChanged(GetPropertyName(o => this.MaxConnectIndexVisibile));
            }
        }
        private Visibility _isMaxConnectIndexVisibile = Visibility.Hidden;
        public Visibility MinConnectIndexVisibile
        {
            get { return _isMinConnectIndexVisibile; }
            set
            {
                _isMinConnectIndexVisibile = value;
                NotifyPropertyChanged(GetPropertyName(o => this.MinConnectIndexVisibile));
            }
        }
        private Visibility _isMinConnectIndexVisibile = Visibility.Hidden;
        public virtual OperatEnvironment OperateEnviron
        {
            get { return _operateEnviron; }
            set
            {
                _operateEnviron = value;
                NotifyPropertyChanged(GetPropertyName(o => this.OperateEnviron));
            }
        }
        protected OperatEnvironment _operateEnviron = OperatEnvironment.DesignScreen;

        public int SenderIndex
        {
            get { return _senderIndex; }
            set
            {
                _senderIndex = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SenderIndex));
            }
        }
        private int _senderIndex = -1;

        public int PortIndex
        {
            get { return _portIndex; }
            set
            {
                _portIndex = value;
                NotifyPropertyChanged(GetPropertyName(o => this.PortIndex));
            }
        }
        private int _portIndex = -1;
        /// <summary>
        /// 层相对于父控件的起始X坐标
        /// </summary>
        public virtual double X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
                NotifyPropertyChanged(GetPropertyName(o => this.X));
            }
        }
        protected double _x = 0.0;
        /// <summary>
        /// 层相对于父控件的起始Y坐标
        /// </summary>
        public virtual double Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
                NotifyPropertyChanged(GetPropertyName(o => this.Y));
            }
        }
        protected double _y = 0.0;
        /// <summary>
        /// 层的宽度
        /// </summary>
        public virtual double Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                Size rectSize = RectSize;
                rectSize.Width = value;
                RectSize = rectSize;
                NotifyPropertyChanged(GetPropertyName(o => this.Width));
            }
        }
        protected double _width = 0;
        /// <summary>
        /// 层的高度
        /// </summary>
        public virtual double Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                Size rectSize = RectSize;
                rectSize.Height = value;
                RectSize = rectSize;
                NotifyPropertyChanged(GetPropertyName(o => this.Height));
            }
        }
        protected double _height = 0;
        /// <summary>
        /// 锁的位置
        /// </summary>
        public virtual Thickness StartAndEndIconMargin
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
        protected Thickness _startAndEndIconMargin = new Thickness();
        /// <summary>
        /// 元素位置
        /// </summary>
        public virtual Thickness Margin
        {
            get
            {
                return _margin;
            }
            set
            {
                _margin = value;
                NotifyPropertyChanged(GetPropertyName(o => this.Margin));
            }
        }
        protected Thickness _margin = new Thickness();
        /// <summary>
        /// 元素中心点位置
        /// </summary>
        public double CenterX
        {
            get
            {
                return _centerX;
            }
            set
            {
                if (value == _centerX)
                {
                    return;
                }
                _centerX = value;
                NotifyPropertyChanged(GetPropertyName(o => this.CenterX));
            }
        }
        protected double _centerX = 0;
        /// <summary>
        /// 元素中心点位置
        /// </summary>
        public double CenterY
        {
            get
            {
                return _centerY;
            }
            set
            {
                if (value == _centerY)
                {
                    return;
                }
                _centerY = value;
                NotifyPropertyChanged(GetPropertyName(o => this.CenterY));
            }
        }
        private double _centerY = 0;

        public ILineElement FrontLine
        {
            get
            {
                return _frontLine;
            }
            set
            {
                _frontLine = value;
               
                NotifyPropertyChanged(GetPropertyName(o => this.FrontLine));
            }
        }
        private ILineElement _frontLine = null;
        public ILineElement EndLine
        {
            get
            {
                return _endLine;
            }
            set
            {
                _endLine = value;
                NotifyPropertyChanged(GetPropertyName(o => this.EndLine));
            }
        }
        private ILineElement _endLine = null;
    }
}
