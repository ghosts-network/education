using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Education.Api.Domain.FlashCards;

public interface IFlashCardsCatalog
{
    Task<(IReadOnlyCollection<FlashCardsSet>, string?)> FindManyAsync(Pagination pagination);

    Task<FlashCardsSetDetails?> FindOneAsync(string id);
}