using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragAndDropMVVM.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DragAndDropMVVM.Demo.ViewModel
{
    class StampLineViewModel : ViewModelBase, IConnectionLineViewModel
    {
        public bool IsSelected
        {
            get;

            set;
        }

        public string LineUUID
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
