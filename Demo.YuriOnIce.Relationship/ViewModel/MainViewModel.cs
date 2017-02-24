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
using System.Collections.Generic;
using DragAndDropMVVM;

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
        public object DraggedDataContext
        {
            get;

            set;
        }

        #endregion


        #region UndoRedo Properties

        protected Stack<UndoRedoManager> UndoStack { get; } = new Stack<UndoRedoManager>();

        protected Stack<UndoRedoManager> RedoStack { get; } = new Stack<UndoRedoManager>();

        protected bool _isUndoHandle = false;

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

        private RelationshipMap _layoutRelationshipMap = new RelationshipMap();

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

        #region IsSyncLayoutRelationshipMap
        /// <summary>
        /// The <see cref="IsSyncLayoutRelationshipMap" /> property's name.
        /// </summary>
        public const string IsSyncLayoutRelationshipMapPropertyName = "IsSyncLayoutRelationshipMap";

        private bool _isSyncLayoutRelationshipMap = false;

        /// <summary>
        /// Sets and gets the IsSyncLayoutRelationshipMap property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public bool IsSyncLayoutRelationshipMap
        {
            get
            {
                return _isSyncLayoutRelationshipMap;
            }

            set
            {
                if (_isSyncLayoutRelationshipMap == value)
                {
                    return;
                }

                var oldValue = _isSyncLayoutRelationshipMap;
                _isSyncLayoutRelationshipMap = value;
                RaisePropertyChanged(IsSyncLayoutRelationshipMapPropertyName, oldValue, value, true);
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
                if (DraggedDataContext != null && DraggedDataContext is DiagramModel)
                {
                    (parameter as DiagramViewModel).Name = (DraggedDataContext as DiagramModel).Name;
                    (parameter as DiagramViewModel).Detail = (DraggedDataContext as DiagramModel).Detail;
                    (parameter as DiagramViewModel).ImagePath = (DraggedDataContext as DiagramModel).ImagePath;
                    (parameter as DiagramViewModel).Index = (DraggedDataContext as DiagramModel).Index;

                }

                var redomemo = new Action<object>((obj) =>
                {
                    if (!Characters.Any(item => item.Index == (parameter as DiagramViewModel).Index))
                        Characters.Add(parameter as DiagramViewModel);
                });

                var undomemo = new Action<object>((obj) => ExecuteDeleteDiagramCommand(parameter));

                if (!_isUndoHandle)
                {
                    UndoStack.Push(new UndoRedoManager()
                    {
                        ActionComment = $"add the {(DraggedDataContext as DiagramModel)}",
                        RedoAction = redomemo,
                        UndoAction = undomemo,
                    });
                }
                else
                {
                    RedoStack.Push(new UndoRedoManager()
                    {
                        ActionComment = $"add the {(DraggedDataContext as DiagramModel)}",
                        RedoAction = undomemo,
                        UndoAction = redomemo,
                    });

                    _isUndoHandle = false;
                }

                redomemo(null);
            }
        }
       

        private bool CanExecuteDropCommand(object parameter)
        {
            if (DraggedDataContext is DiagramModel)
            {
                bool isexist = Characters.Any(item => item.Index == (DraggedDataContext as DiagramModel).Index);

                if (isexist)
                {
                    ErrorMessage = $"{(DraggedDataContext as DiagramModel).Name} is here!";
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

            if(DraggedDataContext!=null && DraggedDataContext is Tuple<object, object>)
            {
                var originvm = (DraggedDataContext as Tuple<object, object>).Item1 as IConnectionDiagramViewModel;
                var terminalvm = (DraggedDataContext as Tuple<object, object>).Item2 as IConnectionDiagramViewModel;

                if (originvm != null)
                {
                    linevm.OriginDiagramViewModel = originvm;
                }

                if (terminalvm != null)
                {
                    linevm.TerminalDiagramViewModel = terminalvm;
                }
            }

            var redomemo = new Action<object>((obj) =>
            {

                linevm.OriginDiagramViewModel.DepartureLinesViewModel.Add(linevm);
                linevm.TerminalDiagramViewModel.ArrivalLinesViewModel.Add(linevm);
                (linevm as LineViewModel).Comment = $"testtttt ";

            });

            var undomemo = new Action<object>((obj) => ExecuteDeleteLineCommand(linevm));
            if (!_isUndoHandle)
            {
                UndoStack.Push(new UndoRedoManager()
                {
                    ActionComment = $"add the line of {(linevm as LineViewModel).Comment}",
                    RedoAction = redomemo,
                    UndoAction = undomemo,
                });
            }
            else
            {
                RedoStack.Push(new UndoRedoManager()
                {
                    ActionComment = $"undo add the line of {(linevm as LineViewModel).Comment}",
                    RedoAction = undomemo,
                    UndoAction = redomemo,
                });
                _isUndoHandle = false;
            }
            redomemo(null);

        }

        private bool CanExecuteDropLineCommand(object parameter)
        {
            if (parameter == null || !(parameter is IConnectionLineViewModel)) return false;

            if (DraggedDataContext == null && !(DraggedDataContext is Tuple<object, object>)) return false;


            var dragobj = (DraggedDataContext as Tuple<object, object>).Item1 as IConnectionDiagramViewModel;
            var dropobj = (DraggedDataContext as Tuple<object, object>).Item2 as IConnectionDiagramViewModel;

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
            var redomemo = new Action<object>((obj) =>
            {
                linevm.OriginDiagramViewModel.DepartureLinesViewModel.Remove(linevm);
                linevm.TerminalDiagramViewModel.ArrivalLinesViewModel.Remove(linevm);
            });
            var undomemo = new Action<object>((obj) => ExecuteDropLineCommand(linevm));

            if (!_isUndoHandle)
            {
                UndoStack.Push(new UndoRedoManager()
                {
                    ActionComment = $"delete line {(parameter as LineViewModel).Comment}. ",
                    RedoAction = redomemo,
                    UndoAction = undomemo,
                });
            }
            else
            {
                RedoStack.Push(new UndoRedoManager()
                {
                    ActionComment = $"undo delete line {(parameter as LineViewModel).Comment}. ",
                    RedoAction = undomemo,
                    UndoAction = redomemo,
                });

                _isUndoHandle = false;
            }

            redomemo(null);
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
            var redomemo = new Action<object>((obj) =>  Characters.Remove(parameter as DiagramViewModel));

            var undomemo = new Action<object>((obj) => ExecuteDropCommand(parameter));

            if (!_isUndoHandle)
            {

                UndoStack.Push(new UndoRedoManager()
                {
                    ActionComment = $"delete diagram of {(parameter as DiagramViewModel).Name} . ",
                    RedoAction = redomemo,
                    UndoAction = undomemo,
                });
            }
            else
            {
                RedoStack.Push(new UndoRedoManager()
                {
                    ActionComment = $"undo delete diagram of {(parameter as DiagramViewModel).Name} . ",
                    RedoAction = undomemo,
                    UndoAction = redomemo,
                });

                _isUndoHandle = false;
            }

            redomemo(null);
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

                //request the newest the layout map
                IsSyncLayoutRelationshipMap = true;

                var map = LayoutRelationshipMap;

                map.Header = "it is relationship map.";
                map.Footer = "Yuri on Ice!!!";

                if (map == null) throw new NullReferenceException("LayoutRelationshipMap is Null");

       
                XmlSerializer serializer = new XmlSerializer(typeof(RelationshipMap));

                //ファイルを作る
                FileStream fs = new FileStream(path, FileMode.Create);
                //書き込み
                serializer.Serialize(fs, map);  //sclsはSampleClassのインスタンス名
                                                   //ファイルを閉じる
                fs.Close();

                // if don't set false, it will update the layoutmap by every action
                IsSyncLayoutRelationshipMap = false;
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
                            var charvm = (DiagramViewModel)character.DataContext;

                            if (character.Connectors != null && character.Connectors.Any())
                            {
                                charvm.DepartureLinesViewModel = new ObservableCollection<IConnectionLineViewModel>(
                                    from line in character.Connectors
                                    select new LineViewModel()
                                    {
                                        Comment = line.Comment,
                                        LineUUID = line.LineUUID,
                                        TerminalDiagramUUID = line.TerminalDiagramUUID,
                                    });
                            }
                            Characters.Add(charvm);
                        }

                        //set the line TerminalDiagramViewModel
                        foreach (var charvm in Characters)
                        {
                            var layoutchar = layout.Characters.FirstOrDefault(diagram => diagram.DiagramUUID == ((DiagramViewModel)charvm).Index.ToString());

                            foreach (var depatureline in charvm.DepartureLinesViewModel)
                            {

                                depatureline.TerminalDiagramViewModel = (from tervm in Characters
                                                                         where tervm.Index.ToString() == ((LineViewModel)depatureline).TerminalDiagramUUID
                                                                         select tervm).FirstOrDefault();
                                depatureline.OriginDiagramViewModel = charvm;

                                if (depatureline.TerminalDiagramViewModel != null)
                                    depatureline.TerminalDiagramViewModel.ArrivalLinesViewModel.Add(depatureline);

                                if (layoutchar.Connectors != null && layoutchar.Connectors.Any())
                                {
                                    var layoutline = layoutchar.Connectors.FirstOrDefault(line => line.LineUUID == depatureline.LineUUID);
                                    if (layoutline != null)
                                    {
                                        layoutline.DataContext = depatureline;
                                    }
                                }
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

        #region UndoCommand

        private RelayCommand<object> _undoCommand;

        /// <summary>
        /// Gets the UndoCommand.
        /// </summary>
        public RelayCommand<object> UndoCommand
        {
            get
            {
                return _undoCommand ?? (_undoCommand = new RelayCommand<object>(
                    ExecuteUndoCommand,
                    CanExecuteUndoCommand));
            }
        }

        private void ExecuteUndoCommand(object parameter)
        {
            var undo = UndoStack.Pop();
            _isUndoHandle = true;

            undo.UndoAction(undo);

        }

        private bool CanExecuteUndoCommand(object parameter)
        {
            return UndoStack.Any();
        }

        #endregion

        #region RedoCommand

        private RelayCommand<object> _redoCommand;

        /// <summary>
        /// Gets the RedoCommand.
        /// </summary>
        public RelayCommand<object> RedoCommand
        {
            get
            {
                return _redoCommand ?? (_redoCommand = new RelayCommand<object>(
                    ExecuteRedoCommand,
                    CanExecuteRedoCommand));
            }
        }

        private void ExecuteRedoCommand(object parameter)
        {
            var redo = RedoStack.Pop();
            _isUndoHandle = false;

            redo.RedoAction(redo);

        }

        private bool CanExecuteRedoCommand(object parameter)
        {
            return true;
        }
        #endregion

        #endregion

    }
}