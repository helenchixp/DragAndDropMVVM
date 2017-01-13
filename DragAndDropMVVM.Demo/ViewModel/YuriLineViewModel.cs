using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragAndDropMVVM.ViewModel;
using GalaSoft.MvvmLight;

namespace DragAndDropMVVM.Demo.ViewModel
{
    public class YuriLineViewModel : ViewModelBase, IConnectionLineViewModel
    {
        public bool IsSelected
        {
            get;

            set;
        }

        public string LineID
        {
            get;

            set;
        }

        public IConnectionDiagramViewModel OriginDiagramViewModel
        {
            get;

            set;
        }

        public IConnectionDiagramViewModel TerminalDiagramViewModel
        {
            get;

            set;
        }
    }
}
