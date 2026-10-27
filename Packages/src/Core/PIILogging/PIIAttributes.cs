using Microsoft.Extensions.Compliance.Classification;

namespace RDH.Core.PIILogging;

public class PrivatePIIAttribute() : DataClassificationAttribute(PIIClassifications.Private);

public class PersonalPIIAttribute() : DataClassificationAttribute(PIIClassifications.Personal);