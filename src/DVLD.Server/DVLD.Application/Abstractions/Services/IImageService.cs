using Microsoft.AspNetCore.Http;

namespace DVLD.Application.Abstractions.Services;

public interface IImageService
{
    void DeleteImage(string imagePath);
    Task<string> UploadImage(IFormFile file,string src);
}