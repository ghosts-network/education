using System.Threading.Tasks;

namespace GhostNetwork.Education.Api.Domain.FlashCards;

public interface IFlashCardsProgressStorage
{
    Task<FlashCardsSetUserProgress> FindSetProgressAsync(FlashCardsSetDetails set, string user);

    Task UpdateProgressAsync(FlashCardsSetDetails set, string user, FlashCardSetTestResult results);
}