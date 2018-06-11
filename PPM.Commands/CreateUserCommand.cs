using System.Collections.Generic;
using Foundation.Messaging;
using PPM.Entities;

namespace PPM.Commands
{
    public class CreateUserCommand : Command
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsEnabled { get; set; }
        public string RealName { get; set; }
        public List<string> RoleIds { get; set; }
        public RoleType RoleType { get; set; }
    }
}