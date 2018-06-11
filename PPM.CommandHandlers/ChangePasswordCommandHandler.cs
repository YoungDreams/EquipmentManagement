using Foundation.Data;
using Foundation.Messaging;
using Foundation.Utils;
using PPM.CommandHandlers.Exceptions;
using PPM.Commands;
using PPM.Entities;

namespace PPM.CommandHandlers
{
    public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
    {
        private readonly IRepository _repository;

        public ChangePasswordCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(ChangePasswordCommand command)
        {
            var user = _repository.Get<User>(command.UserId);

            if (!SaltedHash.Create(user.Salt, user.HashedPassword).Verify(command.OldPassword))
            {
                throw new CommandValidationException("旧密码不正确");
            }
            if (command.Password != command.ConfirmPassword)
            {
                throw new CommandValidationException("两次密码输入不一致");
            }
            SaltedHash saltedHash = SaltedHash.Create(command.Password);

            user.Salt = saltedHash.Salt;
            user.HashedPassword = saltedHash.Hash;

            _repository.Update(user);
        }
    }
}