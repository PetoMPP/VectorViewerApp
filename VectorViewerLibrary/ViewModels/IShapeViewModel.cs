using System.Drawing;

namespace VectorViewerLibrary.ViewModels
{
    public interface IShapeViewModel : IViewModel
    {
        PointF[] Points { get; }
        LineType LineType { get; }
        bool? Filled { get; }
        Color Color { get; }
        new string DisplayName { get; }
    }
}
