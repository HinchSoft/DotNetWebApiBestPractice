using ServiceManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace DemoApi.Endpoints.Dtos;

public record NewUser(
    [Required(ErrorMessage = "First Name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "First name must be between 3 and 50 characters")]
    string FirstName,
    [Required(ErrorMessage = "Last Name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Last name must be between 3 and 50 characters")]
    string LastName,
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    string Email,
    [Required(ErrorMessage = "Date of birth is required")]
    [DateAge(100)]
    DateTime? DatOfBirth);

// DateTime is nullable even though it is marked as required, this prevents the JSon deserializer erroring
// if it cant resolve the date time. The validation rules will check for its pressence and return a meaningful
// error message