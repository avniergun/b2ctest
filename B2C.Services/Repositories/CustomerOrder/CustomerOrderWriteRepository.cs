using B2C.Core.Repositories;
using B2C.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Business.Repositories
{
    public class CustomerOrderWriteRepository : WriteRepository<CustomerOrder>, ICustomerOrderWriteRepository
    {
        public CustomerOrderWriteRepository(B2CContext context) : base(context)
        {

        }
    }
}
