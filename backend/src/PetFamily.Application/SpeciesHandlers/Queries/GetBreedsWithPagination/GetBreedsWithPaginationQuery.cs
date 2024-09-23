using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesHandlers.Queries.GetBreedsWithPagination;

public record GetBreedsWithPaginationQuery(Guid SpeciesId, int Page, int PageSize) : IQuery;