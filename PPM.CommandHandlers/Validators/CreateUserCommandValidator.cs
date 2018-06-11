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
            RuleFor(x => x.Username).NotNull().WithMessage("�û�������Ϊ��");
            RuleFor(x => x.Password).NotNull().WithMessage("���벻��Ϊ��");
            RuleFor(x => x.Username).NotNull().WithMessage("ȷ�����벻��Ϊ��");
            RuleFor(x => x.Password).NotNull().WithMessage("���䲻��Ϊ��");
            RuleFor(x => x.Username).NotNull().WithMessage("��ϵ�绰����Ϊ��");

            Custom(x =>
            {
                if (x.Password != x.ConfirmPassword)
                {
                    return new ValidationFailure("Password", "�����������벻һ��");
                }

                //var roles = _repository.Query<Role>().Where(r => x.RoleIds.Contains(r.Id));
                //if (roles.Any(r => r.RoleType == RoleType.��������Ա) && roles.Count() > 1)
                //{
                //    return new ValidationFailure("RoleIds", "��������Ա���ܺ�������ɫһ��ѡ��");
                //}

                var exsitUser = _repository.Query<User>("SELECT * FROM [User] WHERE Username = @Username", new { Username = x.Username }).FirstOrDefault();

                if (exsitUser != null)
                {
                    return new ValidationFailure("Username", "�û��Ѵ���");
                }
                return null;
            });
        }
    }
}