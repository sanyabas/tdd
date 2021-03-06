﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualisation.Extensions;
using TagsCloudVisualisation.Layouter;
using TagsCloudVisualisation.Visualizer;

namespace CircularCloudVisualisationTest
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private ICLoudLayouter layouter;
        private ICloudVisualizer visualizer;
        private Point center;
        private const double DensityFactor = 0.6;

        [SetUp]
        public void SetUp()
        {
            center = new Point(400, 300);
            layouter = new CircularCloudLayouter(center);
            visualizer = new CircularCloudVisualizer(layouter);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed)
                return;
            var name = TestContext.CurrentContext.Test.Name;
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, $"{name}.bmp");
            visualizer.VisualizeAndSave(layouter.GetLayout(), path, ImageFormat.Bmp);
            TestContext.WriteLine($"The image was saved to {path}");
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
            layouter.PutNextRectangle(new Size(73, 73));
            layouter.PutNextRectangle(new Size(73, 73));
            CheckIntersection(layouter.GetLayout());
        }

        [Test]
        public void PlaceWithoutIntersection_TwoDifferentRectangles()
        {
            layouter.PutNextRectangle(new Size(100, 80));
            layouter.PutNextRectangle(new Size(50, 30));
            CheckIntersection(layouter.GetLayout());

        }

        [Test]
        public void PlaceWithoutIntersection_ThreeRectangles()
        {
            layouter.PutNextRectangle(new SizeF(80, 50));
            layouter.PutNextRectangle(new SizeF(70, 70));
            layouter.PutNextRectangle(new SizeF(100, 30));
            CheckIntersection(layouter.GetLayout());
        }

        [Test]
        [TestCase(1, TestName = "one")]
        [TestCase(2, TestName = "two")]
        [TestCase(3, TestName = "three")]
        [TestCase(4, TestName = "four")]
        [TestCase(5, TestName = "five")]
        [TestCase(8, TestName = "eight")]
        [TestCase(10, TestName = "ten")]
        [TestCase(20, TestName = "twenty")]
        [TestCase(50, TestName = "fifty")]
        [TestCase(100, TestName = "hundred")]
        public void PlaceWithoutIntersection_RandomRectangles(int rectanglesNumber)
        {
            var random = new Random();
            for (var i = 0; i < rectanglesNumber; i++)
                layouter.PutNextRectangle(new SizeF(random.Next(5, 8) * 10, random.Next(2, 5) * 10));
            CheckIntersection(layouter.GetLayout());
        }

        [Test]
        public void PlaceWithoutIntersection_200Squares()
        {
            const int number = 200;
            for (var i = 0; i < number; i++)
            {
                layouter.PutNextRectangle(new SizeF(30, 30));
            }
            CheckIntersection(layouter.GetLayout());
        }

        [Test]
        public void MakeDenseCircularLayout()
        {
            for (var i = 0; i < 200; i++)
                layouter.PutNextRectangle(new SizeF(30, 30));
            CheckCircularity(layouter.GetLayout());

        }

        private void CheckIntersection(List<RectangleF> rectangles)
        {
            for (var i = 0; i < rectangles.Count - 1; i++)
                for (var j = i + 1; j < rectangles.Count; j++)
                    rectangles[i].Should().Match(rect => !((RectangleF)rect).IntersectsWith(rectangles[j]));
        }

        private void CheckCircularity(List<RectangleF> rectangles)
        {
            var farest = rectangles.OrderByDescending(rect => rect.Location.GetDistanceTo(center)).FirstOrDefault();
            var circleRadius = farest.Location.GetDistanceTo(center);
            var area = rectangles.Sum(rectangle => rectangle.Width * rectangle.Height);
            var circleArea = circleRadius * circleRadius * Math.PI;
            area.Should().BeGreaterOrEqualTo((float)(DensityFactor * circleArea));
        }
    }
}