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
using System.Diagnostics;
using Nova.LCT.GigabitSystem.Common;
using System.Collections.ObjectModel;

namespace Nova.SmartLCT.UI
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ScreenVirtualEditer"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ScreenVirtualEditer;assembly=ScreenVirtualEditer"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>

    [TemplatePartAttribute(Name = " PART_SCALEVIEW ", Type = typeof(Viewbox))]
    [TemplatePartAttribute(Name = " PART_SCALEGRID ", Type = typeof(Grid))]
    public class ScreenVirtualEditer : Control
    {
        #region Xaml属性
        private static readonly DependencyProperty FirstLightDataProperty =
           DependencyProperty.Register(
               "FirstLightData", typeof(VirtualEditData), typeof(ScreenVirtualEditer),
               new FrameworkPropertyMetadata(null,
                   new PropertyChangedCallback(OnFristLightDataChanged)
               )
           );
        private static readonly DependencyProperty SecondLightDataProperty =
           DependencyProperty.Register(
               "SecondLightData", typeof(VirtualEditData), typeof(ScreenVirtualEditer),
               new FrameworkPropertyMetadata(null,
                   new PropertyChangedCallback(OnSecondLightDataChanged)
               )
           );
        private static readonly DependencyProperty ThirdLightDataProperty =
           DependencyProperty.Register(
               "ThirdLightData", typeof(VirtualEditData), typeof(ScreenVirtualEditer),
               new FrameworkPropertyMetadata(null,
                   new PropertyChangedCallback(OnThirdLightDataChanged)
               )
           );
        private static readonly DependencyProperty FourthLightDataProperty =
           DependencyProperty.Register(
               "FourthLightData", typeof(VirtualEditData), typeof(ScreenVirtualEditer),
               new FrameworkPropertyMetadata(null,
                   new PropertyChangedCallback(OnFourthLightDataChanged)
               )
           );

        public static readonly DependencyProperty VirtualModeProperty =
          DependencyProperty.Register(
              "VirtualMode", typeof(VirtualModeType), typeof(ScreenVirtualEditer),
              new FrameworkPropertyMetadata(VirtualModeType.Led3,
                  new PropertyChangedCallback(OnVirtualModeChanged)
              )
          );

        private static readonly DependencyProperty VRedLightVisibilityProperty =
          DependencyProperty.Register(
              "VRedLightVisibility", typeof(Visibility), typeof(ScreenVirtualEditer),
              new FrameworkPropertyMetadata(Visibility.Collapsed,
                  new PropertyChangedCallback(OnVRedLightVisibilityChanged)
              )
          );

        private static readonly DependencyProperty RedLightVisibilityProperty =
          DependencyProperty.Register(
              "RedLightVisibility", typeof(Visibility), typeof(ScreenVirtualEditer),
              new FrameworkPropertyMetadata(Visibility.Visible,
                  new PropertyChangedCallback(OnRedLightVisibilityChanged)
              ));

        private static readonly DependencyProperty VirtualLightSequenceProperty =
          DependencyProperty.Register(
              "VirtualLightSequence", typeof(ObservableCollection<VirtualLightType>), typeof(ScreenVirtualEditer),
              new FrameworkPropertyMetadata(null,
                  new PropertyChangedCallback(OnVirtualLightSequenceChanged)
              ));

        #region 图片设置
        private static readonly DependencyProperty RedLightImageProperty =
          DependencyProperty.Register(
              "RedLightImage", typeof(BitmapImage), typeof(ScreenVirtualEditer),
              new FrameworkPropertyMetadata(null,
                  new PropertyChangedCallback(OnRedLightImageChanged)
              ));

        private static readonly DependencyProperty GreenLightImageProperty =
          DependencyProperty.Register(
              "GreenLightImage", typeof(BitmapImage), typeof(ScreenVirtualEditer),
              new FrameworkPropertyMetadata(null,
                  new PropertyChangedCallback(OnGreenLightImageChanged)
              ));

        private static readonly DependencyProperty BlueLightImageProperty =
          DependencyProperty.Register(
              "BlueLightImage", typeof(BitmapImage), typeof(ScreenVirtualEditer),
              new FrameworkPropertyMetadata(null,
                  new PropertyChangedCallback(OnBlueLightImageChanged)
              ));

        private static readonly DependencyProperty VRedLightImageProperty =
          DependencyProperty.Register(
              "VRedLightImage", typeof(BitmapImage), typeof(ScreenVirtualEditer),
              new FrameworkPropertyMetadata(null,
                  new PropertyChangedCallback(OnVRedLightImageChanged)
              ));
        #endregion

        public static readonly DependencyProperty IsEnableExchangeProperty =
          DependencyProperty.Register(
              "IsEnableExchange", typeof(bool), typeof(ScreenVirtualEditer),
              new FrameworkPropertyMetadata(false,
                  new PropertyChangedCallback(OnIsEnableExchangeChanged)
              ));
        public static readonly DependencyProperty IsReverseProperty =
          DependencyProperty.Register(
              "IsReverse", typeof(bool), typeof(ScreenVirtualEditer),
              new FrameworkPropertyMetadata(false,
                  new PropertyChangedCallback(OnIsReverseChanged)
              ));
        #endregion

        #region 属性
        private VirtualEditData FirstLightData
        {
            get { return (VirtualEditData)GetValue(FirstLightDataProperty); }
            set
            {
                SetValue(FirstLightDataProperty, value);
            }
        }

        private VirtualEditData SecondLightData
        {
            get { return (VirtualEditData)GetValue(SecondLightDataProperty); }
            set
            {
                SetValue(SecondLightDataProperty, value);
            }
        }

        private VirtualEditData ThirdLightData
        {
            get { return (VirtualEditData)GetValue(ThirdLightDataProperty); }
            set
            {
                SetValue(ThirdLightDataProperty, value);
            }
        }

        private VirtualEditData FourthLightData
        {
            get { return (VirtualEditData)GetValue(FourthLightDataProperty); }
            set
            {
                SetValue(FourthLightDataProperty, value);
            }
        }

        public VirtualModeType VirtualMode
        {
            get { return (VirtualModeType)GetValue(VirtualModeProperty); }
            set
            {
                SetValue(VirtualModeProperty, value);
            }
        }

        private Visibility VRedLightVisibility
        {
            get { return (Visibility)GetValue(VRedLightVisibilityProperty); }
            set
            {
                SetValue(VRedLightVisibilityProperty, value);
            }
        }

        private Visibility RedLightVisibility
        {
            get { return (Visibility)GetValue(RedLightVisibilityProperty); }
            set
            {
                SetValue(RedLightVisibilityProperty, value);
            }
        }

        public ObservableCollection<VirtualLightType> VirtualLightSequence
        {
            get { return (ObservableCollection<VirtualLightType>)GetValue(VirtualLightSequenceProperty); }
            set
            {
                SetValue(VirtualLightSequenceProperty, value);
            }
        }

        #region 图片
        public BitmapImage RedLightImage
        {
            get { return (BitmapImage)GetValue(RedLightImageProperty); }
            set
            {
                SetValue(RedLightImageProperty, value);
            }
        }
        public BitmapImage GreenLightImage
        {
            get { return (BitmapImage)GetValue(GreenLightImageProperty); }
            set
            {
                SetValue(GreenLightImageProperty, value);
            }
        }
        public BitmapImage BlueLightImage
        {
            get { return (BitmapImage)GetValue(BlueLightImageProperty); }
            set
            {
                SetValue(BlueLightImageProperty, value);
            }
        }
        public BitmapImage VRedLightImage
        {
            get { return (BitmapImage)GetValue(VRedLightImageProperty); }
            set
            {
                SetValue(VRedLightImageProperty, value);
            }
        }
        #endregion

        public bool IsEnableExchange
        {
            get { return (bool)GetValue(IsEnableExchangeProperty); }
            set
            {
                SetValue(IsEnableExchangeProperty, value);
            }
        }

        public bool IsReverse
        {
            get { return (bool)GetValue(IsReverseProperty); }
            set
            {
                SetValue(IsReverseProperty, value);
            }
        }
        #endregion

        #region 字段
        private Viewbox _viewBox = null;
        private Grid _scaleGrid = null;

        /// <summary>
        /// 拖动的区域
        /// </summary>
        private FrameworkElement _dragScope;
        /// <summary>
        /// 用于显示鼠标跟随效果的装饰器
        /// </summary>
        private DragAdorner _adorner;
        /// <summary>
        /// 用于呈现DragAdorner的图画
        /// </summary>
        private AdornerLayer _layer;
        #endregion

        #region 路由事件
        /// <summary>
        /// 属性变更
        /// </summary>
        public static readonly RoutedEvent LightSequenceChangedEvent = EventManager.RegisterRoutedEvent(
            "LightSequenceChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<ObservableCollection<VirtualLightType>>), typeof(ScreenVirtualEditer));

        /// <summary>
        /// Occurs when the Value property changes.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<ObservableCollection<VirtualLightType>> LightSequenceChanged
        {
            add { AddHandler(LightSequenceChangedEvent, value); }
            remove { RemoveHandler(LightSequenceChangedEvent, value); }
        }

        /// <summary>
        /// 属性变更
        /// </summary>
        public static readonly RoutedEvent LightSequenceResettedEvent = EventManager.RegisterRoutedEvent(
            "LightSequenceResetted", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(ScreenVirtualEditer));

        /// <summary>
        /// Occurs when the Value property changes.
        /// </summary>
        public event RoutedEventHandler LightSequenceResetted
        {
            add { AddHandler(LightSequenceResettedEvent, value); }
            remove { RemoveHandler(LightSequenceResettedEvent, value); }
        } 
        #endregion

        static ScreenVirtualEditer()
        {
            EventManager.RegisterClassHandler(typeof(ScreenVirtualEditer),
                UIElement.PreviewMouseMoveEvent, new MouseEventHandler(ScreenVirtualEditer.OnLightPreviewMouseDownEvent), true);

            EventManager.RegisterClassHandler(typeof(Image),
                UIElement.DropEvent, new DragEventHandler(ScreenVirtualEditer.OnLightDragEvent), true);


            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScreenVirtualEditer), new FrameworkPropertyMetadata(typeof(ScreenVirtualEditer)));
        }

        public ScreenVirtualEditer()
        {
            //ObservableCollection<VirtualLightType> squence = new ObservableCollection<VirtualLightType>();
            //squence.Add(VirtualLightType.Red);
            //squence.Add(VirtualLightType.Green);
            //squence.Add(VirtualLightType.Blue);
            //squence.Add(VirtualLightType.VRed);
            //VirtualLightSequence = squence;
        }

        #region 事件处理
        private static bool IsExchangedImage(Image img)
        {
            if (img.DataContext is VirtualEditData)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static void OnLightPreviewMouseDownEvent(object sender, MouseEventArgs e)
        {
            //Debug.WriteLine(sender.ToString() + ":" + e.OriginalSource.ToString());
            ScreenVirtualEditer ctrl = (ScreenVirtualEditer)sender;
            if (!ctrl.IsEnableExchange)
            {
                return;
            }

            if (!(e.OriginalSource is Image))
            {
                return;
            }

            
            Image img = (Image)e.OriginalSource;
            if (!IsExchangedImage(img))
            {
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ctrl.StartDrag(img, e);
            }
        }

        private void StartDrag(Image img, MouseEventArgs e)
        {
            this._dragScope = this;

            this._dragScope.AllowDrop = true;

            DragEventHandler draghandler = new DragEventHandler(DragScope_PreviewDragOver);
            this._dragScope.PreviewDragOver += draghandler;

            double scale = _viewBox.ActualHeight / _scaleGrid.ActualHeight;

            this._adorner = new DragAdorner(this._dragScope, img, 0.5, scale);
            this._layer = AdornerLayer.GetAdornerLayer(this._dragScope as Visual);
            this._layer.Add(this._adorner);

            DataObject data = new DataObject(typeof(Image), img);
            DragDrop.DoDragDrop(img, data, DragDropEffects.Move);

            AdornerLayer.GetAdornerLayer(this._dragScope).Remove(this._adorner);
            this._adorner = null;

            this._dragScope.PreviewDragOver -= draghandler;

            RefreshSequence();
        }

        private void DragScope_PreviewDragOver(object sender, DragEventArgs args)
        {
            if (this._adorner != null)
            {
                this._adorner.LeftOffset = args.GetPosition(this._dragScope).X;
                this._adorner.TopOffset = args.GetPosition(this._dragScope).Y;
                Debug.WriteLine("DragScope_PreviewDragOver:" + _adorner.LeftOffset + "," + _adorner.TopOffset);
            }
        }

        private static void OnLightDragEvent(object sender, DragEventArgs e)
        {
            Image exImg = (Image)sender;
            if (!IsExchangedImage(exImg))
            {
                return;
            }

            IDataObject data = e.Data;
            Image img = data.GetData(typeof(Image)) as Image;


            VirtualEditData first = (VirtualEditData)img.DataContext;
            VirtualEditData second = (VirtualEditData)exImg.DataContext;
            VirtualEditData temp = new VirtualEditData();
            first.CopyTo(temp);
            second.CopyTo(first);
            temp.CopyTo(second);
        }

        private void RefreshSequence()
        {
            if (VirtualLightSequence == null)
            {
                return;
            }
            ObservableCollection<VirtualLightType> temp = new ObservableCollection<VirtualLightType>();
            temp.Add(VirtualLightSequence[0]);
            temp.Add(VirtualLightSequence[1]);
            temp.Add(VirtualLightSequence[2]);
            temp.Add(VirtualLightSequence[3]);

            VirtualLightSequence[0] = FirstLightData.LightType;
            VirtualLightSequence[1] = SecondLightData.LightType;
            VirtualLightSequence[2] = ThirdLightData.LightType;
            VirtualLightSequence[3] = FourthLightData.LightType;
            ObservableCollection<VirtualLightType> old = VirtualLightSequence;
            RoutedPropertyChangedEventArgs<ObservableCollection<VirtualLightType>> e =
                new RoutedPropertyChangedEventArgs<ObservableCollection<VirtualLightType>>(temp, VirtualLightSequence, LightSequenceChangedEvent);

            //VirtualLightSequence = temp;

            RaiseEvent(e);
        }
        #endregion

        private static void OnFristLightDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //ScreenVirtualEditer ctrl = (ScreenVirtualEditer)d;
            //ctrl.TestD();
        }
        private static void OnSecondLightDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        private static void OnThirdLightDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        private static void OnFourthLightDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        private static void OnVRedLightVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        private static void OnRedLightVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        private static void OnIsEnableExchangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        private static void OnIsReverseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        #region 更新界面显示
        private static void OnVirtualModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScreenVirtualEditer ctrl = (ScreenVirtualEditer)d;
            VirtualModeType virtualMode = (VirtualModeType)e.NewValue;
            ctrl.OnVirtualModeSetted(virtualMode);

        }
        private void OnVirtualModeSetted(VirtualModeType virtualMode)
        {
            this.VRedLightVisibility = Visibility.Visible;
            this.RedLightVisibility = Visibility.Visible;
            if (virtualMode == VirtualModeType.Led3)
            {
                this.VRedLightVisibility = Visibility.Collapsed;
                
            }
            else if (virtualMode == VirtualModeType.Led31)
            {
                this.RedLightVisibility = Visibility.Collapsed;
            }
            ObservableCollection<VirtualLightType> sequence = this.VirtualLightSequence;
            ResetLightSquence(virtualMode, ref sequence);
            OnVirtualLightSequenceSetted(sequence);

        }
        private void ResetLightSquence(VirtualModeType virtualMode, ref ObservableCollection<VirtualLightType> virtualSequence)
        {
            if (virtualSequence == null)
            {
                return;
            }
            if (!IsReverse)
            {
                if (virtualMode == VirtualModeType.Led31)
                {
                    virtualSequence[0] = VirtualLightType.VRed;
                    virtualSequence[1] = VirtualLightType.Red;
                    virtualSequence[2] = VirtualLightType.Green;
                    virtualSequence[3] = VirtualLightType.Blue;
                }
                else if (VirtualMode == VirtualModeType.Led3)
                {
                    virtualSequence[0] = VirtualLightType.Red;
                    virtualSequence[1] = VirtualLightType.Green;
                    virtualSequence[2] = VirtualLightType.Blue;
                    virtualSequence[3] = VirtualLightType.VRed;
                }
                else
                {
                    virtualSequence[0] = VirtualLightType.Red;
                    virtualSequence[1] = VirtualLightType.Green;
                    virtualSequence[2] = VirtualLightType.Blue;
                    virtualSequence[3] = VirtualLightType.VRed;
                }
            }
            else
            {
                if (virtualMode == VirtualModeType.Led31)
                {
                    virtualSequence[0] = VirtualLightType.VRed;
                    virtualSequence[1] = VirtualLightType.Blue;
                    virtualSequence[2] = VirtualLightType.Red;
                    virtualSequence[3] = VirtualLightType.Green;
                }
                else if (VirtualMode == VirtualModeType.Led3)
                {
                    virtualSequence[0] = VirtualLightType.Green;
                    virtualSequence[1] = VirtualLightType.Blue;
                    virtualSequence[2] = VirtualLightType.Red;
                    virtualSequence[3] = VirtualLightType.VRed;
                }
                else
                {
                    virtualSequence[0] = VirtualLightType.Red;
                    virtualSequence[1] = VirtualLightType.Green;
                    virtualSequence[2] = VirtualLightType.Blue;
                    virtualSequence[3] = VirtualLightType.VRed;
                }
            }

            RoutedEventArgs e = new RoutedEventArgs(LightSequenceResettedEvent);
            RaiseEvent(e);
        }

        private static void OnVirtualLightSequenceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScreenVirtualEditer ctrl = (ScreenVirtualEditer)d;
            ObservableCollection<VirtualLightType> temp = (ObservableCollection<VirtualLightType>)e.NewValue;
            ctrl.OnVirtualLightSequenceSetted(temp);
        }
        private void OnVirtualLightSequenceSetted(ObservableCollection<VirtualLightType> virtualSequence)
        {
            if (virtualSequence == null)
            {
                return;
            }
            FirstLightData = SetDataToImage(FirstLightData, virtualSequence[0], GetSpecImage(virtualSequence[0]));
            SecondLightData = SetDataToImage(SecondLightData, virtualSequence[1], GetSpecImage(virtualSequence[1]));
            ThirdLightData = SetDataToImage(ThirdLightData, virtualSequence[2], GetSpecImage(virtualSequence[2]));
            FourthLightData = SetDataToImage(FourthLightData, virtualSequence[3], GetSpecImage(virtualSequence[3]));

        }

        private VirtualEditData SetDataToImage(VirtualEditData lastData, VirtualLightType lightType, BitmapImage img)
        {
            VirtualEditData data = null;
            if (lastData == null)
            {
                data = new VirtualEditData();
            }
            else
            {
                data = lastData;
            }
            data.LightType = lightType;
            data.ImageDisplay = img;

            return data;
        }

        private BitmapImage GetSpecImage(VirtualLightType lightType)
        {
            switch (lightType)
            {
                case VirtualLightType.Red: return RedLightImage;
                case VirtualLightType.Green: return GreenLightImage;
                case VirtualLightType.Blue: return BlueLightImage;
                case VirtualLightType.VRed: return VRedLightImage;
                default: return RedLightImage;
            }
        }
        #endregion

        #region 图片设置
        private static void OnRedLightImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScreenVirtualEditer ctrl = (ScreenVirtualEditer)d;
            if (ctrl != null)
            {
                ctrl.SetPicToData(VirtualLightType.Red, (BitmapImage)e.NewValue);
            }
        }

        private static void OnGreenLightImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScreenVirtualEditer ctrl = (ScreenVirtualEditer)d;
            if (ctrl != null)
            {
                ctrl.SetPicToData(VirtualLightType.Green, (BitmapImage)e.NewValue);
            }
        }

        private static void OnBlueLightImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScreenVirtualEditer ctrl = (ScreenVirtualEditer)d;
            if (ctrl != null)
            {
                ctrl.SetPicToData(VirtualLightType.Blue, (BitmapImage)e.NewValue);
            }
        }

        private static void OnVRedLightImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScreenVirtualEditer ctrl = (ScreenVirtualEditer)d;
            if (ctrl != null)
            {
                ctrl.SetPicToData(VirtualLightType.VRed, (BitmapImage)e.NewValue);
            }
        }

        private void SetPicToData(VirtualLightType lightType, BitmapImage img)
        {
            if (FirstLightData != null &&
                FirstLightData.LightType == lightType)
            {
                FirstLightData.ImageDisplay = img;
            }
            if (SecondLightData != null &&
                SecondLightData.LightType == lightType)
            {
                SecondLightData.ImageDisplay = img;
            }

            if (ThirdLightData != null &&
                ThirdLightData.LightType == lightType)
            {
                ThirdLightData.ImageDisplay = img;
            }

            if (FourthLightData != null &&
                FourthLightData.LightType == lightType)
            {
                FourthLightData.ImageDisplay = img;
            }
        }
        #endregion

        //private void TestD()
        //{

        //}

        #region 重写
        public override void OnApplyTemplate()
        {
            var imgae = this.GetTemplateChild("PART_SCALEVIEW") as Viewbox;
            var grid = this.GetTemplateChild("PART_SCALEGRID") as Grid;
            _viewBox = imgae;
            _scaleGrid = grid;
        }
        #endregion
    }
}
