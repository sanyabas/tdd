using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualisation.Visualizer
{
    public class CircularCloudVisualizer : ICloudVisualizer
    {
        public Bitmap Visualize(List<RectangleF> layout)
        {
            var bitmap = new Bitmap(800, 600);
            var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Black);
            graphics.DrawRectangles(new Pen(Color.Brown, 3), layout.ToArray());
            return bitmap;
        }

        public void VisualizeAndSave(List<RectangleF> layout, string fileFullName, ImageFormat format)
        {
            using (var bitmap = Visualize(layout))
                bitmap.Save(fileFullName, format);
        }
    }
}