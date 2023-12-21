
using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IUserRepository : IEntityRepository<User>
    {

        Task<User> GetByUsername(string username);
    }
}
