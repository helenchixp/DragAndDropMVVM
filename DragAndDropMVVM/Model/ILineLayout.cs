using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragAndDropMVVM.Model
{
    public interface ILineLayout
    {
        Type LineUIType { get; set; }

        string TerminalDiagramUUID { get; set; }

        string LineUUID { get; set; }

        object DataContext { get; set; }
    }

 
}
