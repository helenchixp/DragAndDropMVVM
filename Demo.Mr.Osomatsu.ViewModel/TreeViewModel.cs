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
    public class TreeViewModel: ViewModelBase
    {
        private ITreeItemModel _dragItem = null;

        #region Constructor
        public TreeViewModel()
        {
            _palletItems = new ObservableCollection<ITreeItemModel>();

            for (var i = 1; i <= 5; i++)
            {
                _palletItems.Add(new GroupModel()
                {
                    Name = $"Brothers {i}",
                    No = 100 + i,
                    ImagePath = $"/Demo.Mr.Osomatsu;component/ImagesResource/m_brothers_{i.ToString().PadLeft(2, '0')}.png",
                });

            }

            _palletItems.Add(new GroupModel()
            {
                Name = "Brother 1x3",
                No = 13,
                ImagePath = "/Demo.Mr.Osomatsu;component/ImagesResource/m_brothers_1x3_1.png"
            });
            _palletItems.Add(new GroupModel()
            {
                Name = "Brother 1x5",
                No = 15,
                ImagePath = "/Demo.Mr.Osomatsu;component/ImagesResource/m_brothers_1x5.png"
            });
            _palletItems.Add(new GroupModel()
            {
                Name = "Brother 2x4",
                No = 24,
                ImagePath = "/Demo.Mr.Osomatsu;component/ImagesResource/m_brothers_2x4.png"
            });
            _palletItems.Add(new GroupModel()
            {
                Name = "Brother 2x6",
                No = 26,
                ImagePath = "/Demo.Mr.Osomatsu;component/ImagesResource/m_brothers_2x6.png"
            });
            _palletItems.Add(new GroupModel()
            {
                Name = "Brother 4x5",
                No = 45,
                ImagePath = "/Demo.Mr.Osomatsu;component/ImagesResource/m_brothers_4x5.png"
            });

            for (var i = 1; i <= 6; i++)
            {
                _palletItems.Add(new ProfileConnectorModel()
                {
                    Name = Const.BrotherNames[i - 1],
                    No = i,
                    ImagePath = $"/Demo.Mr.Osomatsu;component/ImagesResource/m{i}_03.png",
                });

            }
        }
        #endregion

        #region RaiseProperty
        /// <summary>
        /// The <see cref="PalletItems" /> property's name.
        /// </summary>
        public const string PalletItemsPropertyName = "PalletItems";

        private ObservableCollection<ITreeItemModel> _palletItems = null;

        /// <summary>
        /// Sets and gets the PalletItems property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ObservableCollection<ITreeItemModel> PalletItems
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

        /// <summary>
            /// The <see cref="Group01" /> property's name.
            /// </summary>
        public const string Group01PropertyName = "Group01";

        private GroupModel _group01 = new GroupModel();

        /// <summary>
        /// Sets and gets the Group01 property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public GroupModel Group01
        {
            get
            {
                return _group01;
            }

            set
            {
                if (_group01 == value)
                {
                    return;
                }

                var oldValue = _group01;
                _group01 = value;
                RaisePropertyChanged(Group01PropertyName, oldValue, value, true);
            }
        }
        /// <summary>
            /// The <see cref="Group02" /> property's name.
            /// </summary>
        public const string Group02PropertyName = "Group02";

        private GroupModel _group02 = new GroupModel();

        /// <summary>
        /// Sets and gets the Group02 property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public GroupModel Group02
        {
            get
            {
                return _group02;
            }

            set
            {
                if (_group02 == value)
                {
                    return;
                }

                var oldValue = _group02;
                _group02 = value;
                RaisePropertyChanged(Group02PropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// The <see cref="NodeConnectors" /> property's name.
        /// </summary>
        public const string NodeConnectorsPropertyName = "NodeConnectors";

        private ObservableCollection<NodeConnectorModel> _nodeConnectors = new ObservableCollection<NodeConnectorModel>();

        /// <summary>
        /// Sets and gets the NodeConnectors property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ObservableCollection<NodeConnectorModel> NodeConnectors
        {
            get
            {
                return _nodeConnectors;
            }

            set
            {
                if (_nodeConnectors == value)
                {
                    return;
                }

                var oldValue = _nodeConnectors;
                _nodeConnectors = value;
                RaisePropertyChanged(NodeConnectorsPropertyName, oldValue, value, true);
            }
        }

        #endregion

        #region Dragged Object
        /// <summary>
        /// The <see cref="DraggedDepatureModel" /> property's name.
        /// </summary>
        public const string DraggedDepatureModelPropertyName = "DraggedDepatureModel";

        private object _draggedDepatureModel = null;

        /// <summary>
        /// Sets and gets the DraggedDepatureModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public object DraggedDepatureModel
        {
            get
            {
                return _draggedDepatureModel;
            }

            set
            {
                if (_draggedDepatureModel == value)
                {
                    return;
                }

                var oldValue = _draggedDepatureModel;
                _draggedDepatureModel = value;
                RaisePropertyChanged(DraggedDepatureModelPropertyName, oldValue, value, true);
            }
        }
        #endregion

        #region Command

        private RelayCommand<ITreeItemModel> _dragCommand;

        /// <summary>
        /// Gets the DragCommand.
        /// </summary>
        public RelayCommand<ITreeItemModel> DragCommand
        {
            get
            {
                return _dragCommand ?? (_dragCommand = new RelayCommand<ITreeItemModel>(
                    ExecuteDragCommand,
                    CanExecuteDragCommand));
            }
        }

        private void ExecuteDragCommand(ITreeItemModel parameter)
        {
            _dragItem = (parameter as IClone<ITreeItemModel>).Clone();
        }

        private bool CanExecuteDragCommand(ITreeItemModel parameter)
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
            var dragitem = (_dragItem as IClone<ITreeItemModel>).Clone();

            if (parameter is GroupModel)
            {
                dragitem.Parent = (parameter as IGroupModel);

                var lastchild = (parameter as GroupModel).Children.Any() ? (parameter as GroupModel).Children[(parameter as GroupModel).Children.Count - 1] : null;
                //set the center Yposition in item
                dragitem.Y = (lastchild?.Y ?? 0.0) + (lastchild?.Interval ?? 0.0) / 2 + dragitem.Interval / 2;
           

                (parameter as GroupModel).Children.Add(dragitem);

                (parameter as GroupModel).RefreshByExtended();
            }

            _dragItem = null;
        }

        private bool CanExecuteDropCommand(object parameter)
        {
            var result = false;

            if (parameter is GroupModel)
            {
                result = !(parameter as GroupModel).Children.Any(child => child.No.Equals(_dragItem.No));
            }


            return result;
        }


        private RelayCommand<ProfileConnectorModel> _dragLineCommand;

        /// <summary>
        /// Gets the DragLineCommand.
        /// </summary>
        public RelayCommand<ProfileConnectorModel> DragLineCommand
        {
            get
            {
                return _dragLineCommand ?? (_dragLineCommand = new RelayCommand<ProfileConnectorModel>(
                    ExecuteDragLineCommand,
                    CanExecuteDragLineCommand));
            }
        }

        private void ExecuteDragLineCommand(ProfileConnectorModel parameter)
        {
            _dragItem = parameter;

            System.Diagnostics.Debug.WriteLine($"   _dragItem.Y - {_dragItem.Y} ");
        }

        private bool CanExecuteDragLineCommand(ProfileConnectorModel parameter)
        {
            return true;
        }


        private RelayCommand<ProfileConnectorModel> _dropLineCommand;

        /// <summary>
        /// Gets the DropLineCommand.
        /// </summary>
        public RelayCommand<ProfileConnectorModel> DropLineCommand
        {
            get
            {
                return _dropLineCommand ?? (_dropLineCommand = new RelayCommand<ProfileConnectorModel>(
                    ExecuteDropLineCommand,
                    CanExecuteDropLineCommand));
            }
        }

        private void ExecuteDropLineCommand(ProfileConnectorModel parameter)
        {
            parameter.Departures.Add(_dragItem as ProfileConnectorModel);
            (_dragItem as ProfileConnectorModel).Arrivals.Add(parameter);

            NodeConnectors.Add(new NodeConnectorModel()
            {
                DepartureNode= _dragItem as ProfileConnectorModel,
                ArrivalNode = parameter,
            });
        }

        private bool CanExecuteDropLineCommand(ProfileConnectorModel parameter)
        {

            if (_dragItem is ProfileConnectorModel)
            {
                return (_dragItem as ProfileConnectorModel).GetRoot() != parameter.GetRoot();
            }
            else
            {
                return false;
            }
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
            if (parameter is NodeConnectorModel)
            {
                var connector = (parameter as NodeConnectorModel);

                (connector.ArrivalNode as ProfileConnectorModel).Departures.Remove(connector.DepartureNode as ProfileConnectorModel);
                (connector.DepartureNode as ProfileConnectorModel).Arrivals.Remove(connector.ArrivalNode as ProfileConnectorModel);

                NodeConnectors.Remove(parameter as NodeConnectorModel);
            }
            else if(parameter is ITreeItemModel)
            {
                var root = (parameter as ITreeItemModel).GetRoot();
                (parameter as ITreeItemModel).Parent.Children.Remove(parameter as ITreeItemModel);
                (root as GroupModel).RefreshByExtended();
            }
        }

        private bool CanExecuteDeleteCommand(object parameter)
        {

            if (parameter is NodeConnectorModel)
            {
                return true;
            }
            else if (parameter is ITreeItemModel)
            {
                if ((parameter as ITreeItemModel)?.Parent == null)
                {
                    return false;
                }
                else if (parameter is GroupModel)
                {
                    return !((parameter as GroupModel).Children?.Any() ?? false);
                }
                else if (parameter is ProfileConnectorModel)
                {
                    return !(
                        ((parameter as ProfileConnectorModel).Arrivals?.Any() ?? false)
                        ||
                       ((parameter as ProfileConnectorModel).Departures?.Any() ?? false)
                       );
                }

            }
                return (parameter as ITreeItemModel)?.Parent != null;
        }


        #endregion




    }
}
