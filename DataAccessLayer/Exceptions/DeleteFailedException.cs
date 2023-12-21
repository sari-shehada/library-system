namespace DataAccessLayer.Exceptions
{
    public class DeleteFailedException : Exception
    {
        public DeleteFailedException() : base("Failed to delete record, either the record does not exists or has already been deleted")
        {

        }
    }
    public class UpdateFailedException : Exception
    {
        public UpdateFailedException() : base("Failed to update record, presumably because the record does not exist")
        {

        }
    }
}