namespace BusinessLogicLayer.Exceptions
{
    public class MultipleUsersWithSameCredentialsException : Exception
    {
        public MultipleUsersWithSameCredentialsException() : base("Multiple users found with the same username and password combination") { }
    }
}