using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Mr.Osomatsu.ViewModel
{
    public class ProfileConnectorModel : ProfileModel, ITreeItemModel, IClone<ITreeItemModel>
    {
        public IGroupModel Parent { get; set; }

        public new ITreeItemModel Clone()
        {
            return (ProfileConnectorModel)base.Clone();
        }
    }
}
