using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using DragAndDropMVVM.Controls;
using DragAndDropMVVM.Model;
using DragAndDropMVVM.ViewModel;
using Microsoft.Win32;

namespace DragAndDropMVVM.Extensions
{
    public static class ExtenstionsFunction
    {
        #region Export Image
        public static void ExportImage(this Canvas canvas)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FilterIndex = 1;

            //Extend the image type
            saveFileDialog.Filter = "Png Image(*.png)|*.png|Jpeg Image(.jpg)|*.jpg|All Files (*.*)|*.*";
            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                var path = saveFileDialog.FileName;

                var fileext = saveFileDialog.SafeFileName.Split('.');


                // recalculate this canvas
                var size = new Size(double.NaN.Equals(canvas.Width) ? canvas.ActualWidth : canvas.Width,
                     double.NaN.Equals(canvas.Height) ? canvas.ActualHeight : canvas.Height);
                canvas.Measure(size);
                canvas.Arrange(new Rect(size));

                // convert VisualObject to Bitmap
                var renderBitmap = new RenderTargetBitmap((int)size.Width,       // width
                                                          (int)size.Height,      // Height
                                                          96.0d,                 // Horizonal 96.0DPI
                                                          96.0d,                 // Vertual 96.0DPI
                                                          PixelFormats.Pbgra32); // 32bit(RGBA 8bit)
                renderBitmap.Render(canvas);

                // Default encoder is PNG
                BitmapEncoder encoder = new PngBitmapEncoder(); ;
                if (fileext.Length >= 2 && fileext[fileext.Length - 1] == "jpg")
                {
                    encoder = new JpegBitmapEncoder();
                }


                // Output FileStream 
                using (var os = new FileStream(path, FileMode.Create))
                {
                    // Create the Bitmap FileStream
                    encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                    encoder.Save(os);
                }
            }

        }

        #endregion

        public static void SetExportPosition(this Canvas canvas, IDiagramLayout[] diagrams)
        {
            if (diagrams == null || !diagrams.Any()) return;

            foreach(var element in canvas.Children)
            {
                if(element is ConnectionDiagramBase)
                {
                    var diagram = diagrams.FirstOrDefault(dia => (element as ConnectionDiagramBase).DiagramUUID == dia.DiagramUUID);

                    if(diagram != null)
                    {
                        diagram.X = Canvas.GetLeft(element as ConnectionDiagramBase);
                        diagram.Y = Canvas.GetTop(element as ConnectionDiagramBase);
                        diagram.DiagramUUID = (element as ConnectionDiagramBase).DiagramUUID;
                    }
                }
            }

        }


        internal static void LoadLayout(this Canvas canvas, IDiagramLayout[] diagrams)
        {

            if (diagrams == null || !diagrams.Any()) return;
            canvas.Children.Clear();

            Dictionary<string, ILineLayout[]> uuidLines = new Dictionary<string, ILineLayout[]>();

            foreach (var diagram in diagrams)
            {
                var clnele = Activator.CreateInstance(diagram.DiagramType) as UIElement;
                //clnele.SetValue(ConnectionDiagramBase.DiagramUUIDProperty, diagram.DiagramUUID);

                if (clnele is ContentControl)
                {
                    (clnele as ContentControl).DataContext = diagram.DataContext;
                }

                //add the line by Diagram UUID after finish all diagrams
                uuidLines.Add(diagram.DiagramUUID, diagram.DepartureLines);

                Application.Current.Dispatcher.Invoke(() =>
                {

                    Canvas.SetRight(clnele, diagram.X);
                    Canvas.SetLeft(clnele, diagram.X);
                    Canvas.SetBottom(clnele, diagram.Y);
                    Canvas.SetTop(clnele, diagram.Y);


                    canvas.Children.Add(clnele);
                });
            }

            //update the canvas.
            canvas.UpdateLayout();


            foreach (var dialines in uuidLines)
            {
                ConnectionDiagramBase origindiagram = GetDiagramByUUID(canvas, dialines.Key);
                if (origindiagram != null && dialines.Value != null)
                {
                    foreach (var defline in dialines.Value)
                    {
                        var terminaldiagram = GetDiagramByUUID(canvas, defline.TerminalDiagramUUID);

                        dynamic conline;

                        if (terminaldiagram != null)
                        {
                            conline = Activator.CreateInstance(defline.LineType);
                            if (conline is ConnectionLineBase)
                            {

                                (conline as ConnectionLineBase).OriginDiagram = origindiagram;
                                (conline as ConnectionLineBase).TerminalDiagram = terminaldiagram;
                                (conline as ConnectionLineBase).DataContext = defline.DataContext;
                                (conline as ConnectionLineBase).LineUUID = string.IsNullOrWhiteSpace(defline.LineUUID) ? $"{conline.GetType().Name}_{Guid.NewGuid().ToString()}" : defline.LineUUID;
                                //if inherb
                                if (conline is ILinePosition)
                                {
                                    (conline as ILinePosition).X1 = (double)origindiagram.GetValue(Canvas.LeftProperty) - (double)terminaldiagram.GetValue(Canvas.LeftProperty) + origindiagram.CenterPosition.X;
                                    (conline as ILinePosition).Y1 = (double)origindiagram.GetValue(Canvas.TopProperty) - (double)terminaldiagram.GetValue(Canvas.TopProperty) + origindiagram.CenterPosition.Y; ;
                                    (conline as ILinePosition).X2 = terminaldiagram.CenterPosition.X;
                                    (conline as ILinePosition).Y2 = terminaldiagram.CenterPosition.Y;

                                }

                                origindiagram.DepartureLines.Add(conline);
                                terminaldiagram.ArrivalLines.Add(conline);

                            }
                            Application.Current.Dispatcher.Invoke(() =>
                            {

                                Canvas.SetTop(conline, (double)terminaldiagram.GetValue(Canvas.TopProperty));
                                Canvas.SetLeft(conline, (double)terminaldiagram.GetValue(Canvas.LeftProperty));

                                canvas.Children.Add(conline);
                            });
                        }
                    }
                }
            }

            //////foreach (var child in canvas.Children)
            //////{
            //////    if (!(child is ConnectionDiagramBase)) continue;

            //////    var origindiagram = child as ConnectionDiagramBase;

            //////    foreach (var child2 in canvas.Children)
            //////    {
            //////        if (!(child2 is ConnectionDiagramBase)) continue;

            //////        var terminaldiagram = child2 as ConnectionDiagramBase;



            //////    }
            //////}

        }

        private static ConnectionDiagramBase GetDiagramByUUID(Canvas canvas , string uuid)
        {
            ConnectionDiagramBase result = null;
            foreach (var child in canvas.Children)
            {
                if (!(child is ConnectionDiagramBase)) continue;

                if((child as ConnectionDiagramBase).DiagramUUID == uuid)
                {
                    result = (child as ConnectionDiagramBase);
                    break;
                }
            }

            return result;
        }

    }
}
