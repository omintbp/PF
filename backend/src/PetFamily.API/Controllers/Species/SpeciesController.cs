using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Species.Requests;
using PetFamily.API.Extensions;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DTOs.Species;
using PetFamily.Application.Models;
using PetFamily.Application.SpeciesHandlers.Commands.Create;
using PetFamily.Application.SpeciesHandlers.Commands.CreateBreed;
using PetFamily.Application.SpeciesHandlers.Commands.Delete;
using PetFamily.Application.SpeciesHandlers.Queries.GetBreedsWithPagination;
using PetFamily.Application.SpeciesHandlers.Queries.GetSpeciesWithPagination;

namespace PetFamily.API.Controllers.Species;

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