namespace PetFamily.VolunteerRequests.Contracts.Requests;

public record GetVolunteerRequestByAdminWithPaginationRequest(
    int Page,
    int PageSize,
    string? Status,
    string? SortBy,
    string? SortDirection);