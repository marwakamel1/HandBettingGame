using HandBettingGame.Models;

namespace HandBettingGame.Store;

/// <summary>Draw pile, discard pile, and reshuffle tracking.</summary>
public sealed class DeckState
{
    public List<Tile> DrawPile { get; } = new();

    public List<Tile> DiscardPile { get; } = new();

    public int ReshuffleCount { get; set; }
}
