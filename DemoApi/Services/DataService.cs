
using CQRSServices.ServiceResponses;
using DemoApi.Models;
using System.Runtime.CompilerServices;

namespace DemoApi.Services;

public class DataService
{
    private List<User> _users = new()
    {
        new User(1,"User 001"),
        new User(2,"User 002"),
        new User(3,"User 003"),
        new User(4,"User 004"),
    };

    internal async Task<User> AddUserAsync(string name, CancellationToken cancellationToken)
    {
        var user = new User(_users.Count + 1, name);
        _users.Add(user);
        return user;
    }

    internal async Task<User?> GetUserByNameAsync(string name, CancellationToken cancellationToken)
    {
        return _users.FirstOrDefault(u=>u.Name == name);
    }

    internal async Task<User?> GetUserAsync(int userId, CancellationToken cancellationToken)
    {
        return _users.FirstOrDefault(u=>u.Id == userId);
    }

    internal async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken)
    {
        return _users;
    }
    internal async IAsyncEnumerable<User> GetUsersSlowAsync([EnumeratorCancellation]CancellationToken cancellationToken)
    {
        foreach (var user in _users)
        {
            await Task.Delay(1000, cancellationToken);
            yield return user;
        }
    }
}
