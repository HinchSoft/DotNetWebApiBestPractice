using Microsoft.Extensions.Compliance.Classification;

namespace Domain.PIILogging;

public class PrivatePIIAttribute() : DataClassificationAttribute(PIIClassifications.Private);

public class PersonalPIIAttribute() : DataClassificationAttribute(PIIClassifications.Personal);
