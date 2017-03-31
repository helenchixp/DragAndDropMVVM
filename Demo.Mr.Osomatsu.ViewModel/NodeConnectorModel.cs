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



    }
}
