using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using Nova.SmartLCT.Interface;
using GalaSoft.MvvmLight.Messaging;

namespace SmartLCT
{
    public partial class Frm_AddPort : Form
    {
        public Frm_AddPort()
        {
            InitializeComponent();
        }
        private  Frm_AddPort_VM _addPort_VM = new Frm_AddPort_VM();
        public Frm_AddPort(ObservableCollection<RectLayer> rectLayerCollection)
        {
            InitializeComponent();
            Messenger.Default.Register<bool>(this, MsgToken.MSG_CLOSEADDPORTSTATE, OnCloseAddPortStateChanged);       
            _addPort_VM.RectLayerColletion = rectLayerCollection;
            
        }
        private void OnCloseAddPortStateChanged(bool isOK)
        {
            _addPort_VM.IsOK = isOK;
        }
    }
}
