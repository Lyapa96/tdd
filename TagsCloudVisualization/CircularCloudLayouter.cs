using System;
using System.Collections.Generic;
using System.Drawing;



namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly int Width = 100;
        public readonly int Height = 100;

        public readonly Point CenterPoint;

        public List<Rectangle> Rectangles { get; set; }

        public Spiral Spiral;
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
            if (rectangleSize.Width > Width || rectangleSize.Height > Height) throw new RectangleIsNotPlacedException();
            if (Rectangles.Count == 0)
            {               
                var rectangleCenter = new Point(rectangleSize.Width/2, rectangleSize.Height/2);
                var rectangleLocation = new Point(CenterPoint.X - rectangleCenter.X, CenterPoint.Y - rectangleCenter.Y);
                var firstRectangle = new Rectangle(rectangleLocation, rectangleSize);
                Rectangles.Add(firstRectangle);

                return firstRectangle;
            }
            while (true)
            {
                try
                {
                    var nextPoint = Spiral.GetNextPoint();
                    if (IsIncorrectPoint(nextPoint))
                    {
                        continue;                       
                    }
                    var currentRectangle = new Rectangle(nextPoint, rectangleSize);
                    if (IsPossiblePutRectangle(currentRectangle) && IsRectangleInsideCloud(currentRectangle))
                    {
                        currentRectangle = ShiftRectangleToCenter(currentRectangle);
                        Rectangles.Add(currentRectangle);
                        return currentRectangle;
                    }
                }
                catch (SpiralException exception)
                {
                    throw new RectangleIsNotPlacedException(exception.Message);
                }
            }
        }


        private Rectangle ShiftRectangleToCenter(Rectangle currentRectangle)
        {
            currentRectangle = ShiftOnX(currentRectangle);
            currentRectangle = ShiftOnY(currentRectangle);
            return currentRectangle;
        }

        private Rectangle ShiftOnX(Rectangle rectangle)
        {
            if (rectangle.X > CenterPoint.X)
            {
                while (IsPossiblePutRectangle(rectangle) && rectangle.X > CenterPoint.X) //можно прилижать
                {
                    rectangle.X = rectangle.X - 1;
                }
                rectangle.X = rectangle.X + 1;
                return rectangle;
            }
            while (IsPossiblePutRectangle(rectangle) && rectangle.X < CenterPoint.X) //можно прилижать
            {
                rectangle.X = rectangle.X + 1;
            }
            rectangle.X = rectangle.X - 1;
            return rectangle;
        }
        private Rectangle ShiftOnY(Rectangle rectangle)
        {
            if (rectangle.Y > CenterPoint.Y)
            {
                while (IsPossiblePutRectangle(rectangle) && rectangle.Y > CenterPoint.Y) //можно прилижать
                {
                    rectangle.Y = rectangle.Y - 1;
                }
                rectangle.Y = rectangle.Y + 1;
                return rectangle;
            }
            while (IsPossiblePutRectangle(rectangle) && rectangle.Y < CenterPoint.Y) //можно прилижать
            {
                rectangle.Y = rectangle.Y + 1;
            }
            rectangle.Y = rectangle.Y - 1;
            return rectangle;
        }

        public void CreateSpiral(double densityOfSpiral = 0.001, double deltaInDegrees = 10)
        {
            Spiral = new Spiral(this, densityOfSpiral, deltaInDegrees);
        }

        private bool IsIncorrectPoint(Point point)
        {
            return point.X < 0 || point.Y < 0 || point.X > Width || point.Y > Height;
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
        }
    }
}