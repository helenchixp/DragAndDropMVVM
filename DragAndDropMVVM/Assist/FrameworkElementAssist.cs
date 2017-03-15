using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DragAndDropMVVM.Assist
{
    public static class FrameworkElementAssist
    {
        #region DiagramUUID
        /// <summary>
        /// The DiagramUUID attached property's name.
        /// </summary>
        public const string DiagramUUIDPropertyName = "DiagramUUID";

        /// <summary>
        /// Gets the value of the DiagramUUID attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the DiagramUUID property of the specified object.</returns>
        public static string GetDiagramUUID(DependencyObject obj)
        {
            return (string)obj.GetValue(DiagramUUIDProperty);
        }

        /// <summary>
        /// Sets the value of the DiagramUUID attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the DiagramUUID value of the specified object.</param>
        public static void SetDiagramUUID(DependencyObject obj, string value)
        {
            obj.SetValue(DiagramUUIDProperty, value);
        }

        /// <summary>
        /// Identifies the DiagramUUID attached property.
        /// </summary>
        public static readonly DependencyProperty DiagramUUIDProperty = DependencyProperty.RegisterAttached(
            DiagramUUIDPropertyName,
            typeof(string),
            typeof(FrameworkElementAssist),
            new UIPropertyMetadata(null));
        #endregion

        #region DepartureLines

        #endregion

        #region ArrivalLines

        #endregion


        #region LineUUID
        /// <summary>
        /// The LineUUID attached property's name.
        /// </summary>
        public const string LineUUIDPropertyName = "LineUUID";

        /// <summary>
        /// Gets the value of the LineUUID attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the LineUUID property of the specified object.</returns>
        public static string GetLineUUID(DependencyObject obj)
        {
            return (string )obj.GetValue(LineUUIDProperty);
        }

        /// <summary>
        /// Sets the value of the LineUUID attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the LineUUID value of the specified object.</param>
        public static void SetLineUUID(DependencyObject obj, string  value)
        {
            obj.SetValue(LineUUIDProperty, value);
        }

        /// <summary>
        /// Identifies the LineUUID attached property.
        /// </summary>
        public static readonly DependencyProperty LineUUIDProperty = DependencyProperty.RegisterAttached(
            LineUUIDPropertyName,
            typeof(string),
            typeof(FrameworkElementAssist),
            new UIPropertyMetadata(null));
        #endregion

        #region OriginDiagram

        /// <summary>
        /// The OriginDiagram attached property's name.
        /// </summary>
        public const string OriginDiagramPropertyName = "OriginDiagram";

        /// <summary>
        /// Gets the value of the OriginDiagram attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the OriginDiagram property of the specified object.</returns>
        public static FrameworkElement GetOriginDiagram(DependencyObject obj)
        {
            return (FrameworkElement)obj.GetValue(OriginDiagramProperty);
        }

        /// <summary>
        /// Sets the value of the OriginDiagram attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the OriginDiagram value of the specified object.</param>
        public static void SetOriginDiagram(DependencyObject obj, FrameworkElement value)
        {
            obj.SetValue(OriginDiagramProperty, value);
        }

        /// <summary>
        /// Identifies the OriginDiagram attached property.
        /// </summary>
        public static readonly DependencyProperty OriginDiagramProperty = DependencyProperty.RegisterAttached(
            OriginDiagramPropertyName,
            typeof(FrameworkElement),
            typeof(FrameworkElementAssist),
            new UIPropertyMetadata(null));

        #endregion

        #region TerminalDiagram

        /// <summary>
        /// The TerminalDiagram attached property's name.
        /// </summary>
        public const string TerminalDiagramPropertyName = "TerminalDiagram";

        /// <summary>
        /// Gets the value of the TerminalDiagram attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the TerminalDiagram property of the specified object.</returns>
        public static FrameworkElement GetTerminalDiagram(DependencyObject obj)
        {
            return (FrameworkElement)obj.GetValue(TerminalDiagramProperty);
        }

        /// <summary>
        /// Sets the value of the TerminalDiagram attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the TerminalDiagram value of the specified object.</param>
        public static void SetTerminalDiagram(DependencyObject obj, FrameworkElement value)
        {
            obj.SetValue(TerminalDiagramProperty, value);
        }

        /// <summary>
        /// Identifies the TerminalDiagram attached property.
        /// </summary>
        public static readonly DependencyProperty TerminalDiagramProperty = DependencyProperty.RegisterAttached(
            TerminalDiagramPropertyName,
            typeof(FrameworkElement),
            typeof(FrameworkElementAssist),
            new UIPropertyMetadata(null));
        #endregion

    }
}
