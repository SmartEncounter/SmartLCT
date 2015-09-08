using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Nova.SmartLCT.SmartLCTControl;
using Nova.SmartLCT.Interface;

namespace CommonAdorner
{
    public class ScaleAdorner : Adorner
    {
        public ScaleAdorner(UIElement adornedElement,int num,double height,double width,double mouseX,double mouseY)
            : base(adornedElement)
        {
            if (adornedElement is FrameworkElement)
            {
                _adornedElement = adornedElement as FrameworkElement;
                CreateGrip(num,height,width);
                _mouseX = mouseX;
                _mouseY = mouseY;
            }
        }
        private double _mouseX;
        private double _mouseY;
        private void CreateGrip(int num,double height,double width)
        {
            // Scaling grip
            SmartLCTControl sControl = new SmartLCTControl();
          //  double h=hei
            RectLayer myRectLayer3 = new RectLayer(0, 0, 100, 100, null, 0, ElementType.baselayer, 0);
            RectLayer Layer3_sender1 = new RectLayer(0, 0, 100, 100, myRectLayer3, 3, ElementType.screen, 0);
            myRectLayer3.ElementSelectedState = SelectedState.None;
            myRectLayer3.ElementCollection.Add(Layer3_sender1);
            sControl.MyRectLayer = myRectLayer3;
            if(sControl._scrollViewer!=null)
            sControl._scrollViewer.Visibility=Visibility.Hidden;
                //Rectangle rect = new Rectangle();
                //rect.Stroke = Brushes.Blue;
                //rect.Fill = Brushes.White;
                //rect.Cursor = Cursors.SizeNWSE;

                //rect.MouseDown += OnGripMouseDown;
                //rect.MouseUp += OnGripMouseUp;
                //rect.MouseMove += OnGripMouseMove;
                AddVisualChild(sControl);
                _scalingGrip = sControl;
        


           
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return _adornedElement != null ? 1 : 0;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (_adornedElement != null)
                return _scalingGrip;
            return base.GetVisualChild(index);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Size size = base.ArrangeOverride(finalSize);
            if (_scalingGrip != null)
               
               _scalingGrip.Arrange(new Rect(_mouseX, _mouseY, 100, 100));
            return size;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            //SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
            //Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1);
            //renderPen.DashStyle = new DashStyle(new double[] { 2.5, 2.5 }, 0);

            //Rect rect = new Rect(0, 0, _adornedElement.ActualWidth, _adornedElement.ActualHeight);
            //drawingContext.DrawRectangle(Brushes.Transparent, renderPen, rect);

            base.OnRender(drawingContext);
        }

        //private void OnGripMouseDown(object sender, MouseEventArgs args)
        //{
        //    if (args.LeftButton != MouseButtonState.Pressed)
        //        return;

        //    Rectangle rect = sender as Rectangle;
        //    if (rect != null)
        //        Mouse.Capture(rect);
        //}

        //private void OnGripMouseUp(object sender, MouseEventArgs args)
        //{
        //    Rectangle rect = sender as Rectangle;
        //    if (rect != null)
        //        Mouse.Capture(null);
        //}

        //private void OnGripMouseMove(object sender, MouseEventArgs args)
        //{
        //    if (args.LeftButton != MouseButtonState.Pressed)
        //        return;

        //    Rectangle rect = sender as Rectangle;
        //    if (rect == null || _adornedElement == null)
        //        return;

        //    Point point = args.GetPosition(_adornedElement);
        //    _adornedElement.Width = point.X > 0 ? point.X : 0;
        //    _adornedElement.Height = point.Y > 0 ? point.Y : 0;
        //}

        SmartLCTControl _scalingGrip = null;
        FrameworkElement _adornedElement = null;
    }
}