namespace DVLD.Contracts.People;

public sealed record GetPeopleRequest(
string? Name,
string? Gender,
string? Phone,
string? SortBy,
bool IsDescending,
int PageNumber = 1,
int PageSize = 10
    );