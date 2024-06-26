using ApiShared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Vehicle.Shared;

file class VehicleModule : IModule
{
    public string ModuleName => "Vehicle";
    public IServiceCollection InstallModule(IServiceCollection services)
    {
        return services;
    }

    public IEndpointRouteBuilder AddEndPoints(IEndpointRouteBuilder endpointRoute)
    {
        

        return endpointRoute;
    }

    public IApplicationBuilder InstallModule(IApplicationBuilder app)
    {
        return app;
    }
}