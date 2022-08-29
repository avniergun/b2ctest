using B2C.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Core.Repositories
{
    public interface ICustomerOrderReadRepository : IReadRepository<CustomerOrder>
    {
    }
}
