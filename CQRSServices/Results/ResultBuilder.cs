using System;
using System.Collections.Generic;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CQRSServices.Results;

public class ResultBuilder
{
    readonly HashSet<string> _errors = new();
    bool _isConflict = false;
    bool _isNotFound = false;

    public bool HasFault => _errors.Count > 0;

    public ResultBuilder AddError(string error)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(error);
        _errors.Add(error);
        return this;
    }

    public ResultBuilder AddNotFound(string error = "Not Found")
    {
        _errors.Add(error);
        _isNotFound = true;
        return this;
    }

    public ResultBuilder AddConflict(string conflict)
    {
        _errors.Add(conflict);
        _isConflict = true;
        return this;
    }

    public void AddResponse(Result responseIn)
    {
        foreach(var e in responseIn.Errors)
        {
            _errors.Add(e);
        }
        _isNotFound |= responseIn.NotFound;
        _isConflict |= responseIn.Conflict;
    }

    public TIn? AddResult<TIn>(Result<TIn> responseIn)
    {
        AddResponse((Result)responseIn);
        return responseIn.Value;
    }

    public Result Build()
    { 
        return new Result(_errors.ToArray(),_isNotFound,_isConflict); 
    }

    protected Result<TResult> Build<TResult>(TResult? value)
    {
        return new Result<TResult>(_errors.ToArray(), _isNotFound, _isConflict, value);
    }
}

public class ResultBuilder<TOut>:ResultBuilder 
{
    private TOut? _value = default;

    public ResultBuilder<TOut> AddError(string error)
    {
       base.AddError(error);
       return this;
    }
    public ResultBuilder<TOut> AddNotFound(string error = "Not Found")
    {
       base.AddNotFound(error);
       return this;
    }

    public new ResultBuilder<TOut> AddConflict(string conflict)
    {
        base.AddConflict(conflict);
        return this;
    }

    public ResultBuilder<TOut> AddValue(TOut value)
    {
        _value = value;
        return this;
    }

    public TOut AddResult(Result<TOut> respIn)
    {
        AddResponse((Result)respIn);
        _value= respIn.Value;
        return respIn.Value; 
    }

    public new Result<TOut> Build()
    { 
        return base.Build(_value);
    }
}
