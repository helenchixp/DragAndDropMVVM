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
    public class GroupViewModel: ViewModelBase
    {
        private ProfileModel _draggedObject = null;

        public GroupViewModel()
        {
            for (var i = 1; i <= 6; i++)
            {
                _palletList.Add(new ProfileModel()
                {
                    Name = Const.BrotherNames[i - 1],
                    No = i,
                    //Comment = $"It is normal No.{i} Brother",
                    ImagePath = $"/Demo.Mr.Osomatsu;component/ImagesResource/m{i}_02.png",
                });
             
            }
        }

        #region ObservableCollection
        /// <summary>
        /// The <see cref="PalletList" /> property's name.
        /// </summary>
        public const string PalletListPropertyName = "PalletList";

        private ObservableCollection<ProfileModel> _palletList = new ObservableCollection<ProfileModel>();

        /// <summary>
        /// Sets and gets the PalletList property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ObservableCollection<ProfileModel> PalletList
        {
            get
            {
                return _palletList;
            }

            set
            {
                if (_palletList == value)
                {
                    return;
                }

                var oldValue = _palletList;
                _palletList = value;
                RaisePropertyChanged(PalletListPropertyName, oldValue, value, true);
            }
        }


        /// <summary>
        /// The <see cref="Group1x3" /> property's name.
        /// </summary>
        public const string Group1x3PropertyName = "Group1x3";

        private ObservableCollection<ProfileModel> _group1x3 = new ObservableCollection<ProfileModel>();

        /// <summary>
        /// Sets and gets the Group1x3 property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ObservableCollection<ProfileModel> Group1x3
        {
            get
            {
                return _group1x3;
            }

            set
            {
                if (_group1x3 == value)
                {
                    return;
                }

                var oldValue = _group1x3;
                _group1x3 = value;
                RaisePropertyChanged(Group1x3PropertyName, oldValue, value, true);
            }
        }


        /// <summary>
        /// The <see cref="Group2x6" /> property's name.
        /// </summary>
        public const string Group2x6PropertyName = "Group2x6";

        private ObservableCollection<ProfileModel> _group2x6 = new ObservableCollection<ProfileModel>();

        /// <summary>
        /// Sets and gets the Group2x6 property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ObservableCollection<ProfileModel> Group2x6
        {
            get
            {
                return _group2x6;
            }

            set
            {
                if (_group2x6 == value)
                {
                    return;
                }

                var oldValue = _group2x6;
                _group2x6 = value;
                RaisePropertyChanged(Group2x6PropertyName, oldValue, value, true);
            }
        }

        /// <summary>
            /// The <see cref="Group4x5" /> property's name.
            /// </summary>
        public const string Group4x5PropertyName = "Group4x5";

        private ObservableCollection<ProfileModel> _group4x5 = new ObservableCollection<ProfileModel>();

        /// <summary>
        /// Sets and gets the Group4x5 property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ObservableCollection<ProfileModel> Group4x5
        {
            get
            {
                return _group4x5;
            }

            set
            {
                if (_group4x5 == value)
                {
                    return;
                }

                var oldValue = _group4x5;
                _group4x5 = value;
                RaisePropertyChanged(Group4x5PropertyName, oldValue, value, true);
            }
        }


        /// <summary>
        /// The <see cref="Brothers" /> property's name.
        /// </summary>
        public const string BrothersPropertyName = "Brothers";

        private ObservableCollection<ProfileModel> _brothers = new ObservableCollection<ProfileModel>();

        /// <summary>
        /// Sets and gets the Brothers property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ObservableCollection<ProfileModel> Brothers
        {
            get
            {
                return _brothers;
            }

            set
            {
                if (_brothers == value)
                {
                    return;
                }

                var oldValue = _brothers;
                _brothers = value;
                RaisePropertyChanged(BrothersPropertyName, oldValue, value, true);
            }
        }

        #endregion


        #region Command
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
            _draggedObject = parameter as ProfileModel;
        }

        private bool CanExecuteDragCommand(object parameter)
        {
            return true;
        }

        private RelayCommand<object> _dropCommand;

        /// <summary>
        /// Gets the DropCommon.
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
            Action<ObservableCollection<ProfileModel>, string> listAdd = (group, comment) =>
            {
                if (!group.Contains(_draggedObject))
                {
                    var dragobj = _draggedObject.Clone(); ;
                    dragobj.Comment = comment;
                    group.Add(dragobj);

                    

                    if ("1x3".Equals(_draggedObject.Comment))
                    {
                        Group1x3.Remove(_draggedObject);
                    }
                    else if ("2x6".Equals(_draggedObject.Comment))
                    {
                        Group2x6.Remove(_draggedObject);
                    }
                    else if ("4x5".Equals(_draggedObject.Comment))
                    {
                        Group4x5.Remove(_draggedObject);
                    }
                    else if("1x2x3x4x5x6".Equals(_draggedObject.Comment))
                    {
                        Brothers.Remove(_draggedObject);
                    }

                }
                _draggedObject = null;
            };


            if ("1x3".Equals(parameter))
            {
                listAdd(Group1x3, "1x3");
            }
            else if ("2x6".Equals(parameter))
            {
                listAdd(Group2x6, "2x3");
            }
            else if ("4x5".Equals(parameter))
            {
                listAdd(Group4x5, "4x5");
            }
            else
            {
                listAdd(Brothers, parameter.ToString());
            }

            _draggedObject = null;
        }

        private bool CanExecuteDropCommand(object parameter)
        {
            if (parameter == null || _draggedObject == null)
                return false;

            var group = parameter.ToString().Split('x');
            if (group == null) return false;



            return group.Any(id => id == _draggedObject.No.ToString());
        }


        private RelayCommand<object> _orderChangeCommand;

        /// <summary>
        /// Gets the OrderChangeCommand.
        /// </summary>
        public RelayCommand<object> OrderChangeCommand
        {
            get
            {
                return _orderChangeCommand ?? (_orderChangeCommand = new RelayCommand<object>(
                    ExecuteOrderChangeCommand,
                    CanExecuteOrderChangeCommand));
            }
        }

        private void ExecuteOrderChangeCommand(object parameter)
        {

        }

        private bool CanExecuteOrderChangeCommand(object parameter)
        {
            if(parameter!=null && _draggedObject!=null)
            {
                return !_draggedObject.Equals(parameter);
            }
            return false;
        }
        #endregion

    }
}
