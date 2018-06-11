using System;
using System.Collections.Generic;
using Foundation.Data;
using Foundation.Messaging;
using Foundation.Utils;
using PPM.Commands;
using PPM.Entities;

namespace PPM.CommandHandlers
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        private readonly IRepository _repository;
        public CreateUserCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(CreateUserCommand command)
        {
            var user = command.MapToEntity<User>();
            user.Roles = new List<Role>();
            //foreach (var roleId in command.RoleIds)
            //{
            //    user.Roles.Add(_repository.Get<Role>(roleId));
            //}

            SaltedHash saltedHash = SaltedHash.Create(command.ConfirmPassword);
            user.Salt = saltedHash.Salt;
            user.HashedPassword = saltedHash.Hash;
            user.LastLoggedIn = DateTime.Now;
            _repository.Create(user);
        }
    }
}