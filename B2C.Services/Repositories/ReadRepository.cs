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
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        readonly private B2CContext _context;

        public ReadRepository(B2CContext context)
        {
            _context = context;
        }
        public DbSet<T> Table => _context.Set<T>();

        public IQueryable<T> GetAll(bool isTracking = true)
        {
            var query = Table.AsQueryable();
            query = isTracking ? query : query.AsNoTracking();
            return query;
        }
        public IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate, bool isTracking = true)
        {
            var query = Table.Where(predicate);
            query = isTracking ? query : query.AsNoTracking();
            return query;

        }
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, bool isTracking = true)
        {
            var query = Table.AsQueryable();
            query = isTracking ? query : query.AsNoTracking();
            return await query.FirstOrDefaultAsync(predicate);
        }
        public async Task<T> GetByIdAsync(int id, bool isTracking = true)
        {
            var query = Table.AsQueryable();
            query = isTracking ? query : query.AsNoTracking();
            return await query.FirstOrDefaultAsync(p=>p.Id==id);
        }
    }
}
