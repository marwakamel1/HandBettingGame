namespace HandBettingGame.Store;

/// <summary>Dynamic face values for special tiles, keyed by stable registry keys.</summary>
public sealed class RegistryState
{
    public Dictionary<string, int> SpecialTileValues { get; } = new(StringComparer.Ordinal);
}
