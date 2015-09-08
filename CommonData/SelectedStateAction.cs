using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using GuiLabs.Undo;

namespace Nova.SmartLCT.Interface
{
    public class SelectedStateAction:AbstractAction
    {
        public ObservableCollection<SelectedStateInfo> SelectedStateInfoCollection
        {
            get;
            set;
        }
        public SelectedStateAction()
        {

        }
        public SelectedStateAction(ObservableCollection<SelectedStateInfo> selectedStateInfoCollection)
        {
            SelectedStateInfoCollection = selectedStateInfoCollection;
        }
        protected override void ExecuteCore()
        {
            for (int i = 0; i < SelectedStateInfoCollection.Count; i++)
            {
                SelectedStateInfoCollection[i].Element.ElementSelectedState = SelectedStateInfoCollection[i].NewSelectedState;
            }
            //throw new NotImplementedException();
        }
        protected override void UnExecuteCore()
        {
            for (int i = 0; i < SelectedStateInfoCollection.Count; i++)
            {
                SelectedStateInfoCollection[i].Element.ElementSelectedState = SelectedStateInfoCollection[i].OldSelectedState;
            }
            //throw new NotImplementedException();
        }
        
    }
}
