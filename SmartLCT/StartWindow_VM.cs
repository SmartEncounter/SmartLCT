using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nova.SmartLCT.Interface;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Diagnostics;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Collections.Specialized;

namespace SmartLCT
{
    public class StartWindow_VM : SmartLCTViewModeBase
    {
        #region 属性
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
        public ObservableCollection<ProductInfo> ProductInfoCollection
        {
            get 
            { 
                return _productInfoCollection; 
            }
            set 
            {
                if (_productInfoCollection != null)
                {
                    _productInfoCollection.CollectionChanged -= new NotifyCollectionChangedEventHandler(OnProductInfoCollectionChange);

                }
                _productInfoCollection = value;
                if (_productInfoCollection != null)
                {
                    _productInfoCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(OnProductInfoCollectionChange);

                }
                NotifyPropertyChanged(GetPropertyName(o => this.ProductInfoCollection));
            }
        }
        private void OnProductInfoCollectionChange(object sender, NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                #region 添加
                #endregion
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                #region 移除

                #endregion
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                #region 修改
                #endregion
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                #region 重置
                #endregion
            }
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {
                #region 移动
                SetProductInfoSize(ProductInfoCollection, 1);
                #endregion

            }
            NotifyPropertyChanged(GetPropertyName(o => this.ProductInfoCollection));


        }

        private ObservableCollection<ProductInfo> _productInfoCollection = new ObservableCollection<ProductInfo>();
        public string NovaStarPicPath
        {
            get { return _novaStarPicPath; }
            set
            {
                _novaStarPicPath = value;
                NotifyPropertyChanged(GetPropertyName(o => this.NovaStarPicPath));
            }
        }
        private string _novaStarPicPath = "";
        public bool IsOpenProject
        {
            get { return _isOpenProject; }
            set
            {
                _isOpenProject = value;
                OnOpenProject();
                NotifyPropertyChanged(GetPropertyName(o => this.IsOpenProject));
            }
        }
        private bool _isOpenProject = false;
        public bool IsNewProject
        {
            get { return _isNewProject; }
            set
            {
                _isNewProject = value;
                OnNewProject();
                NotifyPropertyChanged(GetPropertyName(o => this.IsNewProject));
            }
        }
        private bool _isNewProject = false;
        public bool IsNewEmptyProject
        {
            get { return _isNewEmptyProject; }
            set
            {
                _isNewEmptyProject = value;
                OnNewEmptyProject();
                NotifyPropertyChanged(GetPropertyName(o => this.IsNewEmptyProject));
            }
        }
        private bool _isNewEmptyProject = false;

        public bool IsOpenLangSel
        {
            get
            {
                return _isOpenLangSel;
            }
            set
            {
                _isOpenLangSel = value;
                OnOpenLangSel();
                NotifyPropertyChanged(GetPropertyName(o => this.IsOpenLangSel));
            }
        }
        private bool _isOpenLangSel = false;

