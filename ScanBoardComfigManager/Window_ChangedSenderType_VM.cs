using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Messaging;
using Nova.SmartLCT.Interface;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;

namespace Nova.SmartLCT.UI
{
    public class Window_ChangedSenderType_VM:SmartLCTViewModeBase
    {
        public SenderConfigInfo CurrentSenderConfigInfo
        {
            get
            {
                return _currentSenderConfigInfo;
            }
            set
            {
                _currentSenderConfigInfo = value;
                RaisePropertyChanged("CurrentSenderConfigInfo");
            }
        }
        private SenderConfigInfo _currentSenderConfigInfo = new SenderConfigInfo();
        public NotificationMessageAction<SenderConfigInfo> SenderConfigInfoNsa
        {
            get { return _senderConfigInfoNsa; }
            set
            {
                _senderConfigInfoNsa = value;
                RaisePropertyChanged("SenderConfigInfoNsa");
            }
        }
        private NotificationMessageAction<SenderConfigInfo> _senderConfigInfoNsa = null;
        public ObservableCollection<SenderConfigInfo> SenderConfigCollection
        {
            get { return _senderConfigCollection; }
            set
            {
                _senderConfigCollection = value;
                RaisePropertyChanged("SenderConfigCollection");
            }
        }
        private ObservableCollection<SenderConfigInfo> _senderConfigCollection = new ObservableCollection<SenderConfigInfo>();

        #region 命令
        public RelayCommand CmdButton_OK
        {
            get;
            private set;
        }
        public RelayCommand CmdButton_Cancel
        {
            get;
            private set;
        }
        #endregion
        public Window_ChangedSenderType_VM()
        {
            CmdButton_OK = new RelayCommand(OnCmdButton_Ok);
            CmdButton_Cancel = new RelayCommand(OnCmdButton_Cancel);

            SenderConfigCollection = _globalParams.SenderConfigCollection;
            if (SenderConfigCollection != null && SenderConfigCollection.Count != 0)
            {
                CurrentSenderConfigInfo = SenderConfigCollection[0];
            }
        }

        private void OnCmdButton_Ok()
        {
            _senderConfigInfoNsa.Execute(CurrentSenderConfigInfo);
            Messenger.Default.Send(MsgToken.MSG_SHOWCHANGEDSENDETTYPE_OK, MsgToken.MSG_SHOWCHANGEDSENDETTYPE_CLOSE);
        }

        public void OnCmdButton_Cancel()
        {
            Messenger.Default.Send(MsgToken.MSG_SHOWCHANGEDSENDETTYPE_CANCEL, MsgToken.MSG_SHOWCHANGEDSENDETTYPE_CLOSE);
        }

    }
}
