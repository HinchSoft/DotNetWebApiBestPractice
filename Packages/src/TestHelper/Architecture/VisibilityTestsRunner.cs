using ArchUnitNET.xUnit;
using FluentValidation;
using RDH.Core.CQRS;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace TestHelper.Architecture;

public abstract class VisibilityTestsRunner<TArchitecture>
    where TArchitecture : BaseArchitecture, new()
{
    protected readonly TArchitecture Architecture = new();

    [Fact]
    public void CommandHandlers_ShouldBeInternal()
    {
        Classes().That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should().BeInternal()
            .WithoutRequiringPositiveResults()
            .Check(Architecture.ArchitectureAssemblies);
    }

    [Fact]
    public void QueryHandlers_ShouldBeInternal()
    {
        Classes().That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should().BeInternal()
            .WithoutRequiringPositiveResults()
            .Check(Architecture.ArchitectureAssemblies);
    }

    [Fact]
    public void RequestHandlers_ShouldBeInternal()
    {
        Classes().That()
            .ImplementInterface(typeof(IRequestHandler<,>))
            .Should().BeInternal()
            .WithoutRequiringPositiveResults()
            .Check(Architecture.ArchitectureAssemblies);
    }

    [Fact]
    public void Validators_ShouldBeInternal()
    {
        Classes().That()
            .ImplementInterface(typeof(IValidator<>))
            .Should().BeInternal()
            .WithoutRequiringPositiveResults()
            .Check(Architecture.ArchitectureAssemblies);
    }
}
