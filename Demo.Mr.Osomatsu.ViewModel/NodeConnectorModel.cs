using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Demo.Mr.Osomatsu.ViewModel
{
    public class NodeConnectorModel : ViewModelBase
    {
        public int Index { get; set; }

        /// <summary>
            /// The <see cref="DepartureNode" /> property's name.
            /// </summary>
        public const string DepartureNodePropertyName = "DepartureNode";

        private IPosition _departureNode = null;

        /// <summary>
        /// Sets and gets the DepartureNode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public IPosition DepartureNode
        {
            get
            {
                return _departureNode;
            }

            set
            {
                if (_departureNode == value)
                {
                    return;
                }

                var oldValue = _departureNode;
                _departureNode = value;
                RaisePropertyChanged(DepartureNodePropertyName, oldValue, value, true);
            }
        }



        /// <summary>
            /// The <see cref="ArrivalNode" /> property's name.
            /// </summary>
        public const string ArrivalNodePropertyName = "ArrivalNode";

        private IPosition _arrivalNode = null;

        /// <summary>
        /// Sets and gets the ArrivalNode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public IPosition ArrivalNode
        {
            get
            {
                return _arrivalNode;
            }

            set
            {
                if (_arrivalNode == value)
                {
                    return;
                }

                var oldValue = _arrivalNode;
                _arrivalNode = value;
                RaisePropertyChanged(ArrivalNodePropertyName, oldValue, value, true);
            }
        }




        /// <summary>
        /// The <see cref="X1" /> property's name.
        /// </summary>
        public const string X1PropertyName = "X1";


        /// <summary>
        /// Sets and gets the X1 property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public double X1
        {
            get
            {
                return _departureNode?.X ?? 0.0;
            }

            set
            {
                if (_departureNode?.X == value)
                {
                    return;
                }

                var oldValue = _departureNode?.X;
                (_departureNode ??( _departureNode = new ProfileConnectorModel())).X = value;
                RaisePropertyChanged(X1PropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// The <see cref="Y1" /> property's name.
        /// </summary>
        public const string Y1PropertyName = "Y1";


        /// <summary>
        /// Sets and gets the Y1 property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public double Y1
        {
            get
            {
                return _departureNode?.Y ?? 0.0;
            }

            set
            {
                if (_departureNode?.Y == value)
                {
                    return;
                }

                var oldValue = _departureNode?.Y;
                (_departureNode ?? (_departureNode = new ProfileConnectorModel())).Y = value;
                RaisePropertyChanged(Y1PropertyName, oldValue, value, true);
            }
        }


        /// <summary>
        /// The <see cref="X2" /> property's name.
        /// </summary>
        public const string X2PropertyName = "X2";


        /// <summary>
        /// Sets and gets the X2 property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public double X2
        {
            get
            {
                return _arrivalNode?.X ?? 0.0;
            }

            set
            {
                if (_arrivalNode?.X == value)
                {
                    return;
                }

                var oldValue = _arrivalNode?.X;
                (_arrivalNode ?? (_arrivalNode = new ProfileConnectorModel())).X = value;
                RaisePropertyChanged(X2PropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// The <see cref="Y2" /> property's name.
        /// </summary>
        public const string Y2PropertyName = "Y2";


        /// <summary>
        /// Sets and gets the Y2 property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public double Y2
        {
            get
            {
                return _arrivalNode?.Y ?? 0.0;
            }

            set
            {
                if (_arrivalNode?.Y == value)
                {
                    return;
                }

                var oldValue = _arrivalNode?.Y;
                (_arrivalNode ?? (_arrivalNode = new ProfileConnectorModel())).Y = value;
                RaisePropertyChanged(Y2PropertyName, oldValue, value, true);
            }
        }

    }
}
