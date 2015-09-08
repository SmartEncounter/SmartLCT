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
using Nova.SmartLCT.Interface;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using Nova.LCT.GigabitSystem.Common;

namespace SmartLCT
{
    /// <summary>
    /// Frm_AddSender.xaml 的交互逻辑
    /// </summary>
    public partial class Frm_AddSender : Window
    {
        public Frm_AddSender()
        {
            InitializeComponent();
        }
        Frm_AddSender_VM _addSender_VM = null;
        public Frm_AddSender(SenderCollectionInfo senderCollectionInfo)
        {
            InitializeComponent();
            _addSender_VM = (Frm_AddSender_VM)this.FindResource("Frm_AddSender_VMDataSource");
            Messenger.Default.Register<bool>(this, MsgToken.MSG_CLOSE_ADDSENDERFORM, OnCloseAddSenderForm);
            _addSender_VM.SenderInfoCollection = senderCollectionInfo.SenderCollection;
            _addSender_VM.Layer = senderCollectionInfo.Layer;
        }
        private void OnCloseAddSenderForm(bool isOK)
        {
            Messenger.Default.Unregister<bool>(this, MsgToken.MSG_CLOSE_ADDSENDERFORM, OnCloseAddSenderForm);
            this.Close();
        }
    }
}
