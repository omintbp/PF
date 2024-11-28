namespace PetFamily.VolunteerRequests.Contracts.Requests;

public record GetVolunteerRequestByUserIdWithPaginationRequest(
    int Page,
    int PageSize,
    string? Status,
    string? SortBy,
    string? SortDirection);