using ApiShared;
using Dal.Postgres;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vehicle.Infrastructure;
using Vehicle.Infrastructure.Database;

namespace Vehicle.Shared;

file class VehicleModule : IModule
{
    public string ModuleName => "Vehicle";
    public IServiceCollection InstallModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfra(configuration);
        
        return services;
    }

    public IEndpointRouteBuilder AddEndPoints(IEndpointRouteBuilder endpointRoute)
    {
        

        return endpointRoute;
    }

    public IApplicationBuilder InstallModule(IApplicationBuilder app)
    {
        app.InstallInfra();
        return app;
    }
}