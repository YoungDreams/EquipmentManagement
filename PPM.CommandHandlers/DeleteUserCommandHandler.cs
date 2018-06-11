using Foundation.Data;
using Foundation.Messaging;
using PPM.CommandHandlers.Exceptions;
using PPM.Commands;
using PPM.Entities;
using PPM.Shared;

namespace PPM.CommandHandlers
{
    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
    {
        private readonly IRepository _repository;
        public DeleteUserCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(DeleteUserCommand command)
        {
            var user = _repository.Get<User>(command.UserId);
            if (user.Id == WebAppContext.Current.User.Id)
            {
                throw new CommandValidationException("不能删除自己");
            }
            _repository.Delete(user);
        }
    }
}