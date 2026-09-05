using DVLD.Application.Abstractions.Persistence;
using DVLD.Application.DTOs;
using DVLD.Application.Features.People.Get;
using DVLD.Domain.Entities;
using DVLD.Domain.Views;
using DVLD.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Persistence.Repositories;

public class PersonRepository(DvldContext dvldContext) : IPersonRepository
{
    private readonly DvldContext _dvldContext = dvldContext;


    public async Task AddAsync(Person person,CancellationToken cancellationToken)
    {
        _dvldContext.Add(person);
        await _dvldContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid personId, CancellationToken cancellationToken)
    {
        var person = await _dvldContext.People.FindAsync(personId, cancellationToken);
        if (person == null)
            return;
        _dvldContext.People.Remove(person);
        await _dvldContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<PagedList<PersonSummary>> GetPeopleSummaryAsync( GetPeopleQuery query,
        CancellationToken cancellationToken)
    {
        var peopleSummary = _dvldContext.PeopleSummaries.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();

            peopleSummary = peopleSummary.Where(p =>
                p.FullName.Contains(search) ||
                p.NationalId.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(query.NationalId))
        {
            peopleSummary = peopleSummary.Where(p =>
                p.NationalId.Contains(query.NationalId));
        }

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            peopleSummary = peopleSummary.Where(p =>
                p.FullName.Contains(query.Name));
        }

        if (!string.IsNullOrWhiteSpace(query.Gender))
        {
            peopleSummary = peopleSummary.Where(p =>
                p.Gender == query.Gender);
        }

        if (!string.IsNullOrWhiteSpace(query.Phone))
        {
            peopleSummary = peopleSummary.Where(p =>
                p.Phone.Contains(query.Phone));
        }
        
        var totalCount = await peopleSummary.CountAsync(
               cancellationToken);

           // -------------------------
           // Sorting
           // -------------------------

           peopleSummary = query.SortBy?.ToLower() switch
           {
               "name" =>
                   query.IsDescending
                       ? peopleSummary.OrderByDescending(p => p.FullName)
                       : peopleSummary.OrderBy(p => p.FullName),

               "nationalid" =>
                   query.IsDescending
                       ? peopleSummary.OrderByDescending(p => p.NationalId)
                       : peopleSummary.OrderBy(p => p.NationalId),

               _ => peopleSummary.OrderBy(p => p.FullName)
           };

           // -------------------------
           // Pagination
           // -------------------------

           var items = await peopleSummary
               .Skip((query.PageNumber - 1) * query.PageSize)
               .Take(query.PageSize)
               .ToListAsync(cancellationToken);

           return new PagedList<PersonSummary>(
               items,
               totalCount,
               query.PageNumber,
               query.PageSize);
    }

    public async Task<PersonSummary?> GetSummaryByIdAsync(Guid personId,CancellationToken cancellationToken)
    {
        var personSummary = await _dvldContext.PeopleSummaries.FindAsync(personId, cancellationToken);
        return personSummary;
    }

    public async Task<bool> IsNationalIdUnique(string nationalId)
    {
        return !await _dvldContext.People.AnyAsync(p => p.NationalId == nationalId);
    }

    public async Task<bool> IsEmailUnique(string email)
    {
        return !await _dvldContext.People.AnyAsync(p => p.Email == email);
    }
}