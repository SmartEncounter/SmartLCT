using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using Nova.LCT.GigabitSystem.Common;

namespace Nova.SmartLCT.Interface
{
    class ValueConverter
    {
    }
    public class ConnectIndexConvertIsVisible : IValueConverter
    {
        #region IValueConverter 成员
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int connectIndex = (int)value;
            if (connectIndex <= -1)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class SizeConvertString : IValueConverter
    {
        #region IValueConverter 成员
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Size size = (Size)value;
            return Math.Round(size.Width).ToString() + " * " + Math.Round(size.Height).ToString()+"  ";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public class RectConvertString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Rect rect = (Rect)value;

            return rect.Width.ToString() + " * " + rect.Height.ToString();
        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class SenderRectConvertArea : IMultiValueConverter
    {
        #region IMultiValueConverter 成员

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Size maxLoadArea = (Size)values[0];
            Rect loadSize = (Rect)values[1];
            double res = 0;
            if (loadSize.Height * loadSize.Width > maxLoadArea.Height * maxLoadArea.Width)
            {
                res = SmartLCTViewModeBase.SenderMaxLoadSize;
            }
            else
            {
                res = loadSize.Width * loadSize.Height / (maxLoadArea.Height * maxLoadArea.Width / SmartLCTViewModeBase.SenderMaxLoadSize);
            }
            return res;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public class PortRectConvertArea : IMultiValueConverter
    {
        #region IMultiValueConverter 成员

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Size maxLoadArea = (Size)values[0];
            Rect loadSize = (Rect)values[1];
            double res = 0;
            if (loadSize.Height * loadSize.Width > maxLoadArea.Height * maxLoadArea.Width)
            {
                res = SmartLCTViewModeBase.PortMaxLoadSize;
            }
            else
            {
                res = loadSize.Width * loadSize.Height / (maxLoadArea.Height * maxLoadArea.Width / SmartLCTViewModeBase.PortMaxLoadSize);
            }
            return res;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public class IsTrueConvertIsFalse : IValueConverter
    {
        #region IValueConverter 成员
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool realValue = (bool)value;
            return !realValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool realValue = (bool)value;
            return !realValue;
        }

        #endregion
    }
    public class AdjustBrightModeConvertIsSelected : IValueConverter
    {
        #region IValueConverter 成员
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SmartBrightAdjustType mode = (SmartBrightAdjustType)value;
            int res = 0;
            if (mode == SmartBrightAdjustType.FixBright)
            {
                res = 0;
            }
            else if (mode == SmartBrightAdjustType.AutoBright)
            {
                res = 1;
            }
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int selectedIndex = int.Parse(value.ToString());
            SmartBrightAdjustType mode = SmartBrightAdjustType.FixBright;
            if (selectedIndex == 0)
            {
                mode = SmartBrightAdjustType.FixBright;
            }
            else if (selectedIndex == 1)
            {
                mode = SmartBrightAdjustType.AutoBright;
            }
            return mode;
        }

        #endregion
    }


    public class AdjustBrightModeConvertIsCheck : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BrightAdjustMode info = (BrightAdjustMode)value;
            string para = (string)parameter;
            if ((para == "Manual" && info == BrightAdjustMode.Manual)
                || (para == "SmartBright" && info == BrightAdjustMode.SmartBright))
            {
                return true;
            }
            return false;         
        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;
            string para = (string)parameter;
            BrightAdjustMode mode = BrightAdjustMode.Manual;
            if (para == "Manual")
            {
                mode = BrightAdjustMode.SmartBright;
            }
            else if (para == "SmartBright")
            {
                mode = BrightAdjustMode.SmartBright;
            }
            return mode;                        
        }

        #endregion
    }

