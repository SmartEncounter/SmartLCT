using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Nova.SmartLCT.UI
{
    public class ItemCollection : System.Collections.ObjectModel.ObservableCollection<DataGradViewItem>
    {
    }
    public delegate void CheckBoxChangedDel(string commPort,bool isChanged);
    public delegate void MaterCheckBoxChangedDel(string commPort, bool? isChanged);

    public class DataGradViewItem : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        public event CheckBoxChangedDel CheckBoxChangedEvent;

        private void OnCheckBoxChangedEvent(string commPort, bool isChanged)
        {
            if (this.CheckBoxChangedEvent != null)
            {
                this.CheckBoxChangedEvent(commPort,isChanged);
            }
        }
        public event MaterCheckBoxChangedDel MaterCheckBoxAllChangedEvent;
        private void OnMaterCheckBoxlChangedEvent(string commPort, bool? isChanged)
        {
            if (this.MaterCheckBoxAllChangedEvent != null)
            {
                this.MaterCheckBoxAllChangedEvent(commPort, isChanged);
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
                    this.OnPropertyChanged("IsChecked");
                    this.OnCheckBoxChangedEvent(SerialPort,IsChecked);
                }
            }
        }

        private string _SerialPort = string.Empty;

        public string SerialPort
        {
            get
            {
                return this._SerialPort;
            }

            set
            {
                if (this._SerialPort != value)
                {
                    this._SerialPort = value;
                    this.OnPropertyChanged("SerialPort");
                }
            }
        }

        private byte _SenderIndex = 0;

        public byte SenderIndex
        {
            get
            {
                return this._SenderIndex;
            }

            set
            {
                if (this._SenderIndex != value)
                {
                    this._SenderIndex = value;
                    this.OnPropertyChanged("SenderIndex");
                }
            }
        }

        private string _CardType = string.Empty;

        public string CardType
        {
            get
            {
                return this._CardType;
            }

            set
            {
                if (this._CardType != value)
                {
                    this._CardType = value;
                    this.OnPropertyChanged("CardType");
                }
            }
        }

        private string _Reslution = string.Empty;

        public string Reslution
        {
            get
            {
                return this._Reslution;
            }

            set
            {
                if (this._Reslution != value)
                {
                    this._Reslution = value;
                    this.OnPropertyChanged("Reslution");
                }
            }
        }

        private double _RefreshRate = 0;

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
                    this.OnPropertyChanged("RefreshRate");
                }
            }
        }

        private bool? _IsSelectedAll =false;

        public bool? IsSelectedAll
        {
            get
            {
                return this._IsSelectedAll;
            }

            set
            {
                if (this._IsSelectedAll != value)
                {
                    this._IsSelectedAll = value;
                    this.OnMaterCheckBoxlChangedEvent(SerialPort, IsSelectedAll);
                    this.OnPropertyChanged("IsSelectedAll");
                }
            }
        }


    }
}