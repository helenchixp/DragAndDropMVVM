using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

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
                    Name = Const.BrotherNames[i-1],
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

        /// <summary>
            /// The <see cref="ConnectionCollection" /> property's name.
            /// </summary>
        public const string ConnectionCollectionPropertyName = "ConnectionCollection";

        private ObservableCollection<Tuple<int,int>> _connectionCollection = new ObservableCollection<Tuple<int, int>>();

        /// <summary>
        /// Sets and gets the ConnectionCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ObservableCollection<Tuple<int,int>> ConnectionCollection
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
            var item = _connectionCollection.FirstOrDefault(tuple => tuple.Item1 == parameter);

            return item == null;
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
            ConnectionCollection.Add(new Tuple<int, int>(_draggedIndex.Value, parameter));
            _draggedIndex = null;
        }

        private bool CanExecuteDropCommand(int parameter)
        {
            if (_draggedIndex != parameter) return false;


            return true;
        }

        #endregion
    }
}
