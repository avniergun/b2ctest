using B2C.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Core.Repositories
{
   public interface IRepository<T> where T : BaseEntity
    {
        DbSet<T> Table { get; }
    }
}
