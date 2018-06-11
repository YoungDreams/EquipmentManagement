using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Data;
using Foundation.Messaging;
using PPM.Commands;
using PPM.Entities;

namespace PPM.CommandHandlers
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand>
    {
        private readonly IRepository _repository;

        public LoginCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(LoginCommand command)
        {
            var user =
                _repository.Query<User>().FirstOrDefault(x => x.Username == command.Username);
            if (user != null)
            {
                user.LastLoggedIn = DateTime.Now;
                _repository.Update(user);
            }
        }
    }
}
