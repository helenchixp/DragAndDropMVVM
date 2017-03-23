using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Mr.Osomatsu.ViewModel
{
    public interface ITreeItemModel
    {
        int No { get; set; }
        int GroupNo { get; set; }
        IGroupModel Parent { get; set; }

    }


}
