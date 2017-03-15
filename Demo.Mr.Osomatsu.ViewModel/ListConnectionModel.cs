using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Demo.Mr.Osomatsu.ViewModel
{
    public class ListConnectionModel : ViewModelBase
    {

        private int _y1 = 0;

        /// <summary>
        /// Sets and gets the Y1 property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public int Y1
        {
            get
            {
                return _y1;
            }

            set
            {
                if (_y1 == value)
                {
                    return;
                }

                var oldValue = _y1;
                _y1 = value;
                RaisePropertyChanged(Y1PropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// The <see cref="Y2" /> property's name.
        /// </summary>
        public const string Y2PropertyName = "Y2";

        private int _y2 = 0;

        /// <summary>
        /// Sets and gets the Y2 property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public int Y2
        {
            get
            {
                return _y2;
            }

            set
            {
                if (_y2 == value)
                {
                    return;
                }

                var oldValue = _y2;
                _y2 = value;
                RaisePropertyChanged(Y2PropertyName, oldValue, value, true);
            }
        }

        /// <summary>
            /// The <see cref="Y1" /> property's name.
            /// </summary>
        public const string Y1PropertyName = "Y1";

       
        private int _departureIndex = 0;
        public int DepartureIndex
        {
            get
            {
                return _departureIndex;
            }
            set
            {
                _departureIndex = value;
                Y1 = _departureIndex * 40 -20;
            }
        }

        private int _arrivalIndex = 0;
        public int ArrivalIndex
        {
            get
            {
                return _arrivalIndex;
            }
            set
            {
                _arrivalIndex = value;
                Y2 = _arrivalIndex * 40 -20;
            }
        }



        public bool IsSame
        {
           get
            {
                return _arrivalIndex == _departureIndex;
            }
        }
    }
}
