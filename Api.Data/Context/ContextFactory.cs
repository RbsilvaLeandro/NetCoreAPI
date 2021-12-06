using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            var connectionStrings = "Server=localhost;Port=3306;Database=Api;Uid=root;Pwd=Nigt@c#52489700";
            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.UseMySql(connectionStrings, ServerVersion.AutoDetect(connectionStrings));
            return new Context(optionsBuilder.Options);
        }
    }
}
