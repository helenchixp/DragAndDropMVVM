using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Demo.Mr.Osomatsu.ViewModel
{
    public class ProfileModel : ViewModelBase, IClone<ProfileModel>
    {
        public int No { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Comment { get; set; }

        public int GroupNo { get; set; } = -1;

       // public IGroupModel Parent { get; set; }

        public ProfileModel Clone()
        {
            return (ProfileModel)MemberwiseClone();
        }
    }
}
