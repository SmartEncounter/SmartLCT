using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using Nova.SmartLCT.Interface;
using GalaSoft.MvvmLight.Command;
using Nova.LCT.GigabitSystem.Common;
using GalaSoft.MvvmLight.Messaging;

namespace Nova.SmartLCT.UI
{
    public class WinBrightAdjust_VM : SmartLCTViewModeBase
    {
        #region 属性
        public ObservableCollection<ScreenBrightInfo> ScreenBrightCollection
        {
            get { return _screenBrightCollection; }
            set
            {
                _screenBrightCollection = value;
                RaisePropertyChanged(GetPropertyName(o => this.ScreenBrightCollection));
            }
        }
        private ObservableCollection<ScreenBrightInfo> _screenBrightCollection = null;

        #endregion

        #region 字段
        #endregion

        #region 命令
        //public RelayCommand<string> CmdSetDefaultColorTemp
        //{
        //    get;
        //    private set;
        //}
        #endregion

        public WinBrightAdjust_VM()
        {
            if (!this.IsInDesignMode)
            {
                string msg = "";
                CommonStaticMethod.GetLanguageString("亮度调节", "Lang_Bright_MainWinText", out msg);
                this.WindowRealTitle = msg;

                Initialize();
            }
            #region 测试代码
            if (this.IsInDesignMode)
            {
                //string msg = "";
                //CommonStaticMethod.GetLanguageString("", "Lang_Bright_MainWinText", out msg);
                //this.WindowRealTitle = msg;

                ScreenBrightCollection = new ObservableCollection<ScreenBrightInfo>();
                ScreenBrightInfo info = new ScreenBrightInfo() { ScreenName = "Screen1" };
                ScreenBrightCollection.Add(info);

                info = new ScreenBrightInfo() { ScreenName = "Screen2" };
                ScreenBrightCollection.Add(info);
            }
            #endregion

        }
        private void Initialize()
        {
            if (_globalParams.AllCommPortLedDisplayDic == null)
            {
                return;
            }
            lock (_serverProxy.EquimentObject)
            {
                ScreenBrightCollection = new ObservableCollection<ScreenBrightInfo>();

                //foreach (string key in _globalParams.AllCommPortLedDisplayDic.Keys)
                //{
                //    List<ILEDDisplayInfo> screenList = _globalParams.AllCommPortLedDisplayDic[key];
                //    for (int i = 0; i < screenList.Count; i++ )
                //    {
                        
                //        string strScreen = "";
                //        CommonStaticMethod.GetLanguageString("显示屏", "Lang_Global_Screen", out strScreen);

                //        string scrName = key + "-" + strScreen + (i + 1);
                //        ScreenBrightInfo info = new ScreenBrightInfo() 
                //        { 
                //            ScreenName = scrName, 
                //            DisplayInfo = screenList[i],
                //            SelectedPort = key,
                //            DisplayUDID = scrName
                //        };
                //        ScreenBrightCollection.Add(info);
                //        info.CmdReadScanBdProp.Execute(null);
                //        info.CmdReadFromDB.Execute(info.DisplayUDID);
                //    }
                //}

                foreach (SupperDisplay supper in _globalParams.SupperDisplayList)
                {
                    foreach (string comName in _globalParams.AllBaseInfo.AllInfoDict.Keys)
                    {
                        OneCOMHWBaseInfo baseInfo = _globalParams.AllBaseInfo.AllInfoDict[comName];
                        for (int i = 0; i < baseInfo.LEDDisplayInfoList.Count; i++)
                        {
                            string screenID = baseInfo.FirstSenderSN + i.ToString("x2");
                            int index = supper.ScreenList.FindIndex(delegate(OneScreenInSupperDisplay oneScreen)
                            {
                                if (oneScreen.ScreenUDID == screenID)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            });
                            if (index != -1)
                            {
                                string strScreen = "";
                                CommonStaticMethod.GetLanguageString("显示屏", "Lang_Global_Screen", out strScreen);

                                string scrName = comName + "-" + strScreen + (i + 1);
                                ScreenBrightInfo info = new ScreenBrightInfo()
                                {
                                    ScreenName = scrName,
                                    DisplayInfo = baseInfo.LEDDisplayInfoList[i],
                                    SelectedPort = comName,
                                    DisplayUDID = supper.DisplayUDID
                                };
                                ScreenBrightCollection.Add(info);
                                info.CmdReadScanBdProp.Execute(null);
                                info.CmdReadFromDB.Execute(info.DisplayUDID);
                            }
                        }
                    }
                }
            }
        }
    }
}
