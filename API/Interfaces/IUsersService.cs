using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUsersService
    {
        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>List of all users.</returns>
        Task<ActionResult<IEnumerable<User>>> GetUsers();

        /// <summary>
        /// Retrieves a user by Id.
        /// </summary>
        /// <param name="id">Id of the user to retrieve.</param>
        /// <returns>User object.</returns>
        Task<ActionResult<User>> GetUserById(int id);

        /// <summary>
        /// Retrieves a user by username.
        /// </summary>
        /// <param name="username">Username to search for.</param>
        /// <returns>User object.</returns>
        Task<ActionResult<MemberDto>> GetUserByUsername(string username);

        /// <summary>
        /// Retrieves all users whose usernames match a partial username except the requesting user.
        /// </summary>
        /// <param name="requestingUser">Username of user requesting list.</param>
        /// <param name="partialUsername">Partial username to search for.</param>
        /// <returns>List of users containing the partial username excluding the requesting user.</returns>
        Task<ActionResult<List<MemberDto>>> GetUserListByUsername(string requestingUser, string partialUsername);

        /// <summary>
        /// Retrieves all users who have at least one message with the specified user.
        /// </summary>
        /// <param name="username">The user which each returned user must have message(s) with.</param>
        /// <returns>List of users who have message(s) with the specified user.</returns>
        Task<ActionResult<IEnumerable<MemberDto>>> GetUsersWithConversations(string username);

        /// <summary>
        /// Deletes a user by Id.
        /// </summary>
        /// <param name="requestingUser">Username of user requesting delete.</param>
        /// <param name="id">Id of user to delete.</param>
        /// <returns>Message reporting deletion success.</returns>
        Task<ActionResult<string>> DeleteUserById(string requestingUser, int id);
    }
}
