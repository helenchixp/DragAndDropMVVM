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

        ObservableCollection<IConnectionLineViewModel> DepartureLinesViewModel { get; set; }

        ObservableCollection<IConnectionLineViewModel> ArrivalLinesViewModel { get; set; }

        bool IsSelected { get; set; }

        int Index { get; set; }
    }
}
