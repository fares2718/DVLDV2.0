using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Entities;
using DVLD.Domain.Views;
using DVLD.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Persistence.Repositories;

public class PersonRepository(DvldContext dvldContext) : IPersonRepository
{
    private readonly DvldContext _dvldContext = dvldContext;


    public async Task AddAsync(Person person)
    {
        _dvldContext.Add(person);
        await _dvldContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid personId)
    {
        var person = await _dvldContext.People.FindAsync(personId);
        if (person == null)
            return;
        _dvldContext.People.Remove(person);
        await _dvldContext.SaveChangesAsync();
    }
    
    public async Task<IReadOnlyList<PersonSummary>> GetAllSummaryAsync()
    {
        var peopleSummery = await _dvldContext.PeopleSummaries.ToListAsync();
        return peopleSummery;
    }

    public async Task<PersonSummary?> GetSummaryByIdAsync(Guid personId)
    {
        var personSummary = await _dvldContext.PeopleSummaries.FindAsync(personId);
        return personSummary;
    }
}