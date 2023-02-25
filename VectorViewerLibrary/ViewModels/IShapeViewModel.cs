using System.Drawing;

namespace VectorViewerLibrary.ViewModels
{
    public interface IShapeViewModel : IViewModel
    {
        PointF[] Points { get; }
        LineType LineType { get; }
        bool Visible { get; }
        bool Filled { get; }
        Color Color { get; }
        new string DisplayName { get; }

        bool IsPointOnShape(PointF point, float tolerance = 0.25F);
        float GetDistanceToShape(PointF point);
        void Scale(float factor);
        bool IsRectangleIntersecting(RectangleF rectangle);
        bool IsContainedInRectangle(RectangleF rectangle);
    }
}
