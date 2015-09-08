using GalaSoft.MvvmLight;
using Nova.LCT.GigabitSystem.Common;
using GalaSoft.MvvmLight.Command;

namespace Nova.SmartLCT.UI
{
    public delegate void IsCheckedChangedDel(string Commport,bool isChecked);

    public class EquipmentTypeInfo : Nova.SmartLCT.Interface.NotificationST
    {
        public event IsCheckedChangedDel IsCheckedEvent;

        private void OnIsCheckedEvent(string Commport,bool isChecked)
        {
            if (IsCheckedEvent != null)
            {
                this.IsCheckedEvent(Commport,isChecked);
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
                    this.NotifyPropertyChanged(GetPropertyName(o => this.IsChecked));
                }
            }
        }

        private string _serialPort = string.Empty;
        public string SerialPort
        {
            get
            {
                return this._serialPort;
            }

            set
            {
                if (this._serialPort != value)
                {
                    this._serialPort = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.SerialPort));

                }
            }
        }

        private NSCardType _deviceType = NSCardType.Unknown;
        public NSCardType DeviceType
        {
            get
            {
                return this._deviceType;
            }

            set
            {
                if (this._deviceType != value)
                {
                    this._deviceType = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.DeviceType));

                }
            }
        }

        private int _portCount = 0;
        public int PortCount
        {
            get
            {
                return this._portCount;
            }

            set
            {
                if (this._portCount != value)
                {
                    this._portCount = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.PortCount));

                }
            }
        }

        private int _systermCountCount = 0;
        public int SystermCount
        {
            get
            {
                return this._systermCountCount;
            }

            set
            {
                if (this._systermCountCount != value)
                {
                    this._systermCountCount = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.SystermCount));

                }
            }
        }

        private bool _checkBoxEnable = false;
        public bool CheckBoxEnable
        {
            get
            {
                return this._checkBoxEnable;
            }

            set
            {
                if (this._checkBoxEnable != value)
                {
                    this._checkBoxEnable = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.CheckBoxEnable));
                }
            }
        }


        private bool _checkBoxVisible = false;
        public bool CheckBoxVisible
        {
            get
            {
                return this._checkBoxVisible;
            }

            set
            {
                if (this._checkBoxVisible != value)
                {
                    this._checkBoxVisible = value;
                    this.NotifyPropertyChanged(GetPropertyName(o => this.CheckBoxVisible));
                }
            }
        }

        public EquipmentTypeInfo()
        {
            CmdIsChecked = new RelayCommand<bool>(CheckBoxCheckChanged);
        }

        private void CheckBoxCheckChanged(bool isChanged)
        {
            OnIsCheckedEvent(SerialPort, IsChecked);
        }

        public RelayCommand<bool> CmdIsChecked
        {
            get;
            private set;
        }


        public bool CopyTo(object obj)
        {
            if (!(obj is EquipmentTypeInfo))
            {
                return false;
            }
            EquipmentTypeInfo temp = (EquipmentTypeInfo)obj;
            temp.IsChecked = this.IsChecked;
            temp.PortCount = this.PortCount;
            temp.SerialPort = this.SerialPort;
            temp.CheckBoxEnable = this.CheckBoxEnable;
            temp.CheckBoxVisible = this.CheckBoxVisible;
            temp.DeviceType = this.DeviceType;
            return true;
        }

        public object Clone()
        {
            EquipmentTypeInfo temp = new EquipmentTypeInfo();
            if (this.CopyTo(temp))
            {
                return temp;
            }
            else
            {
                return null;
            }
        }
    }
}