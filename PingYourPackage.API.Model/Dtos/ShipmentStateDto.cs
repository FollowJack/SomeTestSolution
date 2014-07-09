using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Model.Dtos
{
    public class ShipmentStateDto : IDto
    {
        public int ID { get; set; }
        public int ShipmentID { get; set; }
        public string ShipmentStatus { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
