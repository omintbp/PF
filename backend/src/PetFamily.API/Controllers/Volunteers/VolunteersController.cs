using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Volunteers.Requests;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.AddPetPhotos;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateRequisites;
using PetFamily.Application.Volunteers.UpdateSocialNetworks;

namespace PetFamily.API.Controllers;

public class VolunteersController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromBody] CreateVolunteerRequest volunteer,
        [FromServices] CreateVolunteerHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(volunteer, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromBody] UpdateMainInfoDto mainInfoDto,
        [FromServices] UpdateMainInfoHandler handler,
        [FromServices] IValidator<UpdateMainInfoRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateMainInfoRequest(id, mainInfoDto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/social-networks")]
    public async Task<ActionResult> UpdateSocialNetworks(
        [FromRoute] Guid id,
        [FromBody] UpdateSocialNetworksDto dto,
        [FromServices] UpdateSocialNetworksHandler handler,
        [FromServices] IValidator<UpdateSocialNetworksRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateSocialNetworksRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/requisites")]
    public async Task<ActionResult> UpdateRequisites(
        [FromRoute] Guid id,
        [FromBody] UpdateRequisitesDto dto,
        [FromServices] UpdateRequisitesHandler handler,
        [FromServices] IValidator<UpdateRequisitesRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateRequisitesRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteVolunteerRequestHandler handler,
        [FromServices] DeleteVolunteerRequestValidator validator,
        CancellationToken cancellationToken = default)
    {
        var request = new DeleteVolunteerRequest(id);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{volunteerId}/pet")]
    public async Task<ActionResult> AddPet(
        [FromRoute] Guid volunteerId,
        [FromBody] AddPetRequest request,
        [FromServices] IValidator<AddPetCommand> validator,
        [FromServices] AddPetCommandHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(volunteerId);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToValidationErrorResponse();

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
        [FromServices] IValidator<AddPetPhotosCommand> validator,
        CancellationToken cancellationToken = default)
    {
        await using var processor = new FormFileProcessor();

        var filesDtos = processor.Process(files);

        var command = new AddPetPhotosCommand(volunteerId, petId, filesDtos);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}