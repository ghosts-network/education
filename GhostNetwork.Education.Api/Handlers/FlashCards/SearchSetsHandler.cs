using System.Threading.Tasks;
using GhostNetwork.Education.Api.Domain;
using GhostNetwork.Education.Api.Domain.FlashCards;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace GhostNetwork.Education.Api.Handlers.FlashCards;

public static class SearchSetsHandler
{
    public static async Task<IResult> HandleAsync(
        HttpResponse response,
        [FromServices] IFlashCardsCatalog flashCardsCatalog,
        [FromQuery] string? cursor,
        [FromQuery] int limit = 20)
    {
        if (limit is < 1 or > 100)
        {
            return Results.BadRequest(new ProblemDetails { Title = "Limit must be between 1 and 100" });
        }

        var (sets, nextCursor) = await flashCardsCatalog.FindManyAsync(new Pagination(cursor, limit));

        if (nextCursor is null)
        {
            response.Headers.Add("X-Cursor", nextCursor);
        }

        return Results.Ok(sets);
    }
}
