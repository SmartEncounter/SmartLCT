using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace Nova.SmartLCT.Interface
{
    public class MyTabControlHeader : Control
    {
        #region 定义xmal属性

        #endregion

        #region 属性

        #endregion

        #region 构造函数
        static MyTabControlHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyTabControlHeader), new FrameworkPropertyMetadata(typeof(MyTabControlHeader)));
        }
        #endregion
     
    }
}
