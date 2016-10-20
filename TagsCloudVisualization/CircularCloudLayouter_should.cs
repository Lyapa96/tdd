using System;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter_should
    {
        [Test]
        public void beEmpty_AfterCreation()
        {
          
            var center = new Point(5, 5);
            var cloud = new CircularCloudLayouter(center);
            cloud.Rectangles.Should().BeEmpty();
        }

        [Test]
        public void putOneRectangleInCenter()
        {
            var center = new Point(10, 10);
            var cloud = new CircularCloudLayouter(center);
            var rectangle = cloud.PutNextRectangle(new Size(5, 5));
            
            cloud.Rectangles.Should().HaveCount(1);
            rectangle.Location.ShouldBeEquivalentTo(center);
        }

        [TestCase(-1, 0, TestName = "negative x")]
        [TestCase(0, -1, TestName = "negative y")]
        [TestCase(-1, -1, TestName = "negative x and negative y")]
        [TestCase(CircularCloudLayouter.Width + 1, 0, TestName = "x is greater than clouds width")]
        [TestCase(0, CircularCloudLayouter.Height + 1, TestName = "y is greater than clouds height")]
        public void throwException_IfIncorrectCenterPoint(int x, int y)
        {
            Assert.Throws<ArgumentException>(() => { var cloud = new CircularCloudLayouter(new Point(x, y)); });
        }

        [Test]
        public void putTwoRectangles()
        {
            Point center= new Point(50,50);
            var cloud = new CircularCloudLayouter(center);

            cloud.PutNextRectangle(new Size(10, 10));
            cloud.PutNextRectangle(new Size(15, 15));

            cloud.Rectangles.Should().HaveCount(2);
        }

        [Test]
        public void createSpiral()
        {
            Point center = new Point(50, 50);
            var cloud = new CircularCloudLayouter(center);

            cloud.CreateSpiral(0.05,60,10,center);
           
            cloud.Spiral.Should().NotBeEmpty();
        }
    }
}