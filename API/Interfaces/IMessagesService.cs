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
        /// <param name="sender">Username of sender.</param>
        /// <param name="recipient">Username of recipient.</param>
        /// <param name="text">Content of the message.</param>
        /// <returns>The created message.</returns>
        Task<ActionResult<Message>> PostMessage(string sender, string recipient, string text);

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
        /// Retrieves list of messages between two users ordered by CreateDate descending.
        /// </summary>
        /// <param name="user1">User in conversation.</param>
        /// <param name="user2">Other user in conversation.</param>
        /// <param name="range">Number of messages to return.</param>
        /// <returns>List of messages between two users, ordered by CreateDate.</returns>
        Task<ActionResult<IEnumerable<Message>>> GetConversationBetweenUsers(string user1, string user2, int range);

        /// <summary>
        /// Retrieves a message suggestion from OpenAiAPI for an existing conversation.
        /// </summary>
        /// <param name="requestingUser">User who is requesting the message suggestion.</param>
        /// <param name="otherUser">Other user in conversation.</param>
        /// <returns>A message suggestion from OpenAiAPI, from the requestingUser's perspective.</returns>
        Task<ActionResult<string>> GetAiMessageSuggestion(string requestingUser, string otherUser);

        /// <summary>
        /// Deletes message by id.
        /// </summary>
        /// <param name="requestingUser">Username of user requesting delete.</param>
        /// <param name="id">Id of message to delete.</param>
        /// <returns>True if message deleted; otherwise, false.</returns>
        Task<ActionResult<bool>> DeleteMessage(string requestingUser, int id);
    }
}