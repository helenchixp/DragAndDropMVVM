using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DragAndDropMVVM.Behavior
{
    /// <summary>
    /// The Connection Line Adorner
    /// </summary>
    public class DrawLineAdorner : Adorner
    {
        private Line _line;
        protected Vector _move;
        private Point _startPoint = new Point(0, 0);


        public DrawLineAdorner(UIElement element, double opacity, Point point) : base(element)
        {
            _startPoint = point;
            _line = new Line()
            {
                Stroke = Brushes.Gray,
                StrokeThickness = 2,
                StrokeDashArray = new DoubleCollection(new double[] { 1, 1, 1, 1, }), //破線のスタイル
                X1 = point.X,
                Y1 = point.X,
                X2 = point.X,
                Y2 = point.Y,
            };

            _move = (Vector)Window.GetWindow(element).PointFromScreen(element.PointToScreen(point));
            _move.Negate();

            AdornerLayer adorner = AdornerLayer.GetAdornerLayer((Visual)WPFUtil.FindVisualParent<Window>(this.AdornedElement).Content);
            if (adorner != null) adorner.Add(this);

        }

        private Point _position;

        public Point Position
        {
            get { return _position; }
            set
            {
                _position = (Point)(value + _move);
                UpdatePosition();
            }
        }
        protected void UpdatePosition()
        {
            AdornerLayer adorner = this.Parent as AdornerLayer;
            if (adorner != null) adorner.Update(this.AdornedElement);
        }


        public void Remove()
        {
            AdornerLayer adorner = this.Parent as AdornerLayer;
            if (adorner != null) adorner.Remove(this);
        }
        protected override Visual GetVisualChild(int index)
        {
            return _line;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        protected override Size MeasureOverride(Size finalSize)
        {
            //System.Diagnostics.Debug.WriteLine("###########    StartPoint    " + _startPoint.ToString());
            //System.Diagnostics.Debug.WriteLine("###########    Current    " + Position.ToString());
            //System.Diagnostics.Debug.WriteLine("finalSize :" + finalSize.ToString() + "_ghost.DesiredSize:" + _line.DesiredSize);

            //System.Diagnostics.Debug.Write(string.Format("_ghost Size {0}, {1}, {2} , {3}" , _ghost.X1,_ghost.Y1, _ghost.X2, _ghost.Y2));

            //System.Diagnostics.Debug.WriteLine("###########      " + Position.ToString());

            _line.X1 = _startPoint.X - Position.X;
            _line.Y1 = _startPoint.Y - Position.Y;

            //System.Diagnostics.Debug.WriteLine($"........Line X1={_line.X1}  Y1={_line.Y1}  X2={_line.X2}  Y2={_line.Y2}");

            _line.Measure(finalSize);

            return _line.DesiredSize;


        }

        protected override Size ArrangeOverride(Size finalSize)
        {

            _line.Arrange(new Rect(_line.DesiredSize));

            return finalSize;
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(Position.X, Position.Y));


            return result;
        }

        public Tuple<double, double, double, double> GetLineStartEndPosition()
        {
            return new Tuple<double, double, double, double>(_line.X1, _line.Y1, _line.X2, _line.Y2);
        }

    }
}
