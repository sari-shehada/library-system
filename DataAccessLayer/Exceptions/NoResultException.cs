namespace DataAccessLayer.Exceptions
{
    public class NoResultException : Exception
    {
        public NoResultException() : base("No Results Where Found")
        {

        }
    }
}