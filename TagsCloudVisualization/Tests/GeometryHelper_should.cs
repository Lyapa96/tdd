using System;
using System.Drawing;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class GeometryHelper_should
    {
        private static readonly TestCaseData[] PointsCases =
        {
            new TestCaseData(new Point(0, 0), new Point(0, 5)).Returns(5),
            new TestCaseData(new Point(5, 5), new Point(1, 2)).Returns(5),
            new TestCaseData(new Point(1, 2), new Point(5, 5)).Returns(5)
        };

        [TestCaseSource(nameof(PointsCases))]
        public double FindDistanceBetwenTwoPoints(Point a, Point b)
        {
            return GeometryHelper.GetDistanceBetweenPoints(a, b);
        }

        private static readonly TestCaseData[] RadiusData =
        {
            new TestCaseData(new Point(3, 4), 6, 8).Returns(5),
            new TestCaseData(new Point(0, 0), 6, 8).Returns(10),
            new TestCaseData(new Point(3, 0), 4, 4).Returns(5)
        };

        [TestCaseSource(nameof(RadiusData))]
        public double FindMaxRadius(Point center, int width, int height)
        {
            return GeometryHelper.GetMaxRadius(center, width, height);
        }

        [TestCase(-1, 0, TestName = "negative x")]
        [TestCase(0, -1, TestName = "negative y")]
        [TestCase(-1, -1, TestName = "negative x and negative y")]
        [TestCase(101, 0, TestName = "x is greater than width")]
        [TestCase(0, 101, TestName = "y is greater than height")]
        public void rightIdentifyIncorrectPoints(int x, int y)
        {
            Assert.That(GeometryHelper.IsIncorrectPoint(new Point(x, y), 100, 100));
        }
    }
}