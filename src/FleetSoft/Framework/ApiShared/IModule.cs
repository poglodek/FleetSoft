using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace ApiShared;

public interface IModule
{
    public string ModuleName { get; }
    IServiceCollection InstallModule(IServiceCollection services);
    public IEndpointRouteBuilder AddEndPoints(IEndpointRouteBuilder endpointRoute);
    public IApplicationBuilder InstallModule(IApplicationBuilder app);
    
}