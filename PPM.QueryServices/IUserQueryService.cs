using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Data;
using PPM.Entities;

namespace PPM.Query
{
    public class UserQuery
    {
        public string Username { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public int? ProjectId { get; set; }
        public int? RoleId { get; set; }
        public RoleType? RoleType { get; set; }
        public bool? IsEnabled { get; set; }
    }
    public interface IUserQueryService
    {
        PagedData<User> Query(int page, int pageSize, UserQuery query);
        User Get(int userId);
        User GetUserByUserNameAndPassword(string userName, string password);
        IEnumerable<User> QueryAllValid();
        string GetSalt(string userName);
        User Authenticate(string userName, string password);
        User GetUser(string userName);
    }
}
