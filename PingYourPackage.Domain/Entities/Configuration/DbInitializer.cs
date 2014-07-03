using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using WebMatrix.WebData;

namespace PingYourPackage.Domain.Entities.Migrations
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
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "User", "Key", "Name", true);
        }
    }
}
