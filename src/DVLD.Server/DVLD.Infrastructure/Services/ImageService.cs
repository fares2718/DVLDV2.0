using DVLD.Application.Abstractions.Services;
using Microsoft.AspNetCore.Http;

namespace DVLD.Infrastructure.Services;

internal class ImageService : IImageService
{
    public void DeleteImage(string src)
    {
        string wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        string imagePath = Path.Combine(wwwrootPath, src.TrimStart('/'));
        if (File.Exists(imagePath))
        {
            File.Delete(imagePath);
        }
    }

    public async Task<string> UploadImage(IFormFile imageFile, string src)
    {
        string folderName = src.Replace(" ", "_");
        string uploadPath = Path.Combine("wwwroot", "Images", folderName);
        
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }
        
        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
        string filePath = Path.Combine(uploadPath, fileName);

        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }
        string uploadedImageUrl = $"/Images/{folderName}/{fileName}";

        return uploadedImageUrl;
    }
}