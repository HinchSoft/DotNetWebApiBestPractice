using ArchUnitNET.xUnit;
using ServiceManagement.CQRS;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchitectureTests;

public class NamingConventionTests:BaseArchitectureTest
{

    [Fact(Skip = "Only enable if Request Handlers exist")]
    public void RequestHandlers_ShouldHave_NameEndingWith_Handler()
    {
        var commandClasses = Classes().That().HaveNameEndingWith("Handler");

        Classes().That()
             .ImplementInterface(typeof(IRequestHandler<,>))
             .Should().BeNestedIn(commandClasses)
             .Because("Command classes should be nested in a class name ending with 'Handler'")
             .Check(Architecture);
    }

    [Fact(Skip = "Only enable if Query Handlers exist")]
    public void QueryHandlers_ShouldHave_NameEndingWith_QueryHandler()
    {
        var commandClasses = Classes().That().HaveNameEndingWith("QueryHandler");

        Classes().That()
             .ImplementInterface(typeof(IQueryHandler<,>))
             .Or()
             .ImplementInterface(typeof(IAsyncEnumerableQueryHandler<,>))
             .Should().BeNestedIn(commandClasses)
             .Because("Command classes should be nested in a class name ending with 'QueryHandler'")
             .Check(Architecture);
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
             .Because("Command Handler classes should be nested in a class name ending with 'Command' and be named 'Handler'")
             .Check(Architecture);

        Classes().That()
             .ImplementInterface(typeof(ICommand))
             .Or()
             .ImplementInterface(typeof(ICommand<>))
             .Should().BeNestedIn(commandClasses).AndShould().HaveName("Command")
             .Because("Command classes should be nested in a class name ending with 'Command' and be named 'Command'")
             .Check(Architecture);

    }
}
