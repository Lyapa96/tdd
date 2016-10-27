using System;
using System.Runtime.Serialization;

namespace TagsCloudVisualization
{
    public class SpiralException : Exception
    {
        public SpiralException()
        {
        }

        public SpiralException(string message) : base(message)
        {
        }

        public SpiralException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SpiralException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}