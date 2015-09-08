using Nova.LCT.GigabitSystem.Common;
using Nova.SmartLCT.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Test
{
	/// <summary>
	/// Window6.xaml 的交互逻辑
	/// </summary>
	public partial class Window6 : Window
	{
		public Window6()
		{
			this.InitializeComponent();
			
			// 在此点之下插入创建对象所需的代码。
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SQLiteAccessor accessor = SQLiteAccessor.Instance;
            
            if (accessor == null)
            {
                MessageBox.Show("初始化数据库失败！");
                return;
            }
            DisplaySmartBrightEasyConfig cfg = GetConfig();
            SmartBrightSeleCondition condition = new SmartBrightSeleCondition()
            {
                BrightAdjMode = BrightAdjustMode.SmartBright,
                DataVersion = 1,
                EasyConfig = cfg
            };
            accessor.SaveDisplayEasyConfig(cfg.DisplayUDID, condition);
            accessor.Dispose();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SQLiteAccessor accessor = new SQLiteAccessor();
            
            if (accessor == null)
            {
                MessageBox.Show("初始化数据库失败！");
                return;
            }
            SmartBrightSeleCondition cfg = accessor.LoadDisplayEasyConfig("0123456789");
            if (cfg == null)
            {
                MessageBox.Show("无法获取指定数据");
            }
            accessor.Dispose();
        }

        private DisplaySmartBrightEasyConfig GetConfig()
        {
            DisplaySmartBrightEasyConfig config = new DisplaySmartBrightEasyConfig();
            config.DisplayUDID = "0123456789";
            config.OneDayConfigList = new List<OneSmartBrightEasyConfig>();

            OneSmartBrightEasyConfig oneItem = new OneSmartBrightEasyConfig()
            {
                ScheduleType = SmartBrightAdjustType.FixBright,
                StartTime = DateTime.Now.AddMinutes(1),
                BrightPercent = 60,
                CustomDayCollection = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Tuesday }
            };
            //config.OneDayConfigList.Add(oneItem);

            oneItem = new OneSmartBrightEasyConfig()
            {
                ScheduleType = SmartBrightAdjustType.AutoBright,
                StartTime = DateTime.Now.AddMinutes(1),
                BrightPercent = 60,
                CustomDayCollection = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Tuesday }
            };
            config.OneDayConfigList.Add(oneItem);

            oneItem = new OneSmartBrightEasyConfig()
            {
                ScheduleType = SmartBrightAdjustType.FixBright,
                StartTime = DateTime.Now.AddMinutes(11),
                BrightPercent = 40,
                CustomDayCollection = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Tuesday }
            };
            config.OneDayConfigList.Add(oneItem);


            List<PeripheralsLocation> locateList = new List<PeripheralsLocation>();
            PeripheralsLocation location = new PeripheralsLocation()
            {
                FirstSenderSN = "abc",
                SenderIndex = 0,
                SensorType = PeripheralsType.LightSensorOnSender
            };
            locateList.Add(location);

            location = new PeripheralsLocation()
            {
                FirstSenderSN = "acb",
                SenderIndex = 0,
                PortIndex = 0,
                FuncCardIndex = 0,
                SensorIndex = 1,
                SensorType = PeripheralsType.LightSensorOnFuncCardInPort
            };
            locateList.Add(location);

            List<DisplayAutoBrightMapping> maplist = new List<DisplayAutoBrightMapping>();
            DisplayAutoBrightMapping map = new DisplayAutoBrightMapping()
            {
                EnvironmentBright = 50,
                DisplayBright = 30
            };
            maplist.Add(map);

            map = new DisplayAutoBrightMapping()
            {
                EnvironmentBright = 300,
                DisplayBright = 60
            };
            maplist.Add(map);

            AutoBrightExtendData autoData = new AutoBrightExtendData()
            {
                CalcType = AutoBrightCalcType.AllAverage,
                UseLightSensorList = locateList,
                AutoBrightMappingList = maplist
            };
            config.AutoBrightSetting = autoData;

            return config;
        }
	}
}