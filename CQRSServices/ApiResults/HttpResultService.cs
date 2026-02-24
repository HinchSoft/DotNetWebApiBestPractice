using CQRSServices.CQRS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRSServices.ApiResults;

public interface IHttpResultService
{
    string GenerateUrlByRouteName(string routeName, object? values = null);
}

public class HttpResultService :IHttpResultService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly LinkGenerator _linkGenerator;


    public HttpResultService(IHttpContextAccessor contextAccessor, LinkGenerator linkGenerator)
    {
        _contextAccessor = contextAccessor;
        _linkGenerator = linkGenerator;
    }

    public string? GenerateUrlByRouteName(string routeName, object? values = null)
    {
        var context = _contextAccessor.HttpContext;
        if (context is null)
            return null;
        var url = _linkGenerator.GetUriByName(context, routeName, values);
        return url;
    }


}
