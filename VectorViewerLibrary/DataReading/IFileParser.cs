using VectorViewerLibrary.Models;

namespace VectorViewerLibrary.DataReading
{
    public interface IFileParser
    {
        Task<IEnumerable<ShapeModel>> GetModelsFromFile(
            string path, CancellationToken cancellationToken);
    }
}