using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DragAndDropMVVM.Controls;
using DragAndDropMVVM.Demo.ViewModel;

namespace DragAndDropMVVM.Demo.Controls
{
    public class StampLine : DrawLineThump
    {
        static StampLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StampLine), new FrameworkPropertyMetadata(typeof(StampLine)));
        }

        public StampLine() : base()
        {
            DataContext = new StampLineViewModel();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
