using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VectorViewerLibrary.Models;

namespace VectorViewerLibrary.DataReading
{
    public class JsonFileParser : IFileParser
    {
        private readonly JsonSerializerSettings _jsonSerializerSetting;
        private readonly string[] _pointNames;

        public JsonFileParser(JsonSerializerSettings jsonSerializerSetting)
        {
            _pointNames = GetPointNames();
            _jsonSerializerSetting = jsonSerializerSetting;
        }

        public async Task<IEnumerable<ShapeModel>> GetModelsFromFile(
            string path, CancellationToken cancellationToken)
        {
            var rawData = await File.ReadAllTextAsync(path, cancellationToken);

            var jArray = JsonConvert.DeserializeObject<JArray>(rawData);

            if (jArray is null)
                throw new InvalidOperationException("Invalid JSON");

            var result = new List<ShapeModel>();

            foreach (JObject shapeObject in jArray)
            {
                var shapeModel = GetShapeModel(shapeObject);
                result.Add(shapeModel);
            }

            return result;
        }

        private ShapeModel GetShapeModel(JObject shapeObject)
        {
            var points = new List<string>();
            var innerShapes = new List<ShapeModel>();

            if (shapeObject.TryGetValue("shapes", out var shapeToken) &&
                shapeToken is JArray shapesArray)
                foreach (JObject shape in shapesArray.Where(t => t is JObject))
                    innerShapes.Add(GetShapeModel(shape));

            foreach (var name in _pointNames)
            {
                if (!shapeObject.TryGetValue(name, out var token))
                    break;

                points.Add(
                    token.Value<string>()
                    ?? throw new InvalidOperationException("Invalid JSON"));
            }

            var shapeModel = shapeObject.ToObject<ShapeModel>(
                JsonSerializer.Create(_jsonSerializerSetting));

            if (shapeModel is null)
                throw new InvalidOperationException("Invalid JSON");

            shapeModel.Points = points.ToArray();
            shapeModel.Shapes = innerShapes.ToArray();
            return shapeModel;
        }

        private static string[] GetPointNames()
        {
            // (int)'a' = 97; (int)'z' = 122;
            var arrayStart = 97;
            var arrayEnd = 123;
            var result = new string[arrayEnd - arrayStart];

            for (int i = 97; i < 123; i++)
                result[i - arrayStart] = ((char)i).ToString();

            return result;
        }
    }
}
