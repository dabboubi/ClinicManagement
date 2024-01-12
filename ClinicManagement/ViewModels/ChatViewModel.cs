using ClinicManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.ViewModels
{
    public class ChatViewModel
    {
        public List<ChatMessage> Messages { get; set; }
        public List<SelectListItem> Users { get; set; }
        public string Heading { get; set; }
    }
}
