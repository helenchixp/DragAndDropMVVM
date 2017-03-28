using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Mr.Osomatsu.ViewModel
{
    public interface IPosition
    {
        double X { get; set; }
        double Y { get; set; }
        double Interval { get; set; }
    }
}