        public bool IsOpenBrightAdjust
        {
            get
            {
                return _isOpenBrightAdjust;
            }
            set
            {
                _isOpenBrightAdjust = value;
                OnOpenBrightAdjust();
                NotifyPropertyChanged(GetPropertyName(o => this.IsOpenBrightAdjust));
            }
        }
        private bool _isOpenBrightAdjust = false;
        public ObservableCollection<ProjectInfo> RecentProjectCollection
        {
            get { return _recentProjectCollection; }
            set
            {
                _recentProjectCollection = value;
                NotifyPropertyChanged(GetPropertyName(o => this.RecentProjectCollection));
            }
        }
        private ObservableCollection<ProjectInfo> _recentProjectCollection = new ObservableCollection<ProjectInfo>();
        public ProjectInfo SelectedRecentProjectFile
        {
            get { return _selectedRecentProjectFile; }
            set
            {
                _selectedRecentProjectFile = value;
                //OnSelectedRecentProjectFile(value);
                NotifyPropertyChanged(GetPropertyName(o => this.SelectedRecentProjectFile));
            }
        }
        private ProjectInfo _selectedRecentProjectFile = new ProjectInfo();
        public int SelectedProductIndex
        {
            get { return _selectedProductIndex; }
            set
            {
                _selectedProductIndex = value;
                NotifyPropertyChanged(GetPropertyName(o => this.SelectedProductIndex));
            }
        }
        private int _selectedProductIndex = -1;
        public DispatcherTimer ProductTimer
        {
            get { return _productTimer; }
            set 
            { 
                _productTimer = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ProductTimer));
            }
        }
        private DispatcherTimer _productTimer = new DispatcherTimer();
        public bool IsMouseOver
        {
            get { return _isMouseOver; }
            set
            {
                _isMouseOver = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsMouseOver));
            }
        }
        private bool _isMouseOver = false;
        #endregion

        #region 命令
        public RelayCommand CmdNewWizard
        {
            get;
            private set;
        }
        public RelayCommand CmdNewEmptyProject
        {
            get;
            private set;
        }
        public RelayCommand CmdOpenProject
        {
            get;
            private set;
        }
        public RelayCommand CmdOpenRecentProject
        {
            get;
            private set;
        }
        public RelayCommand CmdProductForward
        {
            get;
            private set;
        }
        public RelayCommand CmdProductBack
        {
            get;
            private set;
        }
        public RelayCommand CmdMouseMoveWithProductList
        {
            get;
            private set;
        }
        public RelayCommand CmdGotFocusWithProductList
        {
            get;
            private set;
        }
        public RelayCommand CmdLostFocusWithProductList
        {
            get;
            private set;
        }
        #endregion

        #region 构造函数
        public StartWindow_VM()
        {
            if (!this.IsInDesignMode)
            {
                //加载图片配置文件信息
                ObservableCollection<ProductInfo> productInfoCollection;
                //  productInfoCollection.Add(new ProductInfo("E:\\VTS\\SmartLCT V1.1 S2T1\\SmartLCT V1.0 S2 T2代码\\StartUpWindow\\bin\\Release\\Resource\\Config\\productInfo\\ProductPic\\MBI5024.ico", "我们的的的"));
                LoadProductInfoFile(out productInfoCollection);
                SetProductInfoSize(productInfoCollection, 1);
                ProductInfoCollection = productInfoCollection;

                string novaStarPicPath;
                LoadNovaStarPicFile(out novaStarPicPath);
                NovaStarPicPath = novaStarPicPath;
                //最近打开的项目
                RecentProjectCollection = _globalParams.RecentProjectPaths;

                CmdNewWizard = new RelayCommand(OnNewProject);
                CmdNewEmptyProject = new RelayCommand(OnNewEmptyProject);
                CmdOpenProject = new RelayCommand(OnOpenProject);
                CmdOpenRecentProject = new RelayCommand(OnOpenRecentProject);
                CmdProductForward = new RelayCommand(OnProductForward);
                CmdProductBack = new RelayCommand(OnProductBack);
                //CmdMouseMoveWithProductList = new RelayCommand(OnMouseMoveWithProductList);
                CmdGotFocusWithProductList = new RelayCommand(OnGotFocusWithProductList);
                CmdLostFocusWithProductList = new RelayCommand(OnLostFocusWithProductList);
                _productTimer.Interval = new System.TimeSpan(0, 0, 3);
                _productTimer.Tick += new EventHandler(procductTimer_Tick);
                _productTimer.Start();
            }
        }
        #endregion

        #region 私有函数
        private void OnGotFocusWithProductList()
        {
            _productTimer.Stop();
            IsMouseOver = true;
        }
        private void OnLostFocusWithProductList()
        {
            _productTimer.Start();
            IsMouseOver = false;
        }
        private void procductTimer_Tick(object sender,EventArgs e)
        {
            OnProductBack();
        }
        private void OnProductForward()
        {
            ProductInfoCollection.Move(ProductInfoCollection.Count - 1,0);
        }
        private void OnProductBack()
        {
            ProductInfoCollection.Move(0,ProductInfoCollection.Count - 1);
        }
        private void OnOpenRecentProject()
        {
            OnSelectedRecentProjectFile(SelectedRecentProjectFile);
        }
        private void OnSelectedRecentProjectFile(ProjectInfo projectInfo)
        {
            //更新最近文件时，判断设计窗口是否存在，若不判断，则更新最近文件的同时会另开启一个设计界面
            for (int i = 0; i < Application.Current.Windows.Count; i++)
            {
                if (Application.Current.Windows[i] is MainWindow)
                {
                    return;
                }
            }
            ConfigData = new ConfigurationData();
            ConfigData.OperateType = OperateScreenType.CreateScreenAndOpenConfigFile;
            if (!File.Exists(projectInfo.ProjectPath))
            {
                string msg = "";
                string strRemove = "";
                CommonStaticMethod.GetLanguageString("无法打开该文件:", "Lang_StartWindow_CanOpenFile", out msg);
                msg += projectInfo.ProjectPath + "\n";
                CommonStaticMethod.GetLanguageString("是否从\"最近打开\"列表中移除？", "Lang_StartWindow_SureRemoveFile", out strRemove);
                msg += strRemove;
                MessageBoxResult result = ShowQuestionMessage(msg, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _globalParams.RecentProjectPaths.Remove(projectInfo);
                }
                return;
            }
            else
            {
                ConfigData.ProjectName = projectInfo.ProjectName;
                ConfigData.ProjectLocationPath = projectInfo.ProjectPath.Substring(0, projectInfo.ProjectPath.Length - projectInfo.ProjectName.Length - 1-SmartLCTViewModeBase.ProjectExtension.Length);
            }
            //隐藏当前窗口
            Messenger.Default.Send<ConfigurationData>(ConfigData, MsgToken.MSG_CLOSESTARTWINDOW);

        }
        private void OnOpenProject()
        {
            //打开项目
            //打开窗口
            ConfigData = new ConfigurationData();
            ConfigData.OperateType = OperateScreenType.CreateScreenAndOpenConfigFile;
            string defaultProjectPath = Function.GetDefaultCurrentProjectPath(_globalParams.RecentProjectPaths);
            string projectPath = Function.GetConfigFileName(defaultProjectPath);
           if (projectPath == "")
           {
               ConfigData = null;
           }
           else
           {
               string[] splitName = projectPath.Split('\\');
               foreach (string name in splitName)
               {
                   ConfigData.ProjectName = name;
               }
               ConfigData.ProjectName = ConfigData.ProjectName.Substring(0, ConfigData.ProjectName.Length - SmartLCTViewModeBase.ProjectExtension.Length);
               ConfigData.ProjectLocationPath = projectPath.Substring(0, projectPath.Length - ConfigData.ProjectName.Length-SmartLCTViewModeBase.ProjectExtension.Length-1);
           }
            //隐藏当前窗口
           Messenger.Default.Send<ConfigurationData>(ConfigData, MsgToken.MSG_CLOSESTARTWINDOW);

        }
        //private string GetConfigFileName()
        //{
        //    OpenFileDialog ofd = new OpenFileDialog();
        //    string configFile = string.Empty;
        //    string msg = "";
        //    CommonStaticMethod.GetLanguageString("配置文件", "Lang_SmartLCT_VM_ConfileFile", out msg);
        //    configFile = msg;
        //    string allFile = string.Empty;
        //    CommonStaticMethod.GetLanguageString("所有文件", "Lang_SmartLCT_VM_AllFile", out msg);
        //    allFile = msg;
        //    ofd.Filter = configFile + "(*.xml)|*.xml|" + allFile + "(*.*)|*.*";
        //    if (ofd.ShowDialog() != true)
        //    {
        //        return "";
        //    }
        //    else
        //    {
        //        return ofd.FileName;
        //    }
        //}
        private void OnNewProject()
        {
            //新建项目
            ConfigData=new ConfigurationData();
            ConfigData.OperateType=OperateScreenType.CreateScreen;
            NotificationMessageAction<ConfigurationData> nsa =
                new NotificationMessageAction<ConfigurationData>(this,ConfigData, MsgToken.MSG_SHOWGUIDETWO, NewProjectCallBack);
            Messenger.Default.Send(nsa, MsgToken.MSG_SHOWGUIDETWO);

        }
        private void NewProjectCallBack(ConfigurationData info)
        {
            Messenger.Default.Send<ConfigurationData>(info, MsgToken.MSG_CLOSESTARTWINDOW);
        }
        private bool LoadProductInfoFile(out ObservableCollection<ProductInfo> productInfoCollection)
        {
            productInfoCollection=new ObservableCollection<ProductInfo>();
            XmlSerializer xmls = null;
            StreamReader sr = null;
            XmlReader xmlReader = null;
            string msg = string.Empty;
            bool res = false;
            try
            {
                xmls = new XmlSerializer(typeof(ObservableCollection<ProductInfo>));
                sr = new StreamReader(SmartLCTViewModeBase.ReleasePath + "Resource\\Config\\productInfo\\productInfoConfig.xml");
                xmlReader = XmlReader.Create(sr);
                if (!xmls.CanDeserialize(xmlReader))
                {
                    res = false;
                    return res;
                }
                else
                {
                    productInfoCollection = (ObservableCollection<ProductInfo>)xmls.Deserialize(xmlReader);
                    for (int i = 0; i < productInfoCollection.Count; i++)
                    {
                        string picPath = SmartLCTViewModeBase.ReleasePath + productInfoCollection[i].PicPath;
                        if (File.Exists(picPath))
                        {
                            productInfoCollection[i].PicPath = picPath;
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
        private bool LoadNovaStarPicFile(out string novaStarPicPath)
        {
            novaStarPicPath = "";
            XmlSerializer xmls = null;
            StreamReader sr = null;
            XmlReader xmlReader = null;
            string msg = string.Empty;
            bool res = false;
            try
            {
                xmls = new XmlSerializer(typeof(string));
                sr = new StreamReader(SmartLCTViewModeBase.ReleasePath + "Resource\\Config\\productInfo\\novaStarPicConfig.xml");
                xmlReader = XmlReader.Create(sr);
                if (!xmls.CanDeserialize(xmlReader))
                {
                    res = false;
                    return res;
                }
                else
                {
                    novaStarPicPath = (string)xmls.Deserialize(xmlReader);
                    novaStarPicPath = SmartLCTViewModeBase.ReleasePath + novaStarPicPath;
                    if (!File.Exists(novaStarPicPath))
                    {
                        novaStarPicPath = "";
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
        private void OnNewEmptyProject()
        {
            ConfigData = new ConfigurationData();
            ConfigData.OperateType = OperateScreenType.CreateScreen;
            ConfigData.IsCreateEmptyProject = true;
            ConfigData.SelectedScannerConfigInfo = null;
            ConfigData.ProjectLocationPath = Function.GetDefaultCurrentProjectPath(_globalParams.RecentProjectPaths);
            ConfigData.ProjectName = Function.GetDefaultProjectName(SmartLCTViewModeBase.DefaultProjectMainName, ".xml", ConfigData.ProjectLocationPath);
            Messenger.Default.Send<ConfigurationData>(ConfigData, MsgToken.MSG_CLOSESTARTWINDOW);
        }
        private void OnOpenBrightAdjust()
        {
            Messenger.Default.Send<string>("", MsgToken.MSG_SHOWBRIGHTADJUST);
        }
        private void OnOpenLangSel()
        {
            Messenger.Default.Send<string>(_globalParams.AppConfig.LangFlag, MsgToken.MSG_SHOWLANGUAGE_SEL); 
        }
        private void SetProductInfoSize(ObservableCollection<ProductInfo> productInfoCollection,int selectedIndex)
        {
            for (int itemIndex = 0; itemIndex < productInfoCollection.Count; itemIndex++)
            {
                if (selectedIndex == itemIndex)
                {
                    productInfoCollection[itemIndex].Width = 350;
                    productInfoCollection[itemIndex].Height = 130;
                }
                else
                {
                    productInfoCollection[itemIndex].Width = 275;
                    productInfoCollection[itemIndex].Height = 100;
                }
            }
        }
        #endregion
    }
}
