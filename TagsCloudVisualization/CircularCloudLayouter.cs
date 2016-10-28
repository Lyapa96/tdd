using System;
using System.Collections.Generic;
using System.Drawing;


namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly int Width;
        public readonly int Height;
        public readonly Point CenterPoint;

        public List<Rectangle> Rectangles { get; set; }
        public Spiral Spiral;

        public CircularCloudLayouter(Point center, int width, int height, double densityOfSpiral = 0.001,
            double deltaOfSpiralInDegrees = 10)
        {
            Width = width;
            Height = height;
            Rectangles = new List<Rectangle>();
            if (GeometryHelper.IsIncorrectPoint(center, width, height)) throw new ArgumentException();
            CenterPoint = center;
            Spiral = new Spiral(center, width, height, densityOfSpiral, deltaOfSpiralInDegrees);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width > Width || rectangleSize.Height > Height) throw new ArgumentException();
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
                var nextPoint = Spiral.GetNextPoint();
                if (GeometryHelper.IsIncorrectPoint(nextPoint, Width, Height))
                {
                    continue;
                }
                var currentRectangle = new Rectangle(nextPoint, rectangleSize);
                if (IsPossiblePutRectangle(currentRectangle))
                {
                    currentRectangle = RectangleHelper.ShiftRectangleToCenter(currentRectangle, CenterPoint, Rectangles);
                    Rectangles.Add(currentRectangle);
                    return currentRectangle;
                }
            }
        }

        private bool IsPossiblePutRectangle(Rectangle currentRectangle)
        {
            return RectangleHelper.IsRectangleDoesNotIntersectsWithRectangles(currentRectangle, Rectangles) &&
                   RectangleHelper.IsRectangleInsideOtherRectangle(currentRectangle, Width, Height);
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