using System;
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

        [SetUp]
        public void SetUp()
        {
            centerPoint = new Point(50, 50);
            cloud = new CircularCloudLayouter(centerPoint);
        }

        [Test]
        public void beEmpty_AfterCreation()
        {
            cloud.Rectangles.Should().BeEmpty();
        }

        [TestCase(-1, 0, TestName = "negative x")]
        [TestCase(0, -1, TestName = "negative y")]
        [TestCase(-1, -1, TestName = "negative x and negative y")]
        [TestCase(100 + 1, 0, TestName = "x is greater than clouds width")]
        [TestCase(0, 100 + 1, TestName = "y is greater than clouds height")]
        public void throwException_IfIncorrectCenterPoint(int x, int y)
        {
            Assert.Throws<ArgumentException>(() => { var cloud = new CircularCloudLayouter(new Point(x, y)); });
        }


        static object[] FirstRectangleCases = {
        new object[] { new Size(2,2), new Point(49,49) },
        new object[] { new Size(10,10), new Point(45,45) },
        new object[] { new Size(8,4), new Point(46,48) },
        new object[] { new Size(100,100), new Point(0,0) }
        };

        [TestCaseSource("FirstRectangleCases")]
        public void putFirstRectangleInCenter(Size size,Point expectedPoint)
        {
            var rectangle = cloud.PutNextRectangle(size);

            cloud.Rectangles.Should().HaveCount(1);
            rectangle.Location.ShouldBeEquivalentTo(expectedPoint);
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
            var cloud = new CircularCloudLayouter(center,width,height);

            cloud.Height.ShouldBeEquivalentTo(300);
            cloud.Width.ShouldBeEquivalentTo(300);
        }

        [Test]
        public void createSpiral()
        {
            var densityOfSpirals = 0.01;
            var deltaInDegrees = 10;
            cloud.CreateSpiral(densityOfSpirals,deltaInDegrees);
           
            
            cloud.Spiral.Should().NotBeEmpty();
            //90 градусов => радиус примерно 90*(0.01+0.001*9) = 1
            cloud.Spiral.Should().Contain(new Point(centerPoint.X,centerPoint.Y+1));
            //180 градусов => радиус примерно 180*(0.01+0.001*18) = 5
            cloud.Spiral.Should().Contain(new Point(centerPoint.X-5,centerPoint.Y));
            //270 градусов => радиус примерно 270*(0.01+0.001*27) = 10
            cloud.Spiral.Should().Contain(new Point(centerPoint.X,centerPoint.Y-10));
            //360 градусов => радиус примерно 360*(0.01+0.001*36) = 4
            cloud.Spiral.Should().Contain(new Point(centerPoint.X+17, centerPoint.Y));
            //450 градусов => радиус примерно 450*(0.01+0.001*45) = 25
            cloud.Spiral.Should().Contain(new Point(centerPoint.X, centerPoint.Y+25));

        }

        [Test]
        public void deletePointFromSpiral_WhenPointsInsideRectangles()
        {
            cloud.CreateSpiral();
            //Если раскоментить нижние строки повысится понимание теста
            //но это нарушение ААА
            //cloud.Spiral.Should().Contain(new Point(47, 51));
            //cloud.Spiral.Should().Contain(new Point(54, 47));

            cloud.PutNextRectangle(new Size(10, 10));

            cloud.Spiral.Should().NotContain(new Point(50, 50));
            cloud.Spiral.Should().NotContain(new Point(47, 51));
            cloud.Spiral.Should().NotContain(new Point(54, 47));
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

        static object[] IntersectionRectanglesCases = {
        new object[] { new Rectangle(new Point(0,0),new Size(10,10)),new Rectangle(new Point(5,5),new Size(10,10)),true },
        new object[] { new Rectangle(new Point(0,0),new Size(10,10)),new Rectangle(new Point(20,20),new Size(10,10)),false },
        new object[] { new Rectangle(new Point(0,0),new Size(10,10)),new Rectangle(new Point(10,10),new Size(10,10)),false },
        new object[] { new Rectangle(new Point(0,0), new Size(10, 10)), new Rectangle(new Point(10, 5), new Size(10, 10)),true },
        new object[] { new Rectangle(new Point(0,0),new Size(1,1)),new Rectangle(new Point(1,0),new Size(10,10)),true }
        };

        [TestCaseSource("IntersectionRectanglesCases")]
        public void rightСheckRectanglesAtIntersection(Rectangle r1,Rectangle r2,bool expectedResult)
        {
            var actualResult = CircularCloudLayouter.IsRectanglesIntersect(r1, r2);
            Assert.That(expectedResult,Is.EqualTo(actualResult));
        }

    }
}