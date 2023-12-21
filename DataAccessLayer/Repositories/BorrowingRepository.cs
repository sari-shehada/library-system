using DataAccessLayer.Config;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Interfaces;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace DataAccessLayer.Repositories
{
    public class BorrowingRepository : IBorrowingRepository
    {
        private readonly ConnectionSetting _connection;


        public BorrowingRepository(IOptions<ConnectionSetting> connection)
        {
            _connection = connection.Value;
        }



        public async Task<IEnumerable<Borrowing>> GetAll()
        {
            List<Borrowing> borrowings = new();
            await _connection.ExecuteWithConnection(async (connection) =>
            {
                string commandText = "select * from borrowings";
                MySqlCommand command = new MySqlCommand(commandText, connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        borrowings.Add(MapRowToEntity(reader));
                    }
                }
            });
            return borrowings;
        }

        public async Task<Borrowing> GetById(Guid id)
        {
            List<Borrowing> borrowings = new();
            await _connection.ExecuteWithConnection(async (connection) =>
            {
                string commandText = "select * from borrowings where Id = @Id";
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
                        borrowings.Add(MapRowToEntity(reader));
                        if (borrowings.Count > 1)
                        {
                            throw new MultipleResultsException();
                        }
                    }
                }
            });
            return borrowings.First();
        }



        public async Task<Borrowing> Insert(Borrowing entity)
        {
            await _connection.ExecuteWithConnection(async (connection) =>
            {
                string commandText = "INSERT INTO `borrowings` (`Id`, `UserId`, `BookId`, `BorrowDate`, `ReturnDate`) VALUES (@Id,@UserId,@BookId,@BorrowDate,@ReturnDate);";
                MySqlCommand command = new MySqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@Id", entity.Id);
                command.Parameters.AddWithValue("@UserId", entity.UserId);
                command.Parameters.AddWithValue("@BookId", entity.BookId);
                command.Parameters.AddWithValue("@BorrowDate", entity.BorrowDate);
                command.Parameters.AddWithValue("@ReturnDate", entity.ReturnDate == null ? DBNull.Value : entity.ReturnDate);
                using var reader = await command.ExecuteReaderAsync();
            });
            return entity;
        }



        public async Task<Borrowing> Update(Borrowing entity)
        {
            await _connection.ExecuteWithConnection(async (connection) =>
           {
               string commandText = "UPDATE `borrowings` SET `UserId`= @UserId,`BookId`= @BookId,`BorrowDate`= @BorrowDate,`ReturnDate`= @ReturnDate WHERE borrowings.Id = @Id;";
               MySqlCommand command = new MySqlCommand(commandText, connection);
               command.Parameters.AddWithValue("@UserId", entity.UserId);
               command.Parameters.AddWithValue("@BookId", entity.BookId);
               command.Parameters.AddWithValue("@BorrowDate", entity.BorrowDate);
               command.Parameters.AddWithValue("@ReturnDate", entity.ReturnDate);
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
                string commandText = "DELETE FROM `borrowings` WHERE borrowings.Id = @Id;";
                MySqlCommand command = new MySqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.RecordsAffected == 0)
                    {
                        throw new DeleteFailedException();
                    }
                }
            });
        }

        public Borrowing MapRowToEntity(DbDataReader reader)
        {
            string? ReturnDate = reader.GetValue("ReturnDate").ToString() ?? null;
            return new Borrowing()
            {
                Id = Guid.Parse(reader.GetValue("Id").ToString()!),
                UserId = Guid.Parse(reader.GetValue("UserId").ToString()!),
                BookId = Guid.Parse(reader.GetValue("BookId").ToString()!),
                BorrowDate = DateTime.Parse(reader.GetValue("BorrowDate").ToString()!),
                ReturnDate = ReturnDate == null ? null : DateTime.Parse(ReturnDate),
            };
        }
    }

}