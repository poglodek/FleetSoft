using Shared.Exceptions;

namespace Vehicle.Core.Exceptions;

public class InvalidVerificationException(string message) : BaseException(message)
{
    public override string Message => "invalid_verification";
}