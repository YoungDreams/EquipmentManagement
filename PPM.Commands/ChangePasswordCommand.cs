using System.ComponentModel.DataAnnotations;
using Foundation.Messaging;

namespace PPM.Commands
{
    public class ChangePasswordCommand : Command
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "�����벻��Ϊ��")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "�����벻��Ϊ��")]
        public string Password { get; set; }
        [Required(ErrorMessage = "��ȷ�����벻��Ϊ��")]
        public string ConfirmPassword { get; set; }
    }
}