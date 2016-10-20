using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public const int Width = 100;
        public const int Height = 100;

        public readonly Point CenterPoint;

        public List<Rectangle> Rectangles { get; set; }
        public List<Point> Spiral = new List<Point>();

        public CircularCloudLayouter(Point center)
        {
            Rectangles = new List<Rectangle>();
            if (center.X < 0 || center.Y < 0 || center.X > Width || center.Y > Height) throw new ArgumentException();
            CenterPoint = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle(CenterPoint, rectangleSize);
            Rectangles.Add(rectangle);
            return rectangle;
        }

        public void CreateSpiral(double densityOfSpirals, double deltaInDegrees, double numberOfSpirals, Point center)
        {
            double angleInDegrees = 0; 
            Spiral.Add(new Point(center.X, center.Y));

            while (angleInDegrees <= (numberOfSpirals*360))
            {
                angleInDegrees += deltaInDegrees;
                var radius = angleInDegrees*densityOfSpirals;

                //Получаем новые значения координат точки в декартовой системе координат:
                var x = (radius*Math.Cos(angleInDegrees/180*Math.PI)) + center.X;
                var y = (radius*Math.Sin(angleInDegrees/180*Math.PI)) + center.Y;

                Spiral.Add(new Point((int) x, (int) y));
            }
        }
    }
}