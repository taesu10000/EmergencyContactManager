namespace Application.Exceptions
{
    public class DeserializationException : AppException
    {
        public DeserializationException() : base("Errors.ParseFailed") { }
    }
}
