using System.Reflection;
using Dal.Postgres.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dal.Postgres;

public static class Extensions
{
    public static IServiceCollection AddDatabase<T>(this IServiceCollection serviceCollection, IConfiguration configuration ,string connectionStringName) where T: DbContext, IUnitOfWork
    {
        var connectionString = configuration.GetConnectionString(connectionStringName);
        
        serviceCollection.AddDbContext<T>(x =>
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionStringName));
            }

            x.UseNpgsql(connectionString);
        });

        serviceCollection.AddScoped<IUnitOfWork, T>();
        serviceCollection.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(UnitOfWorkPipeline<,>));
        });

        return serviceCollection;
    }

    public static WebApplication UseMigration<T>(this WebApplication application) where T: DbContext
    {
        using var scope = application.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<T>();
        
        db.Database.Migrate();
        
        return application;
    }
}