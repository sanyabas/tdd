using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualisation.Layouter
{
    public interface ITagsCLoudLayouter
    {
        List<RectangleF> GetLayout();
    }
}