using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GhostNetwork.Education.Api.Domain;
using GhostNetwork.Education.Api.Domain.FlashCards;

namespace GhostNetwork.Education.Api.Integrations.FlashCards;

public class FileBasedFlashCardsCatalog : IFlashCardsCatalog
{
    public FileBasedFlashCardsCatalog(string rootDir)
    {
        var json = File.ReadAllText(Path.Combine(rootDir, "catalog.json"));
        Sets = JsonSerializer.Deserialize<List<FlashCardsSetDetails>>(json, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        })!;
    }

    private IReadOnlyCollection<FlashCardsSetDetails> Sets { get; }

    public Task<(IReadOnlyCollection<FlashCardsSet>, string?)> FindManyAsync(Pagination pagination)
    {
        IReadOnlyCollection<FlashCardsSet> list = Sets
            .Select(s => new FlashCardsSet(s.Id, s.Title, new FlashCardsSetInfo(s.Cards.Count)))
            .ToList();

        return Task.FromResult((list, default(string?)));
    }

    public Task<FlashCardsSetDetails?> FindOneAsync(string id)
    {
        return Task.FromResult(Sets.FirstOrDefault(s => s.Id == id));
    }
}