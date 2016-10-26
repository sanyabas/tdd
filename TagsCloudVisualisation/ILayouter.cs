using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualisation
{
    public interface ILayouter
    {
        List<RectangleF> GetLayout();
    }
}