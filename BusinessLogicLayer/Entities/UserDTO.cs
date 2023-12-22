using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Entities
{
    public class UserDTO
    {
        public UserDTO(User user)
        {
            Id = user.Id;
            Username = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}