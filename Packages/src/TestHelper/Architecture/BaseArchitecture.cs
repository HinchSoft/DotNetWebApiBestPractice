using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using Assembly = System.Reflection.Assembly;

namespace TestHelper.Architecture;

public abstract class BaseArchitecture
{
    private static ArchUnitNET.Domain.Architecture? _architectureAssemblies = null;

    private static System.Reflection.Assembly? _domainAssembly;
    private static System.Reflection.Assembly? _applicationAssembly;
    private static System.Reflection.Assembly? _infrastructureAssembly;
    private static System.Reflection.Assembly? _presentationAssembly;

    private static IObjectProvider<IType>? _domainLayer;
    private static IObjectProvider<IType>? _applicationLayer;
    private static IObjectProvider<IType>? _infrastructureLayer;
    private static IObjectProvider<IType>? _presentationLayer;

    protected BaseArchitecture()
    {
        if (_architectureAssemblies is not null) return;
        var al = new ArchLoader();
        _domainAssembly = DomainAssemblyMember?.Assembly;
        if (_domainAssembly is not null)
        {
            al.LoadAssembly(_domainAssembly);
            _domainLayer = Types().That().ResideInAssembly(_domainAssembly).As("Domain layer");
        }

        _applicationAssembly = ApplicationAssemblyMember?.Assembly;
        if (_applicationAssembly is not null)
        {
            al.LoadAssembly(_applicationAssembly);
            _applicationLayer = Types().That().ResideInAssembly(_applicationAssembly).As("Application layer");
        }

        _infrastructureAssembly = InfrastructureAssemblyMember?.Assembly;
        if (_infrastructureAssembly is not null)
        {
            al.LoadAssembly(_infrastructureAssembly);
            _infrastructureLayer = Types().That().ResideInAssembly(_infrastructureAssembly).As("Infrastructure Layer");
        }

        _presentationAssembly = PresentationAssemblyMember?.Assembly;
        if (_presentationAssembly is not null)
        {
            al.LoadAssembly(_presentationAssembly);
            _presentationLayer = Types().That().ResideInAssembly(_presentationAssembly).As("Presentation Layer");
        }

        _architectureAssemblies = al.Build();
    }

    protected virtual System.Type? DomainAssemblyMember => null;
    protected virtual System.Type? ApplicationAssemblyMember => null;
    protected virtual System.Type? InfrastructureAssemblyMember => null;
    protected virtual System.Type? PresentationAssemblyMember => null;

    public ArchUnitNET.Domain.Architecture ArchitectureAssemblies => _architectureAssemblies!;
    public Assembly? DomainAssembly => _domainAssembly;
    public Assembly? ApplicationAssembly => _applicationAssembly;
    public Assembly? InfrastrucureAssembly => _infrastructureAssembly;
    public Assembly? PresentationAssembly => _presentationAssembly;

    public IObjectProvider<IType>? DomainLayer => _domainLayer;
    public IObjectProvider<IType>? ApplicationLayer => _applicationLayer;
    public IObjectProvider<IType>? InfrastructureLayer => _infrastructureLayer;
    public IObjectProvider<IType>? PresentationLayer => _presentationLayer;}
