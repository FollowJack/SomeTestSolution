using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using PingYourPackage.Domain.Entities;
using PingYourPackage.Domain.Entities.Core;
using PingYourPackage.Domain.Entities.Extensions;

namespace PingYourPackage.Domain.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly IEntityRepository<ShipmentType> _shipmentTypeRepository;
        private readonly IEntityRepository<Shipment> _shipmentRepository;
        private readonly IEntityRepository<ShipmentState> _shipmentStateRepository;
        private readonly IEntityRepository<Affiliate> _affiliateRepository;
        private readonly IMembershipService _membershipService;

        public ShipmentService(
            IEntityRepository<ShipmentType> shipmentTypeRepository,
            IEntityRepository<Shipment> shipmentRepository,
            IEntityRepository<ShipmentState> shipmentStateRepository,
            IEntityRepository<Affiliate> affiliateRepository,
            IMembershipService membershipService)
        {

            _shipmentTypeRepository = shipmentTypeRepository;
            _shipmentRepository = shipmentRepository;
            _shipmentStateRepository = shipmentStateRepository;
            _affiliateRepository = affiliateRepository;
            _membershipService = membershipService;
        }

        #region ShipmentType
        public PaginatedList<ShipmentType> GetShipmentTypes(int pageIndex, int pageSize)
        {

            var shipmentTypes = _shipmentTypeRepository
                .Paginate(pageIndex, pageSize, x => x.CreatedOn);

            return shipmentTypes;
        }

        public ShipmentType GetShipmentType(int id)
        {

            var shipmentType = _shipmentTypeRepository.GetSingle(id);
            return shipmentType;
        }

        public OperationResult<ShipmentType> AddShipmentType(ShipmentType shipmentType)
        {

            // If there is already one which has the same name,
            // return unseccessful result back
            if (_shipmentTypeRepository.GetSingleByName(shipmentType.Name) != null)
            {

                return new OperationResult<ShipmentType>(false);
            }

            shipmentType.CreatedOn = DateTime.Now;

            _shipmentTypeRepository.Add(shipmentType);
            _shipmentTypeRepository.Save();

            return new OperationResult<ShipmentType>(true)
            {
                Entity = shipmentType
            };
        }

        public ShipmentType UpdateShipmentType(ShipmentType shipmentType)
        {

            _shipmentTypeRepository.Edit(shipmentType);
            _shipmentTypeRepository.Save();

            return shipmentType;
        }

        #endregion

        #region Affiliate
        public PaginatedList<Affiliate> GetAffiliates(int pageIndex, int pageSize)
        {

            var affiliates = _affiliateRepository
                .AllIncluding(x => x.User).OrderBy(x => x.CreatedOn)
                .ToPaginatedList(pageIndex, pageSize);

            return affiliates;
        }

        public Affiliate GetAffiliate(int id)
        {

            var affiliate = _affiliateRepository
                .AllIncluding(a => a.User)
                .FirstOrDefault(x => x.ID == id);

            return affiliate;
        }

        public OperationResult<Affiliate> AddAffiliate(int id, Affiliate affiliate)
        {

            var user = _membershipService.GetUser(id);
            if (user == null || !Roles.IsUserInRole(user.Name, "Affiliate") || _affiliateRepository.GetSingle(id) != null)
                return new OperationResult<Affiliate>(false);


            affiliate.ID = id;
            affiliate.CreatedOn = DateTime.Now;

            _affiliateRepository.Add(affiliate);
            _affiliateRepository.Save();

            affiliate.User = user;
            return new OperationResult<Affiliate>(true)
            {
                Entity = affiliate
            };
        }

        public Affiliate UpdateAffiliate(Affiliate affiliate)
        {

            _affiliateRepository.Edit(affiliate);
            _affiliateRepository.Save();

            return affiliate;
        }
        #endregion

        #region Shipment
        public PaginatedList<Shipment> GetShipments(int pageIndex, int pageSize)
        {

            //var shipments = _shipmentRepository.Paginate(pageIndex, pageSize, x => x.CreatedOn);
            var shipments = GetInitialShipments()
                .ToPaginatedList(pageIndex, pageSize);

            return shipments;
        }

        public PaginatedList<Shipment> GetShipments(int pageIndex, int pageSize, int affiliateId)
        {

            var shipments = _shipmentRepository
                .GetShipmentsByAffiliateKey(affiliateId)
                .OrderBy(x => x.CreatedOn)
                .ToPaginatedList(pageIndex, pageSize);

            return shipments;
        }

        public Shipment GetShipment(int id)
        {

            var shipment = GetInitialShipments()
                .FirstOrDefault(x => x.ID == id);

            return shipment;
        }

        public OperationResult<Shipment> AddShipment(Shipment shipment)
        {
            var affiliate = _affiliateRepository.GetSingle(shipment.AffiliateID);
            var shipmentType = _shipmentTypeRepository.GetSingle(shipment.ShipmentTypeID);

            if (affiliate == null || shipmentType == null)
                return new OperationResult<Shipment>(false);

            shipment.CreatedOn = DateTime.Now;

            _shipmentRepository.Add(shipment);
            _shipmentRepository.Save();

            // Add the first state for this shipment
            var shipmentState = InsertFirstShipmentState(shipment.ID);

            // Add the down level references manual so that
            // we don't have to have a trip to database to get them
            shipment.ShipmentType = shipmentType;
            shipment.ShipmentStates = new List<ShipmentState> { shipmentState };

            return new OperationResult<Shipment>(true) { Entity = shipment };
        }

        public Shipment UpdateShipment(Shipment shipment)
        {
            _shipmentRepository.Edit(shipment);
            _shipmentRepository.Save();

            // Get the shipment seperately so that 
            // we would have down level references such as ShipmentStates.
            var updatedShipment = GetShipment(shipment.ID);

            return shipment;
        }

        public OperationResult RemoveShipment(Shipment shipment)
        {
            if (IsShipmentRemovable(shipment))
            {
                _shipmentRepository.DeleteGraph(shipment);
                _shipmentRepository.Save();

                return new OperationResult(true);
            }
            return new OperationResult(false);
        }
        #endregion

        #region ShipmentState
        public IEnumerable<ShipmentState> GetShipmentStates(int shipmentID)
        {
            var shipmentStates = _shipmentStateRepository.GetAllByShipmentID(shipmentID);
            return shipmentStates;
        }

        public OperationResult<ShipmentState> AddShipmentState(int shipmentID, ShipmentStatus status)
        {
            if (!IsShipmentStateInsertable(shipmentID, status))
            {
                return new OperationResult<ShipmentState>(false);
            }

            var shipmentState = InsertShipmentState(shipmentID, status);
            return new OperationResult<ShipmentState>(true)
            {
                Entity = shipmentState
            };
        }
        #endregion

        #region Others
        public bool IsAffiliateRelatedToUser(int affiliateID, string username)
        {
            var affiliate = GetAffiliate(affiliateID);

            return affiliate != null &&
                   affiliate.User.Name.Equals(username);
        }
        #endregion

        #region private Helpers

        private IQueryable<Shipment> GetInitialShipments()
        {
            return _shipmentRepository
                .AllIncluding(x => x.ShipmentType, x => x.ShipmentStates)
                .OrderBy(x => x.CreatedOn);
        }

        private ShipmentState InsertFirstShipmentState(int shipmentID)
        {
            return InsertShipmentState(shipmentID, ShipmentStatus.Ordered);
        }

        private ShipmentState InsertShipmentState(int shipmentID, ShipmentStatus status)
        {
            var shipmentState = new ShipmentState
            {
                ShipmentID = shipmentID,
                ShipmentStatus = (int)status,
                CreatedOn = DateTime.Now
            };

            _shipmentStateRepository.Add(shipmentState);
            _shipmentStateRepository.Save();

            return shipmentState;
        }

        private bool IsShipmentStateInsertable(int shipmentID, ShipmentStatus status)
        {
            var shipmentStates = GetShipmentStates(shipmentID);
            var latestState = shipmentStates
                .OrderByDescending(state => state.ShipmentStatus)
                .First();

            return (int)status > latestState.ShipmentStatus;
        }

        private bool IsShipmentRemovable(Shipment shipment)
        {
            var latestStatus =
                shipment.ShipmentStates
                    .OrderByDescending(shipmentState => shipmentState.ShipmentStatus)
                    .First();

            return latestStatus.ShipmentStatus < (int)ShipmentStatus.InTransit;
        }
        #endregion
    }
}
