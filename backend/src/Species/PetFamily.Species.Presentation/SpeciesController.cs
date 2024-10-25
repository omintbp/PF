using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs.Species;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Framework.Extensions;
using PetFamily.Species.Application.Commands.Create;
using PetFamily.Species.Application.Commands.CreateBreed;
using PetFamily.Species.Application.Commands.Delete;
using PetFamily.Species.Application.Commands.DeleteBreed;
using PetFamily.Species.Application.Queries.GetBreedsWithPagination;
using PetFamily.Species.Application.Queries.GetSpeciesWithPagination;
using PetFamily.Species.Presentation.Requests;

namespace PetFamily.Species.Presentation;

[Authorize]
public class SpeciesController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromBody] CreateSpeciesRequest request,
        [FromServices] ICommandHandler<Guid, CreateSpeciesCommand> handler,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand();
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [HttpPost("{speciesId::guid}/breed")]
    public async Task<ActionResult> CreateBreed(
        [FromRoute] Guid speciesId,
        [FromBody] CreateBreedRequest request,
        [FromServices] ICommandHandler<Guid, CreateBreedCommand> handler,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(speciesId);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [HttpDelete("{speciesId::guid}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid speciesId,
        [FromServices] ICommandHandler<DeleteSpeciesCommand> handler,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteSpeciesCommand(speciesId);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }
    
    [HttpDelete("{speciesId::guid}/breeds/{breedId::guid}")]
    public async Task<ActionResult> DeleteBreed(
        [FromRoute] Guid speciesId,
        [FromRoute] Guid breedId,
        [FromServices] ICommandHandler<DeleteBreedCommand> handler,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteBreedCommand(speciesId, breedId);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult> Get(
        [FromQuery] GetSpeciesWithPaginationRequest request,
        [FromServices] IQueryHandler<PagedList<SpeciesDto>, GetSpeciesWithPaginationQuery> handler,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();
        
        var result = await handler.Handle(query, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [HttpGet("{speciesId::guid}/breeds")]
    public async Task<ActionResult> GetBreeds(
        [FromRoute] Guid speciesId,
        [FromQuery] GetBreedsWithPaginationRequest request,
        [FromServices] IQueryHandler<PagedList<BreedDto>, GetBreedsWithPaginationQuery> handler,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery(speciesId);
        
        var result = await handler.Handle(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}