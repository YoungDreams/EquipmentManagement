using Foundation.Data;
using Foundation.Messaging;
using Foundation.Utils;
using PPM.Commands;
using PPM.Entities;

namespace PPM.CommandHandlers
{
    public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand>
    {
        private readonly IRepository _repository;
        public ResetPasswordCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(ResetPasswordCommand command)
        {
            var user = _repository.Get<User>(command.UserId);
            SaltedHash saltedHash = SaltedHash.Create("123456");

            user.Salt = saltedHash.Salt;
            user.HashedPassword = saltedHash.Hash;

            _repository.Update(user);
        }
    }
}