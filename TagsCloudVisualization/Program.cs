

using System.Drawing;
using System.IO;
using System.Net.Mime;
using System.Reflection;

namespace TagsCloudVisualization
{
    public class Program
    {
        static void Main(string[] args)
        {
            var path = Assembly.GetExecutingAssembly().Location;
            var applicationDirectory = Path.GetDirectoryName(path);

            CreateSimpleCloud(applicationDirectory, "1.bmp");
            CreateCloudWhithSmallRectangles(applicationDirectory,"2.bmp");
            CreateCloudWithFiveSimilarRectangles(applicationDirectory, "3.bmp");

        }


        public static void CreateSimpleCloud(string applicationDirectory,string fileName)
        {
            var height = 600;
            var width = 600;
            var center = new Point(width / 2, height / 2);
            var cloud = new CircularCloudLayouter(center, width, height);
            cloud.CreateSpiral();

            for (int i = 0; i < 3; i++)
            {
                cloud.PutNextRectangle(new Size(100, 20));
                cloud.PutNextRectangle(new Size(25, 10));
                cloud.PutNextRectangle(new Size(75, 50));
                cloud.PutNextRectangle(new Size(10, 5));
                cloud.PutNextRectangle(new Size(30, 10));
                cloud.PutNextRectangle(new Size(25, 10));
                cloud.PutNextRectangle(new Size(75, 50));
                cloud.PutNextRectangle(new Size(10, 5));
                cloud.PutNextRectangle(new Size(30, 10));
            }
            

            var filePathToOpen = Path.Combine(applicationDirectory, fileName);

            cloud.CreateBitmap(filePathToOpen);
        }

        public static void CreateCloudWhithSmallRectangles(string applicationDirectory, string fileName)
        {
            var height = 500;
            var width = 500;
            var center = new Point(width / 2, height / 2);
            var cloud = new CircularCloudLayouter(center, width, height);
            cloud.CreateSpiral();

            for (int i = 0; i < 200; i++)
            {
                cloud.PutNextRectangle(new Size(5,2));
            }

            var filePathToOpen = Path.Combine(applicationDirectory, fileName);

            cloud.CreateBitmap(filePathToOpen);
        }

        public static void CreateCloudWithFiveSimilarRectangles(string applicationDirectory, string fileName)
        {
            var height = 300;
            var width = 300;
            var center = new Point(width / 2, height / 2);
            var cloud = new CircularCloudLayouter(center, width, height);
            cloud.CreateSpiral();

            for (int i = 0; i < 5; i++)
            {
                cloud.PutNextRectangle(new Size(80, 80));
            }

            var filePathToOpen = Path.Combine(applicationDirectory, fileName);

            cloud.CreateBitmap(filePathToOpen);
        }
    }
}