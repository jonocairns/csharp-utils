[Serializable]
    public class RedisCacheJsonConvertException : Exception
    {
        public RedisCacheJsonConvertException()
        {
        }

        public RedisCacheJsonConvertException(string message)
            : base(message)
        {
        }

        public RedisCacheJsonConvertException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected RedisCacheJsonConvertException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }