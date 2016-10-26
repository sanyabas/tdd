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
            if (rectangles.Count == 0)
            {
                radius = (float)(rectangleSize.Width / 2.0);
                var x0 = (float)(center.X - rectangleSize.Width / 2.0);
                var y0 = (float)(center.Y - rectangleSize.Height / 2.0);
                rectangles.Add(new RectangleF(new PointF(x0, y0), rectangleSize));
                return rectangles[rectangles.Count - 1];
            }
            else if (rectangles.Count == 1)
            {
                rectangles.Add(new RectangleF(new PointF(center.X + radius, PreviousPoint.Y), rectangleSize));
                previousRadiusPoint = new PointF(center.X + radius, PreviousPoint.Y);
                return rectangles[rectangles.Count - 1];
            }
            radius++;
            //var temp = rectangles[rectangles.Count - 1];
            //var x = (float)(temp.X * (1 - Math.Cos(Math.PI / 4)));
            //var y = (float)(-temp.Y * Math.Cos(Math.PI / 4));
            //var x = (float) (previousPoint.X*Math.Cos(angle) - previousPoint.Y*Math.Sin(angle));
            //var y = (float) (previousPoint.X*Math.Sin(angle) + previousPoint.Y*Math.Cos(angle));
            var tempPrevious = new PointF(previousRadiusPoint.X - center.X, PreviousPoint.Y - center.Y);
            var x = (float)(tempPrevious.X * Math.Cos(Angle) - tempPrevious.Y * Math.Sin(Angle));
            var y = (float)(tempPrevious.X * Math.Sin(Angle) + tempPrevious.Y * Math.Cos(Angle));
            //y = y - Math.Abs(rectangles.OrderBy(rect => rect.Top).First().Y);
            //rectangles.Add(new RectangleF(new PointF(x, y - rectangleSize.Height - rectangles[0].Height), rectangleSize));
            //rectangles.Add(new RectangleF(new PointF(x-Math.Abs(previousPoint.X-rectangleSize.Width),y-Math.Abs(previousPoint.Y-rectangleSize.Height)), rectangleSize));
            //rectangles.Add(new RectangleF(new PointF(x-Math.Abs(previousPoint.X-rectangleSize.Width),y-rectangleSize.Height), rectangleSize));
            rectangles.Add(new RectangleF(new PointF(2 * x - tempPrevious.X + center.X, (y < 0 ? (tempPrevious.Y - rectangleSize.Height) : y) + center.Y), rectangleSize));
            x = (float)(x + Math.Sign(x) * Math.Sqrt(2) / 2 + center.X);
            y = (float)(y + Math.Sign(y) * Math.Sqrt(2) / 2) + center.Y;
            //previousRadiusPoint = new PointF((float)(x + Math.Sign(x) * Math.Sqrt(2) / 2), (float)(y + Math.Sign(y) * Math.Sqrt(2) / 2));
            previousRadiusPoint = new PointF(x, y);

            return rectangles[rectangles.Count - 1];
        }
    }
}