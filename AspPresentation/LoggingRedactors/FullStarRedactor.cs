using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Compliance.Redaction;
using Microsoft.Extensions.Hosting;

namespace AspPresentation.LoggingRedactors;

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
        return environment.IsDevelopment() ? string.Concat(Stars," [", source, "]") : Stars;
    }
}
