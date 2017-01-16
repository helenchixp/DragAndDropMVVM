using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DragAndDropMVVM.Controls
{
    public class ConnectionLineBase : Thumb
    {
        #region Override Method

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
        #endregion

        #region Property

        public ConnectionDiagramBase OriginDiagram { get; internal set; }

        public ConnectionDiagramBase TerminalDiagram { get; internal set; }

        #endregion


        #region Dependence Properties

        #region DoubleClickCommand

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

    }
}
