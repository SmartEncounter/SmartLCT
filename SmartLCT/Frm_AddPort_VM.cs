using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Nova.SmartLCT.Interface;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Nova.LCT.GigabitSystem.Common;
using GalaSoft.MvvmLight;
using System.Windows;

namespace SmartLCT
{
    public class Frm_AddPort_VM : SmartLCTViewModeBase
    {
		#region 属性
        public ObservableCollection<ComboBoxDataSet> SenderInfoList
        {
            get { return _senderInfoList; }
            set
            {
                _senderInfoList = value;
                if (_senderInfoList != null &&
                    _senderInfoList.Count > 0)
                {
                    SelectedSenderInfoValue = (SenderInfo)_senderInfoList[0].Data;
                    IsEnabled = true;
                }
                else
                {
                    IsEnabled = false;
                }
                NotifyPropertyChanged(GetPropertyName(o => this.SenderInfoList));
            }
        }
        private ObservableCollection<ComboBoxDataSet> _senderInfoList = new ObservableCollection<ComboBoxDataSet>();
        public SenderInfo SelectedSenderInfoValue
        {
            get { return _selectedSenderInfoValue; }
            set
            {
                _selectedSenderInfoValue = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SelectedSenderInfoValue));
            }
        }
        private SenderInfo _selectedSenderInfoValue = new SenderInfo();

        public int PortIndex
        {
            get { return _portIndex; }
            set 
            {
                _portIndex = value;
                NotifyPropertyChanged(GetPropertyName(o => this.PortIndex));
            }
        }
        private int _portIndex = 1;
        public RectLayer Layer
        {
            get { return _layer; }
            set
            {
                _layer = value;
                NotifyPropertyChanged(GetPropertyName(o => this.Layer));
            }
        }
        private RectLayer _layer = null;

        public bool IsEnabledSelectedSender
        {
            get { return _isEnabledSelectedSender; }
            set
            {
                _isEnabledSelectedSender = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsEnabledSelectedSender));
            }
        }
        private bool _isEnabledSelectedSender = true;

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
        public RelayCommand CmdAddPort
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
        public Frm_AddPort_VM()
        {
            CmdClosed = new RelayCommand(OnCmdClosedAddPortForm);
            CmdAddPort = new RelayCommand(OnCmdAddPort);
            CmdCancel = new RelayCommand(OnCmdCancel);

        }
        private void OnCmdCancel()
        {
            //关闭窗口
            Messenger.Default.Send<bool>(true, MsgToken.MSG_CLOSE_ADDPORTFORM);
        }
        private void OnCmdAddPort()
        {
            if (SelectedSenderInfoValue.PortCollection.Count >= 6)
            {
                string msg = "";
                CommonStaticMethod.GetLanguageString("最多只能添加6个网口！", "Lang_SmartLCT_FrmAddPortVM_PortIsMaxCount", out msg);
                ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);
            }
            else if (SelectedSenderInfoValue.PortCollection.Contains(PortIndex-1))
            {
                string msg = "";
                CommonStaticMethod.GetLanguageString("网口", "Lang_SmartLCT_FrmAddPortVM_Port", out msg);
                msg += PortIndex.ToString();
                string msgIsExit = "";
                CommonStaticMethod.GetLanguageString("已经存在，请重新填写网口号", "Lang_SmartLCT_FrmAddPortVM_PortIsExit", out msgIsExit);
                msg += msgIsExit;
                ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);
            }
            else
            {
                AddPortInfo info = new AddPortInfo(SelectedSenderInfoValue,PortIndex-1,Layer);
                SelectedSenderInfoValue.PortCollection.Add(PortIndex-1);
                 Messenger.Default.Send<AddPortInfo>(info, MsgToken.MSG_ADDPORT);
                 if (Layer != null)
                 {
                     OnCmdCancel();
                }
            }
            //添加的信息传出去
           // Messenger.Default.Send<>(false, MsgToken.MSG_ADDPORT);
        }
        private void OnCmdClosedAddPortForm()
        {
            Messenger.Default.Send<bool>(true, MsgToken.MSG_CLOSE_ADDPORTFORM);
        }
        //eventcommand绑定窗口closed事件，后将信息发送出去即可
        public class TestInfo
        {
            public string Name
            {
                get;
                set;
            }
            public int Age
            {
                get;
                set;
            }
            public int PhoneNum
            {
                get;
                set;
            }
        }
    }
}
