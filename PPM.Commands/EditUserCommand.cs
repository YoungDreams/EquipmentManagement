using System.Collections.Generic;
using Foundation.Messaging;
using PPM.Entities;

namespace PPM.Commands
{
    public class EditUserCommand : Command
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsEnabled { get; set; }
        public string RealName { get; set; }
        public int? DepartmentId { get; set; }
        public RoleType RoleType { get; set; }
        //public List<int> RoleIds { get; set; }
        //public List<int> ProjectIds { get; set; }
    }
    public class DeleteUserCommand : Command
    {
        public int UserId { get; set; }
    }
    public class ValidUsersCommand : Command
    {
        public List<int> UserIds { get; set; }
    }
    public class InvalidUsersCommand : Command
    {
        public List<int> UserIds { get; set; }
    }
    public class ResetPasswordCommand : Command
    {
        public int UserId { get; set; }
    }
}