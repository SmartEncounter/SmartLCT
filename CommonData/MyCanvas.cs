using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Nova.SmartLCT.Interface
{
    public class MyCanvas:Canvas
    {
        #region 定义xmal属性
        /// <summary>
        /// 控件的IsLockProperty属性值
        /// </summary>
        public static readonly DependencyProperty EleTypeProperty =
            DependencyProperty.Register("EleType", typeof(ElementType), typeof(MyCanvas));
        public static readonly DependencyProperty OperateEnvironProperty =
            DependencyProperty.Register("OperateEnviron", typeof(OperatEnvironment), typeof(MyCanvas));
        #endregion
        #region 属性
        public ElementType EleType
        {
            get { return (ElementType)GetValue(EleTypeProperty); }
            set { SetValue(EleTypeProperty, value); }
        }
        public OperatEnvironment OperateEnviron
        {
            get { return (OperatEnvironment)GetValue(OperateEnvironProperty); }
            set { SetValue(OperateEnvironProperty, value); }
        }
        #endregion

        #region 构造函数
        static MyCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyCanvas), new FrameworkPropertyMetadata(typeof(MyCanvas)));
        }
        #endregion

    }
}
