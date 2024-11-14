using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Queries.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IQuery;