using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nova.SmartLCT.Interface;
using System.Windows.Media.Imaging;
using Nova.LCT.GigabitSystem.Common;
using System.Windows.Data;
using System.Globalization;

namespace Nova.SmartLCT.UI
{
    public enum VirtualLightType { Red, Green, Blue, VRed }
    public class VirtualEditData : NotificationST
    {
        public VirtualLightType LightType
        {
            get { return _lightType; }
            set
            {
                _lightType = value;
                NotifyPropertyChanged(GetPropertyName(o => this.LightType));
            }
        }
        private VirtualLightType _lightType = VirtualLightType.Red;

        public BitmapImage ImageDisplay
        {
            get { return _imageDisplay; }
            set
            {
                _imageDisplay = value;
                NotifyPropertyChanged(GetPropertyName(o => this.ImageDisplay));
            }
        }
        private BitmapImage _imageDisplay = null;

        public bool CopyTo(object data)
        {
            if (!(data is VirtualEditData))
            {
                return false;
            }

            VirtualEditData temp = (VirtualEditData)data;
            temp.LightType = this.LightType;
            temp.ImageDisplay = this.ImageDisplay;
            return true;
        }
    }
    public class VirtualModeTransformParams
    {
        public static VirtualModeType Disable
        {
            get { return VirtualModeType.Disable; }
        }
        public static VirtualModeType Led3
        {
            get { return VirtualModeType.Led3;}
        }
        public static VirtualModeType Led31
        {
            get { return VirtualModeType.Led31; }
        }
        public static VirtualModeType Led4Mode1
        {
            get { return VirtualModeType.Led4Mode1; }
        }
        public static VirtualModeType Led4Mode2
        {
            get { return VirtualModeType.Led4Mode2; }
        }
        public static VirtualModeType Unknown
        {
            get { return VirtualModeType.Unknown; }
        }
    }

    public class VirtualMapSequenceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            int data = (int)value;

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return null;

            bool useValue = (bool)value;
            string targetValue = parameter.ToString();
            if (useValue)
                return Enum.Parse(targetType, targetValue);

            return null;
        }
    }
}
