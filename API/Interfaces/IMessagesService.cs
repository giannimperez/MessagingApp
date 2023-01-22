using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IMessagesService
    {

        /// <summary>
        /// Creates a message.
        /// </summary>
        /// <param name="messageDto">Includes Sender, Recipient, and Text.</param>
        /// <returns>The created message.</returns>
        Task<ActionResult<Message>> PostMessage(MessageDto messageDto);

        /// <summary>
        /// Retrieves list of messages by sender.
        /// </summary>
        /// <param name="sender">Sender to retrieve messages for.</param>
        /// <returns>List of messages with this sender.</returns>
        Task<ActionResult<IEnumerable<Message>>> GetMessagesBySender(string sender);

        /// <summary>
        /// Retrieves list of messages by recipient.
        /// </summary>
        /// <param name="recipient">Recipient to retrieve messages for.</param>
        /// <returns>List of messages with this recipient.</returns>
        Task<ActionResult<IEnumerable<Message>>> GetMessagesByRecipient(string recipient);

        /// <summary>
        /// Retrieves list of messages between a sender and recipient.
        /// </summary>
        /// <param name="sender">Sender to filter by.</param>
        /// <param name="recipient">Recipient to filter by.</param>
        /// <returns>List of messages with both this sender and recipient.</returns>
        Task<ActionResult<IEnumerable<Message>>> GetMessagesBetweenUsers(string sender, string recipient);

        /// <summary>
        /// Deletes message by id.
        /// </summary>
        /// <param name="id">Id of message to delete.</param>
        /// <returns>True if message deleted; otherwise, false.</returns>
        Task<ActionResult<bool>> DeleteMessage(int id);
    }
}
