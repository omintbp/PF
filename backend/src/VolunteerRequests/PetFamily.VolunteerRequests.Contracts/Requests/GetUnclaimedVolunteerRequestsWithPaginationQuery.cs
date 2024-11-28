namespace PetFamily.VolunteerRequests.Contracts.Requests;

public record GetUnclaimedVolunteerRequestsWithPaginationRequest(
    int Page,
    int PageSize,
    string? SortBy,
    string? SortDirection);