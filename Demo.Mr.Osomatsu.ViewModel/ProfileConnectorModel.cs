using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Mr.Osomatsu.ViewModel
{
    public class ProfileConnectorModel : ProfileModel, ITreeItemModel, IClone<ITreeItemModel>
    {
        public IGroupModel Parent { get; set; }

        public new ITreeItemModel Clone()
        {
            return (ProfileConnectorModel)base.Clone();
        }

        /// <summary>
        /// The <see cref="Departures" /> property's name.
        /// </summary>
        public const string DeparturesPropertyName = "Departures";

        private ObservableCollection<ProfileConnectorModel> _departures = null;

        /// <summary>
        /// Sets and gets the Departures property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ObservableCollection<ProfileConnectorModel>  Departures
        {
            get
            {
                return (_departures ?? (_departures = new ObservableCollection<ProfileConnectorModel>()));
            }

            set
            {
                if (_departures == value)
                {
                    return;
                }

                var oldValue = _departures;
                _departures = value;
                RaisePropertyChanged(DeparturesPropertyName, oldValue, value, true);
            }
        }


        /// <summary>
        /// The <see cref="Arrivals" /> property's name.
        /// </summary>
        public const string ArrivalsPropertyName = "Arrivals";

        private ObservableCollection<ProfileConnectorModel> _arrivals = null;

        /// <summary>
        /// Sets and gets the Arrivals property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ObservableCollection<ProfileConnectorModel>  Arrivals
        {
            get
            {
                return (_arrivals ?? (_arrivals = new ObservableCollection<ProfileConnectorModel>()));
            }

            set
            {
                if (_arrivals == value)
                {
                    return;
                }

                var oldValue = _arrivals;
                _arrivals = value;
                RaisePropertyChanged(ArrivalsPropertyName, oldValue, value, true);
            }
        }

        //public ObservableCollection<ProfileConnectorModel> Departures { get; set; }

        //public ObservableCollection<ProfileConnectorModel> Arrivals { get; set; }

        /// <summary>
            /// The <see cref="X" /> property's name.
            /// </summary>
        public const string XPropertyName = "X";

        private double _x = 0.0;

        /// <summary>
        /// Sets and gets the X property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public double X
        {
            get
            {
                return _x;
            }

            set
            {
                if (_x == value)
                {
                    return;
                }

                var oldValue = _x;
                _x = value;
                RaisePropertyChanged(XPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
            /// The <see cref="Y" /> property's name.
            /// </summary>
        public const string YPropertyName = "Y";

        private double _y = 0.0;

        /// <summary>
        /// Sets and gets the Y property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public double Y
        {
            get
            {
                //return _y == 0.0 ? (_y = GetYPosition(this)) : _y;
                return (_y = GetYPosition(this));
                //                return _y == 0.0 ? (_y += Parent.Y + _interval/ 2) : _y ;
            }

            set
            {
                if (_y == value)
                {
                    return;
                }

                var oldValue = _y;
                _y = value;
                RaisePropertyChanged(YPropertyName, oldValue, value, true);
            }
        }


        /// <summary>
        /// The <see cref="Interval" /> property's name.
        /// </summary>
        public const string IntervalPropertyName = "Interval";

        private double _interval = 20.0;

        /// <summary>
        /// Sets and gets the Interval property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public double Interval
        {
            get
            {
                return _interval;
            }

            set
            {
                if (_interval == value)
                {
                    return;
                }

                var oldValue = _interval;
                _interval = value;
                RaisePropertyChanged(IntervalPropertyName, oldValue, value, true);
            }
        }


        public IGroupModel GetRoot()
        {
            return Parent?.GetRoot();
        }


        private double GetYPosition(ITreeItemModel current)
        {
            //var parentBottom = (current.Parent?.Y ?? 0.0) + (current.Parent?.Interval / 2 ?? 0.0);

            var parentBottom = 0.0;

            if ((current.Parent?.Y ?? 0.0) > 0.0)
            {
                parentBottom = current.Parent.Y + current.Parent.Interval / 2;
            }


            var brotherY = parentBottom;

            if (current.Parent?.IsExpanded ?? false)
            {
                foreach (var brother in current.Parent.Children)
                {

                    if (brother == current)
                    {
                        return brotherY + current.Interval / 2;
                    }
                    else
                    {
                        brotherY += brother.Interval;
                        if ((brother is GroupModel) && (brother as GroupModel).IsExpanded)
                        {
                            foreach (var cousin in (brother as GroupModel).Children)
                            {
                                brotherY = GetYPosition(cousin) + cousin.Interval / 2;
                            }
                        }
                    }
                }
            }
            return parentBottom;
        }


        public void RefreshByExtended()
        {
            _y = GetYPosition(this);
            RaisePropertyChanged(YPropertyName);
        }
    }
}
