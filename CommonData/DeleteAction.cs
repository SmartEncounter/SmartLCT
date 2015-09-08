using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuiLabs.Undo;
using System.Collections.ObjectModel;

namespace Nova.SmartLCT.Interface
{
    public class DeleteAction : AbstractAction
    {
         public IRectLayer MyRectLayer
        {
            get;
            set;
        }
        public ObservableCollection<IElement> ElementCollection
         {
             get;
             set;
         }

         public DeleteAction(IRectLayer rectLayer,ObservableCollection<IElement> elementCollection)
        {
            MyRectLayer = rectLayer;
            ElementCollection = elementCollection;
        }

        protected override void ExecuteCore()
        {
            for (int i = 0; i < ElementCollection.Count; i++)
            {
                while (MyRectLayer.ElementCollection.Contains(ElementCollection[i]))
                {
                    MyRectLayer.ElementCollection.Remove(ElementCollection[i]);
                }
            }
        }
        protected override void UnExecuteCore()       
        {
            for (int i = 0; i < ElementCollection.Count; i++)
            {
                if (!MyRectLayer.ElementCollection.Contains(ElementCollection[i]))
                {
                    MyRectLayer.ElementCollection.Add(ElementCollection[i]);
                    if (ElementCollection[i] is LineElement)
                    {
                        ((LineElement)ElementCollection[i]).FrontElement.EndLine = ((LineElement)ElementCollection[i]);
                        ((LineElement)ElementCollection[i]).EndElement.FrontLine = ((LineElement)ElementCollection[i]);
                    }
                }           
            }
        }

        public override bool TryToMerge(IAction followingAction)
        {
            //ElementMoveAction next = (ElementMoveAction)followingAction;
            //if (next.MoveInfoCollection.Count != this.MoveInfoCollection.Count)
            //{
            //    return false;
            //}
            ////bool isSucceed = true;
            //for (int i = 0; i < next.MoveInfoCollection.Count; i++)
            //{
            //    if (!this.MoveInfoCollection.Contains(next.MoveInfoCollection[i]))
            //    {
            //        return false;
            //    }
            //    //MyData findData = this.RCList.Find(delegate(MyData data))
            //    //int index = this.MoveInfoCollection.FindIndex(delegate(ElementMoveInfo data)
            //    //{
            //    //    if (next.MoveInfoCollection[i].Element == data.Element)
            //    //    {
            //    //        return true;
            //    //    }
            //    //    else
            //    //    {
            //    //        return false;
            //    //    }
            //    //});
            //    //var Find = from c in this.RCList
            //    //                    where c.Rect.MyPos.X == 10
            //    //                    select c;


            //    //---------------------------
            //    //_curFind = next.RCList[i];
            //    //int index = this.RCList.FindIndex(Find);

            //    //if (index == -1)
            //    //{
            //    //    isSucceed = false;
            //    //}
            //}
            return false;
            //return isSucceed;
            //return false;
        }
    }
}
