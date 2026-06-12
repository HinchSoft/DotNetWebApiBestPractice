using Microsoft.Extensions.Compliance.Classification;

namespace Domain.PIILogging;

public static class PIIClassifications
{
    public static string Name => "PII";

    public static DataClassification Private => new(Name, nameof(Private));
    public static DataClassification Public => new(Name, nameof(Public));
    public static DataClassification Personal => new(Name, nameof(Personal));
}
