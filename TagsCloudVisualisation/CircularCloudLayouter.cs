using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualisation
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Equals(new Size(0,0)))
                return new Rectangle(1,0,0,0);
            throw new NotImplementedException();
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
        public void PlaceAnywehe_ZeroRectangle()
        {
            var rectangle=layouter.PutNextRectangle(new Size(0, 0));
            rectangle.Location.Should().NotBe(new Point(0, 0));
        }
    }
}