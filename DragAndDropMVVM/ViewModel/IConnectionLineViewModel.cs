using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragAndDropMVVM.ViewModel
{
    public interface IConnectionLineViewModel
    {
        string LineID { get; set; }

        bool IsSelected { get; set; }

        IConnectionDiagramViewModel StartDiagramViewModel { get; set; }

        IConnectionDiagramViewModel EndDiagramViewModel { get; set; }
    }
}
