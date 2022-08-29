using B2C.Core.Repositories;
using B2C.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Business.Repositories
{
    public class CustomerOrderDetailReadRepository : ReadRepository<CustomerOrderDetail>, ICustomerOrderDetailReadRepository
    {
        public CustomerOrderDetailReadRepository(B2CContext context) : base(context)
        {

        }
    }
}
