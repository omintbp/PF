using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersHandlers.Queries.GetPetById;

public record GetPetByIdQuery(Guid PetId) : IQuery;