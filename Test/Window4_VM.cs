using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using Nova.SmartLCT.UI;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using Nova.LCT.GigabitSystem.Common;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Collections.Specialized;

namespace Test
{
    public class Window4_VM : ViewModelBase
    {
        public ObservableCollection<VirtualLightType> LightSequence
        {
            get { return _lightSequence; }
            set
            {
                _lightSequence = value;
                RaisePropertyChanged("LightSequence");
            }
        }
        private ObservableCollection<VirtualLightType> _lightSequence = null;

        public BitmapImage RedImage
        {
            get { return _redImgage; }
            set
            {
                _redImgage = value;
                RaisePropertyChanged("RedImage");
            }
        }
        private BitmapImage _redImgage = null;

        public BitmapImage GreenImage
        {
            get { return _greenImgage; }
            set
            {
                _greenImgage = value;
                RaisePropertyChanged("GreenImage");
            }
        }
        private BitmapImage _greenImgage = null;

        public BitmapImage BlueImage
        {
            get { return _blueImgage; }
            set
            {
                _blueImgage = value;
                RaisePropertyChanged("BlueImage");
            }
        }
        private BitmapImage _blueImgage = null;

        public BitmapImage VRedImage
        {
            get { return _vRedImgage; }
            set
            {
                _vRedImgage = value;
                RaisePropertyChanged("VRedImage");
            }
        }
        private BitmapImage _vRedImgage = null;

        public RelayCommand CmdExchanged
        {
            get;
            private set;
        }

        public RelayCommand CmdLightChanged
        {
            get;
            private set;
        }

        public VirtualModeType VirtualMode
        {
            get { return _virtualMode; }
            set
            {
                _virtualMode = value;
                SetPreviewVirtualMode();
                RaisePropertyChanged("VirtualMode");
            }
        }
        private VirtualModeType _virtualMode = VirtualModeType.Led3;

        public ObservableCollection<VirtualLightType> PreviewFirstLightSequence
        {
            get { return _previewFirstLightSequence; }
            set
            {
                _previewFirstLightSequence = value;
                RaisePropertyChanged("PreviewFirstLightSequence");
            }
        }
        private ObservableCollection<VirtualLightType> _previewFirstLightSequence = null;

        public ObservableCollection<VirtualLightType> PreviewSecondLightSequence
        {
            get { return _previewSecondLightSequence; }
            set
            {
                _previewSecondLightSequence = value;
                RaisePropertyChanged("PreviewSecondLightSequence");
            }
        }
        private ObservableCollection<VirtualLightType> _previewSecondLightSequence = null;

        public VirtualModeType PreviewFirstVirtualMode
        {
            get { return _previewFirstVirtualMode; }
            set
            {
                _previewFirstVirtualMode = value;
                RaisePropertyChanged("PreviewFirstVirtualMode");
            }
        }
        private VirtualModeType _previewFirstVirtualMode = VirtualModeType.Led31;

        public VirtualModeType PreviewSecondVirtualMode
        {
            get { return _previewSecondVirtualMode; }
            set
            {
                _previewSecondVirtualMode = value;
                RaisePropertyChanged("PreviewSecondVirtualMode");
            }
        }
        private VirtualModeType _previewSecondVirtualMode = VirtualModeType.Led3;

        public Nullable<bool> CheckedStaus
        {
            get { return _checkedStatus; }
            set
            {
                _checkedStatus = value;
                if (value != null)
                {
                    Debug.WriteLine(value.ToString());
                }
                else
                {
                    Debug.WriteLine("中间态");
                }
                RaisePropertyChanged("CheckedStaus");
            }
        }
        
        private bool? _checkedStatus = true;

