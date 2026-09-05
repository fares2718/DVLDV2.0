using DVLD.Application.DTOs;
using DVLD.Application.Features.People.Get;
using DVLD.Domain.Entities;
using DVLD.Domain.Views;

namespace DVLD.Application.Abstractions.Persistence;

public interface IPersonRepository
{
    Task AddAsync(Person person, CancellationToken cancellationToken);
    Task DeleteAsync(Guid personId, CancellationToken cancellationToken);
    Task<PagedList<PersonSummary>> GetPeopleSummaryAsync( GetPeopleQuery query,
        CancellationToken cancellationToken);
    Task<PersonSummary?> GetSummaryByIdAsync(Guid personId, CancellationToken cancellationToken);
    Task<bool> IsNationalIdUnique(string nationalId);
    Task<bool> IsEmailUnique(string email);
}