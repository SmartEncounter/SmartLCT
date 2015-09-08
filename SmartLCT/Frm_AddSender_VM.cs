using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Nova.LCT.GigabitSystem.Common;
using Nova.SmartLCT.Interface;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;

namespace SmartLCT
{
    public class Frm_AddSender_VM:SmartLCTViewModeBase
    {
        #region 属性
        public ObservableCollection<int> SenderInfoCollection
        {
            get { return _senderInfoCollection; }
            set
            {
                _senderInfoCollection = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SenderInfoCollection));
            }
        }
        private ObservableCollection<int> _senderInfoCollection = new ObservableCollection<int>();

        public RectLayer Layer
        {
            get { return _layer; }
            set
            {
                _layer = value;
                if (_layer.ConnectedIndex == -1)
                {
                    IsEnabled = false;
                }
                else
                {
                    IsEnabled = true;
                }
                NotifyPropertyChanged(GetPropertyName(o => this.Layer));
            }
        }
        private RectLayer _layer = null;
        public int SenderIndex
        {
            get { return _senderIndex; }
            set
            {
                _senderIndex = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SenderIndex));
            }
        }
        private int _senderIndex = 1;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsEnabled));
            }
        }
        private bool _isEnabled = true;
        #endregion
        #region 命令
        public RelayCommand CmdAddSender
        {
            get;
            private set;
        }
        public RelayCommand CmdCancel
        {
            get;
            private set;
        }
        public RelayCommand CmdClosed
        {
            get;
            private set;
        }
        #endregion
        #region 字段
        #endregion
        public Frm_AddSender_VM()
        {
            CmdClosed = new RelayCommand(OnCmdClosedAddSenderForm);
            CmdAddSender = new RelayCommand(OnCmdAddSender);
            CmdCancel = new RelayCommand(OnCmdCancel);
        }

        private void OnCmdClosedAddSenderForm()
        {
            Messenger.Default.Send<bool>(true, MsgToken.MSG_CLOSE_ADDSENDERFORM);
        }
        private void OnCmdAddSender()
        {
            if (SenderInfoCollection.Count >= 10)
            {
                string msg = "";
                CommonStaticMethod.GetLanguageString("最多只能添加10个发送卡！", "Lang_SmartLCT_FrmAddSenderVM_SenderIsMaxCount", out msg);
                ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);
            }
            else if (SenderInfoCollection.Contains(SenderIndex-1))
            {
                string msg = "";
                CommonStaticMethod.GetLanguageString("发送卡", "Lang_SmartLCT_FrmAddSenderVM_Sender", out msg);
                msg += SenderIndex.ToString();
                string msgIsExit = "";
                CommonStaticMethod.GetLanguageString("已经存在，请重新填写发送卡号", "Lang_SmartLCT_FrmAddSenderVM_SenderIsExit", out msgIsExit);
                msg += msgIsExit;
                ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);
            }
            else
            {
                if (Layer.EleType != ElementType.sender)
                {
                    Messenger.Default.Send<AddSenderInfo>(new AddSenderInfo(SenderIndex - 1, null), MsgToken.MSG_ADDSENDER);
                }
                else
                {
                    Messenger.Default.Send<AddSenderInfo>(new AddSenderInfo(SenderIndex - 1, Layer), MsgToken.MSG_ADDSENDER);
                }
                SenderInfoCollection.Add(SenderIndex-1);
                if (Layer.EleType == ElementType.sender)
                {
                    OnCmdCancel();
                }
            }
        }
        private void OnCmdCancel()
        {
            Messenger.Default.Send<bool>(true, MsgToken.MSG_CLOSE_ADDSENDERFORM);

        }
    }
}