    public class BrightAdjustDataConvertEnabled : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            return true;
        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class EleTypeConvertLayerName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ElementType eleType = (ElementType)value;
            string str = "";
            if (eleType == ElementType.screen)
            {
                CommonStaticMethod.GetLanguageString("显示屏信息", "Lang_Convertor_ScreenInfo", out str);
            }
            else if (eleType == ElementType.sender)
            {
                CommonStaticMethod.GetLanguageString("发送卡信息", "Lang_Convertor_SenderInfo", out str);
            }
            else if (eleType == ElementType.port)
            {
                CommonStaticMethod.GetLanguageString("网口信息", "Lang_Convertor_PortInfo", out str);

            }
            return str;
        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class LayerTypeConvertStr : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ElementType eleType = (ElementType)value;
            string str = "";
            if (eleType == ElementType.screen)
            {
                CommonStaticMethod.GetLanguageString("当前显示屏", "Lang_Convertor_CurScr", out str);
            }
            else if (eleType == ElementType.sender)
            {
                CommonStaticMethod.GetLanguageString("当前发送卡", "Lang_Convertor_CurSender", out str);
            }
            return str;
        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class RectSizeConvertSizeName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Size rectSize = (Size)value;
            string str = "";
            CommonStaticMethod.GetLanguageString("大小", "Lang_Convertor_Size", out str);
            return str + ":(" + rectSize.Width.ToString() + "," + rectSize.Height.ToString() + ")";
        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class MarginConvertMarginName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Thickness margin = (Thickness)value;
            string str = "";
            CommonStaticMethod.GetLanguageString("位置", "Lang_Convertor_Location", out str);
            return str + ":(" + margin.Left.ToString() + "," + margin.Top.ToString() + ")";
        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class ConnectIndexConvertScreenName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int connectIndex = (int)value;
            string str = "";
            CommonStaticMethod.GetLanguageString("显示屏", "Lang_Global_Screen", out str);
            
            return str + " " + (connectIndex + 1);
        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class OperateEnvironConvertVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            OperatEnvironment environment = (OperatEnvironment)value;
            if (environment != OperatEnvironment.DesignScreen)
            {
                return Visibility.Visible;
            }
            
            return Visibility.Hidden;
        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class SelectedArrangeTypeConvertIsChecked : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ArrangeType arrangeType = (ArrangeType)value;
            string para = parameter.ToString();
            if (arrangeType.ToString() == para)
            {
                return true;
            }
            return false;
        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class IsCheckedConvertVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool ischecked = (bool)value;
            if (ischecked)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class ConnectIndexConvertDeleteBtVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int connectIndex = (int)value;
            if (connectIndex == -1)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class ElementConvertPortBeginIconMargin : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IElement element = (IElement)value;
            Thickness margin = new Thickness();
            if (element.EleType == ElementType.receive)
            {
                RectElement rectElement=(RectElement)element;
                margin.Left = rectElement.Width / 2 - 10;
                margin.Top = rectElement.Height / 2 - 10;
                margin.Bottom = margin.Top + 20;
                margin.Right = margin.Left + 20;
            }          
            return margin;
        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class IsOverLoadConvertStr : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool IsOverLoad = (bool)value;
           string para=parameter.ToString();
           string result = "";
           string str = "";

