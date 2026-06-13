using ServiceManagement.CQRS;
using ArchUnitNET.xUnit;

using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchitectureTests;

public class VisibilityTests : BaseArchitectureTest
{
    [Fact]
    public void CommandHandlers_ShouldBeInternal()
    {
        Classes().That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should().BeInternal()
            .Check(Architecture);
    }

    [Fact(Skip = "Only enable if Query Handlers exist")]
    public void QueryHandlers_ShouldBeInternal()
    {
        Classes().That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should().BeInternal()
            .Check(Architecture);
    }

    [Fact(Skip = "Only enable if Request Handlers exist")]
    public void RequestHandlers_ShouldBeInternal()
    {
        Classes().That()
            .ImplementInterface(typeof(IRequestHandler<,>))
            .Should().BeInternal()
            .Check(Architecture);
    }
}