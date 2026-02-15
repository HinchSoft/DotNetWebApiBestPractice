using CQRSServices.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Net.ServerSentEvents;
using HttpResults = Microsoft.AspNetCore.Http.Results;

namespace CQRSServices.ApiResults;

public static class ResultExtensions
{

    public static IResult HttpResult(this Result response)
    {
        if (response.IsSuccess)
            return HttpResults.NoContent();
        else if (response.NotFound)
            return HttpResults.NotFound(response.Errors);
        else if (response.Conflict)
            return HttpResults.Conflict(response.Errors);

        return HttpResults.BadRequest(response.Errors);
    }

    public static IResult HttpResult<T>(this Result<T> response)
    {
        if (response.IsSuccess && response.Value is not null)
        {
            return HttpResults.Ok(response.Value);
        }
        else
            return HttpResult((Result)response);
    }

    public static IResult HttpResultServerSentEvents<T>(this Result<IAsyncEnumerable<T>> response, Func<T,SseItem<T>>? sseItem = null)
    {
        if (response.IsSuccess && response.Value is not null)
        {
            async IAsyncEnumerable<SseItem<T>> GetStream(IAsyncEnumerable<T> value)
            {
                await foreach (var item in value)
                {
                    yield return sseItem is null ? new SseItem<T>(item) : sseItem(item);
                }
            }

            return TypedResults.ServerSentEvents((dynamic)GetStream(response.Value));
        }
        else
            return HttpResult((Result)response);
    }

    public static IResult HttpResultCreated<T>(this Result<T> response, Func<T, string> uri)
    {
        if (response.IsSuccess && response.Value is not null)
        {
            return TypedResults.Created(uri(response.Value),response.Value);
        }
        else
            return HttpResult((Result)response);
    }
    public static IResult HttpResultCreatedAtRoute<T>(this Result<T> response,string routename, Func<T, RouteValueDictionary> routeValues)
    {
        if (response.IsSuccess && response.Value is not null)
        {
            return TypedResults.CreatedAtRoute(routename, routeValues(response.Value));
        }
        else
            return HttpResult((Result)response);
    }

    public static IResult HttpResultAccepted<TVal, TStatus>(this Result<TVal> response, Func<TVal, string> location, Func<TVal,TStatus> status)
    {
        if (response.IsSuccess && response.Value is not null)
        {
            var st=status(response.Value);
            return TypedResults.Accepted(location(response.Value), st);
        }
        else
            return HttpResult((Result)response);
    }

    //public static IResult HttpResultAcceptedAtRoute<T, TStatus>(this Result<T> response,string routename, Func<T, RouteValueDictionary> routeValues,)
    //{
    //    if (response.IsSuccess && response.Value is not null)
    //    {
    //        return TypedResults.AcceptedAtRoute(response.Value, routename, routeValues(response.Value));
    //    }
    //    else
    //        return HttpResult((Result)response);
    //}

    public class Location<TStatus>
    {
        public string Uri { get; set; }
        public TStatus? Status { get; set; }
    }
}
