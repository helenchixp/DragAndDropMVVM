using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragAndDropMVVM.ViewModel
{
    public interface IConnectionDiagramViewModel
    {
        string DiagramID { get; set; }

        ObservableCollection<IConnectionLineViewModel> LinesDataContext { get; set; }

        ObservableCollection<IConnectionDiagramViewModel> ConnectingDiagramsDataContext { get; set; }

        bool IsSelected { get; set; }
    }
}
