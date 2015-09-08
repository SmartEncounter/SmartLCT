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
using SmartLCT;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using Nova.LCT.GigabitSystem.Common;

namespace Nova.SmartLCT.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StartUpWindow : Window
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

        #region 字段
        private Win_ProgressBar _progressBar = null;
        private bool _isStartUpCompleted = false;
        private NotificationMessageAction<SenderConfigInfo> _modifySenderType = null;

        private Stack<Window> _openedWidnowsList = new Stack<Window>();
        #endregion

        public StartUpWindow()
        {
            InitializeComponent();

            Messenger.Default.Register<string>(this, MsgToken.MSG_STARTUP_COMPLETED, OnStartUpCompleted);
            Messenger.Default.Register<DialogMessage>(this, MsgToken.MSG_SHWOGLOBALMSGBOX, OnShowGlobalMsgBox);
            Messenger.Default.Register<string>(this, MsgToken.MSG_SHOWEQUIPMENTMANAGER, OnShowEquipmentManager);
            Messenger.Default.Register<string>(this, MsgToken.MSG_SHOWSCANBOARDCONFIGMANAGER, OnShowScanBoardConfigManager);
            
            Messenger.Default.Register<NotificationMessageAction<CustomReceiveResult>>(this, MsgToken.MSG_SHOWSETCUSTOMRECEIVESIZE, OnShowCustomReceiveSize);
            //Messenger.Default.Register<NotificationMessageAction<SenderConfigInfo>>(this, MsgToken.MSG_SHOWCHANGEDSENDERTYPE, OnShowChangedSenderType);
            Messenger.Default.Register<NotificationMessageAction<SenderConfigInfo>>(this, MsgToken.MSG_ISMODIFYSENDERTYPE, OnIsModifySenderType);

            Messenger.Default.Register<string>(this, MsgToken.MSG_SHOWEDIDMANAGER, OnShowEDIDManager);
            Messenger.Default.Register<string>(this, MsgToken.MSG_SHOWBRIGHTADJUST, OnShowBrightAdjust);
            Messenger.Default.Register<DialogMessage>(this, MsgToken.MSG_SHOWGLOBALQUESTIONMSGBOX, OnShowQuestionMsgBox);
            Messenger.Default.Register<string>(this, MsgToken.MSG_SHOWGLOBALPROCESSBEGIN, OnShowGlobalProcessBegin);
            Messenger.Default.Register<string>(this, MsgToken.MSG_SHOWGLOBALPROCESSEND, OnShowGlobalProcessEnd);
            Messenger.Default.Register<string>(this, MsgToken.MSG_RESETGLOBALPROCESSMSG, OnResetGlobalProcessMsg);

            Messenger.Default.Register<NotificationMessageAction<ConfigurationData>>(this, MsgToken.MSG_SHOWGUIDETWO, OnShowGuideTwo);
            Messenger.Default.Register<NotificationMessageAction<OneSmartBrightEasyConfig>>(this, MsgToken.MSG_SHOWADDSMARTBRIGHT, OnShowAddTimingAdjustBright);
            Messenger.Default.Register<NotificationMessageAction<FastSegmentParam>>(this, MsgToken.MSG_SHOWFASTSEGMENTATION, OnShowFastSegmentation);
            Messenger.Default.Register<NotificationMessageAction<PeripheralsSettingParam>>(this, MsgToken.MSG_SHOWWINPERIPHERALSCONFIG, OnShewWinPeripheralsConfig);

            Messenger.Default.Register<ConfigurationData>(this, MsgToken.MSG_CLOSESTARTWINDOW, OnShowMainWindow);

            Messenger.Default.Register<string>(this, MsgToken.MSG_SHOWLANGUAGE_SEL, OnShowLanguageSel);

            //TextElement.FontFamilyProperty.OverrideMetadata(typeof(TextElement), new FrameworkPropertyMetadata(new FontFamily("Comic Sans MS")));

            //TextBlock.FontFamilyProperty.OverrideMetadata(typeof(TextBlock), new FrameworkPropertyMetadata(new FontFamily("Comic Sans MS")));

        }

        private void OnShowLanguageSel(string langFlag)
        {
            Win_LanguageSelect win = new Win_LanguageSelect();
            win.SelectedLangFlag = langFlag;
            ShowInCenterParent(win);
        }

        private void OnShowMainWindow(ConfigurationData data)
        {
            if (data != null)
            {
                //this.Hide();
            }
            else
            {
                return;
            }
            if (data.OperateType == OperateScreenType.CreateScreenAndOpenConfigFile)
            {
                if (data.ProjectLocationPath != "")
                {
                    // this.Hide();

                    MainWindow win = new MainWindow(data);
                    ShowInCenterParent(win);
                }
            }
            else if (data.OperateType == OperateScreenType.CreateScreen)
            {
                MainWindow win = new MainWindow(data);
                ShowInCenterParent(win);
            }
        } 

        private void OnShowFastSegmentation(NotificationMessageAction<FastSegmentParam> info)
        {
            FastSegmentParam dataInfo = info.Target as FastSegmentParam;
            Window_FastSegmentation winFastSegmentation = new Window_FastSegmentation(dataInfo);
            bool? res=  ShowInCenterParentWithRes(winFastSegmentation);
            if (res==true)
            {
                FastSegmentParam data = winFastSegmentation.GetFastSegmentationInfo();
                info.Execute(data);
            }
        }
        private void OnShowAddTimingAdjustBright(NotificationMessageAction<OneSmartBrightEasyConfig> info)
        {
            OneSmartBrightEasyConfig dataInfo = info.Target as OneSmartBrightEasyConfig;
            Window_AddTimingAdjustData winAddBrightTimingAdjustData = new Window_AddTimingAdjustData(dataInfo);
            bool? res = ShowInCenterParentWithRes(winAddBrightTimingAdjustData);
            if (res == true)
            {
                OneSmartBrightEasyConfig data = winAddBrightTimingAdjustData.GetOneSmartBrightOneItemConfig();   
                info.Execute(data);        
            }

        }

        private void OnShewWinPeripheralsConfig(NotificationMessageAction<PeripheralsSettingParam> info)
        {
            PeripheralsSettingParam dataInfo = info.Target as PeripheralsSettingParam;
            Window_PeripheralsConfig winPeripheralConfig = new Window_PeripheralsConfig(dataInfo);
            bool? res = ShowInCenterParentWithRes(winPeripheralConfig);
            if (res == true)
            {
                PeripheralsSettingParam data = winPeripheralConfig.GetPeripheralConfig();
                info.Execute(data);
            }
        }
        private void OnShowGuideTwo(NotificationMessageAction<ConfigurationData> info)
        {
            ConfigurationData dataInfo = info.Target as ConfigurationData;
            Frm_Guide_two frm_Guide_Two = new Frm_Guide_two(dataInfo.OperateType);

            ShowInCenterParent(frm_Guide_Two);

            ConfigurationData data = frm_Guide_Two.GetConfigData();
            if (data != null)
            {
                info.Execute(data);
                //如果是要创建显示屏则创建
                //if (dataInfo.OperateType == OperateScreenType.CreateScreen)
                //{
                //    MainWindow win = new MainWindow(data);
                //    win.Owner = this;
                //    win.ShowDialog();
                //}

                ////如果是要更新显示屏，则返回信息
                //else if (dataInfo.OperateType == OperateScreenType.UpdateScreen)
                //{
                //    info.Execute(data);
                //}
            }

        }
        private void OnStartUpCompleted(string msg)
        {
            if (_isStartUpCompleted)
            {
                return;
            }
            _isStartUpCompleted = true;


            this.Hide();
            StartWindow start = new StartWindow();
            //start.Owner = this;
            //start.ShowDialog();
            ShowInCenterParent(start);
            //MainWindow win = new MainWindow();
            //win.ShowDialog();
        }

        private void OnShowGlobalMsgBox(DialogMessage msg)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                WinMessageBox msgBox = new WinMessageBox() { MsgContent = msg.Content, Caption = msg.Caption, MsgButton = msg.Button, MsgImage = msg.Icon };

                ShowInCenterParent(msgBox);
            }));
        }

        private void OnShowEquipmentManager(string msg)
        {
            EquipmentManager manager = new EquipmentManager();
            ShowInCenterParent(manager);
        }

        private void OnShowScanBoardConfigManager(string msg)
        {
            ScanBoardConfigManager manager = new ScanBoardConfigManager();
            ShowInCenterParent(manager);
        }

        private void OnShowEDIDManager(string msg)
        {
            SendEDIDManager manager = new SendEDIDManager();
            ShowInCenterParent(manager);
        }

        private void OnShowBrightAdjust(string msg)
        {
            WinBrightAdjust win = new WinBrightAdjust();
            ShowInCenterParent(win);
        }

        private void OnShowQuestionMsgBox(DialogMessage msg)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                WinMessageBox msgBox = new WinMessageBox() { MsgContent = msg.Content, Caption = msg.Caption, MsgButton = msg.Button, MsgImage = msg.Icon };
                bool? res = ShowInCenterParent(msgBox);
                
                if (msg.Button == MessageBoxButton.YesNo ||
                    msg.Button == MessageBoxButton.YesNoCancel)
                {
                    if (res == true)
                    {
                        msg.DefaultResult = MessageBoxResult.Yes;
                    }
                    else
                    {
                        if (msgBox.ButtonSelected == Nova.SmartLCT.UI.WinMessageBox.ButtonSelectedType.Cancel)
                        {
                            msg.DefaultResult = MessageBoxResult.Cancel;
                        }
                        else
                        {
                            msg.DefaultResult = MessageBoxResult.No;
                        }
                    }
                }
                else if (msg.Button == MessageBoxButton.OKCancel)
                {
                    if (res == true)
                    {
                        msg.DefaultResult = MessageBoxResult.OK;
                    }
                    else
                    {
                        msg.DefaultResult = MessageBoxResult.Cancel;
                    }
                }
            }));
        }

        private void OnShowGlobalProcessBegin(string msg)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                if (_progressBar == null)
                {
                    _progressBar = new Win_ProgressBar() { ProcessingMsg = msg };
                }

                ShowInCenterParent(_progressBar);
            }));
        }

        private void OnShowGlobalProcessEnd(string msg)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                if (_progressBar != null)
                {
                    _progressBar.Close();
                    _progressBar = null;
                }
            }));
        }

        private void OnResetGlobalProcessMsg(string msg)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                if (_progressBar != null)
                {
                    _progressBar.ProcessingMsg = msg;
                }
            }));
        }

        private void ShowInCenterParent(Window win)
        {
            bool? res = ShowInCenterParentWithRes(win);
        }

        private bool? ShowInCenterParentWithRes(Window win)
        {
            bool? res = false;
            this.Dispatcher.Invoke(new Action(() =>
            {
                //ActiveWindowHandle = GetActiveWindow();  //获取父窗体句柄  
                int count = Application.Current.MainWindow.OwnedWindows.Count;
                Window parentWin = this;
                //if (count > 0)
                //{
                //    parentWin = Application.Current.MainWindow.OwnedWindows[count - 1];
                //}
                if (_openedWidnowsList.Count > 0)
                {
                    parentWin = _openedWidnowsList.Pop();
                }
                _openedWidnowsList.Push(parentWin);
                _openedWidnowsList.Push(win);
                win.Owner = parentWin;

                ActiveWindowHandle = new WindowInteropHelper(parentWin).Handle;


                WindowInteropHelper helper = new WindowInteropHelper(win);
                //helper.Owner = ActiveWindowHandle;
                //win.Owner = parentWin;
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
                res = win.ShowDialog();
            }));

            _openedWidnowsList.Pop();
            return res;
        }

        private bool? ShowInCenterParent(WinMessageBox win)
        {
            bool? res = false;
            this.Dispatcher.Invoke(new Action(() =>
            {
                //ActiveWindowHandle = GetActiveWindow();  //获取父窗体句柄  
                int count = Application.Current.MainWindow.OwnedWindows.Count;
                Window parentWin = this;
                if (_openedWidnowsList.Count > 0)
                {
                    parentWin = _openedWidnowsList.Pop();
                }
                _openedWidnowsList.Push(parentWin);
                _openedWidnowsList.Push(win);
                win.Owner = parentWin;

                ActiveWindowHandle = new WindowInteropHelper(parentWin).Handle;

                WindowInteropHelper helper = new WindowInteropHelper(win);
                //helper.Owner = ActiveWindowHandle;

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
                res = win.ShowDialog();
            }));

            _openedWidnowsList.Pop();
            return res;
        }

        private void OnShowCustomReceiveSize(NotificationMessageAction<CustomReceiveResult> str)
        {
            CustomReceiveResult info = (CustomReceiveResult)str.Target;
            Window_SetCustomReceive setCustomReceiveWin = new Window_SetCustomReceive(info.Width,info.Height);
            bool? res = setCustomReceiveWin.ShowDialog();
            int width = 0;
            int height = 0;
            setCustomReceiveWin.GetSetSize(out width, out height);
            CustomReceiveResult param = new CustomReceiveResult()
            {
                IsOK = res,
                Width = width,
                Height = height
            };
            str.Execute(param);
        }
        SmartLCTViewModeBase _smartLCTViewModeBase = new SmartLCTViewModeBase();
        private void OnIsModifySenderType(NotificationMessageAction<SenderConfigInfo> info)
        {
            _modifySenderType=info;
            SenderConfigInfo senderConfigInfo = (SenderConfigInfo)info.Sender;
            string msg = "";
            CommonStaticMethod.GetLanguageString("确定将发送卡型号改为 ", "Lang_StartUp_SureToModifySender", out msg);

            MessageBoxResult result= _smartLCTViewModeBase.ShowQuestionMessage(msg +" "+ senderConfigInfo.SenderTypeName.ToString(),MessageBoxButton.YesNo,MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _modifySenderType.Execute(senderConfigInfo);
            }
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //WinBrightAdjust bright = new WinBrightAdjust();
            //bright.ShowDialog();
            WinMessageBox msg = new WinMessageBox();
            msg.ShowDialog();
        }

        private void OnCleanUp()
        {
            Messenger.Default.Unregister<string>(this, MsgToken.MSG_STARTUP_COMPLETED, OnStartUpCompleted);
            Messenger.Default.Unregister<DialogMessage>(this, MsgToken.MSG_SHWOGLOBALMSGBOX, OnShowGlobalMsgBox);
            Messenger.Default.Unregister<string>(this, MsgToken.MSG_SHOWEQUIPMENTMANAGER, OnShowEquipmentManager);
            Messenger.Default.Unregister<string>(this, MsgToken.MSG_SHOWSCANBOARDCONFIGMANAGER, OnShowScanBoardConfigManager);
            Messenger.Default.Unregister<string>(this, MsgToken.MSG_SHOWEDIDMANAGER, OnShowEDIDManager);
            Messenger.Default.Unregister<DialogMessage>(this, MsgToken.MSG_SHOWGLOBALQUESTIONMSGBOX, OnShowQuestionMsgBox);
            Messenger.Default.Unregister<NotificationMessageAction<CustomReceiveResult>>(this, MsgToken.MSG_SHOWSETCUSTOMRECEIVESIZE, OnShowCustomReceiveSize);
            Messenger.Default.Unregister<NotificationMessageAction<ConfigurationData>>(this, MsgToken.MSG_SHOWGUIDETWO, OnShowGuideTwo);
            Messenger.Default.Unregister<NotificationMessageAction<PeripheralsSettingParam>>(this, MsgToken.MSG_SHOWWINPERIPHERALSCONFIG, OnShewWinPeripheralsConfig);
            Messenger.Default.Unregister<ConfigurationData>(this, MsgToken.MSG_CLOSESTARTWINDOW, OnShowMainWindow);
            Messenger.Default.Unregister<string>(this, MsgToken.MSG_SHOWLANGUAGE_SEL, OnShowLanguageSel);

        }
    }
}
