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
            bhcol.Add(new DrawLineDragBehavior());

            if(GetIsDrawLineDropEnabled(this))
            {
                bhcol.Add(new DrawLineDropBehavior());
            }
        }

        #region override method

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            //set the AllowDrop
            if (GetIsDrawLineDropEnabled(this))
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

        #region Attached Property


        #region IsSelected
        /// <summary>
        /// The IsSelected attached property's name.
        /// </summary>
        public const string IsSelectedPropertyName = "IsSelected";

        /// <summary>
        /// Gets the value of the IsSelected attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the IsSelected property of the specified object.</returns>
        public static bool GetIsSelected(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSelectedProperty);
        }

        /// <summary>
        /// Sets the value of the IsSelected attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the IsSelected value of the specified object.</param>
        public static void SetIsSelected(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSelectedProperty, value);
        }

        /// <summary>
        /// Identifies the IsSelected attached property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.RegisterAttached(
            IsSelectedPropertyName,
            typeof(bool),
            typeof(ConnectionDiagramBase),
            new UIPropertyMetadata(false));
        #endregion


        #region CenterPosition
        /// <summary>
        /// The CenterPosition attached property's name.
        /// </summary>
        public const string CenterPositionPropertyName = "CenterPosition";

        /// <summary>
        /// Gets the value of the CenterPosition attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the CenterPosition property of the specified object.</returns>
        public static Point? GetCenterPosition(DependencyObject obj)
        {
            return (Point?)obj.GetValue(CenterPositionProperty);
        }

        /// <summary>
        /// Sets the value of the CenterPosition attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the CenterPosition value of the specified object.</param>
        public static void SetCenterPosition(DependencyObject obj, Point? value)
        {
            obj.SetValue(CenterPositionProperty, value);
        }

        /// <summary>
        /// Identifies the CenterPosition attached property.
        /// </summary>
        public static readonly DependencyProperty CenterPositionProperty = DependencyProperty.RegisterAttached(
            CenterPositionPropertyName,
            typeof(Point?),
            typeof(ConnectionDiagramBase),
            new UIPropertyMetadata(null));

        #endregion


        #region IsDrawLineDropEnabled
        /// <summary>
        /// The IsDrawLineDropEnabled attached property's name.
        /// </summary>
        public const string IsDrawLineDropEnabledPropertyName = "IsDrawLineDropEnabled";

        /// <summary>
        /// Gets the value of the IsDrawLineDropEnabled attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the IsDrawLineDropEnabled property of the specified object.</returns>
        public static bool GetIsDrawLineDropEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDrawLineDropEnabledProperty);
        }

        /// <summary>
        /// Sets the value of the IsDrawLineDropEnabled attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the IsDrawLineDropEnabled value of the specified object.</param>
        public static void SetIsDrawLineDropEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDrawLineDropEnabledProperty, value);
        }

        /// <summary>
        /// Identifies the IsDrawLineDropEnabled attached property.
        /// </summary>
        public static readonly DependencyProperty IsDrawLineDropEnabledProperty = DependencyProperty.RegisterAttached(
            IsDrawLineDropEnabledPropertyName,
            typeof(bool),
            typeof(ConnectionDiagramBase),
            new UIPropertyMetadata(true));
        #endregion

        #endregion


        #region Property

        public ObservableCollection<ConnectionLineBase> DepartureLines { get; internal set; } = new ObservableCollection<ConnectionLineBase>();

        public ObservableCollection<ConnectionLineBase> ArrivalLines { get; internal set; } = new ObservableCollection<ConnectionLineBase>();

        #endregion

    }
}
