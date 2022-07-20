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

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return BadRequest("User not found");

            return user;
        }

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


        [HttpGet("age/{id}")]
        public async Task<ActionResult<int>> GetUserAgeById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user.GetAge();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return $"User with Id: {user.Id} deleted.";
        }
    }
}