using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nova.SmartLCT.Interface;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;
using System.Windows.Data;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;

namespace Test
{
    class MainWindow_VM : NotificationST
    {
        #region 属性
        public RectLayer MyRectLayer
        {
            get
            {
                return _myRectLayer;
            }
            set
            {
                _myRectLayer = value;
                NotifyPropertyChanged(GetPropertyName(o => this.MyRectLayer));
            }
        }
        private RectLayer _myRectLayer = new RectLayer();

        public RectLayer MyRectLayerTest
        {
            get
            {
                return _myRectLayerTest;
            }
            set
            {
                _myRectLayerTest = value;
                NotifyPropertyChanged(GetPropertyName(o => this.MyRectLayerTest));
            }
        }
        private RectLayer _myRectLayerTest = new RectLayer();

        public bool bc
        {
            get { return _bc; }
            set
            {
                _bc = value;
                NotifyPropertyChanged(GetPropertyName(o => this.bc));
            }
        }
        private bool _bc = false;

        public IncreaseOrDecreaseState IncreaseOrDecrease
        {
            get { return _increaseOrDecrease; }
            set
            {
                _increaseOrDecrease = value;
                NotifyPropertyChanged(GetPropertyName(o => this.IncreaseOrDecrease));
            }
        }
        private IncreaseOrDecreaseState _increaseOrDecrease = IncreaseOrDecreaseState.None;

        public AlignmentState Alignment
        {
            get { return _alignment; }
            set
            {
                _alignment = value;
                NotifyPropertyChanged(GetPropertyName(o => this.Alignment));
            }
        }
        private AlignmentState _alignment = AlignmentState.None;
        public UnDoOrReDoState UnDoOrReDo
        {
            get { return _unDoOrReDo; }
            set
            {
                _unDoOrReDo = value;
                NotifyPropertyChanged(GetPropertyName(o => this.UnDoOrReDo));
            }
        }
        private UnDoOrReDoState _unDoOrReDo = UnDoOrReDoState.None;
		public List<int> NameList
		{
			get
			{ return _nameList;}
			set
			{
				_nameList=value;
				NotifyPropertyChanged(GetPropertyName(o => this.NameList));
			}
		}
		public List<int> _nameList=new List<int>();
        #endregion

        #region 命令
        public RelayCommand CmdUnDo
        {
            get;
            private set;
        }
        public RelayCommand CmdReDo
        {
            get;
            private set;
        }
        public RelayCommand CmdIncrease
        {
            get;
            private set;
        }
        public RelayCommand CmdDecrease
        {
            get;
            private set;
        }
        public RelayCommand CmdChangebc
        {
            get;
            private set;
        }
        public RelayCommand CmdBottomAlignment
        {
            get;
            private set;
        }
        public RelayCommand CmdTopAlignment
        {
            get;
            private set;
        }
        public RelayCommand CmdLeftAlignment
        {
            get;
            private set;
        }
        public RelayCommand CmdRightAlignment
        {
            get;
            private set;
        }
        public RelayCommand CmdLevelMiddleAlignment
        {
            get;
            private set;
        }
        public RelayCommand CmdPlumbMiddleAlignment
        {
            get;
            private set;
        }
        public RelayCommand CmdCancelLevelSpace
        {
            get;
            private set;
        }
        public RelayCommand CmdCancelPlumbSpace
        {
            get;
            private set;
        }

        #endregion

        #region 字段
        
        #endregion

