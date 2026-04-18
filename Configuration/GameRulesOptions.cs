namespace HandBettingGame.Configuration;

/// <summary>Game balance and persistence settings, bound from <c>appsettings.json</c> section <see cref="SectionName"/>.</summary>
public sealed class GameRulesOptions
{
    public const string SectionName = "Game";

    public int InitialSpecialValue { get; set; } = 5;

    public int MaxTileValue { get; set; } = 10;

    public int MinTileValue { get; set; } = 0;

    public int MaxReshuffles { get; set; } = 3;

    public int HandSize { get; set; } = 6;

    /// <summary>How many physical copies of each tile type are placed into the draw pile.</summary>
    public int CopiesPerTileType { get; set; } = 2;

    public int PointsPerCorrectBet { get; set; } = 10;

    public int PointsPerIncorrectBet { get; set; } = 5;

    public int LeaderboardSize { get; set; } = 5;

    public string LeaderboardStorageKey { get; set; } = "hbg_leaderboard_v1";
}
