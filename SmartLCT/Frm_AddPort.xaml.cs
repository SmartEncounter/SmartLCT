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
using System.Collections.ObjectModel;
using Nova.SmartLCT.Interface;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using Nova.LCT.GigabitSystem.Common;

namespace SmartLCT
{
    /// <summary>
    /// Frm_AddPort.xaml 的交互逻辑
    /// </summary>
    public partial class Frm_AddPort : Window
    {
        public Frm_AddPort()
        {
            InitializeComponent();
        }
        private Frm_AddPort_VM _addPort_VM = null;
        public Frm_AddPort(SenderAndPortInfo senderAndPortInfo)
        {
            InitializeComponent();

            _addPort_VM = (Frm_AddPort_VM)this.FindResource("Frm_AddPort_VMDataSource");
            Messenger.Default.Register<bool>(this, MsgToken.MSG_CLOSE_ADDPORTFORM, OnCloseAddPortForm);
            ObservableCollection<ComboBoxDataSet> senderDataInfo = new ObservableCollection<ComboBoxDataSet>();
            for (int i = 0; i < senderAndPortInfo.SenderInfoList.Count; i++)
            {
                SenderInfo info = senderAndPortInfo.SenderInfoList[i];
                ComboBoxDataSet ds = new ComboBoxDataSet(info.DisplayName,info);
                senderDataInfo.Add(ds);
            }
            _addPort_VM.SenderInfoList = senderDataInfo;
            _addPort_VM.Layer = senderAndPortInfo.Layer;
            _addPort_VM.IsEnabledSelectedSender = senderAndPortInfo.IsEnabledSelectedSender;
         }

        private void OnCloseAddPortForm(bool isOK)
        {
            Messenger.Default.Unregister<bool>(this, MsgToken.MSG_CLOSE_ADDPORTFORM, OnCloseAddPortForm);
            this.Close();
        }

    }
}
