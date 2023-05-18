using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IAccountsService
    {
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="registerDto">Registration DTO containing Username, Password and DateOfBirth.</param>
        /// <returns>A User DTO containing Username and JWT.</returns>
        Task<ActionResult<UserDto>> CreateUser(RegisterDto registerDto);

        /// <summary>
        /// Authenticates user via Username and Password.
        /// </summary>
        /// <param name="loginDto">Login DTO containing Username and Password.</param>
        /// <returns>A User DTO containing Username and JWT.</returns>
        Task<ActionResult<UserDto>> Login(LoginDto loginDto);
    }
}
