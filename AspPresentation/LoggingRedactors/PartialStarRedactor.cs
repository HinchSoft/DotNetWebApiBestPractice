using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Compliance.Redaction;
using Microsoft.Extensions.Hosting;

namespace AspPresentation.LoggingRedactors;

public sealed class PartialStarRedactor(IWebHostEnvironment environment) : Redactor
{
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
        var idx = source.IndexOf('@');
        string red;
        if (idx != -1)
        {
            red = $"{GenSegment(source.Slice(0, idx))}@{GenSegment(source.Slice(idx + 1))}";
        }
        else
        {
            red = GenSegment(source);
        }
        if (environment.IsDevelopment())
        {
            return string.Concat(red," [", source, "]");
        }

        return red;
    }

    private string GenSegment(ReadOnlySpan<char> source)
    {
        return $"{source[0]}****{source[^1]}";
    }
}
