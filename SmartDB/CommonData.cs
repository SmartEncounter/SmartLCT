using Nova.LCT.GigabitSystem.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nova.SmartLCT.Database
{
    public class SmartBrightSeleCondition
    {
        public BrightAdjustMode BrightAdjMode
        {
            get;
            set;
        }

        public DisplaySmartBrightEasyConfig EasyConfig
        {
            get;
            set;
        }

        public int DataVersion
        {
            get;
            set;
        }
    }

}
