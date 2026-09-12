using DVLD.Domain.Views;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.User.Get;

public sealed record GetUserByUsernameQuery(string Username) : IRequest<ErrorOr<UserDetailsView>>;