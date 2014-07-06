using System.Web.Security;
using WebMatrix.WebData;

namespace PingYourPackage.Domain.Entities.Configuration
{
    public static class DbInitializer
    {
        public static void Initialize()
        {
            InitializeDbConnection();
            AddRoles();
        }

        private static void AddRoles()
        {
            if (!Roles.RoleExists("Admin"))
                Roles.CreateRole("Admin");
            if (!Roles.RoleExists("Employee"))
                Roles.CreateRole("Employee");
            if (!Roles.RoleExists("Affiliate"))
                Roles.CreateRole("Affiliate");
        }

        private static void InitializeDbConnection()
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Users", "ID", "Name", true);
        }
    }
}
