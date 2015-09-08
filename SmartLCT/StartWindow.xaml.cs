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
using GalaSoft.MvvmLight.Messaging;
using Nova.SmartLCT.Interface;

namespace SmartLCT
{
    /// <summary>
    /// StartWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StartWindow : CustomWindow
    {
        private StartWindow_VM _vm=null;
        
        public StartWindow()
        {
            InitializeComponent();
            //Messenger.Default.Register<ConfigurationData>(this, MsgToken.MSG_CLOSESTARTWINDOW, OnClosed);

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _vm = (StartWindow_VM)this.FindResource("StartWindow_VMDataSource");

          
        }
        public ConfigurationData GetConfigData()
        {
            return _vm.ConfigData;
        }
        private void OnClosed(ConfigurationData data)
        {
            if (data != null)
            {
                //this.Hide();
            }
            else
            {
                return;
            }
            if (data.OperateType == OperateScreenType.CreateScreenAndOpenConfigFile)
            {
                if (data.ProjectLocationPath != "")
                {
                   // this.Hide();

                    MainWindow win = new MainWindow(data);
                    win.Owner = this;
                    win.ShowDialog();
                }
            }
            else if (data.OperateType == OperateScreenType.CreateScreen)
            {
                MainWindow win = new MainWindow(data);
                win.Owner = this;
                win.ShowDialog();
            }
            //else if (data.OperateType = OperateScreenType.CreateScreen)
            //{
            //    this.Hide();
            //}
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            _vm.ProductTimer.Stop();
            Messenger.Default.Unregister<ConfigurationData>(this, MsgToken.MSG_CLOSESTARTWINDOW, OnClosed);
            System.Environment.Exit(0);
            //MainWindow win = new MainWindow();
            //win.ShowDialog();
        }

    }
}
