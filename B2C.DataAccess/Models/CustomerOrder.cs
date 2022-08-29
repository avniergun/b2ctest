using System;
using System.Collections.Generic;

namespace B2C.DataAccess.Models
{
    public partial class CustomerOrder: BaseEntity
    {
        public CustomerOrder()
        {
            CustomerOrderDetail = new HashSet<CustomerOrderDetail>();
        }
        public int CustomerId { get; set; }
        public string? OrderCode { get; set; }
        public string? DeliveryAddress { get; set; }
        public decimal OrderAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<CustomerOrderDetail> CustomerOrderDetail { get; set; }
    }
}
