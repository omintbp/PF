using PetFamily.Application.SharedDTOs;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Providers;

public record FileData(Stream Content, FilePath FilePath, string BucketName);