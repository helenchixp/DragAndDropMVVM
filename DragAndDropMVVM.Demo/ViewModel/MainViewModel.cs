using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DragAndDropMVVM.Demo.Model;
using DragAndDropMVVM.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DragAndDropMVVM.Demo.ViewModel
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
    public class MainViewModel : ViewModelBase, IDragged//, IDragable, IDropable
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

            YuriPalletItems.Add(new YuriModel()
            {
                ImagePath = "/DragAndDropMVVM.Demo;component/ImagesResource/yuri_01.png",
                Title = "Sorry!",
                Detail = "Apologize by Japanese Style",
                Index = 1,
            });
            YuriPalletItems.Add(new YuriModel()
            {
                ImagePath = "/DragAndDropMVVM.Demo;component/ImagesResource/yuri_02.png",
                Title = "Really?",
                Detail = "I Can't believe it! But it is really!!!",
                Index = 2,
            });
            YuriPalletItems.Add(new YuriModel()
            {
                ImagePath = "/DragAndDropMVVM.Demo;component/ImagesResource/yuri_03.png",
                Title = "Wow!",
                Detail = "I'm Maccachin.",
                Index = 3,
            });
        }

        public ObservableCollection<YuriModel> YuriPalletItems { get; private set; } = new ObservableCollection<YuriModel>();


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
         //   System.Diagnostics.Debug.WriteLine($"The DragCommandParameter is {(parameter ?? "null").ToString()}");
        }

        private bool CanExecuteDragCommand(object parameter)
        {
            return true;
        }


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
            if(parameter is StampDiagramViewModel)
            {
                (parameter as StampDiagramViewModel).Index = DroppedItemSource.Count;
                DroppedItemSource.Add(parameter as StampDiagramViewModel);
            }

            if (parameter is YuriDiagramViewModel)
            {
                if (DraggedData != null && DraggedData is YuriModel)
                {
                    (parameter as YuriDiagramViewModel).Title = (DraggedData as YuriModel).Title;
                    (parameter as YuriDiagramViewModel).Detail = (DraggedData as YuriModel).Detail;
                    (parameter as YuriDiagramViewModel).ImagePath = (DraggedData as YuriModel).ImagePath;
                    (parameter as YuriDiagramViewModel).IconType = (DraggedData as YuriModel).Index;
                }
            }
            ////System.Diagnostics.Debug.WriteLine($"The DropCommandParameter is {(parameter ?? "null").ToString()}");
            //////DroppedItemSource.Add($"{(parameter ?? "null").ToString()}");
            ////int idx = DroppedItemSource.Count;
            ////DroppedItemSource.Add(new StampDiagramViewModel()
            ////{
            ////    Index = idx,
            ////});

        }

        private bool CanExecuteDropCommand(object parameter)
        {
            return true;
        }

        //ObservableCollection

        /// <summary>
        /// The <see cref="DroppedItemSource" /> property's name.
        /// </summary>
        public const string DroppedItemSourcePropertyName = "DroppedItemSource";

        private ObservableCollection<StampDiagramViewModel> _droppedItemSource = new ObservableCollection<StampDiagramViewModel>();

        /// <summary>
        /// Sets and gets the DroppedItemSource property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ObservableCollection<StampDiagramViewModel> DroppedItemSource
        {
            get
            {
                return _droppedItemSource;
            }

            set
            {
                if (_droppedItemSource == value)
                {
                    return;
                }

                var oldValue = _droppedItemSource;
                _droppedItemSource = value;
                RaisePropertyChanged(DroppedItemSourcePropertyName, oldValue, value, true);
            }
        }


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
            System.Diagnostics.Debug.WriteLine($"Drag Diagram ID:{(parameter as StampDiagramViewModel)?.Index}");
        }

        private bool CanExecuteDragLineCommand(object parameter)
        {
            return true;
        }


        private RelayCommand<object> _dropLineCommand;

        /// <summary>
        /// Gets the DropLineCommand.
        /// </summary>
        public RelayCommand<object> DropLineCommand
        {
            get
            {
                return _dropLineCommand ?? (_dropLineCommand = new RelayCommand<object>(
                    ExecuteDropLineCommand,
                    CanExecuteDropLineCommand));
            }
        }

        private void ExecuteDropLineCommand(object parameter)
        {
            System.Diagnostics.Debug.WriteLine($"Drop Diagram ID:{(parameter as StampDiagramViewModel)?.Index}");
        }

        private bool CanExecuteDropLineCommand(object parameter)
        {
            var ddobj = parameter as Tuple<object, object>;

            if (ddobj == null) return false;

            var dragobj = ddobj.Item1 as IConnectionDiagramViewModel;
            var dropobj = ddobj.Item2 as IConnectionDiagramViewModel;

            if (dropobj == null || dragobj == null) return false;

            if (dragobj.ArrivalLinesViewModel != null)
            {
                foreach (var aline in dragobj.ArrivalLinesViewModel)
                {
                    if(dropobj.Equals(aline.TerminalDiagramViewModel))
                    {
                        return false;
                    }

                }
            }

            if(dropobj.DepartureLinesViewModel != null)
            {
                foreach(var dline in dropobj.DepartureLinesViewModel)
                {
                    if(dragobj.Equals(dline.OriginDiagramViewModel))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public object DraggedData
        {
            get;

            set;
        }


    }

}