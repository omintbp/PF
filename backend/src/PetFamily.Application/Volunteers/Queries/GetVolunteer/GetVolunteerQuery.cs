using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Queries.GetVolunteer;

public record GetVolunteerQuery(Guid Id) : IQuery;