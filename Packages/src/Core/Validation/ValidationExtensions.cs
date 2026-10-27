using FluentValidation;

// ReSharper disable once CheckNamespace
namespace FluentValidation;

public static class ValidationExtensions
{
    private const string DataStatus = "Status";
    private const string DataStatusCode = "StatusCode";

    extension(ValidationException exception)
    {
        /// <summary>
        ///  Gets the status fron the Exception.Data
        /// </summary>
        /// <returns>Int status (default 400)</returns>
        public int GetStatus()
        {
            if (exception.Data.Contains(ValidationExtensions.DataStatus))
                return (int)exception.Data[ValidationExtensions.DataStatus]!;

            return 400;
        }

        /// <summary>
        ///  Gets the statusCode fron the Exception.Data
        /// </summary>
        /// <returns>Int status (default 'Bad Request')</returns>
        public string GetStatusCode()
        {
            if (exception.Data.Contains(ValidationExtensions.DataStatusCode))
                return (string)exception.Data[ValidationExtensions.DataStatusCode]!;

            return "Bad Request";
        }

        /// <summary>
        /// Throws a ValidationException with data set to 400 'Bad Request', with the specified message
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="ValidationException"></exception>
        public static void ThrowConflict(string message)
        {
            var exp = new ValidationException(message);
            exp.Data.Add(DataStatus, 409);
            exp.Data.Add(DataStatusCode, "Conflict");
            throw exp;
        }
    }
}