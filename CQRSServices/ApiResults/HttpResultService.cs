using CQRSServices.CQRS;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRSServices.ApiResults;

public interface IHttpResultService
{
    string GenerateLocationFromPath(string path, params object[] args);
}

public class HttpResultService :IHttpResultService
{
    private readonly HttpContext _context;
    private readonly string _rootUrl;
    public HttpResultService(HttpContext context)
    {
        _context = context;
        // Build the full URL
        var request = context.Request;
        _rootUrl = $"{request.Scheme}://{request.Host}";
    }

    public string GenerateLocationFromPath(string path, params object[] args)
    {
        return string.Format($"{_rootUrl}{path}",args);
    }
}
