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
using Nova.Wpf.Control;
using System.Diagnostics;

namespace Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public class TestAge : INotifyPropertyChanged
        {
            public int Age
            {
                get
                {
                    return _age;
                }
                set
                {
                    _age = value;
                    OnPropertyChanged("Age");
                }
            }
            private int _age = 10;
        
            #region INotifyPropertyChanged 成员

            public event PropertyChangedEventHandler  PropertyChanged;
            private void OnPropertyChanged(string info)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(info));
                }
            }
            #endregion
        }

        private TestAge _bsTest = new TestAge();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Binding bind = new Binding();
            bind.Source = _bsTest;
            bind.Path = new PropertyPath("Age");
            bind.Mode = BindingMode.TwoWay;
            bind.ValidatesOnDataErrors = false;
            bind.ValidatesOnExceptions = false;
          
            num_test.SetBinding(NumericUpDown.ValueProperty, bind);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            _bsTest.Age += 1;
        }


        private void numericUpDown1_ValueChangedEvent(object sender, ValueChangedEventArgs e)
        {
            Debug.WriteLine("ValueChanged:" + e.Value);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
