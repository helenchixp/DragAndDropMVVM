using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DragAndDropMVVM.Demo.Model;
using DragAndDropMVVM.ViewModel;
using DragAndDropMVVM.Extensions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Win32;
using System.Windows;
using System.Text;
using System.Windows.Controls;
using DragAndDropMVVM.Controls;
using System.Collections.Generic;
using System.Linq;

namespace DragAndDropMVVM.Demo.ViewModel
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
    public class MainViewModel : ViewModelBase, IDragged//, IDragable, IDropable
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

            YuriPalletItems.Add(new YuriModel()
            {
                ImagePath = "/DragAndDropMVVM.Demo;component/ImagesResource/yuri_01.png",
                Title = "Sorry!",
                Detail = "Apologize by Japanese Style",
                Index = 1,
            });
            YuriPalletItems.Add(new YuriModel()
            {
                ImagePath = "/DragAndDropMVVM.Demo;component/ImagesResource/yuri_02.png",
                Title = "Really?",
                Detail = "I Can't believe it! But it is really!!!",
                Index = 2,
            });
            YuriPalletItems.Add(new YuriModel()
            {
                ImagePath = "/DragAndDropMVVM.Demo;component/ImagesResource/yuri_03.png",
                Title = "Wow!",
                Detail = "I'm Maccachin.",
                Index = 3,
            });
        }

        public ObservableCollection<YuriModel> YuriPalletItems { get; private set; } = new ObservableCollection<YuriModel>();


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
         //   System.Diagnostics.Debug.WriteLine($"The DragCommandParameter is {(parameter ?? "null").ToString()}");
        }

        private bool CanExecuteDragCommand(object parameter)
        {
            return true;
        }


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
            if(parameter is StampDiagramViewModel)
            {
                (parameter as StampDiagramViewModel).Index = DroppedItemSource.Count;
                DroppedItemSource.Add(parameter as StampDiagramViewModel);
            }

            if (parameter is YuriDiagramViewModel)
            {
                if (DraggedDataContext != null && DraggedDataContext is YuriModel)
                {
                    (parameter as YuriDiagramViewModel).Title = (DraggedDataContext as YuriModel).Title;
                    (parameter as YuriDiagramViewModel).Detail = (DraggedDataContext as YuriModel).Detail;
                    (parameter as YuriDiagramViewModel).ImagePath = (DraggedDataContext as YuriModel).ImagePath;
                    (parameter as YuriDiagramViewModel).IconType = (DraggedDataContext as YuriModel).Index;
                }
            }
        }

        private bool CanExecuteDropCommand(object parameter)
        {
            return true;
        }

        //ObservableCollection

        /// <summary>
        /// The <see cref="DroppedItemSource" /> property's name.
        /// </summary>
        public const string DroppedItemSourcePropertyName = "DroppedItemSource";

        private ObservableCollection<StampDiagramViewModel> _droppedItemSource = new ObservableCollection<StampDiagramViewModel>();

        /// <summary>
        /// Sets and gets the DroppedItemSource property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ObservableCollection<StampDiagramViewModel> DroppedItemSource
        {
            get
            {
                return _droppedItemSource;
            }

            set
            {
                if (_droppedItemSource == value)
                {
                    return;
                }

                var oldValue = _droppedItemSource;
                _droppedItemSource = value;
                RaisePropertyChanged(DroppedItemSourcePropertyName, oldValue, value, true);
            }
        }


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


        private RelayCommand<object> _dropLineCommand;

        /// <summary>
        /// Gets the DropLineCommand.
        /// </summary>
        public RelayCommand<object> DropLineCommand
        {
            get
            {
                return _dropLineCommand ?? (_dropLineCommand = new RelayCommand<object>(
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
        }

        private bool CanExecuteDropLineCommand(object parameter)
        {
            ////var ddobj = parameter as Tuple<object, object>;

            ////if (ddobj == null) return false;

            ////var dragobj = ddobj.Item1 as IConnectionDiagramViewModel;
            ////var dropobj = ddobj.Item2 as IConnectionDiagramViewModel;

            if (parameter == null || !(parameter is IConnectionLineViewModel)) return false;
            var dragobj = (parameter as IConnectionLineViewModel).OriginDiagramViewModel;
            var dropobj = (parameter as IConnectionLineViewModel).TerminalDiagramViewModel;

            if (dragobj == dropobj) return false;

            if (dropobj == null || dragobj == null) return false;

            if (dragobj.ArrivalLinesViewModel != null)
            {
                foreach (var aline in dragobj.ArrivalLinesViewModel)
                {
                    if(dropobj.Equals(aline.TerminalDiagramViewModel))
                    {
                        return false;
                    }

                }
            }

            if(dropobj.DepartureLinesViewModel != null)
            {
                foreach(var dline in dropobj.DepartureLinesViewModel)
                {
                    if(dragobj.Equals(dline.OriginDiagramViewModel))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public object DraggedDataContext
        {
            get;

            set;
        }

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
            if (parameter == null || !(parameter is IConnectionLineViewModel) ) return;

            var linevm = parameter as IConnectionLineViewModel;

            linevm.OriginDiagramViewModel.DepartureLinesViewModel.Remove(linevm);
            linevm.TerminalDiagramViewModel.ArrivalLinesViewModel.Remove(linevm);
        }

        private bool CanExecuteDeleteLineCommand(object parameter)
        {
            return true;
        }



        private RelayCommand<object> _deleteDiagramCommand;

        /// <summary>
        /// Gets the MyCommand.
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

            if(parameter is YuriDiagramViewModel)
            {
                return (parameter as YuriDiagramViewModel).ArrivalLinesViewModel.Count == 0
                     &&
                      (parameter as YuriDiagramViewModel).DepartureLinesViewModel.Count == 0;
            }

            return true;
        }

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
            if(parameter is System.Windows.Controls.Canvas)
            {
                (parameter as System.Windows.Controls.Canvas).ExportImage();
            }
        }

        private bool CanExecuteExportImageCommand(object parameter)
        {
            return true;
        }


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
            if (parameter is System.Windows.Controls.Canvas)
            {
                SaveAsXML((parameter as System.Windows.Controls.Canvas));
            }
        }

        private bool CanExecuteSaveAsXMLCommand(object parameter)
        {
            return true;
        }

        private RelayCommand<object> _loadXMLCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand<object> LoadXMLCommand
        {
            get
            {
                return _loadXMLCommand ?? (_loadXMLCommand = new RelayCommand<object>(
                    ExecuteMyCommand,
                    CanExecuteMyCommand));
            }
        }

        private void ExecuteMyCommand(object parameter)
        {
            if (parameter is System.Windows.Controls.Canvas)
            {
                LoadXML((parameter as System.Windows.Controls.Canvas));
            }
        }

        private bool CanExecuteMyCommand(object parameter)
        {
            return true;
        }


        #region 

        #region SaveAsXML
        public static void SaveAsXML(Canvas canvas)
        {
            if (canvas.Children == null || canvas.Children.Count == 0)
                return;


            ControlsLayout layout = new ControlsLayout();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FilterIndex = 1;

            //Extend the image type
            saveFileDialog.Filter = "XML File(.xml)|*.xml|All Files (*.*)|*.*";
            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                var path = saveFileDialog.FileName;
                int idxline, idxdiagram;
                idxline = idxdiagram = 0;

                Dictionary<int, ConnectionDiagramBase> tempdiagrams = new Dictionary<int, ConnectionDiagramBase>();


                //create the layout object
                foreach (var child in canvas.Children)
                {
                    if (child is ConnectionDiagramBase)
                    {
                        var diagl = ControlsLayout.CreateDiagramLayout(child as ConnectionDiagramBase, idxline++);
                        tempdiagrams.Add(idxline, child as ConnectionDiagramBase);
                        layout.Diagrams.Add(diagl);
                    }
                }


                foreach (var child in canvas.Children)
                {
                    if (child is ConnectionLineBase)
                    {
                        var linec = child as ConnectionLineBase;
                        var linel = ControlsLayout.CreateLineLayout(linec, idxdiagram++);
                        if (linec.OriginDiagram != null)
                        {
                            linel.OriginDiagramID = tempdiagrams.FirstOrDefault(item => item.Value.Equals(linec.OriginDiagram)).Key;
                        }
                        if (linec.TerminalDiagram != null)
                        {
                            linel.TerminalDiagramID = tempdiagrams.FirstOrDefault(item => item.Value.Equals(linec.TerminalDiagram)).Key;
                        }
                        layout.Lines.Add(linel);
                    }
                }

                XmlSerializer serializer = new XmlSerializer(typeof(ControlsLayout));

                //ファイルを作る
                FileStream fs = new FileStream(path, FileMode.Create);
                //書き込み
                serializer.Serialize(fs, layout);  //sclsはSampleClassのインスタンス名
                                                   //ファイルを閉じる
                fs.Close();
            }
        }

        #endregion


        #region LoadXML
        public void LoadXML(Canvas canvas)
        {
            canvas.Children.Clear();

            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog.Filter = "XML File(.xml)|*.xml|All Files (*.*)|*.*";

            bool? result = filedialog.ShowDialog();

            if (result == true)
            {
                var filepath = filedialog.FileName;
                var fs = new StreamReader(filepath, new UTF8Encoding(false));

                XmlSerializer serializer = new XmlSerializer(typeof(ControlsLayout));

                try
                {
                    var layout = serializer.Deserialize(fs) as ControlsLayout;

                    if (layout != null)
                    {
                        foreach (var diagram in layout.Diagrams)
                        {
                            //Activator.CreateComInstanceFrom()
                        }

                        foreach (var line in layout.Lines)
                        {
                            //Activator.CreateComInstanceFrom()
                        }

                        MessageBox.Show("You will load the xml in you project");

                    }
                }
                catch
                {

                }
                finally
                {
                    fs.Close();
                }
            }
        }


        #endregion

        #endregion
    }

}