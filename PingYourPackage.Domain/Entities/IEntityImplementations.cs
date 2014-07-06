using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PingYourPackage.Domain.Entities.Core;

namespace PingYourPackage.Domain.Entities
{
    public partial class User : IEntity { }
    public partial class Affiliate : IEntity { }
    public partial class Shipment : IEntity { }
    public partial class ShipmentType : IEntity { }
    public partial class ShipmentState : IEntity { }
}
