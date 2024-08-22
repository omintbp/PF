using PetFamily.Domain.Entities.SharedValueObjects;

namespace PetFamily.Domain.Entities.Pets;

public record PaymentDetails
{
    private readonly List<Requisite> _requisites = [];

    public IReadOnlyList<Requisite> Requisites => _requisites;

    private PaymentDetails(List<Requisite> requisites)
    {
        _requisites = requisites;
    }

    private PaymentDetails() {}
    
    public static PaymentDetails Create(List<Requisite> requisites)
    {
        var details = new PaymentDetails(requisites);
        
        return details;
    }
}