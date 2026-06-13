using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using DemoApplication.Handlers;
using DemoInfrastructure.Data;

namespace ArchitectureTests;

public abstract class BaseArchitectureTest
{
    protected static readonly System.Reflection.Assembly DomainAssembly = typeof(DemoDomain.Models.User).Assembly;
    protected static readonly System.Reflection.Assembly ApplicationAssembly = typeof(AddUserCommand).Assembly;
    protected static readonly System.Reflection.Assembly InfrastructureAssembly = typeof(ApplicationDbContext).Assembly;
    protected static readonly System.Reflection.Assembly PresentationAssembly = typeof(Program).Assembly;

    protected static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
            DomainAssembly,
            ApplicationAssembly,
            InfrastructureAssembly,
            PresentationAssembly)
        .Build();
        
}
