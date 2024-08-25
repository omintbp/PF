using PetFamily.Domain.Entities.SharedValueObjects;

namespace PetFamily.Domain.Entities.Volunteers;

public record VolunteerRequisites
{
    public IReadOnlyList<Requisite> Values { get; }

    private VolunteerRequisites()
    {
    }

    public VolunteerRequisites(IEnumerable<Requisite> requisites)
    {
        Values = requisites.ToList();
    }
}