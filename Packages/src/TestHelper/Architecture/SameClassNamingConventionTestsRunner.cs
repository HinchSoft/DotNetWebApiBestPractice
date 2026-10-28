using ArchUnitNET.xUnit;
using RDH.Core.CQRS;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace TestHelper.Architecture;

public abstract class SameClassNamingConventionTestsRunner<TArchitecture>
    where TArchitecture : BaseArchitecture, new()
{
    protected readonly TArchitecture Architecture = new();

    [Fact]
    public void RequestHandlers_ShouldHave_NameEndingWith_Handler()
    {
        var commandClasses = Classes().That().HaveNameEndingWith("Handler");

        Classes().That()
            .ImplementInterface(typeof(IRequestHandler<,>))
            .Should().BeNestedIn(commandClasses)
            .Because("Command classes should be nested in a class name ending with 'Handler'")
            .WithoutRequiringPositiveResults()
            .Check(Architecture.ArchitectureAssemblies);
    }

    [Fact]
    public void QueryHandlers_ShouldHave_NameEndingWith_QueryHandler()
    {
        var commandClasses = Classes().That().HaveNameEndingWith("QueryHandler");

        Classes().That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Or()
            .ImplementInterface(typeof(IAsyncEnumerableQueryHandler<,>))
            .Should().BeNestedIn(commandClasses)
            .Because("Command classes should be nested in a class name ending with 'QueryHandler'")
            .WithoutRequiringPositiveResults()
            .Check(Architecture.ArchitectureAssemblies);
    }

    [Fact]
    public void CommandHandlers_ShouldHave_NameEndingWith_CommandHandler()
    {
        var commandClasses = Classes().That().HaveNameEndingWith("Command");

        Classes().That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should().BeNestedIn(commandClasses).AndShould().HaveName("Handler")
            .Because(
                "Command Handler classes should be nested in a class name ending with 'Command' and be named 'Handler'")
            .WithoutRequiringPositiveResults()
            .Check(Architecture.ArchitectureAssemblies);

        Classes().That()
            .ImplementInterface(typeof(ICommand))
            .Or()
            .ImplementInterface(typeof(ICommand<>))
            .Should().BeNestedIn(commandClasses).AndShould().HaveName("Command")
            .Because("Command classes should be nested in a class name ending with 'Command' and be named 'Command'")
            .WithoutRequiringPositiveResults()
            .Check(Architecture.ArchitectureAssemblies);
    }
}
