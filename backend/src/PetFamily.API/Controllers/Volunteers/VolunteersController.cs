using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Volunteers.Requests;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DTOs.Shared;
using PetFamily.Application.DTOs.Volunteers;
using PetFamily.Application.Models;
using PetFamily.Application.VolunteersHandlers.Commands.AddPet;
using PetFamily.Application.VolunteersHandlers.Commands.AddPetPhotos;
using PetFamily.Application.VolunteersHandlers.Commands.Create;
using PetFamily.Application.VolunteersHandlers.Commands.Delete;
using PetFamily.Application.VolunteersHandlers.Commands.DeletePetPhotos;
using PetFamily.Application.VolunteersHandlers.Commands.UpdateMainInfo;
using PetFamily.Application.VolunteersHandlers.Commands.UpdatePet;
using PetFamily.Application.VolunteersHandlers.Commands.UpdateRequisites;
using PetFamily.Application.VolunteersHandlers.Commands.UpdateSocialNetworks;
using PetFamily.Application.VolunteersHandlers.Queries.GetVolunteer;
using PetFamily.Application.VolunteersHandlers.Queries.GetVolunteersWithPagination;

namespace PetFamily.API.Controllers.Volunteers;

public class VolunteersController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromBody] CreateVolunteerRequest request,
        [FromServices] CreateVolunteerHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand();

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromBody] UpdateMainInfoRequest request,
        [FromServices] UpdateMainInfoHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/social-networks")]
    public async Task<ActionResult> UpdateSocialNetworks(
        [FromRoute] Guid id,
        [FromBody] UpdateSocialNetworksRequest request,
        [FromServices] UpdateSocialNetworksHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/requisites")]
    public async Task<ActionResult> UpdateRequisites(
        [FromRoute] Guid id,
        [FromBody] UpdateRequisitesRequest request,
        [FromServices] UpdateRequisitesHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteVolunteerRequestHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteVolunteerCommand(id);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{volunteerId}/pet")]
    public async Task<ActionResult> AddPet(
        [FromRoute] Guid volunteerId,
        [FromBody] AddPetRequest request,
        [FromServices] AddPetCommandHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(volunteerId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{volunteerId}/pet/{petId}/photos")]
    public async Task<ActionResult> AddPetPhotos(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] IFormFileCollection files,
        [FromServices] AddPetPhotosCommandHandler handler,
        CancellationToken cancellationToken = default)
    {
        await using var processor = new FormFileProcessor();

        var filesDtos = processor.Process(files);

        var command = new AddPetPhotosCommand(volunteerId, petId, filesDtos);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult> Get(
        [FromQuery] GetVolunteersWithPaginationRequest request,
        [FromServices] IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery> handler,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();

        var result = await handler.Handle(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpGet("{id::guid}")]
    public async Task<ActionResult> GetById(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<VolunteerDto, GetVolunteerQuery> handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetVolunteerQuery(id);

        var result = await handler.Handle(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{volunteerId:guid}/pets/{petId:guid}")]
    public async Task<ActionResult> UpdatePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] UpdatePetRequest request,
        [FromServices] ICommandHandler<Guid, UpdatePetCommand> handler,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(volunteerId, petId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpDelete("{volunteerId:guid}/pets/{petId:guid}/photos")]
    public async Task<ActionResult> DeletePetPhotos(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromQuery] DeletePetPhotosRequest request,
        ICommandHandler<DeletePetPhotosCommand> handler,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(volunteerId, petId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
    
}