            if (IsOverLoad)
            {
                if (para == "Tooltipdaizai")
                {
                    CommonStaticMethod.GetLanguageString("超出带载", "Lang_Convertor_OverLoad", out str);
                    result= str;
                }
                else
                {
                    CommonStaticMethod.GetLanguageString("超出带载", "Lang_Convertor_OverLoad", out str);
                    result = str;
                }
            }
            else
            {
                if(para=="Tooltipdaizai")
                {
                    CommonStaticMethod.GetLanguageString("当前带载正常", "Lang_Convertor_CurLoadOK", out str);
                    result = str;
                }
                else
                {
                    CommonStaticMethod.GetLanguageString("带载", "Lang_Convertor_Load", out str);
                    result = str;
                }
            }
            return result;
        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class SelectedConvertHeight : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SelectedState state = (SelectedState)value;
            string para = parameter.ToString();
            if (para == "line")
            {
                if (state == SelectedState.None)
                {
                    return 3;
                }
                else
                {
                    return 6;
                }
            }
            else 
            {
                if (state == SelectedState.None)
                {
                    return 16;
                }
                else
                {
                    return 20;
                }
            }

        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class IsSelectedConvertMouseOverColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSelected = (bool)value;
            string color = "";
            if (isSelected)
            {
                color = "#FF1F1F1F";
            }
            else
            {
                color = "#FF303435";
            }
            return color;
        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class IsSelectedConvertColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSelected = (bool)value;
            string color = "";
            if (isSelected)
            {
                color = "#FF14141C";
            }
            else
            {
                color = "#FF4E4E4E";
            }
            return color;
        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class IsOverLoadConvertColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isOverLoad = (bool)value;
            string color = "";
            if (isOverLoad)
            {
                color = "#FFFF0000";
            }
            else
            {
                color = "#FFFFFFFF";
            }
            return color;
        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class SenderIndexConvertSenderName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int senderIndex = (int)value;
            string str = "";
            CommonStaticMethod.GetLanguageString("发送卡", "Lang_Global_SendingBoard", out str);
            return str + (senderIndex+1).ToString();
        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public class PortIndexConvertPortName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int portIndex = (int)value;
            string str = "";
            CommonStaticMethod.GetLanguageString("网口", "Lang_Global_NetPort", out str);
            return str + (portIndex+1).ToString();
        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public class IsLockedConverterSizeButtonIsEnabled : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isLocked = (bool)value;
            return !isLocked;
        }

        #region IValueConverter 成员
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isenabled = (bool)value;
            return !isenabled;
        }

        #endregion
    }

    public class SelectedStateConvertCursor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SelectedState state = (SelectedState)value;
            string cursorName = (string)parameter;
            if (state == SelectedState.None)
            {
                return Cursors.Arrow;
            }
            else
            {
                switch (cursorName)
                {
                    case "SizeNS": return Cursors.SizeNS; 
                    case "SizeNWSE": return  Cursors.SizeNWSE;
                    case "SizeNESW": return Cursors.SizeNESW;
                    case "SizeWE": return Cursors.SizeWE;
                    default:return Cursors.Arrow;
                }
            }
        }
        #region IValueConverter 成员

        public object  ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
 	        throw new NotImplementedException();
        }

       #endregion
   }


    public class EleTypeConverterColorWithAdjustSender : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ElementType eleType = (ElementType)value;
            string color="";

            if (eleType == ElementType.screen)
            {
                color = "#FF414244";
            }
            else if (eleType == ElementType.groupframe)
            {
                color = "#FFFFFFFF";
            }
            else if (eleType == ElementType.baselayer)
            {
                color = "#FF0000";
            }
            return color;
        }
        #region IValueConverter 成员

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class EleTypeConverterColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ElementType eleType = (ElementType)value;
            string color="";

            if (eleType == ElementType.screen)
            {
                color = "#FF14141C";
            }
            else if (eleType == ElementType.groupframe)
            {
                color = "#00FFFFFF";
            }
            else if (eleType == ElementType.baselayer)
            {
                color = "#FF000000";
            }
            return color;
        }
        #region IValueConverter 成员

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class SelectedStateConverterIsSelected : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SelectedState elementSelectedState = (SelectedState)value;
            if (elementSelectedState == SelectedState.None)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSelected = (bool)value;
            if (!isSelected)
            {
                return SelectedState.None;
            }
            else
            {
                return SelectedState.Selected;
            }
        }
    }

    public class RepetitionStateVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RepetitionState elementSelectedState = (RepetitionState)value;
            if (elementSelectedState == RepetitionState.Custom)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class WeekList2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<DayOfWeek> elementSelectedState = (List<DayOfWeek>)value;
            
            if (elementSelectedState != null)
            {
                string msg = "";
                RepetitionState state = CommonStaticMethod.GetWeekRepetition(elementSelectedState);
                if (state == RepetitionState.EveryDay)
                {
                    CommonStaticMethod.GetLanguageString("每天", "Lang_Bright_EveryDay", out msg);
                }
                else if (state == RepetitionState.MonToFri)
                {
                    CommonStaticMethod.GetLanguageString("周一至周五", "Lang_Bright_MonToFri", out msg);
                }
                else
                {
                    foreach (DayOfWeek week in elementSelectedState)
                    {
                        msg += GetWeekDisplay(week);
                        msg += ",";
                    }
                    if (msg.Length != 0)
                    {
                        msg.Remove(msg.Length - 1);
                    }
                }
                return msg;
            }
            else
            {
                return null;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        private string GetWeekDisplay(DayOfWeek weekValue)
        {
            string msg = "";
            switch (weekValue)
            {
                case DayOfWeek.Monday: CommonStaticMethod.GetLanguageString("周一,", "Lang_Bright_Monday", out msg); break;
                case DayOfWeek.Tuesday: CommonStaticMethod.GetLanguageString("周二,", "Lang_Bright_Tuesday", out msg); break;
                case DayOfWeek.Wednesday: CommonStaticMethod.GetLanguageString("周三,", "Lang_Bright_Wednesday", out msg); break;
                case DayOfWeek.Thursday: CommonStaticMethod.GetLanguageString("周四,", "Lang_Bright_Thursday", out msg); break;
                case DayOfWeek.Friday: CommonStaticMethod.GetLanguageString("周五,", "Lang_Bright_Friday", out msg); break;
                case DayOfWeek.Saturday: CommonStaticMethod.GetLanguageString("周六,", "Lang_Bright_Saturday", out msg); break;
                case DayOfWeek.Sunday: CommonStaticMethod.GetLanguageString("周日,", "Lang_Bright_Sunday", out msg); break;
                default: break;
            }
            return msg;
        }

    }
    
    public class SmartBrightModeAndBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SmartBrightAdjustType elementSelectedState = (SmartBrightAdjustType)value;
            string param = (string)parameter;

            if (param == elementSelectedState.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool elementSelectedState = (bool)value;
            string param = (string)parameter;
            if (elementSelectedState)
            {
                if (Enum.IsDefined(typeof(SmartBrightAdjustType), param))
                {
                    SmartBrightAdjustType temp = (SmartBrightAdjustType)Enum.Parse(typeof(SmartBrightAdjustType), param);
                    return temp;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }

    public class BrightPercentAvailableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float elementSelectedState = (float)value;

            if (elementSelectedState < 0)
            {
                return "--";
            }
            else
            {
                return value.ToString();
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
                return null;
        }
    }

    public class BrightAdjustModeAndBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BrightAdjustMode elementSelectedState = (BrightAdjustMode)value;
            string param = (string)parameter;

            if (param == elementSelectedState.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool elementSelectedState = (bool)value;
            string param = (string)parameter;
            if (elementSelectedState)
            {
                if (Enum.IsDefined(typeof(BrightAdjustMode), param))
                {
                    BrightAdjustMode temp = (BrightAdjustMode)Enum.Parse(typeof(SmartBrightAdjustType), param);
                    return temp;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }

    public class EnumBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            string checkValue = value.ToString();
            string targetValue = parameter.ToString();
            return checkValue.Equals(targetValue,
                     StringComparison.InvariantCultureIgnoreCase);
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

    public class BoolReversalVisibilityConvertor : IValueConverter
    {

        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool res = (bool)value;
            if (res)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility res = (Visibility)value;
            if (Visibility.Visible == res)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }

    public class DateTimeTimeShowConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            string format = ((DateTime)value).ToString("HH:mm");
            return format;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
