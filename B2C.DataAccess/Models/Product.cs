using System;
using System.Collections.Generic;

namespace B2C.DataAccess.Models
{
    public partial class Product: BaseEntity
    {
        public Product()
        {
            CustomerOrderDetail = new HashSet<CustomerOrderDetail>();
        }
        
        public string? Barcode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<CustomerOrderDetail> CustomerOrderDetail { get; set; }
    }
}
