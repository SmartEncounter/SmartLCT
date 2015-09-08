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
	public class LockBehavior : Behavior<MyLockButton>
    {
        #region 定义xmal属性
        /// <summary>
        /// 控件的IsLockProperty属性值
        /// </summary>
        public static readonly DependencyProperty IsLockedProperty =
            DependencyProperty.Register("IsLocked", typeof(bool), typeof(LockBehavior));
        /// <summary>
        /// 控件的ParentElementProperty属性值
        /// </summary>
        public static readonly DependencyProperty ParentElementProperty =
            DependencyProperty.Register("ParentElement", typeof(IElement), typeof(LockBehavior));
        /// <summary>
        /// 控件的ParentElementProperty属性值
        /// </summary>
        public static readonly DependencyProperty LockZOrderProperty =
            DependencyProperty.Register("LockZOrder", typeof(int), typeof(LockBehavior));
        /// <summary>
        /// 控件的ElementPropetty属性值
        /// </summary>
        public static readonly DependencyProperty ElementProperty =
    DependencyProperty.Register("Element", typeof(IElement), typeof(LockBehavior));

        #endregion

        #region 属性
        public IElement Element
        {
            get { return (IElement)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }
        public bool IsLocked
        {
            get { return (bool)GetValue(IsLockedProperty); }
            set { SetValue(IsLockedProperty, value); }
        }
        public IElement ParentElement
        {
            get { return (IElement)GetValue(ParentElementProperty); }
            set { SetValue(ParentElementProperty, value); }
        }
        public int LockZOrder
        {
            get { return (int)GetValue(LockZOrderProperty); }
            set { SetValue(LockZOrderProperty, value); }
        }
        #endregion
        public LockBehavior()
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
            this.AssociatedObject.MouseMove += new MouseEventHandler(AssociatedObject_MouseMove);
            this.AssociatedObject.MouseLeave += new MouseEventHandler(AssociatedObject_MouseLeave);
            this.AssociatedObject.MouseLeftButtonDown += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
            // 插入要在将 Behavior 附加到对象时运行的代码。
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
            this.AssociatedObject.MouseMove -= new MouseEventHandler(AssociatedObject_MouseMove);
            this.AssociatedObject.MouseLeave -= new MouseEventHandler(AssociatedObject_MouseLeave);
            this.AssociatedObject.MouseLeftButtonDown -= new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
            // 插入要在从对象中删除 Behavior 时运行的代码。
        }
        #region 控件事件
        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            //if (Element.EleType == ElementType.receive)
            //{
            //    Element.LockVisible = Visibility.Visible;
            //}
        }
        private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
        {
            //if (Element.EleType == ElementType.receive)
            //{
            //    if (!IsLocked)
            //    {
            //        Element.LockVisible = Visibility.Hidden;
            //    }
            //    else
            //    {
            //        Element.LockVisible = Visibility.Visible;
            //    }
            //}
            //if (Element.EleType == ElementType.port)
            //{
            //    if (!IsLocked && Element.ElementSelectedState==SelectedState.None)
            //    {
            //        Element.LockVisible = Visibility.Hidden;
            //    }
            //}
            e.Handled = true;
        }
        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            IElement parent = ParentElement;
            RectLayer old = new RectLayer();
            while (parent.ParentElement != null)
            {
                parent = parent.ParentElement;
            }
            old = (RectLayer)((RectLayer)parent).Clone();

            if (ParentElement.IsLocked)
            {
                return;
            }
            IsLocked = !IsLocked;
            //if (IsLocked)
            //{
            //    Element.LockVisible = Visibility.Visible;
            //}
            RectLayer lockedLayer = new RectLayer();


            RectLayer newValue = new RectLayer();
            IElement newParent = ParentElement;
            while (newParent.ParentElement != null)
            {
                newParent = newParent.ParentElement;
            }
            newValue = (RectLayer)((RectLayer)newParent).Clone();
            PrePropertyChangedEventArgs actionValue = new PrePropertyChangedEventArgs() { PropertyName = "ElementCollection", OldValue = old, NewValue = ParentElement, ZOrder = ParentElement.ZOrder, ParentElement = ParentElement.ParentElement };
            Messenger.Default.Send<PrePropertyChangedEventArgs>(actionValue, MsgToken.MSG_RECORD_ACTIONVALUE);
            this.AssociatedObject.Focus();
            e.Handled = true;
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