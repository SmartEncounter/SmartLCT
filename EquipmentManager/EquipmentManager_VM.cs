using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using Nova.LCT.GigabitSystem.Common;
using GalaSoft.MvvmLight.Command;
using Nova.LCT.Message.Client;
using Nova.SmartLCT.Interface;
using System.Collections.Generic;
using Nova.LCT.GigabitSystem.HWConfigAccessor;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using System.Threading;
using Nova.Equipment.Protocol.TGProtocol;
using Nova.Message.Common;
using Nova.IO.Port;
using Nova.LCT.GigabitSystem.CommonInfoAccessor;
using System;

namespace Nova.SmartLCT.UI
{
    public class EquipmentManager_VM : SmartLCTViewModeBase
    {

        #region 属性
        public ObservableCollection<EquipmentTypeInfo> EquipmentTypeInfoList
        {
            get { return _equipmentTypeInfoList; }
            set
            {
                _equipmentTypeInfoList = value;
                RaisePropertyChanged(GetPropertyName(o => this.EquipmentTypeInfoList));
            }
        }
        private ObservableCollection<EquipmentTypeInfo> _equipmentTypeInfoList = new ObservableCollection<EquipmentTypeInfo>();

        public bool IsSendDataWindow
        {
            get { return _isSendDataWindow; }
            set
            {
                _isSendDataWindow = value;
                SetCheckBoxVisible();
                RaisePropertyChanged(GetPropertyName(o => this.IsSendDataWindow));
            }
        }
        private bool _isSendDataWindow = false;

        public List<SenderRedundancyInfo> SenderReduInfoList
        {
            get
            {
                return _senderReduInfoList;
            }
            set
            {
                _senderReduInfoList = value;

            }
        }
        private List<SenderRedundancyInfo> _senderReduInfoList = new List<SenderRedundancyInfo>();

        public List<ILEDDisplayInfo> OldDisplayList
        {
            get { return _oldDisplayList; }
            set
            {
                _oldDisplayList = value;

            }
        }
        private List<ILEDDisplayInfo> _oldDisplayList = new List<ILEDDisplayInfo>();


        #endregion

        #region 字段
        private EquipmentTypeInfo _equipmentTypeData = new EquipmentTypeInfo();
        private HardwareDistributionCaculator _ledDisplayCaculator = null;
        private string _selectedPortName = string.Empty;
        private bool _isSaveToHWSuccessful = false;
        private NSCardType _hwCardType=NSCardType.Unknown;
        private object _isReduOrScreenInfoCpMetuc = new object();
        private bool _isSendDataToHW = false;

        #endregion

        #region 类型
        internal class ScreenScanBdMapping
        {
            public int ScreenIndex;
            public ScanBoardRegionInfo ScanBdRegionInfo;
        }
        #endregion

        #region 命令
        public RelayCommand CmdSendDataToHW
        {
            get;
            private set;
        }
        public RelayCommand CmdSetDataToHW
        {
            get;
            private set;
        }
        public RelayCommand<System.ComponentModel.CancelEventArgs> CmdMainWindowClose
        {
            get;
            private set;
        }
        #endregion

        #region 构造函数
        public EquipmentManager_VM()
        {

            CmdSendDataToHW=new RelayCommand(SendDataToHW, OnSendBottonCanExecute);
            CmdSetDataToHW = new RelayCommand(SolidDataToHW, OnSolidDataCanExecute);
            CmdMainWindowClose = new RelayCommand<System.ComponentModel.CancelEventArgs>(MainWindowClose);
            if (this.IsInDesignMode)
            {
                IsSendDataWindow = true;
                EquipmentTypeInfoList.Clear();
                EquipmentTypeInfo data = new EquipmentTypeInfo();
                data.DeviceType = NSCardType.Controller;
                data.IsChecked = false;
                data.PortCount = 4;
                data.SerialPort = "COM 2";
                EquipmentTypeInfoList.Add(data);
                data = new EquipmentTypeInfo();
                data.DeviceType = NSCardType.HDMICard;
                data.IsChecked = false;
                data.PortCount = 4;
                data.SerialPort = "COM 4";
                EquipmentTypeInfoList.Add(data);
                data = new EquipmentTypeInfo();
                data.DeviceType = NSCardType.Scanner;
                data.IsChecked = false;
                data.PortCount = 2;
                data.SerialPort = "COM 6";
                EquipmentTypeInfoList.Add(data);
     
            }
            if (!this.IsInDesignMode)
            {
                Initialize();
                _oldDisplayList = _globalParams.CurrentDisplayInfoList;
                IsSendDataWindow = _globalParams.IsSendCurrentDisplayConfig;
                if (EquipmentTypeInfoList != null && EquipmentTypeInfoList.Count != 0)
                {
                    EquipmentTypeInfoList[0].IsChecked = true;
                }
            }
        }
        #endregion

        #region 方法
        private void SendDataToHW()
        {
            for (int i = 0; i < EquipmentTypeInfoList.Count; i++)
            {
                EquipmentTypeInfo info = EquipmentTypeInfoList[i];
                if (info.IsChecked)
                {
                    _equipmentTypeData = (EquipmentTypeInfo)info.Clone();
                    SendScreenInfoToHW();
                    break;
                }
            }
        }

        private bool OnSendBottonCanExecute()
        {
            if (_oldDisplayList == null ||
                _oldDisplayList.Count == 0)
            {
                return false;
            }
            if (EquipmentTypeInfoList.Count <= 0)
            {
                return false;
            }
            for (int i = 0; i < EquipmentTypeInfoList.Count; i++)
            {
                EquipmentTypeInfo info = EquipmentTypeInfoList[i];
                if (info.IsChecked)
                {
                    _equipmentTypeData = (EquipmentTypeInfo)info.Clone();
                    return true;
                }
            }
            return false;
        }

