using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nova.SmartLCT.Interface;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using Nova.LCT.GigabitSystem.Common;
using Nova.LCT.GigabitSystem.HWConfigAccessor;
using System.Windows.Controls;
using System.Globalization;

namespace Nova.SmartLCT.UI
{
    public enum ScreenQuality { Soft, Enhance }
    public enum GammaABMode { GammaA, GammaB }

    public enum BrightGainType { All, Red, Green, Blue, VRed }
    public class BrightGainParams
    {
        public static BrightGainType TypeAll
        {
            get { return BrightGainType.All; }
        }

        public static BrightGainType TypeRed
        {
            get { return BrightGainType.Red; }
        }

        public static BrightGainType TypeGreen
        {
            get { return BrightGainType.Green; }
        }

        public static BrightGainType TypeBlue
        {
            get { return BrightGainType.Blue; }
        }

        public static BrightGainType TypeVRed
        {
            get { return BrightGainType.VRed; }
        }


    }
    
    public class GammaConvertor : IValueConverter
    {

        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int val = 0;
            if (int.TryParse(value.ToString(), out val))
            {
                double res = ((double)val) / 10;
                return res;
            }
            else
            {
                return null;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double val = 0;
            
            if (double.TryParse(value.ToString(), out val))
            {
                if (val > 4)
                {
                    return null;
                }
                int res = (int)(val * 10);
                return res;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }

    public class BrightPercentConvertor : IValueConverter
    {

        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int val = 0;
            if (int.TryParse(value.ToString(), out val))
            {
                double res = ((double)val) / 255 * 100;
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                sb.Append(res.ToString("f2"));
                sb.Append("%)");
                return sb.ToString();
            }
            else
            {
                return null;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //double val = 0;

            //if (double.TryParse(value.ToString(), out val))
            //{
            //    if (val > 4)
            //    {
            //        return null;
            //    }
            //    int res = (int)(val * 10);
            //    return res;
            //}
            //else
            //{
            //    return null;
            //}
            return null;
        }

        #endregion
    }

    public class ColorTempConvertor : IValueConverter
    {

        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int res = ((int)value / 100);
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int data = (int)(double)value;
            int res = data * 100;
            return res;
        }

        #endregion
    }

    public class PeripheralUIData : NotificationST
    {
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IsSelected));
            }
        }
        private bool _isSelected = false;

        public string LocationString
        {
            get
            {
                return _locationString;
            }
            set
            {
                _locationString = value;
                NotifyPropertyChanged(GetPropertyName(o => this.LocationString));
            }
        }
        private string _locationString = "";

        public UseablePeripheral Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                NotifyPropertyChanged(GetPropertyName(o => this.Location));
            }
        }
        private UseablePeripheral _location = null;
    }

    public class PeripheralsSettingParam
    {
        public AutoBrightExtendData ExtendData;
    }
}
