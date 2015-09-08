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
using System.Diagnostics;
using Nova.SmartLCT.Interface;

namespace Nova.SmartLCT.UI
{
    /// <summary>
    /// Window__ImportCfgFile.xaml 的交互逻辑
    /// </summary>
    public partial class Window_ImportCfgFile : CustomWindow
    {
        Window_ImportCfgFile_VM _window_ImportCfgFile_VM = null;

        public Window_ImportCfgFile()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _window_ImportCfgFile_VM = (Window_ImportCfgFile_VM)FindResource("Window_ImportCfgFile_VMDataSource");
            DataContext = _window_ImportCfgFile_VM;
            Messenger.Default.Register<string>(this, MsgToken.MSG_INPORTFILE_CLOSE, msg =>
            {
                this.Close();
            });
            Messenger.Default.Register<string>(this, MsgToken.MSG_INPORTFILE_CLOSE, msg =>
            {
                this.Close();
            });
        }

        public void Initializes(NotificationMessageAction<ObservableCollection<DataGradItemInfo>> nsa)
        {
            _window_ImportCfgFile_VM.Initializes(nsa);
        }
    }
}
