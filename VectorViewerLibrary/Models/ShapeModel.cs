namespace VectorViewerLibrary.Models
{
    public class ShapeModel
    {
        public ShapeType Type { get; set; }
        public string[]? Points { get; set; }
        public string? Center { get; set; }
        public float? Radius { get; set; }
        public bool? Filled { get; set; }
        public string? Color { get; set; }
    }
}
