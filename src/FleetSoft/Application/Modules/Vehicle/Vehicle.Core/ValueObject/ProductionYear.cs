namespace Vehicle.Core.ValueObject;

public record ProductionYear
{
    public int Year { get; private set; }
    
    public ProductionYear(int year)
    {
        if (year < 1900 || year > DateTime.Now.Year)
        {
            throw new ArgumentException("Invalid production year");
        }

        Year = year;
    }
    
}