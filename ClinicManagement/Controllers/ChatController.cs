using ClinicManagement.Models;
using ClinicManagement.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public ChatController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Retrieve a list of all users from the UserManager
            var users = await _userManager.Users.ToListAsync();

            // Create a list of SelectListItem to store user information for the view
            // Each item in the list represents a user with UserName as Text and Id as Value
            // Get the current user
            var currentUser = await _userManager.GetUserAsync(User);

            // Fetch initial messages for the current user
            var messages = await _context.ChatMessages
                .Where(m => m.SenderId == currentUser.Id || m.ReceiverId == currentUser.Id)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            var viewModel = new ChatViewModel
            {
                Users = users.Select(u => new SelectListItem { Text = u.UserName, Value = u.Id }).ToList(),
                Messages = messages,
                Heading = "Chat"
            };
            // Return the view
            return View(viewModel);
        }

    }
}
