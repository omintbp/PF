using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.Volunteers.Domain.AggregateRoot;

namespace PetFamily.Volunteers.Application;

public interface IVolunteerRepository
{
    Task<VolunteerId> Add(Volunteer request, CancellationToken cancellationToken = default!);
    
    Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken = default!);

    Task<Guid> Save(Volunteer volunteer, CancellationToken cancellationToken = default!);
    
    Task<Guid> Delete(Volunteer volunteer, CancellationToken cancellationToken = default!);
}