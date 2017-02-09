using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragAndDropMVVM.ViewModel
{
    public interface IConnectionLineViewModel
    {
        /// <summary>
        /// This is Unique ID for Line
        /// </summary>
        string LineUUID { get; set; }

        bool IsSelected { get; set; }

        IConnectionDiagramViewModel OriginDiagramViewModel { get; set; }

        IConnectionDiagramViewModel TerminalDiagramViewModel { get; set; }
    }
}
