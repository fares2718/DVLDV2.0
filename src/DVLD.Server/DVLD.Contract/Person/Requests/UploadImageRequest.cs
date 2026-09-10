using Microsoft.AspNetCore.Http;

namespace DVLD.Contract.Person.Requests;

public sealed record UploadImageRequest(
    IFormFile Image,
    string FileName
    );