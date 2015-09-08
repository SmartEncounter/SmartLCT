using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using Nova.SmartLCT.Interface;
using System.Windows.Data;

namespace CommonAdorner
{
public class DragAdorner : Adorner
{
    #region 变量
    protected UIElement _child;
    protected VisualBrush _brush;
    protected UIElement _owner;
    protected double XCenter;
    protected double YCenter;

    private double _leftOffset;
    private double _topOffset;
    public double Scale = 1.0;
    #endregion
    public TextBlock textBlock = new TextBlock();

    #region 构造函数
    public DragAdorner(UIElement owner) : base(owner) { }

    public DragAdorner(UIElement owner, double width, double height, Point point, double opacity)
        : base(owner)
    {
        this._owner = owner;
        Rectangle r = new Rectangle();

        r.Width = width;
        r.Height = height;
        
        //XCenter = width / 2;
        ////YCenter = height / 2;
        //XCenter = 0;
        //YCenter = 0;
        r.Opacity = opacity;
        r.Stroke = Function.StrToBrush("#FFB8B8B8");
        r.StrokeThickness = 1;
        r.Fill = Function.StrToBrush("#FF5E626B");
        //r.Margin = new Thickness(500, 500, point.X + width, point.Y + height);
        this._child = r;
    }
    public DragAdorner(UIElement owner, DragAdorner element)
        : base(owner)
    {
        this._owner = owner;
        
        textBlock.Text = "X=" + Math.Round(element.LeftOffset).ToString() + "  Y=" + Math.Round(element.TopOffset).ToString();
        textBlock.Foreground = Brushes.White;
        this._child = textBlock;
    }
    /// <summary>
    /// 添加箱体时用
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="rows"></param>
    /// <param name="cols"></param>
    /// <param name="point"></param>
    /// <param name="opacity"></param>
    public DragAdorner(UIElement owner,double width,double height,double rows,double cols,Point point, double opacity)
        : base(owner)
    {
        this._owner = owner;
        //VisualBrush _brush = new VisualBrush(adornElement);
        //_brush.Opacity = opacity;
        Canvas canvas = new Canvas();
        canvas.Height = height * rows+4;
        canvas.Width = width * cols+4;
        canvas.Background = Function.StrToBrush("#FFB8B8B8");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Rectangle r = new Rectangle();

                r.Width = width;
                r.Height = height;

                XCenter = width/2;
                YCenter = height/2;

                r.Stroke = Function.StrToBrush("#FFB8B8B8");
                r.StrokeThickness = 0.6;
                r.Fill = Function.StrToBrush("#FF6F6F6F");
                canvas.Children.Add(r);
                r.SetValue(Canvas.LeftProperty, width * j+2);
                r.SetValue(Canvas.TopProperty, height * i+2);
            }
        }
        this._child = canvas;
    }
    public DragAdorner(UIElement owner, double opacity)
        : base(owner)
    {
        this._owner = owner;
        //  VisualBrush _brush = new VisualBrush();
        // _brush.Opacity = opacity;
        Rectangle r = new Rectangle();
        r.RadiusX = 3;
        r.RadiusY = 3;

        r.Width = 100;
        r.Height = 100;

        XCenter = 100 / 2;
        YCenter = 100 / 2;

        r.Fill = Brushes.Red;
        this._child = r;
    }
    #endregion

    #region 属性
    public double LeftOffset
    {
        get { return this._leftOffset; }
        set
        {
            this._leftOffset = value - XCenter;
            this.UpdatePosition();
        }
    }

    public double TopOffset
    {
        get { return this._topOffset; }
        set
        {
            this._topOffset = value - YCenter;
            this.UpdatePosition();
        }
    }

    protected override int VisualChildrenCount
    {
        get
        {
            return 1;
        }
    }
    #endregion

    #region 方法
    private void UpdatePosition()
    {
        AdornerLayer adorner = (AdornerLayer)this.Parent;
        if (adorner != null)
        {
            adorner.Update(this.AdornedElement);
        }
    }

    protected override Visual GetVisualChild(int index)
    {
        return _child;
    }

    protected override Size MeasureOverride(Size finalSize)
    {
        this._child.Measure(finalSize);
        return this._child.DesiredSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        this._child.Arrange(new Rect(_child.DesiredSize));
        return finalSize;
    }

    public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
    {
        GeneralTransformGroup result = new GeneralTransformGroup();

        result.Children.Add(base.GetDesiredTransform(transform));
        result.Children.Add(new TranslateTransform(this._leftOffset, this._topOffset));
        return result;
    }
    #endregion
}
}
