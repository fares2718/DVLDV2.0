using System;
using System.Collections.Generic;

namespace DVLD.Infrastructure;

public partial class Continent
{
    public string ContinentCode { get; set; } = null!;

    public string? ContinentName { get; set; }

    public virtual ICollection<Country> Countries { get; set; } = new List<Country>();
}
