namespace Application.Exceptions
{
    public class AppException : Exception
    {
        public string MessageKey { get; }
        public object[] MessageArgs { get; }
        protected AppException(string messageKey, params object[] args) : base(messageKey)
        {
            MessageKey = messageKey;
            MessageArgs = args;
        }
    }
}
