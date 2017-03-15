using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using DragAndDropMVVM.Controls;
using DragAndDropMVVM.ViewModel;

namespace DragAndDropMVVM.Behavior
{
    public class DiagramElementDropBehavior : Behavior<FrameworkElement>
    {

        private DroppingAdorner _adorner;

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.AllowDrop = true;
            this.AssociatedObject.DragEnter += AssociatedObject_DragEnter;
            this.AssociatedObject.DragOver += AssociatedObject_DragOver;
            this.AssociatedObject.DragLeave += AssociatedObject_DragLeave;
            this.AssociatedObject.Drop += AssociatedObject_Drop;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.DragEnter -= AssociatedObject_DragEnter;
            this.AssociatedObject.DragOver -= AssociatedObject_DragOver;
            this.AssociatedObject.DragLeave -= AssociatedObject_DragLeave;
            this.AssociatedObject.Drop -= AssociatedObject_Drop;
        }

        private void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            //if the data type can be dropped 
            if (e.Data.GetDataPresent(typeof(DraggingAdorner)))
            {

                ////drop the data
                //IDropable target = this.AssociatedObject.DataContext as IDropable;
                //target.Drop(source.DataContext);

                ////remove the data from the source
                //IDragable sourcevm = source.DataContext as IDragable;
                //sourcevm.Drag(e.Data.GetData(_dataType));
                UIElement element = sender as FrameworkElement;

                ICommand dropcommand = GetDropCommand(element);

                if (dropcommand != null)
                {
                    object parameter = GetDropCommandParameter(element);

                    if (this.AssociatedObject.DataContext is IDragged)
                    {
                        (this.AssociatedObject.DataContext as IDragged).DraggedDataContext = (e.Data.GetDataPresent(DataFormats.Serializable) ? e.Data.GetData(DataFormats.Serializable) : null);
                    }

                    if (dropcommand.CanExecute(parameter))
                    {

                        Point point = e.GetPosition(element);

                        ////TODO: Add the 
                        Canvas droppedcanvas = GetDroppedCanvas(element);

                        // if copy this dragobj 
                        if (!GetIsFixedPosition(element) && droppedcanvas != null)
                        {
                            UIElement dragelement = e.Data.GetDataPresent(typeof(UIElement)) ? e.Data.GetData(typeof(UIElement)) as UIElement : null;

                            Type droppedcontroltype = dragelement != null ? GetDroppedControlType(dragelement) : null;

                            if (droppedcontroltype != null && !WPFUtility.IsCorrectType(droppedcontroltype, typeof(ContentControl)))
                            {
                                throw new ArgumentException("DroppedControlType is base on ContentControl.");
                            }

                            if (e.Data.GetDataPresent(typeof(DraggingAdorner)))
                            {
                                var adn = e.Data.GetData(typeof(DraggingAdorner)) as DraggingAdorner;

                                UIElement clnele = null;

                                bool iscopy = false;

                                if (droppedcontroltype != null)
                                {

                                    //TODO: Copy Control or create new control
                                    if (GetIsDuplication(dragelement))
                                        clnele = CreateNewContentControl(droppedcontroltype, adn.GetGhostElement() as UIElement);
                                    else
                                        clnele = Activator.CreateInstance(droppedcontroltype) as UIElement;
                                    iscopy = true;
                                }
                                else
                                {
                                    clnele = dragelement;
                                }


                                //add the clone element
                                if (clnele != null)
                                {


                                    Point canvaspoint = (Point)(point - adn.CenterPoint);

                                    Point oldpoint = new Point(Canvas.GetLeft(dragelement), Canvas.GetTop(dragelement));

                                    SetConnectionLinePosition(clnele as ConnectionDiagramBase, (Point)(canvaspoint - oldpoint));


                                    Canvas.SetLeft(clnele, canvaspoint.X);
                                    Canvas.SetTop(clnele, canvaspoint.Y);

                                    (clnele as ConnectionDiagramBase).DiagramUUID = $"{clnele.GetType().Name}_{Guid.NewGuid().ToString()}";

                                    if (iscopy)
                                    {
                                        droppedcanvas.Children.Add(clnele);
                                    }
                                    else
                                    {
                                        //???
                                    }

                                    if (parameter != null)
                                    {
                                        dropcommand.Execute(parameter);
                                    }
                                    else
                                    {
                                        dropcommand.Execute((clnele as ContentControl)?.DataContext);
                                    }

                                }

                            }

                        }
                    }

                    if (this.AssociatedObject.DataContext is IDragged)
                    {
                        (this.AssociatedObject.DataContext as IDragged).DraggedDataContext = null;
                    }
                }

            }


            if (this._adorner != null)
                this._adorner.Remove();


