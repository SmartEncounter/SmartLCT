using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nova.SmartLCT.Interface;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Collections.Specialized;
using Nova.LCT.GigabitSystem.Common;

namespace Nova.SmartLCT.UI
{
    public class Window_AddTimingAdjustData_VM : SmartLCTViewModeBase
    {
        #region 属性
        public EnvironAndDisplayBrightCollection EnvirAndDisplayBrightCollection
        {
            get { return _envirAndScreenBrightCollection; }
            set
            {
                _envirAndScreenBrightCollection = value;
               
                RaisePropertyChanged(GetPropertyName(o => this.EnvirAndDisplayBrightCollection));
            }
        }
        private EnvironAndDisplayBrightCollection _envirAndScreenBrightCollection = new EnvironAndDisplayBrightCollection();
        private void OnElementCollectionChange(object sender, NotifyCollectionChangedEventArgs e)
        {

            //if (e.Action == NotifyCollectionChangedAction.Add)
            //{
            //    #region 添加
            //    EnvironAndDisplayBrightInfo newElement = (EnvironAndDisplayBrightInfo)e.NewItems[0];
            //    List<double> environBrightList = new List<double>();
            //    for (int itemIndex = 0; itemIndex < EnvirAndDisplayBrightCollection.Count; itemIndex++)
            //    {
            //        environBrightList.Add(EnvirAndDisplayBrightCollection[itemIndex].EnvironBright);
            //    }
            //    environBrightList.Sort(delegate(double first,double second)
            //    {
            //        return first.CompareTo(second);
            //    });
            //    int oldIndex=EnvirAndDisplayBrightCollection.IndexOf(newElement);
            //    int newIndex = environBrightList.IndexOf(newElement.EnvironBright);
            //    EnvirAndDisplayBrightCollection.Move(oldIndex, newIndex);
            //    #endregion

            //}
            //else if (e.Action == NotifyCollectionChangedAction.Remove)
            //{
            //    #region 移除
            //    #endregion
            //}
            //else if (e.Action == NotifyCollectionChangedAction.Replace)
            //{
            //    #region 修改
            //    #endregion
            //}
            //else if (e.Action == NotifyCollectionChangedAction.Reset)
            //{
            //    #region 重置

            //    #endregion
            //}
            //else if (e.Action == NotifyCollectionChangedAction.Move)
            //{

            //}

        }

        public DateTime AdjustTime
        {
            get { return _adjustTime; }
            set
            {
                _adjustTime = value;
                RaisePropertyChanged(GetPropertyName(o => this.AdjustTime));
            }
        }
        private DateTime _adjustTime;
        public SmartBrightAdjustType SelectedSmartBrightMode
        {
            get { return _selectedSmartBrightMode; }
            set
            {
                _selectedSmartBrightMode = value;
                RaisePropertyChanged(GetPropertyName(o => this.SelectedSmartBrightMode));
            }
        }
        private SmartBrightAdjustType _selectedSmartBrightMode = SmartBrightAdjustType.FixBright;
        public float BrightValue
        {
            get { return _brightValue; }
            set
            {
                _brightValue = value;
                RaisePropertyChanged(GetPropertyName(o => this.BrightValue));
            }
        }
        private float _brightValue = 100.0f;
        /// <summary>
        /// 自定义时选择了周几
        /// </summary>
        public ObservableCollection<WeekInfo> CustomSelectedWeekCollection
        {
            get { return _customSelectedWeekCollection; }
            set 
            {
                _customSelectedWeekCollection = value;
                RaisePropertyChanged(GetPropertyName(o => this.CustomSelectedWeekCollection));
            }
        }
        private ObservableCollection<WeekInfo> _customSelectedWeekCollection = new ObservableCollection<WeekInfo>();

        /// <summary>
        /// 重复模式，设置重复模式的时候，自动勾选对应的每一天
        /// </summary>
        public RepetitionState SelectedRepetitionState
        {
            get { return _selectedRepetitionState; }
            set 
            { 
                _selectedRepetitionState = value;
                ResetWeekSelected(value);
                RaisePropertyChanged(GetPropertyName(o => this.SelectedRepetitionState));
            }
        }
        private RepetitionState _selectedRepetitionState;

