using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dal.Postgres;

public static class Extensions
{
    public static WebApplicationBuilder AddDatabase<T>(this WebApplicationBuilder builder, string connectionStringName) where T: DbContext, IUnitOfWork
    {
        var connectionString = builder.Configuration.GetConnectionString(connectionStringName);
        
        builder.Services.AddDbContext<T>(x =>
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionStringName));
            }

            x.UseNpgsql(connectionString);
        });

        builder.Services.AddScoped<IUnitOfWork, T>();
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(UnitOfWorkPipeline<,>));
        });

        return builder;
    }

    public static WebApplication UseMigration<T>(this WebApplication application) where T: DbContext
    {
        using var scope = application.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<T>();
        
        db.Database.Migrate();
        
        return application;
    }
}