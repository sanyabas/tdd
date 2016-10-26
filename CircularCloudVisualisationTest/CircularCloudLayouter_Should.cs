using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualisation;

namespace CircularCloudVisualisationTest
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;
        private CircularCloudVisualiser visualiser;
        private Point center;
        private int number;
        [SetUp]
        public void SetUp()
        {
            center = new Point(400, 300);
            layouter = new CircularCloudLayouter(center);
            visualiser = new CircularCloudVisualiser();
        }

        [Test]
        public void PlaceInCenter_ZeroRectangle()
        {
            var rectangle = layouter.PutNextRectangle(new Size(0, 0));
            Assert.AreEqual(rectangle.Location.X, center.X, 1e-3);
            Assert.AreEqual(rectangle.Location.Y, center.Y, 1e-3);
        }

        [Test]
        public void PlaceInCenter_OneRectangle()
        {
            var rectangle = layouter.PutNextRectangle(new Size(100, 100));
            rectangle.Should().Match(rect => ((RectangleF)rect).IntersectsWith(new RectangleF(center, new SizeF(0, 0))));

        }

        [Test]
        public void PlaceWithoutIntersection_TwoRectangles()
        {
            var first = layouter.PutNextRectangle(new Size(73, 73));
            var second = layouter.PutNextRectangle(new Size(73, 73));
            first.Should().Match(rect => !((RectangleF)rect).IntersectsWith(second));
        }

        [Test]
        public void PlaceWithoudIntersection_TwoDifferentRectangles()
        {
            var first = layouter.PutNextRectangle(new Size(100, 80));
            var second = layouter.PutNextRectangle(new Size(50, 30));
            CheckIntersection(first, second);

        }

        [Test]
        public void PlaceWithoutIntersection_ThreeRectangles()
        {
            var first = layouter.PutNextRectangle(new SizeF(80, 50));
            var second = layouter.PutNextRectangle(new SizeF(70, 70));
            var third = layouter.PutNextRectangle(new SizeF(100, 30));
            CheckIntersection(first, second, third);
        }

        [Test]
        [TestCase(1, TestName = "one")]
        [TestCase(2, TestName = "two")]
        [TestCase(3, TestName = "three")]
        [TestCase(4, TestName = "four")]
        [TestCase(5, TestName = "five")]
        [TestCase(20, TestName = "twenty")]
        public void PlaceWithoutIntersection_RandomRectangles(int rectanglesNumber)
        {
            var random = new Random();
            var rectangles = new RectangleF[rectanglesNumber];
            for (var i = 0; i < rectanglesNumber; i++)
                rectangles[i] = layouter.PutNextRectangle(new SizeF(random.Next(80, 200), random.Next(20, 80)));
            CheckIntersection(rectangles);
        }

        [TearDown]
        public void TearDown()
        {
            number++;
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, $"{number}.bmp");
            visualiser.Save(layouter.GetLayout(), path, ImageFormat.Bmp);
        }

        private void CheckIntersection(params RectangleF[] rectangles)
        {
            for (var i = 0; i < rectangles.Length - 1; i++)
                for (var j = i + 1; j < rectangles.Length; j++)
                    rectangles[i].Should().Match(rect => !((RectangleF)rect).IntersectsWith(rectangles[j]));
        }

    }
}