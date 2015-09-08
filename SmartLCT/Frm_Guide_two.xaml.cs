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
using GalaSoft.MvvmLight.Messaging;
using Nova.SmartLCT.Interface;

namespace SmartLCT
{
	/// <summary>
	/// Frm_Guide_two.xaml 的交互逻辑
	/// </summary>
	public partial class Frm_Guide_two : CustomWindow
	{
        private Frm_Guide_two_VM _vm = null;
        private bool isAutoColse = true;
		public Frm_Guide_two()
		{
			this.InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Messenger.Default.Register<object>(this, MsgToken.MSG_CLOSE_GUIDETWOFORM, OnCloseForm);
            //Messenger.Default.Register<ConfigurationData>(this, MsgToken.MSG_CREATEANDSHOWSCREEN, OnCreateAndShowScreen);
            _vm = (Frm_Guide_two_VM)this.FindResource("Frm_Guide_two_VMDataSource");
			// 在此点之下插入创建对象所需的代码。
		}
        public Frm_Guide_two(OperateScreenType type)
        {
            this.InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Messenger.Default.Register<object>(this, MsgToken.MSG_CLOSE_GUIDETWOFORM, OnCloseForm);
            //Messenger.Default.Register<ConfigurationData>(this, MsgToken.MSG_CREATEANDSHOWSCREEN, OnCreateAndShowScreen);
            _vm = (Frm_Guide_two_VM)this.FindResource("Frm_Guide_two_VMDataSource");
            _vm.OperateType = type;
            // 在此点之下插入创建对象所需的代码。
        }
        private void OnCreateAndShowScreen(ConfigurationData data)
        {
            this.Hide();
            Messenger.Default.Unregister<object>(this, MsgToken.MSG_CLOSE_GUIDETWOFORM, OnCloseForm);
            //Messenger.Default.Unregister<ConfigurationData>(this, MsgToken.MSG_CREATEANDSHOWSCREEN, OnCreateAndShowScreen);

            //MainWindow win = new MainWindow(data);
            //win.ShowDialog();
        }
        public ConfigurationData GetConfigData()
        {
            return _vm.ConfigData;
        }
        private void OnCloseForm(object sender)
        {
            isAutoColse = false;
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Messenger.Default.Unregister<object>(this, MsgToken.MSG_CLOSE_GUIDETWOFORM, OnCloseForm);
            //Messenger.Default.Unregister<ConfigurationData>(this, MsgToken.MSG_CREATEANDSHOWSCREEN, OnCreateAndShowScreen);
            if (isAutoColse)
            {
                _vm.ConfigData = null;
            }
            //if (isAutoColse && _vm.OperateType==OperateScreenType.CreateScreen)
            //{
            //    MainWindow win = new MainWindow();
            //    win.ShowDialog();
            //}
            //if (_vm.ConfigData != null)
            //{
            //    this.DialogResult = true;
            //}
        }
	}
}