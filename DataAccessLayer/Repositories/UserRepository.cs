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
    public class UserRepository : IUserRepository
    {
        private readonly ConnectionSetting _connection;

        public UserRepository(IOptions<ConnectionSetting> connection)
        {
            _connection = connection.Value;
        }


        public async Task<IEnumerable<User>> GetAll()
        {
            List<User> users = new();
            await _connection.ExecuteWithConnection(async (connection) =>
           {
               string commandText = "select * from users";
               MySqlCommand command = new MySqlCommand(commandText, connection);

               using (var reader = await command.ExecuteReaderAsync())
               {
                   while (await reader.ReadAsync())
                   {
                       users.Add(MapRowToEntity(reader));
                   }
               }
           });
            return users;
        }

        public async Task<User> GetById(Guid id)
        {
            List<User> users = new();
            await _connection.ExecuteWithConnection(async (connection) =>
            {
                string commandText = "select * from users where Id = @Id";
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
                        users.Add(MapRowToEntity(reader));
                        if (users.Count > 1)
                        {
                            throw new MultipleResultsException();
                        }
                    }
                }
            });
            return users.First();
        }
        public async Task<User> GetByUsername(string username)
        {
            List<User> users = new();
            await _connection.ExecuteWithConnection(async (connection) =>
            {
                string commandText = "select * from users where users.Username = @username";
                MySqlCommand command = new MySqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@username", username);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        throw new NoResultException();
                    }
                    while (await reader.ReadAsync())
                    {
                        users.Add(MapRowToEntity(reader));
                        if (users.Count > 1)
                        {
                            throw new MultipleResultsException();
                        }
                    }
                }
            });
            return users.First();
        }
        public async Task<User> Insert(User entity)
        {
            await _connection.ExecuteWithConnection(async (connection) =>
            {
                string commandText = "INSERT INTO `users`(`Id`, `Username`, `Password`, `FirstName`, `LastName`, `IsAdmin`) VALUES (@Id,@Username,@Password,@FirstName,@LastName,@IsAdmin);";
                MySqlCommand command = new MySqlCommand(commandText, connection);
                System.Console.WriteLine(entity.Id);
                command.Parameters.AddWithValue("@Id", entity.Id);
                command.Parameters.AddWithValue("@Username", entity.Username);
                command.Parameters.AddWithValue("@Password", entity.Password);
                command.Parameters.AddWithValue("@FirstName", entity.FirstName);
                command.Parameters.AddWithValue("@LastName", entity.LastName);
                command.Parameters.AddWithValue("@IsAdmin", entity.IsAdmin);
                using var reader = await command.ExecuteReaderAsync();
            });
            return entity;
        }


        public async Task<User> Update(User entity)
        {
            await _connection.ExecuteWithConnection(async (connection) =>
            {
                string commandText = "UPDATE `users` SET `Username`= @Username,`Password`= @Password,`FirstName`= @FirstName,`LastName`= @LastName,`IsAdmin`= @IsAdmin WHERE Id = @Id;";
                MySqlCommand command = new MySqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@Username", entity.Username);
                command.Parameters.AddWithValue("@Password", entity.Password);
                command.Parameters.AddWithValue("@FirstName", entity.FirstName);
                command.Parameters.AddWithValue("@LastName", entity.LastName);
                command.Parameters.AddWithValue("@IsAdmin", entity.IsAdmin);
                command.Parameters.AddWithValue("@Id", entity.Id);
                using var reader = await command.ExecuteReaderAsync();
                if (reader.RecordsAffected != 1)
                {
                    throw new UpdateFailedException();
                }
            });
            return entity;
        }
        public async void Delete(Guid id)
        {
            await _connection.ExecuteWithConnection(async (connection) =>
           {
               string commandText = "Delete from `users` WHERE Id = @Id;";
               MySqlCommand command = new MySqlCommand(commandText, connection);
               command.Parameters.AddWithValue("@Id", id);
               using var reader = await command.ExecuteReaderAsync();
               if (reader.RecordsAffected != 1)
               {
                   throw new DeleteFailedException();
               }
           });
        }
        public User MapRowToEntity(DbDataReader reader)
        {
            return new User()
            {
                Id = Guid.Parse(reader.GetValue("Id").ToString()!),
                FirstName = reader.GetValue("FirstName").ToString()!,
                LastName = reader.GetValue("LastName").ToString()!,
                Username = reader.GetValue("Username").ToString()!,
                Password = reader.GetValue("Password").ToString()!,
                IsAdmin = bool.Parse(reader.GetValue("IsAdmin").ToString()!)
            };
        }
    }
}