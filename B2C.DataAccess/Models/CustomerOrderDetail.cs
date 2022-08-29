using System;
using System.Collections.Generic;

namespace B2C.DataAccess.Models
{
    public partial class CustomerOrderDetail: BaseEntity
    {
        public int CustomerOrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }

        public virtual CustomerOrder CustomerOrder { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
