using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;


namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly int Width = 100;
        public readonly int Height = 100;

        public readonly Point CenterPoint;

        public List<Rectangle> Rectangles { get; set; }
        public List<Point> Spiral = new List<Point>();

        public CircularCloudLayouter(Point center)
        {
            Rectangles = new List<Rectangle>();
            if (IsIncorrectPoint(center)) throw new ArgumentException();
            CenterPoint = center;
        }

        public CircularCloudLayouter(Point center, int width, int height)
        {
            Width = width;
            Height = height;
            Rectangles = new List<Rectangle>();
            if (IsIncorrectPoint(center)) throw new ArgumentException();
            CenterPoint = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (Rectangles.Count == 0)
            {
                if(rectangleSize.Width>Width || rectangleSize.Height>Height) throw new RectangleIsNotPlacedException();
                var rectangleCenter = new Point(rectangleSize.Width/2, rectangleSize.Height/2);
                var rectangleLocation = new Point(CenterPoint.X - rectangleCenter.X, CenterPoint.Y - rectangleCenter.Y);
                var firstRectangle = new Rectangle(rectangleLocation, rectangleSize);
                Rectangles.Add(firstRectangle);

                DeletePointsInsideRectangle(firstRectangle);

                return firstRectangle;
            }
            
            foreach (var point in Spiral)
            {
                var curentRectangle = new Rectangle(point, rectangleSize);
                if (IsPossiblePutRectangle(curentRectangle) && IsRectangleInsideCloud(curentRectangle))
                {
                    Rectangles.Add(curentRectangle);
                    DeletePointsInsideRectangle(curentRectangle);
                    return curentRectangle;
                }
            }
            throw new RectangleIsNotPlacedException();
        }

        public void CreateSpiral(double densityOfSpirals = 0.001, double deltaInDegrees = 10)
        {
            Spiral.Add(CenterPoint);

            double angleInDegrees = 0;
            var deltaFromDensity = densityOfSpirals*0.1;
            var maxRadius = GetMaxRadius();
            var radius = angleInDegrees*densityOfSpirals;

            while (radius <= maxRadius)
            {
                angleInDegrees += deltaInDegrees;
                densityOfSpirals += deltaFromDensity;

                radius = angleInDegrees*densityOfSpirals;

                //Получаем новые значения координат точки в декартовой системе координат:
                var x = (int) Math.Round(radius*Math.Cos(angleInDegrees/180*Math.PI)) + CenterPoint.X;
                var y = (int) Math.Round(radius*Math.Sin(angleInDegrees/180*Math.PI)) + CenterPoint.Y;

                var nextPoint = new Point(x, y);
                if (IsIncorrectPoint(nextPoint))
                {
                    continue;
                }
                Spiral.Add(nextPoint);
            }
        }

        private double GetMaxRadius()
        {
            var upperLeftCorner = new Point(0, 0);
            var lowerLeftCorner = new Point(0, Height);
            var upperRightCorner = new Point(Width, 0);
            var lowerRightCorner = new Point(Width, Height);

            return
                Math.Max(
                    Math.Max(GetDistanceBetweenPoints(CenterPoint, upperLeftCorner),
                        GetDistanceBetweenPoints(CenterPoint, upperRightCorner)),
                    Math.Max(GetDistanceBetweenPoints(CenterPoint, lowerLeftCorner),
                        GetDistanceBetweenPoints(CenterPoint, lowerRightCorner)));
        }

        private double GetDistanceBetweenPoints(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

        private bool IsIncorrectPoint(Point point)
        {
            return point.X < 0 || point.Y < 0 || point.X > Width || point.Y > Height;
        }

        private void DeletePointsInsideRectangle(Rectangle rectangle)
        {
            var upperLeftPoint = rectangle.Location;
            var lowerRightPoint = new Point(upperLeftPoint.X + rectangle.X, upperLeftPoint.Y + rectangle.Height);
            Spiral = Spiral.Where(point => !IsPointInsideRectangle(point, upperLeftPoint, lowerRightPoint)).ToList();
        }

        private bool IsPointInsideRectangle(Point point, Point upperLeftPoint, Point lowerRightPoint)
        {
            return point.X >= upperLeftPoint.X && point.X <= lowerRightPoint.X && point.Y >= upperLeftPoint.Y &&
                   point.Y <= lowerRightPoint.Y;
        }

        private bool IsPossiblePutRectangle(Rectangle currentRectangle)
        {
            foreach (var existingRectangle in Rectangles)
            {
                if (IsRectanglesIntersect(currentRectangle, existingRectangle))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsRectanglesIntersect(Rectangle r1, Rectangle r2)
        {
            var intersection = Rectangle.Intersect(r1, r2);
            return !(intersection.Width==0 && intersection.Height==0);
        }

        public bool IsRectangleInsideCloud(Rectangle rectangle)
        {
            var intersection = Rectangle.Intersect(rectangle, new Rectangle(0, 0, Width, Height));
            return intersection.Width == rectangle.Width && rectangle.Height == intersection.Height;
        }


        public void CreateBitmap(string path)
        {
            Bitmap bitmap = new Bitmap(Width, Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            foreach (var rectangle in Rectangles)
            {
                graphics.FillRectangle(Brushes.Blue, rectangle);
            }
            bitmap.Save(path);          
            //bitmap.Save(@"C:\Users\Максим\Desktop\ШПоРА\Практика\01.TDD\tdd\TagsCloudVisualization\bin\Debug\1.b");          
        }
    }
}