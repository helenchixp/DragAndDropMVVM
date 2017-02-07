using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragAndDropMVVM.ViewModel;
using GalaSoft.MvvmLight;

namespace Demo.YuriOnIce.Relationship.ViewModel
{
    public class DiagramViewModel : ViewModelBase, IConnectionDiagramViewModel
    {
        #region IConnectionDiagramViewModel
        public ObservableCollection<IConnectionLineViewModel> ArrivalLinesViewModel
        {
            get;

            set;
        } = new ObservableCollection<IConnectionLineViewModel>();


        public ObservableCollection<IConnectionLineViewModel> DepartureLinesViewModel
        {
            get;

            set;
        } = new ObservableCollection<IConnectionLineViewModel>();

        public string DiagramID
        {
            get;

            set;
        }

        public int Index
        {
            get;

            set;
        } = 0;

        public bool IsSelected
        {
            get;

            set;
        }
        #endregion


        #region Properties
        /// <summary>
        /// The <see cref="ImagePath" /> property's name.
        /// </summary>
        public const string ImagePathPropertyName = "ImagePath";

        private string _imagePath = string.Empty;

        /// <summary>
        /// Sets and gets the ImagePath property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string ImagePath
        {
            get
            {
                return _imagePath;
            }

            set
            {
                if (_imagePath == value)
                {
                    return;
                }

                var oldValue = _imagePath;
                _imagePath = value;
                RaisePropertyChanged(ImagePathPropertyName, oldValue, value, true);
            }
        }


        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private string _title = string.Empty;

        /// <summary>
        /// Sets and gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                if (_title == value)
                {
                    return;
                }

                var oldValue = _title;
                _title = value;
                RaisePropertyChanged(TitlePropertyName, oldValue, value, true);
            }
        }


        /// <summary>
        /// The <see cref="Detail" /> property's name.
        /// </summary>
        public const string DetailPropertyName = "Detail";

        private string _detail = string.Empty;

        /// <summary>
        /// Sets and gets the Detail property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string Detail
        {
            get
            {
                return _detail;
            }

            set
            {
                if (_detail == value)
                {
                    return;
                }

                var oldValue = _detail;
                _detail = value;
                RaisePropertyChanged(DetailPropertyName, oldValue, value, true);
            }
        }

        #endregion
    }
}
