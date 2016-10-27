using System.Drawing;

namespace TagsCloudVisualisation
{
    public static class PointFExtensions
    {
        public static PointF Add(this PointF point, PointF another)
        {
            return new PointF(point.X+another.X,point.Y+another.Y);
        }

        public static PointF Sub(this PointF point, PointF another)
        {
            return new PointF(point.X-another.X,point.Y-another.Y);
        }
    }
}