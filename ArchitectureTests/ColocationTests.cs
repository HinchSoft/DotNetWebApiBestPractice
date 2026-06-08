using Demo.Application.Handlers;
using FluentAssertions;
using ServiceManagement.CQRS;


namespace ArchitectureTests;

public class ColocationTests : BaseArchitectureTest
{

    public static TheoryData<Type, Type> GetHandlerAndCommandPairs()
    {
        Type[] handlerInterfaces =
        [
            typeof(IRequestHandler<,>),
            typeof(ICommandHandler<>),
            typeof(ICommandHandler<,>),
            typeof(IQueryHandler<,>),
            typeof(IAsyncEnumerableQueryHandler<,>)
        ];

        var pairs = new TheoryData<Type, Type>();

        IEnumerable<Type> handlers = ApplicationAssembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false })
            .Where(t => t.DeclaringType is not null);

        foreach (Type handler in handlers)
        {
            foreach (Type iface in handler.GetInterfaces())
            {
                if (!iface.IsGenericType)
                {
                    continue;
                }

                Type genericDef = iface.GetGenericTypeDefinition();

                if (!handlerInterfaces.Contains(genericDef))
                {
                    continue;
                }

                Type commandOrQueryType = iface.GetGenericArguments()[0];
                pairs.Add(handler, commandOrQueryType);
            }
        }

        return pairs;
    }

    [Theory]
    [MemberData(nameof(GetHandlerAndCommandPairs))]
    public void Handlers_ShouldResideInSameClass_AsTheirCommandOrQuery(
        Type handlerType,
        Type commandOrQueryType)
    {
        handlerType.DeclaringType.Should().Be(
            commandOrQueryType.DeclaringType,
            $"{handlerType.Name} should be in the same class as {commandOrQueryType.Name}");

    }

}