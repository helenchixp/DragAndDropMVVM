using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragAndDropMVVM.ViewModel;
#if PRISM
using Microsoft.Practices.Prism.Mvvm;
#else
using GalaSoft.MvvmLight;
#endif

namespace Demo.YuriOnIce.Relationship.ViewModel
{
    public class DiagramViewModel :
#if PRISM
        BindableBase,
#else
        ViewModelBase,
#endif
        IConnectionDiagramViewModel
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
                if (Index == 1)
                {
#if PRISM
                    _imagePath = "/Demo.YuriOnIce.Relationship.Prism;component/ImagesResource/Yuri.png";
#else
                    _imagePath = "/Demo.YuriOnIce.Relationship.MvvmLight;component/ImagesResource/Yuri.png";
#endif
                }
                else if (Index == 2)
                {
#if PRISM
                    _imagePath = "/Demo.YuriOnIce.Relationship.Prism;component/ImagesResource/Victory.png";
#else
                    _imagePath = "/Demo.YuriOnIce.Relationship.MvvmLight;component/ImagesResource/Victory.png";
#endif
                }
                else if (Index == 3)
                {
#if PRISM
                    _imagePath = "/Demo.YuriOnIce.Relationship.Prism;component/ImagesResource/Yurio.png";
#else
                    _imagePath = "/Demo.YuriOnIce.Relationship.MvvmLight;component/ImagesResource/Yurio.png";
#endif
                }
                else if (Index == 4)
                {
#if PRISM
                    _imagePath = "/Demo.YuriOnIce.Relationship.Prism;component/ImagesResource/Maccachin.png";
#else
                    _imagePath = "/Demo.YuriOnIce.Relationship.MvvmLight;component/ImagesResource/Maccachin.png";
#endif
                }

                return _imagePath;
            }

//            set
//            {
//                if (_imagePath == value)
//                {
//                    return;
//                }
//#if PRISM
//                this.SetProperty(ref _imagePath, value);
//#else

//                var oldValue = _imagePath;
//                _imagePath = value;
//                RaisePropertyChanged(ImagePathPropertyName, oldValue, value, true);
//#endif
//            }
        }


        /// <summary>
        /// The <see cref="Name" /> property's name.
        /// </summary>
        public const string NamePropertyName = "Name";

        private string _name = string.Empty;

        /// <summary>
        /// Sets and gets the Name property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (_name == value)
                {
                    return;
                }
#if PRISM
                this.SetProperty(ref _name, value);
#else
                var oldValue = _name;
                _name = value;
                RaisePropertyChanged(NamePropertyName, oldValue, value, true);
#endif
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
#if PRISM
                this.SetProperty(ref _detail, value);
#else
                var oldValue = _detail;
                _detail = value;
                RaisePropertyChanged(DetailPropertyName, oldValue, value, true);
#endif
            }
        }

#endregion
    }
}
