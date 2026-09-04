using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using DemoApplication.Handlers;
using DemoApplication.Handlers.AddUser;
using DemoInfrastructure.Data;
using TestHelper.Architecture;
using Type = System.Type;

namespace ArchitectureTests;

public class Architecture:BaseArchitecture
{
    protected override Type? DomainAssemblyMember => typeof(DemoDomain.Models.User);
    protected override Type? ApplicationAssemblyMember => typeof(AddUserCommand);
    protected override Type? InfrastructureAssemblyMember => typeof(ApplicationDbContext);
    protected override Type? PresentationAssemblyMember => typeof(Program);

}
