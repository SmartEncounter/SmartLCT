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
	/// Window_FastSegmentation.xaml 的交互逻辑
	/// </summary>
	public partial class Window_FastSegmentation : CustomWindow
	{
        private Window_FastSegmentation_VM _vm = null;

		public Window_FastSegmentation()
		{
			this.InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _vm = (Window_FastSegmentation_VM)FindResource("Window_FastSegmentation_VMDataSource");           
			// 在此点之下插入创建对象所需的代码。
		}
        public Window_FastSegmentation(FastSegmentParam info)
        {
            this.InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _vm = (Window_FastSegmentation_VM)FindResource("Window_FastSegmentation_VMDataSource");
            _vm.FastSegmentParam = info;
            Messenger.Default.Register<string>(this, MsgToken.MSG_WINFASTSEGMENTATION_CLOSE, CloseDialog);
        }

        private void CloseDialog(string msg)
        {
            if (msg == MsgToken.MSG_WINFASTSEGMENTATION_CANCEL)
            {
                DialogResult = false;
            }
            else
            {
                DialogResult = true;
            }
            this.Close();
        }

        public FastSegmentParam GetFastSegmentationInfo()
        {
            return _vm.FastSegmentParam ;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Messenger.Default.Unregister<string>(this, MsgToken.MSG_WINFASTSEGMENTATION_CLOSE, CloseDialog);

        }


	}
}