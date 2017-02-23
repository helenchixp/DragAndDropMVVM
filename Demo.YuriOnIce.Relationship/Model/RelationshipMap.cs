using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Demo.YuriOnIce.Relationship.ViewModel;
using DragAndDropMVVM;
using DragAndDropMVVM.Model;
using DragAndDropMVVM.ViewModel;

namespace Demo.YuriOnIce.Relationship.Model
{
    [Serializable]
    public class RelationshipMap : IMapLayout
    {
        [XmlAttribute]
        public double Width { get; set; }
        [XmlAttribute]
        public double Height { get; set; }
        [XmlElement]
        public string Header { get; set; }
        [XmlElement]
        public string Footer { get; set; }
        [XmlIgnore]
        public Type DiagramLayoutType
        {
            get
            {
                return typeof(Character);
            }
        }
        [XmlIgnore]
        public Type LineLayouType
        {
            get
            {
                return typeof(Connector);
            }
        }
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
            set
            {
                if (value != null && value.Any())
                {
                    try
                    {
                        Characters = value.Cast<Character>().ToArray();
                    }
                    catch
                    {

                    }
                }

            }

        }

        [XmlElement]
        public Character[] Characters { get; set; }

      

        [Serializable]
        public class Character : IDiagramLayout
        {
            private string _name = string.Empty;
            [XmlAttribute]
            public string Name
            {
                get
                {
                    return string.IsNullOrWhiteSpace(_name) ? (_name = _dataContext?.Name) : _name;
                }
                set
                {
                    _name = value;
                }
            }
            private string _detail = string.Empty;
            [XmlAttribute]
            public string Detail
            {
                get
                {
                    return string.IsNullOrWhiteSpace(_detail) ? (_detail = _dataContext?.Detail) : _detail;
                }
                set
                {
                    _detail = value;
                }
            }
            private string _imagePath = string.Empty;
            [XmlAttribute]
            public string ImagePath
            {
                get
                {
                    return string.IsNullOrWhiteSpace(_imagePath) ? (_name = _dataContext?.ImagePath) : _imagePath;
                }
                set
                {
                    _imagePath = value;
                }
            }
            [XmlAttribute]
            public int Index
            {
                get
                {
                    int id = -1;
                    int.TryParse(DiagramUUID, out id);
                    return id;
                }
                set
                {

                    DiagramUUID = value.ToString();
                }
            }
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

                set
                {
                    if (value != null && value.Any())
                    {
                        try
                        {
                            Connectors = value.Cast<Connector>().ToArray();
                        }
                        catch
                        {

                        }
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
            public Type DiagramUIType
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
                get;
                set;
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


                    }
                    return _dataContext;
                }

                set
                {
                    _dataContext = value as DiagramViewModel;

                    if (_dataContext != null)
                    {
                    }
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
            public int ToIndex
            {
                get
                {
                    int toindex = -1;
                    int.TryParse(TerminalDiagramUUID, out toindex);
                    return toindex;
                }
                set
                {
                    TerminalDiagramUUID = value.ToString();
                }
            }

            private string _comment = string.Empty;
            [XmlElement]
            public string Comment
            {
                get
                {
                    return string.IsNullOrWhiteSpace(_comment) ? (_comment = _dataContext?.Comment) : _comment;
                }
                set
                {
                    _comment = value;
                }
            }

            #region ILineLayout
            private Type _lineType = null;

            [XmlIgnore]
            public Type LineUIType
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
                get;
                set;
            }

            private string _lineUUID = string.Empty;
            [XmlAttribute]
            public string LineUUID
            {
                get
                {
                    return string.IsNullOrWhiteSpace(_lineUUID) ? (_lineUUID = _dataContext?.LineUUID) : _lineUUID;
                }
                set
                {
                    _lineUUID = value;
                }
            }

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

                    if (_dataContext != null)
                    {

                    }
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
