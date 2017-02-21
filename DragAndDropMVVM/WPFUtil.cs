using System;
using System.Windows;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Windows.Markup;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

namespace DragAndDropMVVM
{
    class WPFUtil
    {
        #region FindVisualParent
        public static T FindVisualParent<T>(DependencyObject d) where T : DependencyObject
        {
            if (d == null) return null;

            try
            {
                DependencyObject root = VisualTreeHelper.GetParent(d);

                if (root != null && root is T)
                {
                    return root as T;
                }
                else
                {
                    T parent = FindVisualParent<T>(root);
                    if (parent != null) return parent;
                }

                return null;
            }
            catch
            {
                if (d is FrameworkElement)
                {
                    FrameworkElement element = (FrameworkElement)d;
                    if (element.Parent is T) return element.Parent as T;
                    return FindVisualParent<T>(element.Parent);
                }
                else if (d is FrameworkContentElement)
                {
                    FrameworkContentElement element = (FrameworkContentElement)d;
                    if (element.Parent is T) return element.Parent as T;
                    return FindVisualParent<T>(element.Parent);
                }
                else
                {
                    return null;
                }
            }
        }

        public static T FindVisualParent<T>(DependencyObject d, string name) where T : DependencyObject
        {
            DependencyObject root = d;
            while (true)
            {
                DependencyObject parent = FindVisualParent<T>(root);
                if (parent == null)
                {
                    return null;
                }
                else
                {
                    if (parent is FrameworkElement)
                    {
                        if (((FrameworkElement)parent).Name == name) return parent as T;
                    }
                    else if (parent is FrameworkContentElement)
                    {
                        if (((FrameworkContentElement)parent).Name == name) return parent as T;
                    }
                    else
                    {
                        return null;
                    }

                    root = parent;
                }
            }
        }

        #endregion

        #region FindVisualChild

        public static T FindVisualChild<T>(DependencyObject d) where T : DependencyObject
        {
            if (d == null) return null;

            try
            {
                for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(d) - 1; i++)
                {
                    DependencyObject root = VisualTreeHelper.GetChild(d, i);
                    if (root != null && root is T)
                    {
                        return root as T;
                    }
                    else
                    {
                        T child = FindVisualChild<T>(root);
                        if (child != null) return child;
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public static T FindVisualChild<T>(DependencyObject d, string name) where T : DependencyObject
        {
            if (d == null) return null;

            try
            {
                for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(d) - 1; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(d, i);
                    if (child != null && child is T)
                    {
                        if (child is FrameworkElement)
                        {
                            if (((FrameworkElement)child).Name == name) return child as T;
                        }
                        else if (child is FrameworkContentElement)
                        {
                            if (((FrameworkContentElement)child).Name == name) return child as T;
                        }
                        else
                        {
                            return null;
                        }
                    }

                    T nextChild = FindVisualChild<T>(child, name);
                    if (nextChild != null) return nextChild;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region GetMousePosition

        [DllImport("user32.dll")]
        private static extern void GetCursorPos(out POINT pt);

        [DllImport("user32.dll")]
        private static extern int ScreenToClient(IntPtr hwnd, ref POINT pt);

        private struct POINT
        {
            public UInt32 X;
            public UInt32 Y;
        }

        public static Point GetMousePosition(System.Windows.Media.Visual visual)
        {
            POINT point;
            GetCursorPos(out point);

            HwndSource source = (HwndSource)HwndSource.FromVisual(visual);
            IntPtr hwnd = source.Handle;

            ScreenToClient(hwnd, ref point);
            return new Point(point.X, point.Y);
        }
        #endregion

        #region GetUIElementSimpleClone
        internal static object GetUIElementSimpleClone(object element)
        {
            if (element == null)
                return null;

       

            // Get all properties and clone them.

            var obj = element;

            // from http://kiwigis.blogspot.jp/2010/06/cloning-path-geometry-in-silverlight.html

            PropertyInfo[] properties = obj.GetType().GetProperties();
            object cloneObj = obj.GetType().GetConstructors()[0].Invoke(null);
            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(obj, null);
                if (value != null)
                {
                    if (IsCollectionInterface(value.GetType()))
                    {
                        object collection = property.GetValue(obj, null);
                        int count = (int)collection.GetType().
                            GetProperty("Count").GetValue(collection, null);
                        for (int i = 0; i < count; i++)
                        {
                            // Get each child of the collection.
                            object child = collection.GetType().
                                GetProperty("Item")?.
                                GetValue(collection, new Object[] { i });
                            if (child != null)
                            {
                                object cloneChild = GetUIElementSimpleClone(child);
                                object cloneCollection = property.
                                    GetValue(cloneObj, null);
                                collection.GetType().
                                    InvokeMember("Add",
                                                 BindingFlags.InvokeMethod,
                                                 null,
                                                 cloneCollection,
                                                 new object[] { cloneChild });
                            }
                        }
                    }
                    // If the property is a UIElement, we also need to clone it.
                    else if (value is UIElement)
                    {
                        if (property.CanWrite)
                        {
                            object obj2 = property.PropertyType.GetConstructors()[0].Invoke(null);
                            obj2 = GetUIElementSimpleClone(value) as UIElement;

                            property.SetValue(cloneObj, obj2, null);
                        }
                    }
                    // For a normal property, its value doesn't need to be
                    // cloned. So just copy its value to the new object.
                    else if (property.CanWrite)
                    {
                        property.SetValue(cloneObj, value, null);
                    }
                }
            }
            return cloneObj;

        }


        private static bool IsCollectionInterface(Type type)
        {
            if (type == typeof(object))
            {
                return false;
            }

            if(type.GetInterface("ICollection") != null)
            {
                return true;
            }

            return IsCollectionInterface(type.BaseType);
        }
        #endregion


        #region IsDragging
        internal static bool IsDragging(Point pointA, Point pointB)
        {
            if (Math.Abs(pointA.X - pointB.X) > SystemParameters.MinimumHorizontalDragDistance) return true;
            if (Math.Abs(pointA.Y - pointB.Y) > SystemParameters.MinimumVerticalDragDistance) return true;
            return false;
        }
        #endregion

        #region IsCenterDragging
        internal static bool IsCenterDragging(Point centerpoint, Point currentpoint, double rangedistance = 0.0)
        {
            if (rangedistance < SystemParameters.MinimumHorizontalDragDistance || rangedistance < SystemParameters.MinimumVerticalDragDistance)
            {
                if (Math.Abs(centerpoint.X - currentpoint.X) > SystemParameters.MinimumHorizontalDragDistance) return true;
                if (Math.Abs(centerpoint.Y - currentpoint.Y) > SystemParameters.MinimumVerticalDragDistance) return true;
            }
            else
            {
                if (Math.Abs(centerpoint.X - currentpoint.X) < rangedistance) return true;
                if (Math.Abs(centerpoint.Y - currentpoint.Y) < rangedistance) return true;
            }
            return false;
        }
        #endregion

        #region IsCorrectType
        internal static bool IsCorrectType(Type checktype, Type correcttype)
        {
            if (checktype == null || correcttype == null) return false;

            if (!checktype.Equals(correcttype)) return IsCorrectType(checktype.BaseType, correcttype);

            else return true;
        }
        #endregion

    }
}
