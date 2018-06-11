using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Foundation.Data;
using PPM.Commands;
using PPM.Entities;

namespace PPM.CommandHandlers.Validators
{
    public class EditUserCommandValidator : BaseValidator<EditUserCommand>
    {
        private readonly IRepository _repository;
        public EditUserCommandValidator(IRepository repository)
        {
            _repository = repository;
            RuleFor(x => x.Username).NotNull().WithMessage("用户名不能为空");
            RuleFor(x => x.Email).NotNull().WithMessage("邮箱不能为空");
            //RuleFor(x => x.Phone).NotNull().WithMessage("联系电话不能为空");

            Custom(x =>
            {
                //var roles = _repository.Query<Role>().Where(r => x.RoleIds.Contains(r.Id));
                //if (roles.Any(r => r.RoleType == RoleType.超级管理员) && roles.Count() > 1)
                //{
                //    return new ValidationFailure("RoleIds", "超级管理员不能和其他角色一起选择");
                //}
                return null;
            });
        }
    }
}