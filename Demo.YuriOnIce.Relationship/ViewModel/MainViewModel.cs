using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml.Serialization;
using Demo.YuriOnIce.Relationship.Model;
using DragAndDropMVVM.ViewModel;
using DragAndDropMVVM.Extensions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Demo.YuriOnIce.Relationship.Controls;

namespace Demo.YuriOnIce.Relationship.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase, IDragged
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            PalletItems.Add(new DiagramModel()
            {
                ImagePath = "/Demo.YuriOnIce.Relationship.MvvmLight;component/ImagesResource/Yuri.png",
                Name = "Yuri",
                Detail = "He has a glass heart.",
                Index = 1,
            });
            PalletItems.Add(new DiagramModel()
            {
                ImagePath = "/Demo.YuriOnIce.Relationship.MvvmLight;component/ImagesResource/Victory.png",
                Name = "Victory",
                Detail = "He is Legend!!!",
                Index = 2,
            });
            PalletItems.Add(new DiagramModel()
            {
                ImagePath = "/Demo.YuriOnIce.Relationship.MvvmLight;component/ImagesResource/Yurio.png",
                Name = "Yurio",
                Detail = "He is Tigger(Cat?).",
                Index = 3,
            });
            PalletItems.Add(new DiagramModel()
            {
                ImagePath = "/Demo.YuriOnIce.Relationship.MvvmLight;component/ImagesResource/Maccachin.png",
                Name = "Maccachin",
                Detail = "Maccachin is dog.",
                Index = 4,
            });
            
        }

        #region IDragged Objects
        public object DraggedData
        {
            get;

            set;
        }

        #endregion


        #region PropertyChanged Properties

        #region PalletItems
        /// <summary>
        /// The <see cref="PalletItems" /> property's name.
        /// </summary>
        public const string PalletItemsPropertyName = "PalletItems";

        private ObservableCollection<DiagramModel> _palletItems = new ObservableCollection<DiagramModel>();

        /// <summary>
        /// Sets and gets the PalletItems property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ObservableCollection<DiagramModel> PalletItems
        {
            get
            {
                return _palletItems;
            }

            set
            {
                if (_palletItems == value)
                {
                    return;
                }

                var oldValue = _palletItems;
                _palletItems = value;
                RaisePropertyChanged(PalletItemsPropertyName, oldValue, value, true);
            }
        }
        #endregion

        #region Characters
        /// <summary>
        /// The <see cref="Characters" /> property's name.
        /// </summary>
        public const string CharactersPropertyName = "Characters";

        private ObservableCollection<DiagramViewModel> _characters = new ObservableCollection<DiagramViewModel>();

        /// <summary>
        /// Sets and gets the Characters property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ObservableCollection<DiagramViewModel> Characters
        {
            get
            {
                return _characters;
            }

            set
            {
                if (_characters == value)
                {
                    return;
                }

                var oldValue = _characters;
                _characters = value;
                RaisePropertyChanged(CharactersPropertyName, oldValue, value, true);
            }
        }

        #endregion

        #region ErrorMessage

        /// <summary>
        /// The <see cref="ErrorMessage" /> property's name.
        /// </summary>
        public const string ErrorMessagePropertyName = "ErrorMessage";

        private string _errorMessage = string.Empty;

        /// <summary>
        /// Sets and gets the ErrorMessage property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                if (_errorMessage == value)
                {
                    return;
                }

                var oldValue = _errorMessage;
                _errorMessage = value;
                RaisePropertyChanged(ErrorMessagePropertyName, oldValue, value, true);
            }
        }
        #endregion

        #region LayoutRelationshipMap

        /// <summary>
        /// The <see cref="LayoutRelationshipMap" /> property's name.
        /// </summary>
        public const string LayoutRelationshipMapPropertyName = "LayoutRelationshipMap";

        private RelationshipMap _layoutRelationshipMap = null;

        /// <summary>
        /// Sets and gets the LayoutRelationshipMap property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public RelationshipMap LayoutRelationshipMap
        {
            get
            {
                return _layoutRelationshipMap;
            }

            set
            {
                if (_layoutRelationshipMap == value)
                {
                    return;
                }

                var oldValue = _layoutRelationshipMap;
                _layoutRelationshipMap = value;
                RaisePropertyChanged(LayoutRelationshipMapPropertyName, oldValue, value, true);
            }
        }

        #endregion

        #endregion



        #region Commands

        #region DragCommand

        private RelayCommand<object> _dragCommand;

        /// <summary>
        /// Gets the DragCommand.
        /// </summary>
        public RelayCommand<object> DragCommand
        {
            get
            {
                return _dragCommand ?? (_dragCommand = new RelayCommand<object>(
                    ExecuteDragCommand,
                    CanExecuteDragCommand));
            }
        }

        private void ExecuteDragCommand(object parameter)
        {

        }

        private bool CanExecuteDragCommand(object parameter)
        {
            return true;
        }

        #endregion

        #region DropCommand


        private RelayCommand<object> _dropCommand;

        /// <summary>
        /// Gets the DropCommand.
        /// </summary>
        public RelayCommand<object> DropCommand
        {
            get
            {
                return _dropCommand ?? (_dropCommand = new RelayCommand<object>(
                    ExecuteDropCommand,
                    CanExecuteDropCommand));
            }
        }

        private void ExecuteDropCommand(object parameter)
        {

            if (parameter is DiagramViewModel)
            {
                if (DraggedData != null && DraggedData is DiagramModel)
                {
                    (parameter as DiagramViewModel).Name = (DraggedData as DiagramModel).Name;
                    (parameter as DiagramViewModel).Detail = (DraggedData as DiagramModel).Detail;
                    (parameter as DiagramViewModel).ImagePath = (DraggedData as DiagramModel).ImagePath;
                    (parameter as DiagramViewModel).Index = (DraggedData as DiagramModel).Index;

                    Characters.Add(parameter as DiagramViewModel);
                }
            }
        }

        private bool CanExecuteDropCommand(object parameter)
        {
            if (DraggedData is DiagramModel)
            {
                bool isexist = Characters.Any(item => item.Index == (DraggedData as DiagramModel).Index);

                if (isexist)
                {
                    ErrorMessage = $"{(DraggedData as DiagramModel).Name} is here!";
                    return false;
                }
            }
            ErrorMessage = string.Empty;
            return true;
        }
        #endregion

        #region DragLineCommand

        private RelayCommand<object> _dragLineCommand;

        /// <summary>
        /// Gets the DragLineCommand.
        /// </summary>
        public RelayCommand<object> DragLineCommand
        {
            get
            {
                return _dragLineCommand ?? (_dragLineCommand = new RelayCommand<object>(
                    ExecuteDragLineCommand,
                    CanExecuteDragLineCommand));
            }
        }

        private void ExecuteDragLineCommand(object parameter)
        {

        }

        private bool CanExecuteDragLineCommand(object parameter)
        {
            return true;
        }

        #endregion

        #region DropLineCommand
        private RelayCommand<object> _dropLineCommand;

        /// <summary>
        /// Gets the DropLineCommand.
        /// </summary>
        public RelayCommand<object> DropLineCommand
        {
            get
            {
                return  _dropLineCommand ?? ( _dropLineCommand = new RelayCommand<object>(
                    ExecuteDropLineCommand,
                    CanExecuteDropLineCommand));
            }
        }

        private void ExecuteDropLineCommand(object parameter)
        {
            if (parameter == null || !(parameter is IConnectionLineViewModel)) return;

            var linevm = (parameter as IConnectionLineViewModel);

            linevm.OriginDiagramViewModel.DepartureLinesViewModel.Add(linevm);
            linevm.TerminalDiagramViewModel.ArrivalLinesViewModel.Add(linevm);

            (linevm as LineViewModel).Comment = $"testtttt ";

        }

        private bool CanExecuteDropLineCommand(object parameter)
        {
            if (parameter == null || !(parameter is IConnectionLineViewModel)) return false;
            var dragobj = (parameter as IConnectionLineViewModel).OriginDiagramViewModel;
            var dropobj = (parameter as IConnectionLineViewModel).TerminalDiagramViewModel;

            if (dragobj == dropobj) return false;

            if (dropobj == null || dragobj == null) return false;

            if (dragobj.DepartureLinesViewModel != null && dragobj.DepartureLinesViewModel.Any())
            {
                foreach (var aline in dragobj.DepartureLinesViewModel)
                {
                    if (dropobj.Equals(aline.TerminalDiagramViewModel))
                    {
                        return false;
                    }

                }
            }

            if (dropobj.ArrivalLinesViewModel != null && dropobj.ArrivalLinesViewModel.Any())
            {
                foreach (var dline in dropobj.ArrivalLinesViewModel)
                {
                    if (dragobj.Equals(dline.OriginDiagramViewModel))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion

        #region DeleteLineCommand


        private RelayCommand<object> _deleteLineCommand;

        /// <summary>
        /// Gets the DeleteLineCommand.
        /// </summary>
        public RelayCommand<object> DeleteLineCommand
        {
            get
            {
                return _deleteLineCommand ?? (_deleteLineCommand = new RelayCommand<object>(
                    ExecuteDeleteLineCommand,
                    CanExecuteDeleteLineCommand));
            }
        }

        private void ExecuteDeleteLineCommand(object parameter)
        {
            if (parameter == null || !(parameter is IConnectionLineViewModel)) return;

            var linevm = parameter as IConnectionLineViewModel;

            linevm.OriginDiagramViewModel.DepartureLinesViewModel.Remove(linevm);
            linevm.TerminalDiagramViewModel.ArrivalLinesViewModel.Remove(linevm);
        }

        private bool CanExecuteDeleteLineCommand(object parameter)
        {
            return true;
        }
        #endregion

        #region DeleteDiagramCommand

        private RelayCommand<object> _deleteDiagramCommand;

        /// <summary>
        /// Gets the DeleteDiagramCommand.
        /// </summary>
        public RelayCommand<object> DeleteDiagramCommand
        {
            get
            {
                return _deleteDiagramCommand ?? (_deleteDiagramCommand = new RelayCommand<object>(
                    ExecuteDeleteDiagramCommand,
                    CanExecuteDeleteDiagramCommand));
            }
        }

        private void ExecuteDeleteDiagramCommand(object parameter)
        {

        }

        private bool CanExecuteDeleteDiagramCommand(object parameter)
        {
            if (parameter == null) return true;

            if (parameter is DiagramViewModel)
            {
                return (parameter as DiagramViewModel).ArrivalLinesViewModel.Count == 0
                     &&
                      (parameter as DiagramViewModel).DepartureLinesViewModel.Count == 0;
            }
            return true;
        }
        #endregion

        #region ExportImageCommand
        private RelayCommand<object> _exportImageCommand;

        /// <summary>
        /// Gets the ExportImageCommand.
        /// </summary>
        public RelayCommand<object> ExportImageCommand
        {
            get
            {
                return _exportImageCommand ?? (_exportImageCommand = new RelayCommand<object>(
                    ExecuteExportImageCommand,
                    CanExecuteExportImageCommand));
            }
        }

        private void ExecuteExportImageCommand(object parameter)
        {
            if (parameter is System.Windows.Controls.Canvas)
            {
                (parameter as System.Windows.Controls.Canvas).ExportImage();
            }
        }

        private bool CanExecuteExportImageCommand(object parameter)
        {
            return true;
        }

        #endregion

        #region SaveAsXMLCommand

        private RelayCommand<object> _saveAsXMLCommand;

        /// <summary>
        /// Gets the SaveAsXMLCommand.
        /// </summary>
        public RelayCommand<object> SaveAsXMLCommand
        {
            get
            {
                return _saveAsXMLCommand ?? (_saveAsXMLCommand = new RelayCommand<object>(
                    ExecuteSaveAsXMLCommand,
                    CanExecuteSaveAsXMLCommand));
            }
        }

        private void ExecuteSaveAsXMLCommand(object parameter)
        {
            if (!Characters.Any()) return;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FilterIndex = 1;

            //Extend the image type
            saveFileDialog.Filter = "XML File(.xml)|*.xml|All Files (*.*)|*.*";
            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                var path = saveFileDialog.FileName;

                //Create Serialize Object
                var map = new RelationshipMap()
                {
                    Width = 500,
                    Height = 500,
                    Header = "Relation Map!",
                    Footer = "Copyright",
                    Characters = new RelationshipMap.Character[Characters.Count],
                };

                int idx = 0;

                foreach (var charater in Characters)
                {
                    map.Characters[idx] = new RelationshipMap.Character()
                    {
                        Name = Characters[idx].Name,
                        ImagePath = Characters[idx].ImagePath,
                        Detail = Characters[idx].Detail,
                        Index = Characters[idx].Index,
                        DiagramTypeName = typeof(Controls.CharacterDiagram).FullName,
                    };
                    if (Characters[idx].DepartureLinesViewModel.Any())
                    {
                        map.Characters[idx].Connectors = new RelationshipMap.Connector[Characters[idx].DepartureLinesViewModel.Count];

                        int lineidx = 0;
                        foreach (var line in Characters[idx].DepartureLinesViewModel)
                        {
                            map.Characters[idx].Connectors[lineidx] = new RelationshipMap.Connector()
                            {
                                Comment = (line as LineViewModel)?.Comment,
                                ToIndex = line.TerminalDiagramViewModel?.Index ?? -1,
                                LineTypeName = typeof(Controls.RelationshipLine).FullName,
                            };

                            lineidx++;
                        }
                    }
                    idx++;
                }

                if( parameter is Canvas)
                {
                   // LayoutRelationshipMap = map;
                    (parameter as Canvas).SetExportPosition(map.Characters);
                }
                

                XmlSerializer serializer = new XmlSerializer(typeof(RelationshipMap));

                //ファイルを作る
                FileStream fs = new FileStream(path, FileMode.Create);
                //書き込み
                serializer.Serialize(fs, map);  //sclsはSampleClassのインスタンス名
                                                   //ファイルを閉じる
                fs.Close();


            }

        }

        private bool CanExecuteSaveAsXMLCommand(object parameter)
        {
            return true;
        }

        #endregion
        
        #region LoadXMLCommand

        private RelayCommand<object> _loadXMLCommand;

        /// <summary>
        /// Gets the LoadXMLCommand.
        /// </summary>
        public RelayCommand<object> LoadXMLCommand
        {
            get
            {
                return _loadXMLCommand ?? (_loadXMLCommand = new RelayCommand<object>(
                    ExecuteLoadXMLCommand,
                    CanExecuteLoadXMLCommand));
            }
        }

        private void ExecuteLoadXMLCommand(object parameter)
        {
            //if (!(parameter is System.Windows.Controls.Canvas))
            //    return;

            //var canvas = (parameter as System.Windows.Controls.Canvas);



            //canvas.Children.Clear();

            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog.Filter = "XML File(.xml)|*.xml|All Files (*.*)|*.*";

            bool? result = filedialog.ShowDialog();

            if (result == true)
            {
                var filepath = filedialog.FileName;
                var fs = new StreamReader(filepath, new UTF8Encoding(false));

                XmlSerializer serializer = new XmlSerializer(typeof(RelationshipMap));

                try
                {
                    var layout = serializer.Deserialize(fs) as RelationshipMap;

                    if (layout != null)
                    {

                        //Create the collection in viewmodel
                        Characters.Clear();
                        foreach (var character in layout.Characters)
                        {
                            Characters.Add((DiagramViewModel)character.DataContext);
                        }

                        //set the line TerminalDiagramViewModel
                        foreach (var charvm in Characters)
                        {
                            foreach (var depatureline in charvm.DepartureLinesViewModel)
                            {

                                depatureline.TerminalDiagramViewModel = (from tervm in Characters
                                                                         where tervm.Index.ToString() == ((LineViewModel)depatureline).TerminalDiagramUUID
                                                                         select tervm).FirstOrDefault();

                                if (depatureline.TerminalDiagramViewModel != null)
                                    depatureline.TerminalDiagramViewModel.ArrivalLinesViewModel.Add(depatureline);

                            }
                        }

                        


                        LayoutRelationshipMap = layout;
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }

        }

        private bool CanExecuteLoadXMLCommand(object parameter)
        {
            return true;
        }

        #endregion


        #region ClearCommand
        private RelayCommand<object> _clearCommand;

        /// <summary>
        /// Gets the ClearCommand.
        /// </summary>
        public RelayCommand<object> ClearCommand
        {
            get
            {
                return _clearCommand ?? (_clearCommand = new RelayCommand<object>(
                    ExecuteClearCommand,
                    CanExecuteClearCommand));
            }
        }

        private void ExecuteClearCommand(object parameter)
        {
            if (!(parameter is System.Windows.Controls.Canvas))
                return;

            var canvas = (parameter as System.Windows.Controls.Canvas);
            canvas.Children.Clear();
        }

        private bool CanExecuteClearCommand(object parameter)
        {
            return true;
        }
        #endregion 

        #endregion

    }
}