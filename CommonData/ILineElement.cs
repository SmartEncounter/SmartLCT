using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Nova.SmartLCT.Interface
{
    public abstract class ILineElement: IElement
    {
        /// <summary>
        /// 层相对于父控件的终点X坐标
        /// </summary>
        public virtual double EndX
        {
            get
            {
                return _endX;
            }
            set
            {
                _endX = value;
                NotifyPropertyChanged(GetPropertyName(o => this.EndX));
            }
        }
        protected double _endX = 0;
        /// <summary>
        /// 层相对于父控件的终点Y坐标
        /// </summary>
        public virtual double EndY
        {
            get
            {
                return _endY;
            }
            set
            {
                _endY = value;
                NotifyPropertyChanged(GetPropertyName(o => this.EndY));
            }
        }
        protected double _endY = 0;
        /// <summary>
        /// 该线条元素连接的前一个元素
        /// </summary>
        [XmlIgnore]
        public virtual IRectElement FrontElement
        {
            get
            {
                return _frontElement;
            }
            set
            {
                _frontElement = value;
                NotifyPropertyChanged(GetPropertyName(o => this.FrontElement));
            }
        }
        protected IRectElement _frontElement = null;
        /// <summary>
        /// 该线条元素连接的后一个元素
        /// </summary>
        [XmlIgnore]
        public virtual IRectElement EndElement
        {
            get
            {
                return _endElement;
            }
            set
            {
                _endElement = value;
                NotifyPropertyChanged(GetPropertyName(o => this.EndElement));
            }
        }
        protected IRectElement _endElement = null;
    }
}
