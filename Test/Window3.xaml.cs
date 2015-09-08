using System;
using System.Collections.Generic;
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
using DragDropDemo1;
using System.Diagnostics;

namespace Test
{
    /// <summary>
    /// Window3.xaml 的交互逻辑
    /// </summary>
    public partial class Window3 : Window
    {
        /// <summary>
        /// 拖动的区域
        /// </summary>
        private FrameworkElement _dragScope;
        /// <summary>
        /// 用于显示鼠标跟随效果的装饰器
        /// </summary>
        private DragAdorner _adorner;
        /// <summary>
        /// 用于呈现DragAdorner的图画
        /// </summary>
        private AdornerLayer _layer;

        public Window3()
        {
            InitializeComponent();
        }

        private void pt_redLight_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) 
            {
                this.StartDrag(e);
            }
        }

        private void StartDrag(MouseEventArgs e)
        {
            this._dragScope = Application.Current.MainWindow.Content as FrameworkElement;

            this._dragScope.AllowDrop = true;

            DragEventHandler draghandler = new DragEventHandler(DragScope_PreviewDragOver);
            this._dragScope.PreviewDragOver += draghandler;

            this._adorner = new DragAdorner(this._dragScope, (UIElement)this.pt_redLight, 0.5);
            this._layer = AdornerLayer.GetAdornerLayer(this._dragScope as Visual);
            this._layer.Add(this._adorner);

            DataObject data = new DataObject(typeof(Image), this.pt_redLight);
            DragDrop.DoDragDrop(this.pt_redLight, data, DragDropEffects.Move);

            AdornerLayer.GetAdornerLayer(this._dragScope).Remove(this._adorner);
            this._adorner = null;

            this._dragScope.PreviewDragOver -= draghandler;
        }

        void DragScope_PreviewDragOver(object sender, DragEventArgs args)
        {
            if (this._adorner != null)
            {
                this._adorner.LeftOffset = args.GetPosition(this._dragScope).X;
                this._adorner.TopOffset = args.GetPosition(this._dragScope).Y;
                Debug.WriteLine("DragScope_PreviewDragOver:" + _adorner.LeftOffset + "," + _adorner.TopOffset);
            }
        }

        private void pt_greenLight_Drop(object sender, DragEventArgs e)
        {
            IDataObject data = e.Data;
            Image img = data.GetData(typeof(Image)) as Image;
            Grid first = (Grid)this.pt_redLight.Parent;
            Grid grid = (Grid)this.pt_greenLight.Parent;
            grid.Children.Remove(pt_greenLight);
            first.Children.Remove(pt_redLight);
            grid.Children.Add(img);
            first.Children.Add(pt_greenLight);
        }

        //private
    }
}
