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
	public class SizeChangedToFillBehavior :SizeChangedBehavior
	{
		public SizeChangedToFillBehavior()
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

			// 插入要在将 Behavior 附加到对象时运行的代码。
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();

			// 插入要在从对象中删除 Behavior 时运行的代码。
		}
        protected override void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.AssociatedObject_MouseLeftButtonDown(sender, e);
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