using System.Reflection;
using TestHelper.Architecture;

namespace TestHelperTests.Architecture;

public class Architecture : BaseArchitecture
{
    protected override Type? ApplicationAssemblyMember => typeof(Architecture);
}
