using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualisation.Visualizer
{
    public interface ITagsCloudVisualizer
    {
        Bitmap Visualise(List<RectangleF> layout);
        void VisualizeAndSave(List<RectangleF> layout, string filename, ImageFormat format);
    }
}