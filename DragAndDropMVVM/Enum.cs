using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragAndDropMVVM
{
    public enum DroppingElementAdornerType
    {
        DrawEllipse,
        DrawLine,
        Custom,
    }

    public enum ConnectorPositionType
    {
        Custom,
        Center,
        Left,
        Right,
        Top,
        Bottom,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
    }
}
