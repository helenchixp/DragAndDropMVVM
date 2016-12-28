using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using DragAndDropMVVM.Behavior;

namespace DragAndDropMVVM.Controls
{
    public abstract class ConnectionDiagramBase : ContentControl
    {
        public ConnectionDiagramBase()
        {
            //Add the Behavior in code-behind
            BehaviorCollection bcollection = Interaction.GetBehaviors(this);
            bcollection.Add(new DrawLineDragBehavior());
        }

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

        #endregion

        //public virtual Point CenterPosition
        //{
        //    get;set;
        //}


        //public bool IsSelected
        //{
        //    get;set;
        //}
    }
}
