using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Nova.SmartLCT.Interface;
using GalaSoft.MvvmLight;
using Nova.LCT.GigabitSystem.Common;
using System.Collections.Specialized;
using GalaSoft.MvvmLight.Command;

namespace Test
{
    public class Window2_VM : ViewModelBase
    {
        public ObservableCollection<IRectElement> SelectedElementCollection
        {
            get;
            set;
        }

        public SenderRealParameters MyRectLayer
        {
            get
            {
                return _myRectLayer;
            }
            set
            {
                _myRectLayer = value;
                RaisePropertyChanged("MyRectLayer");
            }
        }
        private SenderRealParameters _myRectLayer = new SenderRealParameters();

        public ObservableCollection<ScannerCofigInfo> ScannerTypeCollection
        {
            get { return _scannerTypeCollection; }
            set
            {
                _scannerTypeCollection = value;
                RaisePropertyChanged("ScannerTypeCollection");
            }
        }
        private ObservableCollection<ScannerCofigInfo> _scannerTypeCollection = new ObservableCollection<ScannerCofigInfo>();

        public PortRealParameters PortRealParams
        {
            get
            {
                return _portRealParams;
            }
            set
            {
                _portRealParams = value;

                RaisePropertyChanged("PortRealParams");
            }
        }
        private PortRealParameters _portRealParams = new PortRealParameters();

        public ScannerRealParameters ScannerRealParams
        {
            get
            {
                return _scannerRealParams;
            }
            set
            {
                _scannerRealParams = value;
            }
        }
        private ScannerRealParameters _scannerRealParams = new ScannerRealParameters();

        public RelayCommand CmdTest
        {
            get;
            private set;
        }

        public int TotalCanAddCount
        {
            get
            {
                return _totalCanAddCount;
            }
            set
            {
                _totalCanAddCount = value;

                RaisePropertyChanged("TotalCanAddCount");
            }
        }
        private int _totalCanAddCount = 0;

        public Window2_VM()
        {
            CmdTest = new RelayCommand(OnCmdTest);
            RectLayer layer = new RectLayer(20, 30, 100, 110, new RectLayer(), 0, ElementType.None,0);
            
            SenderRealParameters rp = new SenderRealParameters();
            rp.Element = layer;
            MyRectLayer = rp;
            if (!this.IsInDesignMode)
            {
                ScannerTypeCollection.Add(new ScannerCofigInfo() { DisplayName = "aa", ScanBdProp = new ScanBoardProperty() });
                ScannerTypeCollection.Add(new ScannerCofigInfo() { DisplayName = "bb", ScanBdProp = new ScanBoardProperty() });

                PortRealParameters param = new PortRealParameters();
                RectLayer rectLayer = new RectLayer();

                SelectedElementCollection = new ObservableCollection<IRectElement>();

                RectElement element = new RectElement(0, 0, 10, 10, rectLayer, 0);
                rectLayer.ElementCollection.Add(element);
                SelectedElementCollection.Add(element);

                element = new RectElement(15, 20, 10, 10, rectLayer, 0);
                rectLayer.ElementCollection.Add(element);
                SelectedElementCollection.Add(element);

                element = new RectElement(18, 20, 10, 10, rectLayer, 0);
                rectLayer.ElementCollection.Add(element);
                SelectedElementCollection.Add(element);

                element = new RectElement(0, 60, 10, 10, rectLayer, 0);
                rectLayer.ElementCollection.Add(element);
                SelectedElementCollection.Add(element);

                element = new RectElement(30, 0, 10, 10, rectLayer, 0);
                rectLayer.ElementCollection.Add(element);
                SelectedElementCollection.Add(element);

                element = new RectElement(30, 60, 10, 10, rectLayer, 0);
                rectLayer.ElementCollection.Add(element);
                SelectedElementCollection.Add(element);

                param.PortLayer = rectLayer;
                PortRealParams = param;


                PortRealParams.SelectedElementCollection = SelectedElementCollection;


                rectLayer = new RectLayer();
                ScannerRealParameters scanParam = new ScannerRealParameters();
                scanParam.ScannerElement = new RectElement(30, 60, 10, 10, rectLayer, 0);
                scanParam.ScannerConfig = new ScannerCofigInfo() { DisplayName = "aa", ScanBdProp = new ScanBoardProperty() };
                ScannerRealParams = scanParam;
            }
        }

        private void OnCmdTest()
        {
            bool res = true;
            if (res)
            {
                double data = 5.71234;
                int dt = (int)Math.Round(data, 0);
                PortRealParams.SelectedElementCollection.Clear();
            }
            else
            {
                RectElement element = new RectElement(0, 0, 10, 10, PortRealParams.PortLayer, 0);
                PortRealParams.SelectedElementCollection.Add(element);
                
            }
        }
    }
}
