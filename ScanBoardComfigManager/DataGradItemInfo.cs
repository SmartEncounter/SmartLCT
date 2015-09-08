using GalaSoft.MvvmLight;
using System.Collections.Generic;
using Nova.SmartLCT.Interface;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Nova.LCT.GigabitSystem.Common;

namespace Nova.SmartLCT.UI
{
    public enum DataSatate { Operability, ExistFile, None };

    public enum HandleWay { Add, Replace, NoAdd ,None};

    public delegate void IsCheckedDel(bool isChecked);
    public class DataGradItemInfo : Nova.SmartLCT.Interface.NotificationST
    {
        public event IsCheckedDel IsCheckedEvent;

        private void OnIsCheckedEvent(bool isChecked)
        {
            if (IsCheckedEvent != null)
            {
                this.IsCheckedEvent(isChecked);
            }
        }

        private bool _IsChecked = false;
        public bool IsChecked
        {
            get
            {
                return this._IsChecked;
            }

            set
            {
                if (this._IsChecked != value)
                {
                    this._IsChecked = value;
                    OnIsCheckedEvent(IsChecked);
                    this.NotifyPropertyChanged(GetPropertyName(o => this.IsChecked));
                }
            }
        }

        private string _scanBoardName= string.Empty;
        public string ScanBoardName
        {
            get
            {
                return this._scanBoardName;
            }

            set
            {
                if (this._scanBoardName != value)
                {
                    this._scanBoardName = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.ScanBoardName));

                }
            }
        }

        private string _scanBoardSize = string.Empty;
        public string ScanBoardSize
        {
            get
            {
                return this._scanBoardSize;
            }

            set
            {
                if (this._scanBoardSize != value)
                {
                    this._scanBoardSize = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.ScanBoardSize));

                }
            }
        }

        private ChipType _chipType = ChipType.Unknown;
        public ChipType ChipType
        {
            get
            {
                return this._chipType;
            }

            set
            {
                if (this._chipType != value)
                {
                    this._chipType = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.ChipType));

                }
            }
        }

        private string _cascadeType = string.Empty;
        public string CascadeType
        {
            get
            {
                return this._cascadeType;
            }

            set
            {
                if (this._cascadeType != value)
                {
                    this._cascadeType = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.CascadeType));

                }
            }
        }

        private DataSatate _dataHandelSatate = DataSatate.None;
        public DataSatate DataHandelSatate
        {
            get
            {
                return this._dataHandelSatate;
            }

            set
            {
                if (this._dataHandelSatate != value)
                {
                    this._dataHandelSatate = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.DataHandelSatate));

                }
            }
        }

        private HandleWay _dataHandleWay = HandleWay.None;
        public HandleWay DataHandleWay
        {
            get
            {
                return this._dataHandleWay;
            }

            set
            {
                if (this._dataHandleWay != value)
                {
                    this._dataHandleWay = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.DataHandleWay));

                }
            }
        }

        private string _fileName = string.Empty;
        public string FileName
        {
            get
            {
                return this._fileName;
            }

            set
            {
                if (this._fileName != value)
                {
                    this._fileName = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.FileName));

                }
            }
        }

        private string _saveFilePath = string.Empty;
        public string SaveFilePath
        {
            get
            {
                return this._saveFilePath;
            }

            set
            {
                if (this._saveFilePath != value)
                {
                    this._saveFilePath = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.SaveFilePath));

                }
            }
        }

        private ObservableCollection<HandleWayInfo> _comBoxDataContext =new  ObservableCollection<HandleWayInfo>();
        public ObservableCollection<HandleWayInfo> ComBoxDataContext
        {
            get
            {
                return this._comBoxDataContext;
            }

            set
            {
                if (this._comBoxDataContext != value)
                {
                    this._comBoxDataContext = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.ComBoxDataContext));

                }
            }
        }

        public DataGradItemInfo()
        {
           
        }
    }

    public class SaveFileDialogData : Nova.SmartLCT.Interface.NotificationST
    {
        private bool _isCheckedOK = false;

        public bool IsCheckedOK
        {
            get
            {
                return this._isCheckedOK;
            }

            set
            {
                if (this._isCheckedOK != value)
                {
                    this._isCheckedOK = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.IsCheckedOK));
                }
            }
        }

        private string _saveFileName = string.Empty;

        public string SaveFileName
        {
            get
            {
                return this._saveFileName;
            }

            set
            {
                if (this._saveFileName != value)
                {
                    this._saveFileName = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.SaveFileName));

                }
            }
        }
    }

    public class OpenFileDialogData :Nova.SmartLCT.Interface.NotificationST
    {
        private bool _isCheckedOK = false;

        public bool IsCheckedOK
        {
            get
            {
                return this._isCheckedOK;
            }

            set
            {
                if (this._isCheckedOK != value)
                {
                    this._isCheckedOK = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.IsCheckedOK));

                }
            }
        }

        private bool _isMultiselect = false;

        public bool IsMultiselect
        {
            get
            {
                return this._isMultiselect;
            }

            set
            {
                if (this._isMultiselect != value)
                {
                    this._isMultiselect = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.IsMultiselect));

                }
            }
        }

        private List<string> _openFileNameList = new List<string>();

        public List<string> OpenFileNameList
        {
            get
            {
                return this._openFileNameList;
            }

            set
            {
                if (this._openFileNameList != value)
                {
                    this._openFileNameList = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.OpenFileNameList));

                }
            }
        }

        private string _fileFilter = string.Empty;

        public string FileFilter
        {
            get
            {
                return this._fileFilter;
            }

            set
            {
                if (this._fileFilter != value)
                {
                    this._fileFilter = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.FileFilter));

                }
            }
        }

        private string _openFileName = string.Empty;

        public string OpenFileName
        {
            get
            {
                return this._openFileName;
            }

            set
            {
                if (this._openFileName != value)
                {
                    this._openFileName = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.OpenFileName));

                }
            }
        }
    }
    public class HandleWayInfo : Nova.SmartLCT.Interface.NotificationST
    {
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                this.NotifyPropertyChanged(GetPropertyName(o => this.DisplayName));

            }
        }
        public string _displayName = string.Empty;

        public HandleWay EnumValue
        {
            get { return _enumValue; }
            set
            {
                _enumValue = value;
                this.NotifyPropertyChanged(GetPropertyName(o => this.EnumValue));

            }
        }
        public HandleWay _enumValue = HandleWay.None;
    }
}