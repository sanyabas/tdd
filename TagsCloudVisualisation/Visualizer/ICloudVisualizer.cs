using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualisation.Visualizer
{
    public interface ICloudVisualizer
    {
        Bitmap Visualize(List<RectangleF> layout);
        void VisualizeAndSave(List<RectangleF> layout, string filename, ImageFormat format);
    }
}