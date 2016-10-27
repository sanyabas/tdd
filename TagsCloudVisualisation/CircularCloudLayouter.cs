using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualisation
{
    public class CircularCloudLayouter : ILayouter
    {
        private readonly PointF center;
        private float radius;
        private List<RectangleF> rectangles;
        private const float Angle = (float)(-Math.PI / 4);
        private PointF PreviousPoint => rectangles.Count == 0 ? new PointF(0, 0) : rectangles[rectangles.Count - 1].Location;
        private PointF previousRadiusPoint;

        public List<RectangleF> GetLayout()
        {
            return rectangles;
        }

        public CircularCloudLayouter(PointF center)
        {
            this.center = center;
            rectangles = new List<RectangleF>();
        }

        public RectangleF PutNextRectangle(SizeF rectangleSize)
        {
            PointF placingPoint;
            if (rectangles.Count == 0)
            {
                radius = (float)(rectangleSize.Width / 2.0);
                var x0 = (float)(center.X - rectangleSize.Width / 2.0);
                var y0 = (float)(center.Y - rectangleSize.Height / 2.0);
                placingPoint = new PointF(x0, y0);
            }
            else if (rectangles.Count == 1)
            {
                placingPoint = new PointF(center.X + radius, PreviousPoint.Y);
                previousRadiusPoint = new PointF(center.X + radius, PreviousPoint.Y);
            }
            else
            {
                radius++;
                var rotated = RotateAroundCenter(previousRadiusPoint);
                var shifted = new PointF(rotated.X-center.X,rotated.Y-center.Y);
                var previousShifted = new PointF(previousRadiusPoint.X-center.X,previousRadiusPoint.Y-center.Y);
                var x = shifted.X;
                var y = shifted.Y;
                var x0 = 2 * x - shifted.X;
                var y0 = y;
                if (y < 0)
                {
                    y0 = previousShifted.Y - rectangleSize.Height;
                }
                placingPoint = new PointF(x0+center.X, y0+center.Y);
                x = (float)(x + Math.Sign(x) * Math.Sin(Math.PI/4) + center.X);
                y = (float)(y + Math.Sign(y) * Math.Sin(Math.PI/4) + center.Y);
                previousRadiusPoint = new PointF(x, y);
            }
            rectangles.Add(new RectangleF(placingPoint, rectangleSize));
            return rectangles[rectangles.Count - 1];
        }

        private PointF RotateAroundCenter(PointF point)
        {
            var movedPoint = new PointF(previousRadiusPoint.X - center.X, PreviousPoint.Y - center.Y);
            var x = (float)(movedPoint.X * Math.Cos(Angle) - movedPoint.Y * Math.Sin(Angle));
            var y = (float)(movedPoint.X * Math.Sin(Angle) + movedPoint.Y * Math.Cos(Angle));
            return new PointF(x+center.X,y+center.Y);
        }
    }
}