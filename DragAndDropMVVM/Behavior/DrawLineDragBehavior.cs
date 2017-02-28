using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using DragAndDropMVVM.Controls;
using DragAndDropMVVM.ViewModel;

namespace DragAndDropMVVM.Behavior
{
    /// <summary>
    /// 
    /// </summary>
    public class DrawLineDragBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;

            this.AssociatedObject.PreviewMouseMove += AssociatedObject_PreviewMouseMove;
            this.AssociatedObject.QueryContinueDrag += AssociatedObject_QueryContinueDrag;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;

            this.AssociatedObject.PreviewMouseMove -= AssociatedObject_PreviewMouseMove;
            this.AssociatedObject.QueryContinueDrag -= AssociatedObject_QueryContinueDrag;

        }

        #region Event Handler
        private void AssociatedObject_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            var element = GetSenderDiagram(sender) ?? sender as UIElement;
            if (element == null) return;

            DrawLineAdorner adorner = GetDraggingLineAdorner(element);
            Point point = WPFUtility.GetMousePosition(element);
            if (adorner != null) adorner.Position = point;
        }

        private void AssociatedObject_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released) return;

            //use the connection object base on ConnectionDiagramBase
            var element = GetSenderDiagram(sender) ?? sender as UIElement;
            if (element == null) return;

            Point point = e.GetPosition(element as UIElement);


            Point startPoint = (element as ConnectionDiagramBase)?.CenterPosition ?? new Point(0,0);

            //change the draw line center position
            int centerradius = (element as ConnectionDiagramBase)?.CenterPositionRadius ?? 0;
            if (!WPFUtility.IsCenterDragging(startPoint, point, centerradius)) return;

            if (!WPFUtility.IsDragging(startPoint, point)) return;

            DrawLineAdorner adorner = new DrawLineAdorner(element, 0.7, point);
            SetDraggingLineAdorner(element, adorner);

            DataObject data = new DataObject();

            //set the adorner for drop action and create the clone element.
            data.SetData(typeof(ConnectionDiagramBase), element);
            data.SetData(typeof(DrawLineAdorner), adorner);

            ICommand dragcommand = GetDragLineCommand(element);

            //cann't drag without command
            if (dragcommand != null)
            {
                object parameter = GetDragLineCommandParameter(element); //?? this.AssociatedObject.DataContext;

                if (parameter != null)
                    data.SetData(DataFormats.Serializable, parameter);

                if (dragcommand.CanExecute(parameter))
                {
                    dragcommand.Execute(parameter);
                    DragDrop.DoDragDrop(element, data, DragDropEffects.Copy | DragDropEffects.Move);
                }
            }

            adorner.Remove();

            SetDraggingLineAdorner(element, null);

        }

        private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            //var element = GetSenderDiagram(sender);
            //if (element == null) return;

            //element.SetValue(ConnectionDiagramBase.IsSelectedProperty, false);

            //if(element.DataContext is IConnectionDiagramViewModel)
            //{
            //    (element.DataContext as IConnectionDiagramViewModel).IsSelected = false;
            //}
        }

        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton != MouseButtonState.Pressed) return;

            var element = GetSenderDiagram(sender);
            //if (element == null) return;
            if (element != null)
                element.Focus();

        }
        #endregion

        #region  Dependency Property

        #region StartPoint

        /// <summary>
        /// The StartPoint attached property's name.
        /// </summary>
        public const string StartPointPropertyName = "StartPoint";

        /// <summary>
        /// Gets the value of the StartPoint attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the StartPoint property of the specified object.</returns>
        public static Point? GetStartPoint(DependencyObject obj)
        {
            return (Point?)obj.GetValue(StartPointProperty);
        }

        /// <summary>
        /// Sets the value of the StartPoint attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the StartPoint value of the specified object.</param>
        public static void SetStartPoint(DependencyObject obj, Point? value)
        {
            obj.SetValue(StartPointProperty, value);
        }

        /// <summary>
        /// Identifies the StartPoint attached property.
        /// </summary>
        public static readonly DependencyProperty StartPointProperty = DependencyProperty.RegisterAttached(
            StartPointPropertyName,
            typeof(Point?),
            typeof(DrawLineDragBehavior),
            new UIPropertyMetadata(null));

        #endregion

        #region DraggingLineAdorner
        /// <summary>
        /// The DraggingLineAdorner attached property's name.
        /// </summary>
        public const string DraggingLineAdornerPropertyName = "DraggingLineAdorner";

        /// <summary>
        /// Gets the value of the DraggingLineAdorner attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the DraggingLineAdorner property of the specified object.</returns>
        public static DrawLineAdorner GetDraggingLineAdorner(DependencyObject obj)
        {
            return (DrawLineAdorner)obj.GetValue(DraggingLineAdornerProperty);
        }

        /// <summary>
        /// Sets the value of the DraggingLineAdorner attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the DraggingLineAdorner value of the specified object.</param>
        public static void SetDraggingLineAdorner(DependencyObject obj, DrawLineAdorner value)
        {
            obj.SetValue(DraggingLineAdornerProperty, value);
        }

        /// <summary>
        /// Identifies the DraggingLineAdorner attached property.
        /// </summary>
        public static readonly DependencyProperty DraggingLineAdornerProperty = DependencyProperty.RegisterAttached(
            DraggingLineAdornerPropertyName,
            typeof(DrawLineAdorner),
            typeof(DrawLineDragBehavior),
            new UIPropertyMetadata(null));
        #endregion

        #region DragLineCommand

        /// <summary>
        /// The DragLineCommand attached property's name.
        /// </summary>
        public const string DragLineCommandPropertyName = "DragLineCommand";

        /// <summary>
        /// Gets the value of the DragLineCommand attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the DragLineCommand property of the specified object.</returns>
        public static ICommand GetDragLineCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DragLineCommandProperty);
        }

        /// <summary>
        /// Sets the value of the DragLineCommand attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the DragLineCommand value of the specified object.</param>
        public static void SetDragLineCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DragLineCommandProperty, value);
        }

        /// <summary>
        /// Identifies the DragLineCommand attached property.
        /// </summary>
        public static readonly DependencyProperty DragLineCommandProperty = DependencyProperty.RegisterAttached(
            DragLineCommandPropertyName,
            typeof(ICommand),
            typeof(DrawLineDragBehavior),
            new UIPropertyMetadata(null));
        #endregion

        #region DragLineCommandParameter

        /// <summary>
        /// The DragLineCommandParameter attached property's name.
        /// </summary>
        public const string DragLineCommandParameterPropertyName = "DragLineCommandParameter";

        /// <summary>
        /// Gets the value of the DragLineCommandParameter attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the DragLineCommandParameter property of the specified object.</returns>
        public static object GetDragLineCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(DragLineCommandParameterProperty);
        }

        /// <summary>
        /// Sets the value of the DragLineCommandParameter attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the DragLineCommandParameter value of the specified object.</param>
        public static void SetDragLineCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(DragLineCommandParameterProperty, value);
        }

        /// <summary>
        /// Identifies the DragLineCommandParameter attached property.
        /// </summary>
        public static readonly DependencyProperty DragLineCommandParameterProperty = DependencyProperty.RegisterAttached(
            DragLineCommandParameterPropertyName,
            typeof(object),
            typeof(DrawLineDragBehavior),
            new UIPropertyMetadata(null));
        #endregion

        #endregion


        #region Private Method

        private ConnectionDiagramBase GetSenderDiagram(object sender)
        {
            ConnectionDiagramBase element = null;
            if (!(sender is ConnectionDiagramBase))
            {
                element = WPFUtility.FindVisualParent<ConnectionDiagramBase>(sender as UIElement);
            }
            else
            {
                element = sender as ConnectionDiagramBase;
            }
            return element;
        }

    #endregion

}
}
