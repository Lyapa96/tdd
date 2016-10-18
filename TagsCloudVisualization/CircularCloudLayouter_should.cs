using System;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter_should
    {
        [Test]
        public void BeEmpty_AfterCreation()
        {
            var center = new Point(5, 5);
            var cloud = new CircularCloudLayouter(center);
            cloud.Rectangles.Should().BeEmpty();
        }


        
        [TestCase(-1, 0, TestName = "negative x")]
        [TestCase(0, -1, TestName = "negative y")]
        [TestCase(-1, -1, TestName = "negative x and negative y")]
        [TestCase(CircularCloudLayouter.Width+1, 0, TestName = "x is greater than clouds width")]
        [TestCase(0, CircularCloudLayouter.Height+1, TestName = "y is greater than clouds height")]
        public void ThrowException_IfIncorrectCenterPoint(int x, int y)
        {
            Assert.Throws<ArgumentException>(() =>
           {
               var cloud = new CircularCloudLayouter(new Point(x, y));
           });
        }

        [Test]
        public void PutOneRectangleInCenter()
        {
            var center = new Point(10,10);
            var cloud = new CircularCloudLayouter(center);

            var rectangle = cloud.PutNextRectangle(new Size(5, 5));

            cloud.Rectangles.Should().HaveCount(1);
            rectangle.Location.ShouldBeEquivalentTo(center);
        }

    }
}