using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;


namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_should
    {
        private Point centerPoint;
        private CircularCloudLayouter cloud;
        private int width;
        private static int height;

        [SetUp]
        public void SetUp()
        {
            centerPoint = new Point(50, 50);
            cloud = new CircularCloudLayouter(centerPoint);
            width = cloud.Width;
            height = cloud.Height;
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
            Assert.Throws<ArgumentException>(() => { var circularCloud = new CircularCloudLayouter(new Point(x, y)); });
        }

        [Test]
        public void createCloudDifferentSizes()
        {
            var height = 300;
            var width = 300;
            var center = new Point(width / 2, height / 2);
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
            cloud.CreateSpiral();

            cloud.PutNextRectangle(new Size(10, 10));
            cloud.PutNextRectangle(new Size(10, 10));

            cloud.Rectangles.Should().HaveCount(2);
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
            return CircularCloudLayouter.IsRectanglesIntersect(r1, r2);
        }

        private static readonly TestCaseData[] RectangleInsideCloudCases =
        {
            new TestCaseData(new Rectangle(5, 5, 50, 50)).Returns(true),
            new TestCaseData(new Rectangle(0, 0, 50, 50)).Returns(true),
            new TestCaseData(new Rectangle(0, 0, 100, 100)).Returns(true),
            new TestCaseData(new Rectangle(0, 0, 120, 10)).Returns(false),
            new TestCaseData(new Rectangle(0, 0, 120, 100)).Returns(false)
        };

        [TestCaseSource(nameof(RectangleInsideCloudCases))]
        public bool rightCheckThatRectagleInCloud(Rectangle rectangle)
        {
            return cloud.IsRectangleInsideCloud(rectangle);
        }

        [Test]
        public void throwException_IfRectangleIsTooLarge()
        {
            Assert.Throws<RectangleIsNotPlacedException>(
                () => { cloud.PutNextRectangle(new Size(width + 1, height + 1)); });
        }

        [Test]
        public void throwException_IfRectangleCannotBePutOnCloud()
        {
            cloud.CreateSpiral();
            cloud.PutNextRectangle(new Size(50, 50));
            Assert.Throws<RectangleIsNotPlacedException>(() => { cloud.PutNextRectangle(new Size(80, 80)); });
        }

        [Test]
        public void shiftRectangleToCenter()
        {
            var height = 300;
            var width = 300;
            var circularCloud = new CircularCloudLayouter(new Point(150,150),width,height);
            circularCloud.CreateSpiral();
            circularCloud.PutNextRectangle(new Size(10, 6));
            circularCloud.PutNextRectangle(new Size(7, 4));
            circularCloud.PutNextRectangle(new Size(7, 3));

            var secondRectangle = circularCloud.Rectangles[2];

            secondRectangle.X.Should().Be(circularCloud.CenterPoint.X+1);
            //правый нижний угол первого прямоугольника 153 
            secondRectangle.Y.Should().Be(154);
        }



        [Test,Explicit]
        public void DrawRectangles()
        {
            var height = 300;
            var width = 300;     

            var vis = new RectangleVisualizer(width,height);
            vis.Cloud.CreateSpiral(0.000001,1);
            for (int i = 0; i < 100; i++)
            {
                vis.Cloud.PutNextRectangle(new Size(7, 3));
                vis.Cloud.PutNextRectangle(new Size(7, 3));
            }

            Application.Run(vis);
        }

        [Test]
        public void addThreeBigRect()
        {
            cloud.CreateSpiral();
            cloud.PutNextRectangle(new Size(50, 50));
            cloud.PutNextRectangle(new Size(20, 20));
            cloud.PutNextRectangle(new Size(50, 50));

            cloud.Rectangles.Should().HaveCount(3);
        }

        [Test]
        public void addHundredRectangles()
        {
            cloud.CreateSpiral();
            for (int i = 0; i < 100; i++)
            {
                cloud.PutNextRectangle(new Size(10, 5));
            }

            cloud.Rectangles.Should().HaveCount(100);
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