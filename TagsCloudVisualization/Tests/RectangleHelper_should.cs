using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    public class RectangleHelper_should
    {
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
            return RectangleHelper.IsRectanglesIntersect(r1, r2);
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
            return RectangleHelper.IsRectangleInsideOtherRectangle(rectangle, width, height);
        }


        private static readonly TestCaseData[] RectangleNotIntersectsOtherRectanglesCases =
        {
            new TestCaseData(new Rectangle(5, 5, 50, 50),
                new List<Rectangle>() {new Rectangle(60, 60, 100, 100), new Rectangle(0, 0, 10, 10)}).Returns(false),
            new TestCaseData(new Rectangle(5, 5, 50, 50),
                new List<Rectangle>() {new Rectangle(60, 60, 100, 100), new Rectangle(0, 0, 4, 4)}).Returns(true)
        };


        [TestCaseSource(nameof(RectangleNotIntersectsOtherRectanglesCases))]
        public bool rightCheckThatRectagleDoesNotIntersectOtherRectangles(Rectangle rectangle,
            IEnumerable<Rectangle> otherRectangles)
        {
            return RectangleHelper.IsRectangleDoesNotIntersectsWithRectangles(rectangle, otherRectangles);
        }


        private static readonly TestCaseData[] ShiftOnXCases =
        {
            new TestCaseData(new Rectangle(5, 5, 50, 50), 0, new List<Rectangle>()).Returns(1),
            new TestCaseData(new Rectangle(5, 1, 50, 50), 0,
                new List<Rectangle>() {new Rectangle(0, 0, 2, 2), new Rectangle(60, 60, 10, 10)}).Returns(3)
        };

        [TestCaseSource(nameof(ShiftOnXCases))]
        public int shiftRectangleOnX(Rectangle rectangle, int coordinateX, List<Rectangle> otherRectangles)
        {
            return RectangleHelper.ShiftOnX(rectangle, coordinateX, otherRectangles).X;
        }


        [Test]
        public void ShiftRectangleToCenter()
        {
            var rectangle = new Rectangle(10,10,10,10);
            var center = new Point(5,5);

            var newRect = RectangleHelper.ShiftRectangleToCenter(rectangle, center, new List<Rectangle>());

            newRect.X.Should().Be(6);
            newRect.Y.Should().Be(6);
        }
    }
}