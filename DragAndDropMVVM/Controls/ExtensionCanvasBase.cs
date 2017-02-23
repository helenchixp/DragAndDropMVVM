using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DragAndDropMVVM.Model;

namespace DragAndDropMVVM.Controls
{
    public abstract class ExtensionCanvasBase : Canvas//, IMapLayout
    {

        #region Properties

        protected Stack<UndoRedoManager> UndoStack { get; } = new Stack<UndoRedoManager>();

        protected Stack<UndoRedoManager> RedoStack { get; } = new Stack<UndoRedoManager>();

        protected bool _isUndoHandle = false;


        private IMapLayout _innerMapLayout = null;

        #endregion

        #region Method

        private void RefreshLayoutInCanvas()
        {

            if (LayoutDataContext == null ||
                this.Children == null ||
                (Children.Count == 0))
                return;

            _innerMapLayout =  LayoutDataContext;

            var diagrams = (_innerMapLayout ?? (_innerMapLayout =  LayoutDataContext)).Diagrams;


            int diagramsum = this.Children.Cast<UIElement>().Sum(child =>
            {
                if (child is ConnectionDiagramBase) return 1;
                else return 0;
            });

            if (diagrams == null || diagrams.Count() > diagramsum)
            {
                diagrams = (from child in this.Children.Cast<UIElement>()
                            where child is ConnectionDiagramBase
                            select Activator.CreateInstance(_innerMapLayout.DiagramLayoutType)).Cast<IDiagramLayout>().ToArray();
            }
            else if (diagrams.Count() < diagramsum)
            {
                Array.Resize(ref diagrams, diagramsum); 
            }

            var idx = 0;

            //set the diagramUI position and properties
            foreach(var child in this.Children)
            {
                if (!(child is ConnectionDiagramBase)) continue;

                var diagramUI = child as ConnectionDiagramBase;

                if (diagrams[idx] == null) diagrams[idx] = Activator.CreateInstance(_innerMapLayout.DiagramLayoutType) as IDiagramLayout;

                diagrams[idx].X = Canvas.GetLeft(diagramUI);
                diagrams[idx].Y = Canvas.GetTop(diagramUI);
                diagrams[idx].DiagramUUID = diagramUI.DiagramUUID;
                diagrams[idx].DiagramUIType = diagramUI.GetType();
                diagrams[idx].DataContext = diagramUI.DataContext;

                //set the Line properties
                if (diagramUI.DepartureLines!= null && diagramUI.DepartureLines.Any())
                {
                    diagrams[idx].DepartureLines = (from line in diagramUI.DepartureLines
                                                    select Activator.CreateInstance(_innerMapLayout.LineLayouType)).Cast<ILineLayout>().ToArray<ILineLayout>();

                    int lidx = 0;

                    foreach(var lineui in diagramUI.DepartureLines)
                    {
                        diagrams[idx].DepartureLines[lidx].LineUUID = lineui.LineUUID;
                        diagrams[idx].DepartureLines[lidx].LineUIType = lineui.GetType();
                        diagrams[idx].DepartureLines[lidx].TerminalDiagramUUID = lineui.TerminalDiagram?.DiagramUUID;
                        diagrams[idx].DepartureLines[lidx].DataContext = lineui.DataContext;
                        lidx++;
                    }

                }

                idx++;
            }


            _innerMapLayout.Diagrams = diagrams;

            _innerMapLayout.Height = Height;
            _innerMapLayout.Width = Width;

            var map = RefreshMapLayout(_innerMapLayout);
            SetValue(LayoutDataContextProperty, map);
        }


        private void ReloadLayoutInCanvas()
        {
            if (_innerMapLayout == null) return;
        }

        #endregion


        #region Virtual
        /// <summary>
        /// If you want to update MapLayout, override it and rewrite this methods
        /// </summary>
        /// <param name="mapLayout"></param>
        /// <returns></returns>
        protected virtual IMapLayout RefreshMapLayout(IMapLayout mapLayout)
        {
            return mapLayout;
        }
        #endregion


        #region Dependency Property

        #region LayoutDataContext
        /// <summary>
        /// The <see cref="LayoutDataContext" /> dependency property's name.
        /// </summary>
        public const string LayoutDataContextPropertyName = "LayoutDataContext";

