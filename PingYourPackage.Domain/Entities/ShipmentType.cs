//------------------------------------------------------------------------------
// <auto-generated>
//     Der Code wurde von einer Vorlage generiert.
//
//     Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten der Anwendung.
//     Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PingYourPackage.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ShipmentType
    {
        public ShipmentType()
        {
            this.Shipments = new HashSet<Shipment>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public System.DateTime CreatedOn { get; set; }
    
        public virtual ICollection<Shipment> Shipments { get; set; }
    }
}
