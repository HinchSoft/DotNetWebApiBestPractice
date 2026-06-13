using DemoDomain.Models;
using Domain.PIILogging;
using Microsoft.Extensions.Logging;

namespace DemoApplication.Logging;

public static partial class UserLogging
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Debug, Message = "User {UserId} created with email {Email}.")]
    public static partial void UserCreated(
        this ILogger logger,
        UserId userId,
        [PersonalPII] string email);

    [LoggerMessage(EventId = 1, Level = LogLevel.Debug,
        Message = "User {Firstname} {LastName} used email {Email} which is already used.")]
    public static partial void EmailInUse(
        this ILogger logger,
        [PrivatePII] string firstName,
        [PrivatePII] string lastName,
        [PersonalPII] string email);
}
