using System;
using System.Runtime.Serialization;

namespace TagsCloudVisualization
{
    [Serializable]
    public class RectangleIsNotPlacedException : Exception
    {
       
        public RectangleIsNotPlacedException()
        {
        }

        public RectangleIsNotPlacedException(string message) : base(message)
        {
        }

        public RectangleIsNotPlacedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected RectangleIsNotPlacedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}