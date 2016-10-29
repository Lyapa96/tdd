using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    public class TagsCloud_should
    {
        [Test]
        public void createFrequencyDictionary()
        {
            var text = new[] {"a b a, b a", "a b c"};
            var tagsCloud = new TagsCloud(text, new Point(50, 50), 100, 100);


            tagsCloud.Stats["a"].Should().Be(4);
            tagsCloud.Stats["b"].Should().Be(3);
            tagsCloud.Stats["c"].Should().Be(1);
        }

        [Test]
        public void FindAverageFrequency()
        {
            var text = new[] {"a a a a a", "b b b c"};
            var tagsCloud = new TagsCloud(text, new Point(50, 50), 100, 100);
            Assert.That(tagsCloud.AverageFrequency, Is.EqualTo(3));
        }

    }
}