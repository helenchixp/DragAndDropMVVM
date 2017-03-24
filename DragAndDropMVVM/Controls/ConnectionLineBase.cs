using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Xml.Serialization;
using DragAndDropMVVM.Assist;
using DragAndDropMVVM.ViewModel;

namespace DragAndDropMVVM.Controls
{
    public abstract class ConnectionLineBase : Thumb
    {
        #region Override Method

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete,
                        (sender, e) =>
                        {
                            DeleteCommand.Execute(DeleteCommandParameter);
                            DeleteLine();
                        },
                        (sender, e) =>
                        {
                            e.CanExecute = DeleteCommand?.CanExecute(DeleteCommandParameter) ?? false;
                        }
                        ));
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            IsSelected = true;
            base.OnGotFocus(e);
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            if(DoubleClickCommand?.CanExecute(DoubleClickCommandParameter) ?? false)
            {
                DoubleClickCommand.Execute(DoubleClickCommandParameter);
            }

            base.OnMouseDoubleClick(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            IsSelected = false;

            base.OnLostFocus(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
            {
                if (DeleteCommand?.CanExecute(DeleteCommandParameter) ?? false)
                {
                    DeleteCommand.Execute(DeleteCommandParameter);
                    DeleteLine();
                }
            }
            else if(e.Key == Key.Enter)
            {
                if (DoubleClickCommand?.CanExecute(DoubleClickCommandParameter) ?? false)
                {
                    DoubleClickCommand.Execute(DoubleClickCommandParameter);
                }
            }

            base.OnKeyDown(e);

        }

        #endregion

        #region Property
       

        public ConnectionDiagramBase OriginDiagram
        {
            get
            {
                return FrameworkElementAssist.GetOriginDiagram(this) as ConnectionDiagramBase;
            }
            //internal set;
        }
       

        public ConnectionDiagramBase TerminalDiagram
        {
            get
            {
                return FrameworkElementAssist.GetTerminalDiagram(this) as ConnectionDiagramBase;
            }
            //internal set;
        }


        #region LineUUID


        /// <summary>
        /// Gets or sets the value of the <see cref="LineUUID" />
        /// property. This is a dependency property.
        /// </summary>
        public virtual string LineUUID
        {
            get;
            set;
        }


        #endregion


        #endregion

        #region Dependence Properties

        #region DoubleClickCommand

        /// <summary>
        /// The <see cref="DoubleClickCommand" /> dependency property's name.
        /// </summary>
        public const string DoubleClickCommandPropertyName = "DoubleClickCommand";

        /// <summary>
        /// Gets or sets the value of the <see cref="DoubleClickCommand" />
        /// property. This is a dependency property.
        /// </summary>
        public ICommand DoubleClickCommand
        {
            get
            {
                return (ICommand)GetValue(DoubleClickCommandProperty);
            }
            set
            {
                SetValue(DoubleClickCommandProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="DoubleClickCommand" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty DoubleClickCommandProperty = DependencyProperty.Register(
            DoubleClickCommandPropertyName,
            typeof(ICommand),
            typeof(ConnectionLineBase),
            new UIPropertyMetadata(null));
        #endregion

        #region DoubleClickCommandParameter
        /// <summary>
        /// The <see cref="DoubleClickCommandParameter" /> dependency property's name.
        /// </summary>
        public const string DoubleClickCommandParameterPropertyName = "DoubleClickCommandParameter";

        /// <summary>
        /// Gets or sets the value of the <see cref="DoubleClickCommandParameter" />
        /// property. This is a dependency property.
        /// </summary>
        public object DoubleClickCommandParameter
        {
            get
            {
                return (object)GetValue(DoubleClickCommandParameterProperty);
            }
            set
            {
                SetValue(DoubleClickCommandParameterProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="DoubleClickCommandParameter" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty DoubleClickCommandParameterProperty = DependencyProperty.Register(
            DoubleClickCommandParameterPropertyName,
            typeof(object),
            typeof(ConnectionLineBase),
            new UIPropertyMetadata(null));

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
            typeof(ConnectionLineBase),
            new UIPropertyMetadata(null));

        #endregion

        #region DeleteCommandParameter

        /// <summary>
        /// The <see cref="DeleteCommandParameter" /> dependency property's name.
        /// </summary>
        public const string DeleteCommandParameterPropertyName = "DeleteCommandParameter";

        /// <summary>
        /// Gets or sets the value of the <see cref="DeleteCommandParameter" />
        /// property. This is a dependency property.
        /// </summary>
        public object DeleteCommandParameter
        {
            get
            {
                return (object)GetValue(DeleteCommandParameterProperty);
            }
            set
            {
                SetValue(DeleteCommandParameterProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="DeleteCommandParameter" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty DeleteCommandParameterProperty = DependencyProperty.Register(
            DeleteCommandParameterPropertyName,
            typeof(object),
            typeof(ConnectionLineBase),
            new UIPropertyMetadata(null));

        #endregion

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
            typeof(ConnectionLineBase),
            new UIPropertyMetadata(false,
                (d, e) =>
                {
                    if ((d is ConnectionLineBase) &&
                        (d as ConnectionLineBase).DataContext is IConnectionLineViewModel)
                    {
                        ((d as ConnectionLineBase).DataContext as IConnectionLineViewModel).IsSelected = (bool)e.NewValue;
                    }

                }));

        #endregion

        #endregion

        #region Virtual Method

        public virtual void ResetPosition(DragEventArgs e)
        {

        }
        #endregion

        #region Private Method
        internal void DeleteLine()
        {
            if (OriginDiagram != null && OriginDiagram.DepartureLines.Any())
            {
                OriginDiagram.DepartureLines.Remove(this);
            }

            if (TerminalDiagram != null && TerminalDiagram.ArrivalLines.Any())
            {
                TerminalDiagram.ArrivalLines.Remove(this);
            }

            WPFUtility.FindVisualParent<Canvas>(this).Children.Remove(this);
        }

        #endregion


    }
}
