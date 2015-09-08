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
using System.ComponentModel;

namespace Nova.Wpf.Control
{
    /// <summary>
    /// NumericUpDown.xaml 的交互逻辑
    /// </summary>
    public partial class NumericUpDown : UserControl, INotifyPropertyChanged
    {
        #region 定义xmal属性
        /// <summary>
        /// 控件的DependencyProperty最大值
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(100.0, MaxMinValueChangedCallBack, MaxValueCoerceCallBack));
        /// <summary>
        /// 控件的DependencyProperty最小值
        /// </summary>
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(0.0, MaxMinValueChangedCallBack, MinValueCoerceCallBack));
        /// <summary>
        /// 控件的DependencyProperty属性值
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(0.0, ValuePropertyChangedCallBack, ValueCoerceCallBack), new ValidateValueCallback(OnValidateValue));
        /// <summary>
        /// 按钮的DependencyProperty步进值
        /// </summary>
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(1.0));
        /// <summary>
        /// 按钮的DependencyProperty值是否溢出
        /// </summary>
        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool), typeof(NumericUpDown), new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty OverflowBgColorProperty =
            DependencyProperty.Register("OverflowBgColor", typeof(Brush), typeof(NumericUpDown), new FrameworkPropertyMetadata(Brushes.Red));

        public static readonly RoutedEvent ValueChangedRoutedEvent =
            EventManager.RegisterRoutedEvent("ValueChangedEvent", RoutingStrategy.Bubble, typeof(RoutedValueChangedHandler), typeof(NumericUpDown));

        public static readonly DependencyProperty IsAutoResetValueProperty =
            DependencyProperty.Register("IsAutoResetValue", typeof(bool), typeof(NumericUpDown), new FrameworkPropertyMetadata(true));
        #endregion

        #region 属性
        /// <summary>
        /// 控件的属性值
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set 
            {
                bool isSame = false;
                if (Value == value)
                {
                    isSame = true;
                }
                SetValue(ValueProperty, value);
                if (!isSame)
                {
                    OnValueChanged(value);
                }
            }
        }
        /// <summary>
        /// 控件的最大值
        /// </summary>
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set 
            {
                if (MaxValue == 11100)
                {
                    int a = 0;
                    int b = 3;
                    a += b;
                }
                SetValue(MaxValueProperty, value); 
            }
        }
        /// <summary>
        /// 控件的最小值
        /// </summary>
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set
            { 
                SetValue(MinValueProperty, value);
            }
        }
        /// <summary>
        /// 步进值
        /// </summary>
        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }
        /// <summary>
        /// 值是否有效
        /// </summary>
        public bool IsValid
        {
            get
            {
                return (bool)GetValue(IsValidProperty);
            }
            set
            {
                SetValue(IsValidProperty, value);
            }
        }
        /// <summary>
        /// 值溢出时显示的颜色
        /// </summary>
        public Brush OverflowBgColor
        {
            get
            {
                return (Brush)GetValue(OverflowBgColorProperty);
            }
            set
            {
                SetValue(OverflowBgColorProperty, value);
            }
        }
        /// <summary>
        /// 是否自动恢复溢出前的值
        /// </summary>
        public bool IsAutoResetValue
        {
            get { return (bool)GetValue(IsAutoResetValueProperty); }
            set
            {
                SetValue(IsAutoResetValueProperty, value);
            }
        }
        #endregion

        #region 字段
        /// <summary>
        /// 保存控件允许的最小值
        /// </summary>
        private double _minValue = 0;
        /// <summary>
        /// 保存控件允许的最大值
        /// </summary>
        private double _maxValue = 100;

        private double _lastValidValue = 0;

        #endregion

        #region 内部方法
        /// <summary>
        /// 属性Value改变时执行的回调函数
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private static void ValuePropertyChangedCallBack(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            bool isValid = true;
            NumericUpDown ctrl = (NumericUpDown)obj;
            double val = (double)e.NewValue;
            if (val < ctrl._minValue)
            {
                isValid = false;
            }
            else if (val > ctrl._maxValue)
            {
                isValid = false;
            }

            ctrl.IsValid = isValid;
        }
        /// <summary>
        /// 属性值验证的时候执行的回调函数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool OnValidateValue(object value)
        {
            return true;
        }

        private static object ValueCoerceCallBack(DependencyObject obj, object value)
        {
            NumericUpDown ctrl = (NumericUpDown)obj;
            double newValue = (double)value;

            if (ctrl.IsAutoResetValue)
            {
                if (newValue > ctrl.MaxValue)
                {
                    return ctrl.MaxValue;
                }
                else if (newValue < ctrl.MinValue)
                {
                    return ctrl.MinValue;
                }
            }
            return newValue;
        }

        /// <summary>
        /// 最大最小值改变后执行的回调
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private static void MaxMinValueChangedCallBack(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown ctrl = (NumericUpDown)obj;
            if (e.Property == MaxValueProperty)
            {
                if (Convert.ToDouble(e.NewValue) == 11100)
                {
                    int a = 0;
                    int b = 3;
                    a+=b;
                }
                ctrl._maxValue = Convert.ToDouble(e.NewValue);
                
            }
            else if (e.Property == MinValueProperty)
            {
                ctrl._minValue = Convert.ToDouble(e.NewValue);
                ctrl._lastValidValue = ctrl._minValue;
            }
            if (ctrl.Value > ctrl.MaxValue)
            {
                ctrl.IsValid = false;
            }
            else if (ctrl.Value < ctrl.MinValue)
            {
                ctrl.IsValid = false;
            }
            else
            {
                ctrl.IsValid = true;
            }
        }

        private static object MaxValueCoerceCallBack(DependencyObject obj, object value)
        {
            NumericUpDown ctrl = (NumericUpDown)obj;
            double newValue = (double)value;

            if (newValue < ctrl.MinValue)
            {
                return ctrl.MinValue;
            }
            return newValue;
        }

        private static object MinValueCoerceCallBack(DependencyObject obj, object value)
        {
            NumericUpDown ctrl = (NumericUpDown)obj;
            double newValue = (double)value;

            if (newValue > ctrl.MaxValue)
            {
                return ctrl.MaxValue;
            }
            return newValue;
        }
        #endregion

        public event RoutedValueChangedHandler ValueChangedEvent
        {
            add 
            {
                base.AddHandler(ValueChangedRoutedEvent, value);
            }
            remove
            {
                base.RemoveHandler(ValueChangedRoutedEvent, value);
            }
        }
        private void OnValueChanged(double value)
        {
            if (IsValid)
            {
                _lastValidValue = value;
            }
            RaiseEvent(new ValueChangedEventArgs(ValueChangedRoutedEvent, this) { Value = value });
        }

        #region 接口
        public void Increment()
        {
            try
            {
                double newValue = Value + Step;
                if (newValue > _maxValue)
                {
                    Value = _maxValue;
                }
                else
                {
                    Value = newValue;
                }
            }
            catch (ArgumentException)
            {
            }
        }

       
        public void Decrement()
        {
            try
            {
                double newValue = Value - Step;
                if (newValue < _minValue)
                {
                    Value = _minValue;
                }
                else
                {
                    Value = newValue;
                }
            }
            catch (ArgumentException)
            {
            }
        }
        #endregion
        private bool _isInit = false;

        public NumericUpDown()
        {
            _isInit = true;
            InitializeComponent();
            _isInit = false;
        }

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
		#endregion
        private void button_up_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Increment();
        }

        private void button_down_Click(object sender, RoutedEventArgs e)
        {
            Decrement();
        }

        private void textBox_value_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isInit)
            {
                return;
            }
            try
            {
                if (!string.IsNullOrEmpty(textBox_Value.Text))
                {
                    
                    if (textBox_Value.Text != "-")
                    {
                        double newValue = double.Parse(textBox_Value.Text);
                        if (newValue > _maxValue || newValue < _minValue)
                        {
                            //string resetText = _maxValue.ToString();
                            //if (newValue > _maxValue)
                            //{
                            //    resetText = _maxValue.ToString();
                            //}
                            //else
                            //{
                            //    resetText = _minValue.ToString();
                            //}
                            //resetText = Value.ToString();
                            //textBox_Value.Text = resetText;
                            //textBox_Value.Select(resetText.Length, resetText.Length);

                        }
                        Value = newValue;

                        BindingExpression exp = this.textBox_Value.GetBindingExpression(TextBox.TextProperty);
                        exp.UpdateSource();

                        textBox_Value.Select(textBox_Value.Text.Length, textBox_Value.Text.Length);
                    }
                }
            }
            catch (System.Exception ex)
            {
                string resetText = Value.ToString();
                textBox_Value.Text = resetText;
                textBox_Value.Select(resetText.Length, resetText.Length);
            }
        }

        private void textBox_Value_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                Increment();
                textBox_Value.Select(textBox_Value.Text.Length, textBox_Value.Text.Length);

            }
            else if (e.Key == Key.Down)
            {
                Decrement();
                textBox_Value.Select(textBox_Value.Text.Length, textBox_Value.Text.Length);

            }

        }

        private void userControl_LostFocus(object sender, System.Windows.RoutedEventArgs e)
       {
        	// 在此处添加事件处理程序实现。
            if (string.IsNullOrWhiteSpace(textBox_Value.Text) || 
                string.IsNullOrEmpty(textBox_Value.Text))
            {
                string resetText = Value.ToString();
                textBox_Value.Text = resetText;
                textBox_Value.Select(resetText.Length, resetText.Length);
            }
            if (IsAutoResetValue &&
                !IsValid)
            {
                string resetText = _lastValidValue.ToString();
                textBox_Value.Text = resetText;
                textBox_Value.Select(resetText.Length, resetText.Length);
            }
        }

        private void ResetLastValue()
        {
           
        }
        
    }
}
