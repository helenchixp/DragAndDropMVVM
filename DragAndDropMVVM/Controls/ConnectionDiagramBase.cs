using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Xml.Serialization;
using DragAndDropMVVM.Behavior;
using DragAndDropMVVM.ViewModel;

namespace DragAndDropMVVM.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ConnectionDiagramBase : ContentControl
    {
        public ConnectionDiagramBase()
        {

        }

        #region override method


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Focusable = true;
            IsHitTestVisible = true;

            //set the AllowDrop
            if (IsDrawLineDropEnabled)
            {
                this.AllowDrop = true;

                if (this.Content is UIElement)
                {
                    SetContentAllowDrop(this.Content as UIElement);
                }
            }

            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete,
                                    (sender, e) =>
                                    {
                                        DeleteCommand.Execute(DataContext);
                                        DeleteDiagramAndLines();
                                    },
                                    (sender, e) =>
                                    {
                                        e.CanExecute = DeleteCommand?.CanExecute(DataContext) ?? false;
                                    }
                                    ));

        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            Focus();
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            IsSelected = true;

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            IsSelected = false;

            base.OnLostFocus(e);
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {

            if (e.Key == Key.Delete)
            {
                if (DeleteCommand != null && DeleteCommand.CanExecute(DataContext))
                {
                    DeleteCommand.Execute(DataContext);
                    DeleteDiagramAndLines();
                }
            }

            base.OnKeyDown(e);
        }

        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            base.OnContextMenuOpening(e);



        }

        protected override void OnContextMenuClosing(ContextMenuEventArgs e)
        {
            base.OnContextMenuClosing(e);
        }

        #endregion

        #region private method

        private void SetContentAllowDrop(DependencyObject d)
        {
            if (d == null) return;

            PropertyInfo propdrop = d.GetType().GetProperty("AllowDrop");

            if (propdrop != null)
            {
                propdrop.SetValue(d, true);
            }

            try
            {
                for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(d) - 1; i++)
                {
                    DependencyObject root = VisualTreeHelper.GetChild(d, i);

                    SetContentAllowDrop(root);
                }

                return;
            }
            catch
            {
                return;
            }
        }


        private void DeleteDiagramAndLines()
        {
            if (DepartureLines != null && DepartureLines.Any())
            {
                foreach (var dline in DepartureLines)
                {
                    dline.TerminalDiagram.ArrivalLines.Remove(dline);

                    WPFUtil.FindVisualParent<Canvas>(dline).Children.Remove(dline);
                }
            }

            if (ArrivalLines != null && ArrivalLines.Any())
            {
                foreach (var aline in ArrivalLines)
                {
                    aline.OriginDiagram.DepartureLines.Remove(aline);

                    WPFUtil.FindVisualParent<Canvas>(aline).Children.Remove(aline);

                }
            }

            this.ArrivalLines.Clear();
            this.DepartureLines.Clear();

            WPFUtil.FindVisualParent<Canvas>(this).Children.Remove(this);
        }
        #endregion

        #region Dependence Properties

        #region IsSelected
        /// <summary>
        /// The <see cref="IsSelected" /> dependency property's name.
        /// </summary>
        public const string IsSelectedPropertyName = "IsSelected";

        /// <summary>
        /// Gets or sets the value of the <see cref="IsSelected" />
        /// property. This is a dependency property.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return (bool)GetValue(IsSelectedProperty);
            }
            set
            {
                SetValue(IsSelectedProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="IsSelected" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            IsSelectedPropertyName,
            typeof(bool),
            typeof(ConnectionDiagramBase),
            new UIPropertyMetadata(false,
                    (d, e) =>
                    {
                        if((d is ConnectionDiagramBase) &&
                            (d as ConnectionDiagramBase).DataContext is IConnectionDiagramViewModel)
                        {
                            ((d as ConnectionDiagramBase).DataContext as IConnectionDiagramViewModel).IsSelected = (bool)e.NewValue;
                        }
                    }));
        #endregion

        #region IsDrawLineDropEnabled
        /// <summary>
        /// The <see cref="IsDrawLineDropEnabled" /> dependency property's name.
        /// </summary>
        public const string IsDrawLineDropEnabledPropertyName = "IsDrawLineDropEnabled";

        /// <summary>
        /// Gets or sets the value of the <see cref="IsDrawLineDropEnabled" />
        /// property. This is a dependency property.
        /// </summary>
        public bool IsDrawLineDropEnabled
        {
            get
            {
                return (bool)GetValue(IsDrawLineDropEnabledProperty);
            }
            set
            {
                SetValue(IsDrawLineDropEnabledProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="IsDrawLineDropEnabled" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDrawLineDropEnabledProperty = DependencyProperty.Register(
            IsDrawLineDropEnabledPropertyName,
            typeof(bool),
            typeof(ConnectionDiagramBase),
            new UIPropertyMetadata(true));
        #endregion

        #region ConnectorPositionType
        /// <summary>
        /// The <see cref="ConnectorPositionType" /> dependency property's name.
        /// </summary>
        public const string ConnectorPositionTypePropertyName = "ConnectorPositionType";

        /// <summary>
        /// Gets or sets the value of the <see cref="ConnectorPositionType" />
        /// property. This is a dependency property.
        /// </summary>
        public ConnectorPositionType ConnectorPositionType
        {
            get
            {
                return (ConnectorPositionType)GetValue(ConnectorPositionTypeProperty);
            }
            set
            {
                SetValue(ConnectorPositionTypeProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="ConnectorPositionType" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty ConnectorPositionTypeProperty = DependencyProperty.Register(
            ConnectorPositionTypePropertyName,
            typeof(ConnectorPositionType),
            typeof(ConnectionDiagramBase),
            new UIPropertyMetadata(ConnectorPositionType.Custom));
        #endregion

        #region DeleteCommand
        /// <summary>
        /// The <see cref="DeleteCommand" /> dependency property's name.
        /// </summary>
        public const string DeleteCommandPropertyName = "DeleteCommand";

        /// <summary>
        /// Gets or sets the value of the <see cref="DeleteCommand" />
        /// property. This is a dependency property.
        /// </summary>
        public ICommand DeleteCommand
        {
            get
            {
                return (ICommand)GetValue(DeleteCommandProperty);
            }
            set
            {
                SetValue(DeleteCommandProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="DeleteCommand" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty DeleteCommandProperty = DependencyProperty.Register(
            DeleteCommandPropertyName,
            typeof(ICommand),
            typeof(ConnectionDiagramBase),
            new UIPropertyMetadata(null, (d, e) =>
            {

            },
                (d, o) =>
                {
                    if (o is ICommand)
                    {
                        //(o as ICommand).CanExecuteChanged += (s, e) =>
                        //{
                        //    System.Diagnostics.Debug.WriteLine("00000000000000000000000");
                        //};

                    }

                    return o;
                }
                ));
        #endregion

        #region AdornerType
        /// <summary>
        /// The <see cref="AdornerType" /> dependency property's name.
        /// </summary>
        public const string AdornerTypePropertyName = "AdornerType";

        /// <summary>
        /// Gets or sets the value of the <see cref="AdornerType" />
        /// property. This is a dependency property.
        /// </summary>
        public FrameworkElementAdornerType AdornerType
        {
            get
            {
                return (FrameworkElementAdornerType)GetValue(AdornerTypeProperty);
            }
            set
            {
                SetValue(AdornerTypeProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="AdornerType" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty AdornerTypeProperty = DependencyProperty.Register(
            AdornerTypePropertyName,
            typeof(FrameworkElementAdornerType),
            typeof(ConnectionDiagramBase),
            new UIPropertyMetadata(FrameworkElementAdornerType.DrawEllipse));
        #endregion


        #endregion

        #region Property

        public ObservableCollection<ConnectionLineBase> DepartureLines { get; internal set; } = new ObservableCollection<ConnectionLineBase>();

        public ObservableCollection<ConnectionLineBase> ArrivalLines { get; internal set; } = new ObservableCollection<ConnectionLineBase>();

        #region DiagramUUID
        /// <summary>
        /// Gets or sets the value of the <see cref="DiagramUUID" />
        /// property. This is a dependency property.
        /// </summary>
        public virtual string DiagramUUID
        {
            get;
            set;
        }

   
        #endregion

        #region CenterPosition

        /// <summary>
        /// Gets or sets the value of the <see cref="CenterPosition" />
        /// property. This is a dependency property.
        /// </summary>
        public virtual Point CenterPosition
        {
            get
            {

                Point retpoint = new Point(0, 0);

                double elewidth = ActualWidth != 0.0 ? ActualWidth : ((Content as FrameworkElement)?.ActualWidth ?? 0.0);
                double eleheight = ActualHeight != 0.0 ? ActualHeight : ((Content as FrameworkElement)?.ActualHeight ?? 0.0);

                switch (ConnectorPositionType)
                {
                    case ConnectorPositionType.Center:
                        retpoint = new Point(elewidth / 2, eleheight / 2);
                        break;
                    case ConnectorPositionType.Top:
                        retpoint = new Point(elewidth / 2, 0);
                        break;
                    case ConnectorPositionType.TopLeft:
                        retpoint = new Point(0, 0);
                        break;
                    case ConnectorPositionType.TopRight:
                        retpoint = new Point(elewidth, 0);
                        break;

                    case ConnectorPositionType.Left:
                        retpoint = new Point(0, eleheight / 2);
                        break;
                    case ConnectorPositionType.Bottom:
                        retpoint = new Point(elewidth / 2, eleheight);
                        break;
                    case ConnectorPositionType.BottomLeft:
                        retpoint = new Point(0, eleheight);
                        break;
                    case ConnectorPositionType.BottomRight:
                        retpoint = new Point(elewidth, eleheight);
                        break;
                    case ConnectorPositionType.Right:
                        retpoint = new Point(elewidth, eleheight / 2);
                        break;

                    default:
                        break;
                }
                return retpoint;
            }

        }

        #endregion


        #endregion

        #region Public Method

        #region ResetLinesPosition
        //public void ResetLinesPosition(DragEventArgs e)
        //{
        //    foreach(var dline in DepartureLines)
        //    {
        //        dline.ResetPosition(e);
        //    }
        //    foreach(var aline in ArrivalLines)
        //    {
        //        aline.ResetPosition(e);
        //    }
        //}
        #endregion

        #endregion

    }
}
