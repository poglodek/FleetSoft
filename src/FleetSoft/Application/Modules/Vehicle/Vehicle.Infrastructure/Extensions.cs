using Dal.Postgres;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Vehicle.Infrastructure.Database;

namespace Vehicle.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfra(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddDatabase<VehicleDbContext>(configuration, "VehicleDatabase");
        
        return serviceCollection;
    }

    public static IApplicationBuilder InstallInfra(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<VehicleDbContext>();
        
        dbContext.Database.Migrate();


        return app;
    }
}