using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuiLabs.Undo;
using System.Collections.ObjectModel;

namespace Nova.SmartLCT.Interface
{
    public class ConnectIconVisibilityAction : AbstractAction
    {
        public ObservableCollection<ConnectIconVisibilityInfo> ConnectIconVisibleCollection
        {
            set;
            get;
        }
        public ConnectIconVisibilityAction(ObservableCollection<ConnectIconVisibilityInfo> connectIconVisibleCollection)
        {
            ConnectIconVisibleCollection = connectIconVisibleCollection;
        }
        protected override void ExecuteCore()
        {
            for (int i = 0; i < ConnectIconVisibleCollection.Count; i++)
            {
                ConnectIconVisibleCollection[i].Element.MaxConnectIndexVisibile = ConnectIconVisibleCollection[i].OldAndNewMaxConnectIndexVisibile.NewValue;
                ConnectIconVisibleCollection[i].Element.MinConnectIndexVisibile = ConnectIconVisibleCollection[i].OldAndNewMinConnectIndexVisibile.NewValue;
            }
        }
        protected override void UnExecuteCore()
        {
            for (int i = 0; i < ConnectIconVisibleCollection.Count; i++)
            {
                ConnectIconVisibleCollection[i].Element.MaxConnectIndexVisibile = ConnectIconVisibleCollection[i].OldAndNewMaxConnectIndexVisibile.OldValue;
                ConnectIconVisibleCollection[i].Element.MinConnectIndexVisibile = ConnectIconVisibleCollection[i].OldAndNewMinConnectIndexVisibile.OldValue;
            }
        }

        public override bool TryToMerge(IAction followingAction)
        {
            return false;
        }

    }
}
