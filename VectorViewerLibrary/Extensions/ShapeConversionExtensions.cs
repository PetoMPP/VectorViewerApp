using VectorViewerLibrary.Models;
using VectorViewerLibrary.ViewModels;

namespace VectorViewerLibrary.Extensions
{
    public static class ShapeConversionExtensions
    {
        public static IShapeViewModel ConvertToViewModel(this ShapeModel model)
        {
            return model.Type switch
            {
                ShapeType.Line or
                ShapeType.Triangle => new LinearShapeViewModel(model),
                ShapeType.Circle => new CurvedShapeViewModel(model),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
