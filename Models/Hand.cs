namespace HandBettingGame.Models;

/// <summary>A player's visible hand of tiles.</summary>
public sealed class Hand
{
    public Hand(IEnumerable<Tile> tiles) => Tiles = tiles.ToList().AsReadOnly();

    public IReadOnlyList<Tile> Tiles { get; }

    public int ComputeTotal(IReadOnlyDictionary<string, int> registry, int initialSpecialWhenMissing)
    {
        var sum = 0;
        foreach (var tile in Tiles)
            sum += tile.ResolveValue(registry, initialSpecialWhenMissing);
        return sum;
    }
}
