﻿using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using API.ErrorHandling;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private IMessagesService _messagesService;
        private ITokenService _tokenService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="messagesService">MessagesService interface.</param>
        /// <param name="tokenService">TokenService interface.</param>
        public MessagesController(IMessagesService messagesService, ITokenService tokenService)
        {
            _messagesService = messagesService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Creates a message.
        /// </summary>
        /// <param name="messageDto">Includes Recipient and Text.</param>
        /// <returns>The created message.</returns>
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(MessageDto messageDto)
        {
            try
            {
                var sender = await _tokenService.GetUsernameFromAuthHeader(HttpContext.Request.Headers["Authorization"]);

                return await _messagesService.PostMessage(sender, messageDto.Recipient, messageDto.Text);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.ToJson());
            }
        }

        /// <summary>
        /// Retrieves a list of messages between two users ordered by CreateDate descending.
        /// </summary>
        /// <param name="otherUser">Other user in conversation.</param>
        /// <param name="range">Number of messages to return.</param>
        /// <returns>List of messages between two users.</returns>
        [HttpGet("{otherUser}/{range}/conversation")]
        public async Task<ActionResult<IEnumerable<Message>>> GetConversationBetweenUsers(string otherUser, int range)
        {
            try
            {
                var requestingUser = await _tokenService.GetUsernameFromAuthHeader(HttpContext.Request.Headers["Authorization"]);

                return await _messagesService.GetConversationBetweenUsers(requestingUser, otherUser, range);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.ToJson());
            }
        }

        /// <summary>
        /// Retrieves a message suggestion from OpenAiAPI for an existing conversation.
        /// </summary>
        /// <param name="otherUser">Other user in conversation.</param>
        /// <returns>A message suggestion from OpenAiAPI, from the requestingUser's perspective.</returns>
        [HttpGet("{otherUser}/aisuggestmessage")]
        public async Task<ActionResult<string>> GetAiMessageSuggestion(string otherUser)
        {
            try
            {
                var requestingUser = await _tokenService.GetUsernameFromAuthHeader(HttpContext.Request.Headers["Authorization"]);

                return await _messagesService.GetAiMessageSuggestion(requestingUser, otherUser);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.ToJson());
            }
        }

        /// <summary>
        /// Deletes a user by id.
        /// </summary>
        /// <param name="id">Id of user to delete.</param>
        /// <returns>True if message deleted; otherwise, false.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteMessage(int id)
        {
            try
            {
                var requestingUser = await _tokenService.GetUsernameFromAuthHeader(HttpContext.Request.Headers["Authorization"]);

                return await _messagesService.DeleteMessage(requestingUser, id);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.ToJson());
            }
        }
    }
}