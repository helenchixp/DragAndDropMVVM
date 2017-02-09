using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using DragAndDropMVVM.Controls;
using DragAndDropMVVM.Model;
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
                        diagram.X = Canvas.GetTop(element as ConnectionDiagramBase);
                        diagram.Y = Canvas.GetLeft(element as ConnectionDiagramBase);
                    }
                }
            }

        }

    }
}
