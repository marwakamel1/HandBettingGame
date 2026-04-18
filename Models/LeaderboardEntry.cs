namespace HandBettingGame.Models;

public sealed record LeaderboardEntry(
    Guid Id,
    int Score,
    DateTimeOffset SavedAt,
    string? DisplayName);
