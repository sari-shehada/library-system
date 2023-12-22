using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Entities
{
    public class BorrowedBookDTO
    {
        public BorrowedBookDTO(Book book, Borrowing borrowing)
        {
            BorrowingId = borrowing.Id;
            BookId = book.Id;
            BookTitle = book.Title;
            BookAuthor = book.Author;
            BorrowDate = borrowing.BorrowDate;
        }

        public Guid BorrowingId { get; set; }

        public Guid BookId { get; set; }

        public string BookTitle { get; set; }

        public string BookAuthor { get; set; }

        public DateOnly BorrowDate { get; set; }


    }
}