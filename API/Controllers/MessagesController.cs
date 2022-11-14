using API.Data;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private DataContext _context;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context"></param>
        public MessagesController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a message.
        /// </summary>
        /// <param name="messageDto">Includes Sender, Recipient, and Text.</param>
        /// <returns>The created message.</returns>
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(MessageDto messageDto)
        {
            Message message = new Message
            {
                Sender = messageDto.Sender,
                Recipient = messageDto.Recipient,
                Text = messageDto.Text
            };

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            return message;
        }

        /// <summary>
        /// Retrieves list of messages by sender.
        /// </summary>
        /// <param name="sender">Sender to retrieve messages for.</param>
        /// <returns>List of messages with this sender.</returns>
        [HttpGet("sender/{sender}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesBySender(string sender)
        {
            var messages = await _context.Messages.Where(x => x.Sender == sender).ToListAsync();
            return messages;
        }

        /// <summary>
        /// Retrieves list of messages by recipient.
        /// </summary>
        /// <param name="recipient">Recipient to retrieve messages for.</param>
        /// <returns>List of messages with this recipient.</returns>
        [HttpGet("recipient/{recipient}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesByRecipient(string recipient)
        {
            var messages = await _context.Messages.Where(x => x.Recipient == recipient).ToListAsync();
            return messages;
        }

        /// <summary>
        /// Retrieves list of messages between a sender and recipient.
        /// </summary>
        /// <param name="sender">Sender to filter by.</param>
        /// <param name="recipient">Recipient to filter by.</param>
        /// <returns>List of messages with both this sender and recipient.</returns>
        [HttpGet("sender/{sender}/recipient/{recipient}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesBetweenUsers(string sender, string recipient)
        {
            var messages = await _context.Messages
                .Where(x => x.Sender == sender)
                .Where(y => y.Recipient == recipient).ToListAsync();

            return messages;
        }
    }
}
