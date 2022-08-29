using System;
using System.Collections.Generic;

namespace B2C.DataAccess.Models
{
    public partial class Customer:BaseEntity
    {
        public Customer()
        {
            CustomerOrder = new HashSet<CustomerOrder>();
        }
 
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Address { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }

        public virtual ICollection<CustomerOrder> CustomerOrder { get; set; }
    }
}
