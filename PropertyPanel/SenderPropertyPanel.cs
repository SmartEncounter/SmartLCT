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
using Nova.SmartLCT.Interface;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using GuiLabs.Undo;
using System.ComponentModel;
using GalaSoft.MvvmLight.Messaging;

namespace Nova.SmartLCT.UI
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:PropertyPanel"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:PropertyPanel;assembly=PropertyPanel"
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
    public class SenderPropertyPanel : Control
    {
        #region Xaml属性
        public static readonly DependencyProperty SenderRealParamsProperty =
           DependencyProperty.Register(
               "SenderRealParams", typeof(SenderRealParameters), typeof(SenderPropertyPanel),
               new FrameworkPropertyMetadata(null,
                   new PropertyChangedCallback(OnSenderRealParamsChanged)
               )
           );
        public static readonly DependencyProperty SmartLCTActionManagerProperty =
    DependencyProperty.Register("SmartLCTActionManager", typeof(ActionManager),
    typeof(SenderPropertyPanel), new FrameworkPropertyMetadata(new ActionManager()));
        public static readonly DependencyProperty MaxSenderHeightProperty =
      DependencyProperty.Register("MaxSenderHeight", typeof(double), typeof(SenderPropertyPanel));
        public static readonly DependencyProperty MaxSenderWidthProperty =
            DependencyProperty.Register("MaxSenderWidth", typeof(double), typeof(SenderPropertyPanel));

        public static RoutedCommand XChangedCommand
        {
            get { return _xChangedCommand; }
        }
        private static RoutedCommand _xChangedCommand;

        public static RoutedCommand XChangedBeforeCommand
        {
            get { return _xChangedBeforeCommand; }
        }
        private static RoutedCommand _xChangedBeforeCommand;
        public static RoutedCommand YChangedCommand
        {
            get { return _yChangedCommand; }
        }
        private static RoutedCommand _yChangedCommand;

        public static RoutedCommand YChangedBeforeCommand
        {
            get { return _yChangedBeforeCommand; }
        }
        private static RoutedCommand _yChangedBeforeCommand;
        public static RoutedCommand HeightChangedBeforeCommand
        {
            get { return _heightChangedBeforeCommand; }
        }
        private static RoutedCommand _heightChangedBeforeCommand;
        public static RoutedCommand HeightChangedCommand
        {
            get { return _heightChangedCommand; }
        }
        private static RoutedCommand _heightChangedCommand;
        public static RoutedCommand WidthChangedBeforeCommand
        {
            get { return _widthChangedBeforeCommand; }
        }
        private static RoutedCommand _widthChangedBeforeCommand; 
        public static RoutedCommand WidthChangedCommand
        {
            get { return _widthChangedCommand; }
        }
        private static RoutedCommand _widthChangedCommand; 
        #endregion

        #region 属性
        public double MinSenderHeight
        {
            get { return (double)GetValue(MaxSenderHeightProperty); }
            set { SetValue(MaxSenderHeightProperty, value); }
        }
        public double MinSenderWidth
        {
            get { return (double)GetValue(MaxSenderWidthProperty); }
            set { SetValue(MaxSenderWidthProperty, value); }
        }
        public ActionManager SmartLCTActionManager
        {
            get { return (ActionManager)GetValue(SmartLCTActionManagerProperty); }
            set
            {
                SetValue(SmartLCTActionManagerProperty, value);
            }
        }
        public SenderRealParameters SenderRealParams
        {
            get { return (SenderRealParameters)GetValue(SenderRealParamsProperty); }
            set
            {
                SetValue(SenderRealParamsProperty, value);
            }
        }

        #endregion

        #region 字段
        private double _lastX = 0;
        private double _lastY = 0;
        private double _lastHeight = 0;
        private double _lastWidth = 0;
        #endregion

        static SenderPropertyPanel()
        {
            InitializeCommands();

            DefaultStyleKeyProperty.OverrideMetadata(typeof(SenderPropertyPanel), new FrameworkPropertyMetadata(typeof(SenderPropertyPanel)));
        }
        private static void InitializeCommands()
        {
            _xChangedCommand = new RoutedCommand("XChangedCommand", typeof(SenderPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(SenderPropertyPanel), new CommandBinding(_xChangedCommand, OnXChangedCommand));
            _xChangedBeforeCommand = new RoutedCommand("XChangedBeforeCommand", typeof(SenderPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(SenderPropertyPanel), new CommandBinding(_xChangedBeforeCommand, OnXChangedBeforeCommand));
           
            _yChangedCommand = new RoutedCommand("YChangedCommand", typeof(SenderPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(SenderPropertyPanel), new CommandBinding(_yChangedCommand, OnYChangedCommand));
            _yChangedBeforeCommand = new RoutedCommand("YChangedBeforeCommand", typeof(SenderPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(SenderPropertyPanel), new CommandBinding(_yChangedBeforeCommand, OnYChangedBeforeCommand));
           
            _heightChangedCommand = new RoutedCommand("HeightChangedCommand", typeof(SenderPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(SenderPropertyPanel), new CommandBinding(_heightChangedCommand, OnHeightChangedCommand));
            _heightChangedBeforeCommand = new RoutedCommand("HeightChangedBeforeCommand", typeof(SenderPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(SenderPropertyPanel), new CommandBinding(_heightChangedBeforeCommand, OnHeightChangedBeforeCommand));
           
            _widthChangedCommand = new RoutedCommand("WidthChangedCommand", typeof(SenderPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(SenderPropertyPanel), new CommandBinding(_widthChangedCommand, OnWidthChangedCommand));
            _widthChangedBeforeCommand = new RoutedCommand("WidthChangedBeforeCommand", typeof(SenderPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(SenderPropertyPanel), new CommandBinding(_widthChangedBeforeCommand, OnWidthChangedBeforeCommand));


        }
        private static void OnHeightChangedBeforeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SenderPropertyPanel control = sender as SenderPropertyPanel;
            if (control != null)
            {
                control.OnHeightChangedBefore((double)e.Parameter);
            }
        }
        private void OnHeightChangedBefore(double value)
        {
            _lastHeight = value;
        }
        private static void OnHeightChangedCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SenderPropertyPanel control = sender as SenderPropertyPanel;
            if (control != null)
            {
                control.OnHeightChanged((double)e.Parameter);
            }
        }
        private void OnHeightChanged(double value)
        {
            RectLayerChangedAction action;
            action = new RectLayerChangedAction(SenderRealParams.Element, "Height", value, _lastHeight);
            SmartLCTActionManager.RecordAction(action);
            _lastHeight = value;
        }


        private static void OnWidthChangedBeforeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SenderPropertyPanel control = sender as SenderPropertyPanel;
            if (control != null)
            {
                control.OnWidthChangedBefore((double)e.Parameter);
            }
        }
        private void OnWidthChangedBefore(double value)
        {
            _lastWidth = value;
        }
        private static void OnWidthChangedCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SenderPropertyPanel control = sender as SenderPropertyPanel;
            if (control != null)
            {
                control.OnWidthChanged((double)e.Parameter);
            }
        }
        private void OnWidthChanged(double value)
        {
            RectLayerChangedAction action;
            action = new RectLayerChangedAction(SenderRealParams.Element, "Width", value, _lastWidth);
            SmartLCTActionManager.RecordAction(action);
            _lastWidth = value;
        }


        private static void OnYChangedBeforeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SenderPropertyPanel control = sender as SenderPropertyPanel;
            if (control != null)
            {
                control.OnYChangedBefore((double)e.Parameter);
            }
        }
        private void OnYChangedBefore(double value)
        {
            _lastY= value;
        }
        private static void OnYChangedCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SenderPropertyPanel control = sender as SenderPropertyPanel;
            if (control != null)
            {
                control.OnYChanged((double)e.Parameter);
            }
        }
        private void OnYChanged(double value)
        {
            OnHandleGroupFrameLocation((IRectElement)SenderRealParams.Element);

            RectLayerChangedAction action;
            action = new RectLayerChangedAction(SenderRealParams.Element, "Y", value, _lastY);
            SmartLCTActionManager.RecordAction(action);
            _lastY = value;
        }


        private static void OnXChangedBeforeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SenderPropertyPanel control = sender as SenderPropertyPanel;
            if (control != null)
            {
                control.OnXChangedBefore((double)e.Parameter);
            }
        }
        private void OnXChangedBefore(double value)
        {
            _lastX = value;
        }
        private static  void OnXChangedCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SenderPropertyPanel control = sender as SenderPropertyPanel;
            if (control != null)
            {
                control.OnXChanged((double)e.Parameter);
            }
        }
        /// <summary>
        /// 更改某个元素的位置后，该元素处于某个组时，处理该元素组框的位置
        /// </summary>
        /// <param name="element"></param>
        private void OnHandleGroupFrameLocation(IRectElement element)
        {
            #region 移动的网口属于某个组，则处理组框的位置
            if (element.EleType == ElementType.receive && element.GroupName >= 0)
            {
                //找到该组下所有的元素
                IRectElement groupFrame = new RectElement();
                ObservableCollection<IRectElement> elementCollection = new ObservableCollection<IRectElement>();
                if (element.ParentElement != null && element.ParentElement.EleType == ElementType.screen)
                {
                    RectLayer screenLayer = (RectLayer)element.ParentElement;
                    for (int i = 0; i < screenLayer.ElementCollection.Count; i++)
                    {
                        if (screenLayer.ElementCollection[i].EleType == ElementType.receive &&
                            screenLayer.ElementCollection[i].GroupName == element.GroupName)
                        {
                            elementCollection.Add((IRectElement)screenLayer.ElementCollection[i]);
                        }
                        else if (screenLayer.ElementCollection[i].EleType == ElementType.groupframe &&
                            screenLayer.ElementCollection[i].GroupName == element.GroupName)
                        {
                            groupFrame = (IRectElement)screenLayer.ElementCollection[i];
                        }
                    }
                }
                //计算组框新值
                if (elementCollection.Count > 0)
                {
                    Rect newGroupframeRect = Function.UnionRectCollection(elementCollection);
                    groupFrame.X = newGroupframeRect.X;
                    groupFrame.Y = newGroupframeRect.Y;
                    groupFrame.Width = newGroupframeRect.Width;
                    groupFrame.Height = newGroupframeRect.Height;
                }
            }
            #endregion
        }
        private void OnXChanged(double value)
        {
            OnHandleGroupFrameLocation((IRectElement)SenderRealParams.Element);
            RectLayerChangedAction action;
            action = new RectLayerChangedAction(SenderRealParams.Element, "X", value,_lastX);
            SmartLCTActionManager.RecordAction(action);
            _lastX = value;
        }

        public override void OnApplyTemplate()
        {
            FrameworkElement ele = (FrameworkElement)GetTemplateChild("myBoder");
            ele = (FrameworkElement)GetTemplateChild("myGrid");
            ele = (FrameworkElement)GetTemplateChild("numericUpDown1");
        }
        private static void OnSenderRealParamsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SenderPropertyPanel ctrl = (SenderPropertyPanel)d;
            ctrl.CoerceValue(SenderRealParamsProperty);
        //    ctrl.OnMaxHeightAndWidth();
            //ctrl.OnSenderRealParams();
        }
        private void OnSenderRealParams()
        {
            if (SenderRealParams != null &&
                   SenderRealParams.Element != null)
            {
                IRectLayer parentLayer = (IRectLayer)SenderRealParams.Element.ParentElement;
                while (parentLayer != null && parentLayer.EleType != ElementType.baselayer)
                {
                    parentLayer = (IRectLayer)parentLayer.ParentElement;
                }
                if (parentLayer != null && parentLayer.EleType == ElementType.baselayer)
                {
                    SmartLCTActionManager = parentLayer.MyActionManager;
                }
            }
        }
        private void OnMaxHeightAndWidth()
        {
            if(SenderRealParams!=null &&
                SenderRealParams.Element != null &&
                ((RectLayer)SenderRealParams.Element).ElementCollection!=null)
            {           
                List<RectLayer> layerList = new List<RectLayer>();
                for (int i = 0; i < ((RectLayer)SenderRealParams.Element).ElementCollection.Count; i++)
                {
                    if (((RectLayer)SenderRealParams.Element).ElementCollection[i] is RectLayer)
                    {
                        layerList.Add((RectLayer)((RectLayer)SenderRealParams.Element).ElementCollection[i]);
                    }
                }
                layerList.Sort(delegate(RectLayer first, RectLayer second)
                {
                    return (first.X + first.Width).CompareTo(second.X + second.Width);
                });
                MinSenderWidth = layerList[layerList.Count - 1].X + layerList[layerList.Count - 1].Width;

                for (int i = 0; i < ((RectLayer)SenderRealParams.Element).ElementCollection.Count; i++)
                {
                    if (((RectLayer)SenderRealParams.Element).ElementCollection[i] is RectLayer)
                    {
                        layerList.Add((RectLayer)((RectLayer)SenderRealParams.Element).ElementCollection[i]);
                    }
                }
                layerList.Sort(delegate(RectLayer first, RectLayer second)
                {
                    return (first.Y + first.Height).CompareTo(second.Y + second.Height);
                });
                MinSenderHeight = layerList[layerList.Count - 1].Y+ layerList[layerList.Count - 1].Height;
            }
        }
        #region 记录活动
        private void OnRecordActionValueChanged(PrePropertyChangedEventArgs e)
        {
            RectElement rectElement = new RectElement();
            //RectLayerChangedAction action;
            //action = new RectLayerChangedAction(MyRectLayer, e.PropertyName, ((RectLayer)e.NewValue).ElementCollection, ((RectLayer)e.OldValue).ElementCollection);
            //SmartLCTActionManager.RecordAction(action);
        }
        #endregion
    }
}
