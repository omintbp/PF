using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesHandlers.Queries.GetSpeciesWithPagination;

public record GetSpeciesWithPaginationQuery(int Page, int PageSize) : IQuery;