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
using Nova.LCT.Message.Client;
using Nova.SmartLCT.Interface;

namespace Nova.SmartLCT.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SendEDIDManager : CustomWindow
    {
        SendEDIDManager_VM _set = null;

        public SendEDIDManager()
        {
            InitializeComponent();
            _set = (SendEDIDManager_VM)FindResource("SetInfoToHWDataSource");
        }

        private void button_Refash_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// 在此处添加事件处理程序实现。
        }
    }
}
