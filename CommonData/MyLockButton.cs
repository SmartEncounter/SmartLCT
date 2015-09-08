using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Nova.SmartLCT.Interface
{
    public class MyLockButton:Control
    {
        #region 定义xmal属性
        /// <summary>
        /// 控件的IsLockProperty属性值
        /// </summary>
        public static readonly DependencyProperty IsLockedProperty =
            DependencyProperty.Register("IsLocked", typeof(bool), typeof(MyLockButton));
        #endregion
        #region 属性
        public bool IsLocked
        {
            get { return (bool)GetValue(IsLockedProperty); }
            set { SetValue(IsLockedProperty, value); }
        }
        #endregion

         #region 构造函数
        static MyLockButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyLockButton), new FrameworkPropertyMetadata(typeof(MyLockButton)));
        }
        #endregion
    }
}
