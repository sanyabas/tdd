using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualisation
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;
        private int number = 0;
        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(400, 300));
        }

        [Test]
        public void PlaceInCenter_ZeroRectangle()
        {
            var rectangle = layouter.PutNextRectangle(new Size(0, 0));
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
            var first = layouter.PutNextRectangle(new Size(3, 3));
            var second = layouter.PutNextRectangle(new Size(3, 3));
            first.Should().Match(rect => !((RectangleF)rect).IntersectsWith(second));
        }

        [Test]
        public void PlaceWithoudIntersection_TwoDifferentRectangles()
        {
            var first = layouter.PutNextRectangle(new Size(10, 10));
            var second = layouter.PutNextRectangle(new Size(10, 10));
            CheckIntersection(first, second);

        }

        [Test]
        public void PlaceWithoutIntersection_ThreeRectangles()
        {
            var first = layouter.PutNextRectangle(new SizeF(8, 5));
            var second = layouter.PutNextRectangle(new SizeF(7, 7));
            var third = layouter.PutNextRectangle(new SizeF(10, 3));
            CheckIntersection(first, second, third);
        }

        [Test]
        [TestCase(1, TestName = "one")]
        [TestCase(2, TestName = "two")]
        [TestCase(3, TestName = "three")]
        [TestCase(4, TestName = "four")]
        [TestCase(5, TestName = "five")]
        [TestCase(20, TestName = "twenty")]
        public void PlaceWithoutIntersection_RandomRectangles(int number)
        {
            var random = new Random();
            var rectangles = new RectangleF[number];
            for (var i = 0; i < number; i++)
                rectangles[i] = layouter.PutNextRectangle(new SizeF(random.Next(3, 8), random.Next(1, 4)));
            CheckIntersection(rectangles);
        }

        [TearDown]
        public void TearDown()
        {
            number++;
            layouter.SaveLayout(number.ToString());
        }

        private void CheckIntersection(params RectangleF[] rectangles)
        {
            for (var i = 0; i < rectangles.Length - 1; i++)
                for (var j = i + 1; j < rectangles.Length; j++)
                    rectangles[i].Should().Match(rect => !((RectangleF)rect).IntersectsWith(rectangles[j]));
        }

    }
}