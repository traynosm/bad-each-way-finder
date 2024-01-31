namespace bad_each_way_finder_domain.Exceptions
{
    public class LoginServiceException : Exception
    {
        public override string Message { get; }
        public Exception SourceException { get; set; }
        public LoginServiceException(Exception exception, string message)
        {
            SourceException = exception;
            Message = message;
        }
    }
}
