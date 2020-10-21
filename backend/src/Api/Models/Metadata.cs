using System;
using HotChocolate;
using HotChocolate.Types;

namespace src.Api.Models
{
    public class Metadata
    {
        [GraphQLNonNullType]
        public int SensorID { get; set; }
        [GraphQLNonNullType]
        public string Number { get; set; }
        public string? Coordinate { get; set; }
        public int? Altitude { get; set; }
        public string? LocationDescription { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string ServicePartner { get; set; }
        public string Department { get; set; }
        public string OwnerID { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyDate { get; set; }
        public string Identificator { get; set; }
        public string ModelNumber { get; set; }
        public string SerialNumber { get; set; }
        public string Tag1 { get; set; }
        public string Tag2 { get; set; }
        public string Tag3 { get; set; }
        public DateTime? NextService { get; set; }
        public DateTime? PlannedDisposal { get; set; }
        public DateTime? ActualDisposal { get; set; }
        public bool? Lending { get; set; }
        public float? LendingPrice { get; set; }
        public bool? Timeless { get; set; }
        public bool? CheckOnInspectionRound { get; set; }
        public bool? Tollerance { get; set; }
        public string Picture { get; set; }
        public float? CableLength { get; set; }
        public string? Voltage { get; set; }
        public string Signal { get; set; }
        public string MeasureArea { get; set; }
        public string Website { get; set; }
        public string InspectionRound { get; set; }

    }
}