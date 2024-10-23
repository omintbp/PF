using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;

namespace PetFamily.Volunteers.Application.Queries.GetFilteredPetsWithPagination;

public record GetFilteredPetsWithPaginationQuery(
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
    int PageSize) : IQuery;