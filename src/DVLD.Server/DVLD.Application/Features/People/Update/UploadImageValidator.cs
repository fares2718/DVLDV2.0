using FluentValidation;

namespace DVLD.Application.Features.People.Update;

public class UploadImageValidator : AbstractValidator<UploadImageCommand>
{
    private const int MaxFileSizeInBytes = 5 * 1024 * 1024; // 5 MB
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];
    private readonly string[] _allowedContentTypes = ["image/jpeg", "image/png"];
    public UploadImageValidator()
    {
        // 1. Ensure file is not null or empty
        RuleFor(x => x.Image)
            .NotNull().WithMessage("A file must be uploaded.")
            .Must(file => file.Length > 0).WithMessage("Uploaded file is empty.");

        // 2. Validate File Size
        RuleFor(x => x.Image.Length)
            .LessThanOrEqualTo(MaxFileSizeInBytes)
            .WithMessage($"File size must be less than {MaxFileSizeInBytes / (1024 * 1024)} MB.");

        // 3. Validate Content Type (MIME)
        RuleFor(x => x.Image.ContentType)
            .Must(contentType => _allowedContentTypes.Contains(contentType.ToLower()))
            .WithMessage("Invalid image format. Only JPEG, JPG , and PNG are allowed.");

        // 4. Validate File Extension
        RuleFor(x => x.Image.FileName)
            .Must(fileName => 
            {
                var extension = Path.GetExtension(fileName);
                return !string.IsNullOrEmpty(extension) && _allowedExtensions.Contains(extension.ToLower());
            })
            .WithMessage("File extension is not allowed.");
    }
}