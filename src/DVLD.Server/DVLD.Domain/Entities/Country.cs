namespace DVLD.Domain.Entities;

public class Country
{
    public string CountryCode { get; private set; } = null!;

    public string CountryName { get; private set; } = null!;

    public string CountryFullName { get; private set; } = null!;

    public string Iso3 { get; private set; } = null!;

    public string CountryNumber { get;private set; } = null!;

    public string ContinentCode { get; private set; } = null!;
    
    //For EF Core
    private Country(){}
}
