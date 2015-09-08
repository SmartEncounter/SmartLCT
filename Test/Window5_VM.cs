using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using Nova.LCT.GigabitSystem.Common;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using Nova.SmartLCT.UI;
using System.Collections.ObjectModel;

namespace Test
{
    public class Window5_VM : ViewModelBase
    {
        public int VirtualMap
        {
            get { return _virtualMap; }
            set
            {
                _virtualMap = value;
                RaisePropertyChanged("VirtualMap");
            }
        }
        private int _virtualMap = 0xe4;

        public VirtualModeType VirtualMode
        {
            get { return _virtualMode; }
            set
            {
                _virtualMode = value;

                RaisePropertyChanged("VirtualMode");
            }
        }
        private VirtualModeType _virtualMode = VirtualModeType.Led3;

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

        public RelayCommand CmdTest
        {
            get;
            private set;
        }

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

        public Window5_VM()
        {
            if (!this.IsInDesignMode)
            {
                CmdTest = new RelayCommand(OnTest);
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

                //VirtualMap = 0x03;
                //VirtualMode = VirtualModeType.Led4Mode1;
                //ObservableCollection<VirtualLightType> squence = new ObservableCollection<VirtualLightType>();
                //squence.Add(VirtualLightType.VRed);
                //squence.Add(VirtualLightType.Blue);
                //squence.Add(VirtualLightType.Red);
                //squence.Add(VirtualLightType.Green);
                //this.LightSequence = squence;
                VirtualMap = 0xe4;
                VirtualMode = VirtualModeType.Led3;
            }
        }
        private void OnTest()
        {

            //VirtualMode = VirtualModeType.Led3;
            //VirtualMap = 0xd8;
            //VirtualMode = VirtualModeType.Led31;
            //VirtualMap = 0xB1;
            VirtualMode = VirtualModeType.Led31;
            VirtualMap = 177;
        }
    }
}
