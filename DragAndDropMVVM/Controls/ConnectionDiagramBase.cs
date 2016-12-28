using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DragAndDropMVVM.Controls
{
    public abstract class ConnectionDiagramBase : ContentControl
    {


        public virtual Point CenterPosition
        {
            get;set;
        }
    }
}
