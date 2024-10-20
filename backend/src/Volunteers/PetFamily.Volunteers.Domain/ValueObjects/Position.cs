using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Domain.ValueObjects;

public record Position
{
    public static Position First => new(1);

    private Position(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public Result<Position, Error> MoveForward() => Create(Value + 1);

    public Result<Position, Error> MoveBackward() => Create(Value - 1);

    public static Result<Position, Error> Create(int value)
    {
        if (value < First.Value)
            return Errors.General.ValueIsInvalid(nameof(Position));

        return new Position(value);
    }
}