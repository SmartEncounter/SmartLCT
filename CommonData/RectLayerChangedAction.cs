             using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuiLabs.Undo;
using System.Reflection;
using System.Diagnostics;

namespace Nova.SmartLCT.Interface
{
    public class RectLayerChangedAction : AbstractAction
    {
        public object ParentObject { get; set; }
        public PropertyInfo Property { get; set; }
        public object Value { get; set; }
        public object OldValue { get; set; }

        public RectLayerChangedAction(object parentObject, string propertyName, object value, object oldValue)
        {
            ParentObject = parentObject;
            Property = parentObject.GetType().GetProperty(propertyName);
            Value = value;
            OldValue = oldValue;

          //  Debug.WriteLine(oldValue.ToString());
        }

        protected override void ExecuteCore()
        {
            //OldValue = Property.GetValue(ParentObject, null);
            Property.SetValue(ParentObject, Value, null);
        }
        protected override void UnExecuteCore()
        
        {
            Property.SetValue(ParentObject, OldValue, null);
        }

        public override bool TryToMerge(IAction followingAction)
        {
            RectLayer r = new RectLayer();
            if (this.Property == r.GetType().GetProperty("ElementCollection"))
            {
                return false;
            }

            RectLayerChangedAction next = followingAction as RectLayerChangedAction;
            if (next != null && next.ParentObject == this.ParentObject && next.Property == this.Property)
            {
                Value = next.Value;
                Property.SetValue(ParentObject, Value, null);
                return true;
            }
            return false;
        }
    }
}
