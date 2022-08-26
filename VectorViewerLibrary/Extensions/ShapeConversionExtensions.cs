using VectorViewerLibrary.Models;
using VectorViewerLibrary.ViewModels;

namespace VectorViewerLibrary.Extensions
{
    public static class ShapeConversionExtensions
    {
        public static IViewModel ConvertToViewModel(this ShapeModel model)
        {
            return model.Type switch
            {
                ShapeType.Line or
                ShapeType.Triangle => new LinearShapeViewModel(model),
                ShapeType.Circle => new CurvedShapeViewModel(model),
                ShapeType.Multi => new MultiShapeViewModel(model),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
