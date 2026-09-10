using DVLD.Application.DTOs;
using DVLD.Application.Features.People.Get;
using DVLD.Domain.Entities;
using DVLD.Domain.Views;

namespace DVLD.Application.Abstractions.Persistence;

public interface IPersonRepository
{
    Task ActivateAsync(Guid personId,CancellationToken cancellationToken);
    Task AddAsync(Person person, CancellationToken cancellationToken);
    Task DeActivateAsync(Guid personId, CancellationToken cancellationToken);
    Task<PagedList<PersonSummary>> GetPeopleSummaryAsync( GetPeopleQuery query,
        CancellationToken cancellationToken);
    Task<PersonSummary?> GetSummaryByIdAsync(Guid personId, CancellationToken cancellationToken);
    Task<bool> IsNationalIdUnique(string nationalId);
    Task<bool> IsEmailUnique(string email);

    Task UpdatePersonName(
        Guid personId,
        string firstName,
        string secondName,
        string? thirdName,
        string lastName,
        string motherName,
        CancellationToken cancellationToken);

    Task UpdatePersonContactInfo(Guid personId,
        string phone, string? altPhone,
        CancellationToken cancellationToken);

    Task UpdatePersonalInfo(Guid personId, DateOnly dateOfBirth,
        string nationalityCountryCode, CancellationToken cancellationToken);
    
    Task UploadImageAsync(Guid personId,string imagePath ,CancellationToken cancellationToken = default);
}