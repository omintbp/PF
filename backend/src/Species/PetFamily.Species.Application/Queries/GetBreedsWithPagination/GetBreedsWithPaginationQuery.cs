using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Queries.GetBreedsWithPagination;

public record GetBreedsWithPaginationQuery(Guid SpeciesId, int Page, int PageSize) : IQuery;