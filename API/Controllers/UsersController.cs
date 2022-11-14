using API.Data;
using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context"></param>
        public UsersController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>List of all users.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Retrieves a user by Id.
        /// </summary>
        /// <param name="id">Id of the user to retrieve.</param>
        /// <returns>User object.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return BadRequest("User not found");

            return user;
        }

        /// <summary>
        /// Retrieves a user by username.
        /// </summary>
        /// <param name="username">Username to search for.</param>
        /// <returns>User object.</returns>
        [HttpGet("username/{username}")]
        public async Task<ActionResult<MemberDto>> GetUserByUsername(string username)
        {
            var user = await _context.Users.Where(u => u.UserName == username).SingleOrDefaultAsync();

            if (user == null)
                return BadRequest("User not found");

            MemberDto memberDto = new MemberDto
            {
                Id = user.Id,
                UserName = user.UserName,
                IsActive = user.IsActive
            };

            return memberDto;
        }

        /// <summary>
        /// Retrieves all users whose usernames match a partial username.
        /// </summary>
        /// <param name="username">Partial username to search for.</param>
        /// <returns>List of users containing the partial username.</returns>
        [HttpGet("partialusername/{username}")]
        public async Task<ActionResult<List<MemberDto>>> GetUserListByUsername(string username)
        {
            var users = await _context.Users.Where(u => u.UserName.ToLower().Contains(username.ToLower())).ToListAsync();

            List<MemberDto> returnUsers = new List<MemberDto>();

            foreach(var user in users)
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

        /// <summary>
        /// Retrieves a users age by Id.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <returns>Age of the user.</returns>
        [HttpGet("age/{id}")]
        public async Task<ActionResult<int>> GetUserAgeById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user.GetAge();
        }

        /// <summary>
        /// Deletes a user by Id.
        /// </summary>
        /// <param name="id">Id of user to delete.</param>
        /// <returns>Message reporting deletion success.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return BadRequest("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return $"User {user.Id} deleted.";
        }
    }
}