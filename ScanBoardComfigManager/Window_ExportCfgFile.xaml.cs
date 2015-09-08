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
    /// Window_ExportCfgFile.xaml 的交互逻辑
    /// </summary>
    public partial class Window_ExportCfgFile : CustomWindow
    {
        private Window_ExportCfgFile_VM _window_ExportCfgFile_VM = null;
        public Window_ExportCfgFile()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _window_ExportCfgFile_VM = (Window_ExportCfgFile_VM)FindResource("Window_ExportCfgFile_VMDataSource");
            Messenger.Default.Register<string>(this, MsgToken.MSG_EXPORTFILE_CLOSE, msg =>
            {
                this.Close();
            });
            Messenger.Default.Register<string>(this, MsgToken.MSG_EXPORTFILE_CLOSE, msg =>
            {
                this.Close();
            });
        }
        public void Initializes(NotificationMessageAction<ObservableCollection<DataGradItemInfo>> nsa)
        {
            _window_ExportCfgFile_VM.Initializes(nsa);
        }
    }
}
