using System.Data.Common;
using DataAccessLayer.Config;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Interfaces;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;

namespace DataAccessLayer.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ConnectionSetting _connection;

        public BookRepository(IOptions<ConnectionSetting> connection)
        {
            _connection = connection.Value;
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            List<Book> books = new();
            await _connection.ExecuteWithConnection(async (connection) =>
            {
                string commandText = "select * from books";
                MySqlCommand command = new MySqlCommand(commandText, connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        books.Add(MapRowToEntity(reader));
                    }
                }
            });
            return books;
        }

        public async Task<Book> GetById(Guid id)
        {
            List<Book> books = new();
            await _connection.ExecuteWithConnection(async (connection) =>
            {
                string commandText = "select * from books where Id = @Id";
                MySqlCommand command = new MySqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        throw new NoResultException();
                    }
                    while (await reader.ReadAsync())
                    {
                        books.Add(MapRowToEntity(reader));
                        if (books.Count > 1)
                        {
                            throw new MultipleResultsException();
                        }
                    }
                }
            });
            return books.First();
        }

        public async Task<Book> GetByISBN(string ISBN)
        {
            List<Book> books = new();
            await _connection.ExecuteWithConnection(async (connection) =>
            {
                string commandText = "select * from books where ISBN = @ISBN";
                MySqlCommand command = new MySqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@ISBN", ISBN);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        throw new NoResultException();
                    }
                    while (await reader.ReadAsync())
                    {
                        books.Add(MapRowToEntity(reader));
                        if (books.Count > 1)
                        {
                            throw new MultipleResultsException();
                        }
                    }
                }
            });
            return books.First();
        }

        public async Task<Book> Insert(Book entity)
        {
            await _connection.ExecuteWithConnection(async (connection) =>
            {
                string commandText = "Insert INTO books (`Id`,`Title`,`Author`,`ISBN`) VALUES (@Id,@Title,@Author,@ISBN);";
                MySqlCommand command = new MySqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@Id", entity.Id);
                command.Parameters.AddWithValue("@Title", entity.Title);
                command.Parameters.AddWithValue("@Author", entity.Author);
                command.Parameters.AddWithValue("@ISBN", entity.ISBN);
                using var reader = await command.ExecuteReaderAsync();
            });
            return entity;
        }

        public async Task<IEnumerable<Book>> SearchByAuthor(string author)
        {

            List<Book> books = new();
            await _connection.ExecuteWithConnection(async (connection) =>
            {
                string commandText = "SELECT * FROM `books` WHERE books.Author LIKE @author;";
                MySqlCommand command = new MySqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@author", "%" + author + "%");
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        books.Add(MapRowToEntity(reader));
                    }
                }
            });
            return books;
        }

        public async Task<IEnumerable<Book>> SearchByTitle(string title)
        {

            List<Book> books = new();
            await _connection.ExecuteWithConnection(async (connection) =>
            {
                string commandText = "SELECT * FROM `books` WHERE books.Title LIKE @title;";
                MySqlCommand command = new MySqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@title", "%" + title + "%");
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        books.Add(MapRowToEntity(reader));
                    }
                }
            });
            return books;
        }

        public async Task<Book> Update(Book entity)
        {

            await _connection.ExecuteWithConnection(async (connection) =>
            {
                string commandText = "UPDATE `books` SET `Title`= @Title,`Author`= @Author,`IsBorrowed`= @IsBorrowed,`ISBN`= @ISBN WHERE books.Id = @Id;";
                MySqlCommand command = new MySqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@Title", entity.Title);
                command.Parameters.AddWithValue("@Author", entity.Author);
                command.Parameters.AddWithValue("@IsBorrowed", entity.IsBorrowed);
                command.Parameters.AddWithValue("@ISBN", entity.ISBN);
                command.Parameters.AddWithValue("@Id", entity.Id);


                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.RecordsAffected == 0)
                    {
                        throw new UpdateFailedException();
                    }
                }
            });
            return entity;
        }

        public async void Delete(Guid id)
        {
            await _connection.ExecuteWithConnection(async (connection) =>
            {
                string commandText = "DELETE FROM `books` WHERE books.Id = @Id;";
                MySqlCommand command = new MySqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.RecordsAffected == 0)
                    {
                        throw new DeleteFailedException();
                    }
                    return;
                }
            });
        }

        public Book MapRowToEntity(DbDataReader reader)
        {
            return new Book()
            {
                Id = Guid.Parse(reader.GetValue("Id").ToString()!),
                Title = reader.GetValue("Title").ToString()!,
                Author = reader.GetValue("Author").ToString()!,
                IsBorrowed = bool.Parse(reader.GetValue("IsBorrowed").ToString()!),
                ISBN = reader.GetValue("ISBN").ToString()!,
            };
        }

    }
}