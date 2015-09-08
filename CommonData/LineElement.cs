using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows;
using System.Linq.Expressions;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.IO;
using System.Reflection;

namespace Nova.SmartLCT.Interface
{
[Serializable]
    public class LineElement : ILineElement
    {

       
        public double Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                _angle = value;
                ArrowAngle = value + 90;
                NotifyPropertyChanged(GetPropertyName(o => this.Angle));
            }
        }
        private double _angle = 8;

        public double ArrowAngle
        {

            get { return _arrowAngle; }
            set 
            {
                _arrowAngle = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ArrowAngle));
            }
        }
        private double _arrowAngle = 0;
        
        /// <summary>
        /// 箭头的坐标
        /// </summary>
        public Thickness ArrowMargin
        {
            get { return _arrowMargin; }
            set
            {
                _arrowMargin = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ArrowMargin));
            }
        }
        private Thickness _arrowMargin = new Thickness();

        #region ILineElement 成员
        public override double EndX
        {
            get
            {
                return _endX;
            }
            set
            {
                if (value == _endX)
                {
                    return;
                }
                _endX = value;
                Width = Math.Sqrt(Math.Pow(value - X, 2) + Math.Pow(EndY - Y, 2));
                if (EndX < X)
                {
                    Angle = Math.Atan((EndY - Y) / (X - EndX));
                    Angle = 180 - Angle * 180 / Math.PI;
                }
                else if (EndX == X && EndY==Y)
                {
                    Angle = 0;
                }
                else
                {
                    Angle = Math.Atan((EndY - Y) / (EndX - X));
                    Angle = Angle * 180 / Math.PI;
                }
                NotifyPropertyChanged(GetPropertyName(o => this.EndX));

            }
        }
        
        public override double EndY
        {
            get
            {
                return _endY;
            }
            set
            {
                if (value == EndY)
                {
                    return;
                }
                _endY = value;
                Width = Math.Sqrt(Math.Pow(EndX - X, 2) + Math.Pow(value - Y, 2));
                if (EndX < X)
                {
                    Angle = Math.Atan((EndY - Y) / (X - EndX));
                    Angle = 180 - Angle * 180 / Math.PI;
                }
                else if (EndX == X && EndY == Y)
                {
                    Angle = 0;
                }
                else
                {
                    Angle = Math.Atan((EndY - Y) / (EndX - X));
                    Angle = Angle * 180 / Math.PI;
                }
                NotifyPropertyChanged(GetPropertyName(o => this.EndY));

            }
        }
        [XmlIgnore]
        public override IRectElement FrontElement
        {
            get { return _frontElement; }
            set
            {
                if (_frontElement != null)
                {
                    _frontElement.PropertyChanged -= new PropertyChangedEventHandler(_frontElement_PropertyChanged);

                }
                _frontElement = value;
                if (_frontElement.ParentElement != null)
                {
                    X = _frontElement.X + _frontElement.Width / 2;
                   
                }
                if (_frontElement.ParentElement != null)
                {
                   
                    Y = _frontElement.Y + _frontElement.Height / 2;
                }
                int colorIndex=4 * _frontElement.SenderIndex + _frontElement.PortIndex;
                if (SmartLCTViewModeBase.LINE_COLOR.Count() < colorIndex+1)
                {
                    colorIndex = 0;
                }
                BackgroundBrush = SmartLCTViewModeBase.LINE_COLOR[colorIndex];
                if (_frontElement != null)
                {
                    _frontElement.PropertyChanged += new PropertyChangedEventHandler(_frontElement_PropertyChanged);
      
                }
                NotifyPropertyChanged(GetPropertyName(o => this.FrontElement));
            }
        }
        void _frontElement_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IRectElement element = (IRectElement)sender;

            if (e.PropertyName == "CenterX")
            {
                this.X = element.CenterX;
                if (EndElement != null && EndElement.ParentElement != null)
                {
                    this.EndX = EndElement.X+EndElement.Width/2;
                }
            }
            else if (e.PropertyName == "CenterY")
            {
                this.Y = element.CenterY;
                if (EndElement != null && EndElement.ParentElement != null)
                {
                    this.EndY = EndElement.Y + EndElement.Height / 2;
                }
            }
            if (FrontElement.ElementSelectedState != SelectedState.None || EndElement.ElementSelectedState!=SelectedState.None)
            {
                Thickness arrowMargin = new Thickness();
                arrowMargin.Left = (EndX - X) / 2 - 8;
                arrowMargin.Top = (EndY - Y) / 2 - 8.5;
                ArrowMargin = arrowMargin;
            }
            else
            {
                Thickness arrowMargin = new Thickness();
                arrowMargin.Left = (EndX - X) / 2 - 8;
                arrowMargin.Top = (EndY - Y) / 2 - 8.5;
                ArrowMargin = arrowMargin;
            }
        }
        
        [XmlIgnore]
        public override IRectElement EndElement
        {
            get { return _endElement; }
            set 
            {
                if (_endElement != null)
                {
                    _endElement.PropertyChanged -= new PropertyChangedEventHandler(_endElement_PropertyChanged);

                }
                _endElement = value;
                if (_endElement.ParentElement != null)
                {
                     EndX = _endElement.X + _endElement.Width / 2;
                }
                if (_endElement.ParentElement != null)
                {
                     EndY = _endElement.Y + _endElement.Height / 2;
                }
                if (_endElement != null)
                {
                    _endElement.PropertyChanged += new PropertyChangedEventHandler(_endElement_PropertyChanged);
                }
                NotifyPropertyChanged(GetPropertyName(o => this.EndElement));
            }
        }
        void _endElement_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IRectElement element = (IRectElement)sender;

            if (e.PropertyName == "CenterX")
            {
                this.EndX = element.CenterX;
                if (FrontElement != null && FrontElement.ParentElement != null)
                {
                    this.X = FrontElement.X + FrontElement.Width / 2;
        
                }
            }
            else if (e.PropertyName == "CenterY")
            {
                this.EndY = element.CenterY;
                if (FrontElement != null && FrontElement.ParentElement != null)
                {
                    this.Y = FrontElement.Y + FrontElement.Height / 2;
                   
                }
            }
            else if (e.PropertyName == "ElementSelectedState")
            {
                if (EndElement.ElementSelectedState != SelectedState.None || FrontElement.ElementSelectedState!=SelectedState.None)
                {
                    Thickness arrowMargin = new Thickness();
                    arrowMargin.Left = (EndX - X) / 2 - 8;
                    arrowMargin.Top = (EndY - Y) / 2 - 8.5;
                    ArrowMargin = arrowMargin;
                }
                else
                {
                    Thickness arrowMargin = new Thickness();
                    arrowMargin.Left = (EndX - X) / 2 - 8;
                    arrowMargin.Top = (EndY - Y) / 2 - 8.5;
                    ArrowMargin = arrowMargin;
                }
            }
        }
        #endregion

        #region IRectElement 成员

        public Thickness StartAndEndIconMargin
        {
            get { return _startAndEndIconMargin; }
            set
            {
                _startAndEndIconMargin = value;
                NotifyPropertyChanged(GetPropertyName(o => this.StartAndEndIconMargin));
            }
        }
        private Thickness _startAndEndIconMargin = new Thickness();
        public double CenterX
        {
            get
            {
                return _centerX;
            }
            set
            {
                _centerX = value;
                NotifyPropertyChanged(GetPropertyName(o => this.CenterX));
            }
        }
        private double _centerX = 0;
        public double CenterY
        {
            get
            {
                return _centerY;
            }
            set
            {
                _centerY = value;
                NotifyPropertyChanged(GetPropertyName(o => this.CenterY));
            }
        }
        private double _centerY = 0;
        public double X
        {
            get
            {
                return _x;
            }
            set
            {
                if (value == _x)
                {
                    return;
                }
                _x = value;
                Thickness margin = Margin;
                margin.Left = value;
                Margin = margin;
                Width = Math.Sqrt(Math.Pow(EndX - value, 2) + Math.Pow(EndY - Y, 2));
                if (EndX < X)
                {
                    Angle = Math.Atan((EndY - Y) / (X - EndX));
                    Angle = 180 - Angle * 180 / Math.PI;
                }
                else if (EndX == X && EndY == Y)
                {
                    Angle = 0;
                }
                else
                {
                    Angle = Math.Atan((EndY - Y) / (EndX - X));
                    Angle = Angle * 180 / Math.PI;
                }
                NotifyPropertyChanged(GetPropertyName(o => this.X));
            }
        }
        private double _x = 0.0;
        public double Y
        {
            get
            {
                return _y;
            }
            set
            {
                if (value == _y)
                {
                    return;
                }
                _y = value;
                Thickness margin = Margin;
                margin.Top = value;
                Margin = margin;
                Width = Math.Sqrt(Math.Pow(EndX - X, 2) + Math.Pow(EndY - value, 2));
                if (EndX < X)
                {
                    Angle = Math.Atan((EndY - Y) / (X - EndX));
                    Angle = 180-Angle * 180 / Math.PI;
                }
                else if (EndX == X && EndY == Y)
                {
                    Angle = 0;
                }
                else
                {
                    Angle = Math.Atan((EndY - Y) / (EndX - X));
                    Angle = Angle * 180 / Math.PI;
                }
                NotifyPropertyChanged(GetPropertyName(o => this.Y));
            }
        }
        private double _y = 0;
        public double Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                Thickness arrowMargin = new Thickness();
                arrowMargin.Left = (EndX-X) / 2-8;
                arrowMargin.Top = (EndY-Y) / 2-8.5;
                ArrowMargin = arrowMargin;
                NotifyPropertyChanged(GetPropertyName(o => this.Width));
            }
        }
        private double _width = 0;
        public double Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                Thickness arrowMargin = new Thickness();
                arrowMargin.Left = (EndX - X) / 2-8;
                arrowMargin.Top = (EndY - Y) / 2-8.5;
                ArrowMargin = arrowMargin;
                NotifyPropertyChanged(GetPropertyName(o => this.Height));
            }
        }
        private double _height = 3;
        public override int ZOrder
        {
            get
            {
                return _zOrder;
            }
            set
            {
                List<int> zorderList = new List<int>();
                if (ParentElement != null)
                {
                    for (int i = 0; i < ((RectLayer)ParentElement).ElementCollection.Count; i++)
                    {
                        zorderList.Add(((RectLayer)ParentElement).ElementCollection[i].ZOrder);
                    }
                    while (zorderList.Contains(value))
                    {
                        value = value + 1;
                    }
                }
                _zOrder = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ZOrder));
            }
        }
        public Thickness Margin
        {
            get { return _margin; }
            set
            {
                if (value == _margin)
                {
                    return;
                }
                _margin = value;
                X = _margin.Left;
                Y = _margin.Top;
         
                NotifyPropertyChanged(GetPropertyName(o => this.Margin));
            }
        }
        private Thickness _margin = new Thickness();
        
        #endregion

        #region IElement 成员      
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
        #endregion

        #region 构造函数
        public LineElement() { }
        public LineElement(int zOrder,IRectElement frontElement,IRectElement endElement)
        {
            EndElement = endElement;
            FrontElement = frontElement;
            X = FrontElement.X + FrontElement.Width / 2;
          
            Y = FrontElement.Y + FrontElement.Height / 2;
           
            EndX = EndElement.X + EndElement.Width / 2;
            EndY = EndElement.Y + EndElement.Height / 2;
            Width = Math.Sqrt(Math.Pow(EndX - X, 2) + Math.Pow(EndY - Y, 2));
            Height = 3;
            ZOrder = zOrder;
            if (EndX < X)
            {
                Angle = Math.Atan((EndY - Y) / (X - EndX));
                Angle = 180 - Angle * 180 / Math.PI;
            }
            else
            {
                Angle = Math.Atan((EndY - Y) / (EndX - X));
                Angle = Angle * 180 / Math.PI;
            }
            Margin = new Thickness(X, Y, EndX, EndY);          
        }
        #endregion

        #region ICopy 成员
        public bool CopyTo(object obj)
        {
            if (!(obj is LineElement))
            {
                return false;
            }
            LineElement temp = (LineElement)obj;
            temp.BackgroundBrush = this.BackgroundBrush;
            temp.CenterX = this.CenterX;
            temp.CenterY = this.CenterY;
            temp.ElementSelectedState = this.ElementSelectedState;
            temp.Height = this.Height;
            temp.IsLocked = this.IsLocked;
            temp.IsOutlineDisplay = this.IsOutlineDisplay;
            temp.StartAndEndIconMargin = this.StartAndEndIconMargin;
            //temp.LockVisible = this.LockVisible;
            temp.Margin = this.Margin;
            temp.Opacity = this.Opacity;
            temp.OutlineBrush = this.OutlineBrush;
            temp.Visible = this.Visible;
            temp.Width = this.Width;
            temp.X = this.FrontElement.X + this.FrontElement.Width / 2;
            temp.Y = this.FrontElement.Y + this.FrontElement.Height / 2;
            temp.ZOrder = this.ZOrder;
            temp.EndX = this.EndElement.X+this.EndElement.Width/2;
            temp.EndY = this.EndElement.Y+this.EndElement.Height/2;
            temp.Angle = this.Angle;
            RectElement frontElement = new RectElement();
            RectElement endElement = new RectElement();
            ((RectElement)this.FrontElement).CopyTo(frontElement);
            ((RectElement)this.EndElement).CopyTo(endElement);
            temp.FrontElement = frontElement;
            temp.EndElement = endElement;
            temp.RectSize = this.RectSize;
           
            return true;
        }
        #endregion

        #region ICloneable 成员
        public object Clone()
        {
            LineElement newObj = new LineElement();
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
        #endregion

    }
}
