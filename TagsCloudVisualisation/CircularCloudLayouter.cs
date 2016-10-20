using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualisation
{
    public class CircularCloudLayouter
    {
        private readonly PointF center;
        private double radius;
        private List<RectangleF> cloud;
        public CircularCloudLayouter(PointF center)
        {
            this.center = center;
            cloud=new List<RectangleF>();
            radius = 1;
        }

        public RectangleF PutNextRectangle(SizeF rectangleSize)
        {
            if (cloud.Count == 0)
            {
                radius = rectangleSize.Width/2.0;
                var x0 = (float) (center.X - rectangleSize.Width/2.0);
                var y0 = (float) (center.Y - rectangleSize.Height/2.0);
                cloud.Add(new RectangleF(new PointF(x0,y0), rectangleSize));
                return cloud[cloud.Count - 1];
            }
            else
            {
                cloud.Add(new RectangleF(new PointF(1,1), rectangleSize));
                return cloud[cloud.Count - 1];
            }
            
        }

    }

    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;
        [SetUp]
        public void SetUp()
        {
            layouter=new CircularCloudLayouter(new Point(0,0));
        }

        [Test]
        public void PlaceInCenter_ZeroRectangle()
        {
            var rectangle=layouter.PutNextRectangle(new Size(0, 0));
            rectangle.Location.Should().Be(new PointF(0, 0));
        }

        [Test]
        public void PlaceInCenter_OneRectangle()
        {
            var rectangle = layouter.PutNextRectangle(new Size(1, 1));
            rectangle.Should().Match(rect => ((RectangleF)rect).IntersectsWith(new RectangleF(0, 0, 0, 0)));

        }

        [Test]
        public void PlaceWithoutIntersection_TwoRectangles()
        {
            var first = layouter.PutNextRectangle(new Size(1, 1));
            var second = layouter.PutNextRectangle(new Size(1, 1));
            first.Should().Match(rect => !((RectangleF) rect).IntersectsWith(second));
        }

        [Test]
        public void PlaceWithoudIntersection_TwoDifferentRectangles()
        {
            var first = layouter.PutNextRectangle(new Size(2, 5));
            var second = layouter.PutNextRectangle(new Size(3, 4));
            Assert.IsTrue(!first.IntersectsWith(second));
        }
        
    }
}