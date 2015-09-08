using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nova.SmartLCT.Interface;
using System.Windows;
using System.Windows.Controls;

namespace Nova.SmartLCT.SmartLCTControl
{
    class MyRectLayer:Control
    {
        #region 定义xmal属性
        public static readonly DependencyProperty ElementSelectedStateProperty =
            DependencyProperty.Register("ElementSelectedState", typeof(SelectedState), typeof(MyRectLayer));

        #endregion

        #region 属性
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


        #endregion
       #region 构造函数
        static MyRectLayer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRectLayer), new FrameworkPropertyMetadata(typeof(MyRectLayer)));
        }
        #endregion
    }
}
