using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragAndDropMVVM.Model
{
    public interface IDiagramLayout
    {
        double X { get; set; }
        double Y { get; set; }
        Type DiagramUIType { get; set; }
        string DiagramUUID { get; set; }
        ILineLayout[] DepartureLines { get; set; }

        object DataContext { get; set; }
    }
}
