using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DragAndDropMVVM.Controls
{
    public class ConnectionLineBase : Thumb
    {
        #region Override Method

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete,
                        (sender, e) =>
                        {
                            DeleteCommand.Execute(DataContext);
                            DeleteLine();
                        },
                        (sender, e) =>
                        {
                            e.CanExecute = DeleteCommand?.CanExecute(DataContext) ?? false;
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
                if (DeleteCommand?.CanExecute(DataContext) ?? false)
                {
                    DeleteCommand.Execute(DataContext);
                    DeleteLine();
                }
            }

            base.OnKeyDown(e);

        }

        #endregion

        #region Property

        public ConnectionDiagramBase OriginDiagram { get; internal set; }

        public ConnectionDiagramBase TerminalDiagram { get; internal set; }

        #endregion


        #region Dependence Properties

        #region DoubleClickCommand

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
            new UIPropertyMetadata(false));

        #endregion

        #endregion

        #region Virtual Method

        public virtual void ResetPosition(DragEventArgs e)
        {

        }
        #endregion

        #region Private Method
        private void DeleteLine()
        {
            if (OriginDiagram != null && OriginDiagram.DepartureLines.Any())
            {
                OriginDiagram.DepartureLines.Remove(this);
            }

            if (TerminalDiagram != null && TerminalDiagram.ArrivalLines.Any())
            {
                TerminalDiagram.ArrivalLines.Remove(this);
            }

            WPFUtil.FindVisualParent<Canvas>(this).Children.Remove(this);
        }

        #endregion


    }
}
