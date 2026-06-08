using FluentValidation;

namespace Domain;

public static class ValidationExtensions
{
    const string DataStatus = "Status";
    const string DataStatusCode = "StatusCode";
 
    extension(ValidationException exception)
    {
        public int GetStatus()
        {
            if (exception.Data.Contains(ValidationExtensions.DataStatus))
                return (int)exception.Data[ValidationExtensions.DataStatus]!;
  
            return 400;
        }
        
        public string GetStatusCode()
        {
            if (exception.Data.Contains(ValidationExtensions.DataStatusCode))
                return (string)exception.Data[ValidationExtensions.DataStatusCode]!;
  
            return "Bad Request";
        }
        
        public static void ThrowConflict(string message)
        {
            var exp = new ValidationException(message);
            exp.Data.Add(DataStatus, 409);
            exp.Data.Add(DataStatusCode, "Conflict");
            throw exp;
        }
    }
}