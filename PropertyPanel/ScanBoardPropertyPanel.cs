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
using GuiLabs.Undo;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using Nova.Wpf.Control;

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
    ///     <MyNamespace:ScanBoardPropertyPanel/>
    ///
    /// </summary>
    public class ScanBoardPropertyPanel : Control
    {
        #region Xaml属性
        public static readonly DependencyProperty ScannerRealParamsProperty =
           DependencyProperty.Register(
               "ScannerRealParams", typeof(ScannerRealParameters), typeof(ScanBoardPropertyPanel),
               new FrameworkPropertyMetadata(null,
                   new PropertyChangedCallback(OnScannerRealParamsChanged)
               )
           );
        public static readonly DependencyProperty SmartLCTActionManagerProperty =
            DependencyProperty.Register("SmartLCTActionManager", typeof(ActionManager),
                typeof(ScanBoardPropertyPanel), new FrameworkPropertyMetadata(new ActionManager()));
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
        #endregion

        #region 属性
        public ActionManager SmartLCTActionManager
        {
            get { return (ActionManager)GetValue(SmartLCTActionManagerProperty); }
            set
            {
                SetValue(SmartLCTActionManagerProperty, value);
            }
        }

        public ScannerRealParameters ScannerRealParams
        {
            get { return (ScannerRealParameters)GetValue(ScannerRealParamsProperty); }
            set
            {
                SetValue(ScannerRealParamsProperty, value);
            }
        }
        #endregion

        #region 字段
        private double _lastX = 0;
        private double _lastY = 0;
        #endregion

        static ScanBoardPropertyPanel()
        {
            InitializeCommands();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScanBoardPropertyPanel), new FrameworkPropertyMetadata(typeof(ScanBoardPropertyPanel)));
        }

        private static void InitializeCommands()
        {
            _xChangedCommand = new RoutedCommand("XChangedCommand", typeof(ScanBoardPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScanBoardPropertyPanel), new CommandBinding(_xChangedCommand, OnXChangedCommand));
            _xChangedBeforeCommand = new RoutedCommand("XChangedBeforeCommand", typeof(ScanBoardPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScanBoardPropertyPanel), new CommandBinding(_xChangedBeforeCommand, OnXChangedBeforeCommand));

            _yChangedCommand = new RoutedCommand("YChangedCommand", typeof(ScanBoardPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScanBoardPropertyPanel), new CommandBinding(_yChangedCommand, OnYChangedCommand));
            _yChangedBeforeCommand = new RoutedCommand("YChangedBeforeCommand", typeof(ScanBoardPropertyPanel));
            CommandManager.RegisterClassCommandBinding(typeof(ScanBoardPropertyPanel), new CommandBinding(_yChangedBeforeCommand, OnYChangedBeforeCommand));

        }

        public ScanBoardPropertyPanel()
        {

        }
        private static void OnYChangedBeforeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ScanBoardPropertyPanel control = sender as ScanBoardPropertyPanel;
            if (control != null)
            {
                control.OnYChangedBefore((double)e.Parameter);
            }
        }
        private void OnYChangedBefore(double value)
        {
            _lastY = value;
        }
        private static void OnYChangedCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ScanBoardPropertyPanel control = sender as ScanBoardPropertyPanel;
            if (control != null)
            {
                control.OnYChanged((double)e.Parameter);
            }
        }
        private void OnYChanged(double value)
        {
            ObservableCollection<ElementMoveInfo> elementMoveInfoCollection = new ObservableCollection<ElementMoveInfo>();
            ObservableCollection<ElementSizeInfo> elementSizeInfoCollection = new ObservableCollection<ElementSizeInfo>();
            ElementMoveInfo moveInfo = new ElementMoveInfo();
            moveInfo.Element = ScannerRealParams.ScannerElement;
            Point oldPoint = new Point(ScannerRealParams.ScannerElement.X, _lastY);
            Point newPoint = new Point(ScannerRealParams.ScannerElement.X, value);
            moveInfo.OldPoint = oldPoint;
            moveInfo.NewPoint = newPoint;
            elementMoveInfoCollection.Add(moveInfo);

            if (ScannerRealParams.Groupframe != null && ScannerRealParams.Groupframe.GroupName != -1)
            {
                ElementMoveInfo groupframeMoveInfo = new ElementMoveInfo();
                groupframeMoveInfo.Element = ScannerRealParams.Groupframe;
                groupframeMoveInfo.OldPoint = new Point(ScannerRealParams.Groupframe.X, ScannerRealParams.Groupframe.Y);

                ElementSizeInfo groupframeSizeInfo = new ElementSizeInfo();
                groupframeSizeInfo.Element = ScannerRealParams.Groupframe;
                groupframeSizeInfo.OldSize = new Size(ScannerRealParams.Groupframe.Width, ScannerRealParams.Groupframe.Height);

                if (ScannerRealParams.NoSelectedElementRect == null)
                {
                    ScannerRealParams.Groupframe.Y = ScannerRealParams.ScannerElement.Y;
                }
                else
                {
                    IRectElement element = ScannerRealParams.ScannerElement;
                    Rect selectedRect = new Rect(element.X, element.Y, element.Width, element.Height);
                    Rect unionRect = Rect.Union(selectedRect, ScannerRealParams.NoSelectedElementRect);
                    ScannerRealParams.Groupframe.Y = unionRect.Y;
                    ScannerRealParams.Groupframe.Height = unionRect.Height;
                }
                groupframeMoveInfo.NewPoint = new Point(ScannerRealParams.Groupframe.X, ScannerRealParams.Groupframe.Y);
                groupframeSizeInfo.NewSize = new Size(ScannerRealParams.Groupframe.Width, ScannerRealParams.Groupframe.Height);
                elementMoveInfoCollection.Add(groupframeMoveInfo);
                elementSizeInfoCollection.Add(groupframeSizeInfo);
            }
            ElementMoveAction elementMoveAction = new ElementMoveAction(elementMoveInfoCollection);
            ElementSizeAction elementSizeAction = new ElementSizeAction(elementSizeInfoCollection);
            using (Transaction.Create(SmartLCTActionManager))
            {
                SmartLCTActionManager.RecordAction(elementMoveAction);
                SmartLCTActionManager.RecordAction(elementSizeAction);
            }
            _lastY = value;
        }


        private static void OnXChangedBeforeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ScanBoardPropertyPanel control = sender as ScanBoardPropertyPanel;
            if (control != null)
            {
                control.OnXChangedBefore((double)e.Parameter);
            }
        }
        private void OnXChangedBefore(double value)
        {
            _lastX = value;
        }
        private static void OnXChangedCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ScanBoardPropertyPanel control = sender as ScanBoardPropertyPanel;
            if (control != null)
            {
                control.OnXChanged((double)e.Parameter);
            }
        }

        private void OnXChanged(double value)
        {
            ObservableCollection<ElementMoveInfo> elementMoveInfoCollection = new ObservableCollection<ElementMoveInfo>();
            ObservableCollection<ElementSizeInfo> elementSizeInfoCollection = new ObservableCollection<ElementSizeInfo>();
            ElementMoveInfo moveInfo = new ElementMoveInfo();
            moveInfo.Element = ScannerRealParams.ScannerElement;
            Point oldPoint = new Point(_lastX, ScannerRealParams.ScannerElement.Y);
            Point newPoint = new Point(value, ScannerRealParams.ScannerElement.Y);
            moveInfo.OldPoint = oldPoint;
            moveInfo.NewPoint = newPoint;
            elementMoveInfoCollection.Add(moveInfo);

            if (ScannerRealParams.Groupframe != null && ScannerRealParams.Groupframe.GroupName != -1)
            {
                ElementMoveInfo groupframeMoveInfo = new ElementMoveInfo();
                groupframeMoveInfo.Element = ScannerRealParams.Groupframe;
                groupframeMoveInfo.OldPoint = new Point(ScannerRealParams.Groupframe.X, ScannerRealParams.Groupframe.Y);

                ElementSizeInfo groupframeSizeInfo = new ElementSizeInfo();
                groupframeSizeInfo.Element = ScannerRealParams.Groupframe;
                groupframeSizeInfo.OldSize = new Size(ScannerRealParams.Groupframe.Width, ScannerRealParams.Groupframe.Height);

                if (ScannerRealParams.NoSelectedElementRect == null)
                {
                    ScannerRealParams.Groupframe.X = ScannerRealParams.ScannerElement.X;
                }
                else
                {
                    IRectElement element = ScannerRealParams.ScannerElement;
                    Rect selectedRect = new Rect(element.X, element.Y, element.Width, element.Height);
                    Rect unionRect = Rect.Union(selectedRect, ScannerRealParams.NoSelectedElementRect);
                    ScannerRealParams.Groupframe.X = unionRect.X;
                    ScannerRealParams.Groupframe.Width = unionRect.Width;
                }
                groupframeMoveInfo.NewPoint = new Point(ScannerRealParams.Groupframe.X, ScannerRealParams.Groupframe.Y);
                groupframeSizeInfo.NewSize = new Size(ScannerRealParams.Groupframe.Width, ScannerRealParams.Groupframe.Height);
                elementMoveInfoCollection.Add(groupframeMoveInfo);
                elementSizeInfoCollection.Add(groupframeSizeInfo);
            }
            ElementMoveAction elementMoveAction = new ElementMoveAction(elementMoveInfoCollection);
            ElementSizeAction elementSizeAction = new ElementSizeAction(elementSizeInfoCollection);
            using (Transaction.Create(SmartLCTActionManager))
            {
                SmartLCTActionManager.RecordAction(elementMoveAction);
                SmartLCTActionManager.RecordAction(elementSizeAction);
            }
            _lastX = value;
        }
        private static void OnScannerRealParamsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScanBoardPropertyPanel control = d as ScanBoardPropertyPanel;
            if (control != null)
            {
                control.OnScannerRealParams();
            }
        }
        private void OnScannerRealParams()
        {
        }
  
    }
}
