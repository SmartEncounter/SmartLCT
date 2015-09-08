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
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Nova.SmartLCT.Interface;

namespace Test
{
    /// <summary>
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class Window2 : Window
    {
        Window2_VM _vm = null;
        public Window2()
        {
            InitializeComponent();
            _vm = (Window2_VM)FindResource("Window2_VMDataSource");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //object obj = propertyPanel1.DataContext;
        }
    }
}
