

using System.Drawing;

namespace TagsCloudVisualization
{
    public class Program
    {
        static void Main(string[] args)
        {
            var height = 600;
            var width = 600;
            var center = new Point(width / 2, height / 2);
            var cloud = new CircularCloudLayouter(center, width, height);
            cloud.CreateSpiral();

            cloud.PutNextRectangle(new Size(100, 20));
            cloud.PutNextRectangle(new Size(25, 10));
            cloud.PutNextRectangle(new Size(75, 50));
            cloud.PutNextRectangle(new Size(10, 5));
            cloud.PutNextRectangle(new Size(30, 10));

            cloud.CreateBitmap();

        }
    }
}