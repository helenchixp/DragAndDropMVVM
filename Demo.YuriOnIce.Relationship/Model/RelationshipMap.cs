using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Demo.YuriOnIce.Relationship.ViewModel;
using DragAndDropMVVM.Model;
using DragAndDropMVVM.ViewModel;

namespace Demo.YuriOnIce.Relationship.Model
{
    [Serializable]
    public class RelationshipMap : IMapLayout
    {
        [XmlAttribute]
        public int Width { get; set; }
        [XmlAttribute]
        public int Height { get; set; }
        [XmlElement]
        public string Header { get; set; }
        [XmlElement]
        public string Footer { get; set; }
        [XmlIgnore]
        public IDiagramLayout[] Diagrams
        {
            get

            {
                if (Characters != null && Characters.Any())
                {
                    return Characters.Cast<IDiagramLayout>().ToArray();
                }
                else
                {
                    return null;
                }
            }
        }
        //[XmlIgnore]
        //public bool IsSync { get; set; }

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

            [XmlIgnore]
            public string DiagramUUID
            {
                get
                {
                    return Index.ToString();
                }
                set { }
            }

            DiagramViewModel _dataContext = null;

            [XmlIgnore]
            public object DataContext
            {
                get
                {
                    if (_dataContext == null)
                    {
                        _dataContext = new DiagramViewModel()
                        {
                            Index = Index,
                            Name = Name,
                            Detail = Detail,
                            ImagePath = ImagePath,
                        };
                        if (Connectors != null && Connectors.Any())
                        {
                            _dataContext.DepartureLinesViewModel = new ObservableCollection<IConnectionLineViewModel>(
                                from line in Connectors
                                select new LineViewModel()
                                {
                                    Comment = line.Comment,
                                    OriginDiagramViewModel = _dataContext,
                                    TerminalDiagramUUID = line.TerminalDiagramUUID,
                                }
                            );
                        }

                    }
                    return _dataContext;
                }

                set
                {
                    _dataContext = value as DiagramViewModel;
                }
            }
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

            [XmlIgnore]
            public string TerminalDiagramUUID
            {
                get
                {
                    return ToIndex.ToString();
                }
                set { }
            }

            [XmlAttribute]
            public string LineUUID { get; set; }

            private LineViewModel _dataContext = null;

            [XmlIgnore]
            public object DataContext
            {
                get
                {
                    if (_dataContext == null)
                    {
                        _dataContext = new LineViewModel()
                        {
                            Comment = Comment,
                            LineUUID = LineUUID,
                        };
                    }
                    return _dataContext;
                }

                set
                {
                    _dataContext = value as LineViewModel;
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
