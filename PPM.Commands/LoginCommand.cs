using Foundation.Messaging;

namespace PPM.Commands
{
    public class LoginCommand : Command
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
