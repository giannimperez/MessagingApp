using API.Data;
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

        /// <inheritdoc/>
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new CustomException(400, "User not found");

            return user;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<ActionResult<List<MemberDto>>> GetUsersByPartialUsername(string requestingUser, string partialUsername)
        {
            List<User> users = new List<User>();
            users = await _context.Users.Where(u => u.UserName.ToLower().Contains(partialUsername.ToLower())).Take(10).ToListAsync();

            List<MemberDto> returnUsers = new List<MemberDto>();

            foreach (var user in users)
            {
                if (user.UserName != requestingUser) // skips requestingUser
                {
                    MemberDto memberDto = new MemberDto
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        IsActive = user.IsActive
                    };
                    returnUsers.Add(memberDto);
                }
            }

            return returnUsers;
        }

        /// <inheritdoc/>
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsersWithConversations(string username)
        {
            var users = await _context.Messages
                .Where(m => (m.Sender == username || m.Recipient == username))
                .Select(m => m.Sender == username ? m.Recipient : m.Sender)
                .Distinct()
                .Join(_context.Users, un => un, u => u.UserName, (un, u) => u)
                .ToListAsync();

            List<MemberDto> returnUsers = new List<MemberDto>();

            foreach (var user in users)
            {
                // doesn't include user provided in argument
                if (user.UserName != username) 
                {
                    MemberDto memberDto = new MemberDto
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        IsActive = user.IsActive
                    };
                    returnUsers.Add(memberDto);
                }
            }

            return returnUsers;
        }

        /// <inheritdoc/>
        public async Task<ActionResult<string>> DeleteUserById(string requestingUser, int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new CustomException(400, "User not found");

            if (requestingUser != "Admin")
                throw new CustomException(400, "Only Admin can delete user");

            // delete messages to and from user
            List<Message> messages = await _context.Messages.Where(m => m.Sender == user.UserName).ToListAsync();
            messages.AddRange(await _context.Messages.Where(m => m.Recipient == user.UserName).ToListAsync());

            foreach(var message in messages)
                _context.Messages.Remove(message);

            // delete user
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return $"User {user.Id} deleted.";
        }
    }
}