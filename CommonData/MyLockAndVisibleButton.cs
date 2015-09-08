using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;

namespace Nova.SmartLCT.Interface
{
    public class MyLockAndVisibleButton : Control
    {
        #region 定义xmal属性
        /// <summary>
        /// 控件的IsLockProperty属性值
        /// </summary>
        public static readonly DependencyProperty IsLockedProperty =
            DependencyProperty.Register("IsLocked", typeof(bool), typeof(MyLockAndVisibleButton));
        /// <summary>
        /// 控件的Visible属性值
        /// </summary>
        public static readonly DependencyProperty VisibleProperty =
            DependencyProperty.Register("Visible", typeof(Visibility), typeof(MyLockAndVisibleButton));
        /// <summary>
        /// 控件的ElementPropetty属性值
        /// </summary>
        public static readonly DependencyProperty ElementProperty =
    DependencyProperty.Register("Element", typeof(IElement), typeof(MyLockAndVisibleButton));
        public static readonly DependencyProperty IsOverLoadProperty =
     DependencyProperty.Register("IsOverLoad", typeof(bool), typeof(MyLockAndVisibleButton), new FrameworkPropertyMetadata(false, MyRectLayerPropertyChangedCallBack));

        #endregion
        #region 属性
        public bool IsOverLoad
        {
            get { return (bool)GetValue(IsOverLoadProperty); }
            set { SetValue(IsOverLoadProperty, value); }
        }
        public IElement Element
        {
            get { return (IElement)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }
        public bool IsLocked
        {
            get { return (bool)GetValue(IsLockedProperty); }
            set { SetValue(IsLockedProperty, value); }
        }
        public Visibility Visible
        {
            get { return (Visibility)GetValue(VisibleProperty); }
            set
            {
                SetValue(VisibleProperty, value);
            }
        }
        #endregion

        public static RoutedCommand LockCommand
        {
            get
            {
                return _lockCommand;
            }
        }
        private static RoutedCommand _lockCommand;
        public static RoutedCommand VisibleCommand
        {
            get
            {
                return _visibleCommand;
            }
        }
        private static RoutedCommand _visibleCommand;

        private static void MyRectLayerPropertyChangedCallBack(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            MyLockAndVisibleButton ctrl = (MyLockAndVisibleButton)obj;
            if (e.Property == IsOverLoadProperty)
            {
                ctrl.OnRectLayerPropertyChanged();
            }
        }
        private void OnRectLayerPropertyChanged()
        {

        }



        #region 构造函数

        static MyLockAndVisibleButton()
        {
            InitializeCommands();
            EventManager.RegisterClassHandler(typeof(MyLockAndVisibleButton),
            Mouse.MouseDownEvent, new MouseButtonEventHandler(MyLockAndVisibleButton.OnMouseLeftButtonDown), true);

            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyLockAndVisibleButton), new FrameworkPropertyMetadata(typeof(MyLockAndVisibleButton)));
        }
        private static void InitializeCommands()
        {
            _lockCommand = new RoutedCommand("IncreaseCommand", typeof(MyLockAndVisibleButton));
            CommandManager.RegisterClassCommandBinding(typeof(MyLockAndVisibleButton), new CommandBinding(_lockCommand, OnLockCommand));
            _visibleCommand = new RoutedCommand("DecreaseCommand", typeof(MyLockAndVisibleButton));
            CommandManager.RegisterClassCommandBinding(typeof(MyLockAndVisibleButton), new CommandBinding(_visibleCommand, OnVisibilityCommand));
            CommandManager.RegisterClassInputBinding(typeof(MyLockAndVisibleButton), new InputBinding(_visibleCommand, new KeyGesture(Key.Down)));

        }
        private static void OnLockCommand(object sender, ExecutedRoutedEventArgs e)
        {
            MyLockAndVisibleButton control = sender as MyLockAndVisibleButton;
            if (control != null)
            {
                control.OnLock();
            }
        }
        private static void OnVisibilityCommand(object sender, ExecutedRoutedEventArgs e)
        {
            MyLockAndVisibleButton control = sender as MyLockAndVisibleButton;
            if (control != null)
            {
                control.OnVisibility();
            }
        }

        protected virtual void OnLock()
        {
            if (Element.ParentElement.IsLocked)
            {
                return;
            }

            IElement parent = Element.ParentElement;
            RectLayer old = new RectLayer();
            if (parent == null && Element is RectLayer)
            {
                old = (RectLayer)((RectLayer)Element).Clone();
            }
            else
            {
                while (parent.ParentElement != null)
                {
                    parent = parent.ParentElement;
                }
                old = (RectLayer)((RectLayer)parent).Clone();
            }


            IsLocked = !IsLocked;


            RectLayer newValue = new RectLayer();
            IElement newParent = Element.ParentElement;
            while (newParent.ParentElement != null)
            {
                newParent = newParent.ParentElement;
            }
            newValue = (RectLayer)((RectLayer)newParent).Clone();
            PrePropertyChangedEventArgs actionValue = new PrePropertyChangedEventArgs() { PropertyName = "ElementCollection", OldValue = old, NewValue = newValue};
            Messenger.Default.Send<PrePropertyChangedEventArgs>(actionValue, MsgToken.MSG_RECORD_ACTIONVALUE);
        }
        protected virtual void OnVisibility()
        {
            if (Element.ParentElement.Visible == Visibility.Hidden)
            {
                return;
            }

            IElement parent = Element.ParentElement;
            RectLayer old = new RectLayer();
            if (parent == null && Element is RectLayer)
            {
                old = (RectLayer)((RectLayer)Element).Clone();
            }
            else
            {
                while (parent.ParentElement != null)
                {
                    parent = parent.ParentElement;
                }
                old = (RectLayer)((RectLayer)parent).Clone();
            }


            if (Visible == Visibility.Visible)
            {
                Visible = Visibility.Hidden;
            }
            else if(Visible==Visibility.Hidden)
            {
                Visible = Visibility.Visible;
            }


            RectLayer newValue = new RectLayer();
            IElement newParent = Element.ParentElement;
            while (newParent.ParentElement != null)
            {
                newParent = newParent.ParentElement;
            }
            newValue = (RectLayer)((RectLayer)newParent).Clone();
            PrePropertyChangedEventArgs actionValue = new PrePropertyChangedEventArgs() { PropertyName = "ElementCollection", OldValue = old, NewValue = newValue };
            Messenger.Default.Send<PrePropertyChangedEventArgs>(actionValue, MsgToken.MSG_RECORD_ACTIONVALUE);
    
        }
        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MyLockAndVisibleButton control = (MyLockAndVisibleButton)sender;

            // When someone click on a part in the NumericUpDown and it's not focusable
            // NumericUpDown needs to take the focus in order to process keyboard correctly
            if (!control.IsKeyboardFocusWithin)
            {
                e.Handled = control.Focus() || e.Handled;
            }
        }
        #endregion
    }

}
