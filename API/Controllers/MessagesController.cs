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
        public MessagesController(DataContext context)
        {
            _context = context;
        }

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

        [HttpGet("sender/{sender}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesBySender(string sender)
        {
            var messages = await _context.Messages.Where(x => x.Sender == sender).ToListAsync();
            return messages;
        }

        [HttpGet("recipient/{recipient}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesByRecipient(string recipient)
        {
            var messages = await _context.Messages.Where(x => x.Recipient == recipient).ToListAsync();
            return messages;
        }

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
