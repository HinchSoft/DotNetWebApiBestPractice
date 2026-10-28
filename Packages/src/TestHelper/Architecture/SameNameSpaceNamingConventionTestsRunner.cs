using ArchUnitNET.xUnit;
using FluentValidation;
using RDH.Core.CQRS;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace TestHelper.Architecture;

public abstract class SameNamespaceNamingConventionTestsRunner<TArchitecture>
    where TArchitecture : BaseArchitecture, new()
{
    protected readonly TArchitecture Architecture = new();

    [Fact]
    public void RequestHandlers_ShouldHave_NameEndingWith_Handler()
    {
        Classes().That()
            .ImplementInterface(typeof(IRequestHandler<,>))
            .Should().HaveNameEndingWith("Handler")
            .WithoutRequiringPositiveResults()
            .Check(Architecture.ArchitectureAssemblies);
    }

    [Fact]
    public void QueryHandlers_ShouldHave_NameEndingWith_QueryHandler()
    {
        Classes().That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Or()
            .ImplementInterface(typeof(IAsyncEnumerableQueryHandler<,>))
            .Should().HaveNameEndingWith("QueryHandler")
            .WithoutRequiringPositiveResults()
            .Check(Architecture.ArchitectureAssemblies);
    }

    [Fact]
    public void CommandHandlers_ShouldHave_NameEndingWith_CommandHandler()
    {

        Classes().That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should().HaveNameEndingWith("CommandHandler")
            .WithoutRequiringPositiveResults()
            .Check(Architecture.ArchitectureAssemblies);

        Classes().That()
            .ImplementInterface(typeof(ICommand))
            .Or()
            .ImplementInterface(typeof(ICommand<>))
            .Should().HaveNameEndingWith("Command")
            .WithoutRequiringPositiveResults()
            .Check(Architecture.ArchitectureAssemblies);
    }

    [Fact]
    public void Validators_ShouldHave_NameEndingWith_Validator()
    {

        Classes().That()
            .ImplementInterface(typeof(IValidator<>))
            .Should().HaveNameEndingWith("Validator")
            .WithoutRequiringPositiveResults()
            .Check(Architecture.ArchitectureAssemblies);

    }
}
