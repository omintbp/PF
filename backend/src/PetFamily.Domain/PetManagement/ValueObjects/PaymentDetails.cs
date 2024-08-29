using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record PaymentDetails
{
    public IReadOnlyList<Requisite> Requisites { get; }

    private PaymentDetails() {}

    private PaymentDetails(List<Requisite> requisites)
    {
        Requisites = requisites;
    }
}