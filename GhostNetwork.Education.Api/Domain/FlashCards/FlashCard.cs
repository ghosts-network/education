using System.Collections.Generic;

namespace GhostNetwork.Education.Api.Domain.FlashCards;

public record FlashCard(string Definition, string Description, IEnumerable<string> Examples)
{
    public string Id => Definition;
}