        public Window4_VM()
        {
            if (!this.IsInDesignMode)
            {
                CmdExchanged = new RelayCommand(OnCmdExchanged);
                CmdLightChanged = new RelayCommand(squence_CollectionChanged);

                BitmapImage img = new System.Windows.Media.Imaging.BitmapImage(new Uri(".\\VirtualLed\\Red.png", UriKind.Relative));
                int x = img.PixelWidth; ;
                RedImage = img;

                img = new System.Windows.Media.Imaging.BitmapImage(new Uri(".\\VirtualLed\\Green.png", UriKind.Relative));
                x = img.PixelWidth; ;
                GreenImage = img;

                img = new System.Windows.Media.Imaging.BitmapImage(new Uri(".\\VirtualLed\\Blue.png", UriKind.Relative));
                x = img.PixelWidth; ;
                BlueImage = img;

                img = new System.Windows.Media.Imaging.BitmapImage(new Uri(".\\VirtualLed\\VRed.png", UriKind.Relative));
                x = img.PixelWidth; ;
                VRedImage = img;

                ObservableCollection<VirtualLightType> squence = new ObservableCollection<VirtualLightType>();
                squence.Add(VirtualLightType.VRed);
                squence.Add(VirtualLightType.Blue);
                squence.Add(VirtualLightType.Red);
                squence.Add(VirtualLightType.Green);
                this.LightSequence = squence;

                //SetPreviewVirtualMode();
                SetPreviewVirtualSequence();
            }
        }
        private void squence_CollectionChanged()
        {
            SetPreviewVirtualSequence();
        }


        private int index = 0;
        private void OnCmdExchanged()
        {
            //VirtualEditData temp = SecondData;
            //SecondData = FirstData;
            //FirstData = temp;
            int left = index % 3;
            if (left == 0)
            {
                VirtualMode = VirtualModeType.Led3;
            }
            else if (left == 1)
            {
                VirtualMode = VirtualModeType.Led31;
            }
            else
            {
                VirtualMode = VirtualModeType.Led4Mode1;
            }
            index++;
        }

        private void SetPreviewVirtualMode()
        {
            PreviewFirstVirtualMode = VirtualMode;
            if (VirtualMode == VirtualModeType.Led3)
            {
                PreviewSecondVirtualMode = VirtualModeType.Led31;
            }
            else if (VirtualMode == VirtualModeType.Led31)
            {
                PreviewSecondVirtualMode = VirtualModeType.Led3;
            }
            else if (VirtualMode == VirtualModeType.Led4Mode1)
            {
                PreviewSecondVirtualMode = VirtualModeType.Led4Mode1;
            }
        }

        private void SetPreviewVirtualSequence()
        {
            #region 统一
            ObservableCollection<VirtualLightType> squence = null;

            squence = new ObservableCollection<VirtualLightType>();
            squence.Add(LightSequence[0]);
            squence.Add(LightSequence[1]);
            squence.Add(LightSequence[2]);
            squence.Add(LightSequence[3]);
            PreviewFirstLightSequence = squence;
            #endregion

            #region Led3
            if (VirtualMode == VirtualModeType.Led31)
            {
                ObservableCollection<VirtualLightType> squence1 = null;
                squence1 = new ObservableCollection<VirtualLightType>();
                squence1.Add(LightSequence[2]);
                squence1.Add(LightSequence[3]);
                squence1.Add(LightSequence[1]);
                squence1.Add(LightSequence[0]);
                PreviewSecondLightSequence = squence1;
            }
            #endregion

            #region Led31
            if (VirtualMode == VirtualModeType.Led3)
            {
                ObservableCollection<VirtualLightType> squence1 = null;
                squence1 = new ObservableCollection<VirtualLightType>();
                squence1.Add(LightSequence[3]);
                squence1.Add(LightSequence[2]);
                squence1.Add(LightSequence[0]);
                squence1.Add(LightSequence[1]);
                PreviewSecondLightSequence = squence1;
            }
            #endregion

            #region Led4Mode1
            if (VirtualMode == VirtualModeType.Led4Mode1)
            {
                ObservableCollection<VirtualLightType> squence1 = null;
                squence1 = new ObservableCollection<VirtualLightType>();
                squence1.Add(LightSequence[0]);
                squence1.Add(LightSequence[1]);
                squence1.Add(LightSequence[2]);
                squence1.Add(LightSequence[3]);
                PreviewSecondLightSequence = squence1;
            }
            #endregion
        }
    }
}
