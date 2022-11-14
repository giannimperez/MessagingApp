using API.Data;
using API.DTOs;
using API.ErrorHandling;
using API.Interfaces;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountsService _accountsService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="accountsService"></param>
        public AccountsController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="registerDto">Includes desired username and password.</param>
        /// <returns>UserDto which includes username and token.</returns>
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            try
            {
                return await _accountsService.CreateUser(registerDto);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves UserDto.
        /// </summary>
        /// <param name="loginDto">Includes username and password of existing account.</param>
        /// <returns>UserDto which includes username and token.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            try
            {
                return await _accountsService.Login(loginDto);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }
    }
}