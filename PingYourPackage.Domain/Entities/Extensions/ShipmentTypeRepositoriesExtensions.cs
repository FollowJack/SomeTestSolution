using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PingYourPackage.Domain.Entities.Core;

namespace PingYourPackage.Domain.Entities.Extensions
{
    public static class ShipmentTypeRepositoriesExtensions
    {
        public static ShipmentType GetSingleByName(
           this IEntityRepository<ShipmentType> shipmentTypeRepository,
           string name)
        {

            return shipmentTypeRepository
                .FindBy(x => x.Name == name).FirstOrDefault();
        }
    }
}
