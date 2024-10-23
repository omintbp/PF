using PetFamily.Core.Models;
using PetFamily.Volunteers.Application.Queries.GetFilteredPetsWithPagination;

namespace PetFamily.Volunteers.Presentation.Pets.Requests;

public record GetFilteredPetsWithPaginationRequest(
    Guid? VolunteerId,
    string? PetName,
    string? SpeciesName,
    string? BreedName,
    string? Color,
    int? MaxAge,
    int? MinAge,
    string? Country,
    string? City,
    string? Street,
    string? House,
    string? PhoneNumber,
    double? MinHeight,
    double? MaxHeight,
    double? MinWeight,
    double? MaxWeight,
    SortPetBy? SortPetBy,
    SortDirection? SortPetDirection,
    int Page,
    int PageSize)
{
    public GetFilteredPetsWithPaginationQuery ToQuery() =>
        new(
            VolunteerId,
            PetName,
            SpeciesName,
            BreedName,
            Color,
            MaxAge,
            MinAge,
            Country,
            City,
            Street,
            House,
            PhoneNumber,
            MinHeight,
            MaxHeight,
            MinWeight,
            MaxWeight,
            SortPetBy,
            SortPetDirection,
            Page,
            PageSize);
}