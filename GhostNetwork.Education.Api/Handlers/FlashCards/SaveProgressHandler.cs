using System.Threading.Tasks;
using GhostNetwork.Education.Api.Domain.FlashCards;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GhostNetwork.Education.Api.Handlers.FlashCards;

public static class SaveProgressHandler
{
    public static async Task<IResult> HandleAsync(
        [FromServices] IFlashCardsCatalog flashCardsCatalog,
        [FromServices] IFlashCardsProgressStorage flashCardsProgressStorage,
        [FromRoute] string setId,
        [FromQuery] string userId,
        [FromBody] FlashCardSetTestResult results)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("UserId is required");
        }

        var set = await flashCardsCatalog.FindOneAsync(setId);
        if (set is null)
        {
            return Results.NotFound();
        }

        await flashCardsProgressStorage.UpdateProgressAsync(set, userId, results);

        return Results.Ok(await flashCardsProgressStorage.FindSetProgressAsync(set, userId));
    }
}