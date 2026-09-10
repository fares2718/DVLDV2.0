using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DVLD.Application.Features.People.Update;

public sealed record UploadImageCommand(
    Guid PersonId,
    IFormFile Image,
    string FileName
    ) : IRequest<ErrorOr<Success>>;