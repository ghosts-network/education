using System.Collections.Generic;

namespace GhostNetwork.Education.Api.Domain.FlashCards;

public record FlashCardSetTestResult(IEnumerable<FlashCardTestAnswer> Answers);
