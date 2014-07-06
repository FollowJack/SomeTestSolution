using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PingYourPackage.Domain.Entities;
using PingYourPackage.Domain.Entities.Core;

namespace PingYourPackage.Domain.Services
{
    public interface IMembershipService
    {
        ValidUserContext ValidateUser(string username, string password);

        OperationResult<User> CreateUser(string username, string email, string password);

        OperationResult<User> CreateUser(string username, string email, string password, string role);

        OperationResult<User> CreateUser(string username, string email, string password, string[] roles);

        User UpdateUser(User user, string username, string email);

        bool ChangePassword(string username, string oldPassword, string newPassword);

        bool AddToRole(int userId, string role);
        bool AddToRole(string username, string role);
        bool RemoveFromRole(string username, string role);

        IEnumerable<string> GetRoles();

        PaginatedList<User> GetUsers(int pageIndex, int pageSize);
        User GetUser(int id);
        User GetUser(string name);
    }
}
