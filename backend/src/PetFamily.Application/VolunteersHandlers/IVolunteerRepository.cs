using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;

namespace PetFamily.Application.VolunteersHandlers;

public interface IVolunteerRepository
{
    Task<VolunteerId> Add(Volunteer request, CancellationToken cancellationToken = default!);
    
    Task<IEnumerable<Volunteer>> GetAll(CancellationToken cancellationToken = default!);

    Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken = default!);

    Task<Guid> Save(Volunteer volunteer, CancellationToken cancellationToken = default!);
    
    Task<Guid> Delete(Volunteer volunteer, CancellationToken cancellationToken = default!);
}