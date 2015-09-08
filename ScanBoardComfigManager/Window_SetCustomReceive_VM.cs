using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using Nova.SmartLCT.Interface;
using Nova.LCT.GigabitSystem.Common;
using System.Windows;

namespace Nova.SmartLCT.UI
{
    public class Window_SetCustomReceive_VM : SmartLCTViewModeBase
    {
        #region 属性]
        public int MaxWidth
        {
            get { return _maxWidth; }
            set
            {
                _maxWidth = value;
                RaisePropertyChanged(GetPropertyName(o => this.MaxWidth));
            }
        }
        private int _maxWidth = 0;
        public int MaxHeight
        {
            get { return _maxHeight; }
            set
            {
                _maxHeight = value;
                RaisePropertyChanged(GetPropertyName(o => this.MaxHeight));
            }
        }
        private int _maxHeight = 0;
        public int Height
        {
            get { return _height; }
            set 
            { 
                _height = value;
                MaxWidth = 256 * 226 / value;
                RaisePropertyChanged(GetPropertyName(o => this.Height));
            }
        }
        private int _height = 64;
        public int Width
        {
            get { return _width; }
            set 
            {
                _width = value;
                MaxHeight = 256 * 226 / value;
                RaisePropertyChanged(GetPropertyName(o => this.Width));
            }
        }
        private int _width = 64;
        #endregion
        #region 命令
        public RelayCommand CmdButton_OK
        {
            get;
            private set;
        }
        public RelayCommand CmdButton_Cancel
        {
            get;
            private set;
        }
        #endregion

        #region 构造函数
        public Window_SetCustomReceive_VM()
        {
            CmdButton_OK = new RelayCommand(OnCmdButton_Ok);
            CmdButton_Cancel = new RelayCommand(OnCmdButton_Cancel);
        }
        #endregion
        private void OnCmdButton_Ok()
        {
            //ScannerCofigInfo scanConfig = new ScannerCofigInfo();
            //ScanBoardProperty scanBdProp = new ScanBoardProperty();
            //scanBdProp.StandardLedModuleProp.DriverChipType = ChipType.Unknown;
            //scanBdProp.ModCascadeType = ModuleCascadeDiretion.Unknown;
            //scanBdProp.Width = Width;
            //scanBdProp.Height = Height;
            //scanConfig.ScanBdProp = scanBdProp;
            //scanConfig.DisplayName = Width.ToString() + "_" + Height.ToString();
            //_globalParams.ScannerConfigCollection.Insert(_globalParams.ScannerConfigCollection.Count-1,scanConfig);
            if (Width < 32 || Height<32)
            {
                string msg = "";
                CommonStaticMethod.GetLanguageString("箱体宽高不能小于32", "Lang_ScanBoardConfigManager_MinScanner", out msg);
                ShowGlobalDialogMessage(msg, MessageBoxImage.Error);
                return;
            }
            Messenger.Default.Send(MsgToken.MSG_SETCUSTOMRECEIVEFILE_OK, MsgToken.MSG_SETCUSTOMRECEIVE_CLOSE);
        }

        public void OnCmdButton_Cancel()
        {
            Messenger.Default.Send(MsgToken.MSG_SETCUSTOMRECEIVEFILE_CANCEL, MsgToken.MSG_SETCUSTOMRECEIVE_CLOSE);
        }
    }
}
