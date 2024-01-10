using ClinicManagement.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;

        // Constructor with dependency injection for ApplicationDbContext
        public ChatHub(AppDbContext context)
        {
            _context = context;
        }

        // Hub method to send a message to a specific user
        public async Task SendMessageToUser(string userId, string message)
        {
            // Get the user ID of the sender from the SignalR context
            var senderId = Context.UserIdentifier;

            // Save the message to the database
            var chatMessage = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = userId,
                Message = message
            };

            // Add the ChatMessage entity to the database context
            _context.ChatMessages.Add(chatMessage);

            // Persist changes to the database
            await _context.SaveChangesAsync();

            // Send the message to the specified user using SignalR
            await Clients.User(userId).SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
        }
        public async Task LoadChatHistory(string userId)
        {
            var senderId = Context.UserIdentifier;
            var messages = await _context.ChatMessages
                .Where(m => (m.SenderId == senderId && m.ReceiverId == userId) || (m.SenderId == userId && m.ReceiverId == senderId))
                .OrderBy(m => m.Timestamp)
                .Select(m => new { UserName = m.Sender.UserName, Message = m.Message, Timestamp = m.Timestamp })
                .ToListAsync();

            // Send the chat history to the caller
            await Clients.Caller.SendAsync("LoadChatHistory", messages);
        }
    }
}
