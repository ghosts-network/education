using System.Collections.Generic;
using System.Linq;

namespace GhostNetwork.Education.Api.Domain.FlashCards;

public record FlashCardsSetDetails(
    string Id,
    string Title,
    IReadOnlyCollection<FlashCard> Cards)
{
    public bool ValidateAnswer(FlashCardTestAnswer answer)
    {
        return Cards.First(c => c.Id == answer.CardId).Definition == answer.Answer;
    }
}