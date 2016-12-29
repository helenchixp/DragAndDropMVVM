using System;
using System.Collections.ObjectModel;
using DragAndDropMVVM.ViewModel;
using GalaSoft.MvvmLight;

namespace DragAndDropMVVM.Demo.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class StampDiagramViewModel : ViewModelBase, DragAndDropMVVM.ViewModel.IConnectionDiagramViewModel
    {
        /// <summary>
        /// Initializes a new instance of the StampDiagramViewModel class.
        /// </summary>
        public StampDiagramViewModel()
        {
        }

        public string DiagramID { get; set; }


        public ObservableCollection<IConnectionDiagramViewModel> ConnectingDiagramsDataContext
        {
            get;

            set;
        }

        public ObservableCollection<IConnectionLineViewModel> LinesDataContext
        {
            get;

            set;
        }

        public bool IsSelected { get; set; }
    }
}