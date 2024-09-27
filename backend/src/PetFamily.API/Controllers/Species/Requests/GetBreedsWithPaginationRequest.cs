using PetFamily.Application.SpeciesHandlers.Queries.GetBreedsWithPagination;

namespace PetFamily.API.Controllers.Species.Requests;

public record GetBreedsWithPaginationRequest(int Page, int PageSize)
{
    public GetBreedsWithPaginationQuery ToQuery(Guid speciesId) =>
        new(speciesId, Page, PageSize);
}