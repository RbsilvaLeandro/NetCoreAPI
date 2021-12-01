using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            var connectionStrings = "Server=localhost;Port=3306;Database=Api;Uid=root;Pwd=nigt@c#52489700";
            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.UseMySql(connectionStrings, ServerVersion.AutoDetect(connectionStrings), null);
            return new Context(optionsBuilder.Options);
        }
    }
}
