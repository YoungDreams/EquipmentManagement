using System;
using System.Collections.Generic;
using System.Linq;
using Foundation.Core;
using Foundation.Data;
using Foundation.Utils;

namespace PPM.Entities
{
    public class User : MyEntity
    {
        public virtual string Username { get; set; }
        public virtual string Salt { get; set; }
        public virtual string HashedPassword { get; set; }
        public virtual string Email { get; set; }
        public virtual string Phone { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual string RealName { get; set; }
        public virtual IList<Role> Roles { get; set; }
        public virtual RoleType RoleType { get; set; }
        public virtual DateTime? LastLoggedIn { get; set; }
        //public virtual IList<Project> Projects { get; set; }
        //public virtual Department Department { get; set; }

        public static User System
        {
            get
            {
                var password = Guid.NewGuid().ToString("N");
                var saltedHash = SaltedHash.Create(password);
                var repository = ServiceLocator.Current.Resolve<IRepository>();
                var systemUser = repository.Query<User>().SingleOrDefault(x => x.Username == "系统");
                if (systemUser == null)
                {
                    systemUser = new User
                    {
                        Username = "系统",
                        RealName = "系统",
                        IsEnabled = true,
                        Salt = saltedHash.Salt,
                        HashedPassword = saltedHash.Hash
                    };
                    systemUser.Roles.Add(repository.Query<Role>().FirstOrDefault(x => x.RoleType == RoleType.超级管理员));
                    repository.Create(systemUser);
                }
                return systemUser;
            }
        }
    }
}
