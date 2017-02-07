using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragAndDropMVVM.ViewModel;
using GalaSoft.MvvmLight;

namespace Demo.YuriOnIce.Relationship.ViewModel
{
    public class LineViewModel : ViewModelBase, IConnectionLineViewModel
    {
        #region IConnectionLineViewModel
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

        #endregion
    }
}
