using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Nova.SmartLCT.Interface
{
    public class WarnControl:Control
    {
                    #region 定义xmal属性
            /// <summary>
            /// 控件的IsLockProperty属性值
            /// </summary>
            public static readonly DependencyProperty IsOverLoadProperty =
                DependencyProperty.Register("IsOverLoad", typeof(bool), typeof(WarnControl), new FrameworkPropertyMetadata(false, MyRectLayerPropertyChangedCallBack));
            #endregion
            #region 属性
            public bool IsOverLoad
            {
                get { return (bool)GetValue(IsOverLoadProperty); }
                set { SetValue(IsOverLoadProperty, value); }
            }

            #endregion
            private static void MyRectLayerPropertyChangedCallBack(DependencyObject obj, DependencyPropertyChangedEventArgs e)
            {
                WarnControl ctrl = (WarnControl)obj;
                if (e.Property == IsOverLoadProperty)
                {
                    ctrl.OnRectLayerPropertyChanged();
                }
            }
            private void OnRectLayerPropertyChanged()
            {

            }


            static WarnControl()
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(WarnControl), new FrameworkPropertyMetadata(typeof(WarnControl)));
            }

    }
}
