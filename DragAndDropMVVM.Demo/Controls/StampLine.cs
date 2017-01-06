using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragAndDropMVVM.Controls;
using DragAndDropMVVM.Demo.ViewModel;

namespace DragAndDropMVVM.Demo.Controls
{
    public class StampLine : DrawLineThump
    {
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            DataContext = new StampLineViewModel();
        }
    }
}
