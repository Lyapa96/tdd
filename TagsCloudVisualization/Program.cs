using System.Drawing;
using System.IO;
using System.Reflection;


namespace TagsCloudVisualization
{
    public class Program
    {
        static void Main(string[] args)
        {            
            var path = Assembly.GetExecutingAssembly().Location;
            var tagsCloudsVisualization = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(path)));

            var lyricsDirectory = Path.Combine(tagsCloudsVisualization, "Lyrics");
            var imagesDirectory = Path.Combine(tagsCloudsVisualization,"Images");


            var text1 = File.ReadAllLines(Path.Combine(lyricsDirectory,"natali.txt"));
            CreateWords(imagesDirectory, "1.bmp",text1);

            var text2 = File.ReadAllLines(Path.Combine(lyricsDirectory, "eminem.txt"));
            CreateWords(imagesDirectory, "2.bmp",text2);

            CreateCloudWhithSmallRectangles(imagesDirectory, "3.bmp");

        }

        public static void CreateCloudWhithSmallRectangles(string applicationDirectory, string fileName)
        {
            var height = 500;
            var width = 500;
            var center = new Point(width / 2, height / 2);
            var cloud = new CircularCloudLayouter(center, width, height, 0.00001, 1);


            for (int i = 0; i < 400; i++)
            {
                cloud.PutNextRectangle(new Size(5,2));
            }

            var filePathToOpen = Path.Combine(applicationDirectory, fileName);

            cloud.CreateBitmap(filePathToOpen);
        }



        public static void CreateWords(string applicationDirectory, string fileName,string[] text)
        {
            var height = 600;
            var width = 600;
            var center = new Point(width / 2, height / 2);
            var cloud = new TagsCloud(text,center,width,height);

            cloud.CreateRectanglesForWords();

            var filePathToOpen = Path.Combine(applicationDirectory, fileName);

            cloud.CreateBirmapWithWords(filePathToOpen);
        }

    }
}