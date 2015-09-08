using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Nova.SmartLCT.Interface;
using GalaSoft.MvvmLight.Command;
using System.Collections.Specialized;

namespace Nova.SmartLCT.UI
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Nova.SmartLCT.UI.Themes"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Nova.SmartLCT.UI.Themes;assembly=Nova.SmartLCT.UI.Themes"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:ButtonCollectionControl/>
    ///
    /// </summary>
    public class ButtonCollectionControl : Control
    {
        #region xmal属性
        public static readonly DependencyProperty ParentWidthProperty =
            DependencyProperty.Register("ParentWidth", typeof(double), typeof(ButtonCollectionControl),
            new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnParentWidthChanged)));

        public static readonly DependencyProperty ButtonCollectionRealParamsProperty =
            DependencyProperty.Register("ButtonCollectionRealParams", typeof(ObservableCollection<ProductInfo>), typeof(ButtonCollectionControl),
            new FrameworkPropertyMetadata(new ObservableCollection<ProductInfo>(), new PropertyChangedCallback(OnButtonCollectionRealParamsChanged)));
       
        #endregion

        #region 属性
        public ObservableCollection<ProductInfo> ButtonCollectionRealParams
        {
            get { return (ObservableCollection<ProductInfo>)GetValue(ButtonCollectionRealParamsProperty); }
            set
            {
                if (ButtonCollectionRealParams != null)
                {
                    ButtonCollectionRealParams.CollectionChanged -= new NotifyCollectionChangedEventHandler(OnButtonCollectionChange);

                }
                SetValue(ButtonCollectionRealParamsProperty, value);
                if (ButtonCollectionRealParams != null)
                {
                    ButtonCollectionRealParams.CollectionChanged += new NotifyCollectionChangedEventHandler(OnButtonCollectionChange);

                }
            }
        }
        private void OnButtonCollectionChange(object sender, NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                #region 添加
                #endregion
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                #region 移除

                #endregion
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                #region 修改
                #endregion
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                #region 重置
                #endregion
            }
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {
                #region 移动
                //向前移动
                if (e.OldStartingIndex > e.NewStartingIndex)
                {
                    if (_currentProductIndex <= 0)
                    {
                        _currentProductIndex = ButtonCollectionRealParams.Count - 1;
                    }
                    else
                    {
                        _currentProductIndex -= 1;
                    }
                }
                //向后移动
                else if (e.OldStartingIndex < e.NewStartingIndex)
                {
                    if (_currentProductIndex >= ButtonCollectionRealParams.Count - 1)
                    {
                        _currentProductIndex = 0;
                    }
                    else
                    {
                        _currentProductIndex += 1;
                    }
                }
                SetSelectedButton(_currentProductIndex);
                #endregion

            }

        }
        public double ParentWidth
        {
            get { return (double)GetValue(ParentWidthProperty); }
            set { SetValue(ParentWidthProperty, value); }
        }
        #endregion

        #region 字段
        private Grid _myGrid=null;
        private int _butWidth = 8;
        private int _butHeight = 8;
        private int _butSpace = 5;
        private int _currentProductIndex = 1;
        #endregion

        #region 构造
        static ButtonCollectionControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonCollectionControl), new FrameworkPropertyMetadata(typeof(ButtonCollectionControl)));
        }

        #endregion

        #region 私有
        private static void OnButtonCollectionRealParamsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ButtonCollectionControl ctrl = (ButtonCollectionControl)d;
            ctrl.OnButtonCollection((ObservableCollection<ProductInfo>)e.NewValue);
        }
        private void OnButtonCollection(ObservableCollection<ProductInfo> productInfoCollection)
        {
            ButtonCollectionRealParams = ButtonCollectionRealParams;

            if (_myGrid == null || productInfoCollection == null ||
                productInfoCollection.Count == 0)
            {
                return;
            }
            this.Width = ParentWidth;
            this.Height = _butHeight;
            _myGrid.Width = productInfoCollection.Count * _butWidth + (productInfoCollection.Count - 1) * _butSpace;
            _myGrid.Height = _butHeight;
            _myGrid.HorizontalAlignment = HorizontalAlignment.Left;
            _myGrid.VerticalAlignment = VerticalAlignment.Bottom;
            Thickness myGridMargin = new Thickness();
            myGridMargin.Bottom = 0;
            if (ParentWidth != 0)
            {
                myGridMargin.Left = (ParentWidth - _myGrid.Width) / 2;              
            }
            _myGrid.Margin = myGridMargin;
            for (int itemIndex = 0; itemIndex < productInfoCollection.Count; itemIndex++)
            {
                Button but = new Button();
                but.Width = _butWidth;
                but.Height = _butHeight;
                but.HorizontalAlignment = HorizontalAlignment.Left;
                but.VerticalAlignment = VerticalAlignment.Top;
                Thickness margin = new Thickness();
                margin.Left = itemIndex * but.Width + itemIndex * _butSpace;
                margin.Top = 0;
                but.Margin = margin;
                but.Content = itemIndex;
                if (itemIndex == 1)
                {
                    but.Background = Function.StrToBrush("#FF2881DB");
                }
                else
                {
                    but.Background = Function.StrToBrush("#FFB1B1B1");
                }
                but.Foreground = Brushes.Transparent;
                but.SetResourceReference(Button.StyleProperty, "ButtonCollectionStyle");
                but.Command = new RelayCommand<Button>(OnButtonWithCmd);
                but.CommandParameter = but;
                _myGrid.Children.Add(but);
            }
            _currentProductIndex = 1;
        }
        private void OnButtonWithCmd(Button butInfo)
        {
            
            int selectedIndex=int.Parse(butInfo.Content.ToString());
            int currentProductIndex = _currentProductIndex;
            if(selectedIndex>currentProductIndex)
            {
                for(int itemIndex=0;itemIndex<(selectedIndex-currentProductIndex);itemIndex++)
                    ButtonCollectionRealParams.Move(0,ButtonCollectionRealParams.Count-1);
            }
            else if(selectedIndex<_currentProductIndex)
            {
                for(int itemIndex=0;itemIndex<(currentProductIndex-selectedIndex);itemIndex++)
                    ButtonCollectionRealParams.Move(ButtonCollectionRealParams.Count-1,0);
            }
        }
        private static void OnParentWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ButtonCollectionControl ctrl = (ButtonCollectionControl)d;
            ctrl.OnParentWidth((double)e.NewValue);
        }
        private void OnParentWidth(double size)
        {

        }
        private void SetSelectedButton(int index)
        {
            if (_myGrid == null)
            {
                return;
            }
            for (int itemIndex = 0; itemIndex < _myGrid.Children.Count; itemIndex++)
            {
                if (_myGrid.Children[itemIndex] is Button)
                {
                    Button but = _myGrid.Children[itemIndex] as Button;
                    if (but.Content.ToString() == index.ToString())
                    {
                        but.Background = Function.StrToBrush("#FF2881DB");
                    }
                    else
                    {
                        but.Background = Function.StrToBrush("#FFB1B1B1");
                    }
                }
            }
        }
        #endregion

        #region  重载
        public override void OnApplyTemplate()
        {
            var myGrid = this.GetTemplateChild("MyGrid") as Grid;
            _myGrid = myGrid;
            OnButtonCollection(ButtonCollectionRealParams);
        }
        #endregion

    }
}
