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

        private Point centerPoint;
        public Point CenterPoint
        {
            get { return centerPoint; }
            private set
            {
                if (value.X < 0 || value.Y < 0 || value.X > Width || value.Y > Height) throw new ArgumentException();
                centerPoint = value;
            }
        }

        public List<Rectangle> Rectangles { get; set; }

        public CircularCloudLayouter(Point center)
        {
            Rectangles = new List<Rectangle>();
            CenterPoint = center;            
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle(centerPoint, rectangleSize);           
            Rectangles.Add(rectangle);
            return rectangle;
        }

        public static void Main(string[] args)
        {
            
        }
    }
}