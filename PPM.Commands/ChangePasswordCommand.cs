using System.ComponentModel.DataAnnotations;
using Foundation.Messaging;

namespace PPM.Commands
{
    public class ChangePasswordCommand : Command
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "旧密码不能为空")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "新密码不能为空")]
        public string Password { get; set; }
        [Required(ErrorMessage = "新确认密码不能为空")]
        public string ConfirmPassword { get; set; }
    }
}