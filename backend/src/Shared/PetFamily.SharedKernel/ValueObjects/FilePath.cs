using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.ValueObjects;

public record FilePath
{
    public static FilePath None => new(string.Empty);

    private FilePath(string path)
    {
        Path = path;
    }

    public string Path { get; }

    public static Result<FilePath, Error> Create(Guid path, string extension)
    {
        if (string.IsNullOrWhiteSpace(extension) || extension.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(extension));

        var fullPath = path + extension;

        return new FilePath(fullPath);
    }

    public static Result<FilePath, Error> Create(string fullPath)
    {
        if (string.IsNullOrWhiteSpace(fullPath) || fullPath.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(fullPath));

        return new FilePath(fullPath);
    }
}