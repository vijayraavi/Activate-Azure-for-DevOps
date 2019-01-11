namespace SimpleCRM.Entities
{
    using SimpleCRM.Entities.Attributes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SalesLT.Address")]
    public partial class Address
    {
        public Address()
        {
            CustomerAddresses = new HashSet<CustomerAddress>();
            SalesOrderHeadersBilledTo = new HashSet<SalesOrderHeader>();
            SalesOrderHeadersShippedTo = new HashSet<SalesOrderHeader>();
        }

        public int AddressID { get; set; }

        [Required]
        [StringLength(60)]
        [ExcludeChar("/.,!@#$%", ErrorMessage = "Address line contains invalid character.")]
        public string AddressLine1 { get; set; }

        [StringLength(60)]
        public string AddressLine2 { get; set; }

        [Required]
        [StringLength(30)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string StateProvince { get; set; }

        [Required]
        [StringLength(50)]
        public string CountryRegion { get; set; }

        [Required]
        [StringLength(15)]
        public string PostalCode { get; set; }

        public Guid rowguid { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }

        public virtual ICollection<SalesOrderHeader> SalesOrderHeadersBilledTo { get; set; }

        public virtual ICollection<SalesOrderHeader> SalesOrderHeadersShippedTo { get; set; }
    }
}
