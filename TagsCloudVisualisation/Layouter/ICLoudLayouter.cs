using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualisation.Layouter
{
    public interface ICLoudLayouter
    {
        List<RectangleF> GetLayout();

        RectangleF PutNextRectangle(SizeF rectangleSize);

        PointF GetCenter();
    }
}