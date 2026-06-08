using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ServiceManagement.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace AspPresentation.Services;

internal class ApiUtilities:IApiUtilities
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly LinkGenerator _linkGenerator;
    private const string HEADER_PAGENO= "X-Page";
    private const string HEADER_PAGESIZE= "X-Page-Size";
    private const string HEADER_PAGECOUNT= "X-Page-Count";
    private const string QUERY_PAGENO= "page";
    private const string QUERY_PAGESIZE= "page-size";

    public ApiUtilities(IHttpContextAccessor contextAccessor, LinkGenerator linkGenerator)
    {
        _contextAccessor = contextAccessor;
        _linkGenerator = linkGenerator;
        SetPagination();
    }

    private void SetPagination()
    {
        int pgNo=0,pgSz=0;
        var query = _contextAccessor.HttpContext?.Request.Query;
        var headers = _contextAccessor.HttpContext?.Request.Headers;

        if (headers.ContainsKey(HEADER_PAGENO))
            int.TryParse(headers[HEADER_PAGENO],out pgNo);
        else if(query.ContainsKey(QUERY_PAGENO))
            int.TryParse(query[QUERY_PAGENO],out pgNo);
        if (headers.ContainsKey(HEADER_PAGESIZE))
            int.TryParse(headers[HEADER_PAGESIZE],out pgSz);
        else if(query.ContainsKey(QUERY_PAGESIZE))
            int.TryParse(query[QUERY_PAGESIZE],out pgSz);
        if(pgNo>0 && pgSz>0)
            Pagination=new Pagination(pgNo,pgSz);
    }

    public bool IsPagedRequest => Pagination is not null;
    public Pagination? Pagination { get; private set; }

    public void AddPageHeaders(int itemCount)
    {

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
