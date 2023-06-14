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
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsersWithConversations(string requestingUser)
        {
            var conversations = await _context.ConversationTrackers
                .Where(c => (c.UserA == requestingUser || c.UserB == requestingUser))
                .OrderByDescending(c => c.MostRecentMessageDate)
                .Distinct()
                .ToListAsync();

            var orderedFlattenedUsernames = conversations
                .SelectMany(c => new[] { c.UserA, c.UserB })
                .Where(u => u != requestingUser)
                .Distinct()
                .OrderByDescending(u => conversations
                    .FirstOrDefault(c => c.UserA == u || c.UserB == u).MostRecentMessageDate)
                .Take(10)
                .ToList();

            List<MemberDto> returnUsers = new List<MemberDto>();

            foreach (var username in orderedFlattenedUsernames)
            {
                var user = await _context.Users
                    .Where(u => u.UserName == username)
                    .SingleOrDefaultAsync();

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

        /// <inheritdoc/>
        public async Task<ActionResult<string>> DeleteUserById(string requestingUser, int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new CustomException(400, "User not found");

            if (requestingUser != "Admin")
                throw new CustomException(400, "Only Admin can delete user");

            // delete Messages to and from user
            var messagesToRemove = await _context.Messages
                .Where(m => m.Sender == user.UserName || m.Recipient == user.UserName)
                .ToListAsync();
            _context.Messages.RemoveRange(messagesToRemove);

            // delete ConversationTrackers which include user
            var conversationTrackersToRemove = await _context.ConversationTrackers
                .Where(c => c.UserA == user.UserName || c.UserB == user.UserName)
                .ToListAsync();
            _context.ConversationTrackers.RemoveRange(conversationTrackersToRemove);

            // delete user
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return $"User {user.Id} deleted.";
        }
    }
}