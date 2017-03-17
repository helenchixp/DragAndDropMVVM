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
    public class DroppingAdorner : Adorner
    {
        private AdornerLayer adornerLayer;

        public DroppingAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            this.adornerLayer = AdornerLayer.GetAdornerLayer(this.AdornedElement);
            this.adornerLayer.Add(this);
        }

        internal void Update()
        {
            this.adornerLayer.Update(this.AdornedElement);
            this.Visibility = System.Windows.Visibility.Visible;
        }

        public void Remove()
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

            SolidColorBrush renderBrush = (SolidColorBrush)Application.Current.Resources["AdornerPanelBrush"];
            renderBrush.Opacity = 0.5;
            Pen renderPen = new Pen((SolidColorBrush)Application.Current.Resources["AdornerPanelBrush"], 1.5);
            double renderRadius = 5.0;

            DroppingElementAdornerType adornertype = DiagramElementDropBehavior.GetAdornerType(this.AdornedElement);

            if (DroppingElementAdornerType.DrawEllipse.Equals(adornertype))
            {
                // Draw a circle at each corner.
                drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
                drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
                drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius);
                drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);
            }
            else if (DroppingElementAdornerType.DrawLine.Equals(adornertype))
            {
                renderPen.DashStyle = DashStyles.DashDot;
                renderPen.Thickness = renderRadius;
                drawingContext.DrawLine(renderPen, adornedElementRect.TopLeft, adornedElementRect.TopRight);
                drawingContext.DrawLine(renderPen, adornedElementRect.TopLeft, adornedElementRect.BottomLeft);
                drawingContext.DrawLine(renderPen, adornedElementRect.TopRight, adornedElementRect.BottomRight);
                drawingContext.DrawLine(renderPen, adornedElementRect.BottomLeft, adornedElementRect.BottomRight);
            }
            else
            {
               
                //get the custom adorner from drop element
                UIElement uc = DiagramElementDropBehavior.GetCustomAdornerContent(this.AdornedElement);

                // Create the visual brush based on the UserControl
                VisualBrush vb = new VisualBrush(uc);

                //get the alignment
                HorizontalAlignment horizon = (HorizontalAlignment)uc.GetValue(HorizontalAlignmentProperty);
                VerticalAlignment vertical = (VerticalAlignment)uc.GetValue(VerticalAlignmentProperty);

            
                if(horizon == HorizontalAlignment.Left)
                {
                    drawingContext.DrawRectangle(vb, null, new Rect(0, 0,renderRadius, ActualHeight));
                }
                else if (horizon == HorizontalAlignment.Right)
                {
                    drawingContext.DrawRectangle(vb, null, new Rect(ActualWidth, 0, renderRadius, ActualHeight));
                }
                else if (vertical == VerticalAlignment.Center)
                {
                    drawingContext.DrawRectangle(vb, null, new Rect(ActualWidth / 2, 0, renderRadius, ActualHeight));
                }

                if (vertical == VerticalAlignment.Bottom)
                {
                    drawingContext.DrawRectangle(vb, null, new Rect(0, ActualHeight, ActualWidth, renderRadius));
                }
                else if (vertical == VerticalAlignment.Top)
                {
                    drawingContext.DrawRectangle(vb, null, new Rect(0, 0, ActualWidth, renderRadius));
                }
                else if (vertical == VerticalAlignment.Center)
                {
                    drawingContext.DrawRectangle(vb, null, new Rect(0, ActualWidth / 2, ActualWidth, renderRadius));
                }

                if(vertical == VerticalAlignment.Stretch && horizon == HorizontalAlignment.Stretch)
                {
                    drawingContext.DrawRectangle(vb, null, new Rect(0, 0, renderRadius, ActualHeight));
                    drawingContext.DrawRectangle(vb, null, new Rect(ActualWidth - renderRadius, 0, renderRadius, ActualHeight));
                    drawingContext.DrawRectangle(vb, null, new Rect(0, ActualHeight - renderRadius, ActualWidth, renderRadius));
                    drawingContext.DrawRectangle(vb, null, new Rect(0, 0, ActualWidth, renderRadius));
                }

                //// Draw using the visual brush in the rect at 10,10,100,25





                ////var grayBrush = renderBrush;

                ////left
                ////drawingContext.DrawRectangle(grayBrush, null, new System.Windows.Rect(1, 1, 5, ActualHeight));
                ////right
                ////drawingContext.DrawRectangle(grayBrush, null, new System.Windows.Rect(ActualWidth - 6, 1, 5, ActualHeight));
                ////bottom
                ////drawingContext.DrawRectangle(grayBrush, null, new System.Windows.Rect(1, ActualHeight, ActualWidth - 2, 5));

            }
        }

    }
}
