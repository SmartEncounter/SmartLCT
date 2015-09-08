using GalaSoft.MvvmLight.Messaging;
using Nova.SmartLCT.Interface;
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

namespace SmartLCT
{
    /// <summary>
    /// Win_LanguageSelect.xaml 的交互逻辑
    /// </summary>
    public partial class Win_LanguageSelect : CustomWindow
    {
        public string SelectedLangFlag
        {
            get
            {
                return _vm.SelectedLangFlag;
            }
            set
            {
                _selectedLangFlag = value;
                _vm.SelectedLangFlag = value;
            }
        }
        private string _selectedLangFlag = "";

        private Win_LanguageSelect_VM _vm = null;
        public Win_LanguageSelect()
        {
            InitializeComponent();
            _vm = (Win_LanguageSelect_VM)FindResource("Win_LanguageSelect_VMDataSource");
            Messenger.Default.Register<string>(this, MsgToken.MSG_CLOSELANGUAGE_SEL, OnClose);
        }

        private void OnClose(string msg)
        {
            this.Close();
        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
        }

        private void button_ok_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
        }

        private void CustomWindow_Closed(object sender, EventArgs e)
        {
            Messenger.Default.Unregister<string>(this, MsgToken.MSG_CLOSELANGUAGE_SEL, OnClose);
        }
    }
}
