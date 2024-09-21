using PetFamily.Application.SharedDTOs;
using PetFamily.Application.Volunteers.UpdateMainInfo;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdateMainInfoRequest(
    FullNameDto FullName,
    string PhoneNumber,
    int Experience,
    string Description)
{
    public UpdateMainInfoCommand ToCommand(Guid id)
        => new(id, FullName, PhoneNumber, Experience, Description);
}