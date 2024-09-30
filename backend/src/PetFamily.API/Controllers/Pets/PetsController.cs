using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Pets.Requests;
using PetFamily.API.Extensions;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DTOs.Volunteers;
using PetFamily.Application.Models;
using PetFamily.Application.VolunteersHandlers.Queries.GetFilteredPetsWithPagination;
using PetFamily.Application.VolunteersHandlers.Queries.GetPetById;

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
    
    [HttpGet("{petId::guid}")]
    public async Task<ActionResult> GetById(
        [FromRoute] Guid petId,
        [FromServices] IQueryHandler<PetDto, GetPetByIdQuery> handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPetByIdQuery(petId);

        var result = await handler.Handle(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}