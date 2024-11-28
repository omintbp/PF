using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerRequests.Application.Queries.GetVolunteerRequestByAdminWithPagination;

public record GetVolunteerRequestByAdminWithPaginationQuery(
    Guid AdminId,
    int Page,
    int PageSize,
    string? Status,
    string? SortBy,
    string? SortDirection) : IQuery;