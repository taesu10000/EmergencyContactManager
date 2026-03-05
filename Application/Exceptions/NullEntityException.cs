namespace Application.Exceptions
{
    public class NullEntityException : AppException
    {
        public NullEntityException() : base("Errors.NullEntity") { }
    }
}
