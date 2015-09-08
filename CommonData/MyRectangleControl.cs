using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;


namespace Nova.SmartLCT.Interface
{
    public class MyRectangleControl:Control
    {
        #region 定义xmal属性
        public static readonly DependencyProperty ElementSelectedStateProperty =
            DependencyProperty.Register("ElementSelectedState", typeof(SelectedState), typeof(MyRectangleControl));
        public static readonly DependencyProperty EleTypeProperty =
           DependencyProperty.Register("EleType", typeof(ElementType), typeof(MyRectangleControl));
        public static readonly DependencyProperty OperateEnvironProperty =
           DependencyProperty.Register("OperateEnviron", typeof(OperatEnvironment), typeof(MyRectangleControl));
        #endregion

        #region 属性
        public OperatEnvironment OperateEnviron
        {
            get { return (OperatEnvironment)GetValue(OperateEnvironProperty); }
            set { SetValue(OperateEnvironProperty,value); }
        }
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
        static MyRectangleControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRectangleControl), new FrameworkPropertyMetadata(typeof(MyRectangleControl)));
        }
        #endregion
    }
    
}
