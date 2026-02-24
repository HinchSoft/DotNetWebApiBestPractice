namespace Microsoft.AspNetCore.Routing;

/// <summary>
/// Implement this interface to register endpoints automaticaly when <code>service.AddEndpoints()</code> in
/// Program.cs
/// </summary>
public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
