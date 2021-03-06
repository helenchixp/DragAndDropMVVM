﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;

namespace DragAndDropMVVM.Controls
{
    public class DrawLineThump : ConnectionLineBase, ILinePosition
    {
        private Line _drawLine;

        static DrawLineThump()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DrawLineThump), new FrameworkPropertyMetadata(typeof(DrawLineThump)));
        }


        public DrawLineThump()
        {
            Focusable = true;
            IsHitTestVisible = true;
        }

        #region Override Methods

        #region OnApplyTemplate
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _drawLine = this.Template.FindName("PART_DrawLine", this) as Line;

            if(_drawLine!=null)
            {
                var angle = Math.Atan2(Y1 - Y2, X1 - X2) *180d / Math.PI;
                Angle = angle < 0 ? angle + 360 : angle;
            }
        }
        #endregion

        #region ResetPosition [Custom Virtual Method]


        #endregion
        #endregion

        #region Dependence Properties

        #region X1

        /// <summary>
        /// The <see cref="X1" /> dependency property's name.
        /// </summary>
        public const string X1PropertyName = "X1";

        /// <summary>
        /// Gets or sets the value of the <see cref="X1" />
        /// property. This is a dependency property.
        /// </summary>
        public double X1
        {
            get
            {
                return (double)GetValue(X1Property);
            }
            set
            {
                SetValue(X1Property, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="X1" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty X1Property = DependencyProperty.Register(
            X1PropertyName,
            typeof(double),
            typeof(DrawLineThump),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion

        #region Y1

        /// <summary>
            /// The <see cref="Y1" /> dependency property's name.
            /// </summary>
        public const string Y1PropertyName = "Y1";

        /// <summary>
        /// Gets or sets the value of the <see cref="Y1" />
        /// property. This is a dependency property.
        /// </summary>
        public double Y1
        {
            get
            {
                return (double)GetValue(Y1Property);
            }
            set
            {
                SetValue(Y1Property, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="Y1" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty Y1Property = DependencyProperty.Register(
            Y1PropertyName,
            typeof(double),
            typeof(DrawLineThump),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion

        #region X2

        /// <summary>
            /// The <see cref="X2" /> dependency property's name.
            /// </summary>
        public const string X2PropertyName = "X2";

        /// <summary>
        /// Gets or sets the value of the <see cref="X2" />
        /// property. This is a dependency property.
        /// </summary>
        public double X2
        {
            get
            {
                return (double)GetValue(X2Property);
            }
            set
            {
                SetValue(X2Property, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="X2" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty X2Property = DependencyProperty.Register(
            X2PropertyName,
            typeof(double),
            typeof(DrawLineThump),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion

        #region Y2
        /// <summary>
            /// The <see cref="Y2" /> dependency property's name.
            /// </summary>
        public const string Y2PropertyName = "Y2";

        /// <summary>
        /// Gets or sets the value of the <see cref="Y2" />
        /// property. This is a dependency property.
        /// </summary>
        public double Y2
        {
            get
            {
                return (double)GetValue(Y2Property);
            }
            set
            {
                SetValue(Y2Property, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="Y2" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty Y2Property = DependencyProperty.Register(
            Y2PropertyName,
            typeof(double),
            typeof(DrawLineThump),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion

        #region Angle

        /// <summary>
        /// The <see cref="Angle" /> dependency property's name.
        /// </summary>
        public const string AnglePropertyName = "Angle";

        /// <summary>
        /// Gets or sets the value of the <see cref="Angle" />
        /// property. This is a dependency property.
        /// </summary>
        public double Angle
        {
            get
            {
                return (double)GetValue(AngleProperty);
            }
            set
            {
                SetValue(AngleProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="Angle" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
            AnglePropertyName,
            typeof(double),
            typeof(DrawLineThump),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion

        #endregion

    }
}