            e.Handled = true;
            return;
        }

        private  void AssociatedObject_DragLeave(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DraggingAdorner)))
            {
                if (this._adorner != null)
                    this._adorner.Remove();
                e.Handled = true;
            }
        }

        private void AssociatedObject_DragOver(object sender, DragEventArgs e)
        {
            //if item can be dropped
            if (e.Data.GetDataPresent(typeof(DraggingAdorner)))
            {
                //give mouse effect
                this.SetDragDropEffects(e);
                //draw the dots
                if (this._adorner != null)
                    this._adorner.Update();

                e.Handled = true;
            }

        }

        private void AssociatedObject_DragEnter(object sender, DragEventArgs e)
        {
            
            if (e.Data.GetDataPresent(typeof(DraggingAdorner)))
            {
                if (this._adorner == null)
                    this._adorner = new DroppingAdorner(sender as UIElement);
                e.Handled = true;
            }

        }

        /// <summary>
        /// Provides feedback on if the data can be dropped
        /// </summary>
        /// <param name="e"></param>
        private void SetDragDropEffects(DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;  //default to None

            //if the data type can be dropped 
            if (e.Data.GetDataPresent(typeof(DraggingAdorner)))
            {
                e.Effects = DragDropEffects.Move;
            }
        }

        #region Dependency Property

        #region IsFixedPosition

        /// <summary>
        /// The IsFixedPosition attached property's name.
        /// </summary>
        public const string IsFixedPositionPropertyName = "IsFixedPosition";

        /// <summary>
        /// Gets the value of the IsFixedPosition attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the IsFixedPosition property of the specified object.</returns>
        public static bool GetIsFixedPosition(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFixedPositionProperty);
        }

        /// <summary>
        /// Sets the value of the IsFixedPosition attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the IsFixedPosition value of the specified object.</param>
        public static void SetIsFixedPosition(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFixedPositionProperty, value);
        }

        /// <summary>
        /// Identifies the IsFixedPosition attached property.
        /// </summary>
        public static readonly DependencyProperty IsFixedPositionProperty = DependencyProperty.RegisterAttached(
            IsFixedPositionPropertyName,
            typeof(bool),
            typeof(DiagramElementDropBehavior),
            new UIPropertyMetadata(false));

        #endregion

        #region AdornerType

        /// <summary>
        /// The AdornerType attached property's name.
        /// </summary>
        public const string AdornerTypePropertyName = "AdornerType";

        /// <summary>
        /// Gets the value of the AdornerType attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the AdornerType property of the specified object.</returns>
        public static DroppingElementAdornerType GetAdornerType(DependencyObject obj)
        {
            return (DroppingElementAdornerType)obj.GetValue(AdornerTypeProperty);
        }

        /// <summary>
        /// Sets the value of the AdornerType attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the AdornerType value of the specified object.</param>
        public static void SetAdornerType(DependencyObject obj, DroppingElementAdornerType value)
        {
            obj.SetValue(AdornerTypeProperty, value);
        }

        /// <summary>
        /// Identifies the AdornerType attached property.
        /// </summary>
        public static readonly DependencyProperty AdornerTypeProperty = DependencyProperty.RegisterAttached(
            AdornerTypePropertyName,
            typeof(DroppingElementAdornerType),
            typeof(DiagramElementDropBehavior),
            new UIPropertyMetadata(DroppingElementAdornerType.DrawEllipse));


        #endregion

        #region DropCommand

        /// <summary>
        /// The DropCommand attached property's name.
        /// </summary>
        public const string DropCommandPropertyName = "DropCommand";

        /// <summary>
        /// Gets the value of the DropCommand attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the DropCommand property of the specified object.</returns>
        public static ICommand GetDropCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DropCommandProperty);
        }

        /// <summary>
        /// Sets the value of the DropCommand attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the DropCommand value of the specified object.</param>
        public static void SetDropCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DropCommandProperty, value);
        }

        /// <summary>
        /// Identifies the DropCommand attached property.
        /// </summary>
        public static readonly DependencyProperty DropCommandProperty = DependencyProperty.RegisterAttached(
            DropCommandPropertyName,
            typeof(ICommand),
            typeof(DiagramElementDropBehavior),
            new UIPropertyMetadata(null));
        #endregion

        #region DropCommandParameter

        /// <summary>
        /// The DropCommandParameter attached property's name.
        /// </summary>
        public const string DropCommandParameterPropertyName = "DropCommandParameter";

        /// <summary>
        /// Gets the value of the DropCommandParameter attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the DropCommandParameter property of the specified object.</returns>
        public static object GetDropCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(DropCommandParameterProperty);
        }

        /// <summary>
        /// Sets the value of the DropCommandParameter attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the DropCommandParameter value of the specified object.</param>
        public static void SetDropCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(DropCommandParameterProperty, value);
        }

        /// <summary>
        /// Identifies the DropCommandParameter attached property.
        /// </summary>
        public static readonly DependencyProperty DropCommandParameterProperty = DependencyProperty.RegisterAttached(
            DropCommandParameterPropertyName,
            typeof(object),
            typeof(DiagramElementDropBehavior),
            new UIPropertyMetadata(null));
        #endregion

        #region DroppedCanvas
        /// <summary>
        /// The DroppedCanvas attached property's name.
        /// </summary>
        public const string DroppedCanvasPropertyName = "DroppedCanvas";

        /// <summary>
        /// Gets the value of the DroppedCanvas attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the DroppedCanvas property of the specified object.</returns>
        public static Canvas GetDroppedCanvas(DependencyObject obj)
        {
            return (Canvas)obj.GetValue(DroppedCanvasProperty);
        }

        /// <summary>
        /// Sets the value of the DroppedCanvas attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the DroppedCanvas value of the specified object.</param>
        public static void SetDroppedCanvas(DependencyObject obj, Canvas value)
        {
            obj.SetValue(DroppedCanvasProperty, value);
        }

        /// <summary>
        /// Identifies the DroppedCanvas attached property.
        /// </summary>
        public static readonly DependencyProperty DroppedCanvasProperty = DependencyProperty.RegisterAttached(
            DroppedCanvasPropertyName,
            typeof(Canvas),
            typeof(DiagramElementDropBehavior),
            new UIPropertyMetadata(null));
        #endregion

        #region DroppedControlType
        /// <summary>
        /// The DroppedControlType attached property's name.
        /// </summary>
        public const string DroppedControlTypePropertyName = "DroppedControlType";

        /// <summary>
        /// Gets the value of the DroppedControlType attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the DroppedControlType property of the specified object.</returns>
        public static Type GetDroppedControlType(DependencyObject obj)
        {
            return (Type)obj.GetValue(DroppedControlTypeProperty);
        }

        /// <summary>
        /// Sets the value of the DroppedControlType attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the DroppedControlType value of the specified object.</param>
        public static void SetDroppedControlType(DependencyObject obj, Type value)
        {
            obj.SetValue(DroppedControlTypeProperty, value);
        }

        /// <summary>
        /// Identifies the DroppedControlType attached property.
        /// </summary>
        public static readonly DependencyProperty DroppedControlTypeProperty = DependencyProperty.RegisterAttached(
            DroppedControlTypePropertyName,
            typeof(Type),
            typeof(DiagramElementDropBehavior),
            new UIPropertyMetadata(null));
        #endregion

        #region IsDuplication
        /// <summary>
        /// The IsDuplication attached property's name.
        /// </summary>
        public const string IsDuplicationPropertyName = "IsDuplication";

        /// <summary>
        /// Gets the value of the IsDuplication attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the IsDuplication property of the specified object.</returns>
        public static bool GetIsDuplication(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDuplicationProperty);
        }

        /// <summary>
        /// Sets the value of the IsDuplication attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the IsDuplication value of the specified object.</param>
        public static void SetIsDuplication(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDuplicationProperty, value);
        }

        /// <summary>
        /// Identifies the IsDuplication attached property.
        /// </summary>
        public static readonly DependencyProperty IsDuplicationProperty = DependencyProperty.RegisterAttached(
            IsDuplicationPropertyName,
            typeof(bool),
            typeof(DiagramElementDropBehavior),
            new UIPropertyMetadata(false));

        #endregion

        #endregion 

        #region Private Method




        private ContentControl CreateNewContentControl(Type contenttype, object content)
        {
            if (content == null) return null;

            var newobj = contenttype.GetConstructors()[0].Invoke(null);

            PropertyInfo contentpro = contenttype.GetProperty("Content");


            if (content.GetType().Equals(contenttype))
            {
                var concon = contentpro.GetValue(content) as UIElement;
                contentpro.SetValue(newobj, concon);
            }
            else
            { 
                contentpro.SetValue(newobj, content);
            }

            return newobj as ContentControl;
        }

        internal static void SetConnectionLinePosition(ConnectionDiagramBase element, Point point)
        {

            if (element == null) return;
            if (element.DepartureLines != null && element.DepartureLines.Any())
            {
                foreach (var dline in element.DepartureLines)
                {
                    if (dline is DrawLineThump)
                    {
                        (dline as DrawLineThump).X1 = (dline as DrawLineThump).X1 + point.X;
                        (dline as DrawLineThump).Y1 = (dline as DrawLineThump).Y1 + point.Y;

                        var angle = Math.Atan2(
                            (dline as DrawLineThump).Y1 - (dline as DrawLineThump).Y2,
                            (dline as DrawLineThump).X1 - (dline as DrawLineThump).X2) * 180d / Math.PI;

                        (dline as DrawLineThump).Angle = angle < 0 ? angle + 360 : angle;
                    }
                }

            }

            if (element.ArrivalLines != null && element.ArrivalLines.Any())
            {
                foreach (var aline in element.ArrivalLines)
                {
                    if (aline is DrawLineThump)
                    {
                        (aline as DrawLineThump).X2 = (aline as DrawLineThump).X2 + point.X;
                        (aline as DrawLineThump).Y2 = (aline as DrawLineThump).Y2 + point.Y;


                        var angle = Math.Atan2(
                            (aline as DrawLineThump).Y1 - (aline as DrawLineThump).Y2,
                            (aline as DrawLineThump).X1 - (aline as DrawLineThump).X2) * 180d / Math.PI;

                        (aline as DrawLineThump).Angle = angle < 0 ? angle + 360 : angle;
                    }
                }
            }
        }


        #endregion

    }
}
