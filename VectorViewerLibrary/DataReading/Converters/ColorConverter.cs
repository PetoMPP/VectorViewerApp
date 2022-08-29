using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorViewerLibrary.DataReading.Converters
{
    public class ColorConverter : JsonConverter<Color?>
    {
        public override Color? ReadJson(
            JsonReader reader,
            Type objectType,
            Color? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            var value = (string?)reader.Value;
            if (value is null)
                return null;

            var colorChannels = value.Split(';');

            if (colorChannels?.Length != 4)
                throw new InvalidOperationException("Invalid color format!");

            return Color.FromArgb(
                int.Parse(colorChannels[0].Trim()),
                int.Parse(colorChannels[1].Trim()),
                int.Parse(colorChannels[2].Trim()),
                int.Parse(colorChannels[3].Trim()));
        }

        public override void WriteJson(
            JsonWriter writer, Color? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
