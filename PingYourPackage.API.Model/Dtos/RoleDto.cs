using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Model.Dtos
{
    public class RoleDto : IDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
