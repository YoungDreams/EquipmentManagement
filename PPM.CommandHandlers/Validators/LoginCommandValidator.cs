using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Foundation.Data;
using Foundation.Utils;
using PPM.Commands;
using PPM.Entities;

namespace PPM.CommandHandlers.Validators
{
    public class LoginCommandValidator : BaseValidator<LoginCommand>
    {
        private readonly IRepository _repository;
        public LoginCommandValidator(IRepository repository)
        {
            _repository = repository;
            RuleFor(x => x.Username).NotNull().WithMessage("用户名不能为空");
            RuleFor(x => x.Password).NotNull().WithMessage("密码不能为空");

            Custom(x =>
            {
                var user =
                    _repository.Query<User>("SELECT * FROM [User] WHERE Username=@Username",
                            new { x.Username })
                        .FirstOrDefault();

                if (user == null)
                {
                    return new ValidationFailure("Username", "用户不存在");
                }

                if (!user.IsEnabled)
                {
                    return new ValidationFailure("Username", "用户已失效");
                }

                if (!SaltedHash.Create(user.Salt, user.HashedPassword).Verify(x.Password))
                {
                    return new ValidationFailure("Password", "用户名或密码有误");
                }

                return null;
            });
        }
    }

}
