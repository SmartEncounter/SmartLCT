using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nova.SmartLCT.Interface;
using System.Timers;
using Nova.LCT.Message.Client;
using Nova.LCT.GigabitSystem.CommonInfoAccessor;
using Nova.LCT.GigabitSystem.Common;
using System.IO;
using System.Diagnostics;
using GalaSoft.MvvmLight.Command;
using Nova.LCT.GigabitSystem.Files;
using System.Threading;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using System.Globalization;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.ObjectModel;
using System.Configuration;
using Nova.LCT.GigabitSystem.HWConfigAccessor;
using Nova.SmartLCT.Database;

namespace Nova.SmartLCT.UI
{
    public class StartUpWindow_VM : SmartLCTViewModeBase
    {
        #region 自定义结构
        /// <summary>
        /// 当前的系统类型
        /// </summary>
        private enum SystemType
        {
            Synchronized,   //同步系统
            Asynchronous,    //异步系统
            TestSystem        //演示系统 
        }
        #endregion

        #region 界面属性
        public string LoadingMsg
        {
            get { return _loadingMsg; }
            set
            {
                _loadingMsg = value;
                NotifyPropertyChanged(GetPropertyName(o => this.LoadingMsg));
            }
        }
        private string _loadingMsg = "";

        public Visibility IsWindowVisible
        {
            get { return _isWindowVisible; }
            set
            {
                _isWindowVisible = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsWindowVisible));
            }
        }
        private Visibility _isWindowVisible = Visibility.Visible;
        #endregion

        #region 属性
        public ILCTServerBaseProxy Proxy
        {
            get
            {
                return _proxy;
            }
            set
            {
                _proxy = value;
            }
        }
        private ILCTServerBaseProxy _proxy = null;

        public Dictionary<string, List<ILEDDisplayInfo>> AllCommPortLedDisplayDic
        {
            get
            {
                return _allCommPortLedDisplayDic;
            }
            set
            {
                _allCommPortLedDisplayDic = value;

                //TODO:设备变更的时候处理
            }
        }
        private Dictionary<string, List<ILEDDisplayInfo>> _allCommPortLedDisplayDic = new Dictionary<string, List<ILEDDisplayInfo>>();
        

        public Dictionary<string, List<SenderRedundancyInfo>> SenderReduInfoDic
        {
            get
            {
                return _senderRedundancyDic;
            }
            set
            {
                _senderRedundancyDic = value;
            }
        }
        private Dictionary<string, List<SenderRedundancyInfo>> _senderRedundancyDic = new Dictionary<string, List<SenderRedundancyInfo>>();

        /// <summary>
        /// 控制系统的个数
        /// </summary>
        public int CtrlSystemCount
        {
            get
            {
                return _ctrlSystemCount;
            }
            set
            {
                _ctrlSystemCount = value;
                RaisePropertyChanged(GetPropertyName(o => this.CtrlSystemCount));
            }
        }
        private int _ctrlSystemCount = 0;
        /// <summary>
        /// 其他设备的个数
        /// </summary>
        public int OtherDeviceCount
        {
            get { return _otherDeviceCount; }
            set
            {
                _otherDeviceCount = value;
                RaisePropertyChanged(GetPropertyName(o => this.OtherDeviceCount));
            }
        }
        private int _otherDeviceCount = 0;
        #endregion

        #region 字段
        private readonly string SERVER_FORM_NAME = "A7F89E4D-04F4-46a6-9754-A334B3E8FEE5";

        /// <summary>
        /// 主要用于服务退出后，重新注册到服务上
        /// </summary>
        private System.Timers.Timer _registToServerTimer = new System.Timers.Timer(2000);

        private string _serverVersion = "";

        private Dictionary<string, List<ILEDDisplayInfo>> _tempCommPortLedDisplayDict = null;
        private Dictionary<string, List<SenderRedundancyInfo>> _tempRedundancyDict = null;
        private string DEFAULT_LANG = "zh-CN";

        private SmartBright _smartBrightManager = null;
        #endregion

        #region 命令
        public RelayCommand CmdInitialize
        {
            get;
            private set;
        }
        public RelayCommand CmdUninitialize
        {
            get;
            private set;
        }
        #endregion

        #region 构造函数
        public StartUpWindow_VM()
        {
            if (!this.IsInDesignMode)
            {
                _registToServerTimer.Elapsed += new ElapsedEventHandler(_registToServerTimer_Elapsed);
                CmdInitialize = new RelayCommand(OnCmdInitialize);
                CmdUninitialize = new RelayCommand(Cleanup);
                WindowRealTitle = "SmartLCT 2014";

                Messenger.Default.Register<string>(this, MsgToken.MSG_SCREENINFO_CHANGED, OnScreenChanged);
            }
            else
            {
                LoadingMsg = "Loading...";
            }
        }
        #endregion

        #region 私有函数

        private void OnScreenChanged(string msg)
        {
            OnEquipmentChangeEvent(null, null);
        }

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        private void OnCmdInitialize()
        {

            new Thread((ThreadStart)delegate()
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    #region 获取应用程序配置
                    SQLiteAccessor accessor = SQLiteAccessor.Instance;
                    AppConfiguration appConfig = null;
                    if (accessor != null)
                    {
                        appConfig = accessor.LoadAppConfig();
                    }
                    
                    if (appConfig == null)
                    {
                        appConfig = new AppConfiguration();
                    }
                    _globalParams.AppConfig = appConfig;
                    #endregion

                    #region 加载语言
                    string lang = appConfig.LangFlag;
                    LoadLanguage(lang);
                    #endregion

                    string msg = "";
                    CommonStaticMethod.GetLanguageString("正在初始化...", "Lang_StartUp_Initialize", out msg);
                    LoadingMsg = msg;
                    CustomTransform.Delay(200, 10);
                    #region 关闭调节小软件
                    KillBrightAdjustToolProc(ConstValue.BRIGHT_ADJUST_TOOL_PROC_NAME);

                    #endregion

                    #region 重新启动服务
                    CommonStaticMethod.GetLanguageString("正在启动服务...", "Lang_StartUp_StartServer", out msg);
                    LoadingMsg = msg;
                    CustomTransform.Delay(200, 10);
                    RestartServerProc(SERVER_PATH);
                    #endregion

                    #region 启动定时调节小软件
                    CommonStaticMethod.GetLanguageString("正在启动亮度调节软件...", "Lang_StartUp_StartBright", out msg);
                    LoadingMsg = msg;
                    CustomTransform.Delay(200, 10);
                    StartBrightAdjustToolProc(ConstValue.BRIGHT_ADJUST_TOOL_PROC_NAME, ConstValue.BRIGHT_ADJUST_TOOL_START_FILE);
                    #endregion

                    #region 获取本地数据
                    //res = CustomTransform.LoadScanProFile(ConstValue.SCANNER_PAGE_CONFIG_FILE, ref _globalParams.ScanBdProp);
                    //MonitorSysData data;
                    //MonitorSettingFileCreator.LoadConfigFile(_newMonitorFileName, out data);
                    //_globalParams.MonitorSetting = data;
                    CommonStaticMethod.GetLanguageString("正在获取数据...", "Lang_StartUp_GetData", out msg);
                    LoadingMsg = msg;
                    CustomTransform.Delay(200, 10);
                    LoadColorTempMapping();
                    //加载
                    LoadRecentProjectFile();
                    LoadScanFileLib();
                    LoadSenderFileLib();
                    LoadSenderAndPortPicFile();
                    
                   
                    #endregion

                    #region 注册服务
                    CommonStaticMethod.GetLanguageString("正在注册服务...", "Lang_StartUp_RegistServer", out msg);
                    LoadingMsg = msg;
                    CustomTransform.Delay(200, 10);

                    InitalizeServerProxy(SystemType.Synchronized);
                    bool res = RegisterToServer(SystemType.Synchronized);
                    if (!res)
                    {
                        _registToServerTimer.Start();
                    }
                    #endregion
                }));
            }).Start();
        }

        #endregion

        #region 服务相关
        /// <summary>
        /// 销毁服务对象
        /// </summary>
        private void TerminateServerProxy()
        {
            if (_proxy == null)
            {
                return;
            }
            _proxy.NotifyRegisterErrEvent -= new NotifyRegisterErrEventHandler(OnNotifyRegisterErrEvent);

            _proxy.CompleteConnectAllController -= new EventHandler(OnCompleteConnectAllController);
            _proxy.CompleteConnectEquipment -= new ConnectEquipmentEventHandler(OnCompleteConnectEquipment);

            _proxy.EquipmentChangeEvent -= new EventHandler(OnEquipmentChangeEvent);
            _proxy.Terminate();
            _proxy = null;
        }

        private void InitalizeServerProxy(SystemType sysType)
        {
            if (_proxy != null)
            {
                TerminateServerProxy();
            }
            if (sysType == SystemType.Synchronized)
            {
                Proxy = new LCTServerMessageProxy();
            }
            else if (sysType == SystemType.TestSystem)
            {
                //Proxy = new LCTServerProxyTest();
            }
            _proxy.Initalize();

            AttachServerEvent(_proxy);

            _globalParams.ServerProxy = _proxy;
        }

        /// <summary>
        /// 向服务注册
        /// </summary>
        private bool RegisterToServer(SystemType sysType)
        {
            bool res = false;
            if (sysType == SystemType.Synchronized)
            {
                res = ((LCTServerMessageProxy)_proxy).Register(SERVER_FORM_NAME, out _serverVersion);
            }
            else if (sysType == SystemType.TestSystem)
            {
                //res = ((LCTServerProxyTest)_proxy).Register(SERVER_FORM_NAME, out _serverVersion);
            }
            if (!res)
            {
                _globalParams.IsDemoMode = true;
            }
            else
            {
                OnEquipmentChangeEvent(null, null);
            }

            CompletedStartUp();
            return res;
        }
        /// <summary>
        /// 响应服务退出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnNotifyRegisterErrEvent(object sender, NotifyRegisterErrorEventArgs e)
        {
            _registToServerTimer.Start();
            ChangeDeviceList(_proxy.EquipmentTable);
        }
        /// <summary>
        /// 所有设备完成重新连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnCompleteConnectAllController(object sender, EventArgs e)
        {
            ChangeDeviceList(_proxy.EquipmentTable);
        }
        /// <summary>
        /// 设备变更事件的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnEquipmentChangeEvent(object sender, EventArgs e)
        {
            ChangeDeviceList(_proxy.EquipmentTable);

            if (_ctrlSystemCount == 0 && _otherDeviceCount == 0)
            {
                ChangeSoftwareMode(true);
                _tempCommPortLedDisplayDict = new Dictionary<string, List<ILEDDisplayInfo>>();
                AllCommPortLedDisplayDic = _tempCommPortLedDisplayDict;

                _tempRedundancyDict = new Dictionary<string, List<SenderRedundancyInfo>>();
                SenderReduInfoDic = _tempRedundancyDict;

                return;
            }
            else
            {
                string msg = "";
                CommonStaticMethod.GetLanguageString("正在获取硬件数据...", "Lang_StartUp_GetHWData", out msg);
                LoadingMsg = msg;
                CustomTransform.Delay(200, 10);

                ChangeSoftwareMode(false);

                #region 从软件空间读取屏体信息
                SoftWareSpaceAccessor accessor = new SoftWareSpaceAccessor(_proxy);
                List<string> readList = new List<string>();
                lock (_proxy.EquimentObject)
                {
                    string[] readArray = new string[_proxy.EquipmentTable.Keys.Count];
                    bool isFirst = true;
                    foreach (string key in _proxy.EquipmentTable.Keys)
                    {
                        EquipmentInfo info = _proxy.EquipmentTable[key];
                        if (CustomTransform.IsSystemController(info.ModuleID))
                        {
                            readList.Add(key);
                        }
                        if (isFirst)
                        {
                            _globalParams.SelectPort = key;
                            isFirst = false;
                        }
                    }
                }
                accessor.ReadPortSoftwareSpaceInfo(readList, OnCompletedReadPortsSoftwareSpaceInfo);

                AllCOMHWBaseInfoAccessor accessor1 = new AllCOMHWBaseInfoAccessor(_proxy);
                accessor1.ReadAllComHWBaseInfo(OnCompleteReadAllComHWBaseInfoCallback, null);
                #endregion
            }

        }

        protected override void OnCompleteConnectEquipment(object sender, ConnectEquipmentEventArgs e)
        {
            //if (e.PortName == _selectedPortName)
            //{
            ChangeDeviceList(_proxy.EquipmentTable);
            //}
        }


        #endregion

        #region 辅助函数
        private void StartBrightAdjustToolProc(string processName, string filePath)
        {

            try
            {

                //KillBrightAdjustToolProc(processName);

                if (File.Exists(filePath))
                {
                    System.Diagnostics.Process.Start(filePath);
                    CustomTransform.Delay(1000, 50);
                    //OutputDebugInfoStr(false, "启动定时调节软件");
                }
                else
                {
                    //OutputDebugInfoStr(false, "定时调节软件路径不存在，路径为;" + filePath);
                    return;
                }
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e.ToString());
                //OutputDebugInfoStr(false, "启动定时调节软件异常：" + e.Message);
            }

        }

        /// <summary>
        /// 关闭亮度调节小软件
        /// </summary>
        /// <param name="procName"></param>
        private void KillBrightAdjustToolProc(string procName)
        {

            try
            {
                System.Diagnostics.Process[] thisProc = System.Diagnostics.Process.GetProcessesByName(procName);
                for (int i = 0; i < thisProc.Length; i++)
                {
                    TerminateProcess(int.Parse(thisProc[i].Handle.ToString()), 0);
                }

            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

        }
        /// <summary>
        /// 重新启动服务
        /// </summary>
        /// <param name="serverPath"></param>
        private void RestartServerProc(string serverPath)
        {
            if (File.Exists(SERVER_PATH))
            {
                Process[] processList = Process.GetProcesses();
                foreach (Process proc in processList)
                {
                    if (proc.ProcessName == SERVER_NAME)
                    {
                        proc.Kill();
                    }
                }
                Process.Start(SERVER_PATH);
            }
            //CustomTransform.Delay(10000, 60);
        }
        /// <summary>
        /// 保存语言代号
        /// </summary>
        /// <param name="langTag"></param>
        private static void SaveLanguageTagForBright(string langTag)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(ConstValue.CONFIG_FOLDER + "Lang.txt", FileMode.Create, FileAccess.Write, FileShare.Read);
                byte[] data = Encoding.Default.GetBytes(langTag);
                if (data != null)
                {

                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Write(data, 0, data.Length);
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }
        /// <summary>
        /// 更改连接设备
        /// </summary>
        /// <param name="EquipmentTable"></param>
        private void ChangeDeviceList(Dictionary<string, EquipmentInfo> equipmentTable)
        {
            int controlSystemCount = 0;
            int otherDeviceCount = 0;

            foreach (string key in equipmentTable.Keys)
            {
                EquipmentInfo info = equipmentTable[key];
                //设备类型更新
                if (CustomTransform.IsSystemController(info.ModuleID))
                {
                    controlSystemCount++;
                }
                else
                {
                    otherDeviceCount++;
                }
            }

            CtrlSystemCount = controlSystemCount;
            OtherDeviceCount = otherDeviceCount;

            if (controlSystemCount == 0 && otherDeviceCount == 0)
            {
                ChangeSoftwareMode(true);
            }
            else
            {
                ChangeSoftwareMode(false);
            }
        }
        /// <summary>
        /// 更改软件的模式(正常或者demo)
        /// </summary>
        /// <param name="isDemoMode"></param>
        private void ChangeSoftwareMode(bool isNoHardware)
        {
            _globalParams.IsDemoMode = isNoHardware;
        }
        /// <summary>
        /// 定时注册到服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _registToServerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool res = RegisterToServer(SystemType.Synchronized);
            if (res)
            {
                _registToServerTimer.Stop();
            }
        }

        #region 对话框
        //protected void ShowDialogMessage(string content, MessageBoxImage icon)
        //{
        //    DialogMessage dlgMsg = new DialogMessage(this, content, null);
        //    dlgMsg.Button = MessageBoxButton.OK;
        //    dlgMsg.Caption = "";
        //    dlgMsg.DefaultResult = MessageBoxResult.Cancel;
        //    dlgMsg.Icon = icon;
        //    Messenger.Default.Send<DialogMessage>(dlgMsg, MsgToken.MainNoCallBackMsg);
        //}

        //private void ShowScreenCfgWin()
        //{
        //    Messenger.Default.Send<string>("", MsgToken.ShowScreenCfgWin);
        //}
        //private void ShowDisplayCtrl()
        //{
        //    Messenger.Default.Send<string>("", MsgToken.ShowDisplayCtrl);
        //}
        //private void ShowBrightAdjust()
        //{
        //    Messenger.Default.Send<string>("", MsgToken.ShowBrightAdjust);
        //}
        //private void ShowMonitorViewer()
        //{
        //    Messenger.Default.Send<string>("", MsgToken.ShowMonitorViewer);
        //}
        //private void ShowPrestorePicture()
        //{
        //    Messenger.Default.Send<string>("", MsgToken.ShowPrestorePicture);
        //}
        //private void ShowFuncCardConfig()
        //{
        //    Messenger.Default.Send<string>("", MsgToken.ShowFunctionCardConfig);
        //}
        //private void ShowHWInfoWin()
        //{
        //    Messenger.Default.Send<string>("", MsgToken.ShowHWInfoWin);
        //}

        //private void ShowDeviceDetailWin()
        //{
        //    Messenger.Default.Send<string>("", MsgToken.ShowDeviceDetailWin);
        //}

        //private void ShowAboutDialog()
        //{
        //    string titleName = "About " + _productName;
        //    BitmapImage img = new BitmapImage();
        //    img.BeginInit();
        //    img.UriSource = new Uri(LOGO_FILE_NAME, UriKind.Absolute);
        //    img.EndInit();

        //    AboutDialogInfo info = new AboutDialogInfo()
        //    {
        //        CompanyName = _companyName,
        //        CompanyURLAddress = _companyURLAddress,
        //        CopyRight = _copyRight,
        //        ProductName = _productName,
        //        TitleName = titleName,
        //        LogoFile = img
        //    };
        //    Messenger.Default.Send<AboutDialogInfo>(info, MsgToken.ShowAboutDialog);
        //}
        #endregion

        private void LoadColorTempMapping()
        {
            List<ColorTempRGBMapping> mappingList = null;
            Dictionary<int, ColorTempRGBMapping> mappingDict = new Dictionary<int, ColorTempRGBMapping>();
            ColorTempRGBMapFileCreator.LoadFromFile(COLORTEMP_MAPPING_FILENAME, out mappingList);
            for (int i = 0; i < mappingList.Count; i++)
            {
                ColorTempRGBMapping tempMap = mappingList[i];
                if (tempMap.MapType == ColorTempRGBMappingType.deg_2)
                {
                    mappingDict.Add(tempMap.ColorTemp, tempMap);
                }
            }
            _globalParams.ColorTempRGBMappingDict = mappingDict;
        }

        private bool LoadRecentProjectFile()
        {
            XmlSerializer xmls = null;
            StreamReader sr = null;
            XmlReader xmlReader = null;
            string msg = string.Empty;
            bool res = false;
            try
            {
                xmls = new XmlSerializer(typeof(ObservableCollection<ProjectInfo>));
                sr = new StreamReader(SmartLCTViewModeBase.ReleasePath + "Resource\\Config\\RecentProjectFile\\recentProjectFile.xml");
                xmlReader = XmlReader.Create(sr);
                if (!xmls.CanDeserialize(xmlReader))
                {
                    _globalParams.RecentProjectPaths = new ObservableCollection<ProjectInfo>();
                    res = false;
                }
                else
                {
                    _globalParams.RecentProjectPaths = (ObservableCollection<ProjectInfo>)xmls.Deserialize(xmlReader);
                    res = true;
                }
                return res;
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                res = false;
                return res;
            }
            finally
            {

                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
            }
        }
        
        
        private void LoadScanFileLib()
        {
            string dir = SCANCONFIGFILES_LIB_PATH;
            if (Directory.Exists(dir))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                FileInfo[] fileInfoList = dirInfo.GetFiles();

                foreach (FileInfo fileInfo in fileInfoList)
                {
                    string fileName = fileInfo.FullName;
                    _globalParams.OriginalScanFiles.Add(fileInfo.FullName);

                    ScanBoardProperty scanBdProp = new ScanBoardProperty();
                    if (CustomTransform.LoadScanProFile(fileName, ref scanBdProp))
                    {
                        ScannerCofigInfo info = new ScannerCofigInfo();
                        info.DataGroup = scanBdProp.StandardLedModuleProp.DataGroup;
                        info.DisplayName = Path.GetFileNameWithoutExtension(fileName);
                        info.ScanBdProp = scanBdProp;
                        
                        string strCascade = scanBdProp.ModCascadeType.ToString();
                        CommonStaticMethod.GetLanguageString(strCascade, strCascade, out strCascade);
                        info.StrCascadeType = strCascade;

                        string strDriverChip = scanBdProp.StandardLedModuleProp.DriverChipType.ToString();
                        CommonStaticMethod.GetLanguageString(strDriverChip, strDriverChip, out strDriverChip);
                        info.StrChipType = strDriverChip;

                        string strScanType = scanBdProp.StandardLedModuleProp.ScanType.ToString();
                        CommonStaticMethod.GetLanguageString(strScanType, strScanType, out strScanType);
                        info.StrScanType = strScanType;
                        info.ScanBdSizeType = ScannerSizeType.NoCustom;
                        _globalParams.ScannerConfigCollection.Add(info);
                    }
                }
                //ScannerCofigInfo customScannerConfigInfo = new ScannerCofigInfo();
                //customScannerConfigInfo.DisplayName = "自定义";
                //customScannerConfigInfo.ScanBdSizeType = ScannerSizeType.Custom;
                //_globalParams.ScannerConfigCollection.Add(customScannerConfigInfo);
            }
        }
        private bool LoadSenderAndPortPicFile()
        {
            XmlSerializer xmls = null;
            StreamReader sr = null;
            XmlReader xmlReader = null;
            string msg = string.Empty;
            bool res = false;
            try
            {
                xmls = new XmlSerializer(typeof(ObservableCollection<SenderAndPortPicInfo>));
                sr = new StreamReader(SmartLCTViewModeBase.ReleasePath + "Resource\\Config\\SenderAndPortPicInfo\\SenderAndPortPicInfo.xml");
                xmlReader = XmlReader.Create(sr);
                if (!xmls.CanDeserialize(xmlReader))
                {
                    res = false;
                    return res;
                }
                else
                {
                    ObservableCollection<SenderAndPortPicInfo> senderAndPortPicCollection=(ObservableCollection<SenderAndPortPicInfo>)xmls.Deserialize(xmlReader);
                    for (int i = 0; i < senderAndPortPicCollection.Count; i++)
                    {
                        SenderAndPortPicInfo info=senderAndPortPicCollection[i];
                        _globalParams.SenderAndPortPicCollection.Add(info.Index, info);
                        string picPath = SmartLCTViewModeBase.ReleasePath + info.SelectedPicPath;
                        if (File.Exists(picPath))
                        {
                            _globalParams.SenderAndPortPicCollection[info.Index].SelectedPicPath = picPath;
                        }
                        picPath = SmartLCTViewModeBase.ReleasePath + info.NoSelectedPicPath;
                        if (File.Exists(picPath))
                        {
                            _globalParams.SenderAndPortPicCollection[info.Index].NoSelectedPicPath = picPath;
                        }
                    }
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                res = false;
                return res;
            }
            finally
            {

                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
            }
        }
        private bool LoadSenderFileLib()
        {
            XmlSerializer xmls = null;
            StreamReader sr = null;
            XmlReader xmlReader = null;
            string msg = string.Empty;
            bool res = false;
            try
            {
                xmls = new XmlSerializer(typeof(ObservableCollection<SenderConfigInfo>));
                sr = new StreamReader(SmartLCTViewModeBase.ReleasePath + "Resource\\Config\\sender\\senderConfig.xml");
                xmlReader = XmlReader.Create(sr);
                if (!xmls.CanDeserialize(xmlReader))
                {
                    res = false;
                    return res;
                }
                else
                {
                    _globalParams.SenderConfigCollection = (ObservableCollection<SenderConfigInfo>)xmls.Deserialize(xmlReader);
                    for (int i = 0; i < _globalParams.SenderConfigCollection.Count; i++)
                    {
                        string picPath = SmartLCTViewModeBase.ReleasePath + "Resource\\Config\\sender\\senderICO\\" + _globalParams.SenderConfigCollection[i].SenderTypeName.ToString() + ".jpg";
                        if (File.Exists(picPath))
                        {
                            _globalParams.SenderConfigCollection[i].SenderPicturePath =picPath;
                        }
                    }
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                res = false;
                return res;
            }
            finally
            {

                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
            }
        }

        private void LoadLanguage(string langTag)
        {
            if (!Directory.Exists(APPLICATION_LANG_PATH))
            {
                Directory.CreateDirectory(APPLICATION_LANG_PATH);

            }
            string[] existFolder = Directory.GetDirectories(APPLICATION_LANG_PATH);
            _globalParams.LangItemCollection.Clear();

            foreach (string info in existFolder)
            {
                string folder = Path.GetFileName(info);
                CultureInfo ci = null;
                try
                {
                    ci = new CultureInfo(folder);
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    continue;
                }
                string itemText = ci.NativeName + "(" + folder + ")";


                //if (folder.ToLower() == STR_LANG_ZHCN.ToLower())
                //{
                //    item.Font = _zhchFont;
                //}
                //else if (folder.ToLower() == STR_LANG_KOKR.ToLower())
                //{
                //    item.Font = _kokrFont;
                //}
                //else
                //{
                //    item.Font = _enFont;
                //}
                LangItemData item = new LangItemData()
                {
                    LangDisplayName = itemText,
                    LangFlag = folder,
                    IsSelected = false
                };
                if (folder == langTag)
                {
                    item.IsSelected = true;
                }
                _globalParams.LangItemCollection.Add(item);
            }
            if (_globalParams.LangItemCollection.Count == 0)
            {
                string msg = "No Language File";

                LangItemData item = new LangItemData()
                {
                    LangDisplayName = msg,
                    LangFlag = "",
                    IsSelected = false
                };

                _globalParams.LangItemCollection.Add(item);
            }

            ChangedLanguage(langTag);
        }

        private void CompletedStartUp()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Messenger.Default.Send<string>("", MsgToken.MSG_STARTUP_COMPLETED);
                }));
        }
        #endregion

        #region 读取软件空间完成
        public void OnCompletedReadPortsSoftwareSpaceInfo(object sender, CompletedReadPortsSoftwareSpaceEventArgs args)
        {
            _tempCommPortLedDisplayDict = new Dictionary<string, List<ILEDDisplayInfo>>();
            _tempRedundancyDict = new Dictionary<string, List<SenderRedundancyInfo>>();
            Dictionary<string, GraphicsDVIPortInfo> graphicsDviDict = new Dictionary<string, GraphicsDVIPortInfo>();

            foreach (string key in args.SoftwareInfoDict.Keys)
            {
                SoftwareSpaceTotalInfo info = args.SoftwareInfoDict[key];
                if (info.ScreenInfo.HWInfoState == CommonInfoCompeleteResult.OK)
                {
                    _tempCommPortLedDisplayDict.Add(key, info.ScreenInfo.DisplayInfoList);
                }

                if (info.DviInfo.HWInfoState == CommonInfoCompeleteResult.OK)
                {
                    graphicsDviDict.Add(key, info.DviInfo.GraphicsDviInfo);
                }
                else
                {
                    graphicsDviDict.Add(key, new GraphicsDVIPortInfo());
                }

                if (info.SenderReduInfo.HWInfoState == CommonInfoCompeleteResult.OK)
                {
                    _tempRedundancyDict.Add(key, info.SenderReduInfo.SenderReduInfoList);
                }
            }
            _allCommPortLedDisplayDic = _tempCommPortLedDisplayDict;
            _globalParams.AllCommPortLedDisplayDic = _allCommPortLedDisplayDic;

            _senderRedundancyDic = _tempRedundancyDict;
            _globalParams.SenderReduInfoDic = _senderRedundancyDic;

            _globalParams.GraphicDviInfoDict = graphicsDviDict;
        }

        private void OnCompleteReadAllComHWBaseInfoCallback(CompleteReadAllComHWBaseInfoParams allBaseInfo, object userToken)
        {
            #region 组合硬件读取到的数据
            _globalParams.AllBaseInfo = allBaseInfo.AllInfo;
            if (_globalParams.SupperDisplayList != null)
            {
                _globalParams.SupperDisplayList.Clear();
            }
            foreach (KeyValuePair<string, OneCOMHWBaseInfo> pair in allBaseInfo.AllInfo.AllInfoDict)
            {
                string firstSN = pair.Value.FirstSenderSN;
                for (int i = 0; i < pair.Value.LEDDisplayInfoList.Count; i++)
                {
                    string udid = firstSN + i.ToString("x2");
                    List<OneScreenInSupperDisplay> scrList = new List<OneScreenInSupperDisplay>();
                    scrList.Add(new OneScreenInSupperDisplay()
                        {
                            ScreenUDID = udid,
                        });
                    SupperDisplay supper = new SupperDisplay()
                    {
                        DisplayName = "DisplayName",
                        DisplayUDID = udid,
                        ScreenList = scrList
                    };

                    _globalParams.SupperDisplayList.Add(supper);
                }
            }
            #endregion

            #region 从数据库读取配置
            SQLiteAccessor accessor = SQLiteAccessor.Instance;
            if (accessor == null)
            {
                Debug.WriteLine("数据库对象错误，终止智能亮度");
                return;
            }
            List<DisplaySmartBrightEasyConfig> cfgList = accessor.GetAllNeedEasyConfig(new SmartBrightSeleCondition() { BrightAdjMode = BrightAdjustMode.SmartBright, DataVersion = -1 });
            
            
            #endregion

            #region 开始智能亮度
            if (_smartBrightManager != null)
            {
                _smartBrightManager.Dispose();
                _smartBrightManager = null;
            }
            _smartBrightManager = new SmartBright(_proxy, _globalParams.AllBaseInfo, _globalParams.SupperDisplayList);
            _globalParams.SmartBrightManager = _smartBrightManager;

            for (int i = 0; i < cfgList.Count; i++)
            {
                DisplaySmartBrightEasyConfig cfg = cfgList[i];
                if (cfg != null &&
                    cfg.OneDayConfigList != null)
                {
                    int index = _globalParams.SupperDisplayList.FindIndex(delegate(SupperDisplay supperTemp)
                    {
                        if (supperTemp.DisplayUDID == cfg.DisplayUDID)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    });
                    if (index != -1)
                    {
                        _smartBrightManager.AttachSmartBright(cfg.DisplayUDID, cfg);
                    }
                }
            }

            #endregion
        }
        #endregion

        #endregion

        #region 重写基类
        protected override void OnAllCommPortLedDisplayDicChanged()
        {
            //RestartMonitor(_globalParams.AllCommPortLedDisplayDic);
            base.OnAllCommPortLedDisplayDicChanged();
        }
        #endregion

        #region IDisposable 成员
        public override void Cleanup()
        {
            TerminateServerProxy();
            Messenger.Default.Unregister<string>(this, MsgToken.MSG_SCREENINFO_CHANGED, OnScreenChanged);
            base.Cleanup();

        }
        #endregion
    }
}
