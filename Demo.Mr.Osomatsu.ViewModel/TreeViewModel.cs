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
                    No = 100+i,
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

        ///// <summary>
        //    /// The <see cref="Group01" /> property's name.
        //    /// </summary>
        //public const string Group01PropertyName = "Group01";

        //private ObservableCollection<ITreeItemModel> _group01 = null;

        ///// <summary>
        ///// Sets and gets the Group01 property.
        ///// Changes to that property's value raise the PropertyChanged event. 
        ///// This property's value is broadcasted by the MessengerInstance when it changes.
        ///// </summary>
        //public ObservableCollection<ITreeItemModel> Group01
        //{
        //    get
        //    {
        //        return _group01 ?? (_group01 = new ObservableCollection<ITreeItemModel>());
        //    }

        //    set
        //    {
        //        if (_group01 == value)
        //        {
        //            return;
        //        }

        //        var oldValue = _group01;
        //        _group01 = value;
        //        RaisePropertyChanged(Group01PropertyName, oldValue, value, true);
        //    }
        //}


        ///// <summary>
        //    /// The <see cref="Group02" /> property's name.
        //    /// </summary>
        //public const string Group02PropertyName = "Group02";

        //private ObservableCollection<ITreeItemModel> _group02 = null;

        ///// <summary>
        ///// Sets and gets the Group02 property.
        ///// Changes to that property's value raise the PropertyChanged event. 
        ///// This property's value is broadcasted by the MessengerInstance when it changes.
        ///// </summary>
        //public ObservableCollection<ITreeItemModel> Group02
        //{
        //    get
        //    {
        //        return _group02 ?? (_group02 = new ObservableCollection<ITreeItemModel>());
        //    }

        //    set
        //    {
        //        if (_group02 == value)
        //        {
        //            return;
        //        }

        //        var oldValue = _group02;
        //        _group02 = value;
        //        RaisePropertyChanged(Group02PropertyName, oldValue, value, true);
        //    }
        //}


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
            _dragItem = parameter;
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

                double ypos = dragitem.Parent.Y;
                if ((parameter as GroupModel).Children.Any())
                {
                    ypos += (parameter as GroupModel).Children[(parameter as GroupModel).Children.Count - 1].Y +
                        (parameter as GroupModel).Children[(parameter as GroupModel).Children.Count - 1].Interval/2 + 
                        dragitem.Interval /2;
                }
                dragitem.Y = ypos;
                (parameter as GroupModel).Children.Add(dragitem);
            }

            ////var dragitem = (_dragItem as IClone<ITreeItemModel>).Clone();
            ////dragitem.GroupNo = (parameter as ITreeItemModel)?.No ?? -1;
            ////dragitem.Parent = (parameter as IGroupModel);

            ////if (parameter is ProfileConnectorModel &&
            ////    _dragItem is ProfileConnectorModel)
            ////{

            ////}
            ////else if ((parameter is GroupModel) &&
            ////  (parameter as GroupModel).No > 100 &&
            ////  (_dragItem.No < 100 && _dragItem.No > 10))
            ////{
            ////    double posY = ((parameter as GroupModel).Parent as IPosition).Y;

            ////    posY += ((parameter as GroupModel).Children?.Count ?? 0) * 30;
            ////    dragitem.Y = posY;
            ////    (parameter as GroupModel).Children.Add(dragitem);
            ////}
            ////else if ((parameter is GroupModel) &&
            ////    (parameter as GroupModel).No < 100)
            ////{
            ////    if (_dragItem.No < 10)
            ////    {
            ////        (parameter as GroupModel).Children.Add(dragitem);
            ////    }
            ////    else if (_dragItem.No < 100)
            ////    {
            ////        int idx = (parameter as GroupModel).Parent.Children.IndexOf(parameter as GroupModel);
            ////        dragitem.Parent = (parameter as GroupModel).Parent;
            ////        dragitem.GroupNo = ((parameter as GroupModel).Parent as ITreeItemModel).No;
            ////        (parameter as GroupModel).Parent.Children.Insert(idx, dragitem);
            ////    }
            ////}
            ////else if (parameter is ObservableCollection<ITreeItemModel>)
            ////{
            ////    if (dragitem is IPosition)
            ////    {
            ////        (dragitem as IPosition).Y = (parameter as ObservableCollection<ITreeItemModel>).Count * 30;
            ////    }

            ////    (parameter as ObservableCollection<ITreeItemModel>).Add(dragitem);
            ////}

            ////if (_dragItem.Parent != null && _dragItem.Parent.Children != null)
            ////{
            ////    _dragItem.Parent.Children.Remove(_dragItem);
            ////}

            _dragItem = null;
        }

        private bool CanExecuteDropCommand(object parameter)
        {
            var result = false;

            if (parameter is GroupModel)
            {
                result = !(parameter as GroupModel).Children.Any(child => child.No.Equals(_dragItem.No));
            }

            ////////if (parameter is ProfileConnectorModel &&
            ////////    _dragItem is ProfileConnectorModel)
            ////////{
            ////////    result = true;
            ////////}
            ////////else if ((parameter is GroupModel) &&
            ////////    (parameter as GroupModel).No > 100 &&
            ////////    (_dragItem.No < 100 && _dragItem.No > 10))
            ////////{
            ////////    result = !(parameter as GroupModel).Children.Any(child => child.No.Equals(_dragItem.No));
            ////////}
            ////////else if ((parameter is GroupModel) &&
            ////////    (parameter as GroupModel).No < 100)
            ////////{
            ////////    if (_dragItem.No < 10)
            ////////    {
            ////////        result = !(parameter as GroupModel).Children.Any(child => child.No.Equals(_dragItem.No));
            ////////    }
            ////////    else if (_dragItem.No < 100)
            ////////    {
            ////////        result = !((parameter as GroupModel).Parent.Children.Any(child => child.No.Equals(_dragItem.No)));
            ////////    }
            ////////}
            ////////else if (parameter is ObservableCollection<ITreeItemModel>)
            ////////{
            ////////    if (_dragItem is GroupModel && _dragItem.No >= 100)
            ////////    {
            ////////        result = !(parameter as ObservableCollection<ITreeItemModel>).Any(item => item.No.Equals(_dragItem.No));
            ////////    }
            ////////    else
            ////////    {
            ////////        foreach (var root in (parameter as ObservableCollection<ITreeItemModel>))
            ////////        {
            ////////            if (!((root as GroupModel)?.Children?.Any(child => child.No.Equals(_dragItem.No)) ?? false))
            ////////            {
            ////////                result = false;
            ////////                break;
            ////////            }
            ////////        }
            ////////    }


            ////////}

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

        #endregion

    }
}
