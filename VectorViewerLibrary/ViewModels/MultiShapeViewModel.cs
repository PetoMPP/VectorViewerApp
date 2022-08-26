using VectorViewerLibrary.Extensions;
using VectorViewerLibrary.Models;

namespace VectorViewerLibrary.ViewModels
{
    public interface IMultiShapeViewModel : IViewModel
    {
        List<IShapeViewModel> InnerShapes { get; }
    }

    public class MultiShapeViewModel : IMultiShapeViewModel
    {
        public string DisplayName { get; }
        public List<IShapeViewModel> InnerShapes { get; }

        public MultiShapeViewModel(ShapeModel model)
        {
            DisplayName = model.Type.ToString();
            InnerShapes = GetShapesFromModel(model);
        }

        private List<IShapeViewModel> GetShapesFromModel(ShapeModel model)
        {
            var result = new List<IShapeViewModel>();

            if (model.Shapes is null)
                throw new ArgumentNullException(nameof(model));

            foreach (var shape in model.Shapes)
            {
                var viewModel = shape.ConvertToViewModel();
                if (viewModel is IShapeViewModel shapeViewModel)
                    result.Add(shapeViewModel);
                if (viewModel is IMultiShapeViewModel)
                    result.AddRange(GetShapesFromModel(shape));
            }

            return result;
        }
    }
}
