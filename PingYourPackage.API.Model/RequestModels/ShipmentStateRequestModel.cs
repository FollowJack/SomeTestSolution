using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PingYourPackage.API.Model.Validation;

namespace PingYourPackage.API.Model.RequestModels
{
    public class ShipmentStateRequestModel
    {

        [Required]
        [Options("Ordered", "Scheduled", "InTransit", "Delivered")]
        public string ShipmentStatus { get; set; }
    }
}
