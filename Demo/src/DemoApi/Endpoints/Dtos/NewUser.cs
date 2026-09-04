using System.ComponentModel.DataAnnotations;

namespace DemoApi.Endpoints.Dtos;

public record NewUser(
    string FirstName,
    string LastName,
    string Email,
    DateTime? DatOfBirth);

// DateTime is nullable even though it is marked as required, this prevents the JSon deserializer erroring
// if it cant resolve the date time. The validation rules will check for its pressence and return a meaningful
// error message
