namespace HandBettingGame.Constants;

/// <summary>Central configuration for numeric game rules. Avoid magic numbers elsewhere.</summary>
public static class GameConfig
{
    public const int InitialSpecialValue = 5;
    public const int MaxTileValue = 10;
    public const int MinTileValue = 0;
    public const int MaxReshuffles = 3;
    public const int HandSize = 5;

    /// <summary>How many physical copies of each tile type are placed into the draw pile (full set per copy).</summary>
    public const int CopiesPerTileType = 2;

    public const int PointsPerCorrectBet = 10;
    public const int PointsPerIncorrectBet = 5;

    public const int LeaderboardSize = 5;

    public const string LeaderboardStorageKey = "hbg_leaderboard_v1";
}
