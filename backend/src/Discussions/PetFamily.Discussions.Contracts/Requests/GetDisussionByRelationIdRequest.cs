namespace PetFamily.Discussions.Contracts.Requests;

public record GetDisussionByRelationIdRequest(
    int Page,
    int PageSize
);