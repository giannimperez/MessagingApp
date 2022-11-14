using API.Data;
using API.DTOs;
using API.ErrorHandling;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Services
{
    public class AccountsService : IAccountsService
    {
        private DataContext _context;
        private ITokenService _tokenService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tokenService"></param>
        public AccountsService(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }


        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="registerDto">Includes desired username and password.</param>
        /// <returns>UserDto which includes username and token.</returns>
        public async Task<ActionResult<UserDto>> CreateUser(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username))
                throw new CustomException(400, "Username already taken");

            CheckAge(registerDto.DateOfBirth);

            var user = new User
            {
                UserName = registerDto.Username,
                DateOfBirth = registerDto.DateOfBirth
            };
            EncodePassword(user, registerDto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto { Username = user.UserName, Token = _tokenService.CreateToken(user) };
        }


        /// <summary>
        /// Retrieves UserDto.
        /// </summary>
        /// <param name="loginDto">Includes username and password of existing account.</param>
        /// <returns>UserDto which includes username and token.</returns>
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            // Validate username
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);
            if (user == null)
                throw new CustomException(400, "Incorrect username");

            // Validate password
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PaswordHash[i])
                    throw new CustomException(400, "Incorrect password");
            }

            return new UserDto { Username = user.UserName, Token = _tokenService.CreateToken(user) };
        }


        /// <summary>
        /// Checks if a user exists.
        /// </summary>
        /// <param name="username">Username to check.</param>
        /// <returns>True if user exists</returns>
        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username);
        }


        /// <summary>
        /// Encodes a user defined password into a hash and salt, to be stored in db.
        /// </summary>
        /// <param name="user">User save encode password for.</param>
        /// <param name="password">Password to encode.</param>
        private void EncodePassword(User user, string password) // TODO: reuse for password changes
        {
            using var hmac = new HMACSHA512();

            user.PaswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            user.PasswordSalt = hmac.Key;
        }


        /// <summary>
        /// Checks age.
        /// </summary>
        /// <param name="dateOfBirth">DateTime to check.</param>
        /// <returns>Age</returns>
        /// <exception cref="CustomException"></exception>
        private int CheckAge(DateTime dateOfBirth)
        {
            var currentDate = DateTime.Today;
            var age = currentDate.Year - dateOfBirth.Year;
            
            if (age < 13)
                throw new CustomException(400, "Must be 13 years or older");

            return age;
        }
    }
}
