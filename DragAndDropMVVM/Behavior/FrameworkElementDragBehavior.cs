using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Input;
using System.Windows.Documents;

namespace DragAndDropMVVM.Behavior
{
    /// <summary>
    /// 
    /// </summary>
    public class FrameworkElementDragBehavior : Behavior<FrameworkElement>
    {
        private bool _isMouseClicked = false;

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;

            this.AssociatedObject.PreviewMouseMove += AssociatedObject_MouseMove;
            this.AssociatedObject.QueryContinueDrag += AssociatedObject_QueryContinueDrag;
            this.AssociatedObject.GiveFeedback += AssociatedObject_GiveFeedback;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;

            this.AssociatedObject.PreviewMouseMove -= AssociatedObject_MouseMove;
            this.AssociatedObject.QueryContinueDrag -= AssociatedObject_QueryContinueDrag;
            this.AssociatedObject.GiveFeedback -= AssociatedObject_GiveFeedback;


        }

        #region EventHander Method

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {

            UIElement element = sender as UIElement;

            if (e.LeftButton == MouseButtonState.Released)
            {
                element.SetValue(StartPointProperty, null);
            }
            if (element.GetValue(StartPointProperty) == null) return;

            Point startPoint = (Point)element.GetValue(StartPointProperty);
            Point point = e.GetPosition(element as UIElement);

            if (!WPFUtil.IsDragging(startPoint, point)) return;


            DraggingAdorner adorner = new DraggingAdorner(element, 0.5, point);
            SetDragAdorner(element, adorner);

            DataObject data = new DataObject();

            //set the adorner for drop action and create the clone element.
            data.SetData(typeof(DraggingAdorner), adorner);
            data.SetData(typeof(UIElement), element);

            ICommand dragcommand = GetDragCommand(element);

            //cann't drag without command
            if (dragcommand != null)
            {
                object parameter = GetDragCommandParameter(element); // ?? this.AssociatedObject.DataContext;

                if (parameter != null)
                {
                    data.SetData(DataFormats.Serializable, parameter);
                }

                if (dragcommand.CanExecute(parameter))
                {
                    dragcommand.Execute(parameter);
                    DragDrop.DoDragDrop(element, data, DragDropEffects.Copy | DragDropEffects.Move);
                }
            }

            adorner.Remove();
            
            SetDragAdorner(element, null);

            //System.Diagnostics.Debug.WriteLine($"{nameof(AssociatedObject_MouseMove)} Current Point : X:{point.X} Y:{point.Y}  and Start Point X:{startPoint.X} Y:{startPoint.Y}");

        }


        private void AssociatedObject_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            UIElement element = sender as UIElement;
            DraggingAdorner adorner = GetDragAdorner(element);
            Point point = WPFUtil.GetMousePosition(element);
            if (adorner != null) adorner.Position = point;


        }

        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            _isMouseClicked = true;

            e.Handled = true;

            UIElement element = sender as UIElement;

            element.SetValue(StartPointProperty, e.GetPosition(element));

        }

        private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseClicked = false;

            UIElement element = sender as UIElement;

            System.Diagnostics.Debug.WriteLine($"{nameof(AssociatedObject_MouseLeftButtonUp)} _ {nameof(e.LeftButton)}:{e.LeftButton}");


            element.SetValue(FrameworkElementDragBehavior.StartPointProperty, null);

        }

        private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_isMouseClicked)
            {
                ////set the item's DataContext as the data to be transferred

            }

            _isMouseClicked = false;


        }

        private void AssociatedObject_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
           // System.Diagnostics.Debug.WriteLine($"------------------{nameof(AssociatedObject_GiveFeedback)}--------------------");
        }



        #endregion

        #region Dependency Property

        private static readonly DependencyProperty StartPointProperty = DependencyProperty.RegisterAttached("StartPoint", typeof(Point?), typeof(FrameworkElementDragBehavior), new PropertyMetadata(null));


        #region DragAdorner
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

        #region DragCommand

        /// <summary>
        /// The DragCommand attached property's name.
        /// </summary>
        public const string DragCommandPropertyName = "DragCommand";

        /// <summary>
        /// Gets the value of the DragCommand attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the DragCommand property of the specified object.</returns>
        public static ICommand GetDragCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DragCommandProperty);
        }

        /// <summary>
        /// Sets the value of the DragCommand attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the DragCommand value of the specified object.</param>
        public static void SetDragCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DragCommandProperty, value);
        }

        /// <summary>
        /// Identifies the DragCommand attached property.
        /// </summary>
        public static readonly DependencyProperty DragCommandProperty = DependencyProperty.RegisterAttached(
            DragCommandPropertyName,
            typeof(ICommand),
            typeof(FrameworkElementDragBehavior),
            new UIPropertyMetadata(null));
        #endregion


        /// <summary>
        /// The DragCommandParameter attached property's name.
        /// </summary>
        public const string DragCommandParameterPropertyName = "DragCommandParameter";

        /// <summary>
        /// Gets the value of the DragCommandParameter attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the DragCommandParameter property of the specified object.</returns>
        public static object GetDragCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(DragCommandParameterProperty);
        }

        /// <summary>
        /// Sets the value of the DragCommandParameter attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the DragCommandParameter value of the specified object.</param>
        public static void SetDragCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(DragCommandParameterProperty, value);
        }

        /// <summary>
        /// Identifies the DragCommandParameter attached property.
        /// </summary>
        public static readonly DependencyProperty DragCommandParameterProperty = DependencyProperty.RegisterAttached(
            DragCommandParameterPropertyName,
            typeof(object),
            typeof(FrameworkElementDragBehavior),
            new UIPropertyMetadata(null));

        #endregion




    }
}
