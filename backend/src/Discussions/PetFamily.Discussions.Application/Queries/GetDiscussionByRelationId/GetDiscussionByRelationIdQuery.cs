using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Queries.GetDiscussionByRelationId;

public record GetDiscussionByRelationIdQuery(
    Guid RelationId,
    int Page,
    int PageSize) : IQuery;