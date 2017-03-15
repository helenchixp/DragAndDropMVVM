/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Demo.Mr.Osomatsu"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;

namespace Demo.Mr.Osomatsu.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private static readonly object _current = new object();
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<TreeViewModel>();
            SimpleIoc.Default.Register<ListViewModel>();
            SimpleIoc.Default.Register<GroupViewModel>();


            Messenger.Default.Register<object>(_current, "DeleteLine", this.DeleteLine);
        }
        #region ViewModel
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        
        public TreeViewModel Tree
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TreeViewModel>();
            }
        }
        public ListViewModel List
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ListViewModel>();
            }
        }
        public GroupViewModel Group
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GroupViewModel>();
            }
        }
        #endregion

        #region Messenger Action Methods

        private void DeleteLine(object line)
        {

        }

        #endregion

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}