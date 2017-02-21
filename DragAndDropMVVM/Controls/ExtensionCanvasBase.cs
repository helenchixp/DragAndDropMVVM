using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DragAndDropMVVM.Controls
{
    public abstract class ExtensionCanvasBase : Canvas
    {

        #region Properties

        protected Stack<UndoRedoManager> UndoStack { get; } = new Stack<UndoRedoManager>();

        protected Stack<UndoRedoManager> RedoStack { get; } = new Stack<UndoRedoManager>();

        protected bool _isUndoHandle = false;

        #endregion

        #region Dependency Property

        #region ClearCommand
        /// <summary>
        /// The <see cref="ClearCommand" /> dependency property's name.
        /// </summary>
        public const string ClearCommandPropertyName = "ClearCommand";

        /// <summary>
        /// Gets or sets the value of the <see cref="ClearCommand" />
        /// property. This is a dependency property.
        /// </summary>
        public ICommand ClearCommand
        {
            get
            {
                return (ICommand)GetValue(ClearCommandProperty);
            }
            set
            {
                SetValue(ClearCommandProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="ClearCommand" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty ClearCommandProperty = DependencyProperty.Register(
            ClearCommandPropertyName,
            typeof(ICommand),
            typeof(ExtensionCanvasBase),
            new UIPropertyMetadata(null));

        #endregion

        #region UndoCommand
        /// <summary>
        /// The <see cref="UndoCommand" /> dependency property's name.
        /// </summary>
        public const string UndoCommandPropertyName = "UndoCommand";

        /// <summary>
        /// Gets or sets the value of the <see cref="UndoCommand" />
        /// property. This is a dependency property.
        /// </summary>
        public ICommand UndoCommand
        {
            get
            {
                return (ICommand)GetValue(UndoCommandProperty);
            }
            set
            {
                SetValue(UndoCommandProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="UndoCommand" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty UndoCommandProperty = DependencyProperty.Register(
            UndoCommandPropertyName,
            typeof(ICommand),
            typeof(ExtensionCanvasBase),
            new UIPropertyMetadata(null));

        #endregion

        #region RedoCommand
        /// <summary>
        /// The <see cref="RedoCommand" /> dependency property's name.
        /// </summary>
        public const string RedoCommandPropertyName = "RedoCommand";

        /// <summary>
        /// Gets or sets the value of the <see cref="RedoCommand" />
        /// property. This is a dependency property.
        /// </summary>
        public ICommand RedoCommand
        {
            get
            {
                return (ICommand)GetValue(RedoCommandProperty);
            }
            set
            {
                SetValue(RedoCommandProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="RedoCommand" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty RedoCommandProperty = DependencyProperty.Register(
            RedoCommandPropertyName,
            typeof(ICommand),
            typeof(ExtensionCanvasBase),
            new UIPropertyMetadata(null));
        #endregion

        #endregion


        #region Override Methods


        #region OnInitialized
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);


            CommandBindings.Add(new CommandBinding(ApplicationCommands.Undo,
                                (sender, ce) =>
                                {
                                    var undo = UndoStack.Pop();
                                    _isUndoHandle = true;
                                    if (!(UndoCommand?.CanExecute(null) ?? true))
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        UndoCommand?.Execute(null);
                                    }
                                    undo.UndoAction(undo);
                                },
                                (sender, ce) =>
                                {
                                    ce.CanExecute = UndoStack.Any();
                                }));

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Redo,
                                (sender, ce) =>
                                {
                                    var redo = RedoStack.Pop();
                                    _isUndoHandle = false;
                                    if (!(RedoCommand?.CanExecute(null) ?? true))
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        RedoCommand?.Execute(null);
                                    }
                                    redo.RedoAction(redo);

                                },
                                (sender, ce) =>
                                {
                                    ce.CanExecute = RedoStack.Any();
                                }));


        }
        #endregion

        #region OnVisualChildrenChanged


        void DeleteElementAction(object element) 
        {
            var visualAdded = element as UIElement;
            if(visualAdded  == null)
            {
                return;
            }
            else if (visualAdded is ConnectionLineBase)
            {
                (visualAdded as ConnectionLineBase).DeleteLine();
            }
            else if (visualAdded is ConnectionDiagramBase)
            {
                (visualAdded as ConnectionDiagramBase).DeleteDiagramAndLines();
            }
            else
            {
                Children.Remove(visualAdded as UIElement);
            }
        }

        void AddElementAction(object element)
        {
            var visualAdded = element as UIElement;

            if (visualAdded is ConnectionLineBase)
            {
                var conline = (visualAdded as ConnectionLineBase);
                conline.OriginDiagram?.DepartureLines.Add(conline);
                conline.TerminalDiagram?.ArrivalLines.Add(conline);
            }
            Children.Add(visualAdded as UIElement);
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            if (visualAdded != null)
            {
                Action<object> delact = (o) => DeleteElementAction(visualAdded );
                Action <object> addact = (o) => AddElementAction(visualAdded);
                if (_isUndoHandle)
                {
                    RedoStack.Push(new UndoRedoManager()
                    {
                        ActionComment = $"A_{Children.Count}",
                        UndoAction = addact,
                        RedoAction = delact,
                    });
                    _isUndoHandle = false;
                }
                else
                {
                    UndoStack.Push(new UndoRedoManager()
                    {
                        ActionComment = $"A_{Children.Count}",
                        UndoAction = delact,
                        RedoAction = addact,
                    });
                }

            }
            else if (visualRemoved != null)
            {
                Action<object> actadd = (o) => AddElementAction(visualRemoved);

                Action<object> actdel = (o) => DeleteElementAction(visualAdded);
                if (_isUndoHandle)
                {
                    RedoStack.Push(new UndoRedoManager()
                    {
                        ActionComment = $"R_{Children.Count}",
                        UndoAction = actdel,
                        RedoAction = actadd,
                    });

                    _isUndoHandle = false;
                }
                else
                {
                    UndoStack.Push(new UndoRedoManager()
                    {
                        ActionComment = $"R_{Children.Count}",
                        UndoAction = actadd,
                        RedoAction = actdel,
                    });
                }
            }

            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }
        #endregion

        #region OnDrop
        protected override void OnDrop(DragEventArgs e)
        {
            //_isUndoRedoAction = false;
            UIElement element = e.Data.GetData(typeof(UIElement)) as UIElement;

            _isUndoHandle = false;

            if (element == null) return;

            if (this.Children?.Contains(element) ?? false)
            {
                double left = GetLeft(element);
                double top = GetTop(element);

                double afterleft = e.GetPosition(this).X - (element as ConnectionDiagramBase)?.CenterPosition.X ?? 0;
                double aftertop = e.GetPosition(this).Y - (element as ConnectionDiagramBase)?.CenterPosition.Y ?? 0;

                var point = e.GetPosition(this);
                //it is move effect
                Action<object> undoact = (ele) =>
                {

                    var oldleft = Canvas.GetLeft(element);
                    var oldtop = Canvas.GetTop(element);

                    SetLeft(element, left);
                    SetTop(element, top);

                    if (element is ConnectionDiagramBase)
                    {
                        Behavior.FrameworkElementDropBehavior.SetConnectionLinePosition(element as ConnectionDiagramBase, new Point(left - oldleft, top - oldtop));
                    }

                    RedoStack.Push(ele as UndoRedoManager);
                };

                Action<object> redoact = (ele) =>
                 {
                     var oldleft = Canvas.GetLeft(element);
                     var oldtop = Canvas.GetTop(element);


                     SetLeft(element, afterleft);
                     SetTop(element, aftertop);
                     if (element is ConnectionDiagramBase)
                     {
                         Behavior.FrameworkElementDropBehavior.SetConnectionLinePosition(element as ConnectionDiagramBase, new Point(afterleft - oldleft, aftertop - oldtop));
                     }

                     UndoStack.Push(ele as UndoRedoManager);
                 };
                UndoStack.Push(new UndoRedoManager()
                {
                    ActionComment = $"M_{this.Children.IndexOf(element)}",
                    UndoAction = undoact,
                    RedoAction = redoact,
                });
            }
            else
            {
                RedoStack.Clear();
            }

            base.OnDrop(e);
        }
        #endregion

        #endregion
    }
}
