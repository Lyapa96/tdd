using System;
using System.Drawing;


namespace TagsCloudVisualization
{
    public class Spiral
    {
        private readonly int width;
        private readonly int height;
        private Point center;

        private readonly double deltaInDegrees;
        private readonly double deltaFromDensity;
        private double currentDensityOfSpiral;
        private double currentAngle = 0;
        public double CurrentRadius { get; private set; }

        public readonly double MaxRadius;

        public Spiral(CircularCloudLayouter cloud, double currentDensityOfSpiral, double deltaInDegrees)
            : this(cloud.CenterPoint, cloud.Width, cloud.Height, currentDensityOfSpiral, deltaInDegrees)
        {
        }

        public Spiral(Point center, int width, int height, double currentDensityOfSpiral,
            double deltaInDegrees)
        {
            this.center = center;
            this.width = width;
            this.height = height;
            this.deltaInDegrees = deltaInDegrees;
            this.currentDensityOfSpiral = currentDensityOfSpiral;
            CurrentRadius = currentAngle*currentDensityOfSpiral;
            deltaFromDensity = currentDensityOfSpiral*0.1;
            MaxRadius = GetMaxRadius();
        }

        public Point GetNextPoint()
        {
            if (CurrentRadius > MaxRadius) throw new SpiralException();
            currentAngle += deltaInDegrees;
            currentDensityOfSpiral += deltaFromDensity;
            CurrentRadius = currentAngle*currentDensityOfSpiral;

            var x = (int) Math.Round(CurrentRadius*Math.Cos(currentAngle/180*Math.PI)) + center.X;
            var y = (int) Math.Round(CurrentRadius*Math.Sin(currentAngle/180*Math.PI)) + center.Y;

            var nextPoint = new Point(x, y);
            return nextPoint;
        }

        private double GetMaxRadius()
        {
            var upperLeftCorner = new Point(0, 0);
            var lowerLeftCorner = new Point(0, height);
            var upperRightCorner = new Point(width, 0);
            var lowerRightCorner = new Point(width, height);

            return
                Math.Max(
                    Math.Max(GetDistanceBetweenPoints(center, upperLeftCorner),
                        GetDistanceBetweenPoints(center, upperRightCorner)),
                    Math.Max(GetDistanceBetweenPoints(center, lowerLeftCorner),
                        GetDistanceBetweenPoints(center, lowerRightCorner)));
        }

        private double GetDistanceBetweenPoints(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }
    }  
}