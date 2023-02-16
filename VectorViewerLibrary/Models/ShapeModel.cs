using System.Drawing;

namespace VectorViewerLibrary.Models
{
    public class ShapeModel
    {
        public ShapeType Type { get; set; }
        public LineType LineType { get; set; }
        public PointF[]? Points { get; set; }
        public PointF? Center { get; set; }
        public float? Radius { get; set; }
        public float? ArcStart { get; set; }
        public float? ArcEnd { get; set; }
        public bool? Filled { get; set; }
        public Color? Color { get; set; }
        public ShapeModel[]? Shapes { get; set; }
    }
}
