namespace DVLD.Domain.Entities;

public class Continent
{
    public string ContinentCode { get; private set; } = null!;

    public string? ContinentName { get; private set; }
    
    //For EF Core
    private Continent(){}
}
