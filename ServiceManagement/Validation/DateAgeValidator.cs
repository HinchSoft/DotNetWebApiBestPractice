using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceManagement.Validation;

public class DateAgeAttribute : ValidationAttribute
{
    public int MaxAge { get; }

    public DateAgeAttribute(int maxAge)
    {
        MaxAge = maxAge;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success; // Let [Required] handle nulls

        if (value is DateTime dateValue)
        {
            var tp = validationContext.GetService<TimeProvider>();
            var minDate = tp.GetLocalNow().AddYears(0-MaxAge);
            if (dateValue < minDate || dateValue > DateTime.Now)
            {
                return new ValidationResult(
                    ErrorMessage ?? $"Date must be between {minDate:yyyy-MM-dd} and now.");
            }
            return ValidationResult.Success;
        }

        return new ValidationResult("Invalid date format.");
    }


}
