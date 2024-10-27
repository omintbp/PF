using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Queries.CheckIfUserHasPermission;

public record CheckIfUserHasPermissionQuery(Guid UserId, string Permission) : IQuery;