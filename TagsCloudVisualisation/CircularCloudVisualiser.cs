using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualisation
{
    public class CircularCloudVisualiser : IVisualiser
    {
        public Bitmap Visualise(List<RectangleF> layout)
        {
            var bitmap = new Bitmap(800, 600);
            var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Black);
            graphics.DrawRectangles(new Pen(Color.Brown, 3), layout.ToArray());
            return bitmap;
        }

        public void Save(List<RectangleF> layout, string fileFullName, ImageFormat format)
        {
            var bitmap = Visualise(layout);
            bitmap.Save(fileFullName, format);
        }
    }
}