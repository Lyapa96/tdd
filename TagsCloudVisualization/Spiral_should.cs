﻿
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class Spiral_should
    {
        [Test]
        public void throwException_IfRectangleCannotBePutOnCloud()
        {
            var spiral = new Spiral(new Point(2,2), 4, 4,0.001,10);
            Assert.Throws<SpiralException>(() =>
            {
                while (spiral.CurrentRadius <= spiral.MaxRadius)
                {
                    spiral.GetNextPoint();
                }
                spiral.GetNextPoint();
            });
        }


        private static readonly TestCaseData[] RadiusData =
        {
            new TestCaseData(new Point(3, 4), 6, 8).Returns(5),
            new TestCaseData(new Point(0, 0), 6, 8).Returns(10),
            new TestCaseData(new Point(3, 0), 4, 4).Returns(5),
        };

        [TestCaseSource(nameof(RadiusData))]
        public double FindMaxRadius(Point center, int width, int height)
        {
            var spiral = new Spiral(center, width, height,0.001,10);
            return spiral.MaxRadius;
        }

        [Test]
        public void createCorrectPoints()
        {
            var densityOfSpirals = 0.01;
            var deltaInDegrees = 10;
            var centerPoint = new Point(50, 50);
            var spiral = new Spiral(centerPoint, 100, 100, densityOfSpirals, deltaInDegrees);

            var pointsOfSpiral = new List<Point>();
            while (spiral.CurrentRadius < 25)
            {
                pointsOfSpiral.Add(spiral.GetNextPoint());
            }
            //90 градусов => радиус примерно 90*(0.01+0.001*9) = 1
            pointsOfSpiral.Should().Contain(new Point(centerPoint.X, centerPoint.Y + 1));
            //180 градусов => радиус примерно 180*(0.01+0.001*18) = 5
            pointsOfSpiral.Should().Contain(new Point(centerPoint.X - 5, centerPoint.Y));
            //270 градусов => радиус примерно 270*(0.01+0.001*27) = 10
            pointsOfSpiral.Should().Contain(new Point(centerPoint.X, centerPoint.Y - 10));
            //360 градусов => радиус примерно 360*(0.01+0.001*36) = 4
            pointsOfSpiral.Should().Contain(new Point(centerPoint.X + 17, centerPoint.Y));
            //450 градусов => радиус примерно 450*(0.01+0.001*45) = 25
            pointsOfSpiral.Should().Contain(new Point(centerPoint.X, centerPoint.Y + 25));
        }

    }
}