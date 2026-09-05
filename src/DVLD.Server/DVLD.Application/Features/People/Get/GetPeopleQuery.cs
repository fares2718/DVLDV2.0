using DVLD.Application.DTOs;
using DVLD.Domain.Views;
using MediatR;
using ErrorOr;

namespace DVLD.Application.Features.People.Get;

public sealed record GetPeopleQuery(
    string? Search,
    string? NationalId,
    string? Name,
    string? Gender,
    string? Phone,
    string? SortBy,
    bool IsDescending,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<ErrorOr<PagedList<PersonSummary>>>;