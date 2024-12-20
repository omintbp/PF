using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.SharedKernel;

namespace PetFamily.Core.Validation;

public static class CustomValidator
{
    public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
        this IRuleBuilder<T, TElement> ruleBuilder,
        Func<TElement, Result<TValueObject, Error>> factoryMethod,
        Predicate<TElement>? predicate = null)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            if (predicate?.Invoke(value) == false) return;
            
            var result = factoryMethod(value);

            if (result.IsSuccess)
                return;

            context.AddFailure(result.Error.Serialize());
        });
    }

    public static IRuleBuilder<T, TElement> WithError<T, TElement>(
        this IRuleBuilderOptions<T, TElement> ruleBuilder,
        Error error)
    {
        ruleBuilder.WithMessage(error.Serialize());

        return ruleBuilder;
    }

    public static IRuleBuilderOptionsConditions<T, string> MustBeAllowedExtension<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        IEnumerable<string> allowedExtensions)
    {
        return ruleBuilder.Custom((path, context) =>
        {
            var extension = Path.GetExtension(path);

            var isAllowedExtension = allowedExtensions.Contains(extension);

            if (isAllowedExtension == false)
                context.AddFailure(Error.Validation("file.path", "Extension is invalid", "path")
                    .Serialize());
        });
    }
}