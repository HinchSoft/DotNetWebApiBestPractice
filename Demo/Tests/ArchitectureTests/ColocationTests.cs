using DemoApplication.Handlers;
using DemoApplication.Handlers.AddUserCommand;
using FluentAssertions;
using TestHelper.Architecture;


namespace ArchitectureTests;

public class ColocationTests : SameNamespaceColocationTestRunner<AddUserRequest>
{
}
