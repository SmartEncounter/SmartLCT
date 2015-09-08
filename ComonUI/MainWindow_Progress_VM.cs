using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace Nova.SmartLCT.UI
{
    public class MainWindow_Progress_VM : ViewModelBase
    {
        public string DisplayTestMsg
        {
            get { return _displayTestMsg; }
            set
            {
                _displayTestMsg = value;
                RaisePropertyChanged("DisplayTestMsg");
            }
        }
        private string _displayTestMsg = string.Empty;

        public MainWindow_Progress_VM()
        {

        }

        public void Initializes(NotificationMessageAction<string> nsa)
        {
            DisplayTestMsg = nsa.Notification;
        }
    }
}