using DVLD.Domain.Views;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.User.Get;

public sealed record GetUserByIdQuery(Guid UserId) : IRequest<ErrorOr<UserDetailsView>>;