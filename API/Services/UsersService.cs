﻿using API.Data;
using API.DTOs;
using API.ErrorHandling;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class UsersService : IUsersService
    {
        private DataContext _context;

        public UsersService(DataContext context)
        {
            _context = context;
        }

        /// <inheritdoc></inheritdoc>
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        /// <inheritdoc></inheritdoc>
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new CustomException(400, "User not found");

            return user;
        }

        /// <inheritdoc></inheritdoc>
        public async Task<ActionResult<MemberDto>> GetUserByUsername(string username)
        {
            var user = await _context.Users.Where(u => u.UserName == username).SingleOrDefaultAsync();

            if (user == null)
                throw new CustomException(400, "User not found");

            MemberDto memberDto = new MemberDto
            {
                Id = user.Id,
                UserName = user.UserName,
                IsActive = user.IsActive
            };

            return memberDto;
        }

        /// <inheritdoc></inheritdoc>
        public async Task<ActionResult<List<MemberDto>>> GetUserListByUsername(string username)
        {
            var users = await _context.Users.Where(u => u.UserName.ToLower().Contains(username.ToLower())).ToListAsync();

            List<MemberDto> returnUsers = new List<MemberDto>();

            foreach (var user in users)
            {
                MemberDto memberDto = new MemberDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    IsActive = user.IsActive
                };
                returnUsers.Add(memberDto);
            }

            return returnUsers;
        }

        /// <inheritdoc></inheritdoc>
        public async Task<ActionResult<int>> GetUserAgeById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user.GetAge();
        }

        /// <inheritdoc></inheritdoc>
        public async Task<ActionResult<string>> DeleteUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new CustomException(400, "User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return $"User {user.Id} deleted.";
        }
    }
}