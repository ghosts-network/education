using System.Collections.Generic;
using System.Threading.Tasks;
using GhostNetwork.Education.Api.Domain.FlashCards;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GhostNetwork.Education.Api.Handlers.FlashCards;

public static class GetSetByIdHandler
{
    public static async Task<IResult> HandleAsync(
        [FromServices] IFlashCardsCatalog flashCardsCatalog,
        [FromServices] IFlashCardsProgressStorage flashCardsProgressStorage,
        [FromRoute] string setId,
        [FromQuery] string? userId)
    {
        return await flashCardsCatalog.FindOneAsync(setId)
            is { } set
            ? Results.Ok(string.IsNullOrEmpty(userId)
                ? new FlashCardsSetDetailsWithProgressViewModel(set.Id, set.Title, set.Cards)
                : new FlashCardsSetDetailsWithProgressViewModel(
                    set.Id,
                    set.Title,
                    set.Cards,
                    await flashCardsProgressStorage.FindSetProgressAsync(set, userId)))
            : Results.NotFound();
    }
}

public record FlashCardsSetDetailsWithProgressViewModel(
    string Id,
    string Title,
    IReadOnlyCollection<FlashCard> Cards,
    FlashCardsSetUserProgress? Progress = null);
