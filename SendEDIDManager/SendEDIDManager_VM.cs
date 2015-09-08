using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using Nova.LCT.GigabitSystem.HWConfigAccessor;
using Nova.LCT.Message.Client;
using System.Collections.Generic;
using Nova.LCT.GigabitSystem.Common;
using Nova.SmartLCT.Interface;
using System.Windows;

namespace Nova.SmartLCT.UI
{
   
    public class SendEDIDManager_VM : SmartLCTViewModeBase
    {

        #region 界面属性
        public ObservableCollection<DataGradViewItem> SenderDisplayInfoList
        {
            get
            {
                return _senderDisplayInfoList;
            }
            set
            {
                _senderDisplayInfoList = value;
                //SetCheckedValue();

                RaisePropertyChanged("SenderDisplayInfoList");
            }
        }
        private ObservableCollection<DataGradViewItem> _senderDisplayInfoList = new ObservableCollection<DataGradViewItem>();


        private double _RefreshRate = 50;
        public double RefreshRate
        {
            get
            {
                return this._RefreshRate;
            }

            set
            {
                if (this._RefreshRate != value)
                {
                    this._RefreshRate = value;
                    RaisePropertyChanged("RefreshRate");
                }
            }
        }

        private int _height = 768;
        public int Height
        {
            get
            {
                return this._height;
            }

            set
            {
                if (this._height != value)
                {
                    this._height = value;
                    RaisePropertyChanged("Height");
                }
            }
        }

        private int _width = 1024;
        public int Width
        {
            get
            {
                return this._width;
            }

            set
            {
                if (this._width != value)
                {
                    this._width = value;
                    RaisePropertyChanged("Width");
                }
            }
        }
        #endregion

        #region  字段
        private Dictionary<string,List<SenderDisplayInfo>> _sendDataList=new Dictionary<string,List<SenderDisplayInfo>>();

        private EDIDAccessor _edidAccessor = null;
        private string _commPort = string.Empty;

        /// <summary>
        /// 水平消隐点数
        /// </summary>
        private int _hBlankPix = 160;
        /// <summary>
        /// 垂直消隐点数
        /// </summary>
        private int _vBlankPix = 35;
        private const int MAXBANDWIDTH = 163;//M

        private bool _isThreeStateChanged = false;
        private bool _isCheckCahnged = false;
        #endregion

        #region  命令

        public RelayCommand CmdRefashSelectedType
        {
            get;
            private set;
        }
        public RelayCommand CmdSetSelectedType
        {
            get;
            private set;
        }
        #endregion

        #region 构造函数
        public SendEDIDManager_VM()
        {
            CmdRefashSelectedType = new RelayCommand(OnCmdRefreshSelectedType, CanCmdRefreshSelectedType);
            CmdSetSelectedType = new RelayCommand(OnCmdSetSelectedType, CanCmdSetSelectedType);

            if (this.IsInDesignMode)
            {
                DataGradViewItem data = new DataGradViewItem();
                data.SenderIndex = 0;
                data.SerialPort = "COM1";
                data.RefreshRate = 60;
                data.Reslution="1024*768";
                data.IsChecked = false;
                SenderDisplayInfoList.Add(data);
                data = new DataGradViewItem();
                data.SenderIndex = 1;
                data.SerialPort = "COM1";
                data.RefreshRate = 60;
                data.Reslution = "1024*768";
                data.IsChecked = false;
                SenderDisplayInfoList.Add(data);

                data = new DataGradViewItem();
                data.SenderIndex = 2;
                data.SerialPort = "COM2";
                data.RefreshRate = 60;
                data.Reslution = "1024*768";
                data.IsChecked = false;
                SenderDisplayInfoList.Add(data);
                data = new DataGradViewItem();
                data.SenderIndex = 3;
                data.SerialPort = "COM2";
                data.RefreshRate = 60;
                data.Reslution = "1024*768";
                data.IsChecked = false;
                SenderDisplayInfoList.Add(data);

            }
            else
            {
                Initialize();
            }
        }
        #endregion

