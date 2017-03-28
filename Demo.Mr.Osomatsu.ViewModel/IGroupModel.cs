using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Mr.Osomatsu.ViewModel
{
    public interface IGroupModel : ITreeItemModel
    {
        ObservableCollection<ITreeItemModel> Children { get; set; }

        bool IsExpanded { get; set; }

        bool IsSelected { get; set; }

    }
}
