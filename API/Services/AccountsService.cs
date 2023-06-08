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

        public AccountsService(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        /// <inheritdoc/>
        public async Task<ActionResult<UserDto>> CreateUser(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username))
                throw new CustomException(400, "Username already taken");

            if (registerDto.Password.Length < 4 || registerDto.Password.Length > 30)
                throw new CustomException(400, "Password must be between 4 and 30 characters");

            CheckAge(registerDto.DateOfBirth);

            var user = new User
            {
                UserName = registerDto.Username,
                DateOfBirth = registerDto.DateOfBirth
            };
            await EncodeUsersPassword(user, registerDto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto { Username = user.UserName, Token = await _tokenService.CreateToken(user) };
        }

        /// <inheritdoc/>
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

            return new UserDto { Username = user.UserName, Token = await _tokenService.CreateToken(user) };
        }


        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username);
        }


        private async Task EncodeUsersPassword(User user, string password)
        {
            using var hmac = new HMACSHA512();

            user.PaswordHash = await Task.Run(() => hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            user.PasswordSalt = hmac.Key;
        }


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