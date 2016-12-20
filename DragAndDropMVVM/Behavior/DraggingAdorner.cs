using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DragAndDropMVVM.Behavior
{
    public class DraggingAdorner : Adorner
    {
        private Rectangle _ghost;
        protected Vector _move;

        public DraggingAdorner(UIElement element, double opacity, Point point) : base(element)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(element);
            _ghost = new Rectangle()
            {
                Height = bounds.Height,
                Width = bounds.Width,
                Fill = new VisualBrush(element) { Opacity = opacity }
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

        public void Remove()
        {
            AdornerLayer adorner = this.Parent as AdornerLayer;
            if (adorner != null) adorner.Remove(this);
        }

        protected void UpdatePosition()
        {
            AdornerLayer adorner = this.Parent as AdornerLayer;
            if (adorner != null) adorner.Update(this.AdornedElement);
        }

        protected override Visual GetVisualChild(int index)
        {
            return _ghost;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        protected override Size MeasureOverride(Size finalSize)
        {
            _ghost.Measure(finalSize);
            return _ghost.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {

            _ghost.Arrange(new Rect(_ghost.DesiredSize));
            return finalSize;
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(Position.X, Position.Y));
            return result;
        }
    }
}
