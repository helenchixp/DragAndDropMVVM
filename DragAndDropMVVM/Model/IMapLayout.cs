using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragAndDropMVVM.Model
{
    public interface IMapLayout
    {
        double Width { get; set; }
        double Height { get; set; }

        Type DiagramLayoutType { get; }

        IDiagramLayout[] Diagrams { get; set; }

      //  Action GetDiagramsPosition { get; set; }
    }
}
