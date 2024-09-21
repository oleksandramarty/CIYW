using CommonModule.Shared.Constants;
using FluentValidation;

namespace CommonModule.Core.Extensions;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> CheckHarmfulContent<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(u => u.NotContainMaliciousContent()).WithMessage(ErrorMessages.PotentialHarmfulContent);
    }
}