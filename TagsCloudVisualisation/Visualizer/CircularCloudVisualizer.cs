using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudVisualisation.Layouter;

namespace TagsCloudVisualisation.Visualizer
{
    public class CircularCloudVisualizer : ICloudVisualizer
    {
        private static readonly Color VisualizationColor = Color.Black;
        private static readonly Pen VisualizationPen = new Pen(Color.Brown, 3);
        private readonly int bitmapWidth;
        private readonly int bitmapHeight;

        public CircularCloudVisualizer(ICLoudLayouter layouter)
        {
            bitmapWidth = (int)(layouter.GetCenter().X * 2);
            bitmapHeight = (int)(layouter.GetCenter().Y * 2);
        }

        public Bitmap Visualize(List<RectangleF> layout)
        {
            var bitmap = new Bitmap(bitmapWidth, bitmapHeight);
            var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(VisualizationColor);
            graphics.DrawRectangles(VisualizationPen, layout.ToArray());
            return bitmap;
        }

        public void VisualizeAndSave(List<RectangleF> layout, string fileFullName, ImageFormat format)
        {
            using (var bitmap = Visualize(layout))
                bitmap.Save(fileFullName, format);
        }
    }
}