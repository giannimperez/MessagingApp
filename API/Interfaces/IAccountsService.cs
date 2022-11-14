using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IAccountsService
    {
        Task<ActionResult<UserDto>> CreateUser(RegisterDto registerDto);
        Task<ActionResult<UserDto>> Login(LoginDto loginDto);
    }
}
