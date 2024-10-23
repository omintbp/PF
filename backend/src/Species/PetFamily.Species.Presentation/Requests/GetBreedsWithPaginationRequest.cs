using PetFamily.Species.Application.Queries.GetBreedsWithPagination;

namespace PetFamily.Species.Presentation.Requests;

public record GetBreedsWithPaginationRequest(int Page, int PageSize)
{
    public GetBreedsWithPaginationQuery ToQuery(Guid speciesId) =>
        new(speciesId, Page, PageSize);
}