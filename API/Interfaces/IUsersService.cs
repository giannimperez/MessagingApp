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
        /// Retrieves all users whose usernames match a partial username.
        /// </summary>
        /// <param name="username">Partial username to search for.</param>
        /// <returns>List of users containing the partial username.</returns>
        Task<ActionResult<List<MemberDto>>> GetUserListByUsername(string username);

        /// <summary>
        /// Retrieves all users who have at least one message with the specified user.
        /// </summary>
        /// <param name="username">The user which each returned user must have message(s) with.</param>
        /// <returns>List of users who have message(s) with the specified user.</returns>
        Task<ActionResult<IEnumerable<MemberDto>>> GetUsersWithConversations(string username);

        /// <summary>
        /// Retrieves a users age by Id.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <returns>Age of the user.</returns>
        Task<ActionResult<int>> GetUserAgeById(int id);

        /// <summary>
        /// Deletes a user by Id.
        /// </summary>
        /// <param name="id">Id of user to delete.</param>
        /// <returns>Message reporting deletion success.</returns>
        Task<ActionResult<string>> DeleteUserById(int id);
    }
}
