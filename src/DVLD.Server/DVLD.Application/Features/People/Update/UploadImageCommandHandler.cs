using DVLD.Application.Abstractions.Persistence;
using DVLD.Application.Abstractions.Services;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Update;

public sealed class UploadImageCommandHandler(IImageService imageService, IUnitOfWork uow, UploadImageValidator validator)
    : IRequestHandler<UploadImageCommand, ErrorOr<Success>>
{
    private readonly IImageService _imageService = imageService;
    private readonly UploadImageValidator _validator = validator;
    private readonly IUnitOfWork _uow = uow;

    public async Task<ErrorOr<Success>> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request,cancellationToken);

        if (!validationResult.IsValid)
            return Error.Validation("Image.Rule.Violation",validationResult.Errors.First().ErrorMessage);

        try
        {
            string imagePath = await _imageService.UploadImage(request.Image, request.FileName);
            await _uow.PersonRepository.UploadImageAsync(request.PersonId, imagePath, cancellationToken);
            return Result.Success;
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return Error.NotFound("Person.NotFound", e.Message);
        }
        catch (Exception e)
        {
            return Error.Failure("Error", e.Message);
        }

    }
}