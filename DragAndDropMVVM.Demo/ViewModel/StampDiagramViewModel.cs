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


        public ObservableCollection<IConnectionDiagramViewModel> ConnectingDiagramsDataContext
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


        ////private RelayCommand<object> _dragLineCommand;

        /////// <summary>
        /////// Gets the DragLineCommand.
        /////// </summary>
        ////public RelayCommand<object> DragLineCommand
        ////{
        ////    get
        ////    {
        ////        return _dragLineCommand ?? (_dragLineCommand = new RelayCommand<object>(
        ////            ExecuteDragLineCommand,
        ////            CanExecuteDragLineCommand));
        ////    }
        ////}

        ////private void ExecuteDragLineCommand(object parameter)
        ////{
        ////    System.Diagnostics.Debug.WriteLine($"The ExecuteDragLineCommand is {((parameter as StampDiagramViewModel)?.Index)}");
        ////}

        ////private bool CanExecuteDragLineCommand(object parameter)
        ////{
        ////    return true;
        ////}


        ////private RelayCommand<object> _dropLineCommand;

        /////// <summary>
        /////// Gets the DropLineCommand.
        /////// </summary>
        ////public RelayCommand<object> DropLineCommand
        ////{
        ////    get
        ////    {
        ////        return _dropLineCommand ?? (_dropLineCommand = new RelayCommand<object>(
        ////            ExecuteDropLineCommand,
        ////            CanExecuteDropLineCommand));
        ////    }
        ////}

        ////private void ExecuteDropLineCommand(object parameter)
        ////{
        ////    System.Diagnostics.Debug.WriteLine($"The ExecuteDropLineCommand is {((parameter as StampDiagramViewModel)?.Index)}");
        ////}

        ////private bool CanExecuteDropLineCommand(object parameter)
        ////{
        ////    return true;
        ////}
    }
}