        private bool OnSolidDataCanExecute()
        {
            return OnSendBottonCanExecute();
        }

        private void SetCheckBoxVisible()
        {
            for (int i = 0; i < EquipmentTypeInfoList.Count; i++)
            {
                EquipmentTypeInfoList[i].CheckBoxVisible = IsSendDataWindow;
            }
        }

        private void SolidDataToHW()
        {
            //TODO:固化数据到硬件；
            for (int i = 0; i < EquipmentTypeInfoList.Count; i++)
            {
                EquipmentTypeInfo info = EquipmentTypeInfoList[i];
                if (info.IsChecked)
                {
                    _equipmentTypeData = (EquipmentTypeInfo)info.Clone();
                    _selectedPortName = info.SerialPort;
                    _hwCardType = info.DeviceType;
                    SaveToHardware();
                    break;
                }
            }
        }

        private void MainWindowClose(System.ComponentModel.CancelEventArgs e)
        {
            if (_isSendDataToHW)
            {
                e.Cancel = true;
                string msg = "数据没有固化到硬件，断电将丢失数据，是否保存数据到硬件？";
                GetLangString(msg, "Lang_ScreenInfo_SaveDataToHW", out msg);
                MessageBoxResult res=SendTextToMessageBox(msg, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    SolidDataToHW();

                    _isSendDataToHW = false;
                    e.Cancel = false;
                }
                else if(res==MessageBoxResult.Cancel)
                {
                    return;
                }
                else
                {
                    e.Cancel = false;
                }
            }
        }

        #region 固化数据


        public void SaveToHardware()
        {
            _isSaveToHWSuccessful = true;

            #region 冗余信息
            //if (UC_SendCardConfig.IsSentReduInfo)
            //{

            //    RedundancyInfoAccessor accessor = new RedundancyInfoAccessor(_serverProxy, _hwCardType, _selectedPortName);
            //    accessor.SaveReduInfoToHW(UC_SendCardConfig.SenderReduInfoList, new SaveCommonInfoCompeleteCallBack(OnSaveReduInfoComplete));
            //    OutPutDebugString("保存冗余信息！");
            //}
            //else
            //{
            //    _isReduInfoComplete = true;
            //    OutPutDebugString("没有发送冗余信息，跳过保存冗余！");
            //}
            #endregion

            #region 屏体信息

            ScreenInfoAccessor screenInfoAccessor = new ScreenInfoAccessor(_serverProxy, _hwCardType, _selectedPortName);
            screenInfoAccessor.SaveDviScreenInfoToHW(_graphicsDviInf, _oldDisplayList, OnSaveScreenInfoCompelete);
            
            #endregion

            
            string msg = string.Empty;
            msg = "正在保存硬件信息，请稍后.......";
            GetLangString(msg, "Lang_ScreenInfo_SaveDataProcessMsg", out msg);

            SendProgressMsg(msg);
        }


        private void OnSaveScreenInfoCompelete(object sender, CommonInfoCompeleteArgs args)
        {
            bool isSendParams = false;
            lock (_isReduOrScreenInfoCpMetuc)
            {
                if (args.Result != CommonInfoCompeleteResult.OK)
                {
                    _isSaveToHWSuccessful = false;
                    
                }
                
                isSendParams = true; 
            }
            if (isSendParams)
            {

                SendSaveParams(_selectedPortName);
            }
        }


        private void SendSaveParams(string selectedPortName)
        {
            string msg = string.Empty;
            if (!_isSaveToHWSuccessful)
            {
                
               msg = "保存信息到硬件失败!";
               GetLangString(msg, "Lang_ScreenInfo_SaveDataFailt", out msg);

               SendTextToMessageBox(msg, MessageBoxImage.Error);
               CloseProcessForm();

                return;
            }

            #region 参数保存

            SpecialPackageRequest spcPack = null;

            int savaStore2SpiFlashTimeOut = CommandTimeOut.SENDER_SAVEPARAMS_TIMEOUT;
            if (_serverProxy is LCTServerMessageProxy)
            {
                spcPack = new SpecialPackageRequest(SpecialPackID.SAVE_PARAMS_ID,
                     SpecialPackTimeOut.SAVE_PARAMS_TIMEOUT);
                savaStore2SpiFlashTimeOut = CommandTimeOut.SENDER_SAVEPARAMS_TIMEOUT;
            }
            else
            {
                spcPack = new SpecialPackageRequest(SpecialPackID.SAVE_PARAMS_ID,
                     (ushort)(SpecialPackTimeOut.SAVE_PARAMS_TIMEOUT + 10000));
                savaStore2SpiFlashTimeOut = CommandTimeOut.SENDER_SAVEPARAMS_TIMEOUT + 10000;
            }

            int curPackID;
            bool res = _serverProxy.SendStartRequestSpecialPacke(spcPack, out curPackID);
            if (!res)
            {
                
                msg = "服务正在处理其他请求，请稍后再试......";
                GetLangString(msg, "Lang_ScreenInfo_ServiceBusy", out msg);

                SendTextToMessageBox(msg,MessageBoxImage.Error);
                CloseProcessForm();

                return;
            }

            PackageRequestWriteData writeData;
            if (_serverProxy != null)
            {

                writeData = TGProtocolParser.SetSaveSendCardsParameters(selectedPortName,
                                                                        SystemAddress.SENDER_BROADCAST_ADDRESS,
                                                                        CommandTimeOut.SENDER_SAVEPARAMS_TIMEOUT,
                                                                        "SetSaveSendCardsParameters",
                                                                        false,
                                                                        0,
                                                                        null,
                                                                        CompleteRequestDealing);
                writeData.Reserved2 = SpecialPackID.SAVE_PARAMS_ID;
                _serverProxy.SendRequestWriteData(writeData);


                writeData = TGProtocolParser.SetParameterStore2SpiFlash(selectedPortName,
                                                                        SystemAddress.SENDER_BROADCAST_ADDRESS,
                                                                        SystemAddress.PORT_BROADCAST_ADDRESS,
                                                                        SystemAddress.SCANBOARD_BROADCAST_ADDRESS,
                                                                        savaStore2SpiFlashTimeOut,
                                                                        "SetParameterStore2SpiFlash",
                                                                        false,
                                                                        0,
                                                                        null,
                                                                        CompleteRequestDealing);
                writeData.Reserved2 = SpecialPackID.SAVE_PARAMS_ID;
                _serverProxy.SendRequestWriteData(writeData);
            }


            #endregion

        }

