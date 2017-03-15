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
using DragAndDropMVVM.Assist;
using DragAndDropMVVM.Controls;
using DragAndDropMVVM.ViewModel;

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
                    FrameworkElement element = sender as FrameworkElement;

                    ICommand dropcommand = GetDropLineCommand(element);

                    if (dropcommand != null)
                    {
                        object parameter = GetDropLineCommandParameter(element);


                        Point point = e.GetPosition(element);

                        var adn = e.Data.GetData(_dataType) as DrawLineAdorner;
                        Point adnPoint = adn.Position;

                        Canvas droppedcanvas = GetDroppedLineCanvas(element);
                        var originelement = (e.Data.GetData(typeof(ConnectionDiagramBase)) as FrameworkElement);

                        //the object for ConnectionDiagram
                        ConnectionDiagramBase origindiagram = originelement as ConnectionDiagramBase;
                        ConnectionDiagramBase terminaldiagram = element as ConnectionDiagramBase;

                        if (droppedcanvas?.DataContext is IDragged)
                        {
                            (droppedcanvas.DataContext as IDragged).DraggedDataContext =
                                new Tuple<object, object>((originelement as ConnectionDiagramBase)?.DataContext, (element as ConnectionDiagramBase)?.DataContext);
                        }

                        double x1, y1, x2, y2 = 0.0;

                        if (ConnectorPositionType.Custom.Equals(origindiagram?.ConnectorPositionType))
                        {
                            //the Ghost line position
                            x1 = adn.GetLineStartEndPosition().Item1;
                            y1 = adn.GetLineStartEndPosition().Item2;
                        }
                        else
                        {
                            x1 = WPFUtility.GetCenterPosition(originelement, droppedcanvas).X;
                            y1 = WPFUtility.GetCenterPosition(originelement, droppedcanvas).Y;
                        }

                        if (ConnectorPositionType.Custom.Equals(terminaldiagram?.ConnectorPositionType))
                        {
                            //the Ghost line position
                            x2 = adn.GetLineStartEndPosition().Item3;
                            y2 = adn.GetLineStartEndPosition().Item4;
                        }
                        else
                        {
                            x2 = WPFUtility.GetCenterPosition(element, droppedcanvas).X;
                            y2 = WPFUtility.GetCenterPosition(element, droppedcanvas).Y;
                        }

                        //the line type of custom
                        Type linetype = GetDropLineControlType(element);

                        if (!WPFUtility.IsCorrectType(linetype, typeof(ConnectionLineBase)))
                        {
                            throw new ArgumentException($"DropLineControlType is base on {nameof(ConnectionLineBase)}.");
                        }

                        dynamic conline;


                        if (!typeof(DrawLineThump).Equals(linetype))
                        {
                            conline = Activator.CreateInstance(linetype);

                            if (conline is ConnectionLineBase)
                            {

                                (conline as ConnectionLineBase).OriginDiagram = origindiagram;
                                (conline as ConnectionLineBase).TerminalDiagram = terminaldiagram;

                                //if inherit from ILinePosition
                                if (conline is ILinePosition)
                                {
                                    (conline as ILinePosition).X1 = x1;
                                    (conline as ILinePosition).Y1 = y1;
                                    (conline as ILinePosition).X2 = x2;
                                    (conline as ILinePosition).Y2 = y2;
                                }
                            }
                        }
                        else
                        {
                            conline = new Line()
                            {
                                X1 = x1,
                                Y1 = y1,
                                X2 = x2,
                                Y2 = y2,
                            };

                            FrameworkElementAssist.SetOriginDiagram(conline, originelement);
                            FrameworkElementAssist.SetTerminalDiagram(conline, element);
                        }

                        //set the parameter
                        var dropparam = parameter ?? conline.DataContext;
                        string lineuuid = $"{conline.GetType().Name}_{Guid.NewGuid().ToString()}";

                        if (dropcommand.CanExecute(dropparam))
                        {
                            Canvas.SetTop(conline, 0);
                            Canvas.SetLeft(conline, 0);

                            //add the relation of the diagram
                            if (origindiagram != null)
                            {
                                origindiagram.DepartureLines.Add(conline);
                            }
                            if (terminaldiagram != null)
                            {
                                terminaldiagram.ArrivalLines.Add(conline);
                            }

                            //TODO:FrameworkElement????
                            if (droppedcanvas != null)
                                droppedcanvas.Children.Add(conline);

                            dropcommand.Execute(dropparam);

                            if (conline is ConnectionLineBase)
                            {
                                (conline as ConnectionLineBase).LineUUID = lineuuid;
                            }
                            else if (conline is Line)
                            {
                                FrameworkElementAssist.SetLineUUID(conline, lineuuid);
                            }
                        }

                        if (droppedcanvas?.DataContext is IDragged)
                        {
                            (droppedcanvas.DataContext as IDragged).DraggedDataContext = null;
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

        #region DropLineControlType

        /// <summary>
        /// The DropLineControlType attached property's name.
        /// </summary>
        public const string DropLineControlTypePropertyName = "DropLineControlType";

        /// <summary>
        /// Gets the value of the DropLineControlType attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the DropLineControlType property of the specified object.</returns>
        public static Type GetDropLineControlType(DependencyObject obj)
        {
            return (Type)obj.GetValue(DropLineControlTypeProperty);
        }

        /// <summary>
        /// Sets the value of the DropLineControlType attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the DropLineControlType value of the specified object.</param>
        public static void SetDropLineControlType(DependencyObject obj, Type value)
        {
            obj.SetValue(DropLineControlTypeProperty, value);
        }

        /// <summary>
        /// Identifies the DropLineControlType attached property.
        /// </summary>
        public static readonly DependencyProperty DropLineControlTypeProperty = DependencyProperty.RegisterAttached(
            DropLineControlTypePropertyName,
            typeof(Type),
            typeof(DrawLineDropBehavior),
            new UIPropertyMetadata(typeof(DrawLineThump)));
        #endregion

        #endregion
    }
}
