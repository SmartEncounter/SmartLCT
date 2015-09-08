using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Nova.SmartLCT.Interface
{
    public class MyMovingIcon:Control
    {
        #region 定义xmal属性

        #endregion
        #region 属性
        #endregion

         #region 构造函数
        static MyMovingIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyMovingIcon), new FrameworkPropertyMetadata(typeof(MyMovingIcon)));
        }
        #endregion
    }
}
