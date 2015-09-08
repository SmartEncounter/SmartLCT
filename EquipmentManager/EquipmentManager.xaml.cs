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
using Nova.LCT.Message.Client;
using GalaSoft.MvvmLight.Messaging;
using Nova.SmartLCT.Interface;
using Nova.LCT.GigabitSystem.Common;
using System.Windows.Threading;
using System.Threading;

namespace Nova.SmartLCT.UI
{
    public partial class EquipmentManager : CustomWindow
    {

        private EquipmentManager_VM _mainVM = null;

        public EquipmentManager()
        {
            InitializeComponent();
            _mainVM = (EquipmentManager_VM)FindResource("EquipmentManager_VMDataSource");
        }

        public void SetServerProxy(LCTServerMessageProxy serverProxy,bool isSendDataWindow,
            List<SenderRedundancyInfo> senderReduInfoList,SenderProperty senderProp,
            List<ILEDDisplayInfo> oldDisplayList,GraphicsDVIPortInfo graphicsDviInf)
        {

        }



        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
