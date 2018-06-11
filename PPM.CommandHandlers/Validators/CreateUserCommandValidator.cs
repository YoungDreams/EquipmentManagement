using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Foundation.Data;
using PPM.Commands;
using PPM.Entities;

namespace PPM.CommandHandlers.Validators
{
    public class CreateUserCommandValidator : BaseValidator<CreateUserCommand>
    {
        private readonly IRepository _repository;
        public CreateUserCommandValidator(IRepository repository)
        {
            _repository = repository;
            RuleFor(x => x.Username).NotNull().WithMessage("用户名不能为空");
            RuleFor(x => x.Password).NotNull().WithMessage("密码不能为空");
            RuleFor(x => x.Username).NotNull().WithMessage("确认密码不能为空");
            RuleFor(x => x.Password).NotNull().WithMessage("邮箱不能为空");
            RuleFor(x => x.Username).NotNull().WithMessage("联系电话不能为空");

            Custom(x =>
            {
                if (x.Password != x.ConfirmPassword)
                {
                    return new ValidationFailure("Password", "两次密码输入不一致");
                }

                //var roles = _repository.Query<Role>().Where(r => x.RoleIds.Contains(r.Id));
                //if (roles.Any(r => r.RoleType == RoleType.超级管理员) && roles.Count() > 1)
                //{
                //    return new ValidationFailure("RoleIds", "超级管理员不能和其他角色一起选择");
                //}

                var exsitUser = _repository.Query<User>("SELECT * FROM [User] WHERE Username = @Username", new { Username = x.Username }).FirstOrDefault();

                if (exsitUser != null)
                {
                    return new ValidationFailure("Username", "用户已存在");
                }
                return null;
            });
        }
    }
}