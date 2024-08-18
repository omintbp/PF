namespace PetFamily.Domain.Entities.ValueObjects;

public record Requisite
{
    public string Name { get; private set; }
    
    public string Description { get; private set; }

    private Requisite(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public static Requisite Create(string name, string description)
    {
        var requisite = new Requisite(name, description);
        
        return requisite;
    }
}