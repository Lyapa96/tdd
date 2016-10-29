﻿using System;
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
                    currentRectangle = ShiftRectangleToCenter(currentRectangle);
                    Rectangles.Add(currentRectangle);
                    return currentRectangle;
                }
            }
        }

        public bool IsRectangleDoesNotIntersectsWithRectanglesFromCloud(Rectangle currentRectangle)
        {
            foreach (var existingRectangle in Rectangles)
            {
                if (GeometryHelper.IsRectanglesIntersect(currentRectangle, existingRectangle))
                {
                    return false;
                }
            }
            return true;
        }
        private bool IsPossiblePutRectangle(Rectangle currentRectangle)
        {
            return IsRectangleDoesNotIntersectsWithRectanglesFromCloud(currentRectangle) &&
                   GeometryHelper.IsRectangleInsideOtherRectangle(currentRectangle, Width, Height);
        }

        public Rectangle ShiftRectangleToCenter(Rectangle currentRectangle)
        {
            currentRectangle = ShiftOn(currentRectangle, true);
            currentRectangle = ShiftOn(currentRectangle, false);
            return currentRectangle;
        }

        private Rectangle ShiftOn(Rectangle rectangle, bool isXCoordinate)
        {
            var currentCoordinate = isXCoordinate ? rectangle.X : rectangle.Y;
            var centerCoordinate = isXCoordinate ? CenterPoint.X : CenterPoint.Y;
            var step = currentCoordinate > centerCoordinate ? -1 : 1;
            while (IsRectangleDoesNotIntersectsWithRectanglesFromCloud(rectangle) && currentCoordinate!=centerCoordinate)
            {
                currentCoordinate += step;
                rectangle = MakeShift(rectangle, step, isXCoordinate);
            }
            rectangle = MakeShift(rectangle, -step, isXCoordinate);
            return rectangle;

        }

        private Rectangle MakeShift(Rectangle rectangle, int step, bool isXCoordinate)
        {
            if (isXCoordinate)
            {
                rectangle.X += step;
            }
            else
            {
                rectangle.Y += step;
            }
            return rectangle;
        }

        public void CreateBitmap(string path)
        {
            Bitmap bitmap = new Bitmap(Width, Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            foreach (var rectangle in Rectangles)
            {
                graphics.FillRectangle(Brushes.Blue, rectangle);
                //graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                //StringFormat sf = new StringFormat();
                //sf.Alignment = StringAlignment.Center;
                //sf.LineAlignment = StringAlignment.Center;
                //graphics.DrawString("Текст по центру", new Font("Times", 15), Brushes.White, rectangle, sf);
            }
            bitmap.Save(path);
        }

        


    }
}