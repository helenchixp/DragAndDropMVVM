using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DragAndDropMVVM.Model;

namespace DragAndDropMVVM.Controls
{
    public abstract class ExtensionCanvasBase : Canvas//, IMapLayout
    {

        #region Properties

        protected Stack<UndoRedoManager> UndoStack { get; } = new Stack<UndoRedoManager>();

        protected Stack<UndoRedoManager> RedoStack { get; } = new Stack<UndoRedoManager>();

        protected bool _isUndoHandle = false;



        #endregion

        #region Method(Import and Export for Layout)

        private void RefreshLayoutInCanvas()
        {

            if (LayoutDataContext == null ||
                this.Children == null ||
                (Children.Count == 0))
                return;

            //    _innerMapLayout =  LayoutDataContext;

            //var diagrams = (_innerMapLayout ?? (_innerMapLayout =  LayoutDataContext)).Diagrams;
            var diagrams =LayoutDataContext.Diagrams;

            int diagramsum = this.Children.Cast<UIElement>().Sum(child =>
            {
                if (child is ConnectionDiagramBase) return 1;
                else return 0;
            });

            if (diagrams == null || diagrams.Count() > diagramsum)
            {
                diagrams = (from child in this.Children.Cast<UIElement>()
                            where child is ConnectionDiagramBase
                            select Activator.CreateInstance(LayoutDataContext.DiagramLayoutType)).Cast<IDiagramLayout>().ToArray();
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

                if (diagrams[idx] == null) diagrams[idx] = Activator.CreateInstance(LayoutDataContext.DiagramLayoutType) as IDiagramLayout;

                diagrams[idx].X = Canvas.GetLeft(diagramUI);
                diagrams[idx].Y = Canvas.GetTop(diagramUI);
                diagrams[idx].DiagramUUID = diagramUI.DiagramUUID;
                diagrams[idx].DiagramUIType = diagramUI.GetType();
                diagrams[idx].DataContext = diagramUI.DataContext;

                //set the Line properties
                if (diagramUI.DepartureLines!= null && diagramUI.DepartureLines.Any())
                {
                    diagrams[idx].DepartureLines = (from line in diagramUI.DepartureLines
                                                    select Activator.CreateInstance(diagrams[idx].LineLayouType)).Cast<ILineLayout>().ToArray<ILineLayout>();

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


            LayoutDataContext.Diagrams = diagrams;

            LayoutDataContext.Height = Height;
            LayoutDataContext.Width = Width;

            var map = RefreshMapLayout(LayoutDataContext);
            SetValue(LayoutDataContextProperty, map);

            UndoStack.Clear();
            RedoStack.Clear();
        }

        private void ReloadLayoutInCanvas(IMapLayout map)
        {
            
            Children.Clear();

            UndoStack.Clear();
            RedoStack.Clear();


            var diagrams = map.Diagrams;

            if (diagrams == null || !diagrams.Any()) return;

            Dictionary<string, ILineLayout[]> uuidLines = new Dictionary<string, ILineLayout[]>();

            foreach (var diagram in diagrams)
            {
                var clnele = Activator.CreateInstance(diagram.DiagramUIType) as UIElement;

                if (clnele is ContentControl)
                {
                    (clnele as ContentControl).DataContext = diagram.DataContext;
                }

                //add the line by Diagram ActionComment after finish all diagrams
                uuidLines.Add(diagram.DiagramUUID, diagram.DepartureLines);

                Application.Current.Dispatcher.Invoke(() =>
                {

                    Canvas.SetRight(clnele, diagram.X);
                    Canvas.SetLeft(clnele, diagram.X);
                    Canvas.SetBottom(clnele, diagram.Y);
                    Canvas.SetTop(clnele, diagram.Y);


                    Children.Add(clnele);
                });
            }

            //update the canvas.
            UpdateLayout();


            foreach (var dialines in uuidLines)
            {
                ConnectionDiagramBase origindiagram = GetDiagramByUUID(dialines.Key);
                if (origindiagram != null && dialines.Value != null)
                {
                    foreach (var defline in dialines.Value)
                    {
                        var terminaldiagram = GetDiagramByUUID(defline.TerminalDiagramUUID);

                        dynamic conline;

                        if (terminaldiagram != null)
                        {
                            conline = Activator.CreateInstance(defline.LineUIType);
                            if (conline is ConnectionLineBase)
                            {

                                (conline as ConnectionLineBase).OriginDiagram = origindiagram;
                                (conline as ConnectionLineBase).TerminalDiagram = terminaldiagram;
                                (conline as ConnectionLineBase).DataContext = defline.DataContext;
                                (conline as ConnectionLineBase).LineUUID = string.IsNullOrWhiteSpace(defline.LineUUID) ? $"{conline.GetType().Name}_{Guid.NewGuid().ToString()}" : defline.LineUUID;


                                //TODOTODOTODO!!!!
                                //TODO: Posistion Recaluter
                                if (conline is ILinePosition)
                                {
                                    (conline as ILinePosition).X1 = (double)origindiagram.GetValue(Canvas.LeftProperty) - (double)terminaldiagram.GetValue(Canvas.LeftProperty) + origindiagram.CenterPosition.X;
                                    (conline as ILinePosition).Y1 = (double)origindiagram.GetValue(Canvas.TopProperty) - (double)terminaldiagram.GetValue(Canvas.TopProperty) + origindiagram.CenterPosition.Y; ;
                                    (conline as ILinePosition).X2 = terminaldiagram.CenterPosition.X;
                                    (conline as ILinePosition).Y2 = terminaldiagram.CenterPosition.Y;

                                }

                                origindiagram.DepartureLines.Add(conline);
                                terminaldiagram.ArrivalLines.Add(conline);

                            }
                            Application.Current.Dispatcher.Invoke(() =>
                            {

                                Canvas.SetTop(conline, (double)terminaldiagram.GetValue(Canvas.TopProperty));
                                Canvas.SetLeft(conline, (double)terminaldiagram.GetValue(Canvas.LeftProperty));

                                Children.Add(conline);
                            });
                        }
                    }
                }
            }
        }

        private ConnectionDiagramBase GetDiagramByUUID(string uuid)
        {
            ConnectionDiagramBase result = null;
            foreach (var child in Children)
            {
                if (!(child is ConnectionDiagramBase)) continue;

                if ((child as ConnectionDiagramBase).DiagramUUID == uuid)
                {
                    result = (child as ConnectionDiagramBase);
                    break;
                }
            }

            return result;
        }


        private void SaveAsImage(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return;


            var fileext = path.Split('.');


            // recalculate this canvas
            var size = new Size(double.NaN.Equals(Width) ? ActualWidth : Width,
                 double.NaN.Equals(Height) ? ActualHeight : Height);
            Measure(size);
            Arrange(new Rect(size));

            // convert VisualObject to Bitmap
            var renderBitmap = new RenderTargetBitmap((int)size.Width,       // width
                                                      (int)size.Height,      // Height
                                                      96.0d,                 // Horizonal 96.0DPI
                                                      96.0d,                 // Vertual 96.0DPI
                                                      PixelFormats.Pbgra32); // 32bit(RGBA 8bit)
            renderBitmap.Render(this);

            // Default encoder is PNG
            BitmapEncoder encoder = new PngBitmapEncoder(); ;
            if (fileext.Length >= 2 && fileext[fileext.Length - 1] == "jpg")
            {
                encoder = new JpegBitmapEncoder();
            }


            // Output FileStream 
            using (var os = new FileStream(path, FileMode.Create))
            {
                // Create the Bitmap FileStream
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(os);
            }

            UndoStack.Clear();
            RedoStack.Clear();

        }



        #endregion


        #region Virtual Method
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
                      var canvas = d as ExtensionCanvasBase;

                      (d as ExtensionCanvasBase).ReloadLayoutInCanvas(map);

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

        #region SaveAsImageFileName
        /// <summary>
        /// The <see cref="SaveAsImageFileName" /> dependency property's name.
        /// </summary>
        public const string SaveAsImageFileNamePropertyName = "SaveAsImageFileName";

        /// <summary>
        /// Gets or sets the value of the <see cref="SaveAsImageFileName" />
        /// property. This is a dependency property.
        /// </summary>
        public string SaveAsImageFileName
        {
            get
            {
                return (string)GetValue(SaveAsImageFileNameProperty);
            }
            set
            {
                SetValue(SaveAsImageFileNameProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="SaveAsImageFileName" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty SaveAsImageFileNameProperty = DependencyProperty.Register(
            SaveAsImageFileNamePropertyName,
            typeof(string),
            typeof(ExtensionCanvasBase),
            new UIPropertyMetadata(string.Empty,
                (d, e) =>
                {
                    if(!string.IsNullOrWhiteSpace(e.NewValue as string))
                    {
                        (d as ExtensionCanvasBase).SaveAsImage(e.NewValue as string);
                    }

                }));
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

            //if (!_isUndoHandle) RedoStack.Clear();

            //update the layout object mode by is sync
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
            }

            base.OnDrop(e);

            RedoStack.Clear();


        }
        #endregion



        #endregion
    }
}
