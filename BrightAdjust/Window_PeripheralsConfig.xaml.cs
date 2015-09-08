using Nova.LCT.GigabitSystem.Common;
using Nova.SmartLCT.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Nova.SmartLCT.UI
{
    /// <summary>
    /// Window_PeripheralsConfig.xaml 的交互逻辑
    /// </summary>
    public partial class Window_PeripheralsConfig : CustomWindow
    {

        private Window_PeripheralsConfig_VM _vm = null;
        public Window_PeripheralsConfig()
        {
            InitializeComponent();
        }

        public Window_PeripheralsConfig(PeripheralsSettingParam cfg)
        {
            InitializeComponent();
            _vm = (Window_PeripheralsConfig_VM)FindResource("Window_PeripheralsConfig_VMDataSource");
            _vm.CmdStartFindPeripherals.Execute(cfg);
        }

        public PeripheralsSettingParam GetPeripheralConfig()
        {
            return _vm.GetPeripheralsSettingParam();
        }

        private void button_ok_Click(object sender, RoutedEventArgs e)
        {
            ValidationError err = GetDataGridRowsFirstError(grid_brightMapping);
            if (err != null)
            {
                _vm.CmdShowValidError.Execute(err.ErrorContent);
                return;
            }
            this.DialogResult = true;
            this.Close();
        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        public static ValidationError GetDataGridRowsFirstError(DataGrid dg) 
        {
            ValidationError err = null; 
            for (int i = 0; i < dg.Items.Count; i++) 
            { 
                DependencyObject o = dg.ItemContainerGenerator.ContainerFromIndex(i); 
                bool hasError = Validation.GetHasError(o); 
                if (hasError) 
                { 
                    err = Validation.GetErrors(o)[0]; 
                    break; 
                } 
            } 
            return err; 
        } 
    }
}
