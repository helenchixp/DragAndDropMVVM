using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;
using DragAndDropMVVM.Controls;

namespace DragAndDropMVVM.Behavior
{
    public class DrawLineDropBehavior : Behavior<FrameworkElement>
    {
        private Type _dataType = typeof(DrawLineAdorner); //the type of the data that can be dropped into this control
        private DroppingDiagramAdorner _adorner;

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
            if (_dataType != null)
            {
                //if the data type can be dropped 
                if (e.Data.GetDataPresent(_dataType))
                {

                    ////drop the data
                    //IDropable target = this.AssociatedObject.DataContext as IDropable;
                    //target.Drop(source.DataContext);

                    ////remove the data from the source
                    //IDragable sourcevm = source.DataContext as IDragable;
                    //sourcevm.Drag(e.Data.GetData(_dataType));
                    UIElement element = sender as FrameworkElement;

                    ICommand dropcommand = GetDropLineCommand(element);

                    if (dropcommand != null)
                    {
                        object parameter = (GetDropLineCommandParameter(element) ??
                            (e.Data.GetDataPresent(DataFormats.Serializable) ? e.Data.GetData(DataFormats.Serializable) : null)) ??
                            this.AssociatedObject.DataContext;

                        if (dropcommand.CanExecute(parameter))
                        {
                            dropcommand.Execute(parameter);

                            Point point = e.GetPosition(element);

                            var adn = e.Data.GetData(_dataType) as DrawLineAdorner;
                            Point adnPoint = adn.Position;

                            Canvas droppedcanvas = GetDroppedLineCanvas(element);

                            //****************************************
                            //TODO:The Line Position is need to Calcute
                            //****************************************


                            DrawLineThump conline = new DrawLineThump()
                            {
                                X1 = adn.GetLineStartEndPosition().Item1,
                                Y1 = adn.GetLineStartEndPosition().Item2,
                                X2 = adn.GetLineStartEndPosition().Item3,
                                Y2 = adn.GetLineStartEndPosition().Item4,
                            };

                            Canvas.SetTop(conline, (double)element.GetValue(Canvas.TopProperty));
                            Canvas.SetLeft(conline, (double)element.GetValue(Canvas.LeftProperty));
             

                            droppedcanvas.Children.Add(conline);


                        }
                    }

                }
            }

            if (this._adorner != null)
                this._adorner.Remove();


            e.Handled = true;
            return;
        }

        private void AssociatedObject_DragLeave(object sender, DragEventArgs e)
        {
            if (_dataType != null)
            {
                //if item can be dropped
                if (e.Data.GetDataPresent(_dataType))
                {
                    if (this._adorner != null)
                        this._adorner.Remove();
                    e.Handled = true;
                }
            }
        }

        private void AssociatedObject_DragOver(object sender, DragEventArgs e)
        {
            if (_dataType != null)
            {
                //if item can be dropped
                if (e.Data.GetDataPresent(_dataType))
                {
                    //give mouse effect
                    //this.SetDragDropEffects(e);
                    //draw the dots
                    if (this._adorner != null)
                        this._adorner.Update();
                    e.Handled = true;
                }
            }
        }

        private void AssociatedObject_DragEnter(object sender, DragEventArgs e)
        {
            if (_dataType != null)
            {
                //if item can be dropped
                if (e.Data.GetDataPresent(_dataType))
                {
                    if (this._adorner == null)
                        this._adorner = new DroppingDiagramAdorner(sender as UIElement);
                    e.Handled = true;
                }
            }
        }



        #region Attached Property

        #region DroppedLineCanvas

        /// <summary>
        /// The DroppedLineCanvas attached property's name.
        /// </summary>
        public const string DroppedLineCanvasPropertyName = "DroppedLineCanvas";

        /// <summary>
        /// Gets the value of the DroppedLineCanvas attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the DroppedLineCanvas property of the specified object.</returns>
        public static Canvas GetDroppedLineCanvas(DependencyObject obj)
        {
            return (Canvas)obj.GetValue(DroppedLineCanvasProperty);
        }

        /// <summary>
        /// Sets the value of the DroppedLineCanvas attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the DroppedLineCanvas value of the specified object.</param>
        public static void SetDroppedLineCanvas(DependencyObject obj, Canvas value)
        {
            obj.SetValue(DroppedLineCanvasProperty, value);
        }

        /// <summary>
        /// Identifies the DroppedLineCanvas attached property.
        /// </summary>
        public static readonly DependencyProperty DroppedLineCanvasProperty = DependencyProperty.RegisterAttached(
            DroppedLineCanvasPropertyName,
            typeof(Canvas),
            typeof(DrawLineDropBehavior),
            new UIPropertyMetadata(null));

        #endregion

        #region DropLineCommand
        /// <summary>
        /// The DropLineCommand attached property's name.
        /// </summary>
        public const string DropLineCommandPropertyName = "DropLineCommand";

        /// <summary>
        /// Gets the value of the DropLineCommand attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the DropLineCommand property of the specified object.</returns>
        public static ICommand GetDropLineCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DropLineCommandProperty);
        }

        /// <summary>
        /// Sets the value of the DropLineCommand attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the DropLineCommand value of the specified object.</param>
        public static void SetDropLineCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DropLineCommandProperty, value);
        }

        /// <summary>
        /// Identifies the DropLineCommand attached property.
        /// </summary>
        public static readonly DependencyProperty DropLineCommandProperty = DependencyProperty.RegisterAttached(
            DropLineCommandPropertyName,
            typeof(ICommand),
            typeof(DrawLineDropBehavior),
            new UIPropertyMetadata(null));
        #endregion


        #region DropLineCommandParameter
        /// <summary>
        /// The DropLineCommandParameter attached property's name.
        /// </summary>
        public const string DropLineCommandParameterPropertyName = "DropLineCommandParameter";

        /// <summary>
        /// Gets the value of the DropLineCommandParameter attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the DropLineCommandParameter property of the specified object.</returns>
        public static object GetDropLineCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(DropLineCommandParameterProperty);
        }

        /// <summary>
        /// Sets the value of the DropLineCommandParameter attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the DropLineCommandParameter value of the specified object.</param>
        public static void SetDropLineCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(DropLineCommandParameterProperty, value);
        }

        /// <summary>
        /// Identifies the DropLineCommandParameter attached property.
        /// </summary>
        public static readonly DependencyProperty DropLineCommandParameterProperty = DependencyProperty.RegisterAttached(
            DropLineCommandParameterPropertyName,
            typeof(object),
            typeof(DrawLineDropBehavior),
            new UIPropertyMetadata(null));
        #endregion

        #endregion
    }
}
