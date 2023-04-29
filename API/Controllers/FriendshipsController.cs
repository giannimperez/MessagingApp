using API.Data;
using API.DTOs;
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
    [Authorize]
    public class FriendshipsController : ControllerBase
    {
        private DataContext _context;

        public FriendshipsController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Friendship>> CreateFriendship(FriendshipDto friendshipDto)
        {

            var user1 = await _context.Users.Where(u => u.UserName == friendshipDto.RequesterName).SingleOrDefaultAsync();
            var user2 = await _context.Users.Where(u => u.UserName == friendshipDto.AddresseeName).SingleOrDefaultAsync();

            if (user1 == null || user2 == null)
                return BadRequest("User not found");

            Friendship friendship = new Friendship
            {
                Requester = user1,
                Addressee = user2
            };

            await _context.Friendships.AddAsync(friendship);
            _context.SaveChanges();

            return friendship;
        }


        // TODO: Implement get friendshipbyid w/ return type FriendshipDTO. Might need to rethink DB entity and DTO.
        [HttpGet("{id}")]
        public async Task<ActionResult<int>> GetFriendshipById(int id)
        {

            // This is great for getting list of all friendships w/ users. But, not what I need. :(
            var userFriendshipData = await _context.Users.Join(_context.Friendships, u => u.Id, f => f.Requester.Id, (u, f) => new
            {
                UserId = u.Id,
                UserId2 = u.Id,
                UserName = u.UserName,
                FriendshipId = f.Id,
                RequesterId = f.Requester.Id,
                AddresseeId = f.Addressee.Id
            }).ToListAsync();

            /*
            // LINQ (using query syntax)
            var userFriendship = from f in _context.Friendships
                                 join u in _context.Users on f.Requester equals u.Id
                                 select new
                                 {

                                 }
                        FriendshipDto friendshipDto = new FriendshipDto
                        {
                            RequesterName = requester.testName,
                            AddresseeName = addressee.testName
                        };
            // LINQ (using method syntax)
            var users = from f in _context.Friendships
                        join u in _context.Users on f.Requester.Id equals u.Id
                        where f.Id == id
                        select User;
            */
            return 1;
        }
    }
}
