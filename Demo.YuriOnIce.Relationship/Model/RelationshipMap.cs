using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DragAndDropMVVM.Model;

namespace Demo.YuriOnIce.Relationship.Model
{
    [Serializable]
    public class RelationshipMap
    {
        [XmlAttribute]
        public int Width { get; set; }
        [XmlAttribute]
        public int Height { get; set; }
        [XmlElement]
        public string Header { get; set; }
        [XmlElement]
        public string Footer { get; set; }

        [XmlElement]
        public Character[] Characters { get; set; }

        [Serializable]
        public class Character : IDiagramLayout
        {


            [XmlAttribute]
            public string Name { get; set; }
            [XmlAttribute]
            public string Detail { get; set; }

            [XmlAttribute]
            public string ImagePath { get; set; }
            [XmlAttribute]
            public int Index { get; set; }
            [XmlElement]
            public Connector[] Connectors { get; set; }

            [XmlIgnore]
            public ILineLayout[] DepartureLines
            {
                get

                {
                    if (Connectors != null && Connectors.Any())
                    {
                        return Connectors.Cast<ILineLayout>().ToArray();
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            #region IDiagramLayout

            [XmlAttribute]
            public double X { get; set; }
            [XmlAttribute]
            public double Y { get; set; }


            private Type _diagramTypeType = null;
            [XmlIgnore]
            public Type DiagramType
            {
                get
                {
                    if (_diagramTypeType == null && !string.IsNullOrWhiteSpace(DiagramTypeName))
                    {
                        _diagramTypeType = Type.GetType(DiagramTypeName);
                    }
                    return _diagramTypeType;
                }

                set
                {
                    if (_diagramTypeType == value)
                        return;

                    _diagramTypeType = value;

                    DiagramTypeName = _diagramTypeType.FullName;
                }
            }

            [XmlElement]
            public string DiagramUUID { get; set; }
            #endregion

            [XmlAttribute]
            public string DiagramTypeName
            {
                get; set;
            }

        }

        [Serializable]
        public class Connector : ILineLayout
        {
            [XmlAttribute]
            public int ToIndex { get; set; } = -1;
            //[XmlElement]
            [XmlElement]
            public string Comment { get; set; }

            #region ILineLayout
            private Type _lineType = null;

            [XmlIgnore]
            public Type LineType
            {
                get
                {
                    if (_lineType == null && !string.IsNullOrWhiteSpace(LineTypeName))
                    {
                        _lineType = Type.GetType(LineTypeName);
                    }
                    return _lineType;
                }

                set
                {
                    if (_lineType == value)
                        return;

                    _lineType = value;

                    LineTypeName = _lineType.FullName;

                }
            }
            #endregion

            [XmlAttribute]
            public string LineTypeName
            {
                get;set;
            }
        }
    }


}
