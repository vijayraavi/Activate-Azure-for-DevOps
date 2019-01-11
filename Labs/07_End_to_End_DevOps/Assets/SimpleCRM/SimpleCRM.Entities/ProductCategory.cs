namespace SimpleCRM.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SalesLT.ProductCategory")]
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
            ProductCategory1 = new HashSet<ProductCategory>();
        }

        public int ProductCategoryID { get; set; }

        public int? ParentProductCategoryID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Category")]
        public string Name { get; set; }

        public Guid rowguid { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<ProductCategory> ProductCategory1 { get; set; }

        public virtual ProductCategory ProductCategory2 { get; set; }
    }
}
