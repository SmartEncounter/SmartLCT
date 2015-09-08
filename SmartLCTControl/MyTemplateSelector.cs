using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using Nova.SmartLCT.Interface;
using System.Windows.Data;

namespace Nova.SmartLCT.SmartLCTControl
{
    public class MyTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && (item is IElement))
            {
                if (item is RectElement)
                {
                    return element.FindResource("DataTemplate_MyRectangle") as DataTemplate;
                }
                else if (item is RectLayer)
                {
                    return element.FindResource("DataTemplate_MyRectLayer") as DataTemplate;
                }
                else if (item is LineElement)
                {
                    return element.FindResource("DataTemplate_MyLine") as DataTemplate;
                }
                else
                {
                    return null;
                }
            }

            return null;
        }
    }

}
