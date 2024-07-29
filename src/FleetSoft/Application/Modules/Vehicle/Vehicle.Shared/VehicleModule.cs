using ApiShared;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vehicle.Application.Command.AddMileage;
using Vehicle.Application.Command.CreateVehicle;
using Vehicle.Application.Command.SetArchive;
using Vehicle.Infrastructure;
using Vehicle.Infrastructure.Query.GetAll;
using Vehicle.Infrastructure.Query.GetById;

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
        endpointRoute.MapGet("/{id}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetByIdRequest(id));

            return Results.Ok(result);
        });
        
        endpointRoute.MapGet("/", async ( IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllRequest());

            return Results.Ok(result);
        });
        
        endpointRoute.MapPost("/", async (CreateVehicleRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(request);

            return Results.Ok(result);
        });
        
        endpointRoute.MapPut("/{id}/Archive", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new SetArchiveRequest(id));

            return Results.Ok(result);
        });
        
        endpointRoute.MapPut("/{id}/Mileage", async (Guid id,[FromQuery]double value, IMediator mediator) =>
        {
            var result = await mediator.Send(new AddMileageRequest(id,value));

            return Results.Ok(result);
        });

        return endpointRoute;
    }

    public IApplicationBuilder InstallModule(IApplicationBuilder app)
    {
        app.InstallInfra();
        return app;
    }
}