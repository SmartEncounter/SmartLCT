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
using System.Windows.Media.Animation;
using GalaSoft.MvvmLight.Messaging;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Nova.SmartLCT.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Win_ProgressBar : Window
    {

        public Win_ProgressBar()
        {
            InitializeComponent();
            _winHandle = new WindowInteropHelper(this).Handle;
        }

         [DllImport("user32.dll")]
        public extern static bool EnableWindow(IntPtr winHandler, bool enable);

        #region 属性
        public string ProcessingMsg
        {
            get { return (string)label_Test.Content; }
            set { label_Test.Content = value; }
        }
        #endregion

        private IntPtr _winHandle = IntPtr.Zero;

        private void Window_Closed(object sender, EventArgs e)
        {
            IntPtr ownerHandle = new WindowInteropHelper(this).Owner;
            if (ownerHandle != IntPtr.Zero)
            {
                EnableWindow(ownerHandle, true);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr ownerHandle = new WindowInteropHelper(this).Owner;
            if (ownerHandle != IntPtr.Zero)
            {
                EnableWindow(ownerHandle, false);
            }
        }
    }
}
