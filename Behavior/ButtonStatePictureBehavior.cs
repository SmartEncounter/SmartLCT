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
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;
//using Microsoft.Expression.Interactivity.Core;

namespace Nova.SmartLCT.Behavior
{
    public class ButtonStatePictureBehavior : Behavior<Button>
    {
        #region 定义xmal属性

        public static readonly DependencyProperty MouseOverImageProperty =
            DependencyProperty.Register("MouseOverImage", typeof(ImageBrush), typeof(ButtonStatePictureBehavior));
        public static readonly DependencyProperty DisableImageProperty =
            DependencyProperty.Register("DisableImage", typeof(ImageBrush), typeof(ButtonStatePictureBehavior));
        public static readonly DependencyProperty NormalImageProperty =
            DependencyProperty.Register("NormalImage", typeof(ImageBrush), typeof(ButtonStatePictureBehavior));
        #endregion

        #region 属性
        public ImageBrush MouseOverImage
        {
            get { return (ImageBrush)GetValue(MouseOverImageProperty); }
            set
            {
                SetValue(MouseOverImageProperty, value);
            }
        }
        public ImageBrush DisableImage
        {
            get { return (ImageBrush)GetValue(DisableImageProperty); }
            set
            {
                SetValue(DisableImageProperty, value);
            }
        }
        public ImageBrush NormalImage
        {
            get { return (ImageBrush)GetValue(NormalImageProperty); }
            set
            {
                SetValue(NormalImageProperty, value);
            }
        }
        #endregion

        #region 字段

        #endregion

        #region 构造函数
        public ButtonStatePictureBehavior()
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


            // 插入要在将 Behavior 附加到对象时运行的代码。
            //if (this.AssociatedType == typeof(Button))
            //{

            //}
            this.AssociatedObject.MouseEnter += new MouseEventHandler(AssociatedObject_MouseEnter);
            this.AssociatedObject.MouseLeave += new MouseEventHandler(AssociatedObject_MouseLeave);
            this.AssociatedObject.IsEnabledChanged += new DependencyPropertyChangedEventHandler(AssociatedObject_IsEnabledChanged);
        }

        protected override void OnDetaching()
        {

            base.OnDetaching();
            this.AssociatedObject.MouseEnter -= new MouseEventHandler(AssociatedObject_MouseEnter);
            this.AssociatedObject.MouseLeave -= new MouseEventHandler(AssociatedObject_MouseLeave);
            this.AssociatedObject.IsEnabledChanged -= new DependencyPropertyChangedEventHandler(AssociatedObject_IsEnabledChanged);

            // 插入要在从对象中删除 Behavior 时运行的代码。
        }
        #endregion

        #region 控件事件
        private void AssociatedObject_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.AssociatedObject.IsEnabled)
            {
                this.AssociatedObject.Background = MouseOverImage;
            }
            else
            {
                this.AssociatedObject.Background = DisableImage;
            }
        }

        private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.AssociatedObject.IsEnabled)
            {
                this.AssociatedObject.Background = NormalImage;
            }
            else
            {
                this.AssociatedObject.Background = DisableImage;
            }
        }

        private void AssociatedObject_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                this.AssociatedObject.Background = NormalImage;
            }
            else
            {
                this.AssociatedObject.Background = DisableImage;
            }
        }
        #endregion

        #region 方法
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