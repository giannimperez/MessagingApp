﻿using API.Data;
using API.DTOs;
using API.ErrorHandling;
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
    [Authorize]
    public class UsersController : ControllerBase
    {
        private IUsersService _usersService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context"></param>
        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
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
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a user by Id.
        /// </summary>
        /// <param name="id">Id of the user to retrieve.</param>
        /// <returns>User object.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                return await _usersService.GetUserById(id);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
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
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all users whose usernames match a partial username.
        /// </summary>
        /// <param name="username">Partial username to search for.</param>
        /// <returns>List of users containing the partial username.</returns>
        [HttpGet("partialusername/{username}")]
        public async Task<ActionResult<List<MemberDto>>> GetUserListByUsername(string username)
        {
            try
            {
                return await _usersService.GetUserListByUsername(username);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        [HttpGet("{username}/conversationlist")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsersWithConversations(string username)
        {
            try
            {
                return await _usersService.GetUsersWithConversations(username);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }


        /// <summary>
        /// Retrieves a users age by Id.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <returns>Age of the user.</returns>
        [HttpGet("age/{id}")]
        public async Task<ActionResult<int>> GetUserAgeById(int id)
        {
            try
            {
                return await _usersService.GetUserAgeById(id);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
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
                return await _usersService.DeleteUserById(id);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }
    }
}