using HandBettingGame.Constants;

namespace HandBettingGame.Models;

public enum TileFamily
{
    Dragon,
    Wind,
    Suit
}

public enum DragonType
{
    Red,
    Green,
    White
}

public enum WindType
{
    North,
    South,
    East,
    West
}

public enum SuitType
{
    Characters,
    Bamboo,
    Circles
}

/// <summary>Mahjong-style tile: dragons, winds, or suited1–9. Special tiles read their face value from the game registry.</summary>
public sealed class Tile
{
    public required string InstanceId { get; init; }

    public TileFamily Family { get; init; }

    public DragonType? Dragon { get; init; }

    public WindType? Wind { get; init; }

    public SuitType? Suit { get; init; }

    /// <summary>Rank1–9 when <see cref="Family"/> is <see cref="TileFamily.Suit"/>.</summary>
    public int? Rank { get; init; }

    /// <summary>When true, contribution uses <see cref="RegistryKey"/> in the special-value dictionary.</summary>
    public bool IsSpecial { get; init; }

    /// <summary>Key into registry (e.g. <c>dragon:red</c>). Required when <see cref="IsSpecial"/> is true.</summary>
    public string? RegistryKey { get; init; }

    /// <summary>Resolves this tile's numeric contribution for the current hand total.</summary>
    public int ResolveValue(IReadOnlyDictionary<string, int> registry)
    {
        if (IsSpecial && !string.IsNullOrEmpty(RegistryKey))
            return registry.TryGetValue(RegistryKey, out var v) ? v : GameConfig.InitialSpecialValue;

        return Family switch
        {
            TileFamily.Suit => Rank ?? 0,
            TileFamily.Dragon => 5,
            TileFamily.Wind => 5,
            _ => 0
        };
    }
}
