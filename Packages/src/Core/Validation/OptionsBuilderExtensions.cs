using Microsoft.Extensions.DependencyInjection;
using RDH.Core.Validation;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Options;

public static class OptionsBuilderExtensions
{
    public static OptionsBuilder<TOptions> ValidateFluentValidation<TOptions>(
        this OptionsBuilder<TOptions> builder)
        where TOptions : class
    {
        builder.Services.AddSingleton<IValidateOptions<TOptions>>(serviceProvider =>
            new OptionsValidatorBridge<TOptions>(
                serviceProvider,
                builder.Name));

        return builder;
    }
}