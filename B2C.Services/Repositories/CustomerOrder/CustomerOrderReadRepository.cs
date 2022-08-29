using B2C.Core.Repositories;
using B2C.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Business.Repositories
{
    public class CustomerOrderReadRepository : ReadRepository<CustomerOrder>, ICustomerOrderReadRepository
    {
        public CustomerOrderReadRepository(B2CContext context) : base(context)
        {


        }
    }
}
