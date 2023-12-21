namespace DataAccessLayer.Exceptions
{
    public class MultipleResultsException : Exception
    {
        public MultipleResultsException() : base("Multiple results returned when only one was expected") { }

    }
}