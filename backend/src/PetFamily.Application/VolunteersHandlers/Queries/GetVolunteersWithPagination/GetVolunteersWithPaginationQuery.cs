using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersHandlers.Queries.GetVolunteersWithPagination;

public record GetVolunteersWithPaginationQuery(int Page, int PageSize) : IQuery;