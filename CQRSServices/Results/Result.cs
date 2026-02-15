using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CQRSServices.Results;

public record Result(string[] Errors,bool NotFound, bool Conflict)
{
    public bool IsSuccess { get; } = Errors.Length == 0;
    public bool IsFail => !IsSuccess;
}


public record Result<TResult>(string[] Errors, bool NotFound, bool Conflict,  TResult? Value)
    :Result(Errors,NotFound,Conflict)
{

}

