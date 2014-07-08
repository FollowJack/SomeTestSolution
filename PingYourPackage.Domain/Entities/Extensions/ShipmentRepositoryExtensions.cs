using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PingYourPackage.Domain.Entities.Core;

namespace PingYourPackage.Domain.Entities.Extensions
{
    public static class ShipmentRepositoryExtensions
    {
        public static IQueryable<Shipment> GetShipmentsByAffiliateKey(this IEntityRepository<Shipment> shipmentRepository, int affiliateId)
        {
            return shipmentRepository.AllIncluding(s => s.ShipmentType, s => s.ShipmentStates).Where(x => x.AffiliateID == affiliateId);
        }

        public static IQueryable<Shipment> GetNotDeliveredShipments(this IEntityRepository<Shipment> shipmentRepository)
        {
            var shipments = shipmentRepository
                .AllIncluding(s => s.ShipmentType, t => t.ShipmentStates)
                .Where(shipment => shipment.ShipmentStates.Any(x => x.ShipmentStatus != (int)ShipmentStatus.Delivered));

            return shipments;
        }

        public static IQueryable<Shipment> GetNotDeliveredShipments(this IEntityRepository<Shipment> shipmentRepository, int affiliateID)
        {
            return shipmentRepository
                .GetNotDeliveredShipments()
                .Where(shipment => shipment.AffiliateID == affiliateID);
        }
    }
}
