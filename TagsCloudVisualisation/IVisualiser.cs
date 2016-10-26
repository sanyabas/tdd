using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualisation
{
    public interface IVisualiser
    {
        Bitmap Visualise(List<RectangleF> layout);
        void Save(List<RectangleF> layout, string filename, ImageFormat format);
    }
}