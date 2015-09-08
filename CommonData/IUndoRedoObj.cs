using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.SmartLCT.Interface
{
    interface IUndoRedoable
    {
        event PrePropertyChangedEventHandler PrePropertyChangedEvent;
    }

    public delegate void PrePropertyChangedEventHandler(object sender, PrePropertyChangedEventArgs e);
    public class PrePropertyChangedEventArgs : EventArgs
    {
        public string PropertyName
        {
            get;
            set;
        }
        public object OldValue
        {
            get;
            set;
        }
        public object NewValue
        {
            get;
            set;
        }
        public object ZOrder
        {
            get;
            set;
        }
        public object ParentElement
        {
            get;
            set;
        }
    }
}
