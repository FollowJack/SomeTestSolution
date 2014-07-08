using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PingYourPackage.Domain.Entities;
using PingYourPackage.Domain.Entities.Core;

namespace PingYourPackage.Domain.Services
{
    public interface IShipmentService
    {
        PaginatedList<ShipmentType> GetShipmentTypes(int pageIndex, int pageSize);
        ShipmentType GetShipmentType(int id);
        OperationResult<ShipmentType> AddShipmentType(ShipmentType shipmentType);
        ShipmentType UpdateShipmentType(ShipmentType shipmentType);

        PaginatedList<Affiliate> GetAffiliates(int pageIndex, int pageSize);
        Affiliate GetAffiliate(int id);
        OperationResult<Affiliate> AddAffiliate(int id, Affiliate affiliate);
        Affiliate UpdateAffiliate(Affiliate affiliate);

        PaginatedList<Shipment> GetShipments(int pageIndex, int pageSize);
        PaginatedList<Shipment> GetShipments(int pageIndex, int pageSize, int affiliateId);
        Shipment GetShipment(int id);
        OperationResult<Shipment> AddShipment(Shipment shipment);
        Shipment UpdateShipment(Shipment shipment);
        OperationResult RemoveShipment(Shipment shipment);

        IEnumerable<ShipmentState> GetShipmentStates(int shipmentId);
        OperationResult<ShipmentState> AddShipmentState(int shipmentId, ShipmentStatus status);

        bool IsAffiliateRelatedToUser(int affiliateId, string username);

    }
}
