using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demo.YuriOnIce.Relationship.MvvmLight
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

#if PRISM
            this.Title = "It is used Prism.";
            this.DataContext = new ViewModel.MainViewModel();
#else
            this.Title = "It is used MVVMLight.";
            this.DataContext = (App.Current.Resources["Locator"] as ViewModel.ViewModelLocator)?.Main;
#endif
        }
    }
}
