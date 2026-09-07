using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected readonly ISender _sender;

    public BaseController(ISender sender)
    {
        _sender = sender;
    }
}