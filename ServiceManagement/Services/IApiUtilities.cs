using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ServiceManagement.Services;

public interface IApiUtilities
{
    bool IsPagedRequest { get;}
    Pagination? Pagination { get;}

    void AddPageHeaders(int itemCount);
    string? GenerateUrlByRouteName(string routeName, object? values = null);
}

public record Pagination(
    int Number,
    int Size)
{
    public int PageCount(int itemCount)
    {
        return (int)Math.Ceiling((double)itemCount/Size);
    }
};
