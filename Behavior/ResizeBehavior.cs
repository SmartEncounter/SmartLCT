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
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using Nova.SmartLCT.Interface;
using GalaSoft.MvvmLight.Messaging;
//using Microsoft.Expression.Interactivity.Core;

namespace Nova.SmartLCT.Behavior
{
	public class ResizeBehavior : Behavior<Rectangle>
	{
        /// <summary>
        /// 控件的ElementPropetty属性值
        /// </summary>
        public static readonly DependencyProperty ElementProperty =
    DependencyProperty.Register("Element", typeof(IElement), typeof(ResizeBehavior));
        /// <summary>
        /// 控件的ElementPropetty属性值
        /// </summary>
        public static readonly DependencyProperty RTProperty =
    DependencyProperty.Register("RT", typeof(ResizeType), typeof(ResizeBehavior));

        public IElement Element
        {
            get { return (IElement)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }
        public ResizeType RT
        {
            get { return (ResizeType)GetValue(RTProperty); }
            set { SetValue(RTProperty, value); }
        }

        private RectLayer old = new RectLayer();

		public ResizeBehavior()
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
            this.AssociatedObject.MouseLeftButtonDown += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
            this.AssociatedObject.MouseMove+=new MouseEventHandler(AssociatedObject_MouseMove);
            this.AssociatedObject.MouseUp+=new MouseButtonEventHandler(AssociatedObject_MouseUp);
            #region 注册消息
            Messenger.Default.Register<bool>(this, MsgToken.MSG_MOUSEUP, MouseUpChangedHandle);
            #endregion
        }

		protected override void OnDetaching()
		{
			base.OnDetaching();
            // 插入要在从对象中删除 Behavior 时运行的代码。
            this.AssociatedObject.MouseLeftButtonDown-=new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
            this.AssociatedObject.MouseMove -= new MouseEventHandler(AssociatedObject_MouseMove);
            this.AssociatedObject.MouseUp -= new MouseButtonEventHandler(AssociatedObject_MouseUp);
            #region 注销消息
            Messenger.Default.Unregister<bool>(this, MsgToken.MSG_MOUSEUP);
            #endregion
        }

        private bool _isMouseLeftDown = false;
        private Point _mouseStartPoint = new Point();
        private void AssociatedObject_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseLeftDown = false;
            Element.AddressVisible = Visibility.Hidden;
            this.AssociatedObject.ReleaseMouseCapture();
            RectLayer newValue = new RectLayer();
            IElement parent = Element.ParentElement;
            while (parent.ParentElement != null)
            {
                parent = parent.ParentElement;
            }
            newValue = (RectLayer)((RectLayer)parent).Clone();
            PrePropertyChangedEventArgs actionValue = new PrePropertyChangedEventArgs() { PropertyName = "ElementCollection", OldValue=old, NewValue=newValue };
            Messenger.Default.Send<PrePropertyChangedEventArgs>(actionValue, MsgToken.MSG_RECORD_ACTIONVALUE);

