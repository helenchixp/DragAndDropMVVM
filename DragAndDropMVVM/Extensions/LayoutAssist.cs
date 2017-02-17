using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DragAndDropMVVM.Model;

namespace DragAndDropMVVM.Extensions
{
    public class LayoutAssist
    {


        #region Attached Property

        #region LayoutDataContext

        /// <summary>
        /// The LayoutDataContext attached property's name.
        /// </summary>
        public const string LayoutDataContextPropertyName = "LayoutDataContext";

        /// <summary>
        /// Gets the value of the LayoutDataContext attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the LayoutDataContext property of the specified object.</returns>
        public static object GetLayoutDataContext(DependencyObject obj)
        {
            return (object)obj.GetValue(LayoutDataContextProperty);
        }

        /// <summary>
        /// Sets the value of the LayoutDataContext attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the LayoutDataContext value of the specified object.</param>
        public static void SetLayoutDataContext(DependencyObject obj, object value)
        {
            obj.SetValue(LayoutDataContextProperty, value);
        }

        /// <summary>
        /// Identifies the LayoutDataContext attached property.
        /// </summary>
        public static readonly DependencyProperty LayoutDataContextProperty = DependencyProperty.RegisterAttached(
            LayoutDataContextPropertyName,
            typeof(object),
            typeof(LayoutAssist),
            new UIPropertyMetadata(null,
                (d, e) =>
                {
                    if (e.NewValue == null
                        || !(e.NewValue is IMapLayout)
                        || !(d is Canvas))
                        return;
                    var map = (e.NewValue as IMapLayout);

                    Canvas canvas = (d as Canvas);
                    canvas.Width = map.Width;
                    canvas.Height = map.Height;
                    canvas.LoadLayout(map.Diagrams );
                   // (e.NewValue as IMapLayout).GetDiagramsPosition = ()=>(d as Canvas).SetExportPosition((e.NewValue as IMapLayout).Diagrams);

                }));
        #endregion

        #endregion

    }
}