        private void CompleteRequestDealing(object sender, CompletePackageRequestEventArgs e)
        {
            PackageRequestWrite writeRequest = (PackageRequestWrite)e.Request;
            if (writeRequest != null)
            {
                string strTag = writeRequest.Tag;
                RequestComplete(writeRequest.PackResult, writeRequest.CommResult, (AckResults)writeRequest.AckCode, strTag, e.Request);
            }
        }

        private void RequestComplete(PackageResults packageRes, CommResults comRes, AckResults ackType, string strTag, PackageBase pack)
        {
            if (ackType != AckResults.ok
               || comRes != CommResults.ok
               || packageRes != PackageResults.ok)
            {
                if (strTag == "SetSaveSendCardsParameters")
                {
                    _isSaveToHWSuccessful = false;
                }
                if (strTag == "SetParameterStore2SpiFlash")
                {
                    string msg = "";
                    
                    msg = "保存信息到硬件失败!";
                    GetLangString(msg, "Lang_ScreenInfo_SaveDataFailt", out msg);

                    SendTextToMessageBox(msg, MessageBoxImage.Error);
                    CloseProcessForm();

                    return;
                }
                if (strTag == "SetReturnFactoryValues")
                {
                    string msg = "";
                    msg = "返回出厂设置失败！";
                    GetLangString(msg, "Lang_ScreenInfo_ReturnFactoryData", out msg);

                    SendTextToMessageBox(msg, MessageBoxImage.Error);
                    CloseProcessForm();

                    return;
                }
            }
            else
            {
                if (strTag == "SetParameterStore2SpiFlash")
                {
                    CustomTransform.Delay(SpecialPackTimeOut.SAVE_PARAMS_TIMEOUT * 1000, 60);
                    string msg = string.Empty;
                    msg = "保存信息到硬件成功!";
                    GetLangString(msg, "Lang_ScreenInfo_SendToHWSuccess", out msg);

                    _isSendDataToHW = false;

                    //if (_globalParams.AllBaseInfo.AllInfoDict.ContainsKey(pack.PortName))
                    //{
                    //    List<ILEDDisplayInfo> infoList = _globalParams.AllBaseInfo.AllInfoDict[pack.PortName].LEDDisplayInfoList;
                    //    infoList.Clear();
                    //    for (int i = 0; i < _oldDisplayList.Count; i++)
                    //    {
                    //        infoList.Add(_oldDisplayList[i]);
                    //    }
                    //}

                    SendTextToMessageBox(msg, MessageBoxImage.Exclamation);                    
                    CloseProcessForm();
                    Messenger.Default.Send<string>("", MsgToken.MSG_SCREENINFO_CHANGED);
                }
                else if (strTag == "SetReturnFactoryValues")
                {
                    SpecialPackageRequest spcPack = new SpecialPackageRequest(SpecialPackID.RETURN_FACTORY_ID,
                        SpecialPackTimeOut.RETURN_FACTORY);
                    int curID;
                    _serverProxy.SendEndRequestSpecialPacke(spcPack, out curID);

                    RedundancyInfoAccessor reduInfoAccessor = new RedundancyInfoAccessor(_serverProxy, _hwCardType, _selectedPortName);
                    HWSoftwareSpaceRes res = reduInfoAccessor.ClearHWReduInfo(OnClearReduInfoComplete);
                    if (res != HWSoftwareSpaceRes.OK)
                    {
                        string msg = "";
                        msg = "返回出厂设置失败！";
                        GetLangString(msg, "Lang_ScreenInfo_ReturnFactoryData", out msg);

                        SendTextToMessageBox(msg,MessageBoxImage.Error);                                            
                        CloseProcessForm();
                        return;
                    }
                }
            }
        }

        
        private void OnClearReduInfoComplete(object sender, CommonInfoCompeleteArgs args)
        {
            if (args.Result != CommonInfoCompeleteResult.OK)
            {
                string msg = "";
                msg = "返回出厂设置失败！";
                GetLangString(msg, "Lang_ScreenInfo_ReturnFactoryData", out msg);
                
                SendTextToMessageBox(msg, MessageBoxImage.Error);
                CloseProcessForm();

                return;
            }
            else
            {
                ScreenInfoAccessor accessor = new ScreenInfoAccessor(_serverProxy, _hwCardType, _selectedPortName);
                HWSoftwareSpaceRes res = accessor.ClearHWScreenInfo(OnClearScreenInfoComplete);
                if (res != HWSoftwareSpaceRes.OK)
                {
                    string msg = "";
                    msg = "返回出厂设置失败！";
                    GetLangString(msg, "Lang_ScreenInfo_ReturnFactoryData", out msg);

                    SendTextToMessageBox(msg, MessageBoxImage.Error);                                            
                    CloseProcessForm();
                    return;
                }
            }
        }

