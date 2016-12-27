using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
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
    public class MainViewModel : ViewModelBase, IDragable, IDropable
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
        }

        Type IDragable.DataType
        {
            get
            {
                return typeof(string);
            }
        }

        Type IDropable.DataType
        {
            get
            {
                return typeof(string);
            }
        }

        public void Drag(object i)
        {
            //throw new NotImplementedException();
        }

        //void IDropable.Drop(object data, int index)
        //{
        //    //throw new NotImplementedException();

        //    System.Diagnostics.Debug.WriteLine($"{nameof(IDropable.Drop)}.{nameof(IDropable.DataType)} : {(data ?? "null").ToString() }" );

        //}


        /// <summary>
            /// The <see cref="FirstName" /> property's name.
            /// </summary>
        public const string FirstNamePropertyName = "FirstName";

        private string _firstName = "Victory";

        /// <summary>
        /// Sets and gets the FirstName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string FirstName
        {
            get
            {
                return _firstName;
            }

            set
            {
                if (_firstName == value)
                {
                    return;
                }

                var oldValue = _firstName;
                _firstName = value;
                RaisePropertyChanged(FirstNamePropertyName, oldValue, value, true);
            }
        }


        /// <summary>
            /// The <see cref="LastName" /> property's name.
            /// </summary>
        public const string LastNamePropertyName = "LastName";

        private object _lastName = new decimal(3.8);

        /// <summary>
        /// Sets and gets the LastName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public object LastName
        {
            get
            {
                return _lastName;
            }

            set
            {
                if (_lastName == value)
                {
                    return;
                }

                var oldValue = _lastName;
                _lastName = value;
                RaisePropertyChanged(LastNamePropertyName, oldValue, value, true);
            }
        }



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
            System.Diagnostics.Debug.WriteLine($"The DragCommandParameter is {(parameter ?? "null").ToString()}");
        }

        private bool CanExecuteDragCommand(object parameter)
        {
            return true;
        }


        private RelayCommand<object> _dropCommand;

        /// <summary>
        /// Gets the DropCommand.
        /// </summary>
        public ICommand DropCommand
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
            System.Diagnostics.Debug.WriteLine($"The DropCommandParameter is {(parameter ?? "null").ToString()}");
            DroppedItemSource.Add($"{(parameter ?? "null").ToString()}");
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

        private ObservableCollection<string> _droppedItemSource = new ObservableCollection<string>();

        /// <summary>
        /// Sets and gets the DroppedItemSource property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ObservableCollection<string> DroppedItemSource
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
    }

}