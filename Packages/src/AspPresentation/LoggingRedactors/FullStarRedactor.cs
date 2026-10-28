using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Compliance.Redaction;
using Microsoft.Extensions.Hosting;

namespace RDH.AspPresentation.LoggingRedactors;

/// <summary>
/// This redactor will replace any targeted field with 5 stars completely hiding the original value. If
/// used in developer mode the original value is still shown in brackets after the redacted value.
/// </summary>
/// <param name="environment"></param>
public sealed class FullStarRedactor(IWebHostEnvironment environment) : Redactor
{
    private const string Stars = "****";

    public override int GetRedactedLength(ReadOnlySpan<char> input)
    {
        return GenerateMessage(input).Length;
    }

    public override int Redact(ReadOnlySpan<char> source, Span<char> destination)
    {
        var message = GenerateMessage(source);
        message.CopyTo(destination);
        return message.Length;
    }

    private string GenerateMessage(ReadOnlySpan<char> source)
    {
        return environment.IsDevelopment() ? string.Concat(Stars, " [", source, "]") : Stars;
    }
}