        private void OnClearScreenInfoComplete(object sender, CommonInfoCompeleteArgs args)
        {
            if (args.Result != CommonInfoCompeleteResult.OK)
            {
                
                string msg = "";
                msg = "返回出厂设置失败！";
                GetLangString(msg, "Lang_ScreenInfo_ReturnFactoryData", out msg);

                SendTextToMessageBox(msg, MessageBoxImage.Error);
                CloseProcessForm();
                return;
            }
            else
            {
               
                string msg = "";
                
                msg = "返回出厂设置成功,请重新配置显示屏和冗余信息!";
                GetLangString(msg, "Lang_ScreenInfo_ReturnFactoryDataSuccess", out msg);

                SendTextToMessageBox(msg, MessageBoxImage.Error);
                CloseProcessForm();
                return;
            }
        }

        #endregion

        private void SendTextToMessageBox(string msg, MessageBoxImage msgImage)
        {
            ShowGlobalDialogMessage(msg, msgImage);
        }

        private MessageBoxResult SendTextToMessageBox(string msg, MessageBoxButton button, MessageBoxImage msgImage)
        {
            return ShowQuestionMessage(msg, button, msgImage);
        }

        private void Initialize()
        {
            EquipmentTypeInfoList.Clear();
            EquipmentTypeInfo info = null;
            foreach (string commPort in _serverProxy.EquipmentTable.Keys)
            {
                info = new EquipmentTypeInfo();
                info.IsCheckedEvent += new IsCheckedChangedDel(OnIsCheckedChanged);
                info.SerialPort = commPort;

                int moduleID = _serverProxy.EquipmentTable[commPort].ModuleID;
                info.DeviceType = CustomTransform.ModelIdToNSCarType(moduleID);
                info.CheckBoxEnable = CustomTransform.IsSystemController(moduleID);
                info.PortCount = CustomTransform.GetPortNumber(moduleID);
                info.SystermCount = _serverProxy.EquipmentTable[commPort].EquipTypeCount;

                info.CheckBoxVisible = true;
                if (!IsSendDataWindow)
                {
                    info.CheckBoxVisible = false;
                }

                EquipmentTypeInfoList.Add(info);
            }
        }

        private void SendScreenInfoToHW()
        {
            string selectedPortName = _equipmentTypeData.SerialPort;
            _selectedPortName = selectedPortName;
            if (_senderProp != null)
            {
                if (_serverProxy.EquipmentTable.ContainsKey(selectedPortName))
                {
                    int modalID = _serverProxy.EquipmentTable[selectedPortName].ModuleID;
                    string errorMsg = "";
                    NSCardType cardType = CustomTransform.ModelIdToNSCarType(modalID);
                    int portCount = CustomTransform.GetPortNumber(cardType);
                    if (cardType == _equipmentTypeData.DeviceType && portCount == _equipmentTypeData.PortCount)
                    {
                        if (!CheckReduSenderExist(selectedPortName, _oldDisplayList, _senderReduInfoList, out errorMsg))
                        {
                            SendTextToMessageBox(errorMsg, MessageBoxImage.Error);

                            return;
                        }
                        ThreadPool.QueueUserWorkItem(new WaitCallback(SendScreenIfoCallBack), null);

                        return;
                    }
                }
            }
            string msg = "发送卡参数不匹配！";
            GetLangString(msg, "Lang_ScreenInfo_SenderCardParameterErr", out msg);

            SendTextToMessageBox(msg, MessageBoxImage.Error);
            //TODO:发送失败
        }

        private void SendScreenIfoCallBack(object state)
        {
            if (!CheckDataValue(_selectedPortName))
            {
                string msg = string.Empty;
                msg = "发送屏体信息失败！";
                GetLangString(msg, "Lang_ScreenInfo_SendScreenDataFailt", out msg);

                CloseProcessForm();
                SendTextToMessageBox(msg,MessageBoxImage.Error);
            }
        }

        protected virtual void CloseProcessForm()
        {
            ShowGlobalProcessEnd();
        }

        private bool CheckDataValue(string selectedPortName)
        {
            string errorMsg;

            if (_ledDisplayCaculator == null)
            {
                _ledDisplayCaculator = new HardwareDistributionCaculator(_graphicsDviInf);
            }
            List<SenderDVIRegionInfo> dviRegionList;
            List<SenderPortRegionInfo> portRegionList;
            List<ScanBoardRegionInfo> scanBdRegionList;
            List<SenderDVIRegionInfo> realDviRegionList;
            _ledDisplayCaculator.GetSenderAllRegionInfo(_oldDisplayList, out dviRegionList, out realDviRegionList,
                out portRegionList, out scanBdRegionList);

            if (scanBdRegionList == null || scanBdRegionList.Count == 0)
            {
                errorMsg =  "至少需要填写一张接收卡的信息！";
                GetLangString(errorMsg, "Lang_ScreenInfo_OneReceiveCardInfo", out errorMsg);

                SendTextToMessageBox(errorMsg,MessageBoxImage.Error);

                return false;
            }
            for (int i = 0; i < scanBdRegionList.Count; i++)
            {
                if ((!_serverProxy.EquipmentTable.ContainsKey(selectedPortName)) || scanBdRegionList[i].SenderIndex > _serverProxy.EquipmentTable[selectedPortName].EquipTypeCount - 1)
                {
                    errorMsg = "发送卡没有连接或发送卡故障：发送卡序号为 ";
                    GetLangString(errorMsg, "Lang_ScreenInfo_NoReceiveCard", out errorMsg);

                    errorMsg += scanBdRegionList[i].SenderIndex + 1;
                    SendTextToMessageBox(errorMsg,MessageBoxImage.Error);
                    return false;
                }
            }
            if (!CheckScreenInfoValidate(_oldDisplayList, realDviRegionList, _ledDisplayCaculator.LastScreenScanBdRegion, out errorMsg))
            {
                SendTextToMessageBox(errorMsg,MessageBoxImage.Error);

                return false;
            }
            if (realDviRegionList == null || realDviRegionList.Count == 0)
            {
                errorMsg = "发送卡的映射区域不在当前显示屏的范围，请确认是否继续发送？";
                GetLangString(errorMsg, "Lang_ScreenInfo_RegionNoExist", out errorMsg);

                errorMsg += "\r\n";
                string realPixel = string.Empty;
                realPixel = "当前显示器分辨率为：";
                GetLangString(realPixel, "Lang_ScreenInfo_DisplayResolution", out realPixel);

                realPixel = "(" + realPixel + _graphicsDviInf.GraphicsWidth + "px × " + _graphicsDviInf.GraphicsHeight + "px)";
                errorMsg += realPixel;
                SendTextToMessageBox(errorMsg,MessageBoxImage.Error);

                dviRegionList = realDviRegionList;
            }
            else
            {
                SendScreenData();                
            }
            return true;
        }

