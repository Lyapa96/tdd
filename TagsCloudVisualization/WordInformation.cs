using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class WordInformation
    {
        private readonly Dictionary<WordSize, int> fontSize = new Dictionary<WordSize, int>()
        {
            {WordSize.Big, 30},
            {WordSize.Normal, 20},
            {WordSize.Small, 10}
        };

        public readonly Font Font;
        public readonly string Word;
        public readonly Size RectangleSize;

        public WordInformation(string word, WordSize size)
        {
            Font = new Font("Arial", fontSize[size]);
            Word = word;
            RectangleSize = new Size((int) (word.Length*Font.Size), Font.Height);
        }
    }
}