using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Nova.SmartLCT.Interface;

namespace Test
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private GlobalParameters _globalParams = new GlobalParameters();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Application.Current.Resources.Add(LCTConstData.GLOBAL_PARAMES_KEY, _globalParams);
        }
    }
}