        private bool CheckReduSenderExist(string selectedPortName,List<ILEDDisplayInfo> ledDisplayInfoList, List<SenderRedundancyInfo> senderReduInfoList, out string errMsg)
        {
            errMsg = "";
            int senderCount = 0;
            lock (_serverProxy.EquimentObject)
            {
                if (_serverProxy.EquipmentTable.ContainsKey(selectedPortName))
                {
                    int moduleID = _serverProxy.EquipmentTable[selectedPortName].ModuleID;
                    NSCardType cardType = CustomTransform.ModelIdToNSCarType(moduleID);
                    if (CustomTransform.IsSystemController(moduleID))
                    {
                        senderCount = _serverProxy.EquipmentTable[selectedPortName].EquipTypeCount;
                    }

                }
            }

            for (int i = 0; i < ledDisplayInfoList.Count; i++)
            {
                List<ScreenPortAddrInfo> screenPortAddrList;
                ledDisplayInfoList[i].GetScreenPortAddrInfo(out screenPortAddrList);
                for (int j = 0; j < screenPortAddrList.Count; j++)
                {
                    byte slaveSender;
                    if (IsReduContainSender(screenPortAddrList[j].SenderIndex, senderReduInfoList, out slaveSender))
                    {
                        if (slaveSender >= senderCount)
                        {
                            string msg = "";
                            string strScreen, strReduNotExist, strReduSender;
                            strScreen = "屏";
                            GetLangString(strScreen, "Lang_ScreenInfo_Screen", out strScreen);
                            
                            
                            strReduNotExist = "冗余设备未连接，请连接冗余设备或者删除该显示屏的冗余信息！";
                            GetLangString(strReduNotExist, "Lang_ScreenInfo_StrReduNotExist", out strReduNotExist);
                            
                            strReduSender = "冗余发送卡序号为：";
                            GetLangString(strReduSender, "Lang_ScreenInfo_StrReduSender", out strReduSender);
                            

                            msg = strScreen + (i + 1) + ": " + strReduNotExist + "\r\n";
                            msg = msg + "(" + strReduSender + (slaveSender + 1) + ")";
                            errMsg = msg;
                            return false;
                        }
                    }

                }
            }

            return true;

        }

