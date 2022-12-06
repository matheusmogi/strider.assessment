using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Strider.Posterr.Domain.Repositories;
using Strider.Posterr.RelationalData.Repositories;

namespace Strider.Posterr.RelationalData.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceRegisterExtensions
{
    public static void Migrate(this IServiceProvider services, bool isTesting)
    {
        if (!isTesting)
            services.GetService<PosterrSqlDbContext>()?.Database.Migrate();
    }

    public static void RegisterDataServices(this IServiceCollection serviceCollection, string connectionString, bool isTesting)
    {
        if (isTesting)
        {
            serviceCollection.AddDbContext<PosterrSqlDbContext>(options => options.UseInMemoryDatabase(databaseName: "InMemoryPosterr"));
        }
        else
        {
            serviceCollection.AddDbContext<PosterrSqlDbContext>(options => options.UseSqlServer(connectionString));
        }

        serviceCollection.AddTransient<IPostRepository, PostRepository>();
        serviceCollection.AddTransient<IUserRepository, UserRepository>();
        serviceCollection.AddTransient<IUnitOfWork, EntityFrameworkUnitOfWork>();
        serviceCollection.AddDatabaseDeveloperPageExceptionFilter();
    }
}