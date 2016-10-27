using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualisation
{
    public interface IVisualizer
    {
        Bitmap Visualise(List<RectangleF> layout);
        void VisualizeAndSave(List<RectangleF> layout, string filename, ImageFormat format);
    }
}