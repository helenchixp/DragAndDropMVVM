using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Demo.Mr.Osomatsu.ViewModel
{
    public class GroupModel : ViewModelBase, IGroupModel, IClone<ITreeItemModel>
    {



        /// <summary>
        /// The <see cref="GroupNo" /> property's name.
        /// </summary>
        public const string GroupNoPropertyName = "GroupNo";

        private int _groupNo = -1;

        /// <summary>
        /// Sets and gets the GroupNo property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public int GroupNo
        {
            get
            {
                return _groupNo;
            }

            set
            {
                if (_groupNo == value)
                {
                    return;
                }

                var oldValue = _groupNo;
                _groupNo = value;
                RaisePropertyChanged(GroupNoPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
            /// The <see cref="No" /> property's name.
            /// </summary>
        public const string NoPropertyName = "No";

        private int _no = 0;

        /// <summary>
        /// Sets and gets the No property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public int No
        {
            get
            {
                return _no;
            }

            set
            {
                if (_no == value)
                {
                    return;
                }

                var oldValue = _no;
                _no = value;
                RaisePropertyChanged(NoPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
            /// The <see cref="Children" /> property's name.
            /// </summary>
        public const string ChildrenPropertyName = "Children";

        private ObservableCollection<ITreeItemModel> _children = null;

        /// <summary>
        /// Sets and gets the Children property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ObservableCollection<ITreeItemModel> Children
        {
            get
            {
                return _children ?? (_children = new ObservableCollection<ITreeItemModel>());
            }

            set
            {
                if (_children == value)
                {
                    return;
                }

                var oldValue = _children;
                _children = value;
                RaisePropertyChanged(ChildrenPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// The <see cref="IsExpanded" /> property's name.
        /// </summary>
        public const string IsExpandedPropertyName = "IsExpanded";

        private bool _isExpanded = true;

        /// <summary>
        /// Sets and gets the IsExpanded property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }

            set
            {
                if (_isExpanded == value)
                {
                    return;
                }

                var oldValue = _isExpanded;
                _isExpanded = value;


                RefreshByExtended();

                RaisePropertyChanged(IsExpandedPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
            /// The <see cref="IsSelected" /> property's name.
            /// </summary>
        public const string IsSelectedPropertyName = "IsSelected";

        private bool _isSelected = false;

        /// <summary>
        /// Sets and gets the IsSelected property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                if (_isSelected == value)
                {
                    return;
                }

                var oldValue = _isSelected;
                _isSelected = value;
                RaisePropertyChanged(IsSelectedPropertyName, oldValue, value, true);
            }
        }

        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Comment { get; set; }
        public IGroupModel Parent { get; set; }

        public ITreeItemModel Clone()
        {

            return new GroupModel()
            {
                Comment = Comment,
                Name = Name,
                ImagePath = ImagePath,
                No = No,
            };
        }


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
                // return _y == 0.0 ? (_y = GetYPosition(this)) : _y;
                return _y;
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

        private double _interval = 30.0;

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

            if(Parent != null && Parent is ITreeItemModel)
            {
                return Parent.GetRoot();
            }
            else
            {
                return this;
            }

            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="lastbottom"></param>
        /// <param name="grandExpanded"></param>
        private void SetYPosition(ITreeItemModel node, ref double lastbottom , bool grandExpanded = true)
        {
            node.Y = lastbottom + (grandExpanded ? node.Interval / 2 : 0.0);

           // var newstartY = node.Y + node.Interval / 2;

            if (node is GroupModel)
            {
                var newstartY = node.Y + node.Interval / 2;
                if ((node as GroupModel).IsExpanded && grandExpanded)
                {
                    foreach (var child in (node as GroupModel).Children)
                    {
                        SetYPosition(child, ref newstartY);

                    }

                }
                else
                {
                    foreach (var child in (node as GroupModel).Children)
                    {
                        SetYPosition(child, ref newstartY, false);
                    }
                }

                lastbottom = newstartY;
            }
            else
            {
                //if lastbottom is equal the node's Y, this node is hidden in it's parent
                if(lastbottom != node.Y)
                {
                    lastbottom = node.Y + node.Interval / 2;
                }
            }
        }


        public void RefreshByExtended()
        {

            var root = GetRoot() as GroupModel;

            double lastbottom = 0.0;

            foreach(var child in root.Children)
            {
                SetYPosition(child, ref lastbottom);
            }
        }


    }
}
