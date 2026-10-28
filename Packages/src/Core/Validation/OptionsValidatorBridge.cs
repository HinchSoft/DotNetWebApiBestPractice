using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace RDH.Core.Validation;

public class OptionsValidatorBridge<TOptions> :
    IValidateOptions<TOptions>
    where TOptions : class
{
    private readonly IServiceProvider _serviceProvider;
    private readonly string? _name;

    public OptionsValidatorBridge(IServiceProvider serviceProvider, string? name)
    {
        _serviceProvider = serviceProvider;
        _name = name;
    }


    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        if (_name is not null && _name != name)
        {
            return ValidateOptionsResult.Skip;
        }

        ArgumentNullException.ThrowIfNull(options);

        // locate the FluentValidator, if one doesn't exist return success
        using var scope = _serviceProvider.CreateScope();
        var validator = scope.ServiceProvider.GetService<IValidator<TOptions>>();
        if (validator is null) return ValidateOptionsResult.Success;

        // run the validator
        var result = validator.Validate(options);
        if (result.IsValid)
        {
            return ValidateOptionsResult.Success;
        }

        var type = options.GetType().Name;
        var errors =
            result.Errors.Select(failure => $"Validation failed for {type}.{failure.PropertyName} "
                                            + $"with the error: {failure.ErrorMessage}").ToList();

        return ValidateOptionsResult.Fail(errors);
    }
}