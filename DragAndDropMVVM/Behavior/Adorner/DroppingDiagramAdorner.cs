using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace DragAndDropMVVM.Behavior
{
    public class DroppingDiagramAdorner : Adorner
    {
        private AdornerLayer adornerLayer;

        public DroppingDiagramAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            this.adornerLayer = AdornerLayer.GetAdornerLayer(this.AdornedElement);
            this.adornerLayer.Add(this);
        }

        internal void Update()
        {
            if (this.adornerLayer.GetAdorners(this.AdornedElement) != null)
            {
                this.adornerLayer.Update(this.AdornedElement);
            }
            else
            {
                this.adornerLayer = AdornerLayer.GetAdornerLayer(this.AdornedElement);
                this.adornerLayer.Add(this);
            }
            this.Visibility = System.Windows.Visibility.Visible;
        }

        public void Remove()
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

            SolidColorBrush renderBrush = (SolidColorBrush)Application.Current.Resources["AdornerDiagramBrush"];
            renderBrush.Opacity = 0.5;

            DroppingElementAdornerType adornertype = (this.AdornedElement as Controls.ConnectionDiagramBase)?.AdornerType ?? DroppingElementAdornerType.DrawEllipse;
            Pen renderPen = new Pen((SolidColorBrush)Application.Current.Resources["AdornerDiagramBrush"], 1.5);
            double renderRadius = 5.0;

            if (DroppingElementAdornerType.DrawEllipse.Equals(adornertype))
            {
                // Draw a circle at each corner.
                drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
                drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
                drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius);
                drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);
            }
            else if(DroppingElementAdornerType.DrawLine.Equals(adornertype))
            {
                renderPen.DashStyle = DashStyles.Dash;
                renderPen.Thickness = 6.0;
                drawingContext.DrawLine(renderPen, adornedElementRect.TopLeft, adornedElementRect.TopRight);
                drawingContext.DrawLine(renderPen, adornedElementRect.TopLeft, adornedElementRect.BottomLeft);
                drawingContext.DrawLine(renderPen, adornedElementRect.TopRight, adornedElementRect.BottomRight);
                drawingContext.DrawLine(renderPen, adornedElementRect.BottomLeft, adornedElementRect.BottomRight);
            }
            else
            {
                //TODO:Extention
            }
        }
    }
}
