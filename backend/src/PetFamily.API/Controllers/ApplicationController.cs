using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Response;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ApplicationController : ControllerBase
{
    public override OkObjectResult Ok(object? response)
    {
        var envelope = Envelope.Ok(response);
        return base.Ok(envelope);
    }
}