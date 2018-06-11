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
            RuleFor(x => x.Username).NotNull().WithMessage("�û�������Ϊ��");
            RuleFor(x => x.Email).NotNull().WithMessage("���䲻��Ϊ��");
            //RuleFor(x => x.Phone).NotNull().WithMessage("��ϵ�绰����Ϊ��");

            Custom(x =>
            {
                //var roles = _repository.Query<Role>().Where(r => x.RoleIds.Contains(r.Id));
                //if (roles.Any(r => r.RoleType == RoleType.��������Ա) && roles.Count() > 1)
                //{
                //    return new ValidationFailure("RoleIds", "��������Ա���ܺ�������ɫһ��ѡ��");
                //}
                return null;
            });
        }
    }
}