using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.Application.Providers;

public record FileInfo(FilePath FilePath, string BucketName);