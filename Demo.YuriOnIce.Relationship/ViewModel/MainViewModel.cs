using System;
using System.Collections.ObjectModel;
using Demo.YuriOnIce.Relationship.Model;
using DragAndDropMVVM.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Demo.YuriOnIce.Relationship.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase, IDragged
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            PalletItems.Add(new DiagramModel()
            {
                ImagePath = "/Demo.YuriOnIce.Relationship.MvvmLight;component/ImagesResource/Yuri.png",
                Title = "Yuri",
                Detail = "Apologize by Japanese Style",
                Index = 1,
            });
            PalletItems.Add(new DiagramModel()
            {
                ImagePath = "/Demo.YuriOnIce.Relationship.MvvmLight;component/ImagesResource/Victory.png",
                Title = "Victory",
                Detail = "Amazing!!!",
                Index = 2,
            });
            PalletItems.Add(new DiagramModel()
            {
                ImagePath = "/Demo.YuriOnIce.Relationship.MvvmLight;component/ImagesResource/Maccachin.png",
                Title = "Maccachin",
                Detail = "Maccachin is dog.",
                Index = 3,
            });
        }

        #region IDragged Objects
        public object DraggedData
        {
            get;

            set;
        }

        #endregion


        #region PropertyChanged Properties
        /// <summary>
        /// The <see cref="PalletItems" /> property's name.
        /// </summary>
        public const string PalletItemsPropertyName = "PalletItems";

        private ObservableCollection<DiagramModel> _palletItems = new ObservableCollection<DiagramModel>();

        /// <summary>
        /// Sets and gets the PalletItems property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ObservableCollection<DiagramModel> PalletItems
        {
            get
            {
                return _palletItems;
            }

            set
            {
                if (_palletItems == value)
                {
                    return;
                }

                var oldValue = _palletItems;
                _palletItems = value;
                RaisePropertyChanged(PalletItemsPropertyName, oldValue, value, true);
            }
        }
        #endregion



        #region Commands

        #region DragCommand

        private RelayCommand<object> _dragCommand;

        /// <summary>
        /// Gets the DragCommand.
        /// </summary>
        public RelayCommand<object> DragCommand
        {
            get
            {
                return _dragCommand ?? (_dragCommand = new RelayCommand<object>(
                    ExecuteDragCommand,
                    CanExecuteDragCommand));
            }
        }

        private void ExecuteDragCommand(object parameter)
        {

        }

        private bool CanExecuteDragCommand(object parameter)
        {
            return true;
        }

        #endregion

        #region DropCommand


        private RelayCommand<object> _dropCommand;

        /// <summary>
        /// Gets the DropCommand.
        /// </summary>
        public RelayCommand<object> DropCommand
        {
            get
            {
                return _dropCommand ?? (_dropCommand = new RelayCommand<object>(
                    ExecuteDropCommand,
                    CanExecuteDropCommand));
            }
        }

        private void ExecuteDropCommand(object parameter)
        {

            if (parameter is DiagramViewModel)
            {
                if (DraggedData != null && DraggedData is DiagramModel)
                {
                    (parameter as DiagramViewModel).Title = (DraggedData as DiagramModel).Title;
                    (parameter as DiagramViewModel).Detail = (DraggedData as DiagramModel).Detail;
                    (parameter as DiagramViewModel).ImagePath = (DraggedData as DiagramModel).ImagePath;
                    (parameter as DiagramViewModel).Index = (DraggedData as DiagramModel).Index;
                }
            }
        }

        private bool CanExecuteDropCommand(object parameter)
        {
            return true;
        }
        #endregion

        #region DragLineCommand

        private RelayCommand<object> _dragLineCommand;

        /// <summary>
        /// Gets the DragLineCommand.
        /// </summary>
        public RelayCommand<object> DragLineCommand
        {
            get
            {
                return _dragLineCommand ?? (_dragLineCommand = new RelayCommand<object>(
                    ExecuteDragLineCommand,
                    CanExecuteDragLineCommand));
            }
        }

        private void ExecuteDragLineCommand(object parameter)
        {

        }

        private bool CanExecuteDragLineCommand(object parameter)
        {
            return true;
        }

        #endregion

        #region DropLineCommand
        private RelayCommand<object> _dropLineCommand;

        /// <summary>
        /// Gets the DropLineCommand.
        /// </summary>
        public RelayCommand<object> DropLineCommand
        {
            get
            {
                return  _dropLineCommand ?? ( _dropLineCommand = new RelayCommand<object>(
                    ExecuteDropLineCommand,
                    CanExecuteDropLineCommand));
            }
        }

        private void ExecuteDropLineCommand(object parameter)
        {
            if (parameter == null || !(parameter is IConnectionLineViewModel)) return;

            var linevm = (parameter as IConnectionLineViewModel);

            linevm.OriginDiagramViewModel.DepartureLinesViewModel.Add(linevm);
            linevm.TerminalDiagramViewModel.ArrivalLinesViewModel.Add(linevm);

        }

        private bool CanExecuteDropLineCommand(object parameter)
        {
            if (parameter == null || !(parameter is IConnectionLineViewModel)) return false;
            var dragobj = (parameter as IConnectionLineViewModel).OriginDiagramViewModel;
            var dropobj = (parameter as IConnectionLineViewModel).TerminalDiagramViewModel;

            if (dragobj == dropobj) return false;

            if (dropobj == null || dragobj == null) return false;

            if (dragobj.ArrivalLinesViewModel != null)
            {
                foreach (var aline in dragobj.ArrivalLinesViewModel)
                {
                    if (dropobj.Equals(aline.TerminalDiagramViewModel))
                    {
                        return false;
                    }

                }
            }

            if (dropobj.DepartureLinesViewModel != null)
            {
                foreach (var dline in dropobj.DepartureLinesViewModel)
                {
                    if (dragobj.Equals(dline.OriginDiagramViewModel))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion


        #region DeleteLineCommand


        private RelayCommand<object> _deleteLineCommand;

        /// <summary>
        /// Gets the DeleteLineCommand.
        /// </summary>
        public RelayCommand<object> DeleteLineCommand
        {
            get
            {
                return _deleteLineCommand ?? (_deleteLineCommand = new RelayCommand<object>(
                    ExecuteDeleteLineCommand,
                    CanExecuteDeleteLineCommand));
            }
        }

        private void ExecuteDeleteLineCommand(object parameter)
        {
            if (parameter == null || !(parameter is IConnectionLineViewModel)) return;

            var linevm = parameter as IConnectionLineViewModel;

            linevm.OriginDiagramViewModel.DepartureLinesViewModel.Remove(linevm);
            linevm.TerminalDiagramViewModel.ArrivalLinesViewModel.Remove(linevm);
        }

        private bool CanExecuteDeleteLineCommand(object parameter)
        {
            return true;
        }
        #endregion


        #region DeleteDiagramCommand

        private RelayCommand<object> _deleteDiagramCommand;

        /// <summary>
        /// Gets the DeleteDiagramCommand.
        /// </summary>
        public RelayCommand<object> DeleteDiagramCommand
        {
            get
            {
                return _deleteDiagramCommand ?? (_deleteDiagramCommand = new RelayCommand<object>(
                    ExecuteDeleteDiagramCommand,
                    CanExecuteDeleteDiagramCommand));
            }
        }

        private void ExecuteDeleteDiagramCommand(object parameter)
        {

        }

        private bool CanExecuteDeleteDiagramCommand(object parameter)
        {
            if (parameter == null) return true;

            if (parameter is DiagramViewModel)
            {
                return (parameter as DiagramViewModel).ArrivalLinesViewModel.Count == 0
                     &&
                      (parameter as DiagramViewModel).DepartureLinesViewModel.Count == 0;
            }
            return true;
        }
        #endregion

        #endregion

    }
}