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

namespace Nova.SmartLCT.UI
{
    /// <summary>
    /// ChangedSenderType.xaml 的交互逻辑
    /// </summary>
    public partial class Window_ChangedSenderType : Window
    {
        private Window_ChangedSenderType_VM _vm = null;
        public Window_ChangedSenderType()
        {
            InitializeComponent();
            Messenger.Default.Register<SenderConfigInfo>(this, MsgToken.MSG_SHOWCHANGEDSENDETTYPE_CLOSE, msg =>
            {
                this.Close();
            });
        }
        public Window_ChangedSenderType(NotificationMessageAction<SenderConfigInfo> info)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _vm = (Window_ChangedSenderType_VM)FindResource("Window_ChangedSenderType_VMDataSource");
            DataContext = _vm;
            _vm.SenderConfigInfoNsa = info;

            Messenger.Default.Register<string>(this, MsgToken.MSG_SHOWCHANGEDSENDETTYPE_CLOSE, msg =>
            {
                this.Close();
            });
        }
        
    }
}
