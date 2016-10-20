using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualisation
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private double radius;
        private List<Rectangle> cloud;
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            cloud=new List<Rectangle>();
            radius = 1;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (cloud.Count == 0)
            {
                cloud.Add(new Rectangle(new Point(1,0), rectangleSize));
                return cloud[cloud.Count - 1];
            }
            else
            {
                cloud.Add(new Rectangle(new Point(1,1), rectangleSize));
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
        public void PlaceAnywhere_ZeroRectangle()
        {
            var rectangle=layouter.PutNextRectangle(new Size(0, 0));
            rectangle.Location.Should().NotBe(new Point(0, 0));
        }

        [Test]
        public void PlaceAnywhere_OneRectangle()
        {
            var rectangle = layouter.PutNextRectangle(new Size(1, 1));
            rectangle.Location.Should().NotBe(new Point(0, 0));
        }

        [Test]
        public void PlaceWithoutIntersection_TwoRectangles()
        {
            var first = layouter.PutNextRectangle(new Size(1, 1));
            var second = layouter.PutNextRectangle(new Size(1, 1));
            first.Should().Match(rect => !((Rectangle) rect).IntersectsWith(second));
        }
    }
}