using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IBookRepository : IEntityRepository<Book>
    {
        public Task<Book> GetByISBN(string ISBN);

        public Task<IEnumerable<Book>> SearchByTitle(string title);
        public Task<IEnumerable<Book>> SearchByAuthor(string author);

    }
}