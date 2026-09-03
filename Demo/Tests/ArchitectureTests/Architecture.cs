using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using DemoApplication.Handlers;
using DemoApplication.Handlers.AddUserCommand;
using DemoInfrastructure.Data;
using TestHelper.Architecture;
using Type = System.Type;

namespace ArchitectureTests;

public class Architecture:BaseArchitecture
{
    protected override Type? DomainAssembly => typeof(DemoDomain.Models.User);
    protected override Type? ApplicationAssembly => typeof(AddUserRequest);
    protected override Type? InfrastructureAssembly => typeof(ApplicationDbContext);
    protected override Type? PresentationAssembly => typeof(Program);

}
