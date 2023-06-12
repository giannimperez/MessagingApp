using API.DTOs;
using API.ErrorHandling;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private IUsersService _usersService;
        private ITokenService _tokenService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="usersService">UserService interface.</param>
        /// <param name="tokenService">TokenService interface.</param>
        public UsersController(IUsersService usersService, ITokenService tokenService)
        {
            _usersService = usersService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>List of all users.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                return await _usersService.GetUsers();
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.ToJson());
            }
        }

        /// <summary>
        /// Retrieves a user by username.
        /// </summary>
        /// <param name="username">Username to search for.</param>
        /// <returns>User object.</returns>
        [HttpGet("username/{username}")]
        public async Task<ActionResult<MemberDto>> GetUserByUsername(string username)
        {
            try
            {
                return await _usersService.GetUserByUsername(username);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.ToJson());
            }
        }

        /// <summary>
        /// Retrieves all users whose usernames match a partial username.
        /// </summary>
        /// <param name="partialUsername">Partial username to search for.</param>
        /// <returns>List of users containing the partial username.</returns>
        [HttpGet("partialusername/{partialUsername}")]
        public async Task<ActionResult<List<MemberDto>>> GetUsersByPartialUsername(string partialUsername)
        {
            try
            {
                var requestingUser = await _tokenService.GetUsernameFromAuthHeader(HttpContext.Request.Headers["Authorization"]);

                return await _usersService.GetUsersByPartialUsername(requestingUser, partialUsername);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.ToJson());
            }
        }

        /// <summary>
        /// Retrieves a list of users that have open conversation with requesting user.
        /// </summary>
        /// <returns>List of users who have messages with the requesting user.</returns>
        [HttpGet("conversationlist")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsersWithConversations()
        {
            try
            {
                var requestingUser = await _tokenService.GetUsernameFromAuthHeader(HttpContext.Request.Headers["Authorization"]);

                return await _usersService.GetUsersWithConversations(requestingUser);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.ToJson());
            }
        }

        /// <summary>
        /// Deletes a user by Id.
        /// </summary>
        /// <param name="id">Id of user to delete.</param>
        /// <returns>Message reporting deletion success.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteUserById(int id)
        {
            try
            {
                var requestingUser = await _tokenService.GetUsernameFromAuthHeader(HttpContext.Request.Headers["Authorization"]);

                return await _usersService.DeleteUserById(requestingUser, id);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.ToJson());
            }
        }
    }
}