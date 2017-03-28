using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Mr.Osomatsu.ViewModel
{
    public class ProfileConnectorModel : ProfileModel, ITreeItemModel, IPosition, IClone<ITreeItemModel>
    {
        public IGroupModel Parent { get; set; }

        public new ITreeItemModel Clone()
        {
            return (ProfileConnectorModel)base.Clone();
        }

        public ObservableCollection<ProfileConnectorModel> Departures { get; set; }

        public ObservableCollection<ProfileConnectorModel> Arrivals { get; set; }

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
                return _y == 0.0 ? (_y += Parent.Y + _interval/ 2) : _y ;
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
    }
}
