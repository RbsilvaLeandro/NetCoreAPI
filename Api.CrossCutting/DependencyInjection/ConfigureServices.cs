using Api.Data.Context;
using Api.Data.Repository;
using Api.Domain.Interfaces.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Api.Domain.Interfaces.Repositories;
using Api.Services.EntityService;

namespace Api.CrossCutting.DependencyInjection
{
    public class ConfigureServices
    {
        public static void ConfigureDependencyService(IServiceCollection serviceCollection)
        {
            //Services
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<ILoginService, LoginService>();

            serviceCollection.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            serviceCollection.AddScoped<IUserRepository , UserRepository>();

            //Repositories
            string connectionStrings = "Server=localhost;Port=3306;Database=Api;Uid=root;Pwd=Nigt@c#52489700";
            serviceCollection.AddDbContext<ApiContext>(options => options.UseMySql(connectionStrings, ServerVersion.AutoDetect(connectionStrings)));

        }
    }
}
