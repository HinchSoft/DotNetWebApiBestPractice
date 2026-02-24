using System.Collections.Concurrent;

namespace CQRSServices.Results;

public record Result(string[] Errors,bool NotFound, bool Conflict)
{
    public bool IsSuccess { get; } = Errors.Length == 0;
    public bool IsFail => !IsSuccess;
}


public record Result<TResult>(string[] Errors, bool NotFound, bool Conflict,  TResult? Value)
    :Result(Errors,NotFound,Conflict)
{
    public ConcurrentDictionary<string,object> metadata { get; } = new ConcurrentDictionary<string, object>();
}

public record Accepted(string StatusId);