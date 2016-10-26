using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace TagsCloudVisualisation
{
    public class CircularCloudLayouter
    {
        private readonly PointF center;
        private float radius;
        private List<RectangleF> rectangles;
        private float angle = (float)(-Math.PI / 4);
        private PointF previousPoint => rectangles.Count == 0 ? new PointF(0, 0) : rectangles[rectangles.Count - 1].Location;
        private PointF previousRadiusPoint;
        public CircularCloudLayouter(PointF center)
        {
            this.center = center;
            rectangles = new List<RectangleF>();
            radius = 1;
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
                rectangles.Add(new RectangleF(new PointF(center.X+radius, previousPoint.Y), rectangleSize));
                previousRadiusPoint = new PointF(center.X+radius, previousPoint.Y);

            SaveLayout(rectangles.Count.ToString());
                return rectangles[rectangles.Count - 1];
            }
            radius++;
            //var temp = rectangles[rectangles.Count - 1];
            //var x = (float)(temp.X * (1 - Math.Cos(Math.PI / 4)));
            //var y = (float)(-temp.Y * Math.Cos(Math.PI / 4));
            //var x = (float) (previousPoint.X*Math.Cos(angle) - previousPoint.Y*Math.Sin(angle));
            //var y = (float) (previousPoint.X*Math.Sin(angle) + previousPoint.Y*Math.Cos(angle));
            var tempPrevious = new PointF(previousRadiusPoint.X-center.X,previousPoint.Y-center.Y);
            var x = (float)(tempPrevious.X * Math.Cos(angle) - tempPrevious.Y * Math.Sin(angle));
            var y = (float)(tempPrevious.X * Math.Sin(angle) + tempPrevious.Y * Math.Cos(angle));
            //y = y - Math.Abs(rectangles.OrderBy(rect => rect.Top).First().Y);
            //rectangles.Add(new RectangleF(new PointF(x, y - rectangleSize.Height - rectangles[0].Height), rectangleSize));
            //rectangles.Add(new RectangleF(new PointF(x-Math.Abs(previousPoint.X-rectangleSize.Width),y-Math.Abs(previousPoint.Y-rectangleSize.Height)), rectangleSize));
            //rectangles.Add(new RectangleF(new PointF(x-Math.Abs(previousPoint.X-rectangleSize.Width),y-rectangleSize.Height), rectangleSize));
            rectangles.Add(new RectangleF(new PointF(2 * x - tempPrevious.X+center.X, (y < 0 ? (tempPrevious.Y - rectangleSize.Height) : y)+center.Y), rectangleSize));
            x =(float) (x+ Math.Sign(x) * Math.Sqrt(2) / 2+center.X);
            y = (float)(y + Math.Sign(y) * Math.Sqrt(2) / 2)+ center.Y;
            //previousRadiusPoint = new PointF((float)(x + Math.Sign(x) * Math.Sqrt(2) / 2), (float)(y + Math.Sign(y) * Math.Sqrt(2) / 2));
            previousRadiusPoint = new PointF(x, y);
            SaveLayout(rectangles.Count.ToString());

            return rectangles[rectangles.Count - 1];
        }

        public void SaveLayout(string filename)
        {
            var bitmap = new Bitmap(800, 600);
            var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Black);
            graphics.DrawRectangles(new Pen(Color.Brown, 3), rectangles.ToArray());
            bitmap.Save(Path.Combine(TestContext.CurrentContext.TestDirectory, $"{filename}.bmp"), ImageFormat.Bmp);
        }
    }
}