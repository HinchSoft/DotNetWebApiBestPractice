using FluentAssertions;
using FluentValidation;
using RDH.Core.CQRS;
using Xunit;

namespace TestHelper.Architecture;

public abstract class SameNamespaceColocationTestRunner<TAppAssebly>
{
    public static TheoryData<Type, Type, Type?> GetHandlerAndCommandPairs()
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

        var sets = new TheoryData<Type, Type, Type?>();

        var handlers = applicationAssembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false })
            .Where(t => t.DeclaringType is null);

        foreach (var handler in handlers)
        {
            foreach (var iface in handler.GetInterfaces())
            {
                if (!iface.IsGenericType)
                {
                    continue;
                }

                var genericDef = iface.GetGenericTypeDefinition();

                if (!handlerInterfaces.Contains(genericDef))
                {
                    continue;
                }

                var commandOrQueryType = iface.GetGenericArguments()[0];

                var abValType = typeof(AbstractValidator<>).MakeGenericType(commandOrQueryType);

                var validatorType = applicationAssembly
                    .GetTypes()
                    .FirstOrDefault(t => t.IsClass && t.IsSubclassOf(abValType));

                sets.Add(handler, commandOrQueryType, validatorType);
            }
        }

        if (!sets.Any())
            throw new Exception(
                $"$XunitDynamicSkip$Skipping test, no handlers found for {typeof(TAppAssebly).FullName}");
        return sets;
    }

    [Theory]
    [MemberData(nameof(GetHandlerAndCommandPairs))]
    public void Handlers_ShouldResideInSameNameSpace_AsTheirCommandOrQueryAndValidator(
        Type handlerType,
        Type commandOrQueryType,
        Type? validatorType)
    {
        var ns = handlerType.Namespace;

        commandOrQueryType.Namespace.Should().Be(ns,
            $"{commandOrQueryType.Name} should be in the same Namespace as {handlerType.Name}");

        validatorType?.Namespace.Should()
            .Be(ns, $"{validatorType.Name} should be in the same Namespace as {handlerType.Name}");
    }
}
