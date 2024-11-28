using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerRequests.Application.Queries.GetUnclaimedVolunteerRequestsWithPagination;

public record GetUnclaimedVolunteerRequestsWithPaginationQuery(
    int Page,
    int PageSize,
    string? SortBy,
    string? SortDirection) : IQuery;