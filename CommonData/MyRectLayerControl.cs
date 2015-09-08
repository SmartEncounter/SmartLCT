using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Nova.SmartLCT.Interface
{
    public class MyRectLayerControl:Control
    {
        #region 定义xmal属性
        public static readonly DependencyProperty ElementSelectedStateProperty =
            DependencyProperty.Register("ElementSelectedState", typeof(SelectedState), typeof(MyRectLayerControl));
        public static readonly DependencyProperty EleTypeProperty =
    DependencyProperty.Register("EleType", typeof(ElementType), typeof(MyRectLayerControl));

        #endregion

        #region 属性
        public ElementType EleType
        {
            get
            {
                return (ElementType)GetValue(EleTypeProperty);
            }
            set { SetValue(EleTypeProperty, value); }
        }
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
        static MyRectLayerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRectLayerControl), new FrameworkPropertyMetadata(typeof(MyRectLayerControl)));
        }
        #endregion
    }
}
