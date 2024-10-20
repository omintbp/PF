using PetFamily.Core.DTOs.Shared;
using PetFamily.Volunteers.Application.Commands.UpdateMainInfo;

namespace PetFamily.Volunteers.Presentation.Volunteers.Requests;

public record UpdateMainInfoRequest(
    FullNameDto FullName,
    string PhoneNumber,
    int Experience,
    string Description)
{
    public UpdateMainInfoCommand ToCommand(Guid id)
        => new(id, FullName, PhoneNumber, Experience, Description);
}