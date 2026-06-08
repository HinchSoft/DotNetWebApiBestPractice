using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using Demo.Application.Handlers;
using Demo.Infrastructure.Data;

namespace ArchitectureTests;

public abstract class BaseArchitectureTest
{
    protected static readonly System.Reflection.Assembly DomainAssembly = typeof(Demo.Domain.Models.User).Assembly;
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
