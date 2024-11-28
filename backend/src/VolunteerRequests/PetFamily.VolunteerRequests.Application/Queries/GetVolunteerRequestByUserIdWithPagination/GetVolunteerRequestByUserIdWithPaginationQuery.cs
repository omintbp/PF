using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerRequests.Application.Queries.GetVolunteerRequestByUserIdWithPagination;

public record GetVolunteerRequestByUserIdWithPaginationQuery(
    Guid UserId,
    int Page,
    int PageSize,
    string? Status,
    string? SortBy,
    string? SortDirection) : IQuery;