﻿using API.Data;
using API.ErrorHandling;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace API.Services
{
    public class MessagesService : IMessagesService
    {
        private DataContext _context;
        private readonly IConfiguration _config;
        private readonly string _openAiApiKey;

        public MessagesService(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _openAiApiKey = _config["OpenAiApiKey"];
        }

        /// <inheritdoc/>
        public async Task<ActionResult<Message>> PostMessage(string sender, string recipient, string text)
        {
            var senderUser = await _context.Users.SingleOrDefaultAsync(x => x.UserName == sender);
            var recipientUser = await _context.Users.SingleOrDefaultAsync(x => x.UserName == recipient);

            // validate sender exists
            if (senderUser == null)
                throw new CustomException(400, "Sender does not exist");

            // validate recipient exists
            if (recipientUser == null)
                throw new CustomException(400, "Recipient does not exist");

            // validate recipient is not sender
            if (recipientUser.UserName == sender)
                throw new CustomException(400, "Cannot send messages to self");

            // strip new lines
            text = text.Replace("\n", " ").Replace("\r", " ");

            Message message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                Text = text
            };

            // update or insert conversation
            var conversationId = await UpsertConversation(message);
            message.ConversationId = conversationId.Value;

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            return message;
        }

        /// <inheritdoc/>
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesBySender(string sender)
        {
            var messages = await _context.Messages.Where(x => x.Sender == sender).ToListAsync();
            return messages;
        }

        /// <inheritdoc/>
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesByRecipient(string recipient)
        {
            var messages = await _context.Messages.Where(x => x.Recipient == recipient).ToListAsync();
            return messages;
        }

        /// <inheritdoc/>
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesBetweenUsers(string sender, string recipient)
        {
            var messages = await _context.Messages
                .Where(x => x.Sender == sender)
                .Where(y => y.Recipient == recipient)
                .OrderBy(z => z.CreateDate)
                .Take(5)
                .ToListAsync();

            return messages;
        }

        /// <inheritdoc/>
        public async Task<ActionResult<IEnumerable<Message>>> GetConversationBetweenUsers(string requestingUser, string otherUser, int range)
        {
            var messages = await _context.Messages
                .Where(x => x.Sender == requestingUser || x.Sender == otherUser)
                .Where(y => y.Recipient == otherUser || y.Recipient == requestingUser)
                .Where(z => z.Sender != z.Recipient)
                .OrderByDescending(z => z.CreateDate)
                .Take(range)
                .ToListAsync();

            messages.Sort((x, y) => x.CreateDate.CompareTo(y.CreateDate));

            return messages;
        }

        /// <inheritdoc/>
        public async Task<ActionResult<string>> GetAiMessageSuggestion(string requestingUser, string otherUser)
        {
            // generate ai message prompt
            var promptActionResult = await GetAiMessagePrompt(requestingUser, otherUser);
            var prompt = promptActionResult.Value;

            // send messages to OpenAiAPI
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("authorization", "Bearer " + _openAiApiKey);

            var promptObject = new
            {
                model = "text-davinci-001",
                prompt,
                temperature = 1,
                max_tokens = 100
            };

            var jsonContent = System.Text.Json.JsonSerializer.Serialize(promptObject); // escapes all special characters that would affect json
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/completions", content);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Error retrieving ai message suggestion: " + response.ReasonPhrase);

            // get message from response
            var responseString = await response.Content.ReadAsStringAsync();
            var dynamicText = JsonConvert.DeserializeObject<dynamic>(responseString).choices[0].text;
            var messageSuggestion = dynamicText.ToString();

            // strip requestingUser from response. OpenAiAPi consistently starts message with "{requestingUser}:"
            messageSuggestion = messageSuggestion.Replace($"{requestingUser}:", "").Trim();
            messageSuggestion = messageSuggestion.Replace("\n", " ").Replace("\r", " ");
            var messageSuggestionJson = $"{{\"AiMessageSuggestion\":\"{messageSuggestion}\"}}";

            return messageSuggestionJson;
        }

        /// <inheritdoc/>
        public async Task<ActionResult<bool>> DeleteMessage(string requestingUser, int id)
        {
            if (requestingUser != "Admin")
                throw new CustomException(400, "Only Admin can delete messages");

            var message = await _context.Messages.FindAsync(id);

            if (message == null)
                return false;

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<ActionResult<string>> GetAiMessagePrompt(string requestingUser, string otherUser)
        {
            // get conversation
            var conversationActionResult = await GetConversationBetweenUsers(requestingUser, otherUser, 10);
            List<Message> conversation = conversationActionResult.Value.ToList();

            // create prompt
            var prompt = $"You are {requestingUser}. Respond to the last message using the context of the entire conversation.";
            foreach (Message message in conversation)
            {
                var strippedMessage = $" {message.Sender}: {message.Text},"; // stripping minimizes OpenAiAPI token usage
                prompt = prompt + strippedMessage; // append prompt with message
            }

            return prompt;
        }

        private async Task<ActionResult<int>> UpsertConversation(Message message)
        {
            ConversationTracker conversation = await _context.ConversationTrackers
                .SingleOrDefaultAsync(c => 
                (c.UserA == message.Sender && c.UserB == message.Recipient) ||
                c.UserB == message.Sender && c.UserA == message.Recipient);

            if (conversation == null)
            {
                conversation = new ConversationTracker
                {
                    UserA = message.Sender,
                    UserB = message.Recipient,
                    MostRecentMessageDate = message.CreateDate
                };
                await _context.ConversationTrackers.AddAsync(conversation);
            }
            else
            {
                conversation.MostRecentMessageDate = message.CreateDate;
            }
            
            await _context.SaveChangesAsync();

            return conversation.Id;
            
        }
    }
}