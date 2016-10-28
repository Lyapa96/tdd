using System.Drawing;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
    public class RectangleVisualizer : Form
    {
        public CircularCloudLayouter Cloud;
        public RectangleVisualizer(int width, int height)
        {
            Width = width;
            Height = height;
            Cloud = new CircularCloudLayouter(new Point(ClientSize.Width/ 2, ClientSize.Height/ 2), ClientSize.Width,ClientSize.Height,0.00001,1);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.FillRectangles(new SolidBrush(Color.Blue), Cloud.Rectangles.ToArray());
            g.FillRectangle(Brushes.Red, Cloud.CenterPoint.X, Cloud.CenterPoint.Y, 2, 2);
        }
    }
}