using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using System.ComponentModel;
using System.Linq.Expressions;
using Nova.LCT.Message.Client;
using Nova.LCT.GigabitSystem.Common;
using System.Runtime.InteropServices;
using System.IO;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Diagnostics;
using System.Windows.Media;

namespace Nova.SmartLCT.Interface
{
    public class SmartLCTViewModeBase : ViewModelBase
    {
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32")]
        protected static extern long TerminateProcess(int handle, int exitCode);

        #region 属性
        /// <summary>
        /// 获取窗口的显示标题（NoHardware字符由内部设置)
        /// </summary>
        public string WindowDisplayTitle
        {
            get { return _windowDisplayTitle; }
            private set
            {
                _windowDisplayTitle = value;
                RaisePropertyChanged(GetPropertyName(o => this.WindowDisplayTitle));
            }
        }
        private string _windowDisplayTitle = "";
        /// <summary>
        /// 获取和设置窗口的实际标题，该标题用了创建WindowDisplayTitle的基本字符串
        /// </summary>
        public string WindowRealTitle
        {
            get { return _windowRealTitle; }
            set
            {
                _windowRealTitle = value;
                if (IsDemoMode)
                {
                    string msg = "";
                    CommonStaticMethod.GetLanguageString("(No Hardware)", "Lang_Global_NoHardware", out msg);
                    
                    WindowDisplayTitle = _windowRealTitle + msg;
                }
                else
                {
                    WindowDisplayTitle = _windowRealTitle;
                }
                RaisePropertyChanged(GetPropertyName(o => this.WindowRealTitle));
            }
        }
        private string _windowRealTitle = "";

        public virtual bool IsDemoMode
        {
            get { return _isDemoMode; }
            private set
            {
                _isDemoMode = value;

                if (value)
                {
                    string msg = "";
                    CommonStaticMethod.GetLanguageString("(No Hardware)", "Lang_Global_NoHardware", out msg);
                    
                    WindowDisplayTitle = WindowRealTitle + msg;
                }
                else
                {
                    WindowDisplayTitle = WindowRealTitle;
                }
                RaisePropertyChanged(GetPropertyName(o => this.IsDemoMode));
            }
        }
        private bool _isDemoMode = false;

        protected GlobalParameters _globalParams = null;
        protected ILCTServerBaseProxy _serverProxy = null;

        protected static Dictionary<ChipType, ChipInfo> ChipInherDict = null;

        public bool IsNoScreen
        {
            get { return _isNoScreen; }
            set
            {
                _isNoScreen = value;
                RaisePropertyChanged(GetPropertyName(o => this.IsNoScreen));
            }
        }
        private bool _isNoScreen = false;
        #endregion

        #region 静态字段
        protected static string AppStartupPath = "";
        protected static string SERVER_PATH = "";
        protected static string PLUG_IN_PATH = "";
        protected static string CACULATOR_PATH = "";
        protected static string LOGO_FILE_NAME = "";
        protected static string COLORTEMP_MAPPING_FILENAME = "";
        protected static string SCANCONFIGFILES_LIB_PATH = "";
        protected static string APPLICATION_LANG_PATH = "";
        public static Brush[] LINE_COLOR = { Function.StrToBrush("#FF23A9C1"), Function.StrToBrush("#FFA35B59"), 
                                             Function.StrToBrush("#FFE78D19"), Function.StrToBrush("#FFCFCA81"), 
                                             Function.StrToBrush("#FF8996A0"), Function.StrToBrush("#FF867CC8"),
                                             Function.StrToBrush("#FF823287"), Function.StrToBrush("#FF998572"), 
                                             Function.StrToBrush("#FFA27533"), Function.StrToBrush("#FFD98398"), 
                                             Function.StrToBrush("#FF0E9671"), Function.StrToBrush("#FF214883") };
        public static int MaxScreenWidth = 4000;
        public static int MaxScreenHeight = 3000;
        public static int ScrollWidth = 10;
        public static int AdsorbValue = 10;
        public static int ViewBoxWidth = 4010;
        public static int ViewBoxHeight = 3010;
        public static int DviViewBoxWidth = 10000;
        public static int DviViewBoxHeight = 6000;
        public static int MaxIncreaseIndex = 10;
        public static int MinIncreaseIndex = -10;
        public static double IncreaseOrDecreaseValue = 1.1;
        public static double SenderMaxLoadSize = 600;
        public static double PortMaxLoadSize = 500;
        public static int MaxRecentOpenFileCount = 3;
        public static string ReleasePath = AppDomain.CurrentDomain.BaseDirectory;
        public static string DefaultProjectMainName = "SmartLCTProject";
        public static string ProjectExtension = ".xml";
        
