using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorViewerLibrary.DataReading.Converters
{
    public class PointFConverter : JsonConverter<PointF?>
    {
        public override PointF? ReadJson(
            JsonReader reader,
            Type objectType,
            PointF? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            var value = (string?)reader.Value;
            if (value is null)
                return null;

            var values = value.Split(';');
            return new PointF(
                float.Parse(
                    values[0].Trim().Replace(',', '.'),
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture),
                -float.Parse(
                    values[1].Trim().Replace(',', '.'),
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture));
        }

        public override void WriteJson(JsonWriter writer, PointF? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
