using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Input;
using DragAndDropMVVM.ViewModel;

namespace DragAndDropMVVM.Behavior
{
    public class FrameworkElementDragBehavior : Behavior<FrameworkElement>
    {
        private bool _isMouseClicked = false;

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
            this.AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            this.AssociatedObject.QueryContinueDrag += AssociatedObject_QueryContinueDrag;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;
            this.AssociatedObject.MouseLeave -= AssociatedObject_MouseLeave;
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            this.AssociatedObject.QueryContinueDrag -= AssociatedObject_QueryContinueDrag;

        }

        #region EventHander Method
        private Boolean IsDragging(Point pointA, Point pointB)
        {
            if (Math.Abs(pointA.X - pointB.X) > SystemParameters.MinimumHorizontalDragDistance) return true;
            if (Math.Abs(pointA.Y - pointB.Y) > SystemParameters.MinimumVerticalDragDistance) return true;
            return false;
        }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            UIElement element = sender as UIElement;

            if (e.LeftButton == MouseButtonState.Released) element.SetValue(FrameworkElementDragBehavior.StartPointProperty, null);
            if (element.GetValue(FrameworkElementDragBehavior.StartPointProperty) == null) return;

            Point startPoint = (Point)element.GetValue(FrameworkElementDragBehavior.StartPointProperty);
            Point point = e.GetPosition(element);

            if (!IsDragging(startPoint, point)) return;

            System.Diagnostics.Debug.WriteLine($"{nameof(AssociatedObject_MouseMove)} Current Point : X:{point.X} Y:{point.Y}");

            DraggingAdorner adorner = new DraggingAdorner(element, 0.5, point);
            FrameworkElementDragBehavior.SetDragAdorner(element, adorner);

            DragDrop.DoDragDrop(element, element, DragDropEffects.Copy | DragDropEffects.Move);

            adorner.Remove();
            FrameworkElementDragBehavior.SetDragAdorner(element, null);
        }


        private void AssociatedObject_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            UIElement element = sender as UIElement;
            DraggingAdorner adorner = FrameworkElementDragBehavior.GetDragAdorner(element);
            if (adorner != null) adorner.Position = WPFUtil.GetMousePosition(element);
        }

        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseClicked = true;

            e.Handled = true;

            UIElement element = sender as UIElement;

            element.SetValue(FrameworkElementDragBehavior.StartPointProperty, e.GetPosition(element));

        }

        private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseClicked = false;

            UIElement element = sender as UIElement;

            element.SetValue(FrameworkElementDragBehavior.StartPointProperty, null);

        }

        private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_isMouseClicked)
            {
                //set the item's DataContext as the data to be transferred
                IDragable dragObject = this.AssociatedObject.DataContext as IDragable;
                if (dragObject != null)
                {
                    DataObject data = new DataObject();
                    data.SetData(dragObject.DataType, this.AssociatedObject.DataContext);
                    System.Windows.DragDrop.DoDragDrop(this.AssociatedObject, data, DragDropEffects.Move);
                }
            }

            _isMouseClicked = false;


        }

        #endregion

        #region Dependency Property

        private static readonly DependencyProperty StartPointProperty = DependencyProperty.RegisterAttached("StartPoint", typeof(Point?), typeof(FrameworkElementDragBehavior), new PropertyMetadata(null));

        private static readonly DependencyProperty DragAdornerProperty = DependencyProperty.RegisterAttached("DragAdorner", typeof(DraggingAdorner), typeof(FrameworkElementDragBehavior), new PropertyMetadata(null));

        public static DraggingAdorner GetDragAdorner(DependencyObject obj)
        {
            return (DraggingAdorner)obj.GetValue(DragAdornerProperty);
        }

        public static void SetDragAdorner(DependencyObject obj, DraggingAdorner value)
        {
            obj.SetValue(DragAdornerProperty, value);
        }

        #endregion


    }
}
