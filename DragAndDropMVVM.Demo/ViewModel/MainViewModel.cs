using System;
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

        private string _lastName = "Niki";

        /// <summary>
        /// Sets and gets the LastName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string LastName
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



        private RelayCommand<string> _dragCommand;

        /// <summary>
        /// Gets the DragCommand.
        /// </summary>
        public RelayCommand<string> DragCommand
        {
            get
            {
                return _dragCommand ?? (_dragCommand = new RelayCommand<string>(
                    ExecuteDragCommand,
                    CanExecuteDragCommand));
            }
        }

        private void ExecuteDragCommand(string parameter)
        {

        }

        private bool CanExecuteDragCommand(string parameter)
        {
            return true;
        }


        private RelayCommand<string> _dropCommand;

        /// <summary>
        /// Gets the DropCommand.
        /// </summary>
        public ICommand DropCommand
        {
            get
            {
                return _dropCommand ?? (_dropCommand = new RelayCommand<string>(
                    ExecuteDropCommand,
                    CanExecuteDropCommand));
            }
        }

        private void ExecuteDropCommand(string parameter)
        {

        }

        private bool CanExecuteDropCommand(string parameter)
        {
            return true;
        }

       

    }

}