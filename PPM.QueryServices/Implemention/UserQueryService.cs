using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Data;
using Foundation.Utils;
using PPM.Entities;
using PPM.Shared;

namespace PPM.Query.Implemention
{
    public class UserQueryService : IUserQueryService
    {
        private readonly IFetcher _fetcher;
        public UserQueryService(IFetcher fetcher)
        {
            _fetcher = fetcher;
        }

        public PagedData<User> Query(int page, int pageSize, UserQuery query)
        {
            var users = _fetcher.Query<User>();

            if (!string.IsNullOrEmpty(query.Username))
            {
                users = users.Where(x => x.Username.Contains(query.Username));
            }
            if (!string.IsNullOrEmpty(query.RealName))
            {
                users = users.Where(x => x.RealName.Contains(query.RealName.Trim()));
            }
            if (!string.IsNullOrEmpty(query.Email))
            {
                users = users.Where(x => x.Email.Contains(query.Email));
            }
            if (query.RoleType != null)
            {
                users = users.Where(x => x.RoleType == (RoleType)query.RoleType.Value);
            }
            //if (query.RoleId.HasValue)
            //{
            //    users = users.Where(x => x.Roles.Select(s => s.Id).Contains(query.RoleId.Value));
            //}
            if (query.IsEnabled.HasValue)
            {
                users = users.Where(x => x.IsEnabled == query.IsEnabled);
            }
            return _fetcher.QueryPaged<User>(users.OrderBy(x => x.RoleType).ThenBy(x => x.CreatedOn), page, pageSize);
        }

        public User GetUserByUserNameAndPassword(string userName, string password)
        {
            var sql = "select * from [User] where Username=@Username and HashedPassword =@HashedPassword";
            return _fetcher.Query<User>(sql, new { Username = userName, HashedPassword = password }).FirstOrDefault();
        }

        public IEnumerable<User> QueryAllValid()
        {
            return _fetcher.Query<User>().Where(x => x.IsEnabled);
        }

        public string GetSalt(string userName)
        {
            var sql = "select * from [User] where Username=@Username";
            return _fetcher.Query<User>(sql, new { Username = userName }).FirstOrDefault().Salt;
        }

        public User Authenticate(string userName, string password)
        {
            var user =
                _fetcher.Query<User>("SELECT * FROM [User] WHERE Username=@Username",
                        new { userName })
                    .FirstOrDefault();

            if (user != null && user.IsEnabled && SaltedHash.Create(user.Salt, user.HashedPassword).Verify(password))
            {
                return user;
            }
            return null;
        }


        public User GetUser(string userName)
        {
            return _fetcher.Query<User>().FirstOrDefault(x => x.Username == userName);
        }

        public User Get(int userId)
        {
            return _fetcher.Get<User>(userId);
        }
    }
}