            e.Handled = true;
        }
        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {          
            if (!_isMouseLeftDown)
            {
                return;
            }
            Element.AddressVisible = Visibility.Visible;
            Point mousePoint = e.GetPosition(this.AssociatedObject);
         //   Console.WriteLine(mousePoint.ToString());
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.AssociatedObject.CaptureMouse();
            }
            else
            {
                this.AssociatedObject.ReleaseMouseCapture();
            }
            RectLayer layer = new RectLayer();
            layer = (RectLayer)Element;
            Point currentMousePoint = Mouse.GetPosition(this.AssociatedObject);
            double changedWidth = 0;
            double changedHeight = 0;
            double changedY = 0;
            double changedX = 0;
            double changedXValue = currentMousePoint.X - _mouseStartPoint.X;
            double changedYValue = currentMousePoint.Y - _mouseStartPoint.Y;
            #region 设置x坐标
            if (RT == ResizeType.CL || RT == ResizeType.TL || RT == ResizeType.BL)
            {
                if (RT == ResizeType.CL || RT == ResizeType.TL || RT == ResizeType.BL)
                {
                    changedX = layer.X + changedXValue;
                }
                if (changedX < 0)
                {
                    changedX = 0;
                    if (RT == ResizeType.TL || RT == ResizeType.BL)
                    {
                        changedXValue = -layer.X;                 
                    }
                    else
                    {
                        e.Handled = true;
                        return; 
                    }                                                            
                }
            }
            else
            {
                changedX = layer.X;
            }
            #endregion
            #region 设置y坐标
            if (RT == ResizeType.TC || RT == ResizeType.TL || RT == ResizeType.TR)
            {
                if (RT == ResizeType.TC || RT == ResizeType.TL || RT == ResizeType.TR)
                {
                    changedY = layer.Y + changedYValue;
                }
                if (changedY < 0)
                {
                    changedY = 0;
                    if (RT == ResizeType.TL || RT == ResizeType.TR)
                    {
                         changedYValue = -layer.Y;                    
                    }
                    else
                    {
                        e.Handled = true;
                        return;
                    }
                }
            }
            else
            {
                changedY = layer.Y;
            }
            #endregion
            #region 设置宽
            if (RT == ResizeType.CL || RT == ResizeType.CR || RT == ResizeType.TL || RT == ResizeType.TR || RT == ResizeType.BL || RT == ResizeType.BR)
            {
                if (RT == ResizeType.CL || RT == ResizeType.TL || RT == ResizeType.BL)
                {
                    changedWidth = layer.Width - changedXValue;
                }
                else if (RT == ResizeType.CR || RT == ResizeType.TR || RT == ResizeType.BR)
                {
                    changedWidth = layer.Width + changedXValue;
                }
                if (changedWidth < 10)
                {
                    changedWidth = 10;

                    if (layer.Width == 10)
                    {
                        changedX = layer.X;
                    }
                    else
                    {
                        if (RT == ResizeType.TR || RT == ResizeType.CR || RT == ResizeType.BR)
                        {
                            changedX = layer.X;
                        }
                        else
                        {
                            changedX = layer.X + (layer.Width - 10);
                        }
                        
                    }
                }
            }
            else
            {
                changedWidth = layer.Width;
            }
            #endregion
            #region 设置高
            if (RT == ResizeType.TC || RT == ResizeType.TL || RT == ResizeType.TR || RT == ResizeType.BC || RT == ResizeType.BL || RT == ResizeType.BR)
            {
                if (RT == ResizeType.TC || RT == ResizeType.TL || RT == ResizeType.TR)
                {
                    changedHeight = layer.Height - changedYValue;
                }
                else if (RT == ResizeType.BC || RT == ResizeType.BL || RT == ResizeType.BR)
                {
                    changedHeight = layer.Height + changedYValue;
                }
                if (changedHeight < 10)
                {
                    changedHeight = 10;

                    if (layer.Height == 10)
                    {
                        changedY = layer.Y;
                    }
                    else
                    {
                        if (RT == ResizeType.BL || RT == ResizeType.BC || RT == ResizeType.BR)
                        {
                            changedY = layer.Y;
                        }
                        else
                        {
                            changedY = layer.Y + (layer.Height - 10);
                        }                     
                    }
                }
            }
            else
            {
                changedHeight = layer.Height;
            }
            if (layer.X + layer.Width <= ((RectLayer)layer.ParentElement).Width)
            {
                if (changedX + changedWidth > ((RectLayer)layer.ParentElement).Width)
                {
                    changedWidth = ((RectLayer)layer.ParentElement).Width - layer.X;
                }
            }
            if (layer.Y + layer.Height <= ((RectLayer)layer.ParentElement).Height)
            {
                if (changedY + changedHeight > ((RectLayer)layer.ParentElement).Height)
                {
                    changedHeight = ((RectLayer)layer.ParentElement).Height - layer.Y;
                }
            }
            #endregion
            layer.X = changedX;
            layer.Y = changedY;   
            layer.Width = changedWidth;
            layer.Height = changedHeight;       
            e.Handled = true;
        }
        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IElement parent = Element.ParentElement;
            while (parent.ParentElement != null)
            {
                parent = parent.ParentElement;
            }
            old = (RectLayer)((RectLayer)parent).Clone();

            _isMouseLeftDown = true;
            _mouseStartPoint= Mouse.GetPosition(this.AssociatedObject);

            e.Handled = true;
        }
        private void MouseUpChangedHandle(bool isMouseUp)
        {
            _isMouseLeftDown = false;
            if (Element != null)
            {
                Element.AddressVisible = Visibility.Hidden;

            }
        }
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