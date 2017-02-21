using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragAndDropMVVM
{

    public class UndoRedoManager
    {
        public string ActionComment { get; set; }
        public Action<object> UndoAction { get; set; }
        public Action<object> RedoAction { get; set; }
    }
}
