using PetFamily.Application.DTOs.Shared;

namespace PetFamily.Application.DTOs.Volunteers;

public class VolunteerDto
{
    public Guid Id { get; init; }
    
    public string Description { get; init; } = default!;
    
    public string Email { get; init; } = default!;
    
    public int Experience { get; init; }
    
    public string FirstName { get; init; } = default!;
    
    public string Patronymic { get; init; } = default!;
    
    public string Surname { get; init; } = default!;
    
    public string PhoneNumber { get; init; } = default!;
    
    public IEnumerable<RequisiteDto> Requisites { get; set; } = [];
    
    public IEnumerable<SocialNetworkDto> SocialNetworks { get; set; } = [];
}