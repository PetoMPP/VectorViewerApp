using System.Drawing;
using System.Numerics;

namespace VectorViewerLibrary.Models
{
    public class ShapeModel
    {
        public ShapeType Type { get; set; }
        public LineType LineType { get; set; }
        public bool Visible { get; set; }
        public PointF[]? Points { get; set; }
        public PointF? Center { get; set; }
        public float? Radius { get; set; }
        public float? ArcStart { get; set; }
        public float? ArcEnd { get; set; }
        public bool? Filled { get; set; }
        public Color? Color { get; set; }
        public ShapeModel[]? Shapes { get; set; }

        public void Offset(Vector2 offset)
        {
            if (offset == Vector2.Zero)
                return;

            if (Points is PointF[] points)
            {
                for (int i = 0; i < points.Length; i++)
                    Points[i] = (PointF)(points[i].ToVector2() + offset);
            }

            if (Center is PointF center)
                Center = (PointF)(center.ToVector2() + offset);

            if (Shapes?.Any() == true)
            {
                foreach (var shape in Shapes)
                    shape.Offset(offset);
            }
        }
    }
}
