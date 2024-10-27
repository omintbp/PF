using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs.Volunteers;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Framework.Extensions;
using PetFamily.Volunteers.Application.Queries.GetFilteredPetsWithPagination;
using PetFamily.Volunteers.Application.Queries.GetPetById;
using PetFamily.Volunteers.Presentation.Pets.Requests;

namespace PetFamily.Volunteers.Presentation.Pets;

[Authorize]
public class PetsController : ApplicationController
{
    [HttpGet]
    [Permission(Permissions.Pets.ReadPet)]
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
    [Permission(Permissions.Pets.ReadPet)]
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