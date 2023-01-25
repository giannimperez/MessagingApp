using API.Data;
using API.DTOs;
using API.ErrorHandling;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class MessagesService : IMessagesService
    {
        private DataContext _context;

        public MessagesService(DataContext context)
        {
            _context = context;
        }


        /// <inheritdoc></inheritdoc>
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

        /// <inheritdoc></inheritdoc>
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesBySender(string sender)
        {
            var messages = await _context.Messages.Where(x => x.Sender == sender).ToListAsync();
            return messages;
        }

        /// <inheritdoc></inheritdoc>
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesByRecipient(string recipient)
        {
            var messages = await _context.Messages.Where(x => x.Recipient == recipient).ToListAsync();
            return messages;
        }

        /// <inheritdoc></inheritdoc>
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesBetweenUsers(string sender, string recipient)
        {
            var messages = await _context.Messages
                .Where(x => x.Sender == sender)
                .Where(y => y.Recipient == recipient).ToListAsync();

            return messages;
        }

        /// <inheritdoc></inheritdoc>
        public async Task<ActionResult<IEnumerable<Message>>> GetConversationBetweenUsers(string requestingUser, string otherUser)
        {
            var messages = await _context.Messages
                .Where(x => x.Sender == requestingUser || x.Sender == otherUser)
                .Where(y => y.Recipient == otherUser || y.Recipient == requestingUser).ToListAsync();


            // SET ReadByRecipient: true on each message in the messages list.
            foreach(Message message in messages)
            {
                if(message.Recipient == requestingUser)
                {
                    // message.ReadByRecipient = true;
                }
            }


            /*messages.AddRange(await _context.Messages
                .Where(x => x.Sender == user2)
                .Where(y => y.Recipient == user1).ToListAsync());*/

            messages.Sort((x, y) => x.CreateDate.CompareTo(y.CreateDate));

            return messages;
        }

        /// <inheritdoc></inheritdoc>
        public async Task<ActionResult<bool>> DeleteMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
                return false;

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return true;
        }

        

    }
}
