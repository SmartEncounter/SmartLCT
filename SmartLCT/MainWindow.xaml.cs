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
using GalaSoft.MvvmLight.Messaging;
using Nova.SmartLCT.Interface;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace SmartLCT
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : CustomWindow
    {
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        private IntPtr ActiveWindowHandle;  //定义活动窗体的句柄
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetActiveWindow();  //获得父窗体句柄  
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);


        private MainWindow_VM _vm = null;


        public MainWindow()
        {
            InitializeComponent();
            _vm = (MainWindow_VM)this.FindResource("MainWindow_VMDataSource");
            Messenger.Default.Register<object>(this,MsgToken.MSG_EXIT, OnExit);
        }
        public MainWindow(ConfigurationData data)
        {
            InitializeComponent();
            _vm = (MainWindow_VM)this.FindResource("MainWindow_VMDataSource");
            _vm.MyConfigurationData = data;
            Messenger.Default.Register<object>(this, MsgToken.MSG_EXIT, OnExit);
        }
        private void OnExit(object sender)
        {
            this.Close();
            //System.Environment.Exit(0);
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ShowInCenterParent(Window win)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                ActiveWindowHandle = GetActiveWindow();  //获取父窗体句柄  
                WindowInteropHelper helper = new WindowInteropHelper(win);
                helper.Owner = ActiveWindowHandle;

                RECT rect;
                GetWindowRect(ActiveWindowHandle, out rect);

                //win.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

                //win.Left = rect.left + ((rect.right - rect.left) - win.Width) / 2;
                //win.Top = rect.top + ((rect.bottom - rect.top) - win.Height) / 2;
                //win.ShowInTaskbar = false;
                //win.ShowDialog();

                win.SourceInitialized += delegate
                {
                    // Get WPF size and location for non-WPF owner window
                    int nonWPFOwnerLeft = rect.left; // Get non-WPF owner’s Left
                    int nonWPFOwnerWidth = rect.right - rect.left; // Get non-WPF owner’s Width
                    int nonWPFOwnerTop = rect.top; // Get non-WPF owner’s Top
                    int nonWPFOwnerHeight = rect.bottom - rect.top; // Get non-WPF owner’s Height 

                    // Get transform matrix to transform non-WPF owner window
                    // size and location units into device-independent WPF 
                    // size and location units

                    HwndSource source = HwndSource.FromHwnd(helper.Handle);
                    if (source == null) return;
                    Matrix matrix = source.CompositionTarget.TransformFromDevice;
                    Point ownerWPFSize = matrix.Transform(
                      new Point(nonWPFOwnerWidth, nonWPFOwnerHeight));
                    Point ownerWPFPosition = matrix.Transform(
                      new Point(nonWPFOwnerLeft, nonWPFOwnerTop));

                    // Center WPF window
                    win.WindowStartupLocation = WindowStartupLocation.Manual;
                    win.Left = ownerWPFPosition.X + (ownerWPFSize.X - win.Width) / 2;
                    win.Top = ownerWPFPosition.Y + (ownerWPFSize.Y - win.Height) / 2;

                };
                win.ShowDialog();
            }));
        }

        private void CustomWindow_Closed(object sender, EventArgs e)
        {
            Messenger.Default.Unregister<object>(this, MsgToken.MSG_EXIT);
            Messenger.Default.Unregister<SenderAndPortInfo>(this, MsgToken.MSG_OPENADDPORTFORM);
            Messenger.Default.Unregister<SenderCollectionInfo>(this, MsgToken.MSG_OPENADDSENDERFORM);

        }
    }
}
