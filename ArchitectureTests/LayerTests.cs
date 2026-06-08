using ArchUnitNET.Domain;
using ArchUnitNET.xUnit;
using System;
using System.Collections.Generic;
using System.Text;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchitectureTests;

public class LayerTests:BaseArchitectureTest
{
    private static readonly IObjectProvider<IType> DomainLayer =
     Types().That().ResideInAssembly(DomainAssembly).As("Domain layer");

    private static readonly IObjectProvider<IType> ApplicationLayer =
        Types().That().ResideInAssembly(ApplicationAssembly).As("Application layer");

    private static readonly IObjectProvider<IType> InfrastructureLayer =
        Types().That().ResideInAssembly(InfrastructureAssembly).As("Infrastructure Layer");

    private static readonly IObjectProvider<IType> PresentationLayer =
        Types().That().ResideInAssembly(PresentationAssembly).As("Presentation Layer");

    [Fact]
    public void DomainLayer_ShouldNotDependOn_ApplicationLayer()
    {
        Types().That().Are(DomainLayer).Should()
            .NotDependOnAny(ApplicationLayer)
            .Check(Architecture);
    }

    [Fact]
    public void DomainLayer_ShouldNotDependOn_InfrastructureLayer()
    {
        Types().That().Are(DomainLayer).Should()
            .NotDependOnAny(InfrastructureLayer)
            .Check(Architecture);
    }

    [Fact]
    public void DomainLayer_ShouldNotDependOn_PresentationLayer()
    {
        Types().That().Are(DomainLayer).Should()
            .NotDependOnAny(PresentationLayer)
            .Check(Architecture);
    }

    [Fact]
    public void ApplicationLayer_ShouldNotDependOn_InfrastructureLayer()
    {
        Types().That().Are(ApplicationLayer).Should()
            .NotDependOnAny(InfrastructureLayer)
            .Check(Architecture);
    }

    [Fact]
    public void ApplicationLayer_ShouldNotDependOn_PresentationLayer()
    {
        Types().That().Are(ApplicationLayer).Should()
            .NotDependOnAny(PresentationLayer)
            .Check(Architecture);
    }

    [Fact]
    public void InfrastructureLayer_ShouldNotDependOn_PresentationLayer()
    {
        Types().That().Are(InfrastructureLayer).Should()
            .NotDependOnAny(PresentationLayer)
            .Check(Architecture);
    }
}
