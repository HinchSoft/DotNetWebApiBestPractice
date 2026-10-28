using FluentAssertions;
using RDH.Core.CQRS;
using Xunit;

namespace TestHelper.Architecture;

public abstract class SameClassColocationTestRunner<TAppAssebly>
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

        var applicationAssembly = typeof(TAppAssebly).Assembly;

        var pairs = new TheoryData<Type, Type>();

        IEnumerable<Type> handlers = applicationAssembly
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

        if (!pairs.Any())
            throw new Exception(
                $"$XunitDynamicSkip$Skipping test, no handlers found for {typeof(TAppAssebly).FullName}");
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