        #region 方法

        #region 命令处理
        private void OnCmdRefreshSelectedType()
        {
            GetSendData();
            if (_sendDataList.Count < 0)
            {
                return;
            }
            ReadOneCardData();
        }
        private bool CanCmdRefreshSelectedType()
        {
            if (SenderDisplayInfoList == null ||
                SenderDisplayInfoList.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < SenderDisplayInfoList.Count; i++ )
            {
                if (SenderDisplayInfoList[i].IsChecked)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnCmdSetSelectedType()
        {
            string msg = "";
            long pixClk = (long)(RefreshRate * ((Height + _hBlankPix) * (Width + _vBlankPix)) / 10000);
            if (pixClk < 0 || pixClk / 100 >= MAXBANDWIDTH)
            {
                 msg = "发送卡不支持当前分辨率";
                 CommonStaticMethod.GetLanguageString(msg, "Lang_Gloabal_ResolutionNotSupport", out msg);

                 ShowGlobalDialogMessage(msg, MessageBoxImage.Error);
                return;
            }

            GetSendData();
            if (_sendDataList.Count <= 0)
            {
                return;
            }
            foreach (string commport in _sendDataList.Keys)
            {
                InitializeData(commport);
                _commPort = commport;
                List<SenderDisplayInfo> list = _sendDataList[commport];
                if (!_edidAccessor.SetMarsDisplayEDID(_sendDataList[commport]))
                {
                     msg = "设置";
                    CommonStaticMethod.GetLanguageString(msg, "Lang_Gloabal_Set", out msg);
                    string err = "分辨率失败！";
                    CommonStaticMethod.GetLanguageString(err, "Lang_Gloabal_SetError", out err);
                    msg += commport + err;
                    ShowGlobalDialogMessage(msg, MessageBoxImage.Error);
                    continue;
                }
                _sendDataList.Remove(commport);
                break;
            }
        }
        private bool CanCmdSetSelectedType()
        {
            if (SenderDisplayInfoList == null ||
                SenderDisplayInfoList.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < SenderDisplayInfoList.Count; i++)
            {
                if (SenderDisplayInfoList[i].IsChecked)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        private void Initialize()
        {
            int senderCount=0;
            _sendDataList.Clear();
            SenderDisplayInfoList.Clear();
            SenderDisplayInfo senderDisplayInfo = null;
            List<SenderDisplayInfo> displayInfoList = null;
            DataGradViewItem dataGradViewItem = null;
            foreach (string commPort in _serverProxy.EquipmentTable.Keys)
            {
                displayInfoList = new List<SenderDisplayInfo>();
                lock (_serverProxy.EquimentObject)
                {
                    senderCount = _serverProxy.EquipmentTable[commPort].EquipTypeCount;
                }
                NSCardType cardType;
                lock (_serverProxy.EquimentObject)
                {
                    EquipmentInfo inf = _serverProxy.EquipmentTable[commPort];
                    cardType = CustomTransform.ModelIdToNSCarType(inf.ModuleID);
                }

                for (int i = 0; i < senderCount; i++)
                {
                    senderDisplayInfo = new SenderDisplayInfo();
                    senderDisplayInfo.SenderAddr = (byte)i;
                    dataGradViewItem = new DataGradViewItem();
                    dataGradViewItem.CheckBoxChangedEvent += new CheckBoxChangedDel(OnCheckBoxChanged);
                    dataGradViewItem.MaterCheckBoxAllChangedEvent += new MaterCheckBoxChangedDel(OnMaterCheckBoxChanged);
                    dataGradViewItem.IsChecked = false;
                    dataGradViewItem.SenderIndex = (byte)i;
                    dataGradViewItem.SerialPort = commPort + "-" + cardType.ToString();
                    dataGradViewItem.CardType = cardType.ToString();
                    displayInfoList.Add(senderDisplayInfo);
                    SenderDisplayInfoList.Add(dataGradViewItem);
                }
                if (!_sendDataList.ContainsKey(dataGradViewItem.SerialPort))
                {
                    _sendDataList.Add(dataGradViewItem.SerialPort, displayInfoList);
                }
                else
                {
                    _sendDataList[dataGradViewItem.SerialPort] = displayInfoList;
                }
            }
            if (_sendDataList.Count > 0)
            {
                ReadOneCardData();
            }
        }

        private void OnCheckBoxChanged(string commPort,bool isChanged)
        {
            if (commPort == string.Empty)
            {
                return;
            }
            if (_isThreeStateChanged)
            {
                _isThreeStateChanged = false;
                return;
            }
            int checkedCount = 0;
            int selectedCount = 0;
            List<DataGradViewItem> list = new List<DataGradViewItem>();
            for (int i = 0; i < SenderDisplayInfoList.Count; i++)
            {
                if (SenderDisplayInfoList[i].SerialPort == commPort)
                {
                    checkedCount++;
                    list.Add(SenderDisplayInfoList[i]);
                    if (SenderDisplayInfoList[i].IsChecked)
                    {
                        selectedCount++;
                    }
                }
            }

            for (int i = 0; i < SenderDisplayInfoList.Count; i++)
            {
                if (SenderDisplayInfoList[i].SerialPort == commPort)
                {
                    _isCheckCahnged = true;
                    if (selectedCount > 0 && checkedCount != selectedCount)
                    {
                        SenderDisplayInfoList[i].IsSelectedAll = null;
                    }
                    else if (checkedCount == selectedCount)
                    {
                        SenderDisplayInfoList[i].IsSelectedAll = true;
                    }
                    else
                    {
                        SenderDisplayInfoList[i].IsSelectedAll = false;
                    }
                }
            }

        }

        private void OnMaterCheckBoxChanged(string commPort, bool? isChanged)
        {
            if (commPort == string.Empty)
            {
                return;
            }
            if (_isCheckCahnged)
            {
                _isCheckCahnged = false;
                return;
            }
            for (int i = 0; i < SenderDisplayInfoList.Count; i++)
            {
                if (SenderDisplayInfoList[i].SerialPort == commPort)
                {
                    _isThreeStateChanged = true;
                    if (isChanged == true)
                    {
                        SenderDisplayInfoList[i].IsSelectedAll = true;
                        SenderDisplayInfoList[i].IsChecked = true;

                    }
                    //else if (isChanged == false)
                    //{
                    //    SenderDisplayInfoList[i].IsSelectedAll = false;
                    //    SenderDisplayInfoList[i].IsChecked = false;
                    //}
                    else
                    {
                        SenderDisplayInfoList[i].IsSelectedAll = false;
                        SenderDisplayInfoList[i].IsChecked = false;
                    }
                }
            }
            _isThreeStateChanged = false;
        }

        private void ReadOneCardData()
        {
            foreach (string commport in _sendDataList.Keys)
            {
                if (_commPort != commport)
                {
                    InitializeData(commport);
                }
                _commPort = commport;
                if (_sendDataList.Count > 0)
                {
                    for (int i = 0; i < _sendDataList[commport].Count; i++)
                    {
                        if (!_edidAccessor.GetMarsDisplayInfo(_sendDataList[commport][i].SenderAddr))
                        {
                            return;
                        }
                        _sendDataList[commport].RemoveAt(i);
                        break;
                    }
                    if (_sendDataList[commport].Count <= 0)
                    {
                        _sendDataList.Remove(commport);
                    }
                    break;
                }
                else
                {
                    _sendDataList.Remove(commport);
                }
            }
        }

        private void InitializeData(string commport)
        {
            if (_edidAccessor != null)
            {
                _edidAccessor.SetMarsDisplayEDIDEvent -= new OperateEventHandler(CompletedSetMarsDisplayEDIDEvent);
                _edidAccessor.GetMarsDisplayPixEvent -= new GetMarsDisplayInfoEventHandler(CompletedGetMarsDisplayPixEvent);

                _edidAccessor = null;
            }
            string[] str = commport.Split('-');
            _edidAccessor = new EDIDAccessor(_serverProxy, str[0]);
            _edidAccessor.SetMarsDisplayEDIDEvent += new OperateEventHandler(CompletedSetMarsDisplayEDIDEvent);
            _edidAccessor.GetMarsDisplayPixEvent += new GetMarsDisplayInfoEventHandler(CompletedGetMarsDisplayPixEvent);

        }

        private void CompletedSetMarsDisplayEDIDEvent(object sender, OperateEventArgs args)
        {
            //设置成功
            if (args.Result)
            {
                if (_sendDataList.Count > 0)
                {
                    foreach (string commport in _sendDataList.Keys)
                    {
                        InitializeData(commport);
                        _commPort = commport;
                        if (!_edidAccessor.SetMarsDisplayEDID(_sendDataList[commport]))
                        {
                            continue;
                        }
                        _sendDataList.Remove(commport);
                        break;
                    }
                }

                string msg = "设置成功!";
                CommonStaticMethod.GetLanguageString(msg, "Lang_Gloabal_SetSucceed", out msg);

                ShowGlobalDialogMessage(msg, MessageBoxImage.Information);
            }
            else
            {
                string msg = "设置失败!";
                CommonStaticMethod.GetLanguageString(msg, "Lang_Gloabal_SetFail", out msg);
                ShowGlobalDialogMessage(msg, MessageBoxImage.Error);
            }
        }

        private void CompletedGetMarsDisplayPixEvent(object sender, GetMarsDisplayInfoEventArgs args)
        {
            if (args.Result)
            {
                for (int i = 0; i < SenderDisplayInfoList.Count; i++)
                {
                    DataGradViewItem source = SenderDisplayInfoList[i];
                    if (source.SenderIndex == args.DisplayInfo.SenderAddr && source.SerialPort == _commPort)
                    {
                        SenderDisplayInfoList[i].RefreshRate = args.DisplayInfo.Refresh;
                        SenderDisplayInfoList[i].Reslution = args.DisplayInfo.Width + "*" + args.DisplayInfo.Height;
                        break;
                    }
                }
                if (_sendDataList.Count > 0)
                {
                    ReadOneCardData();
                    return;
                }

                string msg = "读取成功!";
                CommonStaticMethod.GetLanguageString(msg, "Lang_Gloabal_ReadSucceed", out msg);

                ShowGlobalDialogMessage(msg, MessageBoxImage.Information);
            }
            else
            {
                string msg = "读取失败!";
                CommonStaticMethod.GetLanguageString(msg, "Lang_Gloabal_ReadFail", out msg);

                ShowGlobalDialogMessage(msg, MessageBoxImage.Information);
            }
        }

        private void GetSendData()
        {
            _sendDataList.Clear();
            string commPort="";
            SenderDisplayInfo senderDisplayInfo = null;
            List<SenderDisplayInfo> displayInfoList = null;
            for (int i = 0; i < SenderDisplayInfoList.Count; i++)
            {
                DataGradViewItem source = SenderDisplayInfoList[i];
                    if (source.IsChecked)
                    {
                        senderDisplayInfo = new SenderDisplayInfo();
                        if (commPort != source.SerialPort)
                        {

                            displayInfoList = new List<SenderDisplayInfo>();
                        }

                        senderDisplayInfo.Height = Height;
                        senderDisplayInfo.Width = Width;
                        senderDisplayInfo.Refresh = (int)RefreshRate;
                        senderDisplayInfo.SenderAddr = source.SenderIndex;
                        senderDisplayInfo.IsCustomPix = false;
                        displayInfoList.Add(senderDisplayInfo);
                        if (!_sendDataList.ContainsKey(source.SerialPort))
                        {
                            _sendDataList.Add(source.SerialPort, new List<SenderDisplayInfo>());
                        }
                        _sendDataList[source.SerialPort].Add(senderDisplayInfo);
                        commPort = source.SerialPort;
                    }
                }
        }
        #endregion

    }

}