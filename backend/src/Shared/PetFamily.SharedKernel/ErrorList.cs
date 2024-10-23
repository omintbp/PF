using System.Collections;

namespace PetFamily.SharedKernel;

public class ErrorList : IEnumerable<Error>
{
    private readonly List<Error> _errors;

    public ErrorList(IEnumerable<Error> errors)
    {
        _errors = [.. errors];
    }

    public Error this[int index] => _errors[index];

    public IEnumerator<Error> GetEnumerator()
    {
        return _errors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static implicit operator ErrorList(List<Error> errors)
        => new(errors);

    public static implicit operator ErrorList(Error error)
        => new([error]);
}