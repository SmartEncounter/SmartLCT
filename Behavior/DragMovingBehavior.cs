using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Interactivity;
using Nova.SmartLCT.Interface;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;
using System.Diagnostics;
//using Microsoft.Expression.Interactivity.Core;

namespace Nova.SmartLCT.Behavior
{
    public class DragMovingBehavior : Behavior<FrameworkElement>
    {
        #region 定义xmal属性
        /// <summary>
        /// 控件的XProperty属性值
        /// </summary>
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(DragMovingBehavior),
            new FrameworkPropertyMetadata(0.0));
        /// <summary>
        /// 控件的YProperty属性值
        /// </summary>
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(DragMovingBehavior),
            new FrameworkPropertyMetadata(0.0));
        /// <summary>
        /// 控件的HeightProperty属性值
        /// </summary>
        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.Register("Height", typeof(double), typeof(DragMovingBehavior),
            new FrameworkPropertyMetadata(0.0));
        /// <summary>
        /// 控件的WidthProperty属性值
        /// </summary>
        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register("Width", typeof(double), typeof(DragMovingBehavior),
            new FrameworkPropertyMetadata(0.0));
        /// <summary>
        /// 控件的MarginProperty属性值
        /// </summary>
        public static readonly DependencyProperty MarginProperty =
            DependencyProperty.Register("Margin", typeof(Thickness), typeof(DragMovingBehavior));
        /// <summary>
        /// 控件的ParentElementProperty属性值
        /// </summary>
        public static readonly DependencyProperty ParentElementProperty =
            DependencyProperty.Register("ParentElement", typeof(IElement), typeof(DragMovingBehavior));
        /// <summary>
        /// 控件的ParentElementProperty属性值
        /// </summary>
        public static readonly DependencyProperty VisibleProperty =
            DependencyProperty.Register("Visible", typeof(Visibility), typeof(DragMovingBehavior));
        /// <summary>
        /// 控件的ParentElementProperty属性值
        /// </summary>
        public static readonly DependencyProperty ElementCollectionProperty =
            DependencyProperty.Register("ElementCollection", typeof(ObservableCollection<IElement>), typeof(DragMovingBehavior));
        /// <summary>
        /// 控件的ElementSelectedStateProperty属性值
        /// </summary>
        public static readonly DependencyProperty ElementSelectedStateProperty =
    DependencyProperty.Register("ElementSelectedState", typeof(SelectedState), typeof(DragMovingBehavior));
        /// <summary>
        /// 控件的ElementSelectedStateProperty属性值
        /// </summary>
        public static readonly DependencyProperty ZOrderProperty =
    DependencyProperty.Register("ZOrder", typeof(int), typeof(DragMovingBehavior));
        /// <summary>
        /// 控件的ElementSelectedStateProperty属性值
        /// </summary>
        public static readonly DependencyProperty BackgroundBrushProperty =
    DependencyProperty.Register("BackgroundBrush", typeof(Brush), typeof(DragMovingBehavior));
        /// <summary>
        /// 控件的ElementSelectedStateProperty属性值
        /// </summary>
        public static readonly DependencyProperty OpacityProperty =
    DependencyProperty.Register("Opacity", typeof(double), typeof(DragMovingBehavior));
        /// <summary>
        /// 控件的IsLockProperty属性值
        /// </summary>
        public static readonly DependencyProperty IsLockedProperty =
    DependencyProperty.Register("IsLocked", typeof(bool), typeof(DragMovingBehavior));
        /// <summary>
        /// 控件的IsLockProperty属性值
        /// </summary>
        public static readonly DependencyProperty MovingIconVisibleProperty =
    DependencyProperty.Register("MovingIconVisible", typeof(Visibility), typeof(DragMovingBehavior));
        /// <summary>
        /// 控件的IsLockProperty属性值
        /// </summary>
        public static readonly DependencyProperty EleTypeProperty =
    DependencyProperty.Register("EleType", typeof(ElementType), typeof(DragMovingBehavior));
        /// <summary>
        /// 控件的ElementPropetty属性值
        /// </summary>
        public static readonly DependencyProperty ElementProperty =
    DependencyProperty.Register("Element", typeof(IElement), typeof(DragMovingBehavior));
               /// <summary>
        /// 控件的ElementPropetty属性值
        /// </summary>
        public static readonly DependencyProperty AddressVisibleProperty =
    DependencyProperty.Register("AddressVisible", typeof(Visibility), typeof(DragMovingBehavior));
        #endregion

        #region 属性
        public Visibility AddressVisible
        {
            get { return (Visibility)GetValue(AddressVisibleProperty); }
            set
            {
                SetValue(AddressVisibleProperty, value);
            }
        }
        public IElement Element
        {
            get { return (IElement)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }
        public Thickness Margin
        {
            get { return (Thickness)GetValue(MarginProperty); }
            set { SetValue(MarginProperty, value); }
        }
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set
            {
                SetValue(XProperty, value);
            }
        }
        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set
            {
                SetValue(YProperty, value);
            }
        }
        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }
        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }
        public IElement ParentElement
        {
            get { return (IElement)GetValue(ParentElementProperty); }
            set { SetValue(ParentElementProperty, value); }
        }
        public Visibility Visible
        {
            get { return (Visibility)GetValue(VisibleProperty); }
            set { SetValue(VisibleProperty, value); }
        }
        public ObservableCollection<IElement> ElementCollection
        {
            get { return (ObservableCollection<IElement>)GetValue(ElementCollectionProperty); }
            set { SetValue(ElementCollectionProperty, value); }
        }
        public SelectedState ElementSelectedState
        {
            get
            {
                return (SelectedState)GetValue(ElementSelectedStateProperty);
            }
            set
            {
                SetValue(ElementSelectedStateProperty, value);
            }
        }
        public int ZOrder
        {
            get { return (int)GetValue(ZOrderProperty); }
            set { SetValue(ZOrderProperty, value); }
        }
        public Brush BackgroundBrush
        {
            get { return (Brush)GetValue(BackgroundBrushProperty); }
            set { SetValue(BackgroundBrushProperty, value); }
        }
        public double Opacity
        {
            get { return (double)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }
        public bool IsLocked
        {
            get { return (bool)GetValue(IsLockedProperty); }
            set { SetValue(IsLockedProperty, value); }
        }
        public ElementType EleType
        {
            get { return (ElementType )GetValue(EleTypeProperty); }
            set
            {
                SetValue(EleTypeProperty, value);
            }
        }
        #endregion

        #region 字段
        private Point _mousePoint = new Point();
        private bool _isMouseLeftButtonDown = false;
        private List<int> _zOrderList = new List<int>();
        private RectLayer old = new RectLayer();
        private bool _isMoving = false;
        private ObservableCollection<IElement> selectedElementss = new ObservableCollection<IElement>();

        #endregion

        #region 构造函数
        public DragMovingBehavior()
        {
            // 在此点下面插入创建对象所需的代码。

            //
            // 下面的代码行用于在命令
            // 与要调用的函数之间建立关系。如果您选择
            // 使用 MyFunction 和 MyCommand 的已注释掉的版本，而不是创建自己的实现，
            // 请取消注释以下行并添加对 Microsoft.Expression.Interactions 的引用。
            //
            // 文档将向您提供简单命令实现的示例，
            // 您可以使用该示例，而不是使用 ActionCommand 并引用 Interactions 程序集。
            //
            //this.MyCommand = new ActionCommand(this.MyFunction);
            //this.AssociatedObject

        }
        #endregion

        #region 重载
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseLeftButtonDown += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
            this.AssociatedObject.MouseMove += new MouseEventHandler(AssociatedObject_MouseMove);
            this.AssociatedObject.MouseUp += new MouseButtonEventHandler(AssociatedObject_MouseUp);
            this.AssociatedObject.MouseRightButtonDown+=new MouseButtonEventHandler(AssociatedObject_MouseRightButtonDown);
            this.AssociatedObject.MouseLeave+=new MouseEventHandler(AssociatedObject_MouseLeave);
            #region 注册消息
            Messenger.Default.Register<bool>(this, MsgToken.MSG_MOUSEUP, MouseUpChangedHandle);
            #endregion

            // 插入要在将 Behavior 附加到对象时运行的代码。
            //if (this.AssociatedType == typeof(Button))
            //{

            //}
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.MouseLeftButtonDown -= new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
            this.AssociatedObject.MouseMove -= new MouseEventHandler(AssociatedObject_MouseMove);
            this.AssociatedObject.MouseUp -= new MouseButtonEventHandler(AssociatedObject_MouseUp);
            this.AssociatedObject.MouseRightButtonDown -= new MouseButtonEventHandler(AssociatedObject_MouseRightButtonDown);
            this.AssociatedObject.MouseLeave -= new MouseEventHandler(AssociatedObject_MouseLeave);
            base.OnDetaching();

            Messenger.Default.Unregister<bool>(this, MsgToken.MSG_MOUSEUP);

            // 插入要在从对象中删除 Behavior 时运行的代码。
        }
        #endregion

        #region 控件事件   
        private void AssociatedObject_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ObservableCollection<IElement> selectedElements = new ObservableCollection<IElement>();
            if (sender is MyRectLayerControl)
            {
                #region 点击图层，取消该图层上所有元素的选中状态
                for (int elementIndex = 0; elementIndex < ElementCollection.Count; elementIndex++)
                {
                    ElementCollection[elementIndex].ElementSelectedState = SelectedState.None;
                }
                #endregion
                for (int elementIndex = 0; elementIndex < ElementCollection.Count; elementIndex++)
                {
                    if (!(ElementCollection[elementIndex] is RectElement))
                    {
                        continue;
                    }
                    if (((RectElement)ElementCollection[elementIndex]).ZOrder == -2)
                    {
                        #region 选中元素
                        RectElement rect1 = ((RectElement)ElementCollection[elementIndex]);
                        int index = 0;
                        for (int i = 0; i < ElementCollection.Count; i++)
                        {
                            RectElement rect2 = ((RectElement)ElementCollection[i]);

                            if (Function.IsRectIntersect(rect1, rect2))
                            {
                                index = index + 1;
                                ((RectElement)ElementCollection[i]).ElementSelectedState = SelectedState.Selected;
                                //if (index == 1)
                                //{
                                //    ((RectElement)ElementCollection[i]).ElementSelectedState = SelectedState.SpecialSelected;
                                //}
                            }
                        }
                        ElementCollection.RemoveAt(elementIndex);
                        #endregion
                    }
                }
            }
            else
            {
                #region 该图层上已经选中的元素
                for (int elementIndex = 0; elementIndex < ((RectLayer)ParentElement).ElementCollection.Count; elementIndex++)
                {
                    if (((RectLayer)ParentElement).ElementCollection[elementIndex].ElementSelectedState != SelectedState.None)
                    {
                        selectedElements.Add(((RectLayer)ParentElement).ElementCollection[elementIndex]);
                    }
                }
                #endregion

                #region 点击矩形的处理
                //if ((Keyboard.GetKeyStates(Key.LeftCtrl) == KeyStates.Down || (Keyboard.GetKeyStates(Key.LeftCtrl) == (KeyStates.Down | KeyStates.Toggled))) ||
                //    (Keyboard.GetKeyStates(Key.RightCtrl) == KeyStates.Down || (Keyboard.GetKeyStates(Key.RightCtrl) == (KeyStates.Down | KeyStates.Toggled))))
                //{
                //    #region 按ctrl键
                //    if (ElementSelectedState != SelectedState.None)
                //    {
                //        #region 选中已经被选中的
                //        if (ElementSelectedState == SelectedState.SpecialSelected)
                //        {
                //            for (int selectedElementIndex = 0; selectedElementIndex < selectedElements.Count; selectedElementIndex++)
                //            {
                //                int zorder = ((RectElement)(selectedElements[selectedElementIndex])).ZOrder;
                //                if (zorder != ZOrder)
                //                {
                //                    int index = ((RectLayer)ParentElement).ElementCollection.IndexOf(selectedElements[selectedElementIndex]);
                //                    ((RectLayer)ParentElement).ElementCollection[index].ElementSelectedState = SelectedState.SpecialSelected;
                //                    break;
                //                }
                //            }
                //        }
                //        ElementSelectedState = SelectedState.None;
                //        #endregion
                //    }
                //    else
                //    {
                //        #region 选中还没有被选中
                //        if (selectedElements.Count != 0)
                //        {
                //            ElementSelectedState = SelectedState.Selected;
                //        }
                //        else
                //        {
                //            ElementSelectedState = SelectedState.SpecialSelected;
                //        }
                //        #endregion
                //    }
                //    #endregion
                //}
                //else
                //{
                    #region 不按ctrl键
                    for (int selectedElementIndex = 0; selectedElementIndex < selectedElements.Count; selectedElementIndex++)
                    {
                        int index = ((RectLayer)ParentElement).ElementCollection.IndexOf(selectedElements[selectedElementIndex]);
                        if (ElementSelectedState == SelectedState.None)
                        {
                            ((RectLayer)ParentElement).ElementCollection[index].ElementSelectedState = SelectedState.None;
                        }
                        else
                        {
                            ((RectLayer)ParentElement).ElementCollection[index].ElementSelectedState = SelectedState.Selected;
                        }
                    }
                    ElementSelectedState = SelectedState.Selected;
                    #endregion
                //}
                #endregion
            }
            //_mousePoint = e.GetPosition(this.AssociatedObject);
            //_isMouseLeftButtonDown = true;
            Messenger.Default.Send<SelectedElementInfo>(new SelectedElementInfo((RectLayer)ParentElement, ZOrder), MsgToken.MSG_RIGHTBUTTONDOWN_CHANGED);
            e.Handled = true;
        }
        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
          //  Canvas.SetZIndex(this.AssociatedObject,ZOrder);
            IElement parent = ParentElement;
            if (parent == null && Element is RectLayer)
            {
                old = (RectLayer)((RectLayer)Element).Clone();
            }
            else
            {
                while (parent.ParentElement != null)
                {
                    parent = parent.ParentElement;
                }
                old = (RectLayer)((RectLayer)parent).Clone();
            }
            
            ObservableCollection<IElement> selectedElements = new ObservableCollection<IElement>();
            if (sender is MyRectLayerControl)
            {
                #region 点击图层，取消该图层上所有元素的选中状态
                for (int elementIndex = 0; elementIndex < ElementCollection.Count; elementIndex++)
                {
                    ElementCollection[elementIndex].ElementSelectedState = SelectedState.None;
                }
                #endregion
                for (int elementIndex = 0; elementIndex < ElementCollection.Count; elementIndex++)
                {
                    if (!(ElementCollection[elementIndex] is RectElement))
                    {
                        continue;
                    }
                    if (((RectElement)ElementCollection[elementIndex]).ZOrder == -2)
                    {
                        #region 选中元素
                        RectElement rect1 = ((RectElement)ElementCollection[elementIndex]);
                        int index = 0;
                        for (int i = 0; i < ElementCollection.Count; i++)
                        {
                            RectElement rect2 = ((RectElement)ElementCollection[i]);

                            if (Function.IsRectIntersect(rect1, rect2))
                            {
                                index = index + 1;
                                ((RectElement)ElementCollection[i]).ElementSelectedState = SelectedState.Selected;
                                //if (index == 1)
                                //{
                                //    ((RectElement)ElementCollection[i]).ElementSelectedState = SelectedState.SpecialSelected;
                                //}
                            }
                        }
                        ElementCollection.RemoveAt(elementIndex);
                        #endregion
                    }
                }
            }
            else
            {
                #region 该图层上已经选中的元素
                if (ParentElement != null)
                {

                    for (int elementIndex = 0; elementIndex < ((RectLayer)ParentElement).ElementCollection.Count; elementIndex++)
                    {
                        if (((RectLayer)ParentElement).ElementCollection[elementIndex].ElementSelectedState != SelectedState.None)
                        {
                            selectedElements.Add(((RectLayer)ParentElement).ElementCollection[elementIndex]);
                        }
                    }
                }
                #endregion

                    #region 点击矩形的处理
                    if ((Keyboard.GetKeyStates(Key.LeftCtrl) == KeyStates.Down || (Keyboard.GetKeyStates(Key.LeftCtrl) == (KeyStates.Down | KeyStates.Toggled))) ||
                        (Keyboard.GetKeyStates(Key.RightCtrl) == KeyStates.Down || (Keyboard.GetKeyStates(Key.RightCtrl) == (KeyStates.Down | KeyStates.Toggled))))
                    {
                        #region 按ctrl键
                        if (ElementSelectedState != SelectedState.None)
                        {
                            #region 选中已经被选中的
                            if (ElementSelectedState == SelectedState.SpecialSelected)
                            {
                                for (int selectedElementIndex = 0; selectedElementIndex < selectedElements.Count; selectedElementIndex++)
                                {
                                    int zorder = ((RectElement)(selectedElements[selectedElementIndex])).ZOrder;
                                    if (zorder != ZOrder)
                                    {
                                        int index = ((RectLayer)ParentElement).ElementCollection.IndexOf(selectedElements[selectedElementIndex]);
                                        ((RectLayer)ParentElement).ElementCollection[index].ElementSelectedState = SelectedState.SpecialSelected;
                                        break;
                                    }
                                }
                            }
                            ElementSelectedState = SelectedState.None;
                            #endregion
                        }
                        else
                        {
                            #region 选中还没有被选中
                            if (selectedElements.Count != 0)
                            {
                                ElementSelectedState = SelectedState.Selected;
                            }
                            else
                            {
                                ElementSelectedState = SelectedState.Selected;
                            }
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region 不按ctrl键
                        for (int selectedElementIndex = 0; selectedElementIndex < selectedElements.Count; selectedElementIndex++)
                        {
                            int index = ((RectLayer)ParentElement).ElementCollection.IndexOf(selectedElements[selectedElementIndex]);
                            if (ElementSelectedState == SelectedState.None)
                            {
                                ((RectLayer)ParentElement).ElementCollection[index].ElementSelectedState = SelectedState.None;
                            }
                            else
                            {
                                ((RectLayer)ParentElement).ElementCollection[index].ElementSelectedState = SelectedState.Selected;
                            }
                        }
                        ElementSelectedState = SelectedState.Selected;
                        #endregion
                    }
                
            #endregion
            }
            _mousePoint = e.GetPosition(this.AssociatedObject);
            _isMouseLeftButtonDown = true;
            Messenger.Default.Send<SelectedElementInfo>(new SelectedElementInfo((RectLayer)ParentElement,ZOrder), MsgToken.MSG_ZORDER_CHANGED);
            if (sender is MyRectangleControl)
            {
                #region 该图层上已经选中的元素
                selectedElementss.Clear();
                for (int elementIndex = 0; elementIndex < ((RectLayer)ParentElement).ElementCollection.Count; elementIndex++)
                {
                    if (!(((RectLayer)ParentElement).ElementCollection[elementIndex] is RectElement))
                    {
                        continue;
                    }
                    if (((RectLayer)ParentElement).ElementCollection[elementIndex].ElementSelectedState != SelectedState.None)
                    {
                        selectedElementss.Add(((RectLayer)ParentElement).ElementCollection[elementIndex]);
                    }
                }
                #endregion
            }
            e.Handled = true;
        }
        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            #region 当前矩形置于顶层
            if (sender is MyRectangleControl)
            {
                if (Element.ParentElement != null)
                {
                    for (int i = 0; i < ((RectLayer)Element.ParentElement).ElementCollection.Count; i++)
                    {
                        if (((RectLayer)Element.ParentElement).ElementCollection[i] is RectElement)
                        {
                            ((RectLayer)Element.ParentElement).ElementCollection[i].ZIndex = 0;
                        }
                        if (((RectLayer)Element.ParentElement).ElementCollection[i] is LineElement)
                        {
                            ((RectLayer)Element.ParentElement).ElementCollection[i].ZIndex = 2;
                        }
                    }
                }
                Element.ZIndex = 1;
                //Element.LockVisible = Visibility.Visible;
            }
            #endregion

            //ObservableCollection<IElement> selectedElements = new ObservableCollection<IElement>();

            List<IElement> selectedElementList = new List<IElement>();
            double minX = 0;
            double maxX = 0;
            double minY = 0;
            double maxY = 0;
            Point mousePoint = e.GetPosition(this.AssociatedObject);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.AssociatedObject.CaptureMouse();
            }
            else
            {
                this.AssociatedObject.ReleaseMouseCapture();
            }

            if (!_isMouseLeftButtonDown)
            {
                return;
            }
            if (IsLocked)
            {
                return;
            }
            if (sender is MyRectangleControl)
            {
                AddressVisible = Visibility.Visible;
            }
            if (sender is MyRectLayerControl && ElementCollection!=null && Element.EleType==ElementType.port)
            {
                #region 鼠标框选处理
                for (int elementIndex = 0; elementIndex < ElementCollection.Count; elementIndex++)
                {
                    if (!(ElementCollection[elementIndex] is RectElement))
                    {
                        continue;
                    }
                    if (((RectElement)ElementCollection[elementIndex]).ZOrder == -1)
                    {
                        ElementCollection.RemoveAt(elementIndex);

                    }
                    else if (((RectElement)ElementCollection[elementIndex]).ZOrder == -2)
                    {
                        #region 选中元素
                        RectElement rect1 = ((RectElement)ElementCollection[elementIndex]);
                        int index = 0;
                        for (int i = 0; i < ElementCollection.Count; i++)
                        {
                            RectElement rect2 = ((RectElement)ElementCollection[i]);

                            if (Function.IsRectIntersect(rect1, rect2))
                            {
                                index = index + 1;
                                ((RectElement)ElementCollection[i]).ElementSelectedState = SelectedState.Selected;
                                //if (index == 1)
                                //{
                                //    ((RectElement)ElementCollection[i]).ElementSelectedState = SelectedState.SpecialSelected;
                                //}
                            }
                        }
                        ElementCollection.RemoveAt(elementIndex);
                        #endregion
                        _isMouseLeftButtonDown = false;
                        return;
                    }
                }
                if (mousePoint.X < 0)
                {
                    mousePoint.X = 0;
                }
                if (mousePoint.Y < 0)
                {
                    mousePoint.Y = 0;
                }
                if (mousePoint.X > Width)
                {
                    mousePoint.X = Width;
                }
                if (mousePoint.Y > Height)
                {
                    mousePoint.Y = Height;
                }
                double height = Math.Abs(mousePoint.Y - _mousePoint.Y);
                double width = Math.Abs(mousePoint.X - _mousePoint.X );
                ElementCollection.Insert(0,new RectElement());
                RectElement rect = new RectElement(_mousePoint.X, _mousePoint.Y, width, height, ElementCollection[0].ParentElement, -1);

                if (mousePoint.X > _mousePoint.X && mousePoint.Y > _mousePoint.Y)
                {
                    rect.X = _mousePoint.X;
                    rect.Y = _mousePoint.Y;
                }
                else if (mousePoint.X > _mousePoint.X && mousePoint.Y < _mousePoint.Y)
                {
                    rect.X = _mousePoint.X;
                    rect.Y = mousePoint.Y;
                }
                else if (mousePoint.X < _mousePoint.X && mousePoint.Y > _mousePoint.Y)
                {
                    rect.X = mousePoint.X;
                    rect.Y = _mousePoint.Y;
                }
                else if (mousePoint.X < _mousePoint.X && mousePoint.Y < _mousePoint.Y)
                {
                    rect.X = mousePoint.X;
                    rect.Y = mousePoint.Y;
                }

                Thickness margin = new Thickness(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
                rect.Margin = margin;
                rect.BackgroundBrush = Brushes.Gray;
                rect.Opacity = 0.5;
                rect.ZIndex = 5;
                //rect.LockVisible = Visibility.Hidden;
                rect.MyLockAndVisibleButtonVisible = Visibility.Hidden;
                rect.ElementSelectedState = SelectedState.FrameSelected;
                ElementCollection.Add(rect);
                ElementCollection.RemoveAt(0);
                e.Handled = true;
                return;
                #endregion
            }
            if (sender is MyRectangleControl)
            {
                _isMoving = true;

                if (selectedElementss.Count == 0)
                {
                    e.Handled = true;
                    return;
                }
                #region 选中的元素的边界位置
                for (int i = 0; i < selectedElementss.Count; i++)
                {
                    selectedElementList.Add(selectedElementss[i]);
                }
                selectedElementList.Sort(delegate(IElement first, IElement second)
                {
                    return ((RectElement)first).X.CompareTo(((RectElement)second).X);
                });
                minX = ((RectElement)(selectedElementList[0])).X;
                selectedElementList.Sort(delegate(IElement first, IElement second)
                {
                    return (((RectElement)first).X + ((RectElement)first).Width).CompareTo(((RectElement)second).X + ((RectElement)second).Width);
                });
                maxX = ((RectElement)(selectedElementList[selectedElementList.Count - 1])).X + ((RectElement)(selectedElementList[selectedElementList.Count - 1])).Width;
                selectedElementList.Sort(delegate(IElement first, IElement second)
                {
                    return ((RectElement)first).Y.CompareTo(((RectElement)second).Y);
                });
                minY = ((RectElement)(selectedElementList[0])).Y;
                selectedElementList.Sort(delegate(IElement first, IElement second)
                {
                    return (((RectElement)first).Y + ((RectElement)first).Height).CompareTo(((RectElement)second).Y + ((RectElement)second).Height);
                });
                maxY = ((RectElement)(selectedElementList[selectedElementList.Count - 1])).Y + ((RectElement)(selectedElementList[selectedElementList.Count - 1])).Height;
                #endregion
                
            }
            #region 元素移动位置计算
            double controlMovingValueX = mousePoint.X - _mousePoint.X;
            double controlMovingValueY = mousePoint.Y - _mousePoint.Y;
            if (ParentElement.ParentElement != null)
            {
                if (minX + controlMovingValueX < 0)
                {
                    controlMovingValueX = -(minX);
                }
                else if (maxX + controlMovingValueX > ((RectLayer)ParentElement).Width)
                {
                    controlMovingValueX = ((RectLayer)ParentElement).Width - maxX;
                }
                if (minY + controlMovingValueY < 0)
                {
                    controlMovingValueY = -(minY);
                }
                else if (maxY + controlMovingValueY > ((RectLayer)ParentElement).Height)
                {
                    controlMovingValueY = ((RectLayer)ParentElement).Height - maxY;
                }
            }
            else
            {
                if (sender is MyRectangleControl)
                {
                    if (minX + controlMovingValueX < 0)
                    {
                        controlMovingValueX = -(minX);
                    }
                    if (minY + controlMovingValueY < 0)
                    {
                        controlMovingValueY = -(minY);
                    }

                }
            }
            #endregion
            #region 移动
            for (int selectedElementIndex = 0; selectedElementIndex < selectedElementss.Count; selectedElementIndex++)
            {
                Thickness margin = new Thickness();
                int index = ((RectLayer)ParentElement).ElementCollection.IndexOf(selectedElementss[selectedElementIndex]);
                //if (((RectLayer)ParentElement).ElementCollection[index].IsLocked)
                //{
                //    continue;
                //}
                margin = ((RectElement)(((RectLayer)ParentElement).ElementCollection[index])).Margin;
                margin.Left += controlMovingValueX;
                margin.Right -= controlMovingValueX;
                margin.Top += controlMovingValueY;
                margin.Bottom -= controlMovingValueY;
                ((RectElement)(((RectLayer)ParentElement).ElementCollection[index])).Margin = margin;
            }
            e.Handled = true;
            #endregion
        }
        public void AssociatedObject_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AddressVisible = Visibility.Hidden;

            if (_isMoving)
            {
                RectLayer newValue = new RectLayer();
                IElement parent = ParentElement;
                while (parent.ParentElement != null)
                {
                    parent = parent.ParentElement;
                }
                newValue = (RectLayer)((RectLayer)parent).Clone();
                PrePropertyChangedEventArgs actionValue = new PrePropertyChangedEventArgs() { PropertyName = "ElementCollection", OldValue = old, NewValue = ParentElement, ZOrder = ParentElement.ZOrder, ParentElement = ParentElement.ParentElement };
                Messenger.Default.Send<PrePropertyChangedEventArgs>(actionValue, MsgToken.MSG_RECORD_ACTIONVALUE);
            }
            _isMoving = false;


            _isMouseLeftButtonDown = false;
            if (sender is MyRectLayerControl)
            {
                for (int elementIndex = 0; elementIndex < ElementCollection.Count; elementIndex++)
                {
                    if (!(ElementCollection[elementIndex] is RectElement))
                    {
                        continue;
                    }
                    if (((RectElement)ElementCollection[elementIndex]).ZOrder == -1)
                    {
                        #region 选中框选的元素
                        RectElement rect1 = ((RectElement)ElementCollection[elementIndex]);
                        int index = 0;
                        for (int i = 0; i < ElementCollection.Count; i++)
                        {
                            if (!(ElementCollection[i] is RectElement))
                            {
                                continue;
                            }
                            RectElement rect2 = ((RectElement)ElementCollection[i]);

                            if (Function.IsRectIntersect(rect1, rect2))
                            {
                                index = index + 1;
                                ((RectElement)ElementCollection[i]).ElementSelectedState = SelectedState.Selected;
                                //if (index == 1)
                                //{
                                //    ((RectElement)ElementCollection[i]).ElementSelectedState = SelectedState.SpecialSelected;
                                //}
                            }
                        }
                        #endregion
                        ElementCollection.RemoveAt(elementIndex);
                    }
                }
            }
            else
            {
                if (ParentElement.IsLocked || ParentElement.ElementSelectedState == SelectedState.Selected)
                {
                    this.AssociatedObject.ReleaseMouseCapture();
                    e.Handled = true;
                    return;
                }
                if (ParentElement.ParentElement == null)
                {
                    this.AssociatedObject.ReleaseMouseCapture();
                    e.Handled = true;
                    return;
                }
                if (!_isMouseLeftButtonDown)
                {
                    for (int elementIndex = 0; elementIndex < ((RectLayer)ParentElement).ElementCollection.Count; elementIndex++)
                    {
                        if (((RectLayer)ParentElement).ElementCollection[elementIndex] is RectLayer)
                        {
                            continue;
                        }
                        if (((RectLayer)ParentElement).ElementCollection[elementIndex] is LineElement)
                        {
                            continue;
                        }
                        if (((RectElement)(((RectLayer)ParentElement).ElementCollection[elementIndex])).ZOrder == -1)
                        {
                            #region 选中框选的元素
                            RectElement rect1 = (RectElement)(((RectLayer)ParentElement).ElementCollection[elementIndex]);
                            int index = 0;
                            for (int i = 0; i < ((RectLayer)ParentElement).ElementCollection.Count; i++)
                            {
                                if (((RectLayer)ParentElement).ElementCollection[i] is LineElement)
                                {
                                    continue;
                                }
                                RectElement rect2 = (RectElement)(((RectLayer)ParentElement).ElementCollection[i]);
                                if (Function.IsRectIntersect(rect1, rect2))
                                {
                                    index = index + 1;
                                    (((RectLayer)ParentElement).ElementCollection[i]).ElementSelectedState = SelectedState.Selected;
                                    //if (index == 1)
                                    //{
                                    //    (((RectLayer)ParentElement).ElementCollection[i]).ElementSelectedState = SelectedState.SpecialSelected;
                                    //}
                                }
                            }
                            #endregion
                            ((RectLayer)ParentElement).ElementCollection.RemoveAt(elementIndex);
                            this.AssociatedObject.ReleaseMouseCapture();
                            e.Handled = true;
                            return;
                        }
                    }

                }
            }
            if (EleType != null && EleType == ElementType.screen)
            {

            }
            else
            {
                Messenger.Default.Send<SelectedElementInfo>(new SelectedElementInfo((RectLayer)ParentElement, ZOrder), MsgToken.MSG_ZORDER_CHANGED);
            }
            Messenger.Default.Send<bool>(false, MsgToken.MSG_MOUSEUP);
            this.AssociatedObject.ReleaseMouseCapture();
            e.Handled = true;
        }
        public void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
        {
            //if (Element.EleType == ElementType.receive)
            //{
            //    if (Element.IsLocked)
            //    {
            //        Element.LockVisible = Visibility.Visible;
            //    }
            //    else
            //    {
            //        Element.LockVisible = Visibility.Hidden;
            //    }
            //}
            e.Handled = true;
        }
        #endregion

        #region 方法
        private void MouseUpChangedHandle(bool isMouseUp)
        {
            _isMouseLeftButtonDown = false;
        }
        #endregion
        /*
        public ICommand MyCommand
        {
            get;
            private set;
        }
		 
        private void MyFunction()
        {
            // 插入要在从对象中删除 Behavior 时运行的代码。
        }
        */

    }
}