        protected readonly string SERVER_NAME = "MarsServerProvider";
        private readonly string PLUG_IN_NAME = "NovaTestTool.exe";
        protected string _curLangFlag = "";
        protected SenderProperty _senderProp = new SenderProperty();
        protected GraphicsDVIPortInfo _graphicsDviInf = new GraphicsDVIPortInfo();
        #endregion

        #region 命令
        #endregion

        #region 构造函数
        public SmartLCTViewModeBase()
        {
            if (!this.IsInDesignMode)
            {
                _globalParams = CommonStaticMethod.GetGlobalParamsFormResources();

                #region 初始化静态字段
                if (string.IsNullOrEmpty(AppStartupPath))
                {
                    AppStartupPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                }
                if (string.IsNullOrEmpty(SERVER_PATH))
                {
                    SERVER_PATH = AppStartupPath + "MarsServerProvider\\" + SERVER_NAME + ".exe";
                }
                if (string.IsNullOrEmpty(PLUG_IN_PATH))
                {
                    PLUG_IN_PATH = AppStartupPath + "NovaTestTool\\" + PLUG_IN_NAME;
                }
                if (string.IsNullOrEmpty(CACULATOR_PATH))
                {
                    CACULATOR_PATH = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\calc.exe";
                }
                if (string.IsNullOrEmpty(LOGO_FILE_NAME))
                {
                    LOGO_FILE_NAME = AppStartupPath + "Resource\\AboutImage.png";
                }
                if (string.IsNullOrEmpty(COLORTEMP_MAPPING_FILENAME))
                {
                    COLORTEMP_MAPPING_FILENAME = ConstValue.COMMONDATA_PATH + "\\ColorTRGBMapping.bin";
                }

                if (string.IsNullOrEmpty(SCANCONFIGFILES_LIB_PATH))
                {
                    SCANCONFIGFILES_LIB_PATH = ConstValue.CONFIG_SCANNERFILELIB_FOLDER;
                }

                if (string.IsNullOrEmpty(APPLICATION_LANG_PATH))
                {
                    APPLICATION_LANG_PATH = AppStartupPath + "Lang\\";
                }
                #endregion

                ChipInherentProperty inher = new ChipInherentProperty();
                ChipInherDict = inher.ChipInherentPropertyDict;

                
                this._serverProxy  = _globalParams.ServerProxy;

                AttachServerEvent(_serverProxy);

                #region 初始化参数
                CheckIsNoScreen();
                IsDemoMode = _globalParams.IsDemoMode;
                _senderProp = _globalParams.SenderProp;
                _graphicsDviInf = new GraphicsDVIPortInfo() { DviPortCols = 1, DviPortRows = 1, GraphicsWidth = 4096, GraphicsHeight = 4096 };
                #endregion

                _globalParams.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(_globalParams_PropertyChanged);
            }
        }
        #endregion

