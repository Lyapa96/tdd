using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleHelper
    {
        public static bool IsRectanglesIntersect(Rectangle r1, Rectangle r2)
        {
            var intersection = Rectangle.Intersect(r1, r2);
            return !(intersection.Width == 0 && intersection.Height == 0);
        }

        public static bool IsRectangleInsideOtherRectangle(Rectangle rectangle,int width,int height)
        {
            var intersection = Rectangle.Intersect(rectangle, new Rectangle(0, 0, width, height));
            return intersection.Width == rectangle.Width && rectangle.Height == intersection.Height;
        }
        
        public static bool IsRectangleDoesNotIntersectsWithRectangles(Rectangle currentRectangle, IEnumerable<Rectangle> otherRectangles)
        {
            foreach (var existingRectangle in otherRectangles)
            {
                if (IsRectanglesIntersect(currentRectangle, existingRectangle))
                {
                    return false;
                }
            }
            return true;
        }

        public static Rectangle ShiftOnY(Rectangle rectangle, int coordinateY, List<Rectangle> otherRectangles)
        {
            var step = rectangle.Y > coordinateY ? 1 : -1;
            while (IsRectangleDoesNotIntersectsWithRectangles(rectangle, otherRectangles) && Math.Abs(rectangle.Y - coordinateY) != 0)
            {
                rectangle.Y -= step;
            }
            rectangle.Y += step;
            return rectangle;
        }

        public static Rectangle ShiftOnX(Rectangle rectangle, int coordinateX, List<Rectangle> otherRectangles)
        {
            var step = rectangle.X > coordinateX ? 1 : -1;
            while (IsRectangleDoesNotIntersectsWithRectangles(rectangle, otherRectangles) && Math.Abs(rectangle.X - coordinateX) != 0)
            {
                rectangle.X -= step;
            }
            rectangle.X += step;
            return rectangle;
        }

        public static Rectangle ShiftRectangleToCenter(Rectangle currentRectangle,Point center, List<Rectangle > otherRectangles)
        {
            currentRectangle = ShiftOnX(currentRectangle, center.X, otherRectangles);
            currentRectangle = ShiftOnY(currentRectangle, center.Y, otherRectangles);
            return currentRectangle;
        }
    }
}