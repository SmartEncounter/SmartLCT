using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Nova.SmartLCT.Interface;
using GalaSoft.MvvmLight.Messaging;
using Nova.LCT.GigabitSystem.Common;

namespace Nova.SmartLCT.UI
{
	/// <summary>
	/// Window_AddBrightTimingAdjustData.xaml 的交互逻辑
	/// </summary>
	public partial class Window_AddTimingAdjustData : CustomWindow
	{
        private Window_AddTimingAdjustData_VM _vm = null;

        public Window_AddTimingAdjustData(OneSmartBrightEasyConfig config)
        {
            this.InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _vm = (Window_AddTimingAdjustData_VM)FindResource("Window_AddTimingAdjustData_VMDataSource");
            DataContext = _vm;
            if (config != null)
            {
                _vm.OneSmartBrightItem = config;
            }
            Messenger.Default.Register<string>(this, MsgToken.MSG_ADDBRIGHTTIMINGADJUSTDATA_CLOSE, CloseDialog);
            // 在此点之下插入创建对象所需的代码。
        }

        public OneSmartBrightEasyConfig GetOneSmartBrightOneItemConfig()
        {
            return _vm.OneSmartBrightItem;
        }

        private void CloseDialog(string msg)
        {
            if (msg == MsgToken.MSG_ADDBRIGHTTIMINGADJUSTDATA_CANCEL)
            {
                DialogResult = false;
            }
            else
            {
                DialogResult = true;
            }
            this.Close();
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            Messenger.Default.Unregister<string>(this, MsgToken.MSG_ADDBRIGHTTIMINGADJUSTDATA_CLOSE, CloseDialog);

        }


	}
}