using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.Domain.ValueObjects;

public record PaymentDetails
{
    public IReadOnlyList<Requisite> Requisites { get; }

    private PaymentDetails() {}

    public PaymentDetails(IEnumerable<Requisite> requisites)
    {
        Requisites = requisites.ToList();
    }
}