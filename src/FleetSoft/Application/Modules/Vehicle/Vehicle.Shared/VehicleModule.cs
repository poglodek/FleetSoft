using ApiShared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Vehicle.Shared;

public class VehicleModule : IModule
{
    public string ModuleName => "Vehicle";
    public IServiceCollection InstallModule(IServiceCollection services)
    {
        return services;
    }

    public IEndpointRouteBuilder AddEndPoints(IEndpointRouteBuilder endpointRoute)
    {
        endpointRoute.MapGet("/", () => $"Vehicle Module {DateTimeOffset.Now}");

        return endpointRoute;
    }

    public IApplicationBuilder InstallModule(IApplicationBuilder app)
    {
        return app;
    }
}