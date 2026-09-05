using DVLD.Domain.Entities;
using DVLD.Domain.Views;

namespace DVLD.Application.Abstractions.Persistence;

public interface IPersonRepository
{
    Task AddAsync(Person person);
    Task DeleteAsync(Guid personId);
    Task<IReadOnlyList<PersonSummary>> GetAllSummaryAsync();
    Task<PersonSummary?> GetSummaryByIdAsync(Guid personId);
    Task<bool> IsNationalIdUnique(string nationalId);
    Task<bool> IsEmailUnique(string email);
}