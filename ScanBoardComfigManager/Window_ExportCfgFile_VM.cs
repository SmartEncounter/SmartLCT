using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using Nova.SmartLCT.Interface;

namespace Nova.SmartLCT.UI
{
    public class Window_ExportCfgFile_VM : SmartLCTViewModeBase
    {

        #region 界面属性
        private SaveFileDialogData _saveFile = new SaveFileDialogData();
        public SaveFileDialogData SaveFile
        {
            get { return _saveFile; }
            set
            {
                _saveFile = value;
                RaisePropertyChanged(GetPropertyName(o => this.SaveFile));

            }
        }

        
        private NotificationMessageAction<ObservableCollection<DataGradItemInfo>> _nsaEdit = null;
        public ObservableCollection<DataGradItemInfo> ExportDataGradItemInfoList
        {
            get { return _exportDataList; }
            set
            {
                _exportDataList = value;
                RaisePropertyChanged("ExportDataGradItemInfoList");
            }
        }
        private ObservableCollection<DataGradItemInfo> _exportDataList = new ObservableCollection<DataGradItemInfo>();

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
        #region 命令
        public RelayCommand CmdButton_OK
        {
            get;
            private set;
        }
        public RelayCommand CmdButton_Cancel
        {
            get;
            private set;
        }
        #endregion

        public Window_ExportCfgFile_VM()
        {
            CmdButton_OK = new RelayCommand(OnCmdButton_Ok);
            CmdButton_Cancel = new RelayCommand(OnCmdButton_Cancel);
        }

        public void OnCmdButton_Cancel()
        {
            Messenger.Default.Send(MsgToken.MSG_EXPORTFILE_CANCEL, MsgToken.MSG_EXPORTFILE_CLOSE);
        }
        private void OnCmdButton_Ok()
        {
            if (SaveFile.IsCheckedOK &&
                SaveFile.SaveFileName!=string.Empty)
            {
                for (int i = 0; i < ExportDataGradItemInfoList.Count; i++)
                {
                    ExportDataGradItemInfoList[i].SaveFilePath = SaveFile.SaveFileName + "\\";
                }
            }
            else
            {
                return;
            }
            if (_nsaEdit != null)
            {
                _nsaEdit.Execute(ExportDataGradItemInfoList);
                Messenger.Default.Send(MsgToken.MSG_EXPORTFILE_OK, MsgToken.MSG_EXPORTFILE_CLOSE);
            }
        }
        public void Initializes(NotificationMessageAction<ObservableCollection<DataGradItemInfo>> nsa)
        {
            _nsaEdit = nsa;
            ExportDataGradItemInfoList = (ObservableCollection<DataGradItemInfo>)nsa.Target;
            //_win_RemoveRow.Initializes(nsa);
            if (ExportDataGradItemInfoList != null &&
                ExportDataGradItemInfoList.Count > 0)
            {
                SelectedValue = ExportDataGradItemInfoList[0];
            }
        }
    }
}