        public bool IsReduContainSender(byte masterSender,List<SenderRedundancyInfo> reduInfoList, out byte slaveSender)
        {
            slaveSender = 0;
            SenderRedundancyInfo info = reduInfoList.Find(
                delegate(SenderRedundancyInfo temp)
                {
                    if (temp.MasterSenderIndex == masterSender)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            if (info != null)
            {
                slaveSender = info.SlaveSenderIndex;
                return true;
            }
            else
            {
                return false;
            }
        }
        

        private void SendScreenData()
        {
            string msg = "正在发送数据.....";
            GetLangString(msg, "Lang_ScreenInfo_StrSendDataNow", out msg);

            ScreenCofigAccessor accessor = new ScreenCofigAccessor(_serverProxy,
                                 _senderProp, _graphicsDviInf, _equipmentTypeData.SerialPort, SendScreenInfoCallBack);
            accessor.SendScreenInfoToHW(_oldDisplayList, _senderReduInfoList);
            SendProgressMsg(msg);
        }

        private void SendProgressMsg(string Testss)
        {
            ShowGlobalProcessBegin(Testss);
        }

        private bool CheckScreenInfoValidate(List<ILEDDisplayInfo> screenList, List<SenderDVIRegionInfo> realDviRegionList, Dictionary<int, List<ScanBoardRegionInfo>> screenScanBdRegionDict, out string ErrorMessage)
        {
            ErrorMessage = string.Empty;

            #region 检测接收卡是否没有连线
            for (int i = 0; i < screenList.Count; i++ )
            {
                ILEDDisplayInfo screen = screenList[i];
                int scannerCount = screen.ScannerCount;
                for (int j = 0; j < scannerCount; j++ )
                {
                    ScanBoardRegionInfo regionInfo = screen[j];
                    if (regionInfo.SenderIndex != ConstValue.BLANK_SCANNER &&
                        regionInfo.ConnectIndex == 0xffff)
                    {
                        string msg = "显示屏";
                        GetLangString(msg, "Lang_Global_Screen", out msg);

                        ErrorMessage = msg + (i + 1);

                        msg = "存在没有连线的接收卡";
                        GetLangString(msg, "Lang_Global_NotLink", out msg);
                        ErrorMessage = msg ;
                        return false;
                    }
                }
            }
            #endregion

            #region 检查是否超过了一根网线带载范围，是否超过了允许带载的总数
            int totalScannerCount = 0;
            for (int i = 0; i < screenList.Count; i++)
            {
                ILEDDisplayInfo displayInfo = screenList[i];
                List<ScreenPortAddrInfo> screenPortAddrInfoList;
                displayInfo.GetScreenPortAddrInfo(out screenPortAddrInfoList);

                for (int j = 0; j < screenPortAddrInfoList.Count; j++)
                {
                    totalScannerCount += screenPortAddrInfoList[j].LoadScannerCount;

                    if (screenPortAddrInfoList[j].LoadScannerCount > 1024)
                    {
                        string error = "";
                        if (totalScannerCount > 20480) //软件空间允许的最大接收卡数
                        {

                            
                            error = "一个串口下能带载的总接收卡数不能大于20480！";
                            GetLangString(error, "Lang_ScreenInfo_OnePortLoadQuantity", out error);

                            ErrorMessage = error;
                            return false;
                        }

                        string screenName = string.Empty;
                        screenName = "屏";
                        GetLangString(error, "Lang_ScreenInfo_Screen", out error);

                        ErrorMessage = ErrorMessage + screenName + (i + 1) + ":";
                        error = "";
                        error = "存在带载接收卡数大于1024的网口！";
                        GetLangString(error, "Lang_ScreenInfo_PortLoadQuantityLarge", out error);

                        ErrorMessage += error;
                        screenName = "发送卡序号";
                        GetLangString(error, "Lang_ScreenInfo_SendCardNumber", out error);

                        ErrorMessage = ErrorMessage + "(" + screenName + ":" + (screenPortAddrInfoList[j].SenderIndex + 1).ToString() + ",";

                        screenName = "网口序号";
                        GetLangString(error, "Lang_ScreenInfo_PortNumber", out error);

                        ErrorMessage = ErrorMessage + screenName + ":" + (screenPortAddrInfoList[j].PortIndex + 1).ToString() + ")";
                        return false;
                    }
                }
            }

            #endregion

            List<ScreenScanBdMapping> screenScanBdMapping = new List<ScreenScanBdMapping>();
            List<ScreenPortAddrInfo> allScreenPortInfoList = new List<ScreenPortAddrInfo>();

            #region 首先检查屏体自己是否有重复使用的物理地址

            string msgExtern;
            foreach (int key in screenScanBdRegionDict.Keys)
            {
                //获得网口的信息，设置每个网口的初始序号
                List<ScreenPortAddrInfo> screenPortInfoList;
                screenList[key].GetScreenPortAddrInfo(out screenPortInfoList);
                //把每个显示屏的网口信息加入到列表中，如果列表中已经存在该网口的信息，则增加该网口的带载接收卡的个数
                for (int i = 0; i < screenPortInfoList.Count; i++)
                {
                    ScreenPortAddrInfo find = allScreenPortInfoList.Find(delegate(ScreenPortAddrInfo info)
                    {
                        return info.SenderIndex == screenPortInfoList[i].SenderIndex &&
                            info.PortIndex == screenPortInfoList[i].PortIndex;
                    });
                    if (find != null)
                    {
                        find.LoadScannerCount += screenPortInfoList[i].LoadScannerCount;
                    }
                    else
                    {
                        allScreenPortInfoList.Add((ScreenPortAddrInfo)screenPortInfoList[i].Clone());
                    }
                }

                List<int> portSBIndex = new List<int>();
                for (int i = 0; i < screenPortInfoList.Count; i++)
                {
                    portSBIndex.Add(0);
                }
                for (int i = 0; i < screenScanBdRegionDict[key].Count; i++)
                {
                    int count = 0;
                    ScanBoardRegionInfo temp = screenScanBdRegionDict[key][i];
                    List<ScanBoardRegionInfo> findInfo = screenScanBdRegionDict[key].FindAll(delegate(ScanBoardRegionInfo info)
                    {
                        count++;
                        return ((temp.SenderIndex == info.SenderIndex) &&
                               (temp.PortIndex == info.PortIndex) &&
                               (temp.ConnectIndex == info.ConnectIndex));
                    });


                    if (findInfo.Count > 1)
                    {
                        #region 构造输出字符串


                        msgExtern = "屏";
                        GetLangString(msgExtern, "Lang_ScreenInfo_Screen", out msgExtern);

                        ErrorMessage = msgExtern;

                        msgExtern = "存在重复物理地址的接收卡!";
                        GetLangString(msgExtern, "Lang_ScreenInfo_PhysicalAddressesRepeat", out msgExtern);

                        ErrorMessage = ErrorMessage + (key + 1) + msgExtern + "(";
                         msgExtern = "重复地址为：";
                         GetLangString(msgExtern, "Lang_ScreenInfo_RepeatAddresses", out msgExtern);

                        ErrorMessage += msgExtern;
                        msgExtern = "发送卡序号";
                        GetLangString(msgExtern, "Lang_ScreenInfo_SendCardNumber", out msgExtern);

                        ErrorMessage = ErrorMessage + msgExtern + ":" + (findInfo[0].SenderIndex + 1).ToString() + "，";

                         msgExtern = "网口序号";
                         GetLangString(msgExtern, "Lang_ScreenInfo_PortNumber", out msgExtern);

                        ErrorMessage = ErrorMessage + msgExtern + ":" + (findInfo[0].PortIndex + 1).ToString() + "，";

                        msgExtern = "接收卡序号";
                        GetLangString(msgExtern, "Lang_ScreenInfo_RecieveCardNumber", out msgExtern);
                        
                        ErrorMessage = ErrorMessage + msgExtern + ":" + (findInfo[0].ConnectIndex + 1).ToString() + ")";

                        #endregion
                        return false;
                    }

                    
                    ScreenScanBdMapping map = new ScreenScanBdMapping();
                    map.ScanBdRegionInfo = temp;
                    map.ScreenIndex = key;
                    screenScanBdMapping.Add(map);

                }


            }

            #endregion



            #region 检查屏体之间是否有重复的物理地址


            for (int i = 0; i < screenScanBdMapping.Count; i++)
            {

                List<ScreenScanBdMapping> findInfo = screenScanBdMapping.FindAll(delegate(ScreenScanBdMapping map)
                {
                    return (map.ScanBdRegionInfo.SenderIndex == screenScanBdMapping[i].ScanBdRegionInfo.SenderIndex &&
                        map.ScanBdRegionInfo.PortIndex == screenScanBdMapping[i].ScanBdRegionInfo.PortIndex &&
                        map.ScanBdRegionInfo.ConnectIndex == screenScanBdMapping[i].ScanBdRegionInfo.ConnectIndex);
                });
                if (findInfo.Count > 1)
                {
                    string screenName = string.Empty;
                    screenName = "屏";
                    GetLangString(screenName, "Lang_ScreenInfo_Screen", out screenName);

                    for (int j = 0; j < findInfo.Count; j++)
                    {
                        screenName = screenName + (findInfo[j].ScreenIndex + 1);
                        if (j < findInfo.Count - 1)
                        {
                            screenName += "、";
                        }
                    }
                    ErrorMessage = screenName;
                    screenName = "存在重复物理地址的接收卡!";
                    GetLangString(screenName, "Lang_ScreenInfo_PhysicalAddressesRepeat", out screenName);
                    
                    ErrorMessage = ErrorMessage + screenName + "(";
                    screenName = "重复地址为：";
                    GetLangString(screenName, "Lang_ScreenInfo_RepeatAddresses", out screenName);
                    
                    ErrorMessage = ErrorMessage + screenName;
                    screenName = "发送卡序号";
                    GetLangString(screenName, "Lang_ScreenInfo_SendCardNumber", out screenName);
                    
                    ErrorMessage = ErrorMessage + screenName + ":" + (findInfo[0].ScanBdRegionInfo.SenderIndex + 1).ToString() + ",";

                    screenName = "网口序号";
                    GetLangString(screenName, "Lang_ScreenInfo_PortNumber", out screenName);
                    
                    ErrorMessage = ErrorMessage + screenName + ":" + (findInfo[0].ScanBdRegionInfo.PortIndex + 1).ToString() + ",";
                    screenName = "接收卡序号";
                    GetLangString(screenName, "Lang_ScreenInfo_RecieveCardNumber", out screenName);
                    
                    ErrorMessage = ErrorMessage + screenName + ":" + (findInfo[0].ScanBdRegionInfo.ConnectIndex + 1).ToString() + ")";


                    return false;
                }
            }
            #endregion

            #region 发送卡网口的接收卡序号连续及从1开始
            for (int i = 0; i < allScreenPortInfoList.Count; i++)//所有网口列表
            {
                bool findFirst = false;
                bool continuous = true;
                int unContinuousIndex = 0;
                int portLoadScannerCount = allScreenPortInfoList[i].LoadScannerCount;
                for (int j = 0; j < portLoadScannerCount; j++)//网口下的带载接收卡数
                {
                    bool isFind = false;
                    for (int k = 0; k < screenList.Count; k++)//显示屏个数
                    {
                        int scannerCount = screenList[k].ScannerCount;
                        for (int m = 0; m < scannerCount; m++)//显示屏接收卡数
                        {
                            ScanBoardRegionInfo info = screenList[k][m];
                            if (info.SenderIndex == allScreenPortInfoList[i].SenderIndex &&
                                info.PortIndex == allScreenPortInfoList[i].PortIndex &&
                                info.ConnectIndex == j)
                            {
                                if (j == 0)
                                {
                                    findFirst = true;
                                }
                                isFind = true;
                                break;
                            }
                        }
                    }
                    if (!isFind)
                    {
                        continuous = false;
                        unContinuousIndex = j;
                        break;
                    }
                }

                if (!findFirst)
                {
                    msgExtern = "发送卡每个网口下的接收卡序号必须从1开始！";
                    GetLangString(msgExtern, "Lang_ScreenInfo_MsgExtern", out msgExtern);
                    
                    string strSenderNum = string.Empty;
                    strSenderNum = "发送卡序号";
                    GetLangString(strSenderNum, "Lang_ScreenInfo_SendCardNumber", out strSenderNum);
                    
                    string strPortNum = string.Empty;
                    strPortNum = "网口序号";
                    GetLangString(strPortNum, "Lang_ScreenInfo_PortNumber", out strPortNum);
                    
                    ErrorMessage = msgExtern + "(" +
                        strSenderNum + (allScreenPortInfoList[i].SenderIndex + 1) + "," +
                        strPortNum + (allScreenPortInfoList[i].PortIndex + 1) + ")";
                    return false;
                }
                if (!continuous)
                {
                    msgExtern = "接收卡的序号必须是连续的!";
                    GetLangString(msgExtern, "Lang_ScreenInfo_MsgExternContinuous", out msgExtern);
                    
                    ErrorMessage = ErrorMessage + msgExtern + "(";

                    msgExtern = "错误：接收卡不存在,序号为:";
                    GetLangString(msgExtern, "Lang_ScreenInfo_MsgExternErr", out msgExtern);
                    
                    string strSenderNum = string.Empty;
                    strSenderNum = "发送卡序号";
                    GetLangString(strSenderNum, "Lang_ScreenInfo_SendCardNumber", out strSenderNum);
                    
                    string strPortNum = string.Empty;
                    strPortNum = "网口序号";
                    GetLangString(strPortNum, "Lang_ScreenInfo_PortNumber", out strPortNum);
                    
                    ErrorMessage = ErrorMessage + msgExtern + (unContinuousIndex + 1) + "," +
                        strSenderNum + ":" + (allScreenPortInfoList[i].SenderIndex + 1) + "," +
                        strPortNum + ":" + (allScreenPortInfoList[i].PortIndex + 1) + ")";
                    return false;
                }
            }
            #endregion

            #region 判断是否有发送卡跨越了DVI头
            bool isOverDvi = false;
            int senderIndex = -1;
            for (int i = 0; i < realDviRegionList.Count; i++)
            {
                List<SenderDVIRegionInfo> tempList = realDviRegionList.FindAll(delegate(SenderDVIRegionInfo info)
                {

                    if (info.SenderIndex == realDviRegionList[i].SenderIndex)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
                if (tempList.Count > 1)
                {
                    isOverDvi = true;
                    senderIndex = realDviRegionList[i].SenderIndex;
                    break;
                }
            }
            if (isOverDvi)
            {
                string msg = "";
                ErrorMessage = "发送卡不能跨域了两个以上的DVI头显示区域！发送卡序号：";
                GetLangString(ErrorMessage, "Lang_ScreenInfo_SenderCardStride", out ErrorMessage);
                
                ErrorMessage += (senderIndex + 1).ToString() + "\r\n";
                msg = "当前设置的DVI头的分辨率为：";
                GetLangString(msg, "Lang_ScreenInfo_ScreenResolution", out msg);
                
                ErrorMessage += "(" + msg + _graphicsDviInf.GraphicsWidth + " x " + _graphicsDviInf.GraphicsHeight + " px)";
                return false;
            }
            #endregion

            #region 判断每个屏是否有接收卡跨域DIV头
            string screenText = string.Empty;
            string scannerText = string.Empty;
            string errorText = string.Empty;
            for (int i = 0; i < screenList.Count; i++)
            {
                int scannerCount = screenList[i].ScannerCount;
                for (int j = 0; j < scannerCount; j++)
                {
                    ScanBoardRegionInfo temp = screenList[i][j];
                    if (temp.SenderIndex != ConstValue.BLANK_SCANNER)
                    {
                        //求其所在那个DVI头的Rectangle
                        int dviX = temp.X / _graphicsDviInf.GraphicsWidth * _graphicsDviInf.GraphicsWidth;
                        int dviY = temp.Y / _graphicsDviInf.GraphicsHeight * _graphicsDviInf.GraphicsHeight;
                        int dviWidth = _graphicsDviInf.GraphicsWidth;
                        int dviHeight = _graphicsDviInf.GraphicsHeight;
                        Rectangle dviRect = new Rectangle(dviX, dviY, dviWidth, dviHeight);
                        //接收卡所占区域
                        Rectangle scanRect = new Rectangle(temp.X, temp.Y, temp.Width, temp.Height);
                        //如果接收卡跨越了DVI头
                        if (!dviRect.Contains(scanRect))
                        {
                            string msg = "";
                            screenText = "屏";
                            GetLangString(screenText, "Lang_ScreenInfo_Screen", out screenText);
                            
                            scannerText = "接收卡";
                            GetLangString(scannerText, "Lang_ScreenInfo_ScannerText", out scannerText);
                            
                           errorText = "接收卡所在的区域跨越了两个以上的DVI头显示区域！";
                           GetLangString(errorText, "Lang_ScreenInfo_SenderCardStride", out errorText);
                            
                            ErrorMessage = errorText + "(" + screenText + ":" + (i + 1) + "," +
                                scannerText + "(" + (temp.SenderIndex + 1) + "," +
                                (temp.PortIndex + 1) + "," + (temp.ConnectIndex + 1) + "))\r\n";

                            msg = "当前设置的DVI头的分辨率为：";
                            GetLangString(msg, "Lang_ScreenInfo_ScreenResolution", out msg);
                            
                            ErrorMessage += "(" + msg + _graphicsDviInf.GraphicsWidth + " x " + _graphicsDviInf.GraphicsHeight + " px)";
                            return false;
                        }
                        else
                        {
                        }
                    }
                }
            }
            #endregion

            return true;

        }

        private void SendScreenInfoCallBack(WriteConfigRes res)
        {
            string errorMsg = "";
            if (res != WriteConfigRes.Succeed)
            {
                errorMsg = "发送失败！";
                GetLangString(errorMsg, "Lang_ScreenInfo_SendFailt", out errorMsg);

            }
            else
            {
                _isSendDataToHW = true;

                errorMsg = "发送成功！";
                GetLangString(errorMsg, "Lang_ScreenInfo_SendSuccessed", out errorMsg);
            }
            CloseProcessForm();

            SendTextToMessageBox(errorMsg,MessageBoxImage.Asterisk);

        }

        private void OnSendScreenInfoCallBack(string str)
        {
            
        }
        private void OnIsCheckedChanged(string commport,bool isChecked)
        {
            for (int i = 0; i < EquipmentTypeInfoList.Count; i++)
            {
                if (EquipmentTypeInfoList[i].SerialPort != commport)
                {
                    if (EquipmentTypeInfoList[i].IsChecked)
                    {
                        EquipmentTypeInfoList[i].IsChecked = false;
                    }
                }
                else
                {
                    EquipmentTypeInfoList[i].IsChecked = isChecked;
                }
            }
        }

        private void GetLangString(string defaultStr,string key,out string msg)
        {
            CommonStaticMethod.GetLanguageString(defaultStr, key, out msg);
        }

        //TODO:重写设备变更
        protected override void OnEquipmentChangeEvent(object sender, EventArgs e)
        {
            Initialize();
        }
        #endregion

    }
}