        public OneSmartBrightEasyConfig OneSmartBrightItem
        {
            get
            {
                FixParamToSmartBrightItem(_oneSmartBrightItem);
                return _oneSmartBrightItem;
            }
            set
            {
                if (value != null)
                {
                    UpdateSmartBrightToUI(value);
                }
                _oneSmartBrightItem = value;
            }
        }
        private OneSmartBrightEasyConfig _oneSmartBrightItem = new OneSmartBrightEasyConfig();

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(GetPropertyName(o => this.Title));
            }
        }
        private string _title = "";
        public bool IsSelectedAllHours
        {
            get { return _isSelectedAllHours; }
            set
            {
                _isSelectedAllHours = value;
                RaisePropertyChanged(GetPropertyName(o => this.IsSelectedAllHours));
            }
        }
        private bool _isSelectedAllHours = false;

        public ObservableCollection<ComboBoxDataSet> RepetitionStateDataSource
        {
            get
            {
                return _repetitionStateDataSource;
            }
            set
            {
                _repetitionStateDataSource = value;
                RaisePropertyChanged(GetPropertyName(o => this.RepetitionStateDataSource));
            }
        }
        private ObservableCollection<ComboBoxDataSet> _repetitionStateDataSource = new ObservableCollection<ComboBoxDataSet>();
        #endregion

        #region 命令
        public RelayCommand CmdOK
        {
            get;
            private set;
        }
        public RelayCommand CmdCancel
        {
            get;
            private set;
        }

        public RelayCommand CmdFastSegmentation
        {
            get;
            private set;
        }
        #endregion

        #region 字段
        private FastSegmentParam _fastSementParam;

        #endregion

        #region 构造
        public Window_AddTimingAdjustData_VM()
        {
            if (!this.IsInDesignMode)
            {
                string title;
                CommonStaticMethod.GetLanguageString("设置智能亮度", "Lang_Bright_AddSmartBrightTitle", out title);
                this.WindowRealTitle = title;

                CmdOK = new RelayCommand(OnOK);
                CmdCancel = new RelayCommand(OnCancel);

                #region 初始化CustomTimePeriodDataCollection
                foreach (DayOfWeek item in Enum.GetValues(typeof(DayOfWeek)))
                {
                    WeekInfo info = new WeekInfo();
                    info.IsSelected = false;
                    info.WeekValue = item;
                    CustomSelectedWeekCollection.Add(info);
                }
                #endregion

                #region 初始化RepetitionState
                foreach (RepetitionState item in Enum.GetValues(typeof(RepetitionState)))
                {
                    string msg = "";
                    switch (item)
                    {
                        case RepetitionState.MonToFri: CommonStaticMethod.GetLanguageString("周一至周五", "Lang_Bright_MonToFri", out msg); break;
                        case RepetitionState.EveryDay: CommonStaticMethod.GetLanguageString("每天", "Lang_Bright_EveryDay", out msg); break;
                        case RepetitionState.Custom: CommonStaticMethod.GetLanguageString("自定义", "Lang_Bright_Custom", out msg); break;
                        default: break;
                    }

                    RepetitionStateDataSource.Add(new ComboBoxDataSet(msg, item));
                }
                SelectedRepetitionState = RepetitionState.EveryDay;
                #endregion

                
            }
        }
        #endregion

        #region 私有
        private void OnOK()
        {
            Messenger.Default.Send(MsgToken.MSG_ADDBRIGHTTIMINGADJUSTDATA_OK, MsgToken.MSG_ADDBRIGHTTIMINGADJUSTDATA_CLOSE);
        }
        private void OnCancel()
        {
            Messenger.Default.Send(MsgToken.MSG_ADDBRIGHTTIMINGADJUSTDATA_CANCEL, MsgToken.MSG_ADDBRIGHTTIMINGADJUSTDATA_CLOSE);
        }

        private void ResetWeekSelected(RepetitionState state)
        {
            foreach (WeekInfo info in CustomSelectedWeekCollection)
            {
                if (state == RepetitionState.EveryDay)
                {
                    info.IsSelected = true;
                }
                else if (state == RepetitionState.MonToFri)
                {
                    if (info.WeekValue == DayOfWeek.Saturday ||
                        info.WeekValue == DayOfWeek.Sunday)
                    {
                        info.IsSelected = false;
                    }
                    else
                    {
                        info.IsSelected = true;
                    }
                }
            }
        }
        
        private void FixParamToSmartBrightItem(OneSmartBrightEasyConfig config)
        {
            config.BrightPercent = BrightValue;

            if (SelectedSmartBrightMode == SmartBrightAdjustType.AutoBright)
            {
                config.BrightPercent = -1.0f;
            }
            List<DayOfWeek> weekInfo = new List<DayOfWeek>();

            foreach (WeekInfo info in CustomSelectedWeekCollection)
            {
                if (info.IsSelected)
                {
                    weekInfo.Add(info.WeekValue);
                }
            }
            config.ScheduleType = SelectedSmartBrightMode;
            config.StartTime = AdjustTime;
            config.CustomDayCollection = weekInfo;
            
            Random rand = new Random((int)DateTime.Now.ToFileTimeUtc());
            
            config.IsConfigEnable = true;
        }

        private void UpdateSmartBrightToUI(OneSmartBrightEasyConfig config)
        {
            if (config != null)
            {
                SelectedRepetitionState = CommonStaticMethod.GetWeekRepetition(config.CustomDayCollection);
                if (config.ScheduleType != SmartBrightAdjustType.FixBright)
                {
                    BrightValue = 100;
                }
                else
                {
                    BrightValue = config.BrightPercent;
                }
                foreach (WeekInfo info in CustomSelectedWeekCollection)
                {
                    if (config.CustomDayCollection.Contains(info.WeekValue))
                    {
                        info.IsSelected = true;
                    }
                    else
                    {
                        info.IsSelected = false;
                    }
                }

                SelectedSmartBrightMode = config.ScheduleType;
                AdjustTime = config.StartTime;
            }
        }
        #endregion
    }
}
