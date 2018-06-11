using Foundation.Data;
using Foundation.Messaging;
using PPM.Commands;
using PPM.Entities;

namespace PPM.CommandHandlers
{
    public class InvalidUsersCommandHandler : ICommandHandler<InvalidUsersCommand>
    {
        private readonly IRepository _repository;

        public InvalidUsersCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(InvalidUsersCommand command)
        {
            foreach (var userId in command.UserIds)
            {
                var user = _repository.Get<User>(userId);
                user.IsEnabled = false;
                _repository.Update(user);
            }
        }
    }
}