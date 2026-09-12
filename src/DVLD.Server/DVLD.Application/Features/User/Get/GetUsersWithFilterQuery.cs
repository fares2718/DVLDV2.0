using DVLD.Application.DTOs;
using DVLD.Application.Filters.User;
using DVLD.Domain.Views;
using MediatR;
using ErrorOr;

namespace DVLD.Application.Features.User.Get;

public sealed record GetUsersWithFilterQuery(GetUsersFilter Filter) : IRequest<ErrorOr<PagedList<UserView>>>;