        public MainWindow_VM()
        {
            NameList.Add(1);
            NameList.Add(2);

            //_myRectLayer = new RectLayer(10, 10, 600, 800, null, 0, null, null);
            //_myRectLayer.ElementCollection.Add(new RectElement(10, 10, 50, 50, _myRectLayer, 1, null, null,-1));
            //_myRectLayer.ElementCollection.Add(new RectElement(50, 130, 50, 50, _myRectLayer, 2, null, null,-1));

            //RectLayer _myRectLayerTwo = new RectLayer(120, 30, 600, 450, _myRectLayer, 3, null, null);
            //_myRectLayerTwo.ElementCollection.Add(new RectElement(10, 10, 50, 50, _myRectLayerTwo, 4, null, null,-1));
            //_myRectLayerTwo.ElementCollection.Add(new RectElement(20, 80, 50, 50, _myRectLayerTwo, 5, null, null,-1));
            //_myRectLayerTwo.Visible = Visibility.Visible;
            //_myRectLayer.ElementCollection.Add(_myRectLayerTwo);
            //RectLayer _myRectLayerThree = new RectLayer(200, 80, 600, 450, _myRectLayer, 6, null, null);
            //RectElement rect1 = new RectElement(10, 10, 50, 50, _myRectLayerThree, 7);
            //RectElement rect2 = new RectElement(20, 80, 50, 50, _myRectLayerThree, 8);
            //RectElement rect3 = new RectElement(80, 90, 50, 50, _myRectLayerThree, 9);
            //RectElement rect4 = new RectElement(80, 150, 60, 60, _myRectLayerThree, 10);
            //RectElement rect5 = new RectElement(80, 210, 50, 80, _myRectLayerThree, 11);
            //LineElement line1 = new LineElement(18, rect1, rect2);
            //LineElement line2 = new LineElement(19, rect2, rect3);
            //LineElement line3 = new LineElement(20, rect3, rect4);
            //LineElement line4 = new LineElement(21, rect4, rect5);
            //rect1.EndLine = line1;
            //rect2.FrontLine = line1; rect2.EndLine = line2;
            //rect3.FrontLine = line2; rect3.EndLine = line3;
            //rect4.FrontLine = line3; rect4.EndLine = line4;
            //rect5.FrontLine = line4;

            //_myRectLayerThree.ElementCollection.Add(rect1);
            //_myRectLayerThree.ElementCollection.Add(rect2);
            //_myRectLayerThree.ElementCollection.Add(rect3);
            //_myRectLayerThree.ElementCollection.Add(rect4);
            //_myRectLayerThree.ElementCollection.Add(rect5);

            //_myRectLayerThree.ElementCollection.Add(line1);
            //_myRectLayerThree.ElementCollection.Add(line2);
            //_myRectLayerThree.ElementCollection.Add(line3);
            //_myRectLayerThree.ElementCollection.Add(line4);
            //_myRectLayer.ElementCollection.Add(_myRectLayerThree);


            CmdChangebc = new RelayCommand(OnCmdChangebc);
            CmdDecrease = new RelayCommand(OnCmdDecrease);
            CmdIncrease = new RelayCommand(OnCmdIncrease);
            CmdLeftAlignment = new RelayCommand(OnCmdLeftAlignment);
            CmdRightAlignment = new RelayCommand(OnCmdRightAlignment);
            CmdTopAlignment = new RelayCommand(OnCmdTopAlignment);
            CmdBottomAlignment = new RelayCommand(OnCmdBottomAlignment);
            CmdLevelMiddleAlignment = new RelayCommand(OnCmdLevelMiddleAlignment);
            CmdPlumbMiddleAlignment = new RelayCommand(OnCmdPlumbMiddleAlignment);
            CmdCancelLevelSpace = new RelayCommand(OnCmdCancelLevelSpace);
            CmdCancelPlumbSpace = new RelayCommand(OnCmdCancelPlumbSpace);
            CmdUnDo = new RelayCommand(OnCmdUnDo);
            CmdReDo = new RelayCommand(OnCmdReDo);
        }

        private void OnCmdUnDo()
        {
            UnDoOrReDo = UnDoOrReDoState.UnDo;
            UnDoOrReDo = UnDoOrReDoState.None;
        }
        private void OnCmdReDo()
        {
            UnDoOrReDo = UnDoOrReDoState.ReDo;
            UnDoOrReDo = UnDoOrReDoState.None;
        }
        private void OnCmdChangebc()
        {
            bc = true;
            bc = false;
        }
        private void OnCmdDecrease()
        {

            IncreaseOrDecrease = IncreaseOrDecreaseState.Decrease;
        }
        private void OnCmdIncrease()
        {       
            IncreaseOrDecrease = IncreaseOrDecreaseState.Increase;
        }
        private void OnCmdBottomAlignment()
        {
            Alignment = AlignmentState.BottomAlignment;
        }
        private void OnCmdTopAlignment()
        {
            Alignment = AlignmentState.TopAlignment;
        }
        private void OnCmdLeftAlignment()
        {
            Alignment = AlignmentState.LeftAlignment;
        }
        private void OnCmdRightAlignment()
        {
            Alignment = AlignmentState.RightAlignment;
        }
        private void OnCmdLevelMiddleAlignment()
        {
            Alignment = AlignmentState.LevelMiddleAlignment;
        }
        private void OnCmdPlumbMiddleAlignment()
        {
            Alignment = AlignmentState.PlumbMiddleAlignment;
        }
        private void OnCmdCancelLevelSpace()
        {
            Alignment = AlignmentState.CancelLevelSpace;
        }
        private void OnCmdCancelPlumbSpace()
        {
            Alignment = AlignmentState.CancelPlumbSpace;
        }

    }
}
