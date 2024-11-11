namespace PetFamily.Volunteers.Infrastructure.Options;

public class EntitiesCleanerOptions
{
    public const string ENTITIES_CLEANER = "EntitiesCleaner";

    public int ExpiredDaysTime { get; init; }

    public int CleaningIntervalDays { get; init; }
}