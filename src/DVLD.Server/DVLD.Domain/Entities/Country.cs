using System;
using System.Collections.Generic;

namespace DVLD.Infrastructure;

public partial class Country
{
    public string CountryCode { get; set; } = null!;

    public string CountryName { get; set; } = null!;

    public string CountryFullName { get; set; } = null!;

    public string Iso3 { get; set; } = null!;

    public string CountryNumber { get; set; } = null!;

    public string ContinentCode { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual Continent ContinentCodeNavigation { get; set; } = null!;

    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}
