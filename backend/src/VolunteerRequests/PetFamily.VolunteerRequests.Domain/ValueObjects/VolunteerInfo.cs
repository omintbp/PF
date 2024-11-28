using System.Collections;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.VolunteerRequests.Domain.ValueObjects;

public class VolunteerInfo : ValueObject
{
    private readonly List<Requisite> _requisites = [];

    private VolunteerInfo()
    {
    }

    private VolunteerInfo(
        Experience experience,
        IEnumerable<Requisite> requisites)
    {
        Experience = experience;
        _requisites = requisites.ToList();
    }

    public Experience Experience { get; }

    public IReadOnlyList<Requisite> Requisites => _requisites;

    public static Result<VolunteerInfo, Error> Create(
        Experience experience,
        IEnumerable<Requisite> requisites)
    {
        return new VolunteerInfo(experience, requisites);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Experience.Value;

        foreach (var requisite in _requisites)
            yield return requisite;
    }
}