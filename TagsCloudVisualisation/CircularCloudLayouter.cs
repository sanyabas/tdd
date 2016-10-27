using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualisation
{
    public class CircularCloudLayouter : ILayouter
    {
        private readonly PointF center;
        private List<RectangleF> rectangles;
        private const float Angle = (float)(-Math.PI / 10);
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
                var delta = new PointF(rectangleSize.Width / 2, rectangleSize.Height / 2);
                placingPoint = center.Sub(delta);
            }
            else if (rectangles.Count == 1)
                previousRadiusPoint = placingPoint = new PointF(rectangles.Last().Right, rectangles.Last().Y);
            else
            {
                while (true)
                {
                    var temporaryPoint = GetNextSpiralPoint();
                    var tempRectangle = new RectangleF(temporaryPoint, rectangleSize);
                    var intersects = rectangles.Any(rectangle => rectangle.IntersectsWith(tempRectangle));
                    if (!intersects)
                    {
                        placingPoint = previousRadiusPoint = temporaryPoint;
                        break;
                    }
                    previousRadiusPoint = temporaryPoint;
                }
                var tempResult = new RectangleF(placingPoint, rectangleSize);
                placingPoint = ShiftToCenter(tempResult).Location;
            }
            rectangles.Add(new RectangleF(placingPoint, rectangleSize));
            return rectangles[rectangles.Count - 1];
        }

        private PointF RotateAroundCenter(PointF point, double angle)
        {
            var movedPoint = point.Sub(center);
            var x = (float)(movedPoint.X * Math.Cos(angle) - movedPoint.Y * Math.Sin(angle));
            var y = (float)(movedPoint.X * Math.Sin(angle) + movedPoint.Y * Math.Cos(angle));
            return new PointF(x, y).Add(center);
        }

        private PointF GetNextSpiralPoint()
        {
            var rotated = RotateAroundCenter(previousRadiusPoint, Angle);
            var delta = (float)Math.Sin(Math.PI / 4);
            var relativeToCenter = rotated.Sub(center);
            var shift = new PointF(Math.Sign(relativeToCenter.X) * delta, Math.Sign(relativeToCenter.Y) * delta);
            var result = relativeToCenter.Add(shift).Add(center);
            const int limit = 12;
            var number = 0;
            while (IsBehindBounds(result) && number <= limit)
            {
                number++;
                result = RotateAroundCenter(result, -Math.PI / 6);
            }
            return result;
        }

        private bool IsBehindBounds(PointF point)
        {
            return point.X > 2 * center.X || point.Y > 2 * center.Y || point.X < 0 || point.Y < 0;
        }

        private bool RectangleIsBeyondBounds(RectangleF rectangle)
        {
            return rectangle.Bottom > 2 * center.Y || rectangle.Top < 0 || rectangle.Left < 0 ||
                   rectangle.Right > 2 * center.X;
        }

        private RectangleF ShiftToCenter(RectangleF initialPoint)
        {
            const int delta = 5;
            var dx = initialPoint.X < center.X ? delta : -delta;
            var dy = initialPoint.Y < center.Y ? delta : -delta;
            var shift = new PointF(dx, dy);
            var tempResult = new RectangleF(initialPoint.Location.Add(shift), initialPoint.Size);
            while (!rectangles.Any(rect => rect.IntersectsWith(tempResult)))
                tempResult = new RectangleF(tempResult.Location.Add(shift), tempResult.Size);
            tempResult = new RectangleF(tempResult.Location.Sub(shift), tempResult.Size);
            dx = initialPoint.X < center.X ? delta : -delta;
            shift = new PointF(dx, 0);
            while (!rectangles.Any(rect => rect.IntersectsWith(tempResult)) && Math.Abs(tempResult.X-center.X)>5)
                tempResult = new RectangleF(tempResult.Location.Add(shift), tempResult.Size);
            tempResult = new RectangleF(tempResult.Location.Sub(shift), tempResult.Size);
            dy = initialPoint.Y < center.Y ? delta : -delta;
            shift = new PointF(0, dy);
            while (!rectangles.Any(rect => rect.IntersectsWith(tempResult)) && Math.Abs(tempResult.Y-center.Y)>5)
                tempResult = new RectangleF(tempResult.Location.Add(shift), tempResult.Size);
            tempResult = new RectangleF(tempResult.Location.Sub(shift), tempResult.Size);
            return tempResult;
        }
    }
}