using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Nova.SmartLCT.Interface;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Nova.LCT.GigabitSystem.Common;
using System.Windows.Input;
using Nova.SmartLCT.UI;
using System.Linq;
using System.Windows;

namespace Nova.SmartLCT.UI
{

    public class ScanBoardConfigManager_VM : SmartLCTViewModeBase
    {
        #region 界面属性
        public ObservableCollection<DataGradItemInfo> DataGradItemInfoList
        {
            get { return _dataGradItemInfoList; }
            set 
            { 
                _dataGradItemInfoList = value;
                RaisePropertyChanged(GetPropertyName(o => this.DataGradItemInfoList));
            }
        }
        private ObservableCollection<DataGradItemInfo> _dataGradItemInfoList = new ObservableCollection<DataGradItemInfo>();

        private OpenFileDialogData _openFile = new OpenFileDialogData();
        public OpenFileDialogData OpenFile
        {
            get { return _openFile; }
            set
            {
                _openFile = value;
                RaisePropertyChanged(GetPropertyName(o => this.OpenFile));
                ;
            }
        }


        public bool? SelectedAll
        {
            get { return _selectedAll; }
            set
            {
                _selectedAll = value;
                if (value == null && !_isSelectedSomeOne)
                {
                    _selectedAll = false;
                }
                SetCheckBoxSelectState();
                RaisePropertyChanged("SelectedAll");
            }
        }
        private bool? _selectedAll = false;

