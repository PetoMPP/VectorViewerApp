using Autofac.Features.Indexed;
using VectorViewerLibrary.Extensions;
using VectorViewerLibrary.ViewModels;

namespace VectorViewerLibrary.DataReading
{
    public interface IDataProvider
    {
        Task<IEnumerable<IShapeViewModel>> GetShapesFromFile(
            string path, CancellationToken cancellationToken);
    }

    public class DataProvider : IDataProvider
    {
        private readonly IIndex<string, IFileParser> _fileParsers;

        public DataProvider(IIndex<string, IFileParser> fileParsers)
        {
            _fileParsers = fileParsers;
        }

        public async Task<IEnumerable<IShapeViewModel>> GetShapesFromFile(
            string path, CancellationToken cancellationToken)
        {
            var parser = _fileParsers[Path.GetExtension(path)[1..].ToUpper()];

            if (parser is null)
                throw new NotSupportedException("Unsupported file type.");

            var rawShapes = await parser
                .GetModelsFromFile(path, cancellationToken);

            return rawShapes.Select(s => s.ConvertToViewModel());
        }
    }
}
