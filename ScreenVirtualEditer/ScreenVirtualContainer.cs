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
using Nova.LCT.GigabitSystem.Common;
using GalaSoft.MvvmLight.Command;
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
    ///     xmlns:MyNamespace="clr-namespace:Nova.SmartLCT.UI"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Nova.SmartLCT.UI;assembly=Nova.SmartLCT.UI"
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
    ///     <MyNamespace:ScreenVirtualContainer/>
    ///
    /// </summary>
    [TemplatePartAttribute(Name = " PART_SCREENVIRTUALEDIER ", Type = typeof(Grid))]
    public class ScreenVirtualContainer : Control
    {
        #region Xaml
        #region 图片设置
        public static readonly DependencyProperty RedLightImageProperty =
          DependencyProperty.Register(
              "RedLightImage", typeof(BitmapImage), typeof(ScreenVirtualContainer),
              new FrameworkPropertyMetadata(null,
                  new PropertyChangedCallback(OnRedLightImageChanged)
              ));

        public static readonly DependencyProperty GreenLightImageProperty =
          DependencyProperty.Register(
              "GreenLightImage", typeof(BitmapImage), typeof(ScreenVirtualContainer),
              new FrameworkPropertyMetadata(null,
                  new PropertyChangedCallback(OnGreenLightImageChanged)
              ));

        public static readonly DependencyProperty BlueLightImageProperty =
          DependencyProperty.Register(
              "BlueLightImage", typeof(BitmapImage), typeof(ScreenVirtualContainer),
              new FrameworkPropertyMetadata(null,
                  new PropertyChangedCallback(OnBlueLightImageChanged)
              ));

        public static readonly DependencyProperty VRedLightImageProperty =
          DependencyProperty.Register(
              "VRedLightImage", typeof(BitmapImage), typeof(ScreenVirtualContainer),
              new FrameworkPropertyMetadata(null,
                  new PropertyChangedCallback(OnVRedLightImageChanged)
              ));
        #endregion

        public static readonly DependencyProperty VirtualMapProperty =
          DependencyProperty.Register(
              "VirtualMap", typeof(int), typeof(ScreenVirtualContainer),
              new FrameworkPropertyMetadata(0xe4,
                  new PropertyChangedCallback(OnVirtualMapChanged)
              ));
        public static readonly DependencyProperty VirtualModeProperty =
         DependencyProperty.Register(
             "VirtualMode", typeof(VirtualModeType), typeof(ScreenVirtualContainer),
             new FrameworkPropertyMetadata(VirtualModeType.Led3,
                 new PropertyChangedCallback(OnVirtualModeChanged)
             )
         );

        #region 子控件参数
        //internal static readonly DependencyProperty PreviewFirstLightSequenceProperty =
        // DependencyProperty.Register(
        //     "PreviewFirstLightSequence", typeof(ObservableCollection<VirtualLightType>), typeof(ScreenVirtualContainer),
        //     new FrameworkPropertyMetadata(null,
        //         new PropertyChangedCallback(OnPreviewFirstLightSequenceChanged)
        //     )
        // );

        //internal static readonly DependencyProperty PreviewSecondLightSequenceProperty =
        // DependencyProperty.Register(
        //     "PreviewSecondLightSequence", typeof(ObservableCollection<VirtualLightType>), typeof(ScreenVirtualContainer),
        //     new FrameworkPropertyMetadata(null,
        //         new PropertyChangedCallback(OnPreviewSecondLightSequenceChanged)
        //     )
        // );

        internal static readonly DependencyProperty PreviewFirstVirtualModeProperty =
         DependencyProperty.Register(
             "PreviewFirstVirtualMode", typeof(VirtualModeType), typeof(ScreenVirtualContainer),
             new FrameworkPropertyMetadata(VirtualModeType.Led3,
                 new PropertyChangedCallback(OnPreviewFirstVirtualModeChanged)
             )
         );

        internal static readonly DependencyProperty PreviewSecondVirtualModeProperty =
         DependencyProperty.Register(
             "PreviewSecondVirtualMode", typeof(VirtualModeType), typeof(ScreenVirtualContainer),
             new FrameworkPropertyMetadata(VirtualModeType.Led31,
                 new PropertyChangedCallback(OnPreviewSecondVirtualModeChanged)
             )
         );

        //public static readonly DependencyProperty LightSequenceProperty =
        //DependencyProperty.Register(
        //    "LightSequence", typeof(ObservableCollection<VirtualLightType>), typeof(ScreenVirtualContainer),
        //    new FrameworkPropertyMetadata(null,
        //        new PropertyChangedCallback(OnLightSequenceChanged)
        //    )
        //);

        internal static readonly DependencyProperty CmdLightChangedProperty =
        DependencyProperty.Register(
            "CmdLightChanged", typeof(RelayCommand<RoutedPropertyChangedEventArgs<ObservableCollection<VirtualLightType>>>), typeof(ScreenVirtualContainer),
            new FrameworkPropertyMetadata(null,
                new PropertyChangedCallback(OnCmdLightChangedProertySet)
            )
        );

        internal static readonly DependencyProperty CmdLightSequenceResettedProperty =
       DependencyProperty.Register(
           "CmdLightSequenceResetted", typeof(RelayCommand), typeof(ScreenVirtualContainer),
           new FrameworkPropertyMetadata(null,
               new PropertyChangedCallback(OnCmdLightSequenceProertySet)
           )
       );
        #endregion

        #endregion

        #region 属性
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

        #region 子控件参数
        //internal ObservableCollection<VirtualLightType> PreviewFirstLightSequence
        //{
        //    get { return (ObservableCollection<VirtualLightType>)GetValue(PreviewFirstLightSequenceProperty); }
        //    set
        //    {
        //        SetValue(PreviewFirstLightSequenceProperty, value);
        //    }
        //}
        //internal ObservableCollection<VirtualLightType> PreviewSecondLightSequence
        //{
        //    get { return (ObservableCollection<VirtualLightType>)GetValue(PreviewSecondLightSequenceProperty); }
        //    set
        //    {
        //        SetValue(PreviewSecondLightSequenceProperty, value);
        //    }
        //}
        //public ObservableCollection<VirtualLightType> LightSequence
        //{
        //    get { return (ObservableCollection<VirtualLightType>)GetValue(LightSequenceProperty); }
        //    set
        //    {
        //        SetValue(LightSequenceProperty, value);
        //    }
        //}
        //internal ObservableCollection<VirtualLightType> LightSequence
        //{
        //    get { return _lightSequence; }
        //    set
        //    {
        //        _lightSequence = value;
        //    }
        //}
        //private ObservableCollection<VirtualLightType> _lightSequence = null;
        internal VirtualModeType PreviewFirstVirtualMode
        {
            get { return (VirtualModeType)GetValue(PreviewFirstVirtualModeProperty); }
            set
            {
                SetValue(PreviewFirstVirtualModeProperty, value);
            }
        }
        internal VirtualModeType PreviewSecondVirtualMode
        {
            get { return (VirtualModeType)GetValue(PreviewSecondVirtualModeProperty); }
            set
            {
                SetValue(PreviewSecondVirtualModeProperty, value);
            }
        }
        #endregion

        public int VirtualMap
        {
            get { return (int)GetValue(VirtualMapProperty); }
            set
            {
                SetValue(VirtualMapProperty, value);
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

        #endregion

        #region 命令
        internal RelayCommand<RoutedPropertyChangedEventArgs<ObservableCollection<VirtualLightType>>> CmdLightChanged
        {
            get
            {
                return (RelayCommand<RoutedPropertyChangedEventArgs<ObservableCollection<VirtualLightType>>>)GetValue(CmdLightChangedProperty);
            }
            set
            {
                SetValue(CmdLightChangedProperty, value);
            }
        }

        internal RelayCommand CmdLightSequenceResetted
        {
            get
            {
                return (RelayCommand)GetValue(CmdLightSequenceResettedProperty);
            }
            set
            {
                SetValue(CmdLightSequenceResettedProperty, value);
            }
        }
        #endregion

        #region 字段
        private ScreenVirtualContainer_VM _vm = null;
        #endregion

        static ScreenVirtualContainer()
        {
            //InitializeCommands();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScreenVirtualContainer), new FrameworkPropertyMetadata(typeof(ScreenVirtualContainer)));
        }

        public ScreenVirtualContainer()
        {
            //ObservableCollection<VirtualLightType> squence = new ObservableCollection<VirtualLightType>();
            //squence.Add(GetVirtualLightType(VirtualMap, 0));
            //squence.Add(GetVirtualLightType(VirtualMap, 1));
            //squence.Add(GetVirtualLightType(VirtualMap, 2));
            //squence.Add(GetVirtualLightType(VirtualMap, 3));
            //LightSequence = squence;

            //squence = new ObservableCollection<VirtualLightType>();
            //squence.Add(GetVirtualLightType(VirtualMap, 0));
            //squence.Add(GetVirtualLightType(VirtualMap, 1));
            //squence.Add(GetVirtualLightType(VirtualMap, 2));
            //squence.Add(GetVirtualLightType(VirtualMap, 3));
            //PreviewFirstLightSequence = squence;

            //squence = new ObservableCollection<VirtualLightType>();
            //squence.Add(GetVirtualLightType(VirtualMap, 0));
            //squence.Add(GetVirtualLightType(VirtualMap, 1));
            //squence.Add(GetVirtualLightType(VirtualMap, 2));
            //squence.Add(GetVirtualLightType(VirtualMap, 3));
            //PreviewSecondLightSequence = squence;
            //_lightSequence = new ObservableCollection<VirtualLightType>();
            CmdLightChanged = new RelayCommand<RoutedPropertyChangedEventArgs<ObservableCollection<VirtualLightType>>>(OnCmdLightChanged);
            CmdLightSequenceResetted = new RelayCommand(OnCmdLightSequenceResetted);
            //_vm = (ScreenVirtualContainer_VM)this.FindResource("ScreenVirtualContainer_VMDataSource");
        }

        #region 依赖项值变更

        #region 图片设置
        private static void OnRedLightImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnGreenLightImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnBlueLightImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnVRedLightImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion

        private static void OnVirtualMapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScreenVirtualContainer ctrl = (ScreenVirtualContainer)d;
            if (ctrl != null)
            {
                ctrl.OnVirtualMapSetted(e);
            }
        }
        private void OnVirtualMapSetted(DependencyPropertyChangedEventArgs e)
        {
            int rgbMap = (int)e.NewValue;
            VirtualMapChangedToSequence();
        }

        private static void OnVirtualModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScreenVirtualContainer ctrl = (ScreenVirtualContainer)d;
            if (ctrl != null)
            {
                ctrl.SetPreviewVirtualMode();
            }
        }

        private static void OnPreviewFirstLightSequenceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnPreviewSecondLightSequenceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnPreviewFirstVirtualModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnPreviewSecondVirtualModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnLightSequenceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnCmdLightChangedProertySet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        
        private static void OnCmdLightSequenceProertySet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion

        #region 私有函数
        #region 命令处理
        //private static void InitializeCommands()
        //{
        //    _cmdLightChanged = new RoutedCommand("CmdLightChanged", typeof(ScreenVirtualContainer));
        //    CommandManager.RegisterClassCommandBinding(typeof(ScreenVirtualContainer), new CommandBinding(_cmdLightChanged, OnCmdLightChanged));

        //}
        private void OnCmdLightChanged(RoutedPropertyChangedEventArgs<ObservableCollection<VirtualLightType>> args)
        {
            object data = args.NewValue;
            SequenceChangedToVirtualMap();
            SetPreviewVirtualSequence();
        }

        private void OnCmdLightSequenceResetted()
        {
            SequenceChangedToVirtualMap();

        }

        private void Data(object sender, RoutedEventArgs e)
        {
            SequenceChangedToVirtualMap();
        }
    
        #endregion
        private bool _isWork = false;
        private void VirtualMapChangedToSequence()
        {
            if (_isWork)
            {
                return;
            }
            ObservableCollection<VirtualLightType> squence = new ObservableCollection<VirtualLightType>();
            squence.Add(GetVirtualLightType(VirtualMap, 2));
            squence.Add(GetVirtualLightType(VirtualMap, 1));
            squence.Add(GetVirtualLightType(VirtualMap, 0));
            squence.Add(GetVirtualLightType(VirtualMap, 3));
            _vm.LightSequence = squence;
            SetPreviewVirtualSequence();
        }

        private VirtualLightType GetVirtualLightType(int virtualMap, int index)
        {
            int data = 0;
            data = ((virtualMap >> (index * 2)) & 0x03);
            VirtualLightType virtualLight = VirtualLightType.Red;
            switch (data)
            {
                case 0: virtualLight = VirtualLightType.Blue; break;
                case 1: virtualLight = VirtualLightType.Green; break;
                case 2: virtualLight = VirtualLightType.Red; break;
                case 3: virtualLight = VirtualLightType.VRed; break;
            }
            return virtualLight;
        }

        private void SequenceChangedToVirtualMap()
        {
            _isWork = true;
            int data = (GetModeData(_vm.LightSequence[3]) << 6);
            data += (GetModeData(_vm.LightSequence[0]) << 4);
            data += (GetModeData(_vm.LightSequence[1]) << 2);
            data += GetModeData(_vm.LightSequence[2]);
            VirtualMap = data;
            _isWork = false;
        }

        private int GetModeData(VirtualLightType virtualType)
        {
            if (virtualType == VirtualLightType.Red)
            {
                return 2;
            }
            else if (virtualType == VirtualLightType.Green)
            {
                return 1;
            }
            else if (virtualType == VirtualLightType.Blue)
            {
                return 0;
            }
            else
            {
                return 3;
            }
        }

        private void SetPreviewVirtualMode()
        {
            PreviewFirstVirtualMode = VirtualMode;
            if (VirtualMode == VirtualModeType.Led3)
            {
                PreviewSecondVirtualMode = VirtualModeType.Led31;
            }
            else if (VirtualMode == VirtualModeType.Led31)
            {
                PreviewSecondVirtualMode = VirtualModeType.Led3;
            }
            else if (VirtualMode == VirtualModeType.Led4Mode1)
            {
                PreviewSecondVirtualMode = VirtualModeType.Led4Mode1;
            }

        }

        private void SetPreviewVirtualSequence()
        {
            #region 统一
            ObservableCollection<VirtualLightType> squence = null;

            squence = new ObservableCollection<VirtualLightType>();
            squence.Add(_vm.LightSequence[0]);
            squence.Add(_vm.LightSequence[1]);
            squence.Add(_vm.LightSequence[2]);
            squence.Add(_vm.LightSequence[3]);
            _vm.PreviewFirstLightSequence = squence;
            #endregion

            #region Led3
            if (VirtualMode == VirtualModeType.Led31)
            {
                ObservableCollection<VirtualLightType> squence1 = null;
                squence1 = new ObservableCollection<VirtualLightType>();
                squence1.Add(_vm.LightSequence[2]);
                squence1.Add(_vm.LightSequence[3]);
                squence1.Add(_vm.LightSequence[1]);
                squence1.Add(_vm.LightSequence[0]);
                _vm.PreviewSecondLightSequence = squence1;
            }
            #endregion

            #region Led31
            if (VirtualMode == VirtualModeType.Led3)
            {
                ObservableCollection<VirtualLightType> squence1 = null;
                squence1 = new ObservableCollection<VirtualLightType>();
                squence1.Add(_vm.LightSequence[3]);
                squence1.Add(_vm.LightSequence[2]);
                squence1.Add(_vm.LightSequence[0]);
                squence1.Add(_vm.LightSequence[1]);
                _vm.PreviewSecondLightSequence = squence1;
            }
            #endregion

            #region Led4Mode1
            if (VirtualMode == VirtualModeType.Led4Mode1)
            {
                ObservableCollection<VirtualLightType> squence1 = null;
                squence1 = new ObservableCollection<VirtualLightType>();
                squence1.Add(_vm.LightSequence[0]);
                squence1.Add(_vm.LightSequence[1]);
                squence1.Add(_vm.LightSequence[2]);
                squence1.Add(_vm.LightSequence[3]);
                _vm.PreviewSecondLightSequence = squence1;
            }
            #endregion
        }
        #endregion

        #region 重写
        public override void OnApplyTemplate()
        {
            ScreenVirtualEditer temp = this.GetTemplateChild("PART_SCREENVIRTUALEDIER") as ScreenVirtualEditer;
            _vm = (ScreenVirtualContainer_VM)temp.DataContext;
        }
        #endregion
    }
}
