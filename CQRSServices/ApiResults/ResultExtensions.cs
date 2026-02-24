using CQRSServices.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Net.ServerSentEvents;
using HttpResults = Microsoft.AspNetCore.Http.Results;

namespace CQRSServices.ApiResults;

public static class ResultExtensions
{

    public static IResult HttpResult(this Result result)
    {
        if (result.IsSuccess)
            return HttpResults.NoContent();
        else if (result.NotFound)
            return HttpResults.NotFound(result.Errors);
        else if (result.Conflict)
            return HttpResults.Conflict(result.Errors);

        return HttpResults.BadRequest(result.Errors);
    }

    public static IResult HttpResult<T>(this Result<T> result)
    {
        if (result.IsSuccess && result.Value is not null)
        {
            return HttpResults.Ok(result.Value);
        }
        else
            return HttpResult((Result)result);
    }

    public static IResult HttpResultServerSentEvents<T>(this Result<IAsyncEnumerable<T>> result, Func<T,SseItem<T>>? sseItem = null)
    {
        if (result.IsSuccess && result.Value is not null)
        {
            async IAsyncEnumerable<SseItem<T>> GetStream(IAsyncEnumerable<T> value)
            {
                await foreach (var item in value)
                {
                    yield return sseItem is null ? new SseItem<T>(item) : sseItem(item);
                }
            }

            return TypedResults.ServerSentEvents((dynamic)GetStream(result.Value));
        }
        else
            return HttpResult((Result)result);
    }

    public static IResult HttpResultCreated<T>(this Result<T> result, Func<T, string> uri)
    {
        if (result.IsSuccess && result.Value is not null)
        {
            return TypedResults.Created(uri(result.Value),result.Value);
        }
        else
            return HttpResult((Result)result);
    }
    public static IResult HttpResultCreatedAtRoute<T>(this Result<T> result,string routename, Func<T, RouteValueDictionary> routeValues)
    {
        if (result.IsSuccess && result.Value is not null)
        {
            return TypedResults.CreatedAtRoute(routename, routeValues(result.Value));
        }
        else
            return HttpResult((Result)result);
    }

    public static IResult HttpResultAccepted(this Result<Accepted> result, Func<Accepted, string> location)
    {
        if (result.IsSuccess && result.Value is not null)
        {
                return TypedResults.Accepted(location(result.Value),result.Value);
        }
        else
            return HttpResult((Result)result);
    }


}
