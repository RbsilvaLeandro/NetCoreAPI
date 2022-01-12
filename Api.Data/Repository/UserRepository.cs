using System.Threading.Tasks;
using Api.Data.Context;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private DbSet<User> _db;

        public UserRepository(ApiContext context) : base(context)
        {
            _db = context.Set<User>();
        }

        public async Task<User> FindByLogin(string email)
        {
            return await _db.FirstOrDefaultAsync(u => u.Email == email);
        }

    }
}
