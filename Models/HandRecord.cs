namespace HandBettingGame.Models;

/// <summary>Snapshot of a completed hand for history UI.</summary>
public sealed record HandRecord(
    int RoundIndex,
    IReadOnlyList<Tile> Tiles,
    int TotalValue);