        #region 私有函数
        private void _globalParams_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == GetPropertyName(o => _globalParams.IsDemoMode))
            {
                IsDemoMode = _globalParams.IsDemoMode;
            }
            else if (e.PropertyName == GetPropertyName(o => _globalParams.ServerProxy))
            {
                _serverProxy = _globalParams.ServerProxy;
            }
            else if (e.PropertyName == GetPropertyName(o => _globalParams.AllCommPortLedDisplayDic))
            {
                OnAllCommPortLedDisplayDicChanged();
            }
            else if (e.PropertyName == GetPropertyName(o => _globalParams.LangFlag))
            {
                OnLangFlagChanged(_globalParams.LangFlag);
            }
        }

        protected virtual void OnAllCommPortLedDisplayDicChanged()
        {
            CheckIsNoScreen();
        }

        protected virtual void OnLangFlagChanged(string langFlag)
        {
            _curLangFlag = langFlag;
        }
        private void CheckIsNoScreen()
        {
            if (_globalParams == null ||
                _globalParams.AllCommPortLedDisplayDic == null ||
                _globalParams.AllCommPortLedDisplayDic.Count == 0)
            {
                IsNoScreen = true;
            }
            else
            {
                IsNoScreen = false;
            }
        }

        #region 打开关闭外部程序
        //private void StartCalculater()
        //{
        //    if (!File.Exists(CACULATOR_PATH))
        //    {
        //        string msg = string.Empty;
        //        if (!CommonStaticMethod.GetLanguageString("ScrCfg_NotFileCacls", out msg))
        //        {
        //            msg = "在系统目录下找不到计算器！";
        //        }
        //        ShowDialogMessage(msg, MessageBoxImage.Error);
        //        return;
        //    }
        //    Process proc = Process.Start(CACULATOR_PATH);
        //    _myStartProcessList.Add(proc);
        //}

        //private void StartTestTool()
        //{
        //    if (!File.Exists(PLUG_IN_PATH))
        //    {
        //        string msg = string.Empty;
        //        if (!CommonStaticMethod.GetLanguageString("ScrCfg_NotFileTestTool", out msg))
        //        {
        //            msg = "在程序路径下找不到测试工具！";
        //        }
        //        ShowDialogMessage(msg, MessageBoxImage.Error);
        //        return;
        //    }
        //    Process proc = Process.Start(PLUG_IN_PATH, "En");
        //    _myStartProcessList.Add(proc);
        //}
        //private void TerminateSoftStartProc()
        //{
        //    for (int i = 0; i < _myStartProcessList.Count; i++)
        //    {
        //        Process proc = _myStartProcessList[i];
        //        TerminateProcess(proc.Handle.ToInt32(), 0);
        //    }
        //}
        #endregion


        protected void AttachServerEvent(ILCTServerBaseProxy serverProxy)
        {
            if (serverProxy != null)
            {
                serverProxy.HandshakeServerProviderInterval = 10000;
                serverProxy.NotifyRegisterErrEvent += new NotifyRegisterErrEventHandler(OnNotifyRegisterErrEvent);

                serverProxy.CompleteConnectAllController += new EventHandler(OnCompleteConnectAllController);
                serverProxy.CompleteConnectEquipment += new ConnectEquipmentEventHandler(OnCompleteConnectEquipment);
                serverProxy.EquipmentChangeEvent += new EventHandler(OnEquipmentChangeEvent);

            }
        }

        #region 服务事件响应
        /// <summary>
        /// 响应服务退出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnNotifyRegisterErrEvent(object sender, NotifyRegisterErrorEventArgs e)
        {
            
        }
        /// <summary>
        /// 所有设备完成重新连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnCompleteConnectAllController(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 设备变更事件的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnEquipmentChangeEvent(object sender, EventArgs e)
        {

        }

        protected virtual void OnCompleteConnectEquipment(object sender, ConnectEquipmentEventArgs e)
        {

        }
        #endregion

        #region 对话框显示
        public void ShowGlobalDialogMessage(string content, MessageBoxImage icon)
        {
            DialogMessage dlgMsg = new DialogMessage(this, content, null);
            dlgMsg.Button = MessageBoxButton.OK;
            dlgMsg.Caption = "";
            dlgMsg.DefaultResult = MessageBoxResult.Cancel;
            dlgMsg.Icon = icon;
            Messenger.Default.Send<DialogMessage>(dlgMsg, MsgToken.MSG_SHWOGLOBALMSGBOX);
        }

        protected void ShowGlobalProcessBegin(string msg)
        {
            Messenger.Default.Send<string>(msg, MsgToken.MSG_SHOWGLOBALPROCESSBEGIN);
        }

        protected void ShowGlobalProcessEnd()
        {
            Messenger.Default.Send<string>("", MsgToken.MSG_SHOWGLOBALPROCESSEND);
        }

        protected void ResetGlobalProcessingBar(string msg)
        {
            Messenger.Default.Send<string>(msg, MsgToken.MSG_RESETGLOBALPROCESSMSG);
        }

        public MessageBoxResult ShowQuestionMessage(string content, MessageBoxButton button, MessageBoxImage icon)
        {
            DialogMessage dlgMsg = new DialogMessage(this, content, null);
            dlgMsg.Button = button;
            dlgMsg.Caption = "";
            dlgMsg.DefaultResult = MessageBoxResult.Cancel;
            dlgMsg.Icon = icon;
            Messenger.Default.Send<DialogMessage>(dlgMsg, MsgToken.MSG_SHOWGLOBALQUESTIONMSGBOX);
            return dlgMsg.DefaultResult;
        }
        #endregion

        #region 语言更新
        protected void ChangedLanguage(string langTag)
        {
            try
            {
                string fileName =  "\\Lang\\" + langTag +"\\AppLanguage." + langTag + ".xaml";
                ResourceDictionary res = (ResourceDictionary)Application.LoadComponent(new Uri(fileName, UriKind.RelativeOrAbsolute));

                Application.Current.Resources.MergedDictionaries.RemoveAt(0);
                Application.Current.Resources.MergedDictionaries.Insert(0, res);

                _globalParams.LangFlag = langTag;
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                ShowGlobalDialogMessage("Fail To Change language!", MessageBoxImage.Error);
            }
        }
        #endregion
        #endregion

        public string GetPropertyName<T>(Expression<Func<NotificationST, T>> expr)
        {
            var name = ((MemberExpression)expr.Body).Member.Name;
            return name;
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            this.RaisePropertyChanged(propertyName);
        }
    }
}
