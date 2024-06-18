using Shared.Exceptions;

namespace Vehicle.Core.Exceptions;

public class InvalidYearException : BaseException
{
    public InvalidYearException(string message) : base(message)
    {
    }

    public override string Message => "invalid_vehicle_year";
}