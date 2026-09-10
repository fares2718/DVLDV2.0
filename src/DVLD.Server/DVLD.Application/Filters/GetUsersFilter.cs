namespace DVLD.Application.Filters;

public sealed record GetUsersFilter(
    string? Search,
    string? Username,
    string? NationalId,
    string? Phone,
    bool? IsActive,
    bool? IsLocked,
    int PageNumber = 1,
    int PageSize = 10,
    string? SortBy = null,
    bool IsDescending = false
);