using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using DragAndDropMVVM.Behavior;
using DragAndDropMVVM.Controls;
using DragAndDropMVVM.Demo.ViewModel;

namespace DragAndDropMVVM.Demo.Controls
{
    public class StampDiagram : ConnectionDiagramBase
    {


        public StampDiagram() : base()
        {
            DataContext = new StampDiagramViewModel();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();


            BehaviorCollection bhcol = Interaction.GetBehaviors(this);
            //bhcol.Add(new FrameworkElementDragBehavior());
            bhcol.Add(new DrawLineDragBehavior());

            bhcol.Add(new DrawLineDropBehavior());

        }

    }
}
