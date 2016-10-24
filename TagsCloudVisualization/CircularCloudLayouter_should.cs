using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
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
            Assert.Throws<ArgumentException>(() => { var cloud = new CircularCloudLayouter(new Point(x, y)); });
        }


        static IEnumerable FirstRectangleCases
        {
            get
            {
                yield return new TestCaseData(new Size(2, 2)).Returns(new Point(49, 49));
                yield return new TestCaseData(new Size(10, 10)).Returns(new Point(45, 45));
                yield return new TestCaseData(new Size(8, 4)).Returns(new Point(46, 48));
                yield return new TestCaseData(new Size(100, 100)).Returns(new Point(0, 0));
            }
        }

        [Test, TestCaseSource("FirstRectangleCases")]
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

        [Test]
        public void createCloudDifferentSizes()
        {
            var height = 300;
            var width = 300;
            var center = new Point(width/2, height/2);
            var cloud = new CircularCloudLayouter(center, width, height);

            cloud.Height.ShouldBeEquivalentTo(300);
            cloud.Width.ShouldBeEquivalentTo(300);
        }

        [Test]
        public void createSpiral()
        {
            var densityOfSpirals = 0.01;
            var deltaInDegrees = 10;
            cloud.CreateSpiral(densityOfSpirals, deltaInDegrees);

            cloud.Spiral.Should().NotBeEmpty();
            //90 градусов => радиус примерно 90*(0.01+0.001*9) = 1
            cloud.Spiral.Should().Contain(new Point(centerPoint.X, centerPoint.Y + 1));
            //180 градусов => радиус примерно 180*(0.01+0.001*18) = 5
            cloud.Spiral.Should().Contain(new Point(centerPoint.X - 5, centerPoint.Y));
            //270 градусов => радиус примерно 270*(0.01+0.001*27) = 10
            cloud.Spiral.Should().Contain(new Point(centerPoint.X, centerPoint.Y - 10));
            //360 градусов => радиус примерно 360*(0.01+0.001*36) = 4
            cloud.Spiral.Should().Contain(new Point(centerPoint.X + 17, centerPoint.Y));
            //450 градусов => радиус примерно 450*(0.01+0.001*45) = 25
            cloud.Spiral.Should().Contain(new Point(centerPoint.X, centerPoint.Y + 25));
        }

        [Test]
        public void createSpiral_whichDoesNotContainIncorrectPoints()
        {
            cloud.CreateSpiral();
            cloud.Spiral.Should()
                .NotContain(point => point.X < 0 || point.Y < 0 || point.X > cloud.Width || point.Y > cloud.Height);
        }

        [Test]
        public void deletePointsFromSpiral_WhenPointsInsideRectangles()
        {
            var firstCheckPoint = new Point(47, 51);
            var secondCheckPoint = new Point(54, 47);
            cloud.CreateSpiral();

            cloud.Spiral.Should().Contain(firstCheckPoint);
            cloud.Spiral.Should().Contain(secondCheckPoint);

            cloud.PutNextRectangle(new Size(10, 10));

            cloud.Spiral.Should().NotContain(firstCheckPoint);
            cloud.Spiral.Should().NotContain(secondCheckPoint);
        }

        [Test]
        public void deletePointsFromSpiral_WhenRectangleCrossesSpiral()
        {
            
        }

        [Test]
        public void putThreeSimilarRectanglesOnSpiral()
        {
            cloud.CreateSpiral();
            cloud.Spiral.Should().NotBeEmpty();

            cloud.PutNextRectangle(new Size(10, 10));
            cloud.PutNextRectangle(new Size(10, 10));
            cloud.PutNextRectangle(new Size(10, 10));

            cloud.Rectangles.Should().HaveCount(3);
            cloud.Spiral.Should().NotContain(cloud.Rectangles[1].Location);
            cloud.Spiral.Should().NotContain(cloud.Rectangles[2].Location);
        }

        static IEnumerable IntersectionRectanglesCases
        {
            get
            {
                yield return
                    new TestCaseData(new Rectangle(new Point(0, 0), new Size(10, 10)),
                        new Rectangle(new Point(5, 5), new Size(10, 10))).Returns(true).SetName("one intersects other");
                yield return
                    new TestCaseData(new Rectangle(new Point(0, 0), new Size(10, 10)),
                        new Rectangle(new Point(20, 20), new Size(10, 10))).Returns(false).SetName("rectangles do not intersect");
                yield return
                    new TestCaseData(new Rectangle(new Point(0, 0), new Size(10, 10)),
                        new Rectangle(new Point(10, 10), new Size(10, 10))).Returns(false).SetName("rectangles do not intersect when upper left corner equal to lower left corner other");
                yield return
                    new TestCaseData(new Rectangle(new Point(0, 0), new Size(10, 10)),
                        new Rectangle(new Point(10, 5), new Size(10, 10))).Returns(true).SetName("one intersects other at border");
                yield return
                  new TestCaseData(new Rectangle(new Point(0, 0), new Size(10, 10)),
                      new Rectangle(new Point(2, 2), new Size(2, 2))).Returns(true).SetName("one inside other");
            }
        }

        [TestCaseSource("IntersectionRectanglesCases")]
        public bool rightСheckRectanglesAtIntersection(Rectangle r1, Rectangle r2)
        {
            return CircularCloudLayouter.IsRectanglesIntersect(r1, r2);
        }

        static IEnumerable RectangleInsideCloudCases
        {
            get
            {
                yield return new TestCaseData(new Rectangle(5,5,50,50)).Returns(true);
                yield return new TestCaseData(new Rectangle(0,0,50,50)).Returns(true);
                yield return new TestCaseData(new Rectangle(0,0,100,100)).Returns(true);
                yield return new TestCaseData(new Rectangle(0,0,120,10)).Returns(false);
                yield return new TestCaseData(new Rectangle(0, 0, 120, 100)).Returns(false);      
            }
        }
        [TestCaseSource("RectangleInsideCloudCases")]
        public bool rightCheckThatRectagleInCloud(Rectangle rectangle)
        {
            return cloud.IsRectangleInsideCloud(rectangle);
        }

        [Test]
        public void throwException_IfRectangleIsTooLarge()
        {
            Assert.Throws<RectangleIsNotPlacedException>(() => { cloud.PutNextRectangle(new Size(width+1,height+1));});
        }

        [Test]
        public void throwException_IfRectangleCannotBePutOnCloud()
        {
            cloud.PutNextRectangle(new Size(50, 50));
            Assert.Throws<RectangleIsNotPlacedException>(() => { cloud.PutNextRectangle(new Size(80, 80)); });
        }

    }
}