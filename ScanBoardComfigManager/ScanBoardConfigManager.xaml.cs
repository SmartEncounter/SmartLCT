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
using GalaSoft.MvvmLight.Messaging;
using Nova.SmartLCT.Interface;
using System.Collections.ObjectModel;
using Nova.SmartLCT.UI;

namespace Nova.SmartLCT.UI
{
    public partial class ScanBoardConfigManager : CustomWindow
    {
        private ScanBoardConfigManager_VM _mainWindow_VM = null;

        public ScanBoardConfigManager()
        {
            InitializeComponent();
            _mainWindow_VM = (ScanBoardConfigManager_VM)FindResource("ScanBoardConfigManager_VMDataSource");
            DataContext = _mainWindow_VM;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Messenger.Default.Register<NotificationMessageAction<ObservableCollection<DataGradItemInfo>>>(this, MsgToken.MSG_IMPORTCONFIGFOLE, ShowImportWindow);
            Messenger.Default.Register<NotificationMessageAction<ObservableCollection<DataGradItemInfo>>>(this, MsgToken.MSG_EXPORTCONFIGFOLE, ShowExportWindow);
            Messenger.Default.Register<NotificationMessageAction<ButtonClickType>>(this, MsgToken.MSG_DELETEMSG_MSGBOX, ShowMsgBox);

        }

        private void ShowMsgBox(NotificationMessageAction<ButtonClickType> msg)
        {
            if (msg.Notification == MsgToken.MSG_DELETEMSG_MSGBOX)
            {
                NotificationMessageAction<ButtonClickType> msgs = msg;
                 ButtonClickType butType= (ButtonClickType)msg.Sender;
                if (MessageBox.Show(msg.Target.ToString(), "", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    butType = ButtonClickType.Ok;
                }
                else
                {
                    butType = ButtonClickType.Cancel;
                }
                if (msgs != null)
                {
                    msgs.Execute(butType);
                }
            }
        }

        private void ShowImportWindow(NotificationMessageAction<ObservableCollection<DataGradItemInfo>> msg)
        {
            if (msg.Notification == MsgToken.MSG_IMPORTCONFIGFOLE)
            {
                Window_ImportCfgFile importCfgFileWin = new Window_ImportCfgFile();
                importCfgFileWin.Initializes(msg);
                importCfgFileWin.Owner = this;
                importCfgFileWin.ShowDialog();
            }
        }

        private void ShowExportWindow(NotificationMessageAction<ObservableCollection<DataGradItemInfo>> msg)
        {
            if (msg.Notification == MsgToken.MSG_EXPORTCONFIGFOLE)
            {
                Window_ExportCfgFile exportCfgFileWin = new Window_ExportCfgFile();
                exportCfgFileWin.Initializes(msg);
                exportCfgFileWin.Owner = this;
                exportCfgFileWin.ShowDialog();
            }
        }
        
        private void Window_Closed(object sender, EventArgs e)
        {
            Messenger.Default.Unregister<NotificationMessageAction<ObservableCollection<DataGradItemInfo>>>(this, MsgToken.MSG_IMPORTCONFIGFOLE, ShowImportWindow);
            Messenger.Default.Unregister<NotificationMessageAction<ObservableCollection<DataGradItemInfo>>>(this, MsgToken.MSG_EXPORTCONFIGFOLE, ShowExportWindow);
            Messenger.Default.Unregister<NotificationMessageAction<ButtonClickType>>(this, MsgToken.MSG_DELETEMSG_MSGBOX, ShowMsgBox);                            
        }

        private void bt_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
