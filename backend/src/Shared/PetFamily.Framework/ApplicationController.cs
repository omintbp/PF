using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Models;

namespace PetFamily.Framework;

[ApiController]
[Route("api/[controller]")]
public class ApplicationController : ControllerBase
{
    public override OkObjectResult Ok(object? response)
    {
        var envelope = Envelope.Ok(response);
        return base.Ok(envelope);
    }
}