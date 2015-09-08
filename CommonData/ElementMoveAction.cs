using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuiLabs.Undo;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Nova.SmartLCT.Interface
{
    public class ElementMoveAction : AbstractAction
    {
        public ObservableCollection<ElementMoveInfo> MoveInfoCollection
        {
            get;
            set;
        }

        public ElementMoveAction(ObservableCollection<ElementMoveInfo> moveInfo)
        {
            MoveInfoCollection = moveInfo;
        }

        protected override void ExecuteCore()
        {
            for (int i = 0; i < MoveInfoCollection.Count; i++)
            {
                MoveInfoCollection[i].Element.X = MoveInfoCollection[i].NewPoint.X;
                MoveInfoCollection[i].Element.Y = MoveInfoCollection[i].NewPoint.Y;
            }
        }
        protected override void UnExecuteCore()
        
        {
            for (int i = 0; i < MoveInfoCollection.Count; i++)
            {
                MoveInfoCollection[i].Element.X = MoveInfoCollection[i].OldPoint.X;
                MoveInfoCollection[i].Element.Y = MoveInfoCollection[i].OldPoint.Y;
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
