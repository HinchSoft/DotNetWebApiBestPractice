using RDH.Core.CQRS;

namespace TestHelperTests.MockData.Handlers.CommandHandler;

internal class MockCommandHandler : ICommandHandler<MockCommand>
{
    public Task Execute(MockCommand request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
