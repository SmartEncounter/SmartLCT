using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using Nova.SmartLCT.Interface;

namespace Nova.SmartLCT.UI
{
    public class MapLocationThumbNailPropertyPanel:Control
    {
        #region 定义xmal属性
        /// <summary>
        /// 控件的IsLockProperty属性值
        /// </summary>
        public static readonly DependencyProperty MapLocationRealParamsProperty =
            DependencyProperty.Register("MapLocationRealParams", typeof(MapLocationRealParameters), typeof(MapLocationThumbNailPropertyPanel),
            new FrameworkPropertyMetadata(new MapLocationRealParameters(), OnMapLocationRealParamsChanged));
        #endregion

        #region 属性
        public MapLocationRealParameters MapLocationRealParams
        {
            get { return (MapLocationRealParameters)GetValue(MapLocationRealParamsProperty); }
            set
            {
                SetValue(MapLocationRealParamsProperty, value);
            }
        }
       
        #endregion


        #region 构造函数
        static MapLocationThumbNailPropertyPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MapLocationThumbNailPropertyPanel), new FrameworkPropertyMetadata(typeof(MapLocationThumbNailPropertyPanel)));
        }
        #endregion

        #region 私有
        private static void OnMapLocationRealParamsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion
    }
}
