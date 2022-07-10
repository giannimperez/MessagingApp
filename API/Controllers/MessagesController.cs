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
        public async Task<ActionResult<IEnumerable<Message>>> PostMessage(MessageDto messageDto)
        {
            Message message = new Message
            {
                Sender = messageDto.Sender,
                Recipient = messageDto.Recipient,
                Text = messageDto.Text
            };

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            return Ok(message);
        }

        [HttpGet("sender/{sender}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesBySender(string sender)
        {
            var messages = _context.Messages.Where(x => x.Sender == sender).ToListAsync();
            return Ok(await messages);
        }

        [HttpGet("recipient/{recipient}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesByRecipient(string recipient)
        {
            var message = _context.Messages.Where(x => x.Recipient == recipient).ToListAsync();
            return Ok(await message);
        }

        [HttpGet("{sender}/{recipient}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesBetweenUsers(string sender, string recipient)
        {

            var messages = _context.Messages
                .Where(x => x.Sender == sender)
                .Where(y => y.Recipient == recipient).ToListAsync();

            return Ok(await messages);
        }
    }
}
