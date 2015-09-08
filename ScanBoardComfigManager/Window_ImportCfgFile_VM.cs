using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using Nova.SmartLCT.Interface;
using Nova.SmartLCT.UI;
using Nova.LCT.GigabitSystem.Common;

namespace Nova.SmartLCT.UI
{

    public class Window_ImportCfgFile_VM : SmartLCTViewModeBase
    {
        private NotificationMessageAction<ObservableCollection<DataGradItemInfo>> _nsaEdit = null;
        public ObservableCollection<DataGradItemInfo> ImportDataGradItemInfoList
        {
            get { return _importDataList; }
            set
            {
                _importDataList = value;
                RaisePropertyChanged(GetPropertyName(o => this.ImportDataGradItemInfoList));
            }
        }
        private ObservableCollection<DataGradItemInfo> _importDataList = new ObservableCollection<DataGradItemInfo>();

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
    

        public Window_ImportCfgFile_VM()
        {
            CmdButton_OK = new RelayCommand(OnCmdButton_Ok);
            CmdButton_Cancel = new RelayCommand(OnCmdButton_Cancel);

        }

        public void OnCmdButton_Cancel()
        {
            ImportDataGradItemInfoList.Clear();
            Messenger.Default.Send(MsgToken.MSG_INPORTFILE_CANCEL, MsgToken.MSG_INPORTFILE_CLOSE);
        }
        private void OnCmdButton_Ok()
        {
            if (_nsaEdit != null)
            {
                _nsaEdit.Execute(ImportDataGradItemInfoList);
                Messenger.Default.Send(MsgToken.MSG_INPORTFILE_OK, MsgToken.MSG_INPORTFILE_CLOSE);
            }
        }

        public void Initializes(NotificationMessageAction<ObservableCollection<DataGradItemInfo>> nsa)
        {
            _nsaEdit = nsa;
            ImportDataGradItemInfoList = (ObservableCollection<DataGradItemInfo>)nsa.Target;
            ScanBoardConfigManager_VM vm = (ScanBoardConfigManager_VM)nsa.Sender;
            ObservableCollection<DataGradItemInfo> oldData = vm.DataGradItemInfoList;

            for (int i = 0; i < ImportDataGradItemInfoList.Count; i++)
            {
                string msgAdd = "";
                CommonStaticMethod.GetLanguageString("添加", "Lang_ScanBoardConfigManager_Add", out msgAdd);

                string msgNoAdd = "";
                CommonStaticMethod.GetLanguageString("不添加", "Lang_ScanBoardConfigManager_NoAdd", out msgNoAdd);

                string msgRePlace = "";
                CommonStaticMethod.GetLanguageString("替换", "Lang_ScanBoardConfigManager_Replace", out msgRePlace);


                if (oldData.Count <= 0)
                {
                    ImportDataGradItemInfoList[i].DataHandleWay = HandleWay.Add;
                    ImportDataGradItemInfoList[i].ComBoxDataContext.Clear();
                    ImportDataGradItemInfoList[i].DataHandelSatate = DataSatate.Operability;
                    ImportDataGradItemInfoList[i].ComBoxDataContext.Add(new HandleWayInfo() { DisplayName = msgAdd, EnumValue = HandleWay.Add });
                    ImportDataGradItemInfoList[i].ComBoxDataContext.Add(new HandleWayInfo() { DisplayName = msgNoAdd, EnumValue = HandleWay.NoAdd });
                }
                else
                {
                    for (int j = 0; j < oldData.Count; j++)
                    {
                        if (oldData[j].ScanBoardName == ImportDataGradItemInfoList[i].ScanBoardName)
                        {
                            ImportDataGradItemInfoList[i].ComBoxDataContext.Clear();
                            ImportDataGradItemInfoList[i].DataHandleWay = HandleWay.Replace;
                            ImportDataGradItemInfoList[i].DataHandelSatate = DataSatate.ExistFile;
                            ImportDataGradItemInfoList[i].ComBoxDataContext.Add(new HandleWayInfo() { DisplayName = msgRePlace, EnumValue = HandleWay.Replace });
                            ImportDataGradItemInfoList[i].ComBoxDataContext.Add(new HandleWayInfo() { DisplayName = msgNoAdd, EnumValue = HandleWay.NoAdd });
                            break;
                        }
                        else
                        {
                            ImportDataGradItemInfoList[i].ComBoxDataContext.Clear();
                            ImportDataGradItemInfoList[i].DataHandleWay = HandleWay.Add;
                            ImportDataGradItemInfoList[i].DataHandelSatate = DataSatate.Operability;
                            ImportDataGradItemInfoList[i].ComBoxDataContext.Add(new HandleWayInfo() { DisplayName = msgAdd, EnumValue = HandleWay.Add });
                            ImportDataGradItemInfoList[i].ComBoxDataContext.Add(new HandleWayInfo() { DisplayName = msgNoAdd, EnumValue = HandleWay.NoAdd });
                        }
                    }
                }

                if (ImportDataGradItemInfoList != null &&
                    ImportDataGradItemInfoList.Count > 0)
                {
                    SelectedValue = ImportDataGradItemInfoList[0];
                }
            }
        }
    }


}