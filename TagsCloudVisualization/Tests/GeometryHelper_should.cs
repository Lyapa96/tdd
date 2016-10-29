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


        private static readonly TestCaseData[] IntersectionRectanglesCases =
       {
            new TestCaseData(new Rectangle(new Point(0, 0), new Size(10, 10)),
                new Rectangle(new Point(5, 5), new Size(10, 10))).Returns(true).SetName("one intersects other"),
            new TestCaseData(new Rectangle(new Point(0, 0), new Size(10, 10)),
                new Rectangle(new Point(20, 20), new Size(10, 10))).Returns(false)
                .SetName("rectangles do not intersect"),
            new TestCaseData(new Rectangle(new Point(0, 0), new Size(10, 10)),
                new Rectangle(new Point(10, 10), new Size(10, 10))).Returns(false)
                .SetName("rectangles do not intersect when upper left corner equal to lower left corner other"),
            new TestCaseData(new Rectangle(new Point(0, 0), new Size(10, 10)),
                new Rectangle(new Point(10, 5), new Size(10, 10))).Returns(true)
                .SetName("one intersects other at border"),
            new TestCaseData(new Rectangle(new Point(0, 0), new Size(10, 10)),
                new Rectangle(new Point(2, 2), new Size(2, 2))).Returns(true).SetName("one inside other")
        };

        [TestCaseSource(nameof(IntersectionRectanglesCases))]
        public bool rightСheckRectanglesAtIntersection(Rectangle r1, Rectangle r2)
        {
            return GeometryHelper.IsRectanglesIntersect(r1, r2);
        }

        private static readonly TestCaseData[] RectangleInsideOtherRectangleCases =
        {
            new TestCaseData(new Rectangle(5, 5, 50, 50), 100, 100).Returns(true),
            new TestCaseData(new Rectangle(0, 0, 50, 50), 100, 100).Returns(true),
            new TestCaseData(new Rectangle(0, 0, 100, 100), 100, 100).Returns(true),
            new TestCaseData(new Rectangle(0, 0, 120, 10), 100, 100).Returns(false),
            new TestCaseData(new Rectangle(0, 0, 120, 100), 100, 100).Returns(false)
        };

        [TestCaseSource(nameof(RectangleInsideOtherRectangleCases))]
        public bool rightCheckThatRectagleInOtherRectangle(Rectangle rectangle, int width, int height)
        {
            return GeometryHelper.IsRectangleInsideOtherRectangle(rectangle, width, height);
        }
    }
}