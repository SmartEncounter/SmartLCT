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
using Nova.SmartLCT.Interface;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;

namespace Nova.SmartLCT.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WinBrightAdjust : CustomWindow
    {

        public WinBrightAdjust()
        {
            InitializeComponent();
            //Messenger.Default.Register<string>(this, MsgToken.MSG_WINBRIGHTADJUST_CLOSE, CloseDialog);

        }

        //public WinBrightAdjust(ObservableCollection<BrightTimingAdjustData> timingAdjustBrightCollection)
        //{
        //    InitializeComponent();

        //}
        //private void CloseDialog(string msg)
        //{
        //    if (msg == MsgToken.MSG_WINBRIGHTADJUST_CANCEL)
        //    {
        //        DialogResult = false;
        //    }
        //    else
        //    {
        //        DialogResult = true;
        //    }
        //    this.Close();
        //}
    }
}
