using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Providers;

public record FileInfo(FilePath FilePath, string BucketName);