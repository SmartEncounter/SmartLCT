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
using System.Collections.ObjectModel;
using Nova.SmartLCT.Interface;

namespace Nova.SmartLCT.UI
{
    /// <summary>
    /// Window_SetCustomReceive.xaml 的交互逻辑
    /// </summary>
    public partial class Window_SetCustomReceive : CustomWindow
    {
        Window_SetCustomReceive_VM _window_SetCustomReceive_VM = null;

        public Window_SetCustomReceive()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _window_SetCustomReceive_VM = (Window_SetCustomReceive_VM)FindResource("Window_SetCustomReceive_VMDataSource");
            DataContext = _window_SetCustomReceive_VM;
            Messenger.Default.Register<string>(this, MsgToken.MSG_SETCUSTOMRECEIVE_CLOSE, CloseDialog);
        }
        public Window_SetCustomReceive(int width,int height)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _window_SetCustomReceive_VM = (Window_SetCustomReceive_VM)FindResource("Window_SetCustomReceive_VMDataSource");
            DataContext = _window_SetCustomReceive_VM;
            _window_SetCustomReceive_VM.Height = height;
            _window_SetCustomReceive_VM.Width = width;
            Messenger.Default.Register<string>(this, MsgToken.MSG_SETCUSTOMRECEIVE_CLOSE, CloseDialog);
        }
        private void CloseDialog(string msg)
        {
            if (msg == MsgToken.MSG_SETCUSTOMRECEIVEFILE_CANCEL)
            {
                DialogResult = false;
            }
            else
            {
                DialogResult = true;
            }
            this.Close();
        }

        public void GetSetSize(out int width, out int height)
        {
            width = _window_SetCustomReceive_VM.Width;
            height = _window_SetCustomReceive_VM.Height;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Messenger.Default.Unregister<string>(this, MsgToken.MSG_SETCUSTOMRECEIVE_CLOSE, CloseDialog);

        }

    }
}
