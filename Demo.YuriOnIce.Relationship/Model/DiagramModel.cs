using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.YuriOnIce.Relationship.Model
{
    [Serializable]
    public class DiagramModel
    {
        public string ImagePath { get; set; }

        public string Title { get; set; }

        public string Detail { get; set; }

        public int Index { get; set; }

    }
}
