using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloudVisualization
{
    public class TagsCloud
    {
        public readonly IDictionary<string, int> Stats = new Dictionary<string, int>();

        public int AverageFrequency
        {
            get
            {
                if (Stats.Values.Count != 0)
                    return Stats.Values.Sum()/Stats.Values.Count;
                throw new ArgumentException();
            }
        }


        public Dictionary<WordInformation, Rectangle> WordToRectangle = new Dictionary<WordInformation, Rectangle>();
        public CircularCloudLayouter Cloud;


        public TagsCloud(string[] textLines, Point center, int width, int height)
        {
            CreateFrequencyDictionary(textLines);
            Cloud = new CircularCloudLayouter(center, width, height,0.0001,10);

        }

        private void CreateFrequencyDictionary(string[] textLines)
        {
            var words = textLines.SelectMany(w => Regex.Split(w, @"\W+"))
                .Select(word => word.ToLower()).ToArray();
            foreach (var word in words)
            {
                if (Stats.ContainsKey(word))
                {
                    Stats[word]++;
                }
                else
                {
                    Stats.Add(word, 1);
                }
            }
        }

        public void CreateRectanglesForWords()
        {
            
            foreach (var word in Stats.Keys)
            {
                if (Stats[word] > 1.5*AverageFrequency)
                {
                    PutWord(word,WordSize.Big);
                }
                else if (Stats[word] <  0.3*AverageFrequency)
                {
                    PutWord(word,WordSize.Normal);
                }
                else
                {
                    PutWord(word,WordSize.Small);
                }
            }
        }

        public void PutWord(string word,WordSize size)
        {
            var wordInformation = new WordInformation(word, size);
            var rectangle = Cloud.PutNextRectangle(wordInformation.RectangleSize);
            WordToRectangle.Add(wordInformation, rectangle);
        }

        public void CreateBirmapWithWords(string path)
        {
            Bitmap bitmap = new Bitmap(Cloud.Width, Cloud.Height);
            Graphics g = Graphics.FromImage(bitmap);
            foreach (var wordInformation in WordToRectangle.Keys)
            {
                var rectangle = WordToRectangle[wordInformation];
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                
                g.DrawString(wordInformation.Word, wordInformation.Font, Brushes.Aqua, rectangle);
            }
            bitmap.Save(path);
        }
    }

}