using System.Collections.Generic;

namespace GhostNetwork.Education.Api.Domain.FlashCards;

public record FlashCardsSetUserProgress(decimal Fraction, Dictionary<string, int> CardsProgress)
{
    public static FlashCardsSetUserProgress Empty => new FlashCardsSetUserProgress(decimal.Zero, new Dictionary<string, int>());
}