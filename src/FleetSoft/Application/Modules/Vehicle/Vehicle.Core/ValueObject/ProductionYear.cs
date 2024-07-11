using Vehicle.Core.Exceptions;

namespace Vehicle.Core.ValueObject;

public record ProductionYear
{
    public int Year { get; private set; }
    
    public ProductionYear(int year)
    {
        if (year is < 1900 or > 2030)
        {
            throw new InvalidYearException("Invalid production year");
        }

        Year = year;
    }
    
}