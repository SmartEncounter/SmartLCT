using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nova.SmartLCT.Interface;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Nova.LCT.GigabitSystem.Common;

namespace Nova.SmartLCT.UI
{
    public class Window_FastSegmentation_VM : SmartLCTViewModeBase
    {
        #region 属性
        public int MaxEnvironmentBright
        {
            get { return _maxEnvironmentBright; }
            set
            {
                _maxEnvironmentBright = value;
                RaisePropertyChanged(GetPropertyName(o => this.MaxEnvironmentBright));
            }
        }
        private int _maxEnvironmentBright = 12000;
        public int MinEnvironmentBright
        {
            get { return _minEnvironmentBright; }
            set
            {
                _minEnvironmentBright = value;
                RaisePropertyChanged(GetPropertyName(o => this.MinEnvironmentBright));
            }
        }
        private int _minEnvironmentBright = 20;
        public int MaxScreenBright
        {
            get { return _maxScreenBright; }
            set
            {
                _maxScreenBright = value;
                RaisePropertyChanged(GetPropertyName(o => this.MaxScreenBright));
            }
        }
        private int _maxScreenBright=80;
        public int MinScreenBright
        {
            get { return _minScreenBright; }
            set
            {
                _minScreenBright = value;
                RaisePropertyChanged(GetPropertyName(o => this.MinScreenBright));
            }
        }
        private int _minScreenBright=40;
        public int SegmentationNum
        {
            get { return _segmentationNum; }
            set
            {
                _segmentationNum = value;
                RaisePropertyChanged(GetPropertyName(o => this.SegmentationNum));
            }
        }
        private int _segmentationNum=10;

        public FastSegmentParam FastSegmentParam
        {
            get
            {
                return _fastSegmentParam;
            }
            set
            {
                _fastSegmentParam = value;
            }
        }
        private FastSegmentParam _fastSegmentParam = new FastSegmentParam();

        #endregion

        #region 字段
        #endregion

        #region 命令
        public RelayCommand CmdOk
        {
            get;
            private set;
        }
        public RelayCommand CmdCancel
        {
            get;
            private set;
        }
        #endregion

        #region 构造
        public Window_FastSegmentation_VM()
        {
            CmdOk = new RelayCommand(OnCmdOk);
            CmdCancel = new RelayCommand(OnCmdCancel);
        }

        #region 私有
        private void OnCmdOk()
        {
            FastSegmentParam = new FastSegmentParam()
            {
                MaxEnvironmentBright = MaxEnvironmentBright, 
                MinEnvironmentBright = MinEnvironmentBright, 
                MaxDisplayBright = MaxScreenBright,
                MinDisplayBright = MinScreenBright, 
                SegmentNum = SegmentationNum
            };
            Messenger.Default.Send(MsgToken.MSG_WINFASTSEGMENTATION_OK, MsgToken.MSG_WINFASTSEGMENTATION_CLOSE);
        }
        private void OnCmdCancel()
        {
            Messenger.Default.Send(MsgToken.MSG_WINFASTSEGMENTATION_CANCEL, MsgToken.MSG_WINFASTSEGMENTATION_CLOSE);
        }
        #endregion
        #endregion
    }
}
