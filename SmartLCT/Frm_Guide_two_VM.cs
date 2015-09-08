using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Command;
using Nova.SmartLCT.Interface;
using System.Windows.Forms;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using GalaSoft.MvvmLight.Messaging;
using Nova.LCT.GigabitSystem.Common;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;
using System.ComponentModel;


namespace SmartLCT
{
    public class Frm_Guide_two_VM : SmartLCTViewModeBase
    {
        #region 属性
        public OperateScreenType OperateType
        {
            get { return _operateType; }
            set
            {
                _operateType = value;
                NotifyPropertyChanged(GetPropertyName(o => this.OperateType));
            }
        }
        private OperateScreenType _operateType = OperateScreenType.CreateScreen;
        public string ProjectName
        {
            get { return _projectName; }
            set 
            {
                _projectName = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ProjectName));
            }
        }
        private string _projectName;
        public string ProjectLocationPath
        {
            get { return _projectLocationPath; }
            set 
            { 
                _projectLocationPath = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ProjectLocationPath));
            }
        }
        private string _projectLocationPath = "";
        public ObservableCollection<SenderConfigInfo> SenderConfigCollection
        {
            get { return _senderConfigCollection; }
            set
            {
                _senderConfigCollection = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SenderConfigCollection));
            }
        }
        private ObservableCollection<SenderConfigInfo> _senderConfigCollection = new ObservableCollection<SenderConfigInfo>();
        public SenderConfigInfo SelectedSenderConfigInfo
        {
            get { return _selectedSenderConfigInfo; }
            set
            {
                _selectedSenderConfigInfo = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SelectedSenderConfigInfo));
            }
        }
        private SenderConfigInfo _selectedSenderConfigInfo = new SenderConfigInfo();
        public ObservableCollection<ScannerCofigInfo> ScannerCofigCollection
        {
            get { return _scannerCofigCollection; }
            set
            {
                if (ScannerCofigCollection != null)
                {
                    ScannerCofigCollection.CollectionChanged -= new NotifyCollectionChangedEventHandler(OnScannerCofigCollectionChanged);
                }
                _scannerCofigCollection = value;
                if (ScannerCofigCollection != null)
                {
                    ScannerCofigCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(OnScannerCofigCollectionChanged);
                }
                NotifyPropertyChanged(GetPropertyName(o => this.ScannerCofigCollection));
            }
        }
        private ObservableCollection<ScannerCofigInfo> _scannerCofigCollection = new ObservableCollection<ScannerCofigInfo>();
        public ScannerCofigInfo SelectedScannerConfigInfo
        {
            get
            {
                return _selectedScannerConfigInfo;
            }
            set
            {
                _selectedScannerConfigInfo = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SelectedScannerConfigInfo));
            }
        }
        private ScannerCofigInfo _selectedScannerConfigInfo = new ScannerCofigInfo();
        public bool IsCreateEmptyProject
        {
            get 
            {
                return _isCreateEmptyProject; 
            }
            set 
            {
                _isCreateEmptyProject = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsCreateEmptyProject));
            }
        }
        private bool _isCreateEmptyProject = false;
        public ArrangeType SelectedArrangeType
        {
            get { return _selectedArrangeType; }
            set
            {
                _selectedArrangeType = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SelectedArrangeType));
            }
        }
        private ArrangeType _selectedArrangeType = ArrangeType.LeftBottom_Hor;
        public int Rows
        {
            get { return _rows; }
            set
            {
                _rows = value;
                OnAddColsCount();
                NotifyPropertyChanged(GetPropertyName(o => this.Rows));
            }
        }
        private int _rows = 1;
        public int Cols
        {
            get { return _cols; }
            set 
            {
                _cols = value;
                OnAddColsCount();
                NotifyPropertyChanged(GetPropertyName(o => this.Cols));
            }
        }
        private int _cols = 1;
        public ConfigurationData ConfigData
        {
            get { return _configData; }
            set
            {
                _configData = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ConfigData));
            }
        }
        private ConfigurationData _configData = new ConfigurationData();
        private void OnAddRowsCount()
        {
            if (SelectedScannerConfigInfo == null || SelectedScannerConfigInfo.ScanBdProp == null)
            {
                return;
            }
            ScanBoardProperty scanBdProp = SelectedScannerConfigInfo.ScanBdProp;
            int height = scanBdProp.Height;
            double portHeight = SmartLCTViewModeBase.MaxScreenHeight;
            int maxRowsCount = (int)portHeight / height;
            if (Rows > maxRowsCount)
            {
                Rows = maxRowsCount;
            }
        }

        private void OnAddColsCount()
        {
            if (SelectedScannerConfigInfo == null || SelectedScannerConfigInfo.ScanBdProp == null)
            {
                return;
            }
            ScanBoardProperty scanBdProp = SelectedScannerConfigInfo.ScanBdProp;
            int width = scanBdProp.Width;
            double portWidth = SmartLCTViewModeBase.MaxScreenWidth;
            int maxColsCount = (int)portWidth / width;
            if (Cols > maxColsCount)
            {
                Cols = maxColsCount;
            }
        }



        #endregion

        #region 命令
        public RelayCommand<TextChangedEventArgs> CmdProjectNameChanged
        {
            get;
            private set;
        }
        public RelayCommand CmdBrowseProjectLocation
        {
            get;
            private set;
        }
        public RelayCommand CmdCustomSizeDropDownClosed
        {
            get;
            private set;
        }
        public RelayCommand CmdShowScanBoardConfigManager
        {
            get;
            private set;
        }
        public RelayCommand CmdCreate
        {
            get;
            private set;
        }
        public RelayCommand CmdCancel
        {
            get;
            private set;
        }
        public RelayCommand<string> CmdArrangeScanner
        {
            get;
            private set;
        }
        #endregion

        #region 构造
        public Frm_Guide_two_VM()
        {
            if (!this.IsInDesignMode)
            {
                ProjectLocationPath = Function.GetDefaultCurrentProjectPath(_globalParams.RecentProjectPaths);
                ProjectName = Function.GetDefaultProjectName(SmartLCTViewModeBase.DefaultProjectMainName, ".xml", ProjectLocationPath);
                CmdProjectNameChanged = new RelayCommand<TextChangedEventArgs>(OnProjectNameChanged);
                CmdBrowseProjectLocation = new RelayCommand(OnBrowseProjectLocation);
                CmdCustomSizeDropDownClosed = new RelayCommand(OnCustomReceiveSize);
                CmdShowScanBoardConfigManager = new RelayCommand(OnShowScanBoardConfigManager);
                CmdCreate = new RelayCommand(OnCreate, CanCreate);
                CmdCancel = new RelayCommand(OnCancel);
                CmdArrangeScanner = new RelayCommand<string>(OnCmdArrangeScanner);
                SenderConfigCollection = _globalParams.SenderConfigCollection;
                if (SenderConfigCollection != null && SenderConfigCollection.Count != 0)
                {
                    SelectedSenderConfigInfo = SenderConfigCollection[0];
                }

                ScannerCofigCollection = _globalParams.ScannerConfigCollection;
                if (ScannerCofigCollection == null ||
                    ScannerCofigCollection.Count == 0)
                {
                    ScannerCofigInfo customScannerConfigInfo = new ScannerCofigInfo();
                    string strCustom = "";
                    CommonStaticMethod.GetLanguageString("自定义", "Lang_Global_Custom", out strCustom);
                    customScannerConfigInfo.DisplayName = strCustom;
                    customScannerConfigInfo.ScanBdSizeType = ScannerSizeType.Custom;
                    //ScannerCofigCollection = new ObservableCollection<ScannerCofigInfo>();
                    ScannerCofigCollection.Add(customScannerConfigInfo);
                }
                else
                {
                    bool isHaveCustom = false;
                    for (int i = 0; i < ScannerCofigCollection.Count; i++)
                    {
                        if (ScannerCofigCollection[i].ScanBdSizeType == ScannerSizeType.Custom)
                        {
                            isHaveCustom = true;
                            break;
                        }
                    }
                    if (!isHaveCustom)
                    {
                        ScannerCofigInfo customScannerConfigInfo = new ScannerCofigInfo();
                        string strCustom = "";
                        CommonStaticMethod.GetLanguageString("自定义", "Lang_Global_Custom", out strCustom);
                        customScannerConfigInfo.DisplayName = strCustom;
                        customScannerConfigInfo.ScanBdSizeType = ScannerSizeType.Custom;
                        ScannerCofigCollection.Insert(ScannerCofigCollection.Count, customScannerConfigInfo);
                    }
                }
                if (ScannerCofigCollection != null)
                {
                    if (ScannerCofigCollection.Count == 1)
                    {
                        SelectedScannerConfigInfo = null;
                    }
                    else
                    {
                        SelectedScannerConfigInfo = ScannerCofigCollection[0];
                    }
                }
                // 在此点下面插入创建对象所需的代码。
            }
        }
        #endregion

        #region 命令执行函数
        private void OnProjectNameChanged(TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textbox_ProjectName = e.Source as System.Windows.Controls.TextBox;
            if (textbox_ProjectName.Text.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0)
            {
                textbox_ProjectName.Text = textbox_ProjectName.Text.Substring(0, textbox_ProjectName.Text.Length - 1);
                textbox_ProjectName.Select(textbox_ProjectName.Text.Length, 0);
            }
        }
        private void OnCmdArrangeScanner(string para)
        {
           foreach (ArrangeType  arrangeType in Enum.GetValues(typeof(ArrangeType)))
             {
                 if (arrangeType.ToString() == para)
                 {
                     SelectedArrangeType = arrangeType;
                     break;
                 }
             }
        }
        private bool CanCreate()
        {
            if (ProjectName == "")
            {
                //ShowGlobalDialogMessage("工程名不能为空", MessageBoxImage.Warning);
                return false;
            }
            else if ((!IsCreateEmptyProject) && SelectedScannerConfigInfo == null)
            {
                //ShowGlobalDialogMessage("箱体大小不能为空", MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        private void OnCreate()
        {
            //判断选择的路径下是否有同名文件
            string filename = ProjectLocationPath + "\\" + ProjectName+".xml";
            if (File.Exists(filename))
            {
                string msg = "";
                CommonStaticMethod.GetLanguageString("创建失败！目录下存在相同文件名的文件！", "Lang_ScanBoardConfigManager_SameFile", out msg);
                ShowGlobalDialogMessage(msg, MessageBoxImage.Error);
                return;
            }

            if (IsCreateEmptyProject)//创建空工程
            {
                ConfigurationData configurationData = new ConfigurationData(ProjectName, ProjectLocationPath, SelectedSenderConfigInfo, SelectedScannerConfigInfo, Rows, Cols, SelectedArrangeType, IsCreateEmptyProject, OperateType);
                ConfigData = configurationData;
                Messenger.Default.Send<object>(null, MsgToken.MSG_CLOSE_GUIDETWOFORM);
                return;
            }

            //判断是否超出带载（向导只支持单发送卡配置）
            Size portLoadPoint = Function.CalculatePortLoadSize(60,24);
            double portLoadSize = portLoadPoint.Width * portLoadPoint.Height;
            double senderLoadSize = portLoadSize * SelectedSenderConfigInfo.PortCount;
            ScanBoardProperty scanBdProp = SelectedScannerConfigInfo.ScanBdProp;
            int width = scanBdProp.Width;
            int height = scanBdProp.Height;
            double currentLoadSize = width * Cols * height * Rows;
            if (currentLoadSize > senderLoadSize)
            {
                string msg = "";
                CommonStaticMethod.GetLanguageString("超出带载，请重新设置!", "Lang_ScanBoardConfigManager_OverLoad", out msg);
                ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);
            }
            else
            {
                //每个网口行的整数倍和列的整数倍
                double rowIndex = -1;
                double colIndex = -1;
                //需要多少网口可以带载完           
                //1、需要几个网口
                double portCount = Math.Ceiling(Rows * Cols / (portLoadSize / (height * width)));

                //2、计算每个网口的行列数（水平串线则是列的整数倍，垂直串线则是行的整数倍）
                while (true)
                {
                    if (SelectedArrangeType == ArrangeType.LeftBottom_Hor
                        || SelectedArrangeType == ArrangeType.LeftTop_Hor
                        || SelectedArrangeType == ArrangeType.RightBottom_Hor
                        || SelectedArrangeType == ArrangeType.RightTop_Hor)//水平串线
                    {
                        rowIndex = Math.Ceiling(Rows * Cols / portCount / Cols);
                        if (rowIndex * Cols * height * width > portLoadSize)
                        {
                            portCount += 1;
                            if (portCount > SelectedSenderConfigInfo.PortCount)
                            {
                                string msg = "";
                                CommonStaticMethod.GetLanguageString("超出带载，请重新设置!", "Lang_ScanBoardConfigManager_OverLoad", out msg);
                                ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);
                                return;
                            }                         
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        colIndex = Math.Ceiling(Rows * Cols / portCount / Rows);
                        if (colIndex * Rows * height * width > portLoadSize)
                        {
                            portCount += 1;
                            if (portCount > SelectedSenderConfigInfo.PortCount)
                            {
                                string msg = "";
                                CommonStaticMethod.GetLanguageString("超出带载，请重新设置!", "Lang_ScanBoardConfigManager_OverLoad", out msg);
                                ShowGlobalDialogMessage(msg, MessageBoxImage.Warning);
                                return;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
  
                //根据配置数据生成显示
                ConfigurationData configurationData = new ConfigurationData(ProjectName, ProjectLocationPath, SelectedSenderConfigInfo, SelectedScannerConfigInfo, Rows, Cols, SelectedArrangeType, IsCreateEmptyProject,OperateType);
                ConfigData = configurationData;
                Messenger.Default.Send<object>(null, MsgToken.MSG_CLOSE_GUIDETWOFORM);
            }
        }
        private void OnCancel()
        {
            ConfigurationData configurationData = null;
            ConfigData = configurationData;
            //取消向导配置
            Messenger.Default.Send<object>(null, MsgToken.MSG_CLOSE_GUIDETWOFORM);

        }
        private void OnCustomReceiveSize()
        {
            if (SelectedScannerConfigInfo == null)
            {
                return;
            }
            if (SelectedScannerConfigInfo.ScanBdSizeType == ScannerSizeType.Custom)
            {
                CustomReceiveResult info = new CustomReceiveResult();
                info.Width = 64;
                info.Height = 64;
                NotificationMessageAction<CustomReceiveResult> nsa =
                new NotificationMessageAction<CustomReceiveResult>(this, info, MsgToken.MSG_SHOWSETCUSTOMRECEIVESIZE, SetCustomReceiveNotifycationCallBack);
                Messenger.Default.Send(nsa, MsgToken.MSG_SHOWSETCUSTOMRECEIVESIZE);

            }
        }
        private void SetCustomReceiveNotifycationCallBack(CustomReceiveResult info)
        {
            if (info.IsOK != true)
            {
                if (ScannerCofigCollection != null && ScannerCofigCollection.Count == 1)
                {
                    this.SelectedScannerConfigInfo = null;
                }
                else
                {
                    this.SelectedScannerConfigInfo = ScannerCofigCollection[0];
                }
            }
            else
            {
                ScannerCofigInfo scanConfig = new ScannerCofigInfo();
                ScanBoardProperty scanBdProp = new ScanBoardProperty();
                scanBdProp.StandardLedModuleProp.DriverChipType = ChipType.Unknown;
                scanBdProp.ModCascadeType = ModuleCascadeDiretion.Unknown;
                scanBdProp.Width = info.Width;
                scanBdProp.Height = info.Height;
                scanConfig.ScanBdProp = scanBdProp;
                scanConfig.DisplayName = info.Width.ToString() + "_" + info.Height.ToString();
                scanConfig.ScanBdSizeType = ScannerSizeType.NoCustom;
                int index = -1;
                for (int i = 0; i < ScannerCofigCollection.Count; i++)
                {
                    if (ScannerCofigCollection[i].DisplayName == scanConfig.DisplayName)
                    {
                        index = i;
                        break;

                    }
                }
                if (index >= 0)
                {
                    SelectedScannerConfigInfo = ScannerCofigCollection[index];
                }
                else
                {
                    ScannerCofigCollection.Insert(ScannerCofigCollection.Count - 1, scanConfig);
                }
                SelectedScannerConfigInfo = scanConfig;
            }
        }

        private void OnShowScanBoardConfigManager()
        {
            Messenger.Default.Send<string>("", MsgToken.MSG_SHOWSCANBOARDCONFIGMANAGER);
        }

        private void OnBrowseProjectLocation()
        {       
            BrowseProjectLocation();
        }
        private void BrowseProjectLocation()
        {
            if (CommonFileDialog.IsPlatformSupported)
            {
                var folderSelectorDialog = new CommonOpenFileDialog();
                folderSelectorDialog.EnsureReadOnly = true;
                folderSelectorDialog.IsFolderPicker = true;
                folderSelectorDialog.AllowNonFileSystemItems = false;
                folderSelectorDialog.Multiselect = false;
                folderSelectorDialog.InitialDirectory = ProjectLocationPath;
                
                folderSelectorDialog.Title = "工程位置";

                if (folderSelectorDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    ProjectLocationPath = folderSelectorDialog.FileName;
                }


            }
        }
        private void BrowseProjectLocation(string b)
        {
            string msg = string.Empty;
            using (FolderBrowserDialog sfd = new FolderBrowserDialog())
            {
                sfd.Description = "项目位置";
                sfd.ShowNewFolderButton = true;
 
                if (!Directory.Exists(ProjectLocationPath))
                {
                    Directory.CreateDirectory(ProjectLocationPath);
                }
                sfd.SelectedPath = ProjectLocationPath;
                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }
        }
        private void BrowseProjectLocation(int a)
        {
            string msg = string.Empty;
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                msg = "系统配置文件";
                sfd.Filter = msg + "(*.xml)|*.xml";
                sfd.DefaultExt = ".xml";
                sfd.Title = "新建工程";
                sfd.FileName = ProjectName;
                if (!Directory.Exists(ProjectLocationPath))
                {
                    Directory.CreateDirectory(ProjectLocationPath);
                }
                sfd.InitialDirectory = ProjectLocationPath;
                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                int lastIndex = sfd.FileName.LastIndexOf('\\');
                ProjectLocationPath = sfd.FileName.Substring(0, lastIndex + 1);
                ProjectName = sfd.FileName.Substring(lastIndex + 1);
            }
        }
        #endregion

        #region 私有函数

        private void OnScannerCofigCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (ScannerCofigCollection.Count == 1)
                {
                    SelectedScannerConfigInfo = null;
                }
                else
                {
                    SelectedScannerConfigInfo = (ScannerCofigInfo)e.NewItems[0];
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (ScannerCofigCollection != null)
                {
                    if(ScannerCofigCollection.Count>1)
                    {
                       SelectedScannerConfigInfo = ScannerCofigCollection[0];
                    }
                    else if(ScannerCofigCollection.Count==1)
                    {
                        SelectedScannerConfigInfo=null;
                    }
                }
            }

        }
        #endregion
    }
}