using API.DTOs;
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
        /// <param name="context"></param>
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
                return StatusCode(ex.StatusCode, ex.MessageJson);
            }
        }

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
                return StatusCode(ex.StatusCode, ex.MessageJson);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteMessage(int id)
        {
            try
            {
                return await _messagesService.DeleteMessage(id);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.MessageJson);
            }
        }
    }
}