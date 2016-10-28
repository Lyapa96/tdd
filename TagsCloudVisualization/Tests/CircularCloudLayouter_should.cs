using System;
using System.Drawing;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouter_should
    {
        private Point centerPoint;
        private CircularCloudLayouter cloud;
        private int width;
        private int height;

        [SetUp]
        public void SetUp()
        {
            centerPoint = new Point(50, 50);
            width = 100;
            height = 100;
            cloud = new CircularCloudLayouter(centerPoint, width, height);
        }

        [Test]
        public void beEmpty_AfterCreation()
        {
            cloud.Rectangles.Should().BeEmpty();
        }

        [TestCase(-1, 0, TestName = "negative x")]
        [TestCase(0, -1, TestName = "negative y")]
        [TestCase(-1, -1, TestName = "negative x and negative y")]
        [TestCase(101, 0, TestName = "x is greater than clouds width")]
        [TestCase(0, 101, TestName = "y is greater than clouds height")]
        public void throwException_IfIncorrectCenterPoint(int x, int y)
        {
            Assert.Throws<ArgumentException>(
                () => { var circularCloud = new CircularCloudLayouter(new Point(x, y), 100, 100); });
        }

        [Test]
        public void createCloudDifferentSizes()
        {
            var height = 300;
            var width = 300;
            var center = new Point(width/2, height/2);
            var circularCloud = new CircularCloudLayouter(center, width, height);

            circularCloud.Height.ShouldBeEquivalentTo(300);
            circularCloud.Width.ShouldBeEquivalentTo(300);
        }


        private static readonly TestCaseData[] FirstRectangleCases =
        {
            new TestCaseData(new Size(2, 2)).Returns(new Point(49, 49)),
            new TestCaseData(new Size(10, 10)).Returns(new Point(45, 45)),
            new TestCaseData(new Size(8, 4)).Returns(new Point(46, 48)),
            new TestCaseData(new Size(100, 100)).Returns(new Point(0, 0))
        };

        [Test, TestCaseSource(nameof(FirstRectangleCases))]
        public Point putFirstRectangle_thatRectangleCenterWasEqualToCloudCenter(Size size)
        {
            var rectangle = cloud.PutNextRectangle(size);

            return rectangle.Location;
        }

        [Test]
        public void putTwoRectangles()
        {
            cloud.PutNextRectangle(new Size(10, 10));
            cloud.PutNextRectangle(new Size(10, 10));

            cloud.Rectangles.Should().HaveCount(2);
        }

        [Test]
        public void throwException_IfRectangleIsTooLarge()
        {
            Assert.Throws<ArgumentException>(
                () => { cloud.PutNextRectangle(new Size(width + 1, height + 1)); });
        }

        [Test]
        public void throwException_IfRectangleCannotBePutOnCloud()
        {
            cloud.PutNextRectangle(new Size(50, 50));
            Assert.Throws<IndexOutOfRangeException>(() => { cloud.PutNextRectangle(new Size(80, 80)); });
        }

        [Test]
        public void shiftRectangleToCenter()
        {
            var height = 300;
            var width = 300;
            var circularCloud = new CircularCloudLayouter(new Point(150, 150), width, height);

            circularCloud.PutNextRectangle(new Size(10, 6));
            circularCloud.PutNextRectangle(new Size(7, 4));
            circularCloud.PutNextRectangle(new Size(7, 3));

            var secondRectangle = circularCloud.Rectangles[2];

            secondRectangle.X.Should().Be(circularCloud.CenterPoint.X + 1);
            //правый нижний угол первого прямоугольника 153 
            secondRectangle.Y.Should().Be(154);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var fileName = TestContext.CurrentContext.Test.Name + ".bmp";
                var path = Path.Combine(TestContext.CurrentContext.TestDirectory, fileName);
                cloud.CreateBitmap(path);
                Console.WriteLine($"bitmap saved to {path}");
            }
        }
    }
}