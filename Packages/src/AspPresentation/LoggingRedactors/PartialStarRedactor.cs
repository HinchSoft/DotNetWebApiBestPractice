using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Compliance.Redaction;
using Microsoft.Extensions.Hosting;

namespace RDH.AspPresentation.LoggingRedactors;

/// <summary>
/// This redactor will replace any targeted field displaying the first and last letter and 5 stars hiding
/// the original value, if an email the @ and a repeat of the format. If
/// used in developer mode the original value is still shown in brackets after the redacted value.
/// example d*****h@g*****m
/// </summary>
/// <param name="environment"></param>
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
            return string.Concat(red, " [", source, "]");
        }

        return red;
    }

    private string GenSegment(ReadOnlySpan<char> source)
    {
        return $"{source[0]}****{source[^1]}";
    }
}