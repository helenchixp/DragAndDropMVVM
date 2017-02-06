using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;
using DragAndDropMVVM.Controls;

namespace DragAndDropMVVM.Demo.Model
{
    [Serializable]
    public class ControlsLayout
    {
        public ObservableCollection<LineLayout> Lines { get; set; } = new ObservableCollection<LineLayout>();

        public ObservableCollection<DiagramLayout> Diagrams { get; set; } = new ObservableCollection<DiagramLayout>();

        [Serializable]
        public class DiagramLayout
        {
            [XmlAttribute]
            public int Index { get; set; }
            [XmlAttribute]
            public string TypeName { get; set; }
            [XmlAttribute]
            public double Top { get; set; }
            [XmlAttribute]
            public double Left { get; set; }
            [XmlAttribute]
            public ConnectorPositionType PositionType { get; set; }
            [XmlIgnore]
            public object DataContext { get; set; }
        }

        [Serializable]
        public class LineLayout
        {
            [XmlAttribute]
            public int Index { get; set; }
            [XmlAttribute]
            public string TypeName { get; set; }
            [XmlAttribute]
            public double Top { get; set; }
            [XmlAttribute]
            public double Left { get; set; }
            [XmlIgnore]
            public object DataContext { get; set; }
            [XmlAttribute]
            public int OriginDiagramID { get; set; }
            [XmlAttribute]
            public int TerminalDiagramID { get; set; }

        }

        internal static LineLayout CreateLineLayout(ConnectionLineBase line, int index = 0)
        {
            if (line == null)
                return null;

            return new LineLayout()
            {
                TypeName = line.GetType().FullName,
                Top = (double)line.GetValue(Canvas.TopProperty),
                Left = (double)line.GetValue(Canvas.LeftProperty),
                DataContext = line.DataContext,
                Index = index,
            };
        }

        internal static DiagramLayout CreateDiagramLayout(ConnectionDiagramBase diagram, int index = 0)
        {
            if (diagram == null) return null;
            return new DiagramLayout()
            {
                TypeName = diagram.GetType().FullName,
                Top = (double)diagram.GetValue(Canvas.TopProperty),
                Left = (double)diagram.GetValue(Canvas.LeftProperty),
                PositionType = diagram.ConnectorPositionType,
                DataContext = diagram.DataContext,
                Index = index,
            };
        
        }
    }
}
