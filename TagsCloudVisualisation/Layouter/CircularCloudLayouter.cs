﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualisation.Extensions;

namespace TagsCloudVisualisation.Layouter
{
    public class CircularCloudLayouter : ICLoudLayouter
    {
        private readonly PointF center;
        private List<RectangleF> rectangles;
        private const float SpiralRotationAngle = (float)(-Math.PI / 10);
        private static readonly float SpiralLengthDelta = (float)Math.Sin(Math.PI / 4);
        private const double BoundRotationAngle = -Math.PI / 6;
        private PointF previousRadiusPoint;
        private const int Delta = 5;
        private readonly SizeF bounds;
        private const int RotationLimit = 12;

        public List<RectangleF> GetLayout()
        {
            return rectangles;
        }

        public CircularCloudLayouter(PointF center)
        {
            this.center = center;
            rectangles = new List<RectangleF>();
            bounds = new SizeF(center.X * 2, center.Y * 2);
        }

        public RectangleF PutNextRectangle(SizeF rectangleSize)
        {
            RectangleF result;
            if (rectangles.Count == 0)
            {
                var delta = new PointF(rectangleSize.Width / 2, rectangleSize.Height / 2);
                var placingPoint = center.Sub(delta);
                result=new RectangleF(placingPoint,rectangleSize);
            }
            else
            {
                var placingPoint = FindSuitablePoint(rectangleSize);
                previousRadiusPoint = placingPoint;
                result = GetProcessedRectangle(placingPoint, rectangleSize);
            }
            rectangles.Add(result);
            return result;
        }

        private RectangleF GetProcessedRectangle(PointF placingPoint, SizeF rectangleSize)
        {
            var tempResult = new RectangleF(placingPoint, rectangleSize);
            tempResult = ShiftToCenter(tempResult);
            if (tempResult.IsBehindBounds(bounds))
                tempResult = TryRotateInBounds(tempResult);
            var result = ShiftToCenter(tempResult);
            return result;
        }

        private PointF FindSuitablePoint(SizeF rectangleSize)
        {
            PointF placingPoint;
            while (true)
            {
                var temporaryPoint = GetNextSpiralPoint(previousRadiusPoint);
                var tempRectangle = new RectangleF(temporaryPoint, rectangleSize);
                var intersects = tempRectangle.IntersectsWith(rectangles);
                placingPoint = previousRadiusPoint = temporaryPoint;
                if (intersects)
                    continue;
                break;
            }

            return placingPoint;
        }

        private PointF GetNextSpiralPoint(PointF previousPoint)
        {
            if (rectangles.Count == 1)
            {
                var previousRectangle = rectangles.Last();
                return new PointF(previousRectangle.Right, previousRectangle.Top);
            }
            var rotated = previousPoint.RotateAround(center, SpiralRotationAngle);
            var relativeToCenter = rotated.Sub(center);
            var shift = new PointF(Math.Sign(relativeToCenter.X) * SpiralLengthDelta, Math.Sign(relativeToCenter.Y) * SpiralLengthDelta);
            var result = relativeToCenter.Add(shift).Add(center);
            var number = 0;
            while (result.IsBehindBounds(bounds) && number <= RotationLimit)
            {
                number++;
                result = result.RotateAround(center, BoundRotationAngle);
            }
            return result;
        }

        private RectangleF ShiftToCenter(RectangleF initialPoint)
        {
            var shiftedByDiagonal = ShiftToCenter(initialPoint, GetShiftToCenter, rect => true);
            var shiftedByX = ShiftToCenter(shiftedByDiagonal, GetShiftToCenterByX,
                rect => Math.Abs(rect.X - center.X) > 5);
            var shiftedByY = ShiftToCenter(shiftedByX, GetShiftToCenterByY, rect => Math.Abs(rect.Y - center.Y) > 5);
            return shiftedByY;
        }

        private PointF GetShiftToCenter(RectangleF rectangle)
        {
            var dx = rectangle.X < center.X ? Delta : -Delta;
            var dy = rectangle.Y < center.Y ? Delta : -Delta;
            return new PointF(dx, dy);
        }

        private PointF GetShiftToCenterByX(RectangleF rectangle)
        {
            var dx = rectangle.X < center.X ? Delta : -Delta;
            return new PointF(dx, 0);
        }

        private PointF GetShiftToCenterByY(RectangleF rectangle)
        {
            var dy = rectangle.Y < center.Y ? Delta : -Delta;
            return new PointF(0, dy);
        }

        private RectangleF ShiftToCenter(RectangleF initial, Func<RectangleF, PointF> getShift,
            Func<RectangleF, bool> condition)
        {
            var shift = getShift(initial);
            var tempResult = new RectangleF(initial.Location.Add(shift), initial.Size);
            while (!tempResult.IntersectsWith(rectangles) && condition(tempResult))
                tempResult = new RectangleF(tempResult.Location.Add(shift), tempResult.Size);
            return new RectangleF(tempResult.Location.Sub(shift), tempResult.Size);
        }

        private RectangleF TryRotateInBounds(RectangleF rectangle)
        {
            var number = 0;
            while ((rectangle.IntersectsWith(rectangles) || rectangle.IsBehindBounds(bounds)) && number < RotationLimit)
            {
                number++;
                rectangle = new RectangleF(rectangle.Location.RotateAround(center, BoundRotationAngle), rectangle.Size);
            }
            return rectangle;
        }
    }
}