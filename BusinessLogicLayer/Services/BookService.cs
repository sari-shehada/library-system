using BusinessLogicLayer.Entities;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace BusinessLogicLayer.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBorrowingRepository _borrowingRepository;


        public BookService(IBookRepository bookRepository, IBorrowingRepository borrowingRepository)
        {
            _bookRepository = bookRepository;
            _borrowingRepository = borrowingRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _bookRepository.GetAll();
        }

        public async Task<Book> GetByISBN(string ISBN)
        {
            return await _bookRepository.GetByISBN(ISBN);
        }

        public async Task<IEnumerable<Book>> SearchByAuthor(string author)
        {
            return await _bookRepository.SearchByAuthor(author);
        }

        public async Task<IEnumerable<Book>> SearchByTitle(string title)
        {
            return await _bookRepository.SearchByTitle(title);
        }
        public async Task ReturnBook(Guid borrowingId)
        {
            Borrowing borrowing = await _borrowingRepository.GetById(borrowingId);
            if (borrowing.ReturnDate != null)
            {
                return;
            }
            //TODO: make it a transaction
            Book book = await _bookRepository.GetById(borrowing.BookId);
            borrowing.ReturnDate = DateOnly.FromDateTime(DateTime.Now);
            await _borrowingRepository.Update(borrowing);
            book.IsBorrowed = false;
            await _bookRepository.Update(book);
            return;
        }

        public async Task<Borrowing> BorrowBook(Guid UserId, Guid BookId)
        {
            Book book = await _bookRepository.GetById(BookId);
            //TODO: Add user existence validation
            if (book.IsBorrowed)
            {
                throw new BookBorrowedException();
            }
            //TODO: make it a transaction
            Borrowing borrowing = new Borrowing()
            {
                UserId = UserId,
                BookId = BookId,
            };
            await _borrowingRepository.Insert(borrowing);
            book.IsBorrowed = true;
            await _bookRepository.Update(book);
            return borrowing;
        }

        public async Task<IEnumerable<BorrowedBookDTO>> GetUserBorrowedBooks(Guid userId)
        {
            List<BorrowedBookDTO> result = new List<BorrowedBookDTO>();
            IEnumerable<Borrowing> borrowings = await _borrowingRepository.GetAll();
            borrowings = borrowings.Where((e) => e.UserId == userId && e.ReturnDate == null).ToList();
            foreach (Borrowing borrowing in borrowings)
            {
                Book book = await _bookRepository.GetById(borrowing.BookId);
                result.Add(new BorrowedBookDTO(book, borrowing));
            }
            return result;
        }

        public async Task<Book> AddNewBook(Book book)
        {
            if (book.ISBN.Length != 13 || !int.TryParse(book.ISBN, out _))
            {
                throw new InvalidISBNFormatException();
            }
            return await _bookRepository.Insert(book);
        }
    }
}