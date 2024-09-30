using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Pets.Requests;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DTOs.Volunteers;
using PetFamily.Application.Models;
using PetFamily.Application.VolunteersHandlers.Queries.GetFilteredPetsWithPagination;

namespace PetFamily.API.Controllers.Pets;

public class PetsController : ApplicationController
{
    [HttpGet]
    public async Task<ActionResult> Get(
        [FromQuery] GetFilteredPetsWithPaginationRequest request,
        [FromServices] IQueryHandler<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery> handler,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();

        var result = await handler.Handle(query, cancellationToken);
        
        return Ok(result.Value);
    }
}