using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.PetManagement.ValueObjects;

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