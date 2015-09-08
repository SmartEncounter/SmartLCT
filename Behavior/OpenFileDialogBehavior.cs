using GalaSoft.MvvmLight;
using System.Windows.Interactivity;
using System.Windows;
using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Nova.SmartLCT.Behavior
{
    public class OpenFileDialogBehavior : Behavior<System.Windows.Controls.Button>
    {
        #region DependencyProperty
        public string SelectedFileName
        {
            get { return (string)GetValue(SelectedFileNameProperty); }
            set { SetValue(SelectedFileNameProperty, value); }
        }

        public static readonly DependencyProperty SelectedFileNameProperty =
            DependencyProperty.Register("SelectedFileName", typeof(string), typeof(OpenFileDialogBehavior), new UIPropertyMetadata(""));

        public List<string> SelectedFileNames
        {
            get { return (List<string>)GetValue(SelectedFileNamesProperty); }
            set { SetValue(SelectedFileNamesProperty, value); }
        }

        public static readonly DependencyProperty SelectedFileNamesProperty =
            DependencyProperty.Register("SelectedFileNames",typeof(List<string>), typeof(OpenFileDialogBehavior), new UIPropertyMetadata(new List<string>()));

        public string FilterString
        {
            get { return (string)GetValue(FilterStringProperty); }
            set { SetValue(FilterStringProperty, value); }
        }
        public static readonly DependencyProperty FilterStringProperty =
            DependencyProperty.Register("FilterString", typeof(string), typeof(OpenFileDialogBehavior), new UIPropertyMetadata(""));

        public bool IsFileNameValid
        {
            get { return (bool)GetValue(IsFileNameValidProperty); }
            set
            {
                SetValue(IsFileNameValidProperty, value);
            }
        }
        public static readonly DependencyProperty IsFileNameValidProperty =
            DependencyProperty.Register("IsFileNameValid", typeof(bool), typeof(OpenFileDialogBehavior), new UIPropertyMetadata(false));

        public bool IsMultiselect
        {
            get { return (bool)GetValue(IsMultiselectValidProperty); }
            set
            {
                SetValue(IsMultiselectValidProperty, value);
            }
        }
        public static readonly DependencyProperty IsMultiselectValidProperty =
            DependencyProperty.Register("IsMultiselect", typeof(bool), typeof(OpenFileDialogBehavior), new UIPropertyMetadata(false));

        #endregion
        public OpenFileDialogBehavior()
        {
           
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            // 插入要在将 Behavior 附加到对象时运行的代码。


            this.AssociatedObject.Click += new RoutedEventHandler(Button_Click);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.AddExtension = true;
                ofd.Filter = this.FilterString;
                ofd.Multiselect = IsMultiselect;
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    IsFileNameValid = false;
                }
                else
                {
                    SelectedFileName = ofd.FileName;
                    IsFileNameValid = true;
                    foreach (string fileName in ofd.FileNames)
                    {
                        SelectedFileNames.Add(fileName);
                    }
                }
            }

        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            // 插入要在从对象中删除 Behavior 时运行的代码。
            this.AssociatedObject.Click -= new RoutedEventHandler(Button_Click);
        }
    }
}