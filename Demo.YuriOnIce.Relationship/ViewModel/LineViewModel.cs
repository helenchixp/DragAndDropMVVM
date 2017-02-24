using System;
using System.Collections.Generic;
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
    public class LineViewModel :
#if PRISM
        BindableBase,
#else
        ViewModelBase,
#endif
        IConnectionLineViewModel
    {
        #region IConnectionLineViewModel
        public bool IsSelected
        {
            get;

            set;
        }

        public string LineUUID
        {
            get;

            set;
        }

        public IConnectionDiagramViewModel OriginDiagramViewModel
        {
            get;
            set;
        }

        //TODO 再検討
        //private DiagramViewModel _originDiagramViewModel = null;

        //public IConnectionDiagramViewModel OriginDiagramViewModel
        //{
        //    get
        //    {
        //        return _originDiagramViewModel;
        //    }

        //    set
        //    {
        //        if (_originDiagramViewModel == value)
        //        {
        //            return;
        //        }
        //        _originDiagramViewModel = value as DiagramViewModel;

        //        if (_originDiagramViewModel != null)
        //        {
        //            if (_originDiagramViewModel.DepartureLinesViewModel.Contains(this))
        //                return;
        //            else
        //                _originDiagramViewModel.DepartureLinesViewModel.Add(this);
        //        }
        //    }
        //}

        public IConnectionDiagramViewModel TerminalDiagramViewModel
        {
            get;

            set;
        }


        public string TerminalDiagramUUID
        {
            get;
            set;
        }
#endregion

#region Property Changes

#region Comment
        /// <summary>
        /// The <see cref="Comment" /> property's name.
        /// </summary>
        public const string CommentPropertyName = "Comment";

        private string _comment = string.Empty;

        /// <summary>
        /// Sets and gets the Comment property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string Comment
        {
            get
            {
                return _comment;
            }

            set
            {
                if (_comment == value)
                {
                    return;
                }

#if PRISM
                this.SetProperty(ref _comment, value);
#else
                var oldValue = _comment;
                _comment = value;

                IsCompleted = !string.IsNullOrWhiteSpace(_comment);
                RaisePropertyChanged(CommentPropertyName, oldValue, value, true);
#endif
            }
        }
#endregion

        #region IsCompleted
        /// <summary>
        /// The <see cref="IsCompleted" /> property's name.
        /// </summary>
        public const string IsCompletedPropertyName = "IsCompleted";

        private bool _isCompleted = false;

        /// <summary>
        /// Sets and gets the IsCompleted property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                return _isCompleted;
            }

            set
            {
                if (_isCompleted == value)
                {
                    return;
                }
#if PRISM
                this.SetProperty(ref _isCompleted, value);
#else
                var oldValue = _isCompleted;
                _isCompleted = value;
                RaisePropertyChanged(IsCompletedPropertyName, oldValue, value, true);
#endif
            }
        }
#endregion

#endregion
    }
}
