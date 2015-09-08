using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Nova.LCT.GigabitSystem.Common;
using Nova.SmartLCT.Database;
using Nova.SmartLCT.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmartLCT
{
    public class Win_LanguageSelect_VM : SmartLCTViewModeBase
    {

        #region 界面属性
        public ObservableCollection<ComboBoxDataSet> LanguageList
        {
            get
            {
                return _languageList;
            }
            set
            {
                _languageList = value;
                RaisePropertyChanged("LanguageList");
            }
        }
        private ObservableCollection<ComboBoxDataSet> _languageList = new ObservableCollection<ComboBoxDataSet>();

        public string SelectedLangFlag
        {
            get
            {
                return _selectedLangFlag;
            }
            set
            {
                _selectedLangFlag = value;
                RaisePropertyChanged("SelectedLangFlag");
            }
        }
        private string _selectedLangFlag = "";
        #endregion

        #region 命令
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
        #endregion

        public Win_LanguageSelect_VM()
        {
            string title = "";
            CommonStaticMethod.GetLanguageString("语言", "Lang_LanguangSelect_Title", out title);
            this.WindowRealTitle = title;
            List<string> langFlagList = new List<string>();
            langFlagList.Add("zh-cn");
            langFlagList.Add("en");

            for (int i = 0; i < langFlagList.Count; i++)
            {
                string flag = langFlagList[i];
                string name = GetCultureInfo(flag);
                if (!string.IsNullOrEmpty(name))
                {
                    LanguageList.Add(new ComboBoxDataSet(name, flag));
                }
            }

            SelectedLangFlag = _globalParams.AppConfig.LangFlag;

            CmdOK = new RelayCommand(OnCmdOK);
            CmdCancel = new RelayCommand(OnCmdCancel);            
        }

        private void OnCmdOK()
        {
            if (string.IsNullOrEmpty(SelectedLangFlag))
            {
                string msg = "";
                CommonStaticMethod.GetLanguageString("", "", out msg);
                ShowGlobalDialogMessage("请选择要更改的语言！", System.Windows.MessageBoxImage.Error);
                return;
            }
            if (SelectedLangFlag == _globalParams.AppConfig.LangFlag)
            {
                Messenger.Default.Send<string>("", MsgToken.MSG_CLOSELANGUAGE_SEL);
                return;
            }
            string reMsg = "";
            CommonStaticMethod.GetLanguageString("更新语言需要重新启动软件，是否重新启动软件？", "Lang_LanguangSelect_NeedRestartForLang", out reMsg);
            if (ShowQuestionMessage(reMsg, System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) != System.Windows.MessageBoxResult.Yes)
            {
                return;
            }

            SQLiteAccessor accessor = SQLiteAccessor.Instance;
            _globalParams.AppConfig.LangFlag = SelectedLangFlag;
            if (accessor != null)
            {
                accessor.SaveAppConfig(_globalParams.AppConfig);
            }

            Application.Restart();

            System.Environment.Exit(0);
        }
        private void OnCmdCancel()
        {
            Messenger.Default.Send<string>("", MsgToken.MSG_CLOSELANGUAGE_SEL);
        }

        private string GetCultureInfo(string flag)
        {
            try
            {
                CultureInfo info = new CultureInfo(flag);
                return info.NativeName;
            }
            catch
            {
                return "";
            }

        }
    }
}
