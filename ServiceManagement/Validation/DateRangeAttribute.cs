using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServiceManagement.Validation;

    /// <summary>
    /// Validates that a DateTime property falls within a specified range.
    /// </summary>
    public class DateRangeAttribute : ValidationAttribute
    {
        public DateTime MinDate { get; }
        public DateTime MaxDate { get; }

        public DateRangeAttribute(string minDate, string maxDate)
        {
            if (!DateTime.TryParse(minDate, out var min))
                throw new ArgumentException("Invalid minimum date format.", nameof(minDate));

            if (!DateTime.TryParse(maxDate, out var max))
                throw new ArgumentException("Invalid maximum date format.", nameof(maxDate));

            MinDate = min;
            MaxDate = max;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
                return ValidationResult.Success; // Let [Required] handle nulls

            if (value is DateTime dateValue)
            {
                if (dateValue < MinDate || dateValue > MaxDate)
                {
                    return new ValidationResult(
                        ErrorMessage ?? $"Date must be between {MinDate:yyyy-MM-dd} and {MaxDate:yyyy-MM-dd}.");
                }
                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid date format.");
        }
    }

