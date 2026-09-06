using DVLD.Application.Abstractions.Persistence;
using DVLD.Application.DTOs;
using DVLD.Application.Features.People.Get;
using DVLD.Domain.Common;
using DVLD.Domain.Entities;
using DVLD.Domain.Views;
using DVLD.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Persistence.Repositories;

public class PersonRepository(DvldContext dvldContext) : IPersonRepository
{
    private readonly DvldContext _dvldContext = dvldContext;


    public async Task ActivateAsync(Guid personId,CancellationToken cancellationToken)
    {
        var person = await GetPersonById(personId, cancellationToken);
        if (person == null)
            throw new KeyNotFoundException($"Person with ID {personId} was not found.");
        person.Activate();
        await _dvldContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAsync(Person person,CancellationToken cancellationToken)
    {
        if (await IsNationalIdUnique(person.NationalId))
            throw new DomainException("National ID already exists.");
        if(await IsEmailUnique(person.Email))
            throw new DomainException("Email already exists.");
        
        _dvldContext.Add(person);
        await _dvldContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeActivateAsync(Guid personId, CancellationToken cancellationToken)
    {
        var person = await GetPersonById(personId, cancellationToken);
        if (person == null)
            throw new KeyNotFoundException($"Person with ID {personId} was not found.");
        person.Deactivate();
        await _dvldContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<PagedList<PersonSummary>> GetPeopleSummaryAsync( GetPeopleQuery query,
        CancellationToken cancellationToken)
    {
        var peopleSummary = _dvldContext.PeopleSummaries.AsNoTracking();

        if (query.IsActive.HasValue)
            peopleSummary = peopleSummary.Where(p => p.IsActive == query.IsActive);
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
        
        if (!string.IsNullOrWhiteSpace(query.Email))
        {
            peopleSummary = peopleSummary.Where(p =>
                p.Email.Contains(query.Email));
        }
        
        

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
               
               "phone" => query.IsDescending
                   ? peopleSummary.OrderByDescending(p => p.Phone)
                   : peopleSummary.OrderBy(p => p.Phone),
               
               "email" => query.IsDescending
                   ? peopleSummary.OrderByDescending(p => p.Email)
                   : peopleSummary.OrderBy(p => p.Email),

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
               items.Count,
               query.PageNumber,
               query.PageSize);
    }

    private async Task<Person?> GetPersonById(Guid personId, CancellationToken cancellationToken)
    {
        var person = await _dvldContext.People.FindAsync(personId, cancellationToken);
        return person;
    }

    public async Task<PersonSummary?> GetSummaryByIdAsync(Guid personId,CancellationToken cancellationToken)
    {
        var personSummary = await _dvldContext.PeopleSummaries
            .FirstOrDefaultAsync(p=>p.PersonId == personId, cancellationToken);
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

    public async Task UpdatePersonName(Guid personId,string firstName, string secondName, string? thirdName, string lastName, string motherName,
        CancellationToken cancellationToken)
    {
        var person = await GetPersonById(personId, cancellationToken);
        if (person is null)
            throw new KeyNotFoundException($"Person with ID {personId} was not found.");
        person.UpdateName(firstName, secondName, thirdName, lastName, motherName);
        await _dvldContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdatePersonContactInfo(Guid personId,string phone, string? altPhone,CancellationToken cancellationToken)
    {
        var person = await GetPersonById(personId, cancellationToken);
        if (person is null)
            throw new KeyNotFoundException($"Person with ID {personId} was not found.");
        person.UpdateContactInfo(phone, altPhone);
        await _dvldContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdatePersonalInfo(Guid personId, DateOnly dateOfBirth, string nationalityCountryCode,
        CancellationToken cancellationToken)
    {
        var person = await GetPersonById(personId, cancellationToken);
        if (person is null)
            throw new KeyNotFoundException($"Person with ID {personId} was not found.");
        person.UpdatePersonalInfo(dateOfBirth, nationalityCountryCode);
        await _dvldContext.SaveChangesAsync(cancellationToken);
    }
}