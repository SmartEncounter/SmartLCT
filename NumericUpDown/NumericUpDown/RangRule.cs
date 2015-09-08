using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Nova.Wpf.Control
{
    //public class RangRule : ValidationRule
    //{
    //    public double MaxValue
    //    {
    //        get
    //        {
    //            return _maxValue;
    //        }
    //        set
    //        {
    //            _maxValue = value;
    //        }
    //    }
    //    private double _maxValue;

    //    public double MinValue
    //    {
    //        get
    //        {
    //            return _minValue;
    //        }
    //        set
    //        {
    //            _minValue = value;
    //        }
    //    }
    //    private double _minValue;

    //    public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
    //    {
    //        try
    //        {
    //            string temp = (string)value;
    //            double newValue = 0;
    //            if (!double.TryParse(temp, out newValue))
    //            {
    //                return new ValidationResult(false, -1);
    //            }
    //            if (newValue > _maxValue || newValue < _minValue)
    //            {
    //                return new ValidationResult(false, -2);
    //            }
    //            else
    //            {
    //                return new ValidationResult(true, 0);
    //            }
    //        }
    //        catch (System.Exception ex)
    //        {
    //            return new ValidationResult(false, -3);
    //        }
    //    }
    //}
}
