using BusinessLogicLayer.Entities;

namespace BusinessLogicLayer.Interfaces
{
    public interface IAuthService
    {
        public Task<UserDTO> GetUserInfoById(Guid userId);
        public Task<UserDTO> GetUserInfoByUsername(string username);

        //TODO: Change to return a token
        public Task<UserDTO> LoginUser(string username, string password);

    }
}