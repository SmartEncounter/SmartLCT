using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Nova.SmartLCT.Interface
{
    public class MyTreeViewSenderButton : Control
    {
        #region 定义xmal属性
        /// <summary>
        /// 控件的IsLockProperty属性值
        /// </summary>
        public static readonly DependencyProperty IsOverLoadProperty =
            DependencyProperty.Register("IsOverLoad", typeof(bool), typeof(MyTreeViewSenderButton));
        /// <summary>
        /// 控件的IsSelectedProperty属性值
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(MyTreeViewSenderButton));
        #endregion
        #region 属性
        public bool IsOverLoad
        {
            get { return (bool)GetValue(IsOverLoadProperty); }
            set { SetValue(IsOverLoadProperty, value); }
        }
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        #endregion


        static MyTreeViewSenderButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyTreeViewSenderButton), new FrameworkPropertyMetadata(typeof(MyTreeViewSenderButton)));
        }

    }

}
