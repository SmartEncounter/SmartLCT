using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using Nova.LCT.Message.Client;
using Nova.LCT.GigabitSystem.Common;
using System.Diagnostics;
using Nova.Message.Common;
using Nova.IO.Port;
using Nova.SmartLCT.Interface;
using Nova.Equipment.Protocol.TGProtocol;
using Nova.LCT.GigabitSystem.Files;
using Nova.LCT.GigabitSystem.HWConfigAccessor;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;
using Nova.SmartLCT.Database;

namespace Nova.SmartLCT.UI
{
    public class ScreenBrightInfo : SmartLCTViewModeBase
    {
        #region 界面属性

        /// <summary>
        /// 当前选中的亮度调节模式
        /// </summary>
        public BrightAdjustMode SelectedBrightAdjustMode
        {
            get
            {
                return _selectedBrightAdjustMode;
            }
            set
            {
                _selectedBrightAdjustMode = value;
                
                NotifyPropertyChanged(GetPropertyName(o => this.SelectedBrightAdjustMode));
            }
        }
        private BrightAdjustMode _selectedBrightAdjustMode = BrightAdjustMode.Manual;

        public ObservableCollection<OneSmartBrightEasyConfig> DisplaySmartBrightCfg
        {
            get
            {
                return _displaySmartBrightCfg;
            }
            set
            {
                _displaySmartBrightCfg = value;
                RaisePropertyChanged(GetPropertyName(o => this.DisplaySmartBrightCfg));
            }
        }
        private ObservableCollection<OneSmartBrightEasyConfig> _displaySmartBrightCfg = new ObservableCollection<OneSmartBrightEasyConfig>();


