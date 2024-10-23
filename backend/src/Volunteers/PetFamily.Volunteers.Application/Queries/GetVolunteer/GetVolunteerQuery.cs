using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Queries.GetVolunteer;

public record GetVolunteerQuery(Guid Id) : IQuery;