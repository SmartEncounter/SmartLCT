using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Linq.Expressions;
using System.ComponentModel;
using GalaSoft.MvvmLight.Messaging;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.Windows.Documents;
using System.IO;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Nova.SmartLCT.Interface
{
    [Serializable]
    public class RectElement : IRectElement
    {
        #region 属性
        public override ElementType EleType
        {
            get
            {
                return base.EleType;
            }
            set
            {
                base.EleType = value;
                if (EleType != ElementType.receive)
                {
                    ConnectedVisible = Visibility.Hidden;
                }
            }
        }
        public Visibility ConnectedVisible
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ConnectedVisible));
            }
        }
        private Visibility _isConnected = Visibility.Visible;
        public override SelectedState ElementSelectedState
        {
            get
            {
                return base.ElementSelectedState;
            }
            set
            {
                base.ElementSelectedState = value;
                if (EleType == ElementType.receive)
                {
                    if (this.FrontLine != null)
                    {
                        if (value == SelectedState.None)
                        {
                            if (this.FrontLine.FrontElement!=null 
                                && this.FrontLine.FrontElement.ElementSelectedState != SelectedState.None)
                            {
                                this.FrontLine.ElementSelectedState = SelectedState.Selected;
                            }
                            else
                            {
                                this.FrontLine.ElementSelectedState = value;
                            }
                        }
                        else
                        {
                            this.FrontLine.ElementSelectedState = value;
                        }
                    }
                    if (this.EndLine != null)
                    {
                        if (value == SelectedState.None)
                        {
                            if (this.EndLine.EndElement != null 
                                && this.EndLine.EndElement.ElementSelectedState != SelectedState.None)
                            {
                                this.EndLine.ElementSelectedState = SelectedState.Selected;
                            }
                            else
                            {
                                this.EndLine.ElementSelectedState = value;
                            }
                        }
                        else
                        {
                            this.EndLine.ElementSelectedState = value;
                        }                  
                    }
                    
                }
                NotifyPropertyChanged(GetPropertyName(o => this.ElementSelectedState));

            }
        }

        public Visibility MyLockAndVisibleButtonVisible
        {
            get { return _myLockAndVisibleButtonVisible; }
            set
            {
                if (value == _myLockAndVisibleButtonVisible)
                {
                    return;
                }
                _myLockAndVisibleButtonVisible = value;
                NotifyPropertyChanged(GetPropertyName(o => this.MyLockAndVisibleButtonVisible));
            }
        }
        private Visibility _myLockAndVisibleButtonVisible = Visibility.Hidden;

        #region IRectElement 成员
        public override double Height
        {
            get
            {
                return base.Height;
            }
            set
            {
                base.Height = Math.Round(value);
                #region 更新带载区域信息
                if (ParentElement != null && ParentElement.EleType == ElementType.screen)
                {
                    CenterY = Y + Height / 2;
                    Function.UpdateSenderConnectInfo(((RectLayer)ParentElement).SenderConnectInfoList,this);
                    //for (int i = 0; i < ((RectLayer)ParentElement).SenderConnectInfoList.Count; i++)
                    //{
                    //    for (int j = 0; j < ((RectLayer)ParentElement).SenderConnectInfoList[i].PortConnectInfoList.Count; j++)
                    //    {
                    //        ((RectLayer)ParentElement).SenderConnectInfoList[i].PortConnectInfoList[j].LoadSize = Function.UnionRectCollection(((RectLayer)ParentElement).SenderConnectInfoList[i].PortConnectInfoList[j].ConnectLineElementList);
                    //    }
                    //    ((RectLayer)ParentElement).SenderConnectInfoList[i].LoadSize = Function.UnionRectCollection(((RectLayer)ParentElement).SenderConnectInfoList[i].PortConnectInfoList);
                    //}

                }
                #endregion
                StartAndEndIconMargin = new Thickness(Width / 2 - 10, Height / 2 - 10, Width / 2 + 10, Height / 2 + 10);
                NotifyPropertyChanged(GetPropertyName(o => this.Height));
            }
        }
        public override double Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                base.Width = Math.Round(value);
                #region 更新带载区域信息
                if (ParentElement != null && ParentElement.EleType == ElementType.screen && SenderIndex != -1 && PortIndex != -1)
                {
                    CenterX = X + Width / 2;
                    Function.UpdateSenderConnectInfo(((RectLayer)ParentElement).SenderConnectInfoList,this);

                    //for (int i = 0; i < ((RectLayer)ParentElement).SenderConnectInfoList.Count; i++)
                    //{
                    //    for (int j = 0; j < ((RectLayer)ParentElement).SenderConnectInfoList[i].PortConnectInfoList.Count; j++)
                    //    {
                    //        ((RectLayer)ParentElement).SenderConnectInfoList[i].PortConnectInfoList[j].LoadSize = Function.UnionRectCollection(((RectLayer)ParentElement).SenderConnectInfoList[i].PortConnectInfoList[j].ConnectLineElementList);
                    //    }
                    //    ((RectLayer)ParentElement).SenderConnectInfoList[i].LoadSize = Function.UnionRectCollection(((RectLayer)ParentElement).SenderConnectInfoList[i].PortConnectInfoList);
                    //}

                }
                #endregion
                StartAndEndIconMargin = new Thickness(Width / 2 - 10, Height / 2 - 10, Width / 2 + 10, Height / 2 + 10);
                NotifyPropertyChanged(GetPropertyName(o => this.Width));

            }
        }
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
                if (value < 0)
                {
                    _x = 0;
                    margin.Left = _x;
                    Margin = margin;
                    CenterX = _x + Width / 2;
                    NotifyPropertyChanged(GetPropertyName(o => this.X));
                    return;
                }
                if (_x == value)
                {
                    return;
                }

                if (ParentElement != null && ParentElement.EleType == ElementType.screen && ((RectLayer)ParentElement).OperateEnviron==OperatEnvironment.DesignScreen)
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
                if (ParentElement != null && ParentElement.EleType == ElementType.screen && 
                    (((RectLayer)ParentElement).OperateEnviron == OperatEnvironment.AdjustScreenLocation ||
                      ((RectLayer)ParentElement).OperateEnviron == OperatEnvironment.AdjustSenderLocation)
                    )
                {
                    if (value + Width > ((RectLayer)ParentElement).Width)
                    {
                        value = ((RectLayer)ParentElement).Width - Width;
                    }
                }
                if (value < 0)
                {
                    value = 0;
                }
                _x = value;
                margin.Left = _x;
                Margin = margin;
                CenterX = _x + Width / 2;
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
                if (value < 0)
                {
                    _y = 0;
                    margin.Top = _y;
                    Margin = margin;
                    CenterY = _y + Height / 2;
                    NotifyPropertyChanged(GetPropertyName(o => this.Y));
                    return;
                }
                if (value == _y)
                {
                    return;
                }
                if (ParentElement != null && ParentElement.EleType == ElementType.screen && ((RectLayer)ParentElement).OperateEnviron==OperatEnvironment.DesignScreen)
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
                if (ParentElement != null && ParentElement.EleType == ElementType.screen && 
                     (((RectLayer)ParentElement).OperateEnviron == OperatEnvironment.AdjustScreenLocation ||
                       ((RectLayer)ParentElement).OperateEnviron == OperatEnvironment.AdjustSenderLocation)
                    )
                {
                    if (value + Height > ((RectLayer)ParentElement).Height)
                    {
                        value = ((RectLayer)ParentElement).Height - Height;
                    }
                }
                if (value < 0)
                {
                    value = 0;
                }
                _y = value;
                margin.Top = _y;
                Margin = margin;
                CenterY = _y + Height / 2;
                NotifyPropertyChanged(GetPropertyName(o => this.Y));
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
                //List<int> zorderList = new List<int>();
                //if (ParentElement != null)
                //{
                //    for (int i = 0; i < ((RectLayer)ParentElement).ElementCollection.Count; i++)
                //    {
                //        zorderList.Add(((RectLayer)ParentElement).ElementCollection[i].ZOrder);
                //    }
                //    while (zorderList.Contains(value))
                //    {
                //        value = value + 1;
                //    }
                //}

                _zOrder = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ZOrder));
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
                #region 更新带载区域信息
                if (ParentElement != null && ParentElement.EleType == ElementType.screen && SenderIndex!=-1 && PortIndex!=-1 && ((IRectElement)ParentElement).OperateEnviron==OperatEnvironment.DesignScreen)
                {
                    Function.UpdateSenderConnectInfo(((RectLayer)ParentElement).SenderConnectInfoList, this);
                }
                #endregion

                NotifyPropertyChanged(GetPropertyName(o => this.Margin));
                //OnPrePropertyChangedEvent(this, new PrePropertyChangedEventArgs() { PropertyName = "Margin", OldValue = old, NewValue = _margin, ZOrder = ZOrder,ParentElement=ParentElement });
            }
        }
       
        #endregion
        #region IElement 成员
        
        /// <summary>
        /// 该元素是否被锁定
        /// </summary>
        public override bool IsLocked
        {
            get
            {
                return _isLocked;
            }
            set
            {
                _isLocked = value;            
                NotifyPropertyChanged(GetPropertyName(o => this.IsLocked));
            }
        }
        public override int ConnectedIndex
        {
            get { return _connectedIndex; }
            set
            {
                //if (value == _connectedIndex)
                //{
                //    return;
                //}
                int old = _connectedIndex;
                _connectedIndex = value;
                if (EleType==ElementType.receive && value >= 0)
                {
                    ConnectedVisible = Visibility.Hidden;
                }
                else if (EleType == ElementType.receive && value < 0)
                {
                    ConnectedVisible = Visibility.Visible;
                }
                else
                {
                    ConnectedVisible = Visibility.Hidden;
                }
                if (OperateEnviron != OperatEnvironment.DesignScreen || EleType!=ElementType.receive)
                {
                    NotifyPropertyChanged(GetPropertyName(o => this.ConnectedIndex));
                    return;
                }
                if (value == -2)
                {
                    NotifyPropertyChanged(GetPropertyName(o => this.ConnectedIndex));
                    return;
                }
                if (value == -1)
                {
                    if (FrontLine != null)
                    {
                        FrontLine.FrontElement.EndLine = null;
                        FrontLine = null;
                    }
                    if (EndLine != null)
                    {
                        EndLine.EndElement.FrontLine = null;
                        EndLine = null;
                    }
                    MaxConnectIndexVisibile = Visibility.Hidden;
                    MinConnectIndexVisibile = Visibility.Hidden;
                }
                else if (value >= 0)
                {
                    RectElement frontRectElement = new RectElement();
                    RectLayer screenLayer = (RectLayer)ParentElement;
                    PortConnectInfo portConnectInfo=new PortConnectInfo();
                    if(SenderIndex!=-1 && PortIndex!=-1)
                    {
                        portConnectInfo=screenLayer.SenderConnectInfoList[SenderIndex].PortConnectInfoList[PortIndex];
                    }
                    if (FrontLine == null && screenLayer!=null && old==-1)
                    {
                        frontRectElement = portConnectInfo.MaxConnectElement;
                        if (value != 0 && old==-1 && frontRectElement!=null)
                        {                   
                            LineElement line = new LineElement();
                            line.FrontElement = frontRectElement;
                            line.ZOrder = 0;
                            line.EndElement = this;
                            this.FrontLine = line;
                            frontRectElement.EndLine = line;
                            line.ParentElement = screenLayer;
                            string msgSender = "";
                            CommonStaticMethod.GetLanguageString("发送卡", "Lang_CommonData_RectLayer_Sender", out msgSender);
                            string msgPort = "";
                            CommonStaticMethod.GetLanguageString("网口", "Lang_CommonData_RectLayer_Port", out msgPort);

                            line.Name = msgSender +":"+ (SenderIndex + 1).ToString() + msgPort+" :" + (PortIndex + 1).ToString();
                            line.ZIndex = 7;
                            screenLayer.ElementCollection.Add(line);
                            frontRectElement.MaxConnectIndexVisibile = Visibility.Hidden;
                        }
                        if(value==0 || frontRectElement==null)
                        {
                            MinConnectIndexVisibile = Visibility.Visible;
                            MaxConnectIndexVisibile = Visibility.Hidden;
                        }
                        portConnectInfo.MaxConnectElement = this;
                        if (value != 0)
                        {
                            MaxConnectIndexVisibile = Visibility.Visible;
                        }
                        portConnectInfo.MaxConnectIndex += 1;
                        if (portConnectInfo.ConnectLineElementList == null)
                        {
                            portConnectInfo.ConnectLineElementList = new ObservableCollection<IRectElement>();
                            portConnectInfo.ConnectLineElementList = portConnectInfo.ConnectLineElementList;
                        }
                        if (!portConnectInfo.ConnectLineElementList.Contains(this))
                        {
                            if (portConnectInfo.MaxConnectIndex == this.ConnectedIndex)
                            {
                                portConnectInfo.ConnectLineElementList.Add(this);
                            }
                            else
                            {
                                List<IRectElement> connectLineList = portConnectInfo.ConnectLineElementList.ToList<IRectElement>();
                                connectLineList.Add(this);
                                connectLineList.Sort(delegate(IRectElement first,IRectElement second)
                                {
                                    return first.ConnectedIndex.CompareTo(second.ConnectedIndex);
                                });
                                int indexOfThis = connectLineList.IndexOf(this);
                                portConnectInfo.ConnectLineElementList.Insert(indexOfThis, this);
                            }
                        }
                    }
                    else if (FrontLine == null && screenLayer != null && old != -1 && old!=-2)
                    {
                        int frontIndex=-1;
                        if (portConnectInfo.ConnectLineElementList != null)
                        {
                            List<IRectElement> connectList = portConnectInfo.ConnectLineElementList.ToList<IRectElement>();
                            connectList.Sort(delegate(IRectElement first, IRectElement second)
                            {
                                return first.ConnectedIndex.CompareTo(second.ConnectedIndex);
                            });
                            frontIndex = connectList.IndexOf(this);


                            if (frontIndex - 2 >= 0)
                            {
                                frontRectElement = (RectElement)connectList[frontIndex - 2];
                                LineElement line = new LineElement();
                                line.FrontElement = frontRectElement;
                                line.ZOrder = 0;
                                line.EndElement = this;
                                this.FrontLine = line;
                                frontRectElement.EndLine = line;
                                line.ParentElement = screenLayer;
                                string msgSender = "";
                                CommonStaticMethod.GetLanguageString("发送卡", "Lang_CommonData_RectLayer_Sender", out msgSender);
                                string msgPort = "";
                                CommonStaticMethod.GetLanguageString("网口", "Lang_CommonData_RectLayer_Port", out msgPort);

                                line.Name = msgSender+":" + (SenderIndex + 1).ToString() + msgPort+" :" + (PortIndex + 1).ToString();
                                line.ZIndex = 2;
                                screenLayer.ElementCollection.Add(line);
                                frontRectElement.MaxConnectIndexVisibile = Visibility.Hidden;
                            }
                        }
                        if (frontIndex == 0)
                        {
                            MinConnectIndexVisibile = Visibility.Visible;
                            MaxConnectIndexVisibile = Visibility.Hidden;
                        }
                        else if (value == portConnectInfo.MaxConnectIndex)
                        {
                            MinConnectIndexVisibile = Visibility.Hidden;
                            MaxConnectIndexVisibile = Visibility.Visible;
                        }
                    }
                    else if (FrontLine == null && screenLayer != null && old == -2)
                    {
                        List<IRectElement> connectList = portConnectInfo.ConnectLineElementList.ToList<IRectElement>();
                        connectList.Sort(delegate(IRectElement first, IRectElement second)
                        {
                            return first.ConnectedIndex.CompareTo(second.ConnectedIndex);
                        });
                        int currentIndex = connectList.IndexOf(this);
                        if (currentIndex > 0)
                        {
                            frontRectElement = (RectElement)connectList[currentIndex - 1];
                            LineElement line = new LineElement();
                            line.FrontElement = frontRectElement;
                            line.ZOrder = 0;
                            line.EndElement = this;
                            this.FrontLine = line;
                            frontRectElement.EndLine = line;
                            line.ParentElement = screenLayer;
                            string msgSender = "";
                            CommonStaticMethod.GetLanguageString("发送卡", "Lang_CommonData_RectLayer_Sender", out msgSender);
                            string msgPort = "";
                            CommonStaticMethod.GetLanguageString("网口", "Lang_CommonData_RectLayer_Port", out msgPort);

                            line.Name = msgSender+":" + SenderIndex.ToString() + msgPort+" :" + PortIndex.ToString();
                            line.ZIndex = 2;
                            screenLayer.ElementCollection.Add(line);
                            frontRectElement.MaxConnectIndexVisibile = Visibility.Hidden;
                        }

                        if (currentIndex  == 0)
                        {
                            MinConnectIndexVisibile = Visibility.Visible;
                            MaxConnectIndexVisibile = Visibility.Hidden;
                        }
                        else if (value == portConnectInfo.MaxConnectIndex)
                        {
                            MinConnectIndexVisibile = Visibility.Hidden;
                            MaxConnectIndexVisibile = Visibility.Visible;
                        }
                    }
                    #region 更新带载区域信息
                    if (ParentElement != null && ParentElement.EleType == ElementType.screen)
                    {
                        Function.UpdateSenderConnectInfo(((RectLayer)ParentElement).SenderConnectInfoList,this);
                    }
                    #endregion
                }

                NotifyPropertyChanged(GetPropertyName(o => this.ConnectedIndex));
            }
        }
		
        #endregion
        #endregion

        #region 构造函数
        public RectElement() { }
        public RectElement(double x, double y, double width, double height, IElement parentElement, int zOrder)
        {
            Margin = new Thickness(x, y, (x + width), (y + height));

            Width = width;
            Height = height;

            ParentElement = parentElement;
            ZOrder = zOrder;
            CenterX = _x + width / 2;
            CenterY = _y + height / 2;
            StartAndEndIconMargin = new Thickness(Width / 2 - 10, Height / 2 - 10, Width / 2 + 10, Height / 2 + 10);
            IsLocked = true;
            MyLockAndVisibleButtonVisible = Visibility.Visible;
            AddressVisible = Visibility.Hidden;
            EleType = ElementType.receive;
        }
        public RectElement(double x, double y, double width, double height, IElement parentElement, int zOrder, ILineElement fronLine, ILineElement endLine,int flag)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Margin = new Thickness(x, y, (x + width), (y + height));
            ParentElement = parentElement;
            ZOrder = zOrder;
            CenterX = _x + width / 2;
            CenterY = _y + height / 2;
            StartAndEndIconMargin = new Thickness(Width / 2 - 10, Height / 2 - 10, Width / 2 + 10, Height / 2 + 10);
            FrontLine = fronLine;
            EndLine = endLine;
            //LockVisible = Visibility.Hidden;
            IsLocked = true;
            MyLockAndVisibleButtonVisible = Visibility.Visible;
            AddressVisible = Visibility.Hidden;
            EleType = ElementType.receive;
        }
        #endregion

        #region ICopy 成员
        public bool CopyTo(object obj)
        {
            if (!(obj is RectElement))
            {
                return false;
            }
            RectElement temp = (RectElement)obj;
            temp.BackgroundBrush = this.BackgroundBrush;
            temp.CenterX = this.CenterX;
            temp.CenterY = this.CenterY;
            temp.ElementSelectedState = this.ElementSelectedState;
            temp.Height = this.Height;
            temp.IsLocked = this.IsLocked;
            temp.IsOutlineDisplay = this.IsOutlineDisplay;
            temp.StartAndEndIconMargin = this.StartAndEndIconMargin;
            //temp.LockVisible = this.LockVisible;
            Thickness margin = new Thickness();
            margin.Left = this.Margin.Left;
            margin.Right = this.Margin.Right;
            margin.Bottom = this.Margin.Bottom;
            margin.Top = this.Margin.Top;
            temp.Margin = margin;
            temp.Opacity = this.Opacity;
            temp.OutlineBrush = this.OutlineBrush;
      //      temp.ParentElement = temp.ParentElement;
            temp.Visible = this.Visible;
            temp.Width = this.Width;
            temp.FrontLine = null;
            temp.EndLine = null;
            temp.X = this.X;
            temp.Y = this.Y;
            temp.ZOrder = this.ZOrder;
            temp.Tag = this.Tag;
            temp.Name = this.Name;
            temp.ConnectedIndex = this.ConnectedIndex;
            temp.EleType = this.EleType;
            temp.SenderIndex = this.SenderIndex;
            temp.PortIndex = this.PortIndex;
            temp.GroupName = this.GroupName;
            temp.MinConnectIndexVisibile = this.MinConnectIndexVisibile;
            temp.MaxConnectIndexVisibile = this.MaxConnectIndexVisibile;
            temp.RectSize = this.RectSize;
            temp.OperateEnviron = this.OperateEnviron;
            return true;
        }

        #endregion

        #region ICloneable 成员
        public object Clone()
        {
            RectElement newObj = new RectElement();
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

        public event PrePropertyChangedEventHandler PrePropertyChangedEvent;
        private void OnPrePropertyChangedEvent(object sender, PrePropertyChangedEventArgs e)
        {
            if (PrePropertyChangedEvent != null)
            {
                PrePropertyChangedEvent(sender, e);
            }
        }
    }
}