        public DataGradItemInfo SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                _selectedValue = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SelectedValue));
            }
        }
        private DataGradItemInfo _selectedValue = null;
        #endregion

        #region 属性
        
        #endregion
        
        #region 命令
        /// <summary>
        /// 添加行命令
        /// </summary>
        public RelayCommand CmdDeleteConfig
        {
            get;
            private set;
        }
        public RelayCommand CmdImortConfigFile
        {
            get;
            private set;
        }
        public RelayCommand CmdExportConfifFile
        {
            get;
            private set;
        }
        #endregion

        #region 字段
        private bool _isSelectedAllCheck = false;
        private bool _isSelectedSomeOne = false;

        private List<string> _initializeFileNameList = new List<string>();

        private string _saveFilePath = string.Empty;
        #endregion

        #region 构造函数
        public ScanBoardConfigManager_VM()
        {
            #region 设计器测试数据
            if (this.IsInDesignMode)
            {
                DataGradItemInfoList.Clear();
                DataGradItemInfo data = new DataGradItemInfo();
                data.CascadeType = "从左到右";
                data.ChipType = ChipType.Chip_MBI5036;
                data.ScanBoardName = "shasddas";
                data.ScanBoardSize = "128x6400";
                DataGradItemInfoList.Add(data);
                data = new DataGradItemInfo();
                data.CascadeType = "从上到下";
                data.ChipType = ChipType.Chip_MBI5036;
                data.ScanBoardName = "aaaaaaaaa";
                data.ScanBoardSize = "1028x64";
                DataGradItemInfoList.Add(data);
            }
            #endregion 设计器数据

            CmdDeleteConfig = new RelayCommand(DeleteConfigData,OnDeleteButtonCanExecute);
            CmdExportConfifFile = new RelayCommand(OpenExportConfigFile, OnDeleteButtonCanExecute);
            CmdImortConfigFile = new RelayCommand(OpenImportConfigFile);

            string scanFile = "接收卡配置文件";
            GetLangString(scanFile, "Lang_Global_ScannerFile", out scanFile);
            string allFile = "所有文件";
            GetLangString(allFile, "Lang_Gloabal_AllFile", out allFile);
            string filter=scanFile + "(*.rcfg)|*.rcfg|"+ allFile + "(*.*)|*.*";

            OpenFile.FileFilter = filter;
            
            OpenFile.IsMultiselect = true;

            #if Test
            InitializeFileNameList.Clear();//需修改
            InitializeFileNameList.Add(@"D:\text\5555555.rcfg");//需修改
            InitializeFileNameList.Add(@"D:\text\sssaaas.rcfg");//需修改
            _saveFileName = @"D:\TEST";
            #endif
            if (!this.IsInDesignMode)
            {
                _initializeFileNameList = _globalParams.OriginalScanFiles;

            }
            _saveFilePath = SCANCONFIGFILES_LIB_PATH;

            Initialize();//TODO:需修改
            
        }
        #endregion
        
        #region 私有函数
        private void DeleteConfigData()
        {
            string msgData = "是否要删除选中数据！";
            GetLangString(msgData, "Lang_ReceiveCard_DeleteSeletedData", out msgData);

            //ButtonClickType buttonType = ButtonClickType.Cancel;
            //NotificationMessageAction<ButtonClickType> nsa =
            //          new NotificationMessageAction<ButtonClickType>(buttonType, msgData, MsgToken.MSG_DELETEMSG_MSGBOX, MesBoxNotifycationCallBack);
            //Messenger.Default.Send(nsa, MsgToken.MSG_DELETEMSG_MSGBOX);

            if (ShowQuestionMessage(msgData, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DeleteFile();
            }
        }

        private bool OnDeleteButtonCanExecute()
        {
            if (DataGradItemInfoList.Count > 0)
            {
                for (int i = 0; i < DataGradItemInfoList.Count; i++)
                {
                    if (DataGradItemInfoList[i].IsChecked)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void OpenExportConfigFile()
        {
            ObservableCollection<DataGradItemInfo> data = GetSelectedDataList();
            if (data.Count <= 0)
            {
                return;
            }
            NotificationMessageAction<ObservableCollection<DataGradItemInfo>> nsa =
                    new NotificationMessageAction<ObservableCollection<DataGradItemInfo>>(this, data, MsgToken.MSG_EXPORTCONFIGFOLE, ExportCfgFileNotifycationCallBack);
            Messenger.Default.Send(nsa, MsgToken.MSG_EXPORTCONFIGFOLE);
        }

        private void OpenImportConfigFile()
        {
            if (OpenFile.IsCheckedOK)
            {
                ObservableCollection<DataGradItemInfo> data = GetFileDataList();
                //ObservableCollection<DataGradItemInfo> data = new ObservableCollection<DataGradItemInfo>();
                //DataGradItemInfo ta = new DataGradItemInfo();
                //ta.ScanBoardSize = "50*80";
                //data.Add(ta);
                if (data.Count <= 0)
                {
                    return;
                }
                NotificationMessageAction<ObservableCollection<DataGradItemInfo>> nsa =
                        new NotificationMessageAction<ObservableCollection<DataGradItemInfo>>(this, data, MsgToken.MSG_IMPORTCONFIGFOLE, ImportCfgFileNotifycationCallBack);
                Messenger.Default.Send(nsa, MsgToken.MSG_IMPORTCONFIGFOLE);
            }
        }
            
        private bool SaveScanBdConfig(string saveFileName,ScanBoardProperty scanBdProperty)
        {
            XmlSerializer xmls = null;
            StreamWriter sw = null;
            bool res = false; 
            string msg = string.Empty;
            try
            {
                xmls = new XmlSerializer(typeof(ScanBoardProperty));
                sw = new StreamWriter(saveFileName);
                //XmlWriterSettings setting = new XmlWriterSettings();
                //setting.CloseOutput = true;

                XmlWriter xmlWriter = XmlWriter.Create(sw);
                xmls.Serialize(sw, scanBdProperty);
                sw.Close();
                res = true;
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
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
        }
        private void ExportCfgFileNotifycationCallBack(ObservableCollection<DataGradItemInfo> info)
        {
            if (info.Count>0)
            {
                for (int i = 0; i < info.Count; i++)
                {
                    if (File.Exists(info[i].FileName))
                    {
                        CopyDirectory(info[i].FileName,info[i].SaveFilePath);
                    }
                }
            }

            string msg = "导出文件成功！";
            CommonStaticMethod.GetLanguageString(msg, "Lang_Gloabal_ExportFileSuccessed", out msg);
            ShowGlobalDialogMessage(msg, MessageBoxImage.Asterisk);
        }

        private void ImportCfgFileNotifycationCallBack(ObservableCollection<DataGradItemInfo> info)
        {
            if (_saveFilePath==string.Empty)
            {
                return;
            }
            if (!Directory.Exists(_saveFilePath))
            {
                Directory.CreateDirectory(_saveFilePath);
            }
            string sourceDirName = "";
            DataGradItemInfo dataInfo = null;
            for (int i = 0; i < info.Count; i++)
            {
                DataGradItemInfo data = info[i];
                if ((data.DataHandleWay == HandleWay.Add ||
                    data.DataHandleWay == HandleWay.Replace) &&
                    data.DataHandelSatate != DataSatate.None)
                {
                    sourceDirName = Path.GetDirectoryName(data.FileName);
                    CopyDirectory(data.FileName, _saveFilePath);
                    dataInfo = new DataGradItemInfo();
                    dataInfo.IsCheckedEvent += new IsCheckedDel(OnIsCheckedChanged);
                    dataInfo.CascadeType = data.CascadeType;
                    dataInfo.ChipType = data.ChipType;
                    dataInfo.DataHandelSatate = data.DataHandelSatate;
                    dataInfo.DataHandleWay = data.DataHandleWay;
                    dataInfo.FileName = data.FileName;
                    dataInfo.SaveFilePath = data.SaveFilePath;
                    dataInfo.ScanBoardName = data.ScanBoardName;
                    dataInfo.ScanBoardSize = data.ScanBoardSize;
                    if (data.DataHandleWay == HandleWay.Add)
                    {
                        DataGradItemInfoList.Add(dataInfo);
                        _initializeFileNameList.Add(dataInfo.SaveFilePath);
                    }
                    else if (data.DataHandleWay == HandleWay.Replace)
                    {
                        int count = DataGradItemInfoList.Count;
                        int index = -1;
                        for (int m = 0; m < count; m++ )
                        {
                            if (DataGradItemInfoList[m].ScanBoardName == dataInfo.ScanBoardName)
                            {
                                index = m;
                                break;
                            }
                        }
                        DataGradItemInfoList[index] = dataInfo;
                    }

                    #region 添加到箱体库
                    string originalFile = info[i].FileName;
                    ScanBoardProperty scanBdProp = new ScanBoardProperty();

                    if (CustomTransform.LoadScanProFile(originalFile, ref scanBdProp))
                    {
                        ScannerCofigInfo scanConfig = new ScannerCofigInfo();
                        scanConfig.DataGroup = scanBdProp.StandardLedModuleProp.DataGroup;
                        scanConfig.DisplayName = Path.GetFileNameWithoutExtension(originalFile);
                        scanConfig.ScanBdProp = scanBdProp;

                        string strCascade = scanBdProp.ModCascadeType.ToString();
                        CommonStaticMethod.GetLanguageString(strCascade, strCascade, out strCascade);
                        scanConfig.StrCascadeType = strCascade;

                        string strDriverChip = scanBdProp.StandardLedModuleProp.DriverChipType.ToString();
                        CommonStaticMethod.GetLanguageString(strDriverChip, strDriverChip, out strDriverChip);
                        scanConfig.StrChipType = strDriverChip;

                        string strScanType = scanBdProp.StandardLedModuleProp.ScanType.ToString();
                        CommonStaticMethod.GetLanguageString(strScanType, strScanType, out strScanType);
                        scanConfig.StrScanType = strScanType;

                        if (info[i].DataHandleWay == HandleWay.Add)
                        {
                            int index = _globalParams.ScannerConfigCollection.IndexOf(scanConfig);
                            if (_globalParams.ScannerConfigCollection.Count == 0)
                            {
                                _globalParams.ScannerConfigCollection.Add(scanConfig);
                            }
                            else
                            {
                                _globalParams.ScannerConfigCollection.Insert(_globalParams.ScannerConfigCollection.Count - 1, scanConfig);
                            }
                        }
                        else if (info[i].DataHandleWay == HandleWay.Replace)
                        {
                            //ObservableCollection<ScannerCofigInfo> find = 
                            int count = _globalParams.ScannerConfigCollection.Count;
                            int index = -1;
                            for (int m = 0; m < count; m++)
                            {
                                if (_globalParams.ScannerConfigCollection[m].DisplayName == data.ScanBoardName)
                                {
                                    index = m;
                                    break;
                                }
                            }
                            if (index >= 0 && _globalParams.ScannerConfigCollection != null && _globalParams.ScannerConfigCollection.Count != 0)
                            {

                                _globalParams.ScannerConfigCollection.RemoveAt(index);
                                _globalParams.ScannerConfigCollection.Insert(index, scanConfig);
                            }
                        }
                    }
                    #endregion
                }
            }
            OnIsCheckedChanged(false);
        }

        private bool IsExistList(DataGradItemInfo item)
        {
            for (int i = 0; i < DataGradItemInfoList.Count; i++)
            {
                DataGradItemInfo info = DataGradItemInfoList[i];
                if (info.ScanBoardName == item.ScanBoardName)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CopyDirectory(string sourceDirName, string destDirName)
        {
            string name = Path.GetFileName(sourceDirName);
            string[] fileList = Directory.GetFiles(destDirName);
            bool isExists=false;
            for (int i = 0; i < fileList.Length; i++)
            {
                if (fileList[i] == sourceDirName)
                {
                    isExists = true;
                }
            }
            if (!isExists)
            {
                File.Copy(sourceDirName, destDirName + name, true);
                return true;
            }
            return false;
        }  

        private void SetCheckBoxSelectState()
        {
            for (int i = 0; i < DataGradItemInfoList.Count; i++)
            {
                _isSelectedAllCheck = true;
                if (SelectedAll == true)
                {
                    DataGradItemInfoList[i].IsChecked = true;
                }
                else if(SelectedAll==false)
                {
                    DataGradItemInfoList[i].IsChecked = false;
                }
            }
            _isSelectedAllCheck = false;
            _isSelectedSomeOne = false;
        }

        private void Initialize()
        {
            DataGradItemInfoList.Clear();
            if (_initializeFileNameList.Count <= 0)
            {
                return;
            }
            ScanBoardProperty scanBDproData;
            DataGradItemInfo info;
            for (int i = 0; i < _initializeFileNameList.Count; i++)
            {
                if (LoadScanProFile(_initializeFileNameList[i], out scanBDproData))
                {
                    if (scanBDproData != null)
                    {
                        info = new DataGradItemInfo();
                        info.IsCheckedEvent += new IsCheckedDel(OnIsCheckedChanged);
                        info.ChipType = scanBDproData.StandardLedModuleProp.DriverChipType;
                        info.ScanBoardName = Path.GetFileNameWithoutExtension(_initializeFileNameList[i]);
                        info.ScanBoardSize = scanBDproData.Width + "x" + scanBDproData.Height;
                        info.CascadeType = GetModCascadeType(scanBDproData.ModCascadeType);
                        info.FileName = _initializeFileNameList[i];
                        info.SaveFilePath = _initializeFileNameList[i];
                        DataGradItemInfoList.Add(info);
                        CopyDirectory(_initializeFileNameList[i], _saveFilePath);
                    }
                }
            }

            if (DataGradItemInfoList.Count > 0)
            {
                SelectedValue = DataGradItemInfoList[0];
            }

        }

        private void DeleteFile()
        {
            for (int i = 0; i < DataGradItemInfoList.Count; i++)
            {
                DataGradItemInfo info = DataGradItemInfoList[i];
                if (info.IsChecked)
                {
                    if (File.Exists(info.SaveFilePath))
                    {
                        File.Delete(info.SaveFilePath);
                    }
                    DataGradItemInfoList.RemoveAt(i);
                    _initializeFileNameList.Remove(info.SaveFilePath);
                    i--;

                    #region 删除箱体选择中的对应配置
                    int count = _globalParams.ScannerConfigCollection.Count;
                    for (int j = 0; j < count; j++)
                    {
                        if (_globalParams.ScannerConfigCollection[j].DisplayName == info.ScanBoardName)
                        {
                            _globalParams.ScannerConfigCollection.RemoveAt(j);
                            break;
                        }
                    }
                    #endregion
                }
            }
        }

        private void OnIsCheckedChanged(bool isChecked)
        {
            if (!_isSelectedAllCheck)
            {
                _isSelectedSomeOne = true;
                int isCheckedCount = 0;
                int allDataCount = DataGradItemInfoList.Count;
                for (int i = 0; i < allDataCount; i++)
                {
                    if (DataGradItemInfoList[i].IsChecked)
                    {
                        isCheckedCount++;
                    }
                }
                if (isCheckedCount == 0)
                {
                    SelectedAll = false;
                }
                else if (isCheckedCount>0 &&
                    isCheckedCount < allDataCount)
                {
                    SelectedAll = null;
                }
                else if(isCheckedCount == allDataCount)
                {
                    SelectedAll = true;
                }
            }
        }

        private ObservableCollection<DataGradItemInfo> GetSelectedDataList()
        {
            ObservableCollection<DataGradItemInfo> data = new ObservableCollection<DataGradItemInfo>();
            for (int i = 0; i < DataGradItemInfoList.Count; i++)
            {
                if (DataGradItemInfoList[i].IsChecked)
                {
                    data.Add(DataGradItemInfoList[i]);
                }
            }
            return data;
        }

        private ObservableCollection<DataGradItemInfo> GetFileDataList()
        {
            ObservableCollection<DataGradItemInfo> data = new ObservableCollection<DataGradItemInfo>();
            if (OpenFile.OpenFileNameList.Count > 0)
            {
                for (int i = 0; i < OpenFile.OpenFileNameList.Count; i++)
                {
                    DataGradItemInfo info;
                    GetOneFileDataInfo(OpenFile.OpenFileNameList[i], out info);
                    if (info != null)
                    {
                        data.Add(info);
                    }
                }
            }
            OpenFile.OpenFileNameList.Clear();
            return data;
        }

        private void GetOneFileDataInfo(string fileName,out DataGradItemInfo info)
        {
            info = new DataGradItemInfo();
            ScanBoardProperty scanBDproData;
            if (LoadScanProFile(fileName,out scanBDproData))
            {
                if (scanBDproData != null)
                {
                    info.ChipType = scanBDproData.StandardLedModuleProp.DriverChipType;
                    info.ScanBoardName = Path.GetFileNameWithoutExtension(fileName);
                    info.ScanBoardSize = scanBDproData.Width + "x" + scanBDproData.Height;
                    info.CascadeType = GetModCascadeType(scanBDproData.ModCascadeType);
                    info.FileName = fileName;

                    string fileNameWithExt = Path.GetFileName(fileName);
                    info.SaveFilePath = _saveFilePath + fileNameWithExt;
                    return;
                }
            }
            info = null;
        }

        private string GetModCascadeType(ModuleCascadeDiretion modCascadeType)
        {
            string msg = "";
            switch (modCascadeType)
            {
                case ModuleCascadeDiretion.RightLeft:
                    msg="从右到左";
                    GetLangString(msg, "Lang_ReceiveCard_RightToLeft", out msg);
                    return msg;
                case ModuleCascadeDiretion.LeftRight:
                    msg = "从左到右";
                    GetLangString(msg, "Lang_ReceiveCard_LeftToRight", out msg);
                    return msg;
                case ModuleCascadeDiretion.DownUp:
                    msg = "从下到上";
                    GetLangString(msg, "Lang_ReceiveCard_BottomToTop", out msg);
                    return msg;
                case ModuleCascadeDiretion.UpDown:

                    msg = "从上到下";
                    GetLangString(msg, "Lang_ReceiveCard_TopToBottom", out msg);
                    return msg;
                default:

                    msg = "从上到下";
                    GetLangString(msg, "Lang_ReceiveCard_TopToBottom", out msg);
                    return msg;
            }
        }

        private bool LoadScanProFile(string openFileName, out ScanBoardProperty scanBDproData)
        {
            scanBDproData = new ScanBoardProperty();
            XmlSerializer xmls = null;
            StreamReader sr = null;
            XmlReader xmlReader = null;
            string msg = string.Empty;
            bool res = false;
            try
            {
                xmls = new XmlSerializer(typeof(ScanBoardProperty));
                sr = new StreamReader(openFileName);
                xmlReader = XmlReader.Create(sr);
                if (!xmls.CanDeserialize(xmlReader))
                {
                    res = false;
                    return res;
                }
                else
                {
                    ScanBoardProperty scanBDpro = (ScanBoardProperty)xmls.Deserialize(xmlReader);
                    if (!scanBDpro.CopyTo(scanBDproData))
                    {
                        res = false;
                        scanBDproData = null;
                        return res;
                    }
                    else
                    {
                        res = true;

                        return res;
                    }
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

        private void GetLangString(string defaultStr, string key, out string msg)
        {
            CommonStaticMethod.GetLanguageString(defaultStr, key, out msg);
        }
        #endregion
    }
}