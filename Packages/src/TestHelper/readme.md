# RDH.TestHelper

`RDH.TestHelper` provides reusable testing utilities, architecture enforcement runners (built on ArchUnitNET), and test harness helpers for xUnit v3 and FluentAssertions.

## Installation

Install the package via the .NET CLI:

```bash
dotnet add package RDH.TestHelper
```

Or via the Package Manager Console:

```powershell
Install-Package RDH.TestHelper
```

---

## Key Features

- **Architecture Boundary Verification:** Base test runners (`LayerTestsRunner`, `VisibilityTestsRunner`, `SameNamespaceColocationTests`, `NamingConventionTestsRunner`) to enforce clean layering in automated test suites.
- **Operating System Constraints:** `SupportedOSAttribute` to target tests to specific platforms (Linux, macOS, Windows).
- **Modern Test Stack:** Optimized for **xUnit v3** and **FluentAssertions**.

---

## Sample Code

### Enforcing Clean Architecture Layering

Define your architecture definition and inherit from `LayerTestsRunner`:

```csharp
using TestHelper.Architecture;

namespace ArchitectureTests;

public class LayerTests : LayerTestsRunner<Architecture>
{
}
```
