using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Api.Data.Repository
{
    public class RepositoryBase<T> : IRepository<T> where T : BaseEntity
    {
        private readonly Api.Data.Context.Context _context;
        private DbSet<T> _db;
        public RepositoryBase(Api.Data.Context.Context context)
        {
            _context = context;
            _db = _context.Set<T>();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try{
               var data = await _db.SingleOrDefaultAsync(p => p.Id == id);

               if(data == null)
                  return false;

                _db.Remove(data);
                await _context.SaveChangesAsync();   
            }
            catch(Exception ex){
                throw ex;
            }

            return true;
        }

        public async Task<IEnumerable<T>> GetallAsync()
        {
             try{
               return await _db.ToListAsync();             
            }
            catch(Exception ex){
                throw ex;
            }
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            try{
               return await _db.SingleOrDefaultAsync(p => p.Id == id);             
            }
            catch(Exception ex){
                throw ex;
            }
        }

        public async Task<T> InsertAsync(T item)
        {
            try
            {
                if (item.Id == Guid.Empty)
                    item.Id = Guid.NewGuid();

                item.CreatedAt = DateTime.UtcNow;
                _db.Add(item);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return item;
        }

        public async Task<T> UpdateAsync(T item)
        {
            try
            {
                var data = await _db.SingleOrDefaultAsync(p => p.Id == item.Id);

                if (data == null)
                    return null;

                item.UpdatedAt = DateTime.UtcNow;
                item.CreatedAt = data.CreatedAt;

                _context.Entry(data).CurrentValues.SetValues(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return item;
        }
    }
}
