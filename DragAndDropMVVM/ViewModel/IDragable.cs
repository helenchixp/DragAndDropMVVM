using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DragAndDropMVVM.ViewModel
{
    [Obsolete]
    public interface IDragable
    {
        /// <summary>
        /// Type of the data item
        /// </summary>
        Type DataType { get; }

        /// <summary>
        /// Remove the object from the collection
        /// </summary>
       // void Drag(object i);

       //ICommand DragCommand { get; }
    }
}
