using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorViewerLibrary.Extensions
{
    public static class ColorExtensions
    {
        public static Color ConvertToColor(this string input)
        {
            var colorChannels = input.Split(';');

            if (colorChannels?.Length != 4)
                throw new InvalidOperationException("Invalid color format!");

            return Color.FromArgb(
                int.Parse(colorChannels[0].Trim()),
                int.Parse(colorChannels[1].Trim()),
                int.Parse(colorChannels[2].Trim()),
                int.Parse(colorChannels[3].Trim()));
        }
    }
}
