namespace Application.Exceptions
{
    public class CreateContactException : AppException
    {
        public CreateContactException() : base("Errors.InvalidInput") { }
    }
}
