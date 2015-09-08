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
using GalaSoft.MvvmLight.Messaging;
//using Microsoft.Expression.Interactivity.Core;

namespace Nova.SmartLCT.Behavior
{
	public class RelayMovingIconBehavior : Behavior<Image>
    {
        #region 定义xmal属性
        /// <summary>
        /// 控件的ParentElementProperty属性值
        /// </summary>
        public static readonly DependencyProperty ParentElementProperty =
            DependencyProperty.Register("ParentElement", typeof(IElement), typeof(RelayMovingIconBehavior));
        /// <summary>
        /// 控件的ParentElementProperty属性值
        /// </summary>
        public static readonly DependencyProperty RelayZOrderProperty =
            DependencyProperty.Register("RelayZOrder", typeof(int), typeof(RelayMovingIconBehavior));

        public static readonly DependencyProperty AddressVisibleProperty =
    DependencyProperty.Register("AddressVisible", typeof(Visibility), typeof(RelayMovingIconBehavior));
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
        public IElement ParentElement
        {
            get { return (IElement)GetValue(ParentElementProperty); }
            set { SetValue(ParentElementProperty, value); }
        }
        public int RelayZOrder
        {
            get { return (int)GetValue(RelayZOrderProperty); }
            set { SetValue(RelayZOrderProperty, value); }
        }
        #endregion
        #region 字段
        private bool _isMouseLeftDown = false;
        private RectLayer _selectedLayer = new RectLayer();
        private Point _mousePoint = new Point();
        private RectLayer old = new RectLayer();
        private bool _isMoving = false;
        #endregion
        public RelayMovingIconBehavior()
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
		}

		protected override void OnAttached()
		{
			base.OnAttached();
            this.AssociatedObject.MouseLeftButtonDown+=new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
			this.AssociatedObject.MouseMove+=new MouseEventHandler(AssociatedObject_MouseMove);
            this.AssociatedObject.MouseLeftButtonUp+=new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonUp);
            Messenger.Default.Register<bool>(this, MsgToken.MSG_MOUSEUP,OnMouseUp);
            // 插入要在将 Behavior 附加到对象时运行的代码。
		}
        private void OnMouseUp(bool value)
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
            _isMouseLeftDown = false;
            this.AssociatedObject.ReleaseMouseCapture();
        }
		protected override void OnDetaching()
		{
			base.OnDetaching();
            this.AssociatedObject.MouseLeftButtonDown -= new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
            this.AssociatedObject.MouseMove -= new MouseEventHandler(AssociatedObject_MouseMove);
            this.AssociatedObject.MouseLeftButtonUp -= new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonUp);
            Messenger.Default.Unregister<bool>(this, MsgToken.MSG_MOUSEUP);
            // 插入要在从对象中删除 Behavior 时运行的代码。
        }
        #region 控件事件
        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ParentElement.IsLocked)
            {
                return;
            }
            IElement parent = ParentElement;
            while (parent.ParentElement != null)
            {
                parent = parent.ParentElement;
            }
            old = (RectLayer)((RectLayer)parent).Clone();
            _isMouseLeftDown = true;
            if (ParentElement is RectLayer)
            {
                for (int i = 0; i < ((RectLayer)ParentElement).ElementCollection.Count; i++)
                {
                    if (((RectLayer)ParentElement).ElementCollection[i].ZOrder == RelayZOrder)
                    {
                        if (((RectLayer)ParentElement).ElementCollection[i] is RectLayer)
                        {
                            _selectedLayer = (RectLayer)((RectLayer)ParentElement).ElementCollection[i];
                            break;
                        }
                    }
                }
            }
            _mousePoint = e.GetPosition(this.AssociatedObject);
            Messenger.Default.Send<SelectedElementInfo>(new SelectedElementInfo((RectLayer)ParentElement,_selectedLayer.ZOrder), MsgToken.MSG_ZORDER_CHANGED);

            e.Handled = true;
        }
        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {

           if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.AssociatedObject.CaptureMouse();
            }
            else
            {
                this.AssociatedObject.ReleaseMouseCapture();
            }
            if (!_isMouseLeftDown)
            {
                return;
            }
            if (_selectedLayer.IsLocked)
            {
                return;
            }
            AddressVisible = Visibility.Visible;

            #region 计算移动的距离
            Point mousePoint = e.GetPosition(this.AssociatedObject);
            double controlMovingValueX = mousePoint.X - _mousePoint.X;
            double controlMovingValueY = mousePoint.Y - _mousePoint.Y;
            #endregion
            #region 移动
            Thickness margin = new Thickness();
            margin = _selectedLayer.Margin;
            margin.Left += controlMovingValueX;
            margin.Right -= controlMovingValueX;
            margin.Top += controlMovingValueY;
            margin.Bottom -= controlMovingValueY;
            _selectedLayer.Margin = margin;
            _isMoving = true;
            #endregion

            e.Handled = true;
        }
        private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
            _isMouseLeftDown = false;
            this.AssociatedObject.ReleaseMouseCapture();
            e.Handled = false;
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