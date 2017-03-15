using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Demo.Mr.Osomatsu.ViewModel
{
    public class ListViewModel : ViewModelBase
    {

        private int? _draggedIndex = null;

        public ListViewModel()
        {

            for (var i = 1; i <= 6; i++)
            {
                _beforeItems.Add(new ProfileModel()
                {
                    Name = Const.BrotherNames[i - 1],
                    No = i,
                    //Comment = $"It is normal No.{i} Brother",
                    ImagePath = $"/Demo.Mr.Osomatsu;component/ImagesResource/m{i}_01.png",
                });
                _afterItems.Add(new ProfileModel()
                {
                    Name = Const.BrotherNames[i - 1],
                    No = i,
                    //Comment = $"It is handsome No.{i} Brother",
                    ImagePath = $"/Demo.Mr.Osomatsu;component/ImagesResource/m{i}_04.png",
                });
            }
        }

        #region ObservableCollection
        /// <summary>
        /// The <see cref="BeforeItems" /> property's name.
        /// </summary>
        public const string LeftItemsPropertyName = "BeforeItems";

        private ObservableCollection<ProfileModel> _beforeItems = new ObservableCollection<ProfileModel>();

        /// <summary>
        /// Sets and gets the LeftItems property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ObservableCollection<ProfileModel> BeforeItems
        {
            get
            {
                return _beforeItems;
            }

            set
            {
                if (_beforeItems == value)
                {
                    return;
                }

                var oldValue = _beforeItems;
                _beforeItems = value;
                RaisePropertyChanged(LeftItemsPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// The <see cref="RightItems" /> property's name.
        /// </summary>
        public const string AfterItemsPropertyName = "AfterItems";

        private ObservableCollection<ProfileModel> _afterItems = new ObservableCollection<ProfileModel>();

        /// <summary>
        /// Sets and gets the AfterItems property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ObservableCollection<ProfileModel> AfterItems
        {
            get
            {
                return _afterItems;
            }

            set
            {
                if (_afterItems == value)
                {
                    return;
                }

                var oldValue = _afterItems;
                _afterItems = value;
                RaisePropertyChanged(AfterItemsPropertyName, oldValue, value, true);
            }
        }

        ///////// <summary>
        //////    /// The <see cref="ConnectionCollection" /> property's name.
        //////    /// </summary>
        //////public const string ConnectionCollectionPropertyName = "ConnectionCollection";

        //////private ObservableCollection<Tuple<int,int>> _connectionCollection = new ObservableCollection<Tuple<int, int>>();

        ///////// <summary>
        ///////// Sets and gets the ConnectionCollection property.
        ///////// Changes to that property's value raise the PropertyChanged event. 
        ///////// This property's value is broadcasted by the MessengerInstance when it changes.
        ///////// </summary>
        //////public ObservableCollection<Tuple<int,int>> ConnectionCollection
        //////{
        //////    get
        //////    {
        //////        return _connectionCollection;
        //////    }

        //////    set
        //////    {
        //////        if (_connectionCollection == value)
        //////        {
        //////            return;
        //////        }

        //////        var oldValue = _connectionCollection;
        //////        _connectionCollection = value;
        //////        RaisePropertyChanged(ConnectionCollectionPropertyName, oldValue, value, true);
        //////    }
        //////}
        /// <summary>
        /// The <see cref="ConnectionCollection" /> property's name.
        /// </summary>
        public const string ConnectionCollectionPropertyName = "ConnectionCollection";

        private ObservableCollection<ListConnectionModel> _connectionCollection = new ObservableCollection<ListConnectionModel>();

        /// <summary>
        /// Sets and gets the ConnectionCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ObservableCollection<ListConnectionModel> ConnectionCollection
        {
            get
            {
                return _connectionCollection;
            }

            set
            {
                if (_connectionCollection == value)
                {
                    return;
                }

                var oldValue = _connectionCollection;
                _connectionCollection = value;
                RaisePropertyChanged(ConnectionCollectionPropertyName, oldValue, value, true);
            }
        }


        #endregion

        #region Command
        private RelayCommand<int> _dragCommand;

        /// <summary>
        /// Gets the DragCommand.
        /// </summary>
        public RelayCommand<int> DragCommand
        {
            get
            {
                return _dragCommand ?? (_dragCommand = new RelayCommand<int>(
                    ExecuteDragCommand,
                    CanExecuteDragCommand));
            }
        }

        private void ExecuteDragCommand(int parameter)
        {
            _draggedIndex = parameter;
        }

        private bool CanExecuteDragCommand(int parameter)
        {
            // var item = _connectionCollection.FirstOrDefault(tuple => tuple.Item1 == parameter);

            // return item == null;
            return true;
        }

        private RelayCommand<int> _dropCommand;

        /// <summary>
        /// Gets the DropCommand.
        /// </summary>
        public RelayCommand<int> DropCommand
        {
            get
            {
                return _dropCommand ?? (_dropCommand = new RelayCommand<int>(
                    ExecuteDropCommand,
                    CanExecuteDropCommand));
            }
        }

        private void ExecuteDropCommand(int parameter)
        {
            //  ConnectionCollection.Add(new Tuple<int, int>(_draggedIndex.Value, parameter));
            //  ConnectionCollection.Add(parameter.ToString());

            var line = new ListConnectionModel()
            {
                DepartureIndex = _draggedIndex.Value,
                ArrivalIndex = parameter,
            };
            ConnectionCollection.Add(line);
            _draggedIndex = null;
        }

        private bool CanExecuteDropCommand(int parameter)
        {
            return !ConnectionCollection.Any(item => item.DepartureIndex == _draggedIndex && item.ArrivalIndex == parameter);
        }

        private RelayCommand<object> _doubleClickCommand;

        /// <summary>
        /// Gets the LineDoubleClickCommand.
        /// </summary>
        public RelayCommand<object> DoubleClickCommand
        {
            get
            {
                return _doubleClickCommand ?? (_doubleClickCommand = new RelayCommand<object>(
                    ExecuteDoubleClickCommand,
                    CanExecuteDoubleClickCommand));
            }
        }

        private void ExecuteDoubleClickCommand(object parameter)
        {
            var lineobj = parameter as ListConnectionModel;
            if (lineobj == null) return;
            var before = BeforeItems.FirstOrDefault(item => item.No == lineobj.DepartureIndex);
            var after = AfterItems.FirstOrDefault(item => item.No == lineobj.ArrivalIndex);

            if (before != null && after != null)
            {
                Messenger.Default.Send<object>(new Tuple<ProfileModel, ProfileModel>(before, after), "ShowDetail");
            }
        }

        private bool CanExecuteDoubleClickCommand(object parameter)
        {
            return true;
        }

        private RelayCommand<object> _deleteCommand;

        /// <summary>
        /// Gets the DeleteCommand.
        /// </summary>
        public RelayCommand<object> DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand = new RelayCommand<object>(
                    ExecuteDeleteCommand,
                    CanExecuteDeleteCommand));
            }
        }

        private void ExecuteDeleteCommand(object parameter)
        {
            if(ConnectionCollection.Any(item => item.Equals(parameter)))
            {
                ConnectionCollection.Remove(parameter as ListConnectionModel);
            }
        }

        private bool CanExecuteDeleteCommand(object parameter)
        {
            return true;
        }


        private RelayCommand _autoConnectCommand;

        /// <summary>
        /// Gets the AutoConnectCommand.
        /// </summary>
        public RelayCommand AutoConnectCommand
        {
            get
            {
                return _autoConnectCommand
                    ?? (_autoConnectCommand = new RelayCommand(
                    () =>
                    {
                        foreach(var bitem in BeforeItems)
                        {

                            if (!ConnectionCollection.Any(citem => citem.DepartureIndex == bitem.No && citem.ArrivalIndex == bitem.No))
                            {
                                var line = new ListConnectionModel()
                                {
                                    DepartureIndex = bitem.No,
                                    ArrivalIndex = bitem.No,
                                };
                                ConnectionCollection.Add(line);
                                break;
                            }
                        }


                    }));
            }
        }

        #endregion
    }
}
