using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Nova.SmartLCT.Interface
{

        public class MyTreeViewPortButton : Control
        {
            #region 定义xmal属性
            /// <summary>
            /// 控件的IsOverLoadProperty属性值
            /// </summary>
            public static readonly DependencyProperty IsOverLoadProperty =
                DependencyProperty.Register("IsOverLoad", typeof(bool), typeof(MyTreeViewPortButton));
            /// <summary>
            /// 控件的IsSelectedProperty属性值
            /// </summary>
            public static readonly DependencyProperty IsSelectedProperty =
                DependencyProperty.Register("IsSelected", typeof(bool), typeof(MyTreeViewPortButton));
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


            static MyTreeViewPortButton()
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(MyTreeViewPortButton), new FrameworkPropertyMetadata(typeof(MyTreeViewPortButton)));
            }

        }

}