        public OneSmartBrightEasyConfig SelectedSmartBrightItem
        {
            get
            {
                return _selectedSmartBrightItem;
            }
            set
            {
                _selectedSmartBrightItem = value;
                RaisePropertyChanged(GetPropertyName(o => this.SelectedSmartBrightItem));
            }
        }
        private OneSmartBrightEasyConfig _selectedSmartBrightItem = null;
        /// <summary>
        /// 显示屏的名称
        /// </summary>
        public string ScreenName
        {
            get { return _screenName; }
            set
            {
                _screenName = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ScreenName));
            }
        }
        private string _screenName = "";

        public string DisplayUDID
        {
            get
            {
                return _displayUDID;
            }
            set
            {
                _displayUDID = value;
                RaisePropertyChanged(GetPropertyName(o => this.DisplayUDID));
            }
        }
        private string _displayUDID = "Design";

        /// <summary>
        /// 画质
        /// </summary>
        public ScreenQuality ScreenDisplayQuality
        {
            get { return _screenDisplayQuality; }
            set
            {
                _screenDisplayQuality = value;
                CmdSetGamma.Execute(GammaValue);
                NotifyPropertyChanged(GetPropertyName(o => this.ScreenDisplayQuality));
            }
        }
        private ScreenQuality _screenDisplayQuality = ScreenQuality.Soft;
        /// <summary>
        /// AB模式
        /// </summary>
        public GammaABMode GammaDisplayABMode
        {
            get { return _gammaDisplayABMode; }
            set
            {
                _gammaDisplayABMode = value;
                CmdSetGamma.Execute(GammaValue);
                NotifyPropertyChanged(GetPropertyName(o => this.GammaDisplayABMode));

            }
        }
        private GammaABMode _gammaDisplayABMode = GammaABMode.GammaA;
        /// <summary>
        /// 全局量度
        /// </summary>
        public int GlobalBright
        {
            get { return _globalBright; }
            set
            {
                _globalBright = value;
                NotifyPropertyChanged(GetPropertyName(o => this.GlobalBright));
            }
        }
        private int _globalBright = 234;
        /// <summary>
        /// gamma值
        /// </summary>
        public int GammaValue
        {
            get { return _gammaValue; }
            set
            {
                _gammaValue = value;
                NotifyPropertyChanged(GetPropertyName(o => this.GammaValue));
            }
        }
        private int _gammaValue = 28;
        /// <summary>
        /// 红色分量
        /// </summary>
        public int RedBright
        {
            get { return _redBright; }
            set
            {
                _redBright = value;
                if (_isSyncBright)
                {
                    AdjustBrightSynchronous(value);
                }
                NotifyPropertyChanged(GetPropertyName(o => this.RedBright));
            }
        }
        private int _redBright = 254;
        /// <summary>
        /// 绿色分量
        /// </summary>
        public int GreenBright
        {
            get { return _greenBright; }
            set
            {
                _greenBright = value;
                if (_isSyncBright)
                {
                    AdjustBrightSynchronous(value);
                }
                NotifyPropertyChanged(GetPropertyName(o => this.GreenBright));
            }
        }
        private int _greenBright = 250;
        /// <summary>
        /// 蓝色分量
        /// </summary>
        public int BlueBright
        {
            get { return _blueBright; }
            set
            {
                _blueBright = value;
                if (_isSyncBright)
                {
                    AdjustBrightSynchronous(value);
                }
                NotifyPropertyChanged(GetPropertyName(o => this.BlueBright));
            }
        }
        private int _blueBright = 240;
        /// <summary>
        /// 色温
        /// </summary>
        public int ColorTemp
        {
            get { return _colorTemp; }
            set
            {
                _colorTemp = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ColorTemp));
            }
        }
        private int _colorTemp = 6500;
        /// <summary>
        /// 是否是自定义分量
        /// </summary>
        public bool IsCustomRGBBright
        {
            get { return _isCustomRGBBright; }
            set
            {
                _isCustomRGBBright = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsCustomRGBBright));
            }
        }
        private bool _isCustomRGBBright = false;
        /// <summary>
        /// 红增益
        /// </summary>
        public int RedGain
        {
            get { return _redGain; }
            set
            {
                _redGain = value;
                if (_isSyncGain)
                {
                    AdjustCurrentGainSynchronous(RedGain);
                }
                if (_curChipInfo != null)
                {
                    int registorVal = 0;
                    float curGain = 0;
                    GetRegistorGainFromStep(_curChipInfo, value, out registorVal, out curGain);
                    curGain = curGain * 100;
                    string temp = "(" + curGain.ToString("f2") + " %)";
                    RedGainPercent = temp;
                }
                NotifyPropertyChanged(GetPropertyName(o => this.RedGain));
            }
        }
        private int _redGain = 63;
        /// <summary>
        /// 绿增益
        /// </summary>
        public int GreenGain
        {
            get { return _greenGain; }
            set
            {
                _greenGain = value;
                if (_isSyncGain)
                {
                    AdjustCurrentGainSynchronous(GreenGain);
                }
                if (_curChipInfo != null)
                {
                    int registorVal = 0;
                    float curGain = 0;
                    GetRegistorGainFromStep(_curChipInfo, value, out registorVal, out curGain);
                    curGain = curGain * 100;
                    string temp = "(" + curGain.ToString("f2") + " %)";
                    GreenGainPercent = temp;
                }
                NotifyPropertyChanged(GetPropertyName(o => this.GreenGain));
            }
        }
        private int _greenGain = 63;
        /// <summary>
        /// 红增益
        /// </summary>
        public int BlueGain
        {
            get { return _blueGain; }
            set
            {
                _blueGain = value;
                if (_isSyncGain)
                {
                    AdjustCurrentGainSynchronous(BlueGain);
                }
                if (_curChipInfo != null)
                {
                    int registorVal = 0;
                    float curGain = 0;
                    GetRegistorGainFromStep(_curChipInfo, value, out registorVal, out curGain);
                    curGain = curGain * 100;
                    string temp = "(" + curGain.ToString("f2") + " %)";
                    BlueGainPercent = temp;
                }
                NotifyPropertyChanged(GetPropertyName(o => this.BlueGain));
            }
        }
        private int _blueGain = 63;
        /// <summary>
        /// 红电流增益百分比
        /// </summary>
        public string RedGainPercent
        {
            get { return _redGainPercent; }
            set
            {
                _redGainPercent = value;
                NotifyPropertyChanged(GetPropertyName(o => this.RedGainPercent));
            }
        }
        private string _redGainPercent = "0";
        /// <summary>
        /// 绿电流增益百分比
        /// </summary>
        public string GreenGainPercent
        {
            get { return _greenGainPercent; }
            set
            {
                _greenGainPercent = value;
                NotifyPropertyChanged(GetPropertyName(o => this.GreenGainPercent));
            }
        }
        private string _greenGainPercent = "0";
        /// <summary>
        /// 蓝电流增益百分比
        /// </summary>
        public string BlueGainPercent
        {
            get { return _blueGainPercent; }
            set
            {
                _blueGainPercent = value;
                NotifyPropertyChanged(GetPropertyName(o => this.BlueGainPercent));
            }
        }
        private string _blueGainPercent = "0";

        /// <summary>
        /// 默认的色温
        /// </summary>
        public DoubleCollection ColorTempTicksCollection
        {
            get { return _colorTempTicksCollection; }
            set
            {
                _colorTempTicksCollection = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ColorTempTicksCollection));
            }
        }
        private DoubleCollection _colorTempTicksCollection = null;
        /// <summary>
        /// 是否同步分量亮度
        /// </summary>
        public bool IsSyncBright
        {
            get { return _isSyncBright; }
            set
            {
                _isSyncBright = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsSyncBright));
            }
        }
        private bool _isSyncBright = false;
        /// <summary>
        /// 是否同步分量亮度
        /// </summary>
        public bool IsSyncGain
        {
            get { return _isSyncGain; }
            set
            {
                _isSyncGain = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsSyncGain));
            }
        }
        private bool _isSyncGain = false;

        public bool IsSupportCurrentGain
        {
            get { return _isSupportCurrentGain; }
            set
            {
                _isSupportCurrentGain = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsSupportCurrentGain));
            }
        }
        private bool _isSupportCurrentGain = false;

        public string ChipName
        {
            get { return _chipName; }
            set
            {
                _chipName = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ChipName));
            }
        }
        private string _chipName = "Design";

        public bool IsEnableLowGrayBright
        {
            get { return _isEnableLowGrayBright; }
            set
            {
                _isEnableLowGrayBright = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsEnableLowGrayBright));
            }
        }
        private bool _isEnableLowGrayBright = false;

        public int MaxGainStep
        {
            get { return _maxGainStep; }
            set
            {
                _maxGainStep = value;
                NotifyPropertyChanged(GetPropertyName(o => this.MaxGainStep));
            }
        }
        private int _maxGainStep = 63;

        #endregion

        #region 命令
        public RelayCommand<double> CmdSetDefaultColorTemp
        {
            get;
            private set;
        }

        public RelayCommand<int> CmdSetGamma
        {
            get;
            private set;
        }
        public RelayCommand CmdAddTimingAdjustData
        {
            get;
            private set;
        }
        public RelayCommand CmdEditTimingAdjustData
        {
            get;
            private set;
        }
        public RelayCommand CmdDeleteSelectedTimingAdjustData
        {
            get;
            private set;
        }
        public RelayCommand CmdClearTimingAdjustData
        {
            get;
            private set;
        }
        public RelayCommand<string> CmdOK
        {
            get;
            private set;
        }

        public RelayCommand CmdBrightAdjustModeClick
        {
            get;
            private set;
        }
        #region 亮度
        public RelayCommand<int> CmdSetGlobalBright
        {
            get;
            private set;
        }

        public RelayCommand<int> CmdSetRedBright
        {
            get;
            private set;
        }

        public RelayCommand<int> CmdSetGreenBright
        {
            get;
            private set;
        }

        public RelayCommand<int> CmdSetBlueBright
        {
            get;
            private set;
        }

        #endregion

        #region 电流增益
        public RelayCommand CmdSetCurrentGain
        {
            get;
            private set;
        }
        public RelayCommand CmdSetDefalutCurGain
        {
            get;
            private set;
        }
        #endregion

        #region 同步Checked
        public RelayCommand<bool> CmdSetSyncBright
        {
            get;
            private set;
        }
        public RelayCommand<bool> CmdSetSyncGain
        {
            get;
            private set;
        }
        #endregion

        #region 色温
        public RelayCommand<int> CmdSetColorTemp
        {
            get;
            private set;
        }
        #endregion

        #region 读接收卡信息
        public RelayCommand CmdReadScanBdProp
        {
            get;
            private set;
        }
        #endregion

        #region 从数据库读取配置
        public RelayCommand<string> CmdReadFromDB
        {
            get;
            private set;
        }
        #endregion

        #region 外设配置
        public RelayCommand CmdPeripheralsSetting
        {
            get;
            private set;
        }
        #endregion

        #endregion

        #region 属性

        public ILEDDisplayInfo DisplayInfo
        {
            get { return _displayInfo; }
            set { _displayInfo = value; }
        }
        private ILEDDisplayInfo _displayInfo = null;

        public string SelectedPort
        {
            get { return _selectedPort; }
            set
            {
                _selectedPort = value;
            }
        }
        private string _selectedPort = "";

        #endregion

        #region 字段
        private bool _isBusySynchronous = false;
        private ScanBoardProperty _scanBdProp = null;
        private ChipInfo _curChipInfo = null;
        private ChipRegistorConfigBase _chipConfig = null;
        private Dictionary<int, ColorTempRGBMapping> _colorTempRGBMapDict = null;
        private PeripheralsSettingParam _sensorConfig = new PeripheralsSettingParam();
        #endregion

        public ScreenBrightInfo()
        {
            DoubleCollection collection = new DoubleCollection();
            collection.Add(3000);
            collection.Add(5000);
            collection.Add(6500);
            collection.Add(9300);
            ColorTempTicksCollection = collection;
        
            #region 命令绑定

            CmdSetDefaultColorTemp = new RelayCommand<double>(OnCmdSetDefaultColorTemp);
            CmdSetGamma = new RelayCommand<int>(OnCmdSetGamma);
            CmdSetGlobalBright = new RelayCommand<int>(OnCmdSetGlobalBright);
            CmdSetRedBright = new RelayCommand<int>(OnCmdSetRedBright);
            CmdSetGreenBright = new RelayCommand<int>(OnCmdSetGreenBright);
            CmdSetBlueBright = new RelayCommand<int>(OnCmdSetBlueBright);
            CmdSetCurrentGain = new RelayCommand(OnCmdSetCurrentGain);
            CmdSetSyncBright = new RelayCommand<bool>(OnCmdSetSyncBright);
            CmdSetSyncGain = new RelayCommand<bool>(OnCmdSetSyncGain);
            CmdSetColorTemp = new RelayCommand<int>(OnCmdSetColorTemp);
            CmdReadScanBdProp = new RelayCommand(OnCmdReadScanBdProp);
            CmdSetDefalutCurGain = new RelayCommand(OnCmdSetDefalutCurGain);

            CmdAddTimingAdjustData = new RelayCommand(OnAddTimingAdjustData);
            CmdEditTimingAdjustData = new RelayCommand(OnEditSmartBrightItemData, CanEditTimingAdjustData);
            CmdDeleteSelectedTimingAdjustData = new RelayCommand(OnDeleteSelectedTimingAdjustData, CanDeleteSelectedTimingAdjustData);
            CmdClearTimingAdjustData = new RelayCommand(OnClearTimingAdjustData, CanClearTimingAdjustData);
            CmdOK = new RelayCommand<string>(OnOk);

            CmdReadFromDB = new RelayCommand<string>(OnCmdReadFromDB);

            CmdPeripheralsSetting = new RelayCommand(OnCmdPeripheralsSetting);

            CmdBrightAdjustModeClick = new RelayCommand(OnCmdBrightAdjustModeClick);
            #endregion

            #region 初始化
            if (!this.IsInDesignMode)
            {
                _colorTempRGBMapDict = _globalParams.ColorTempRGBMappingDict;

                //SelectedBrightAdjustMode = BrightAdjustMode.SmartBright;

            }
            #endregion
        }

        #region 私有函数
        #region 命令响应
        private void OnCmdBrightAdjustModeClick()
        {
            CmdOK.Execute("Change");
        }

        private void OnCmdPeripheralsSetting()
        {
            NotificationMessageAction<PeripheralsSettingParam> nmcl = new NotificationMessageAction<PeripheralsSettingParam>(this, _sensorConfig, "", OnPeripheralsSettingCallback);
            Messenger.Default.Send<NotificationMessageAction<PeripheralsSettingParam>>(nmcl, MsgToken.MSG_SHOWWINPERIPHERALSCONFIG);
        }

        private void OnPeripheralsSettingCallback(PeripheralsSettingParam param)
        {
            _sensorConfig.ExtendData = param.ExtendData;
        }

        private void OnCmdReadFromDB(string displayUDID)
        {
            SQLiteAccessor accessor = SQLiteAccessor.Instance;
            if (accessor == null)
            {
                return;
            }
            SmartBrightSeleCondition condition = accessor.LoadDisplayEasyConfig(displayUDID);
            if (condition != null &&
                condition.EasyConfig != null &&
                condition.EasyConfig.OneDayConfigList != null)
            {
                for (int i = 0; i < condition.EasyConfig.OneDayConfigList.Count; i++)
                {
                    DisplaySmartBrightCfg.Add(condition.EasyConfig.OneDayConfigList[i]);
                }
            }

            if (condition != null)
            {
                _sensorConfig.ExtendData = condition.EasyConfig.AutoBrightSetting;

                SelectedBrightAdjustMode = condition.BrightAdjMode;
            }           
        }

        private void OnCmdSetDefaultColorTemp(double colorTemp)
        {
            int col = (int)colorTemp;
            this.ColorTemp = col;

            OnCmdSetColorTemp(col);
        }

        private void OnCmdSetGamma(int gammaValue)
        {
            if (_scanBdProp == null)
            {
                return;
            }
            SendGamma(_displayInfo, gammaValue, false, _screenDisplayQuality, _gammaDisplayABMode);
        }

        private void OnCmdReadScanBdProp()
        {
            ReadScanBoardProperty();
        }

        private void OnAddTimingAdjustData()
        {
            NotificationMessageAction<OneSmartBrightEasyConfig> nsa =
                new NotificationMessageAction<OneSmartBrightEasyConfig>(this, null, MsgToken.MSG_SHOWADDSMARTBRIGHT, SetAddSmartBrightItemNotifycationCallBack);
            Messenger.Default.Send(nsa, MsgToken.MSG_SHOWADDSMARTBRIGHT);

        }

        private void SetAddSmartBrightItemNotifycationCallBack(OneSmartBrightEasyConfig config)
        {
            DisplaySmartBrightCfg.Add(config);
            UpdateSmartBrightItem();
        }

        private bool CanEditTimingAdjustData()
        {
            if (SelectedSmartBrightItem == null)
            {
                return false;
            }
            return true;
        }
        private void OnEditSmartBrightItemData()
        {
            NotificationMessageAction<OneSmartBrightEasyConfig> nsa =
            new NotificationMessageAction<OneSmartBrightEasyConfig>(this, SelectedSmartBrightItem, MsgToken.MSG_SHOWEditTIMINGADJUSTBRIGHT, SetEditTimingAdjustDataNotifycationCallBack);
            Messenger.Default.Send(nsa, MsgToken.MSG_SHOWADDSMARTBRIGHT);
        }
        private void SetEditTimingAdjustDataNotifycationCallBack(OneSmartBrightEasyConfig config)
        {
            UpdateSmartBrightItem();
        }
        private bool CanDeleteSelectedTimingAdjustData()
        {
            if (SelectedSmartBrightItem == null)
            {
                return false;
            }
            return true;
        }
        private void OnDeleteSelectedTimingAdjustData()
        {
            string msg = "";
            CommonStaticMethod.GetLanguageString("确定删除选择智能亮度项吗？", "Lang_Bright_SureDeleSmartBrightItem", out msg);
            if (ShowQuestionMessage(msg, System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.OK)
            {
                DisplaySmartBrightCfg.Remove(SelectedSmartBrightItem);
            }
        }
        private void OnClearTimingAdjustData()
        {
            string msg = "";
            CommonStaticMethod.GetLanguageString("确定清除所有智能亮度项吗？", "Lang_Bright_SureClearSmartBrightItem", out msg);
            if (ShowQuestionMessage(msg, System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.OK)
            {
                DisplaySmartBrightCfg.Clear();
            }
        }

        private bool CanClearTimingAdjustData()
        {
            if (DisplaySmartBrightCfg.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void OnOk(string param)
        {
            if (IsHaveAutoBright() &&
                SelectedBrightAdjustMode == BrightAdjustMode.SmartBright)
            {
                string msg = "";
                if (_sensorConfig == null ||
                    _sensorConfig.ExtendData == null)
                {
                    CommonStaticMethod.GetLanguageString("智能亮度使用了自动亮度调节，需要首先配置外设！", "Lang_Bright_SmartAutoBrightNeedConfig", out msg);
                }
                else
                {
                    if (_sensorConfig.ExtendData.AutoBrightMappingList == null ||
                        _sensorConfig.ExtendData.AutoBrightMappingList.Count == 0)
                    {
                        CommonStaticMethod.GetLanguageString("没有配置环境亮度与显示屏亮度之间的关系！", "Lang_Bright_NoConfigEnviScrBrightMapping", out msg);
                    }
                    else if (_sensorConfig.ExtendData.UseLightSensorList == null ||
                        _sensorConfig.ExtendData.UseLightSensorList.Count == 0)
                    {
                        CommonStaticMethod.GetLanguageString("请选择显示屏所使用到的光探头！", "Lang_Bright_NoLightSensorSele", out msg);
                    }
                }

                if (!string.IsNullOrEmpty(msg))
                {
                    ShowGlobalDialogMessage(msg, System.Windows.MessageBoxImage.Error);
                    return;
                }
            }

            bool res = SaveConfigToDB();

            if (string.IsNullOrEmpty(param))
            {
                string okMsg = "";
                if (res)
                {
                    CommonStaticMethod.GetLanguageString("设置成功！", "Lang_Bright_SmartBrightSettingSucceed", out okMsg);
                }
                else
                {
                    CommonStaticMethod.GetLanguageString("设置失败！", "Lang_Bright_SmartBrightSettingFailed", out okMsg);
                }
                ShowGlobalDialogMessage(okMsg, System.Windows.MessageBoxImage.Information);
            }
        }

        private void UpdateSmartBrightItem()
        {
            DisplaySmartBrightCfg.OrderBy(o => o.StartTime.Hour);
        }

        #region 亮度

        private void OnCmdSetGlobalBright(int bright)
        {
            SendGlobalBright(_displayInfo, bright);
        }

        private void OnCmdSetRedBright(int bright)
        {
            if (!_isSyncBright)
            {
                SendRedBright(_displayInfo, bright);
            }
            else
            {
                SendSynchronousBright(_displayInfo, bright);
            }
        }

        private void OnCmdSetGreenBright(int bright)
        {
            if (!_isSyncBright)
            {
                SendGreenBright(_displayInfo, bright);
            }
            else
            {
                SendSynchronousBright(_displayInfo, bright);
            }
        }

        private void OnCmdSetBlueBright(int bright)
        {
            if (!_isSyncBright)
            {
                SendBlueBright(_displayInfo, bright);
            }
            else
            {
                SendSynchronousBright(_displayInfo, bright);
            }
        }
        #endregion

        #region 电流增益
        private void OnCmdSetCurrentGain()
        {
            SendCurrentGain(_displayInfo, RedGain, GreenGain, BlueGain);
        }

        private void OnCmdSetDefalutCurGain()
        {
            if (_scanBdProp == null)
            {
                return;
            }

            ChipType chipType = _scanBdProp.StandardLedModuleProp.DriverChipType;

            if (_curChipInfo != null)
            {
                ChipInfo chipInfo = _curChipInfo;
                int nDefalutStep = 0;
                byte registorValue = 0;
                float fCurrentGain = 0.0f;

                chipInfo.GetDefaultGain(out nDefalutStep,
                                        out registorValue,
                                        out fCurrentGain);

                RedGain = (byte)nDefalutStep;
                GreenGain = (byte)nDefalutStep;
                BlueGain = (byte)nDefalutStep;

                SendCurrentGain(_displayInfo, RedGain, GreenGain, BlueGain);
            }
        }
        #endregion

        #region 同步Checked
        private void OnCmdSetSyncBright(bool isSync)
        {
            if (isSync)
            {
                AdjustBrightSynchronous(RedBright);

                SendSynchronousBright(_displayInfo, RedBright);
            }
        }
        private void OnCmdSetSyncGain(bool isSync)
        {
            if (_isSyncGain)
            {
                AdjustCurrentGainSynchronous(RedGain);

                SendCurrentGain(_displayInfo, RedGain, GreenGain, BlueGain);
            }
        }
        #endregion
        #endregion

        #region 数据库操作
        private bool SaveConfigToDB()
        {
            DisplaySmartBrightEasyConfig easyConfig = GetEasyConfigFromUI();

            ApplyHWBrightMode(easyConfig);

            SQLiteAccessor accessor = SQLiteAccessor.Instance;
            
            if (accessor == null)
            {
                return false;
            }
            SmartBrightSeleCondition condition = new SmartBrightSeleCondition()
            {
                BrightAdjMode = SelectedBrightAdjustMode,
                DataVersion = 0,
                EasyConfig = easyConfig
            };
            return accessor.SaveDisplayEasyConfig(easyConfig.DisplayUDID, condition);
        }

        private void ApplyHWBrightMode(DisplaySmartBrightEasyConfig easyConfig)
        {
            if (_globalParams.SmartBrightManager != null)
            {
                if (SelectedBrightAdjustMode == BrightAdjustMode.SmartBright)
                {
                    ((SmartBright)_globalParams.SmartBrightManager).AttachSmartBright(easyConfig.DisplayUDID, easyConfig);
                }
                else
                {
                    ((SmartBright)_globalParams.SmartBrightManager).DetachSmartBright(easyConfig.DisplayUDID);
                }
            }
        }

        private DisplaySmartBrightEasyConfig GetEasyConfigFromUI()
        {
            DisplaySmartBrightEasyConfig easyConfig = new DisplaySmartBrightEasyConfig()
            {
                DisplayUDID = this.DisplayUDID,
                OneDayConfigList = new List<OneSmartBrightEasyConfig>(),
                AutoBrightSetting = _sensorConfig.ExtendData
            };
            for (int i = 0; i < DisplaySmartBrightCfg.Count; i++)
            {
                easyConfig.OneDayConfigList.Add(DisplaySmartBrightCfg[i]);
            }
            return easyConfig;
        }
        #endregion

        #region 色温调节
        private void OnCmdSetColorTemp(int colorTemp)
        {
            if (_colorTempRGBMapDict != null &&
                _colorTempRGBMapDict.ContainsKey(colorTemp))
            {
                ColorTempRGBMapping map = _colorTempRGBMapDict[colorTemp];
                this.IsSyncBright = false;
                this.RedBright = map.RedBright;
                this.GreenBright = map.GreenBright;
                this.BlueBright = map.BlueBright;

                SendRedBright(_displayInfo, this.RedBright);
                SendGreenBright(_displayInfo, this.GreenBright);
                SendBlueBright(_displayInfo, this.BlueBright);

            }
        }
        #endregion

        #region 硬件处理
        private void SendGamma(ILEDDisplayInfo displayInfo, int gammaValue,
            bool isCustomGamma, ScreenQuality screenDisplayQuality, GammaABMode gammaABMode)
        {
            bool isContrastStrength = false;
            bool isModeB = false;
            if (screenDisplayQuality == ScreenQuality.Enhance)
            {
                isContrastStrength = true;
            }
            if (gammaABMode == GammaABMode.GammaB)
            {
                isModeB = true;
            }
            //这里的gammaValue是没有添加上自定义信息之前的gamma
            SaveToGammaTable(gammaValue, isContrastStrength, isModeB);

            byte sendGamma = CustomTransform.LogicalGammaToRealGamma((byte)gammaValue, isCustomGamma, isContrastStrength, isModeB);


            List<ScreenPortAddrInfo> addrList;
            displayInfo.GetScreenPortAddrInfo(out addrList);
            for (int i = 0; i < addrList.Count; i++)
            {
                ScreenPortAddrInfo info = addrList[i];
                PackageRequestWriteData writePack = TGProtocolParser.SetGamma(SelectedPort,
                    info.SenderIndex,
                    info.PortIndex,
                    SystemAddress.SCANBOARD_BROADCAST_ADDRESS,
                    CommandTimeOut.SENDER_SIMPLYCOMMAND_TIMEOUT,
                    "SetGamma",
                    false,
                    sendGamma,
                    null,
                    CompleteRequestDealing);
                _serverProxy.SendRequestWriteData(writePack);

                PackageRequestWriteFile writeFilePack = TGProtocolParser.SetRedGammaTable(SelectedPort,
                                                             info.SenderIndex,
                                                             info.PortIndex,
                                                             SystemAddress.SCANBOARD_BROADCAST_ADDRESS,
                                                             CommandTimeOut.SENDER_SIMPLYFILE_TIMEOUT,
                                                             "SetRedGammaTable",
                                                             false,
                                                             ConstValue.TEMP_GAMMA_FILE,
                                                             0,
                                                             null,
                                                             null);
                _serverProxy.SendRequestWriteFile(writeFilePack);
            }
        }

        private void SaveToGammaTable(int gammaValue, bool isContrastStrength, bool isModeB)
        {
            GammaFileCreator.WriteGammaTable(ConstValue.TEMP_GAMMA_FILE,
                                             gammaValue / 10.0f,
                                             _scanBdProp.GrayDepth,
                                             1.0f,
                                             isContrastStrength,
                                             isModeB,
                                             _scanBdProp.LowGrayQuery,
                                             _scanBdProp.GrayRealize);
        }

        private void SendGlobalBright(ILEDDisplayInfo displayInfo, int globalBright)
        {
            List<ScreenPortAddrInfo> addrList;
            displayInfo.GetScreenPortAddrInfo(out addrList);

            //if (!ChipInherentProperty.IsPWMChip(_scanBdProp.StandardLedModuleProp.DriverChipType))
            //{
            //    #region 将低灰模式表按亮度由低到高排序
            //    LowBrightnessTable lowBrightTable = HWAccessorCalculator.GetUseableLowBrightTable(_scanBdProp, _senderProp);
            //    lowBrightTable.LowBrightInfoList.Sort(delegate(OneLowBrightnessInfo first, OneLowBrightnessInfo second)
            //    {
            //        if (first.BrightnessEfficiency > second.BrightnessEfficiency)
            //        {
            //            return 1;
            //        }
            //        else if (first.BrightnessEfficiency < second.BrightnessEfficiency)
            //        {
            //            return -1;
            //        }
            //        else
            //        {
            //            return 0;
            //        }
            //    });
            //    #endregion

            //    #region 根据亮度获取要使用的模式表
            //    int count = lowBrightTable.LowBrightInfoList.Count;
            //    float maxBrightEf = lowBrightTable.LowBrightInfoList[count - 1].BrightnessEfficiency;
            //    float curBrightEf = globalBright / 255.0f * maxBrightEf;

            //    OneLowBrightnessInfo oneInfo = null;
            //    for (int i = 0; i < count; i++)
            //    {
            //        if (curBrightEf > lowBrightTable.LowBrightInfoList[i].BrightnessEfficiency)
            //        {
            //            continue;
            //        }
            //        oneInfo = lowBrightTable.LowBrightInfoList[i];
            //    }
            //    #endregion
            //}

            for (int i = 0; i < addrList.Count; i++)
            {
                ScreenPortAddrInfo info = addrList[i];
                PackageRequestWriteData writePack = TGProtocolParser.SetGlobalBrightness(SelectedPort,
                    info.SenderIndex,
                    info.PortIndex,
                    SystemAddress.SCANBOARD_BROADCAST_ADDRESS,
                    CommandTimeOut.SENDER_SIMPLYCOMMAND_TIMEOUT,
                    "SetGlobalBrightness",
                    false,
                    globalBright,
                    null,
                    CompleteRequestDealing);
                _serverProxy.SendRequestWriteData(writePack);
            }
        }

        private void SendRedBright(ILEDDisplayInfo displayInfo, int redBright)
        {
            List<ScreenPortAddrInfo> addrList;
            displayInfo.GetScreenPortAddrInfo(out addrList);

            for (int i = 0; i < addrList.Count; i++)
            {
                ScreenPortAddrInfo info = addrList[i];
                PackageRequestWriteData writePack = TGProtocolParser.SetRedBrightness(SelectedPort,
                    info.SenderIndex,
                    info.PortIndex,
                    SystemAddress.SCANBOARD_BROADCAST_ADDRESS,
                    CommandTimeOut.SENDER_SIMPLYCOMMAND_TIMEOUT,
                    "SetRedBrightness",
                    false,
                    redBright,
                    null,
                    CompleteRequestDealing);
                _serverProxy.SendRequestWriteData(writePack);
            }
        }

        private void SendGreenBright(ILEDDisplayInfo displayInfo, int greenBright)
        {
            List<ScreenPortAddrInfo> addrList;
            displayInfo.GetScreenPortAddrInfo(out addrList);

            for (int i = 0; i < addrList.Count; i++)
            {
                ScreenPortAddrInfo info = addrList[i];
                PackageRequestWriteData writePack = TGProtocolParser.SetGreenBrightness(SelectedPort,
                    info.SenderIndex,
                    info.PortIndex,
                    SystemAddress.SCANBOARD_BROADCAST_ADDRESS,
                    CommandTimeOut.SENDER_SIMPLYCOMMAND_TIMEOUT,
                    "SetGreenBrightness",
                    false,
                    greenBright,
                    null,
                    CompleteRequestDealing);
                _serverProxy.SendRequestWriteData(writePack);
            }
        }

        private void SendBlueBright(ILEDDisplayInfo displayInfo, int blueBright)
        {
            List<ScreenPortAddrInfo> addrList;
            displayInfo.GetScreenPortAddrInfo(out addrList);

            for (int i = 0; i < addrList.Count; i++)
            {
                ScreenPortAddrInfo info = addrList[i];
                PackageRequestWriteData writePack = TGProtocolParser.SetBlueBrightness(SelectedPort,
                    info.SenderIndex,
                    info.PortIndex,
                    SystemAddress.SCANBOARD_BROADCAST_ADDRESS,
                    CommandTimeOut.SENDER_SIMPLYCOMMAND_TIMEOUT,
                    "SetBlueBrightness",
                    false,
                    blueBright,
                    null,
                    CompleteRequestDealing);
                _serverProxy.SendRequestWriteData(writePack);
            }
        }

        private void SendSynchronousBright(ILEDDisplayInfo displayInfo, int bright)
        {
            SendRedBright(displayInfo, bright);
            SendGreenBright(displayInfo, bright);
            SendBlueBright(displayInfo, bright);
         
        }

        private void SendCurrentGain(ILEDDisplayInfo displayInfo, int redGain, int greenGain, int blueGain)
        {
            if (_chipConfig == null)
            {
                return;
            }
            
            #region 获取要发送的数据
            ChipRegistorConfigBase chipConfig = _chipConfig;

            int registor = 0;
            float curGainPercent = 0;
            GetRegistorGainFromStep(_curChipInfo, redGain, out registor, out curGainPercent);
            chipConfig.RedGain = (byte)registor;
            GetRegistorGainFromStep(_curChipInfo, greenGain, out registor, out curGainPercent);
            chipConfig.GreenGain = (byte)registor;
            GetRegistorGainFromStep(_curChipInfo, blueGain, out registor, out curGainPercent);
            chipConfig.BlueGain = (byte)registor;
            GetRegistorGainFromStep(_curChipInfo, redGain, out registor, out curGainPercent);
            chipConfig.VRedGain = (byte)registor;

            byte[] sendData = chipConfig.ToBytes();
            #endregion

            #region 发送数据
            List<ScreenPortAddrInfo> addrList;
            displayInfo.GetScreenPortAddrInfo(out addrList);

            for (int i = 0; i < addrList.Count; i++)
            {
                ScreenPortAddrInfo info = addrList[i];

                PackageRequestWriteData writeDataPack = null;
                if (!chipConfig.IsExtendCfgCmd)
                {
                    writeDataPack = TGProtocolParser.SetConfigRegister(SelectedPort,
                                                                       info.SenderIndex,
                                                                       info.PortIndex,
                                                                       SystemAddress.SCANBOARD_BROADCAST_ADDRESS,
                                                                       CommandTimeOut.SENDER_SIMPLYCOMMAND_TIMEOUT,
                                                                       "SetCurrentGain",
                                                                       false,
                                                                       sendData,
                                                                       null,
                                                                      CompleteRequestDealing);
                }
                else
                {
                    writeDataPack = TGProtocolParser.SetConfigRegisterWriteType2(SelectedPort,
                                                                       info.SenderIndex,
                                                                       info.PortIndex,
                                                                       SystemAddress.SCANBOARD_BROADCAST_ADDRESS,
                                                                       CommandTimeOut.SENDER_SIMPLYCOMMAND_TIMEOUT,
                                                                       "SetCurrentGain",
                                                                       false,
                                                                       sendData,
                                                                       null,
                                                                      CompleteRequestDealing);
                }
                _serverProxy.SendRequestWriteData(writeDataPack);
            }
            #endregion
        }

        private void ReadScanBoardProperty()
        {
            ScanBoardRegionInfo scanBdInfo;
            int index = _displayInfo.GetFirstScanBoardInList(out scanBdInfo);
            if (index == -1)
            {
                return;
            }

            ScannerPropertyReader read = new ScannerPropertyReader(_serverProxy, SelectedPort);
            read.ReadScanBd200ParasInfo(new ScanBoardPosition()
            {
                SenderIndex = scanBdInfo.SenderIndex,
                PortIndex = scanBdInfo.PortIndex,
                ScanBdIndex = scanBdInfo.ConnectIndex
            }, CompletedReadScanBoardProp);
        }
        #endregion

        #region 硬件处理回调
        private void CompleteRequestDealing(object sender, CompletePackageRequestEventArgs e)
        {
            RequestComplete(e.Request.PackResult, e.Request.CommResult, (AckResults)e.Request.AckCode, e.Request.Tag);
        }
        private void RequestComplete(PackageResults packageRes, CommResults comRes, AckResults ackType, string strTag)
        {
            #region 操作类型
            string operateStr = "";
            if (strTag == "SetGlobalBrightness")
            {
                CommonStaticMethod.GetLanguageString("设置亮度", "Lang_Bright_SetGlobalBrightness", out operateStr);
            }
            else if (strTag == "SetRedBrightness")
            {
                CommonStaticMethod.GetLanguageString("设置红色亮度", "Lang_Bright_SetRedBrightness", out operateStr);
            }
            else if (strTag == "SetGreenBrightness")
            {
                CommonStaticMethod.GetLanguageString("设置绿色亮度", "Lang_Bright_SetGreenBrightness", out operateStr);
            }
            else if (strTag == "SetBlueBrightness")
            {
                CommonStaticMethod.GetLanguageString("设置蓝色亮度", "Lang_Bright_SetBlueBrightness", out operateStr);
            }
            else if (strTag == "SetGamma")
            {
                CommonStaticMethod.GetLanguageString("设置Gamma", "Lang_Bright_SetRedGammaTable", out operateStr);
            }
            else if (strTag == "SetParameterStore2SpiFlash_Complete")
            {
                CommonStaticMethod.GetLanguageString("保存配置", "Lang_Bright_SaveScanBdConfig", out operateStr);
            }
            else if (strTag == "SetCurrentGain")
            {
                CommonStaticMethod.GetLanguageString("设置电流增益", "Lang_Bright_SetCurrentGain", out operateStr);
            }
            #endregion

            string resStr = "";
            CommonStaticMethod.GetLanguageString("结果", "Lang_Bright_Result", out resStr);
            string errStr = "";

            CommMsgType commMsgType = CommMsgType.Information;

            if (ackType != AckResults.ok
               || comRes != CommResults.ok
               || packageRes != PackageResults.ok)
            {
                commMsgType = CommMsgType.Error;

                if (packageRes == PackageResults.timeOut)
                {
                    CommonStaticMethod.GetLanguageString("失败!原因:超时!", "Lang_Bright_Timeout", out errStr);
                }
                else
                {
                    CommonStaticMethod.GetLanguageString("失败!原因:未知!", "Lang_Bright_Unkonwn", out errStr);
                }
            }
            else
            {
                CommonStaticMethod.GetLanguageString("成功!", "Lang_Bright_Success", out errStr);
                
            }

            string msg = operateStr + "," + resStr + "-" + errStr;
            //NotifyMessage(this, new NotifyMessageEventArgs(new CommMsg() { Msg = msg, MsgType = commMsgType }));
        }
        private void CompletedReadScanBoardProp(CompletedReadScanBdPropInfo info)
        {
            string operateStr = "";

            CommMsgType commType = CommMsgType.Information;

            if (info.ReadRes == ReadConfigRes.Succeed)
            {
                _scanBdProp = info.ScanBdProperty;
                ChipType chipType = _scanBdProp.StandardLedModuleProp.DriverChipType;
                _chipConfig = ChipRegistorConfigFactory.GetChipRegistorConfig(chipType);

                if (ChipInherDict.ContainsKey(chipType))
                {
                    _curChipInfo = ChipInherDict[chipType];
                    IsSupportCurrentGain = _curChipInfo.IsSupportCurrentGain;
                }
                else
                {
                    IsSupportCurrentGain = false;
                }
                UpdateScanBdPropToUI(info.ScanBdProperty);

                CommonStaticMethod.GetLanguageString("读取参数成功！", "Lang_Bright_ReadParams_Succeed", out operateStr);
                commType = CommMsgType.Information;
            }
            else
            {
                _scanBdProp = new ScanBoardProperty();
                IsSupportCurrentGain = false;
                string msg = "";
                CommonStaticMethod.GetLanguageString("读取芯片类型失败！", "Lang_Bright_ReadChipFailed", out msg);
                ChipName = msg;

                CommonStaticMethod.GetLanguageString("读取参数失败！", "Lang_Bright_ReadParams_Failed", out operateStr);
                commType = CommMsgType.Error;
            }

            //NotifyMessage(this, new NotifyMessageEventArgs(new CommMsg() { Msg = operateStr, MsgType = commType }));
        }

        #endregion

        #region 其他
        private void AdjustBrightSynchronous(int bright)
        {
            if (_isBusySynchronous)
            {
                return;
            }
            _isBusySynchronous = true;
            RedBright = bright;
            GreenBright = bright;
            BlueBright = bright;
            _isBusySynchronous = false;
        }

        private void AdjustCurrentGainSynchronous(int currentGain)
        {
            if (_isBusySynchronous)
            {
                return;
            }
            _isBusySynchronous = true;
            RedGain = currentGain;
            GreenGain = currentGain;
            BlueGain = currentGain;
            _isBusySynchronous = false;
        }

        private void GetRegistorGainFromStep(ChipInfo chipInfo, int step, out int registorVal, out float curGainPercent)
        {
            registorVal = 0;
            curGainPercent = 0;
            if (chipInfo == null)
            {
                return;
            }
            Dictionary<int, RegistorGainMapping> mapping = chipInfo.CurrentGainStepInfo.GainStepMapping;
            if (!mapping.ContainsKey(step))
            {
                return;
            }
            registorVal = mapping[step].RegistorValue;
            curGainPercent = mapping[step].CurrentGain;
        }

        private void UpdateScanBdPropToUI(ScanBoardProperty scanBdProp)
        {
            #region 更新亮度
            //_isUpdating = true;
            if (scanBdProp.RedBright == scanBdProp.GreenBright &&
                scanBdProp.RedBright == scanBdProp.BlueBright)
            {
                IsSyncBright = true;
            }
            else
            {
                IsSyncBright = false;
            }

            GlobalBright = scanBdProp.Brightness;
            RedBright = scanBdProp.RedBright;
            GreenBright = scanBdProp.GreenBright;
            BlueBright = scanBdProp.BlueBright;
            #endregion

            #region 更新色温
            ColorTempRGBMapping findMap = _colorTempRGBMapDict.Values.FirstOrDefault(delegate(ColorTempRGBMapping map)
            {
                if (map.RedBright == RedBright &&
                    map.GreenBright == GreenBright &&
                    map.BlueBright == BlueBright)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
            if (findMap != null)
            {
                IsCustomRGBBright = false;
                ColorTemp = findMap.ColorTemp;
            }
            else
            {
                IsCustomRGBBright = true;
            }
            #endregion

            #region 更新电流增益
            if (_curChipInfo != null
                && IsSupportCurrentGain)
            {
                MaxGainStep = _curChipInfo.CurrentGainStepInfo.TotalStep - 1;


                int redStep = 0;
                float redCurGain = 0.0f;
                int greenStep = 0;
                float greenCurGain = 0.0f;
                int blueStep = 0;
                float blueCurGain = 0.0f;

                _curChipInfo.GetStepCurrentGain(scanBdProp.RedGain, out redStep, out redCurGain);
                _curChipInfo.GetStepCurrentGain(scanBdProp.GreenGain, out greenStep, out greenCurGain);
                _curChipInfo.GetStepCurrentGain(scanBdProp.BlueGain, out blueStep, out blueCurGain);

                if (redStep == greenStep &&
                    redStep == blueStep)
                {
                    IsSyncGain = true;
                }
                else
                {
                    IsSyncGain = false;
                }

                RedGain = redStep;
                GreenGain = greenStep;
                BlueGain = blueStep;
            }
            #endregion

            #region 更新Gamma
            bool isCustomGamma = false;
            bool isEhancedMode = false;
            bool isModeB = false;
            byte realGamma = CustomTransform.RealGammaToLogicalGamma(scanBdProp.GammaValue, out isCustomGamma, out isEhancedMode, out isModeB);

            GammaValue = realGamma;

            if (isEhancedMode)
            {
                ScreenDisplayQuality = ScreenQuality.Enhance; 
            }
            else
            {
                ScreenDisplayQuality = ScreenQuality.Soft;
            }

            if (isModeB)
            {
                GammaDisplayABMode = GammaABMode.GammaB;
            }
            else
            {
                GammaDisplayABMode = GammaABMode.GammaA;
            }
            #endregion

            #region 更新芯片类型
            string key = scanBdProp.StandardLedModuleProp.DriverChipType.ToString();
            string chipName = "";
            CommonStaticMethod.GetLanguageString(key, key, out chipName);
            ChipName = chipName;
            #endregion
        }

        private bool IsHaveAutoBright()
        {
            if (SelectedBrightAdjustMode == BrightAdjustMode.Manual)
            {
                return false;
            }
            foreach (OneSmartBrightEasyConfig oneCfg in DisplaySmartBrightCfg)
            {
                if (oneCfg.ScheduleType == SmartBrightAdjustType.AutoBright)
                {
                    return true;
                }
            }
            return false;
                 
        }
        #endregion
        #endregion
    }
}
