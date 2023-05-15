using API.Data;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using API.Interfaces;
using API.ErrorHandling;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

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
                var sender = _tokenService.GetUserFromAuthHeader(HttpContext.Request.Headers["Authorization"]);

                return await _messagesService.PostMessage(sender, messageDto.Recipient, messageDto.Text);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

/*        /// <summary>
        /// Retrieves list of messages by sender.
        /// </summary>
        /// <param name="sender">Sender to retrieve messages for.</param>
        /// <returns>List of messages with this sender.</returns>
        [HttpGet("sender/{sender}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesBySender(string sender)
        {
            try
            {
                return await _messagesService.GetMessagesBySender(sender);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }*/

/*        /// <summary>
        /// Retrieves list of messages by recipient.
        /// </summary>
        /// <param name="recipient">Recipient to retrieve messages for.</param>
        /// <returns>List of messages with this recipient.</returns>
        [HttpGet("recipient/{recipient}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesByRecipient(string recipient)
        {
            try
            {
                return await _messagesService.GetMessagesByRecipient(recipient);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }*/

/*        /// <summary>
        /// Retrieves list of messages between a sender and recipient.
        /// </summary>
        /// <param name="sender">Sender to filter by.</param>
        /// <param name="recipient">Recipient to filter by.</param>
        /// <returns>List of messages with both this sender and recipient.</returns>
        [HttpGet("sender/{sender}/recipient/{recipient}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesBetweenUsers(string sender, string recipient)
        {
            try
            {
                return await _messagesService.GetMessagesBetweenUsers(sender, recipient);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }*/

        [HttpGet("{otherUser}/{range}/conversation")]
        public async Task<ActionResult<IEnumerable<Message>>> GetConversationBetweenUsers(string otherUser, int range)
        {
            try
            {
                var requestingUser = _tokenService.GetUserFromAuthHeader(HttpContext.Request.Headers["Authorization"]);

                return await _messagesService.GetConversationBetweenUsers(requestingUser, otherUser, range);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
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
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }
    }
}