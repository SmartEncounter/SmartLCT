using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Nova.LCT.GigabitSystem.Common;
using Nova.LCT.GigabitSystem.HWConfigAccessor;
using Nova.SmartLCT.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Nova.SmartLCT.UI
{
    public class Window_PeripheralsConfig_VM : SmartLCTViewModeBase
    {
        #region 界面属性
        public ObservableCollection<PeripheralUIData> PeripheralList
        {
            get
            {
                return _peripheralList;
            }
            set
            {
                _peripheralList = value;
                RaisePropertyChanged(GetPropertyName(o => this.PeripheralList));
            }
        }
        private ObservableCollection<PeripheralUIData> _peripheralList = new ObservableCollection<PeripheralUIData>();

        public EnvironAndDisplayBrightCollection EnvirAndDisplayBrightCollection
        {
            get { return _envirAndScreenBrightCollection; }
            set
            {
                _envirAndScreenBrightCollection = value;

                RaisePropertyChanged(GetPropertyName(o => this.EnvirAndDisplayBrightCollection));
            }
        }
        private EnvironAndDisplayBrightCollection _envirAndScreenBrightCollection = new EnvironAndDisplayBrightCollection();
        #endregion

        #region 命令
        public RelayCommand<PeripheralsSettingParam> CmdStartFindPeripherals
        {
            get;
            private set;
        }
        public RelayCommand CmdFastSegmentation
        {
            get;
            private set;
        }

        public RelayCommand CmdOK
        {
            get;
            private set;
        }

        public RelayCommand CmdCancel
        {
            get;
            private set;
        }

        public RelayCommand<string> CmdShowValidError
        {
            get;
            private set;
        }
        #endregion

        #region 字段
        PeripheralsFinderAccessor _peripheralFinder = null;
        private PeripheralsSettingParam _oldData = null;
        private FastSegmentParam _fastSementParam;
        #endregion
        public Window_PeripheralsConfig_VM()
        {
            if (!this.IsInDesignMode)
            {
                string title = "";
                CommonStaticMethod.GetLanguageString("光探头设置", "Lang_Bright_PeripheralSettingTitle", out title);
                this.WindowRealTitle = title;

                _peripheralFinder = new PeripheralsFinderAccessor(_globalParams.ServerProxy);
            }
            CmdStartFindPeripherals = new RelayCommand<PeripheralsSettingParam>(OnCmdStartFindPeripherals);
            CmdFastSegmentation = new RelayCommand(OnFastSegmentation);
            CmdOK = new RelayCommand(OnCmdOK);
            CmdCancel = new RelayCommand(OnCmdCancel);
            CmdShowValidError = new RelayCommand<string>(ShowValidError);
        }

        #region 公有函数
        public PeripheralsSettingParam GetPeripheralsSettingParam()
        {
            PeripheralsSettingParam param = new PeripheralsSettingParam();

            List<DisplayAutoBrightMapping> mapList = new List<DisplayAutoBrightMapping>();
            foreach (DisplayAutoBrightMapping map in EnvirAndDisplayBrightCollection)
            {
                mapList.Add((DisplayAutoBrightMapping)map.Clone());
            }

            List<PeripheralsLocation> peripheralList = new List<PeripheralsLocation>();
            foreach (PeripheralUIData periUIData in PeripheralList)
            {
                if (periUIData.IsSelected)
                {
                    peripheralList.Add((PeripheralsLocation)periUIData.Location.Clone());
                }
            }

            param.ExtendData = new AutoBrightExtendData()
            {
                CalcType = AutoBrightCalcType.AllAverage,
                AutoBrightMappingList= mapList,
                UseLightSensorList = peripheralList
            };

            return param;
        }
        #endregion

        private void OnCmdOK()
        {
            
        }

        private void OnCmdCancel()
        {

        }

        private void OnCmdStartFindPeripherals(PeripheralsSettingParam data)
        {
            _oldData = data;

            _peripheralFinder.ReadAllPeripheralsOnSenderOrPortFuncCard(OnReadPeripheralsCallback, null);
            string msg = "";
            CommonStaticMethod.GetLanguageString("正在搜索接入系统的光探测器，请稍后...", "Lang_Bright_ReadAllPeripherals", out msg);
            ShowGlobalProcessBegin(msg);
        }

        private void OnReadPeripheralsCallback(PeripheralsLocateInfo periInfo, object userToken)
        {
            ShowGlobalProcessEnd();

            for (int i = 0; i < periInfo.UseablePeripheralList.Count; i++)
            {
                UseablePeripheral peri = periInfo.UseablePeripheralList[i];
                if (peri.SensorType == PeripheralsType.LightSensorOnSender ||
                    peri.SensorType == PeripheralsType.LightSensorOnFuncCardInPort)
                {
                    PeripheralUIData data = new PeripheralUIData();
                    data.IsSelected = false;
                    data.Location = peri;
                    data.LocationString = GetLocationString(peri, _globalParams.AllBaseInfo);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        PeripheralList.Add(data);
                    }));
                }
            }

            UpdateUI(_oldData);
        }

        private string GetLocationString(UseablePeripheral periInfo, AllCOMHWBaseInfo info)
        {
            string comName = CustomTransform.GetSerialPortNameBySn(periInfo.FirstSenderSN, info);
            string strSender = "";
            CommonStaticMethod.GetLanguageString("发送卡", "Lang_Global_SendingBoard", out strSender);

            string strPort = "";
            CommonStaticMethod.GetLanguageString("网口", "Lang_Global_NetPort", out strPort);

            string strFuncCard = "";
            CommonStaticMethod.GetLanguageString("多功能卡", "Lang_Gloabal_FunctionCard", out strFuncCard);

            string strSensor = "";
            CommonStaticMethod.GetLanguageString("接口", "Lang_Gloabal_SensorIndexOnFuncCard", out strSensor);

            string strSensorInSender = "";
            CommonStaticMethod.GetLanguageString("发送卡上的光探头：", "Lang_Bright_SensorOnSender", out strSensorInSender);

            string strSensorInFunc = "";
            CommonStaticMethod.GetLanguageString("多功能卡上的光探头：", "Lang_Bright_SensorOnFuncCard", out strSensorInFunc);

            string msg = "";
            if (periInfo.SensorType == PeripheralsType.LightSensorOnFuncCardInPort)
            {
                msg = strSensorInFunc + comName + "-" + strSender + (periInfo.SenderIndex + 1) + "-" +
                    strPort + (periInfo.PortIndex + 1) + "-" +
                    strFuncCard + (periInfo.FuncCardIndex + 1) + "-" +
                    strSensor + (periInfo.SensorIndex + 1);
            }
            else
            {
                msg = strSensorInSender + comName + "-" + strSender + (periInfo.SenderIndex + 1);
            }
            return msg;
        }

        private void OnFastSegmentation()
        {
            NotificationMessageAction<FastSegmentParam> nsa =
                new NotificationMessageAction<FastSegmentParam>(this, _fastSementParam, MsgToken.MSG_SHOWFASTSEGMENTATION, SetSegmentationNotifycationCallBack);
            Messenger.Default.Send(nsa, MsgToken.MSG_SHOWFASTSEGMENTATION);
        }

        private void SetSegmentationNotifycationCallBack(FastSegmentParam param)
        {
            if (param != null)
            {
                EnvirAndDisplayBrightCollection.Clear();

                List<DisplayAutoBrightMapping> mappingList;
                bool res = CustomTransform.FastSegment(param, out mappingList);
                if (res)
                {
                    foreach (DisplayAutoBrightMapping map in mappingList)
                    {
                        EnvirAndDisplayBrightCollection.Add(map);
                    }
                }
            }
        }

        private void UpdateUI(PeripheralsSettingParam param)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (param != null &&
                    param.ExtendData != null &&
                    param.ExtendData.AutoBrightMappingList != null)
                {
                    for (int i = 0; i < param.ExtendData.AutoBrightMappingList.Count; i++)
                    {
                        DisplayAutoBrightMapping map = param.ExtendData.AutoBrightMappingList[i];
                        EnvirAndDisplayBrightCollection.Add((DisplayAutoBrightMapping)map.Clone());
                    }
                }

                if (param != null &&
                    param.ExtendData != null &&
                    param.ExtendData.UseLightSensorList != null)
                {
                    for (int i = 0; i < param.ExtendData.UseLightSensorList.Count; i++)
                    {
                        PeripheralsLocation location = param.ExtendData.UseLightSensorList[i];
                        for (int j = 0; j < PeripheralList.Count; j++)
                        {
                            if (location.Equals(PeripheralList[j].Location))
                            {
                                PeripheralList[j].IsSelected = true;
                            }
                        }
                    }
                }

            }));
        }

        private void ShowValidError(string err)
        {
            ShowGlobalDialogMessage(err, MessageBoxImage.Error);
        }
    }
}
