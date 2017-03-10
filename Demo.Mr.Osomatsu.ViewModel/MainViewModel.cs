using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Demo.Mr.Osomatsu.ViewModel
{
    public class MainViewModel : ViewModelBase
    {


        /// <summary>
        /// The <see cref="CurrentContent" /> property's name.
        /// </summary>
        public const string CurrentContentPropertyName = "CurrentContent";

        private ViewModelBase _currentContent = null;

        /// <summary>
        /// Sets and gets the CurrentContent property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ViewModelBase CurrentContent
        {
            get
            {
                return _currentContent;
            }

            set
            {
                if (_currentContent == value)
                {
                    return;
                }

                var oldValue = _currentContent;
                _currentContent = value;
                RaisePropertyChanged(CurrentContentPropertyName, oldValue, value, true);
            }
        }

        private RelayCommand<ViewModelBase> _changeCommand;

        /// <summary>
        /// Gets the ChangeCommand.
        /// </summary>
        public RelayCommand<ViewModelBase> ChangeCommand
        {
            get
            {
                return _changeCommand ?? (_changeCommand = new RelayCommand<ViewModelBase>(
                    parameter =>
                    {
                        CurrentContent = parameter;
                    }));
            }
        }


      
    }
}