        /// <summary>
        /// Gets or sets the value of the <see cref="LayoutDataContext" />
        /// property. This is a dependency property.
        /// </summary>
        public IMapLayout LayoutDataContext
        {
            get
            {
                return (IMapLayout)GetValue(LayoutDataContextProperty);
            }
            set
            {
                SetValue(LayoutDataContextProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="LayoutDataContext" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty LayoutDataContextProperty = DependencyProperty.Register(
            LayoutDataContextPropertyName,
            typeof(IMapLayout),
            typeof(ExtensionCanvasBase),
            new UIPropertyMetadata(null,
                  (d, e) =>
                  {
                      if (e.NewValue == null
                        || !(e.NewValue is IMapLayout)
                        || (e.NewValue == e.OldValue)
                        || !(d is Canvas))
                          return;
                      var map = (e.NewValue as IMapLayout);
                      var canvas = d as Canvas;

                      canvas.Children.Clear();


                  },
                  (d, baseValue) =>
                  {
                      return baseValue;
                  })
            );
        #endregion

        #region IsSyncLayoutDataContext
        /// <summary>
        /// The <see cref="IsSyncLayoutDataContext" /> dependency property's name.
        /// </summary>
        public const string IsSyncLayoutDataContextPropertyName = "IsSyncLayoutDataContext";

        /// <summary>
        /// Gets or sets the value of the <see cref="IsSyncLayoutDataContext" />
        /// property. This is a dependency property.
        /// </summary>
        public bool IsSyncLayoutDataContext
        {
            get
            {
                return (bool)GetValue(IsSyncLayoutDataContextProperty);
            }
            set
            {
                SetValue(IsSyncLayoutDataContextProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="IsSyncLayoutDataContext" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSyncLayoutDataContextProperty = DependencyProperty.Register(
            IsSyncLayoutDataContextPropertyName,
            typeof(bool),
            typeof(ExtensionCanvasBase),
            new UIPropertyMetadata(false,
                (d, e) =>
                {
                    if (true.Equals(e.NewValue))
                    {
                        (d as ExtensionCanvasBase).RefreshLayoutInCanvas();
                    }
                }
                ));
        #endregion

        #region ClearCommand[Obsoleted]
        /////// <summary>
        /////// The <see cref="ClearCommand" /> dependency property's name.
        /////// </summary>
        ////public const string ClearCommandPropertyName = "ClearCommand";

        /////// <summary>
        /////// Gets or sets the value of the <see cref="ClearCommand" />
        /////// property. This is a dependency property.
        /////// </summary>
        ////public ICommand ClearCommand
        ////{
        ////    get
        ////    {
        ////        return (ICommand)GetValue(ClearCommandProperty);
        ////    }
        ////    set
        ////    {
        ////        SetValue(ClearCommandProperty, value);
        ////    }
        ////}

        /////// <summary>
        /////// Identifies the <see cref="ClearCommand" /> dependency property.
        /////// </summary>
        ////public static readonly DependencyProperty ClearCommandProperty = DependencyProperty.Register(
        ////    ClearCommandPropertyName,
        ////    typeof(ICommand),
        ////    typeof(ExtensionCanvasBase),
        ////    new UIPropertyMetadata(null));

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
                Action<object> delact = (o) => DeleteElementAction(visualAdded);
                Action<object> addact = (o) => AddElementAction(visualAdded);
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

            if (IsSyncLayoutDataContext)
                RefreshLayoutInCanvas();
        }
        #endregion

        #region OnDrop
        protected override void OnDrop(DragEventArgs e)
        {
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
                        Behavior.DiagramElementDropBehavior.SetConnectionLinePosition(element as ConnectionDiagramBase, new Point(left - oldleft, top - oldtop));
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
                         Behavior.DiagramElementDropBehavior.SetConnectionLinePosition(element as ConnectionDiagramBase, new Point(afterleft - oldleft, aftertop - oldtop));
                     }

                     UndoStack.Push(ele as UndoRedoManager);
                 };
                UndoStack.Push(new UndoRedoManager()
                {
                    ActionComment = $"M_{this.Children.IndexOf(element)}",
                    UndoAction = undoact,
                    RedoAction = redoact,
                });

                if (IsSyncLayoutDataContext)
                    RefreshLayoutInCanvas();
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
