using Shared.Exceptions;

namespace Vehicle.Infrastructure.Exceptions;

public class VehicleNotFoundException : BaseException
{
    public VehicleNotFoundException(string message) : base(message)
    {
    }

    public override string Message => "vehicle_not_found";
}