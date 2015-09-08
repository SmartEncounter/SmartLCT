using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using Nova.SmartLCT.Interface;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;

namespace Nova.SmartLCT.UI
{
    public class CustomText:Control
    {
        #region 定义xmal属性
        /// <summary>
        /// 控件的IsLockProperty属性值
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(CustomText));
        
        private static readonly DependencyProperty SenderConfigCollectionProperty =
            DependencyProperty.Register("SenderConfigCollection", typeof(ObservableCollection<SenderConfigInfo>), typeof(CustomText));
        
        public static readonly DependencyProperty SelectedSenderConfigInfoProperty =
            DependencyProperty.Register("SelectedSenderConfigInfo", typeof(SenderConfigInfo), typeof(CustomText));
        public static readonly DependencyProperty CurrentSenderConfigInfoProperty =
            DependencyProperty.Register("CurrentSenderConfigInfo", typeof(SenderConfigInfo), typeof(CustomText));
        public static readonly DependencyProperty ScreenRealParamsProperty =
            DependencyProperty.Register("ScreenRealParams", typeof(ScreenRealParameters), typeof(CustomText));
        #endregion
        #region 属性
        public ScreenRealParameters ScreenRealParams
        {
            get { return (ScreenRealParameters)GetValue(ScreenRealParamsProperty); }
            set
            {
                SetValue(ScreenRealParamsProperty, value);
            }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public SenderConfigInfo SelectedSenderConfigInfo
        {
            get { return (SenderConfigInfo)GetValue(SelectedSenderConfigInfoProperty); }
            set { SetValue(SelectedSenderConfigInfoProperty, value); }
        }
        public ObservableCollection<SenderConfigInfo> SenderConfigCollection
        {
            get { return (ObservableCollection<SenderConfigInfo>)GetValue(SenderConfigCollectionProperty); }

            set
            {
                SetValue(SenderConfigCollectionProperty, value);
            }
        }
        private ObservableCollection<SenderConfigInfo> _senderConfigCollection = new ObservableCollection<SenderConfigInfo>();
        public SenderConfigInfo CurrentSenderConfigInfo
        {
            get { return (SenderConfigInfo)GetValue(CurrentSenderConfigInfoProperty); }
            set { SetValue(CurrentSenderConfigInfoProperty, value); }
        }

        #endregion
        public static RoutedCommand MouseDoubleClickWithSenderType
        {
            get { return _mouseDoubleClickWithSenderType; }
        }
        private static RoutedCommand _mouseDoubleClickWithSenderType;
         #region 构造函数
        static CustomText()
        {
            InitializeCommands();
            EventManager.RegisterClassHandler(typeof(CustomText),
             Mouse.MouseDownEvent, new MouseButtonEventHandler(CustomText.OnMouseLeftButtonDown), true);
            EventManager.RegisterClassHandler(typeof(CustomText),
            Mouse.MouseUpEvent, new MouseButtonEventHandler(CustomText.OnMouseUp), true);

            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomText), new FrameworkPropertyMetadata(typeof(CustomText)));
        }
        private static void InitializeCommands()
        {
            _mouseDoubleClickWithSenderType = new RoutedCommand("MouseDoubleClickWithSenderType", typeof(CustomText));
            CommandManager.RegisterClassCommandBinding(typeof(CustomText), new CommandBinding(_mouseDoubleClickWithSenderType, OnMouseDoubleClickWithSenderType));
        }

        #endregion

        private static void OnMouseDoubleClickWithSenderType(object sender, ExecutedRoutedEventArgs e)
        {
            CustomText control = sender as CustomText;
            if (control != null)
            {
                control.OnMouseDoubleClickWithSenderType();
            }
        }
        private bool _isCompleteChangeSenderType = false;
        private void OnMouseDoubleClickWithSenderType()
        {
            if (SelectedSenderConfigInfo == null)
            {
                return;
            }
            if (CurrentSenderConfigInfo == SelectedSenderConfigInfo)
            {
                return;
            }
            NotificationMessageAction<SenderConfigInfo> nsa =
     new NotificationMessageAction<SenderConfigInfo>(SelectedSenderConfigInfo, "", MsgToken.MSG_ISMODIFYSENDERTYPE, SetChangedSenderTypeCallBack);
            Messenger.Default.Send(nsa, MsgToken.MSG_ISMODIFYSENDERTYPE);
            _isCompleteChangeSenderType = true;
        }
        private void SetChangedSenderTypeCallBack(SenderConfigInfo info)
        {
                CurrentSenderConfigInfo=info;

        }
        private void OnCurrentSenderConfigInfo()
        {
            if (ScreenRealParams.ScreenLayer != null && ScreenRealParams.ScreenLayer.SenderConnectInfoList != null)
            {
                RectLayer removeLayer = (RectLayer)ScreenRealParams.ScreenLayer;
                while (removeLayer != null && removeLayer.EleType != ElementType.baseScreen)
                {
                    removeLayer = (RectLayer)removeLayer.ParentElement;
                }
                if (removeLayer == null)
                {
                    return;
                }
                for (int j = 0; j < removeLayer.ElementCollection.Count; j++)
                {
                    if (removeLayer.ElementCollection[j].EleType == ElementType.newLayer)
                    {
                        continue;
                    }
                    RectLayer reLayer = (RectLayer)((RectLayer)removeLayer.ElementCollection[j]).ElementCollection[0];
                    for (int m = 0; m < removeLayer.SenderConnectInfoList.Count;m++ )
                        UpdateSenderConnectInfo(reLayer, reLayer.SenderConnectInfoList[m]);
                }
            }
        }

        private void UpdateSenderConnectInfo(RectLayer layer, SenderConnectInfo senderConnectInfo)
        {
            senderConnectInfo.IsExpand = false;
            senderConnectInfo.IsSelected = false;
            for (int m = 0; m < senderConnectInfo.PortConnectInfoList.Count; m++)
            {
                if (senderConnectInfo.PortConnectInfoList[m].ConnectLineElementList != null)
                {
                    ObservableCollection<IRectElement> connectLineElementList = senderConnectInfo.PortConnectInfoList[m].ConnectLineElementList;
                    for (int i = 0; i < connectLineElementList.Count; i++)
                    {
                        if (connectLineElementList[i].FrontLine != null)
                        {
                            layer.ElementCollection.Remove(connectLineElementList[i].FrontLine);
                        }
                        if (connectLineElementList[i].EndLine != null)
                        {
                            layer.ElementCollection.Remove(connectLineElementList[i].EndLine);
                        }
                        connectLineElementList[i].ConnectedIndex = -1;
                        connectLineElementList[i].SenderIndex = -1;
                        connectLineElementList[i].PortIndex = -1;
                    }
                    senderConnectInfo.PortConnectInfoList[m].ConnectLineElementList.Clear();
                }


                senderConnectInfo.PortConnectInfoList[m].MaxConnectIndex = -1;
                senderConnectInfo.PortConnectInfoList[m].MaxConnectElement = null;

                senderConnectInfo.PortConnectInfoList[m].IsSelected = false;
            }
        }


        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CustomText control = (CustomText)sender;

            // When someone click on a part in the NumericUpDown and it's not focusable
            // NumericUpDown needs to take the focus in order to process keyboard correctly
            if (!control.IsKeyboardFocusWithin)
            {
                e.Handled = control.Focus() || e.Handled;
            }
            control.OnDown();
        }
        private void OnDown()
        {
            if (_isCompleteChangeSenderType)
            {
                _isCompleteChangeSenderType = false;

                return;
            }
            _myPopup.IsOpen = true;
           // _myPopup.StaysOpen = false;

            _myPopup.StaysOpen = true;
            _isCompleteChangeSenderType = false;
        }
        private static void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            CustomText control = (CustomText)sender;
            control.OnUp();
        }
        private void OnUp()
        {
            _myPopup.StaysOpen = false;

        }
        #region  重载
        private Popup _myPopup = null;
        private TextBlock _myTextBlock = null;
        public override void OnApplyTemplate()
        {
            var myTextBlock = this.GetTemplateChild("MyTextBlock") as TextBlock;
            var mypopup = this.GetTemplateChild("Mypopup") as Popup;
            _myPopup = mypopup;
            _myTextBlock = myTextBlock;
        }
        #endregion
    }
}
