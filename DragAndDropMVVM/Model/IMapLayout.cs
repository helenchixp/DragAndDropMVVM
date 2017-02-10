using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragAndDropMVVM.Model
{
    public interface IMapLayout
    {
        int Width { get; set; }
        int Height { get; set; }
        
        IDiagramLayout[] Diagrams { get;  }

      //  Action GetDiagramsPosition { get; set; }
    }
}
