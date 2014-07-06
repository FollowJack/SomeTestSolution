using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using PingYourPackage.Domain.Entities;
using PingYourPackage.Domain.Entities.Core;
using PingYourPackage.Domain.Entities.Extensions;
using WebMatrix.WebData;

namespace PingYourPackage.Domain.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IEntityRepository<User> _userRepository;

        public MembershipService(IEntityRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public ValidUserContext ValidateUser(string username, string password)
        {
            var userContext = new ValidUserContext();
            var user = _userRepository.GetSingleByUsername(username);
            var isValid = Membership.ValidateUser(username, password);

            if (user != null && isValid)
            {
                var identity = new GenericIdentity(user.Name);
                var rolesForUser = Roles.GetRolesForUser(user.Name);

                userContext.Principal = new GenericPrincipal(identity, rolesForUser);
            }

            return userContext;
        }

        public OperationResult<User> CreateUser(string username, string email, string password)
        {
            return CreateUser(username, password, email, roles: null);
        }

        public OperationResult<User> CreateUser(string username, string email, string password, string role)
        {
            return CreateUser(username, password, email, roles: new[] { role });
        }

        public OperationResult<User> CreateUser(string username, string email, string password, string[] roles)
        {

            var existingUser = _userRepository.GetAll().Any(
                x => x.Name == username);

            if (existingUser)
            {
                return new OperationResult<User>(false);
            }

            WebSecurity.CreateUserAndAccount(username, password, new { Email = email, IsLocked = false, CreatedOn = DateTime.Now });

            var user = _userRepository.GetSingleByUsername(username);

            if (roles != null && roles.Length > 0)
            {

                foreach (var roleName in roles)
                {
                    addUserToRole(user, roleName);
                }
            }

            return new OperationResult<User>(true) { Entity = user };
        }

        private void addUserToRole(User user, string roleName)
        {
            if (!Roles.RoleExists(roleName))
                Roles.CreateRole(roleName);

            if (!Roles.IsUserInRole(user.Name, roleName))
                Roles.AddUserToRole(user.Name, roleName);
        }

        public User UpdateUser(User user, string username, string email)
        {
            user.Name = username;
            user.Email = email;
            user.LastUpdatedOn = DateTime.Now;

            _userRepository.Edit(user);
            _userRepository.Save();

            return user;
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return WebSecurity.ChangePassword(username, oldPassword, newPassword);
        }

        public bool AddToRole(string username, string role)
        {
            var user = _userRepository.GetSingleByUsername(username);
            if (user != null)
            {
                addUserToRole(user, role);
                return true;
            }

            return false;
        }

        public bool AddToRole(int userId, string role)
        {

            var user = _userRepository.GetSingle(userId);
            if (user != null)
            {
                addUserToRole(user, role);
                return true;
            }

            return false;
        }

        public bool RemoveFromRole(string username, string role)
        {
            var user = _userRepository.GetSingleByUsername(username);

            if (user == null && string.IsNullOrWhiteSpace(role))
                return false;

            if (Roles.IsUserInRole(username, role))
            {
                Roles.RemoveUserFromRole(username, role);
                return true;
            }
            return false;


        }

        public IEnumerable<string> GetRoles()
        {
            return Roles.GetAllRoles().AsEnumerable();
        }

        public PaginatedList<User> GetUsers(int pageIndex, int pageSize)
        {
            var users = _userRepository.Paginate(pageIndex, pageSize, x => x.ID);

            return new PaginatedList<User>(
                users.PageIndex,
                users.PageSize,
                users.TotalCount,
                users.AsQueryable());
        }

        public User GetUser(int id)
        {
            return _userRepository.GetSingle(id);
        }

        public User GetUser(string name)
        {
            return _userRepository.GetSingleByUsername(name);

        }
    }
}
