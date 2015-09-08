using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuiLabs.Undo;
using System.Collections.ObjectModel;

namespace Nova.SmartLCT.Interface
{
    public class ElementSizeAction : AbstractAction
    {
        public ObservableCollection<ElementSizeInfo> SizeInfoCollection
        {
            get;
            set;
        }

        public ElementSizeAction(ObservableCollection<ElementSizeInfo> sizeInfo)
        {
            SizeInfoCollection = sizeInfo;
        }

        protected override void ExecuteCore()
        {
            for (int i = 0; i < SizeInfoCollection.Count; i++)
            {
                SizeInfoCollection[i].Element.Height = SizeInfoCollection[i].NewSize.Height;
                SizeInfoCollection[i].Element.Width = SizeInfoCollection[i].NewSize.Width;
            }
        }
        protected override void UnExecuteCore()
        {
            for (int i = 0; i < SizeInfoCollection.Count; i++)
            {
                SizeInfoCollection[i].Element.Height = SizeInfoCollection[i].OldSize.Height;
                SizeInfoCollection[i].Element.Width = SizeInfoCollection[i].OldSize.Width;
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
