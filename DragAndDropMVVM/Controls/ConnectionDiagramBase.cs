using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using DragAndDropMVVM.Behavior;

namespace DragAndDropMVVM.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ConnectionDiagramBase : ContentControl
    {
        public ConnectionDiagramBase()
        {
            //Add the Behavior in code-behind
            BehaviorCollection bhcol= Interaction.GetBehaviors(this);
            bhcol.Add(new FrameworkElementDragBehavior());
            bhcol.Add(new DrawLineDragBehavior());

            if(IsDrawLineDropEnabled)
            {
                bhcol.Add(new DrawLineDropBehavior());
            }
        }

        #region override method

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            //set the AllowDrop
            if (IsDrawLineDropEnabled)
            {
                this.AllowDrop = true;

                if(this.Content is UIElement)
                {
                    // (this.Content as UIElement).AllowDrop = true;
                    SetContentAllowDrop(this.Content as UIElement);
                }
            }

            
        }

        #endregion

        #region private method

        private void SetContentAllowDrop(DependencyObject d)
        {
            if (d == null) return;

            PropertyInfo propdrop = d.GetType().GetProperty("AllowDrop");

            if(propdrop != null)
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
            new UIPropertyMetadata(false));
        #endregion

        #region CenterPosition
        /// <summary>
        /// The <see cref="CenterPosition" /> dependency property's name.
        /// </summary>
        public const string CenterPositionPropertyName = "CenterPosition";

        /// <summary>
        /// Gets or sets the value of the <see cref="CenterPosition" />
        /// property. This is a dependency property.
        /// </summary>
        public Point? CenterPosition
        {
            get
            {
                return (Point?)GetValue(CenterPositionProperty);
            }
            set
            {
                SetValue(CenterPositionProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="CenterPosition" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty CenterPositionProperty = DependencyProperty.Register(
            CenterPositionPropertyName,
            typeof(Point?),
            typeof(ConnectionDiagramBase),
            new UIPropertyMetadata(null));
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

        #endregion

        #region Property

        public ObservableCollection<ConnectionLineBase> DepartureLines { get; internal set; } = new ObservableCollection<ConnectionLineBase>();

        public ObservableCollection<ConnectionLineBase> ArrivalLines { get; internal set; } = new ObservableCollection<ConnectionLineBase>();

        #endregion

    }
}
