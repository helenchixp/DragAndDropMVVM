using System;
using System.Collections.ObjectModel;
using DragAndDropMVVM.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DragAndDropMVVM.Demo.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class StampDiagramViewModel : ViewModelBase, IConnectionDiagramViewModel
    {
        /// <summary>
        /// Initializes a new instance of the StampDiagramViewModel class.
        /// </summary>
        public StampDiagramViewModel()
        {
        }

        public string DiagramID { get; set; }


        public IConnectionDiagramViewModel DragFromDiagramViewModel
        {
            get;

            set;
        }

        public ObservableCollection<IConnectionLineViewModel> DepartureLinesViewModel
        {
            get;

            set;
        } = new ObservableCollection<IConnectionLineViewModel>();

        public ObservableCollection<IConnectionLineViewModel> ArrivalLinesViewModel
        {
            get;

            set;
        } = new ObservableCollection<IConnectionLineViewModel>();

        public bool IsSelected { get; set; }


        public int Index { get; set; } = 0;


    }
}