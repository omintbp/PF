using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.Domain.ValueObjects;

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