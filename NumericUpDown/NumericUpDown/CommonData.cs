using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Nova.Wpf.Control
{
    public class ValueChangedEventArgs : RoutedEventArgs
    {
        public double Value
        {
            get;
            set;
        }

        public ValueChangedEventArgs() :
            base()
        {

        }

        public ValueChangedEventArgs(RoutedEvent routedEvent) :
            base(routedEvent)
        {

        }

        public ValueChangedEventArgs(RoutedEvent routedEvent, object source) : 
            base(routedEvent, source)
        {

        }
    }

    public delegate void RoutedValueChangedHandler(object sender, ValueChangedEventArgs e);
}
