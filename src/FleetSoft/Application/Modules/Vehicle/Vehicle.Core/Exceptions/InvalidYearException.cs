using Shared.Exceptions;

namespace Vehicle.Core.Exceptions;

public class InvalidYearException(string message) : BaseException(message)
{
    public override string Message => "invalid_vehicle_year";
}