using B2C.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Core.Repositories
{
    public interface IReadRepository<T> :IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll(bool isTracking=true);
        IQueryable<T> GetWhere(Expression<Func<T,bool>> predicate, bool isTracking = true);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, bool isTracking = true);
        Task<T> GetByIdAsync(int id, bool isTracking = true);

    }
}
