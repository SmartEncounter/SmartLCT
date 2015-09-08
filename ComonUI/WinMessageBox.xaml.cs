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

namespace Nova.SmartLCT.UI
{
	/// <summary>
	/// WinMessageBox.xaml 的交互逻辑
	/// </summary>
    public partial class WinMessageBox : Window
    {
        public enum ButtonSelectedType { OK, Cancel, Yes, No, };

        public MessageBoxButton MsgButton
        {
            get;
            set;
        }

        public string Caption
        {
            get;
            set;
        }

        public string MsgContent
        {
            get;
            set;
        }

        public MessageBoxImage MsgImage
        {
            get;
            set;
        }

        public ButtonSelectedType ButtonSelected
        {
            get { return _buttonSelected; }
        }
        private ButtonSelectedType _buttonSelected = ButtonSelectedType.Cancel;

        private BitmapImage ErrorImg = null;
        private BitmapImage QuestionImg = null;
        private BitmapImage InformationImg = null;

        private enum ImgType {Error, Question, Information }
        public WinMessageBox()
		{
			this.InitializeComponent();
			
			// 在此点之下插入创建对象所需的代码。
            //BitmapImage bmp
            ErrorImg = GetBitmapImg(ImgType.Error);
            QuestionImg = GetBitmapImg(ImgType.Question);
            InformationImg = GetBitmapImg(ImgType.Information);
		}


        private BitmapImage GetBitmapImg(ImgType imgType)
        {
            Uri uri = null;
            switch (imgType)
            {
                case ImgType.Error: uri = new Uri("/Nova.SmartLCT.UI.CommonUI;component/images/MessageBox/Error.png", UriKind.RelativeOrAbsolute); break;
                case ImgType.Question: uri = new Uri("/Nova.SmartLCT.UI.CommonUI;component/images/MessageBox/Question.png", UriKind.RelativeOrAbsolute); break;
                case ImgType.Information:
                default: uri = new Uri("/Nova.SmartLCT.UI.CommonUI;component/images/MessageBox/Alert.png", UriKind.RelativeOrAbsolute); break;
            }
            BitmapImage bmpImg = new BitmapImage(uri);
            bmpImg.CacheOption = BitmapCacheOption.OnLoad;
            return bmpImg;
        }

        public new bool? ShowDialog()
        {
            #region 控制Buttons的显示
            bt_OK.Visibility = Visibility.Collapsed;
            bt_Yes.Visibility = Visibility.Collapsed;
            bt_No.Visibility = Visibility.Collapsed;
            bt_cancel.Visibility = Visibility.Collapsed;
            
            switch (MsgButton)
            {
                case MessageBoxButton.OK: bt_OK.Visibility = Visibility.Visible; break;
                case MessageBoxButton.OKCancel:
                    {
                        bt_OK.Visibility = bt_cancel.Visibility = Visibility.Visible;
                    }break;
                case MessageBoxButton.YesNo:
                    {
                        bt_Yes.Visibility = bt_No.Visibility = Visibility.Visible;
                    } break;
                case MessageBoxButton.YesNoCancel:
                    {
                        bt_Yes.Visibility = bt_No.Visibility = bt_cancel.Visibility = Visibility.Visible;
                    } break;
                default:
                    bt_OK.Visibility = Visibility.Visible; break;
            }
            #endregion

            #region 控制图标的显示
            switch (MsgImage)
            {
                case MessageBoxImage.Error: img_msgIcon.Source = ErrorImg; break;
                case MessageBoxImage.Question: img_msgIcon.Source = QuestionImg; break;
                case MessageBoxImage.Information:
                default: img_msgIcon.Source = InformationImg; break;
            }
            #endregion

            this.Title = Caption;

            this.tbk_msg.Text = MsgContent;

            return base.ShowDialog();
        }

        private void bt_OK_Click(object sender, RoutedEventArgs e)
        {
            _buttonSelected = ButtonSelectedType.OK;
            this.DialogResult = true;
        }

        private void bt_Yes_Click(object sender, RoutedEventArgs e)
        {
            _buttonSelected = ButtonSelectedType.Yes;
            this.DialogResult = true;
        }

        private void bt_No_Click(object sender, RoutedEventArgs e)
        {
            _buttonSelected = ButtonSelectedType.No;
            this.DialogResult = false;
        }

        private void bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            _buttonSelected = ButtonSelectedType.Cancel;
            this.DialogResult = false;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //if (this.Owner != null)
            //{
            //    Window owner = this.Owner;
            //    this.Left = owner.Left + (owner.Width - this.Width) / 2;
            //    this.Top = owner.Top + (owner.Height - this.Height) / 2;
            //}
        }
    }
}