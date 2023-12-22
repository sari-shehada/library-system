using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using BusinessLogicLayer.Entities;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLogicLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDTO> GetUserInfoById(Guid userId)
        {
            User user = await _userRepository.GetById(userId);
            return new UserDTO(user);
        }
        public async Task<UserDTO> GetUserInfoByUsername(string username)
        {
            User user = await _userRepository.GetByUsername(username);
            return new UserDTO(user);
        }

        public async Task<ClaimsIdentity> LoginUser(string username, string password)
        {
            try
            {
                if (username is null || password is null)
                {
                    throw new InvalidCredentialsException();
                }
                User user = await _userRepository.GetByUsername(username);
                if (user.Password == password)
                {
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
                    identity.AddClaim(new Claim("Id", user.Id.ToString()));
                    return identity;
                }
                throw new InvalidCredentialsException();
            }
            catch (Exception exception) when (exception is MultipleResultsException || exception is NoResultException)
            {
                throw new InvalidCredentialsException();
            }
        }

    }
}