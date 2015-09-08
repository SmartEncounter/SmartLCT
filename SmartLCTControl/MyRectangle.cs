using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using Nova.SmartLCT.Interface;

namespace Nova.SmartLCT.SmartLCTControl
{
    class MyRectangle:Control
    {
        #region 定义xmal属性
        public static readonly DependencyProperty ElementSelectedStateProperty =
            DependencyProperty.Register("ElementSelectedState", typeof(SelectedState), typeof(MyRectangle));

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
        static MyRectangle()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRectangle), new FrameworkPropertyMetadata(typeof(MyRectangle)));
        }
        #endregion
    }
    
}
