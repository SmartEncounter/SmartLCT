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
//using Microsoft.Expression.Interactivity.Core;

namespace Nova.SmartLCT.Behavior
{
	public class MouseMoveSelectRectBehavior : Behavior<Canvas>
	{
		public MouseMoveSelectRectBehavior()
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

        #region 字段
        //鼠标左键按下时的坐标,用于按下鼠标左键时画矩形框选滑块
        private Point _beginMousePoint = new Point();
        private bool _isMouseLeftDown = false;
        private Rectangle _rect = new Rectangle();
        #endregion

        protected override void OnAttached()
		{
			base.OnAttached();
            this.AssociatedObject.MouseLeftButtonDown+=new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
            this.AssociatedObject.MouseMove+=new MouseEventHandler(AssociatedObject_MouseMove);
            this.AssociatedObject.MouseUp+=new MouseButtonEventHandler(AssociatedObject_MouseUp);
            this.AssociatedObject.GotMouseCapture+=new MouseEventHandler(AssociatedObject_GotMouseCapture);

			// 插入要在将 Behavior 附加到对象时运行的代码。
		}

		protected override void OnDetaching()
		{
            this.AssociatedObject.MouseLeftButtonDown -= new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
            this.AssociatedObject.MouseMove -= new MouseEventHandler(AssociatedObject_MouseMove);
            this.AssociatedObject.MouseUp -= new MouseButtonEventHandler(AssociatedObject_MouseUp);
            this.AssociatedObject.GotMouseCapture -= new MouseEventHandler(AssociatedObject_GotMouseCapture);

			base.OnDetaching();

			// 插入要在从对象中删除 Behavior 时运行的代码。
        }

        #region 控件事件
        private void AssociatedObject_GotMouseCapture(object sender, MouseEventArgs e)
        {

        }
        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseLeftDown = true;
            _beginMousePoint = e.GetPosition(this.AssociatedObject);
            this.AssociatedObject.Children.Remove(_rect);
        }
        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {

        }
        private void AssociatedObject_MouseUp(object sender, MouseButtonEventArgs e)
        {

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