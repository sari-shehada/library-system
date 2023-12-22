namespace BusinessLogicLayer.Exceptions
{
    public class BookBorrowedException : Exception
    {
        public BookBorrowedException() : base("Book is already borrowed")
        {
        }

    }
}