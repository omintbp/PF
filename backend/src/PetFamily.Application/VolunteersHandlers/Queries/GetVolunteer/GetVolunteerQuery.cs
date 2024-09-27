using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersHandlers.Queries.GetVolunteer;

public record GetVolunteerQuery(Guid Id) : IQuery;