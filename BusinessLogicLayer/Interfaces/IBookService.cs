using BusinessLogicLayer.Entities;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Interfaces
{
    public interface IBookService
    {
        public Task<IEnumerable<Book>> GetAllBooks();

        public Task<Book> AddNewBook(Book book);
        public Task<IEnumerable<Book>> SearchByTitle(string title);
        public Task<IEnumerable<Book>> SearchByAuthor(string author);
        public Task<Book> GetByISBN(string ISBN);

        public Task<Borrowing> BorrowBook(Guid userId, Guid bookId);
        public Task ReturnBook(Guid borrowingId);

        public Task<IEnumerable<BorrowedBookDTO>> GetUserBorrowedBooks(Guid userId);

    }
}