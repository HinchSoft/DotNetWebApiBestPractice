using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace TestHelper.Architecture;

public abstract class LayerTestsRunner<TArchitecture>
    where TArchitecture : BaseArchitecture, new()
{
    protected readonly TArchitecture Architecture = new();


    [Fact]
    public void DomainLayer_ShouldNotDependOn_EntityFramework()
    {
        if(Architecture.DomainAssembly is null)
            throw new Exception($"$XunitDynamicSkip$Domain assembly is not set.");

        Types().That().ResideInAssembly(Architecture.DomainAssembly).Should()
            .NotDependOnAnyTypesThat()
            .ResideInNamespace("Microsoft.EntityFrameworkCore")
            .Check(Architecture.ArchitectureAssemblies);
    }

    [Fact]
    public void ApplicationLayer_ShouldNotDependOn_EntityFramework()
    {
        if(Architecture.ApplicationAssembly is null)
            throw new Exception($"$XunitDynamicSkip$Application assembly is not set.");

        Types().That().ResideInAssembly(Architecture.ApplicationAssembly).Should()
            .NotDependOnAnyTypesThat()
            .ResideInNamespace("Microsoft.EntityFrameworkCore")
            .Check(Architecture.ArchitectureAssemblies);
    }

    [Fact]
    public void DomainLayer_ShouldNotDependOn_ApplicationLayer()
    {
        if(Architecture.DomainLayer is null || Architecture.ApplicationLayer is null)
            throw new Exception($"$XunitDynamicSkip$Domain or Application assembly is not set.");

        Types().That().Are(Architecture.DomainLayer).Should()
            .NotDependOnAny(Architecture.ApplicationLayer)
            .Check(Architecture.ArchitectureAssemblies);
    }

    [Fact]
    public void DomainLayer_ShouldNotDependOn_InfrastructureLayer()
    {
        if(Architecture.DomainLayer is null || Architecture.InfrastructureLayer is null)
            throw new Exception($"$XunitDynamicSkip$Domain or Infrastructure assembly is not set.");

        Types().That().Are(Architecture.DomainLayer).Should()
            .NotDependOnAny(Architecture.InfrastructureLayer)
            .Check(Architecture.ArchitectureAssemblies);
    }

    [Fact]
    public void DomainLayer_ShouldNotDependOn_PresentationLayer()
    {
        if(Architecture.DomainLayer is null || Architecture.PresentationLayer is null)
            throw new Exception($"$XunitDynamicSkip$Domain or Presentation assembly is not set.");

        Types().That().Are(Architecture.DomainLayer).Should()
            .NotDependOnAny(Architecture.PresentationLayer)
            .Check(Architecture.ArchitectureAssemblies);
    }

    [Fact]
    public void ApplicationLayer_ShouldNotDependOn_InfrastructureLayer()
    {
        if(Architecture.ApplicationLayer is null || Architecture.InfrastructureLayer is null)
            throw new Exception($"$XunitDynamicSkip$Application or Infrastructure assembly is not set.");
        Types().That().Are(Architecture.ApplicationLayer).Should()
            .NotDependOnAny(Architecture.InfrastructureLayer)
            .Check(Architecture.ArchitectureAssemblies);
    }

    [Fact]
    public void ApplicationLayer_ShouldNotDependOn_PresentationLayer()
    {
        if(Architecture.ApplicationLayer is null || Architecture.PresentationLayer is null)
            throw new Exception($"$XunitDynamicSkip$Application or Presentation assembly is not set.");

        Types().That().Are(Architecture.ApplicationLayer).Should()
            .NotDependOnAny(Architecture.PresentationLayer)
            .Check(Architecture.ArchitectureAssemblies);
    }

    [Fact]
    public void InfrastructureLayer_ShouldNotDependOn_PresentationLayer()
    {
        if(Architecture.InfrastructureLayer is null || Architecture.PresentationLayer is null)
            throw new Exception($"$XunitDynamicSkip$Infrastructure or Presentation assembly is not set.");

        Types().That().Are(Architecture.InfrastructureLayer).Should()
            .NotDependOnAny(Architecture.PresentationLayer)
            .Check(Architecture.ArchitectureAssemblies);
    }
}
