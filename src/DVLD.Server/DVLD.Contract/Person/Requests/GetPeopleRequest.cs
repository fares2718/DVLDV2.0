namespace DVLD.Contract.Person.Requests;

public sealed record GetPeopleRequest(
    string? Search,
    string? NationalId,
    string? Name,
    string? Gender,
    string? Phone,
    string? Email,
    string? SortBy,
    bool IsDescending,
    bool? IsActive,
    int PageNumber = 1,
    int PageSize = 10
    );