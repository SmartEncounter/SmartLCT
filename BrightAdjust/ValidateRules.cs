using Nova.SmartLCT.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Nova.SmartLCT.UI
{
    public class EnvironmentBrightValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);
            int temp = 0;
            bool res = int.TryParse((string)value, out temp);
            if (!res || temp > 60000)
            {
                string msg = "";
                CommonStaticMethod.GetLanguageString("环境亮度范围是：(0~60000)", "Lang_Bright_EnvironmentBrightRange", out msg);
                result = new ValidationResult(false, msg);
            }

            return result;

        }
    }

    public class ScreenBrightValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);
            int temp = 0;
            bool res = int.TryParse((string)value, out temp);
            if (!res || temp > 100)
            {
                string msg = "";
                CommonStaticMethod.GetLanguageString("显示屏的亮度范围是：(0%~100%)", "Lang_Bright_ScreenBrightRange", out msg);
                result = new ValidationResult(false, msg);
            }

            return result;

        }

    } 
}
